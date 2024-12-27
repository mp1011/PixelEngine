using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

[DllImport("kernel32.dll")]
static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

[DllImport("kernel32.dll", SetLastError = true)]
static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

[DllImport("kernel32.dll", SetLastError = true)]
static extern bool CloseHandle(IntPtr hObject);

// Process access flags
const int PROCESS_VM_READ = 0x0010;


var gensProcess = LaunchGens();
IntPtr gensProcessHandle = OpenProcess(PROCESS_VM_READ, false, gensProcess.Id);

Console.WriteLine("Pause Gens and export vram, then press any key");
Console.ReadKey();

var sampleVram = File.ReadAllBytes("vram.ram");

var vramAddress = ScanMemory(gensProcessHandle, sampleVram.Take(10).ToArray());

Console.WriteLine("Vram found, press any key to begin recording");

var recordedFrames = RecordVramSection(gensProcessHandle, vramAddress, 0x625, 0x631,32);

int num = 0;
foreach(var frame in recordedFrames)
{
    File.WriteAllBytes($"D:\\GitHub\\PixelEngine\\PixelEngine.SampleGame\\Content\\SampleVRAM\\batch\\kid_walk\\out_{num}.ram", frame);
    num++;
}

Console.WriteLine("Done!");

Console.ReadKey();

CloseHandle(gensProcessHandle);

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

int ScanMemory(IntPtr processHandle, byte[] search)
{
    for (int i = 0; i < int.MaxValue; i++)
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


List<byte[]> RecordVramSection(IntPtr processHandle, int vramAddress, int tileStart, int tileEnd, int maxFrames)
{
    int tileLength = (tileEnd - tileStart) + 1;

    var snapshots = new List<byte[]>();

    while (true)
    {
        var tiles = ReadTileMemory(processHandle, vramAddress, tileStart, tileLength);

        if (snapshots.Count == 0 || !Enumerable.SequenceEqual(snapshots.Last(), tiles))
            snapshots.Add(tiles);

        if (snapshots.Count == maxFrames)
            return snapshots;
    }
}