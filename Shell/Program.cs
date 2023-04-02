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
        //This is used to store original path. It is used to find the commands after directory changes.
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

                if (splitInput[0] == "cd ")
                {
                    try
                    {
                        MemoryMappedViewStream mmvStream = mmf.CreateViewStream(0, 1024, MemoryMappedFileAccess.ReadWrite);
                        BinaryFormatter formatter = new BinaryFormatter();
                        string directory = (string)formatter.Deserialize(mmvStream);
                        Directory.SetCurrentDirectory(directory);
                    }
                    catch (Exception e)
                    {
                        //Exception should be handled but let's just do nothing here.
                    }
                }

                return 0;
            }
            if (input.StartsWith("wc "))
            {
                string[] inputParts = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (inputParts.Length < 2)
                {
                    Console.WriteLine("Please provide a file name after the 'wc' command.");
                    return 1;
                }
                string fileName = string.Join(" ", inputParts.Skip(1));
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(@".\WordCount.exe")
                    {
                        UseShellExecute = false,
                        Arguments = "\"" + fileName + "\""
                    }
                };

                process.Start();
                process.WaitForExit();

                return 0;
            }

            Console.WriteLine($"{input} not found");
            return 1;
        }
    }
}