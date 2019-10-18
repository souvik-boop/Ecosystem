using Ecosystem.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Ecosystem.Engine.Constants;

namespace Ecosystem.Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            int dayCycle = 100;
            int areaSize = 70;
            int food = 100;
            int animal = 100;
            int obstacle = 0;
            Core sim1 = new Core();
            int startDay = 1;
            List<int> pop = new List<int>();

            while (animal != 0)
            {
                //GeneratePopulationProgressGraph(animal, startDay);
                pop.Add(animal);
                if (pop.Count > 5) pop.Remove(pop.FirstOrDefault());

                Console.Clear();
                var newResult = sim1.GenerateScene(areaSize, areaSize, obstacle, food, animal).Result;
                int fedPop;
                int matedPop;
                GenerateDisplay(areaSize, newResult, out fedPop, out matedPop);
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
                        flag = GenerateDisplay(areaSize, newResult, out fedPop, out matedPop);
                        Console.WriteLine("Time : " + iter);
                    }
                    //Console.ReadKey();
                    Task.Delay(10).Wait();
                }

                animal = fedPop + matedPop * 4;
                ++startDay;
                Console.WriteLine("Day Number : " + startDay);
                Console.WriteLine("Current Population : " + animal);

                foreach (var item in pop)
                {
                    Console.Write(item + " --> ");
                }
                Console.WriteLine();
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Going to next day in 5 secs ...");
                Task.Delay(5000).Wait();
                //Console.ReadKey();
            }
            Console.Clear();
            Console.WriteLine($"Population survived for {startDay} days");
        }

        private static bool GenerateDisplay(int size, int[,] result, out int fedPop, out int matedPop)
        {
            int unfed = 0;
            int fed = 0;
            int mated = 0;
            int food = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    switch ((EntityTypes)result[i, j])
                    {
                        case EntityTypes.Empty:
                            break;
                        case EntityTypes.Rabbit:
                            unfed++;
                            break;
                        case EntityTypes.Berries:
                            food++;
                            break;
                        case EntityTypes.Tree:
                            break;
                        case EntityTypes.FedRabbit:
                            fed++;
                            break;
                        case EntityTypes.MatedRabbit:
                            mated++;
                            break;
                        default:
                            break;
                    }
                }
            }

            const int squareWidth = 10;
            const int squareHeight = 10;
            using (Bitmap bmp = new Bitmap((result.GetUpperBound(0) + 1) * squareWidth, (result.GetUpperBound(1) + 1) * squareHeight))
            {
                using (Graphics gfx = Graphics.FromImage(bmp))
                {
                    gfx.Clear(Color.Black);
                    for (int y = 0; y <= result.GetUpperBound(1); y++)
                    {
                        for (int x = 0; x <= result.GetUpperBound(0); x++)
                        {
                            switch ((EntityTypes)result[x, y])
                            {
                                case EntityTypes.Empty:
                                    gfx.FillRectangle(Brushes.White, new Rectangle(x * squareWidth, y * squareHeight, squareWidth, squareHeight));
                                    break;
                                case EntityTypes.Rabbit:
                                    gfx.FillRectangle(Brushes.LightBlue, new Rectangle(x * squareWidth, y * squareHeight, squareWidth, squareHeight));
                                    break;
                                case EntityTypes.Berries:
                                    gfx.FillRectangle(Brushes.Yellow, new Rectangle(x * squareWidth, y * squareHeight, squareWidth, squareHeight));
                                    break;
                                case EntityTypes.Tree:
                                    gfx.FillRectangle(Brushes.DarkGreen, new Rectangle(x * squareWidth, y * squareHeight, squareWidth, squareHeight));
                                    break;
                                case EntityTypes.FedRabbit:
                                    gfx.FillRectangle(Brushes.DarkBlue, new Rectangle(x * squareWidth, y * squareHeight, squareWidth, squareHeight));
                                    break;
                                case EntityTypes.MatedRabbit:
                                    gfx.FillRectangle(Brushes.DarkRed, new Rectangle(x * squareWidth, y * squareHeight, squareWidth, squareHeight));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                try
                {
                    bmp.Save($"Result/result.bmp");
                }
                catch (Exception)
                {
                }
            }

            Console.WriteLine($"Unfed : {unfed}");
            Console.WriteLine($"Fed : {fed}");
            Console.WriteLine($"Food : {food}");
            Console.WriteLine($"Mated : {mated}");
            fedPop = fed;
            matedPop = mated;
            return unfed == 0;
        }

        private static PointF[] StarPoints(int num_points, Rectangle bounds)
        {
            // Make room for the points.
            PointF[] pts = new PointF[num_points];

            double rx = bounds.Width / 2;
            double ry = bounds.Height / 2;
            double cx = bounds.X + rx;
            double cy = bounds.Y + ry;

            // Start at the top.
            double theta = -Math.PI / 2;
            double dtheta = 4 * Math.PI / num_points;
            for (int i = 0; i < num_points; i++)
            {
                pts[i] = new PointF(
                    (float)(cx + rx * Math.Cos(theta)),
                    (float)(cy + ry * Math.Sin(theta)));
                theta += dtheta;
            }
            return pts;
        }
    }
}
