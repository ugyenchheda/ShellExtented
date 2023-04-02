using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrentDirectory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var directory = Directory.GetCurrentDirectory();
            Console.WriteLine(directory);
        }
    }
}
