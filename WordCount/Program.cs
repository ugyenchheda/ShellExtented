using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordCount
{
    internal class Program
    {

        static void Main(string[] args)
        {


            string fileName = args[0];
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            int wordCount = 0;

            try
            {
                using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
                {
                    int totalWordCount = 0;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.Length > 0)
                        {
                            string pattern = @"[\p{L}\p{M}\p{P}\p{S}]+";
                            int newWordCount = Regex.Matches(line, pattern).Count;
                            totalWordCount += newWordCount;
                        }
                    }
                    Console.WriteLine("Total words in the file is {0}.", totalWordCount);
                }


                //string contents = File.ReadAllText(filePath);
                ////string[] words = Regex.Split(contents, @"\s+");
                //string[] words = contents.Split(new char[] {',',  '.',  ';',  ':', ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                ////string[] words = Regex.Split(contents, @"\W+");

                //int countingwords = contents.Split(words, StringSplitOptions.RemoveEmptyEntries).Length;
                //wordCount = words.Length;
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
