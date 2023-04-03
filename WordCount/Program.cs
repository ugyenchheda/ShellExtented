using System;
using System.IO;
using System.Linq;

namespace WordCount
{
    internal class Program
    {

        static void Main(string[] args)
        {


            string fileName = args[0];
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            int wordCount = 0;

            string fileContents = File.ReadAllText(filePath);

            string[] words = fileContents.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            int newwordCount = words.Length;

            Console.WriteLine("The new file contains {0} words.", newwordCount);
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        wordCount += line.Split(' ', '\n', '\r', '\t').Length;
                    }
                }


                //string contents = File.ReadAllText(filePath);
                ////string[] words = Regex.Split(contents, @"\s+");
                //string[] words = contents.Split(new char[] {',',  '.',  ';',  ':', ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                ////string[] words = Regex.Split(contents, @"\W+");

                var count = File.ReadAllText(filePath).Split(' ').Count();
                //wordCount = words.Length;
                Console.WriteLine("Word count is {0}", count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading the file: " + ex.Message);
            }

            Console.WriteLine("Number of words in the file: " + wordCount);



            Console.ReadKey();
        }
    }
}
