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
            string fileContents = File.ReadAllText(filePath);
            string[] words = fileContents.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            //int newwordCount = words.Length;

            Console.WriteLine("My solution: Word Count:  {0}.", newwordCount);
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    char[] splitterChars = { };
                    int counter = fileContents.Split(splitterChars, StringSplitOptions.RemoveEmptyEntries).Length;
                    Console.WriteLine("Teacher's Solution: Word Count: {0}.", counter);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading the file: " + ex.Message);
            }




            Console.ReadKey();
        }
    }
}
