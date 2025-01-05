using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Linq;

public class VramStealer
{ 
    [DllImport("kernel32.dll")]
    static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool CloseHandle(IntPtr hObject);

    // Process access flags
    const int PROCESS_VM_READ = 0x0010;

    private Process _gensProcess;
    private IntPtr _gensProcessHandle;
    private int _vramAddress;
    private int _cramAddress;
    private int _vsramAddress;


    public void Setup(int? knownVramAddress, int? knownCramAddress, int? knownVsramAddress)
    {
        _gensProcess = LaunchGens();
        _gensProcessHandle = OpenProcess(PROCESS_VM_READ, false, _gensProcess.Id);

        if (knownVramAddress.HasValue)        
            _vramAddress = knownVramAddress.Value;     
        
        if(knownCramAddress.HasValue)
            _cramAddress = knownCramAddress.Value;

        if (knownVsramAddress.HasValue)
            _vsramAddress = knownVsramAddress.Value;
    }

    public void ScanForVram()
    {
        _vramAddress = ScanForBytes("vram");
    }

    public void ScanForCram()
    {
        _cramAddress = ScanForBytes("cram");
    }

    public void ScanForVsRam()
    {
        Console.WriteLine("Pause gens, then save state to vstest.gs0");
        Console.ReadKey();

        var sampleData = File.ReadAllBytes($"D:\\GitHub\\PixelEngine\\PixelEngine.SampleGame\\Content\\GensSaveStates\\vstest.gs0");

        sampleData = sampleData.Skip(0x192).Take(0x50).ToArray();
        var maybeAddress = ScanMemory(_gensProcessHandle, 0, sampleData.Take(10).ToArray());
        maybeAddress = ScanMemory(_gensProcessHandle, _vramAddress, sampleData.Take(100).ToArray());

        Console.WriteLine($"VSRam found, press any key");
        Console.ReadKey();

        _vsramAddress = maybeAddress;
    }

    private int ScanForBytes(string name)
    {
        Console.WriteLine($"Pause Gens and export {name}, then press any key");
        Console.ReadKey();

        var sampleData= File.ReadAllBytes($"D:\\Games\\Emulation\\Genesis\\{name}.ram");

        var maybeAddress = ScanMemory(_gensProcessHandle, 0, sampleData.Take(10).ToArray());
        maybeAddress = ScanMemory(_gensProcessHandle, _vramAddress, sampleData.Take(100).ToArray());

        Console.WriteLine($"{name} found, press any key");
        Console.ReadKey();
        return maybeAddress;
    }

    public byte[] CurrentVram()
    {
        return ReadMemory(_gensProcessHandle, _vramAddress, 0x10000);
    }
    public byte[] CurrentCram()
    {
        return ReadMemory(_gensProcessHandle, _cramAddress, 256);
    }
    public byte[] CurrentVsram()
    {
        return ReadMemory(_gensProcessHandle, _vsramAddress, 256);
    }

    public void RecordSnapshots()
    {

        Console.WriteLine("Recording...");
        int num = 0;
        foreach (var snapshot in RecordVramSection(_gensProcessHandle, _vramAddress, 0x625, 0x631))
        {
            File.WriteAllBytes($"D:\\GitHub\\PixelEngine\\PixelEngine.SampleGame\\Content\\SampleVRAM\\batch\\kid\\out_{num.ToString("00")}.ram", snapshot);
            num++;
        }

        Console.WriteLine("Done!");

        Console.ReadKey();
    }

    public void Shutdown()
    {
        CloseHandle(_gensProcessHandle);
    }

    Process LaunchGens() => Process.Start(@"D:\Games\Emulation\Genesis\kmod73\gens.exe");

    byte[] ReadMemory(IntPtr processHandle, int address, int length)
    {
        byte[] buffer = new byte[length];
        int bytesRead = 0;

        ReadProcessMemory((int)processHandle, address, buffer, buffer.Length, ref bytesRead);

        if (bytesRead == length)
            return buffer;
        else
            return Array.Empty<byte>();
    }

    int ScanMemory(IntPtr processHandle, int start, byte[] search)
    {
        for (int i = start; i < int.MaxValue; i++)
        {
            if ((i % 100000) == 0)
                Console.Write('.');

            var result = ReadMemory(processHandle, i, search.Length);

            if (result.Length == search.Length)
            {
                if (Enumerable.SequenceEqual(result, search))
                {
                    Console.WriteLine();
                    Console.WriteLine($"Match found at {i}");
                    return i;
                }
            }
        }

        return -1;
    }


    byte[] ReadTileMemory(IntPtr processHandle, int vramAddress, int tileStart, int tileLength)
    {
        int tileAddress = vramAddress + (tileStart * 0x20);
        var numBytes = tileLength * 0x20;

        return ReadMemory(processHandle, tileAddress, numBytes);
    }


    IEnumerable<byte[]> RecordVramSection(IntPtr processHandle, int vramAddress, int tileStart, int tileEnd)
    {
        int tileLength = (tileEnd - tileStart) + 1;

        var snapshots = new List<byte[]>();

        while (true)
        {
            var tiles = ReadTileMemory(processHandle, vramAddress, tileStart, tileLength);

            bool alreadyTaken = false;
            foreach (var snapshot in snapshots)
            {
                if (Enumerable.SequenceEqual(snapshot, tiles))
                {
                    alreadyTaken = true;
                    break;
                }
            }

            if (alreadyTaken)
                continue;

            Console.WriteLine("Snapshot taken");
            snapshots.Add(tiles);

            yield return tiles;
        }
    }
}