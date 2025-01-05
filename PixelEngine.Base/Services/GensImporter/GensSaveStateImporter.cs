public static class GensSaveStateImporter
{
    private static class Locations
    {
        public static int VRam = 0x12478;
        public static int CRam = 0x112 + 0;
        public static int VSRam = 0x192;
        public static int Registers = 0xFA;
    }

    public static void ReadSaveState(byte[] fileData, RenderService renderService, Specs specs)
    {
        var originalFileData = new byte[fileData.Length];
        Array.Copy(fileData, originalFileData, fileData.Length);

        fileData = fileData.EndianSwap();

        byte[] vram = new byte[0x10000];
        Array.Copy(fileData, Locations.VRam, vram, 0, vram.Length);
       
        byte[] paddedCram = new byte[256];
        Array.Copy(fileData, Locations.CRam, paddedCram, 0, paddedCram.Length);

        byte[] cram = new byte[128];
        // every other byte in save state is 0, for some reason
        for (int i = 0; i < cram.Length; i++)
        {
            cram[i] = paddedCram[i * 2];
        }

        byte[] vsram = new byte[256];
        Array.Copy(fileData, Locations.VRam, vsram, 0, vsram.Length);

        // register bytes are not swapped
        BitStreamReader registerReader = new BitStreamReader(originalFileData);

        // register 2 - Plane A Location
        registerReader.Seek(Locations.Registers + 2);
        registerReader.ReadNextBits(3);
        var planeANameTableLocation = registerReader.ReadNextBits(3) * 0x2000; // is this 0x400?
        registerReader.ReadNextBits(2);

        // register 4 - Plane B Location
        registerReader.Seek(Locations.Registers + 4);
        var planeBNameTableLocation = registerReader.ReadNextBits(3) * 0x2000;
        registerReader.ReadNextBits(5);

        // register 5 - Sprite table location
        var spriteTableLocation = registerReader.ReadNextBits(7) * 0x200;

        // register 7 - Background color
        registerReader.Seek(Locations.Registers + 7);
        var color = registerReader.ReadNextBits(4);
        var line = registerReader.ReadNextBits(2);
        registerReader.ReadNextBits(2);

        // register 11 - Scroll Modes
        registerReader.Seek(Locations.Registers + 11);
        var horizontalScrollMode = registerReader.ReadNextBits(2);
        var verticalScrollMode = registerReader.ReadNextBits(1);
        registerReader.ReadNextBits(5);

        // register 12 - Screen Size
        var screenSize = registerReader.ReadNextBits(1);
        registerReader.ReadNextBits(7);

        // register 13 - Horizontal Scroll Data
        var hScrollLocation = registerReader.ReadNextBits(5) * 0x400;
        registerReader.ReadNextBits(3);

        // register 16 - Plane Size
        registerReader.Seek(Locations.Registers + 16);
        var planeWidth = registerReader.ReadNextBits(2);
        registerReader.ReadNextBits(2);
        var planeHeight = registerReader.ReadNextBits(2);
        registerReader.ReadNextBits(2);

        renderService.Layers = new LayerGroup(
        new ScrollingLayer(specs, LayerTiles(planeWidth), LayerTiles(planeHeight)),
            new ScrollingLayer(specs, LayerTiles(planeWidth), LayerTiles(planeHeight)),
                new Layer(specs, 32, 32),
                new Layer(specs, 4, 4)); //todo, sprite layer should be different

        ReadVram(renderService, 
            vram,
            Array.Empty<byte>(),
            cram,            
            horizontalScrollMode, 
            verticalScrollMode, 
            hScrollLocation, 
            planeANameTableLocation, 
            planeBNameTableLocation, 
            spriteTableLocation, 
            specs);

        LoadVScroll(renderService.Layers, verticalScrollMode, originalFileData, Locations.VSRam, specs);
        GensVramImporter.LoadColors(cram, renderService, specs);
    }

    public static void ReadVram(RenderService renderService, 
        byte[] vram,
        byte[] vsram,
        byte[] cram,
        byte horizontalScrollMode, 
        byte verticalScrollMode, 
        int hScrollLocation, 
        int planeANameTableLocation,
        int planeBNameTableLocation,
        int spriteTableLocation,
        Specs specs)
    {

        GensVramImporter.LoadColors(cram, renderService, specs);

        renderService.PatternTable.SetData(vram);

        LoadHScroll(renderService.Layers, horizontalScrollMode, vram, hScrollLocation, specs);

        if(vsram.Length > 0)
            LoadVScroll(renderService.Layers, verticalScrollMode, vsram, 0, specs);

        GensVramImporter.LoadLayer(vram, renderService.Layers.Foreground, planeANameTableLocation);
        GensVramImporter.LoadLayer(vram, renderService.Layers.Background, planeBNameTableLocation);
        GensVramImporter.LoadSprites(vram, spriteTableLocation, renderService.Sprites);
    }
    private static int LayerTiles(int planeSize) =>
        planeSize switch
        {
            0 => 32,
            1 => 64,
            3 => 128,
            _ => throw new Exception($"Invalid plane size {planeSize}")
        };


    private static void LoadHScroll(LayerGroup layers, byte mode, byte[] fileData, int location, Specs specs)
    {
        var scrollType = mode switch
        {
            0 => ScrollTableType.FullScreen,
            2 => ScrollTableType.EightPixelStrips,
            3 => ScrollTableType.Line,
            _ => throw new Exception($"Invalid HScroll mode {mode}")
        };

        layers.Foreground.HScrollTable = new ScrollTable(scrollType, true, layers.Foreground.PixelSize.Width, specs);
        layers.Background.HScrollTable = new ScrollTable(scrollType, true, layers.Background.PixelSize.Width, specs);

       
        switch(layers.Foreground.HScrollTable.Type)
        {
            case ScrollTableType.FullScreen:
                LoadWholeScreenScrollData(layers, true, fileData, location);
                break;
            case ScrollTableType.EightPixelStrips:
                LoadEightPixelStripHScrollData(layers, fileData, location);
                break;
            default:
                LoadScanlineScrollData(layers, fileData, location);
                break;
        }
    }

    private static void LoadVScroll(LayerGroup layers, byte mode, byte[] fileData, int location, Specs specs)
    {
        var scrollType = mode switch
        {
            0 => ScrollTableType.FullScreen,
            1 => ScrollTableType.EightPixelStrips,
            _ => throw new Exception($"Invalid VScroll mode {mode}")
        };

        layers.Foreground.VScrollTable = new ScrollTable(scrollType, false, layers.Foreground.PixelSize.Height, specs);
        layers.Background.VScrollTable = new ScrollTable(scrollType, false, layers.Background.PixelSize.Height, specs);


        switch (layers.Foreground.VScrollTable.Type)
        {
            case ScrollTableType.FullScreen:
                LoadWholeScreenScrollData(layers, false, fileData, location);
                break;
            default:
                LoadEightPixelStripVScrollData(layers, fileData, location);
                break;
        }
    }

    private static void ReadScrollEntry(LayerGroup layers, bool horizontal, byte[] fileData, int location, int scrollEntry)
    {
        var foreground = horizontal ? layers.Foreground.HScrollTable : layers.Foreground.VScrollTable;
        var background = horizontal ? layers.Background.HScrollTable : layers.Background.VScrollTable;

        byte fHigh2 = (byte)(fileData[location + 1] & 3);
        byte fLow2 = fileData[location + 0];

        byte bHigh2 = (byte)(fileData[location + 3] & 3);
        byte bLow2 = fileData[location + 2];

        var fScroll2 = (short)((fHigh2 << 8) | fLow2);
        var bScroll2 = (short)((bHigh2 << 8) | bLow2);

        foreground.Set(scrollEntry, fScroll2);
        background.Set(scrollEntry, bScroll2);
    }

    private static void LoadWholeScreenScrollData(LayerGroup layers, bool horizontal, byte[] fileData, int location)
    {
        ReadScrollEntry(layers, horizontal, fileData, location, 0);
    }

    private static void LoadEightPixelStripHScrollData(LayerGroup layers, byte[] fileData, int location)
    {
        for(int strip = 0; strip < layers.Background.HScrollTable.Values.Length; strip++)
        {
            ReadScrollEntry(layers, true, fileData, location, strip);
            location += 32;
        }
    }

    private static void LoadEightPixelStripVScrollData(LayerGroup layers, byte[] fileData, int location)
    {
        for (int strip = 0; strip < layers.Background.HScrollTable.Values.Length; strip++)
        {
            ReadScrollEntry(layers, false, fileData, location, strip);
            location += 4;
        }
    }

    private static void LoadScanlineScrollData(LayerGroup layers, byte[] fileData, int location)
    {
        for (int line = 0; line < layers.Background.HScrollTable.Values.Length; line++)
        {
            ReadScrollEntry(layers,true, fileData, location, line);
            location += 4;
        }
    }
}


