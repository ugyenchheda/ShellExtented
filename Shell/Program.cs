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
            //Separate command and arguments
            string[] splitInput = input.Split(' ');
            if (Aliases.Keys.Contains(splitInput[0]))
            {
                var process = new Process();
                //TODO: Test print
                //Console.WriteLine(this.originalPath + Aliases[splitInput[0]]);
                process.StartInfo = new ProcessStartInfo(this.originalPath + "\\" + Aliases[splitInput[0]])
                {
                    UseShellExecute = false,
                };

                //Just check if there are arguments and pass the first to the application
                //This is bit "dirty" way as all the arguments should be usually passed
                if (splitInput.Length > 1)
                {
                    process.StartInfo.Arguments = splitInput[1];
                }

                //MemoryMappedFile is used for interprocess communication
                //It works but looks bit clumsy way. Mutex should be used to protect the value.
                MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen("sharedVar", 1024);

                process.Start();
                process.WaitForExit();

                //Set directory only in case of cd command
                if (splitInput[0] == "cd ")
                {
                    try
                    {
                        //Read the MemoryMappedFile and set directory based on it
                        MemoryMappedViewStream mmvStream = mmf.CreateViewStream(0, 1024, MemoryMappedFileAccess.ReadWrite);
                        //Binary formatter is used to deserialize the shared stream
                        BinaryFormatter formatter = new BinaryFormatter();
                        string directory = (string)formatter.Deserialize(mmvStream);
                        //TODO: a test print
                        //Console.WriteLine(directory);
                        //Set the directory base to input
                        Directory.SetCurrentDirectory(directory);
                    }
                    catch (Exception e)
                    {
                        //Exception should be handled but let's just do nothing here.
                        //We end up here if directory change failed and those are handled in ChangeDirectory process.
                    }
                }
                
                return 0;
            }

            Console.WriteLine($"{input} not found");
            return 1;
        }
    }
}
