using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.Hosting;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            //Shell start
            var shell = new Shell();
            shell.originalPath = Directory.GetCurrentDirectory();
            shell.Run();
        }
    }

    public class Shell
    {
        public string originalPath;

        private Dictionary<string, string> Aliases = new Dictionary<string, string>
        {
            { "ls", @"ListDirectories.exe" },
            { "clear", @"Clear.exe" },
            { "pwd", @"CurrentDirectory.exe" },
            { "cd", @"ChangeDirectory.exe" }
        };

        public void Run()
        {
            string input = null;

            do
            {
                Console.Write("$ ");
                input = Console.ReadLine();
                Execute(input);
            } while (input != "exit");
        }

        public int Execute(string input)
{
    string[] splitInput = input.Split(' ');
    if (Aliases.Keys.Contains(splitInput[0]))
    {
        var process = new Process();
        process.StartInfo = new ProcessStartInfo(this.originalPath + "\\" + Aliases[splitInput[0]])
        {
            UseShellExecute = false,
        };
        if (splitInput.Length > 1)
        {
            process.StartInfo.Arguments = splitInput[1];
        }

        MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen("sharedVar", 1024);

        process.Start();
        process.WaitForExit();

        // Get the current directory from the memory mapped file
        MemoryMappedViewStream mmvStream = mmf.CreateViewStream(0, 1024, MemoryMappedFileAccess.ReadWrite);
        BinaryFormatter formatter = new BinaryFormatter();
        string directory = (string)formatter.Deserialize(mmvStream);

        // Set the current directory of the parent process
        Directory.SetCurrentDirectory(directory);

        return 0;
    }

    Console.WriteLine($"{input} not found");
    return 1;
}
    }
}
