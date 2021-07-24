using System;
using KLibrary;

namespace Sample
{
    class Sample
    {
        static void Main()
        {
            LaunchPF1();
        }

        static void LaunchPF1()
        {
            Console.Write("Height : ");
            int height = Convert.ToInt32(Console.ReadLine());
            Console.Write("Width : ");
            int width = Convert.ToInt32(Console.ReadLine());

            if (width > 20 || height > 20)
            {
                Console.WriteLine("Number must less than 20");
                LaunchPF1();
            }

            Console.Write("Start Pos : ");
            int[] startPos = Array.ConvertAll(
                Console.ReadLine().Split(' '), x => int.Parse(x));

            Console.Write("End Pos : ");
            int[] endPos = Array.ConvertAll(
                Console.ReadLine().Split(' '), x => int.Parse(x));

            Pathfinding p = new Pathfinding(height, width, startPos, endPos);
            p.Launch();
        }
    }
}
