using System;
using System.Threading.Tasks;
using static Ecosystem.Engine.Constants;

namespace Ecosystem.Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            int numOfDays = 100;
            int dayCycle = 100;
            int areaSize = 40;
            int food = 12;
            int animal = 10;
            Engine.Main sim1 = new Engine.Main();
            int startDay = 1;
            while (animal != 0)
            {
                Console.Clear();
                var newResult = sim1.GenerateScene(areaSize, areaSize, food, animal).Result;
                int fedPop;
                GenerateDisplay(areaSize, newResult, out fedPop);
                Console.Clear();
                int iter = 0;
                bool flag = false;
                while (true)
                {
                    ++iter;
                    if (iter > dayCycle || flag) break;
                    else
                    {
                        Console.Clear();
                        newResult = sim1.NextFrame(areaSize, areaSize, newResult).Result;
                        flag = GenerateDisplay(areaSize, newResult, out fedPop);
                        Console.WriteLine("Time : " + iter);
                    }
                    Task.Delay(50).Wait();
                }
                animal = fedPop * 2;
                ++startDay;
                Console.WriteLine("Day Number : " + startDay);
                Console.WriteLine("Current Population : " + animal);
                Console.ReadKey();
            }
            Console.ReadLine();
        }

        private static bool GenerateDisplay(int size, int[,] result, out int fedPop)
        {
            int unfed = 0;
            int fed = 0;
            int food = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    switch ((EntityTypes)result[i, j])
                    {
                        case EntityTypes.Empty:
                            Console.Write(" ");
                            break;
                        case EntityTypes.Rabbit:
                            Console.Write("X");
                            unfed++;
                            break;
                        case EntityTypes.Berries:
                            Console.Write("o");
                            food++;
                            break;
                        case EntityTypes.FedRabbit:
                            Console.Write("#");
                            fed++;
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Unfed : {unfed}");
            Console.WriteLine($"Fed : {fed}");
            Console.WriteLine($"Food : {food}");
            fedPop = fed;
            return unfed == 0;
        }
    }
}
