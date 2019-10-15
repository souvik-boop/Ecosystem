using Ecosystem.Engine;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using static Ecosystem.Engine.Constants;

namespace Ecosystem.Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            //int numOfDays = 100;
            int dayCycle = 100;
            int areaSize = 50;
            int food = 10;
            int animal = 10;
            int obstacle = 500;
            Core sim1 = new Core();
            int startDay = 1;
            while (animal != 0)
            {
                Console.Clear();
                var newResult = sim1.GenerateScene(areaSize, areaSize, obstacle, food, animal).Result;
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
                    //Console.ReadKey();
                    //Task.Delay(1000).Wait();
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
                            //Console.Write(" ");
                            break;
                        case EntityTypes.Rabbit:
                            //Console.Write("X");
                            unfed++;
                            break;
                        case EntityTypes.Berries:
                            //Console.Write("o");
                            food++;
                            break;
                        case EntityTypes.Tree:
                            //Console.Write("|");
                            break;
                        case EntityTypes.FedRabbit:
                            //Console.Write("#");
                            fed++;
                            break;
                        default:
                            break;
                    }
                }
                //Console.WriteLine();
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
                                    gfx.FillRectangle(Brushes.Green, new Rectangle(x * squareWidth, y * squareHeight, squareWidth, squareHeight));
                                    break;
                                case EntityTypes.FedRabbit:
                                    gfx.FillRectangle(Brushes.DarkBlue, new Rectangle(x * squareWidth, y * squareHeight, squareWidth, squareHeight));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                bmp.Save($"Result/result.bmp");
            }

            Console.WriteLine($"Unfed : {unfed}");
            Console.WriteLine($"Fed : {fed}");
            Console.WriteLine($"Food : {food}");
            fedPop = fed;
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
