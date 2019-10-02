using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Ecosystem.Engine.Constants;

namespace Ecosystem.Engine
{
    public class Main
    {
        private Task<int[,]> GenerateArea(int length, int breadth)
        {
            return Task.FromResult(new int[length, breadth]);
        }
        private Task<int[,]> GenerateConsumer(int length, int breadth, int count, int[,] matrix)
        {
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                var randomXPos = random.Next(0, length - 1);
                var randomYPos = random.Next(0, breadth - 1);
                if (matrix[randomXPos, randomYPos] == (int)EntityTypes.Empty)
                    matrix[randomXPos, randomYPos] = (int)EntityTypes.Rabbit;
                else
                {
                    --i;
                    continue;
                }

            }
            return Task.FromResult(matrix);
        }
        private Task<int[,]> GenerateFoodSource(int length, int breadth, int count, int[,] matrix)
        {
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                var randomXPos = random.Next(0, length - 1);
                var randomYPos = random.Next(0, breadth - 1);
                if (matrix[randomXPos, randomYPos] == (int)EntityTypes.Empty)
                    matrix[randomXPos, randomYPos] = (int)EntityTypes.Berries;
                else
                {
                    --i;
                    continue;
                }

            }
            return Task.FromResult(matrix);
        }
        public Task<int[,]> GenerateScene(int length, int breadth, int foodCount, int consumerCount)
        {
            var generatedArea = GenerateArea(length, breadth).Result;
            var generatedAreaWithFood = GenerateFoodSource(length, breadth, foodCount, generatedArea).Result;
            var generatedAreaWithConsumers = GenerateConsumer(length, breadth, consumerCount, generatedAreaWithFood).Result;
            return Task.FromResult(generatedAreaWithConsumers);
        }
        public Task<int[,]> NextFrame(int length, int breadth, int[,] matrix)
        {
            //var matrix = matrix;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < breadth; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        //Case 1 : when all the nearby positions are occupied and it cant move
                        int spaces = 0;
                        int occuSpaces = 0;
                        for (int k = Math.Max(0, i - 1); k <= Math.Min(length - 1, i + 1); k++)
                        {
                            for (int l = Math.Max(0, j - 1); l <= Math.Min(breadth - 1, j + 1); l++)
                            {
                                spaces++;
                                // obstacles
                                if (matrix[k, l] == (int)EntityTypes.FedRabbit || matrix[k, l] == (int)EntityTypes.Rabbit)
                                {
                                    occuSpaces++;
                                }
                            }
                        }
                        if (occuSpaces == spaces)
                        {
                            //Console.WriteLine($"Consumer ({i},{j}) can't move!");
                            continue;
                        }

                        //Case 2 : When there is a food source in the nearby slot
                        bool flag = false;
                        for (int k = Math.Max(0, i - 1); k <= Math.Min(length - 1, i + 1); k++)
                        {
                            for (int l = Math.Max(0, j - 1); l <= Math.Min(breadth - 1, j + 1); l++)
                            {
                                // move towards the food
                                if (matrix[k, l] == (int)EntityTypes.Berries)
                                {
                                    matrix[i, j] = (int)EntityTypes.Empty;
                                    matrix[k, l] = (int)EntityTypes.FedRabbit;
                                    flag = true;
                                    //Console.WriteLine($"Consumer from ({i},{j}) ate food at ({k},{l})!");
                                    break;
                                }
                            }
                            if (flag) break;
                        }
                        if (flag) continue;

                        //Case 3 : When all the nearby slots are empty
                        var numXPos = i;
                        var numYPos = j;

                        // check if postion isn't already occupied
                        while (matrix[numXPos, numYPos] == (int)EntityTypes.Rabbit ||
                               matrix[numXPos, numYPos] == (int)EntityTypes.FedRabbit ||
                               matrix[numXPos, numYPos] == (int)EntityTypes.MovedEntity)
                        {
                            numXPos = random.Next(Math.Max(0, i - 1), Math.Min(length - 1, i + 1) + 1);
                            numYPos = random.Next(Math.Max(0, j - 1), Math.Min(breadth - 1, j + 1) + 1);
                        }

                        // move to new postion
                        matrix[i, j] = (int)EntityTypes.Empty;
                        matrix[numXPos, numYPos] = (int)EntityTypes.MovedEntity;
                        //Console.WriteLine($"Moving Consumer from ({i},{j}) -> ({numXPos},{numYPos})");
                    }
                }
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < breadth; j++)
                {
                    if (matrix[i, j] == -2)
                        matrix[i, j] = 1;
                }
            }
            return Task.FromResult(matrix);
        }
    }
}
