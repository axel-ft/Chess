using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Projet C# - Echecs";
            Console.SetWindowSize(80, 40);
            Console.SetBufferSize(80, 80);

            Game test = new Game();

            Console.ReadKey();
        }
    }
}
