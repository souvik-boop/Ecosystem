using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Ecosystem.Engine.Constants;

namespace Ecosystem.Engine
{
    public class Core
    {
        private Task<int[,]> GenerateArea(int length, int breadth)
        {
            int[,] result = new int[length, breadth];
            return Task.FromResult(result);
        }
        private Task<int[,]> GenerateEntity(int length, int breadth, int[,] matrix, int count, EntityTypes type)
        {
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                var randomXPos = random.Next(0, length - 1);
                var randomYPos = random.Next(0, breadth - 1);
                if (matrix[randomXPos, randomYPos] == (int)EntityTypes.Empty)
                    matrix[randomXPos, randomYPos] = (int)type;
                else
                {
                    --i;
                    continue;
                }
            }
            return Task.FromResult(matrix);
        }
        public Task<int[,]> GenerateScene(int length, int breadth, int obstacleCount, int foodCount, int consumerCount)
        {
            var generatedArea = GenerateArea(length, breadth).Result;
            var generatedAreaWithObstacle = GenerateEntity(length, breadth, generatedArea, obstacleCount, EntityTypes.Tree).Result;
            var generatedAreaWithFood = GenerateEntity(length, breadth, generatedAreaWithObstacle, foodCount, EntityTypes.Berries).Result;
            var generatedAreaWithConsumers = GenerateEntity(length, breadth, generatedAreaWithFood, consumerCount, EntityTypes.Rabbit).Result;
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
                    if (matrix[i, j] == (int)EntityTypes.Rabbit)
                    {
                        //Case 1 : when all the nearby positions are occupied and it cant move
                        int spaces = 0;
                        int occupiedSpaces = 0;
                        for (int k = Math.Max(0, i - 1); k <= Math.Min(length - 1, i + 1); k++)
                        {
                            for (int l = Math.Max(0, j - 1); l <= Math.Min(breadth - 1, j + 1); l++)
                            {
                                spaces++;
                                // obstacles
                                if (matrix[k, l] == (int)EntityTypes.FedRabbit ||
                                    matrix[k, l] == (int)EntityTypes.Rabbit ||
                                    matrix[k, l] == (int)EntityTypes.Tree)
                                {
                                    occupiedSpaces++;
                                }
                            }
                        }
                        if (occupiedSpaces == spaces)
                        {
                            continue;
                        }

                        //Case 2 : When there is a food source in the nearby slot
                        bool flag = false;
                        for (int k = Math.Max(0, i - 2); k <= Math.Min(length - 1, i + 2); k++)
                        {
                            for (int l = Math.Max(0, j - 2); l <= Math.Min(breadth - 1, j + 2); l++)
                            {
                                // move towards the food
                                if (matrix[k, l] == (int)EntityTypes.Berries)
                                {
                                    double distanceBetweenConsumerAndFood = Math.Sqrt((i - k) * (i - k) + (j - l) * (j - l));
                                    if (distanceBetweenConsumerAndFood <= 1.0F)
                                    {
                                        matrix[i, j] = (int)EntityTypes.Empty;
                                        matrix[k, l] = (int)EntityTypes.FedRabbit;
                                        Console.WriteLine($"Found food at ({k},{l}) | Moving from : ({i},{j}) --> ({k},{l})");
                                    }
                                    else
                                    {
                                        bool innerFlag = false;
                                        // move towards the closest empty space to move closer to food
                                        for (int k1 = Math.Max(0, i - 1); k1 <= Math.Min(length - 1, i + 1); k1++)
                                        {
                                            for (int l1 = Math.Max(0, j - 1); l1 <= Math.Min(breadth - 1, j + 1); l1++)
                                            {
                                                double distanceBetweenFoodAndEmptySpace = Math.Sqrt((k - k1) * (k - k1) + (l - l1) * (l - l1));
                                                if (matrix[k1, l1] == (int)EntityTypes.Empty)
                                                {
                                                    if (distanceBetweenFoodAndEmptySpace < distanceBetweenConsumerAndFood)
                                                    {
                                                        matrix[i, j] = (int)EntityTypes.Empty;
                                                        matrix[k1, l1] = (int)EntityTypes.MovedRabbit;
                                                        Console.WriteLine($"Found food at ({k},{l}) | Moving from : ({i},{j}) --> ({k1},{l1})");
                                                        innerFlag = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (innerFlag) break;
                                        }
                                    }
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag) break;
                        }
                        if (flag) continue;

                        //Case 3 : When all the nearby slots are empty
                        var numXPos = i;
                        var numYPos = j;
                        double distance = 0;

                        // check if postion isn't already occupied
                        while (matrix[numXPos, numYPos] == (int)EntityTypes.Rabbit ||
                               matrix[numXPos, numYPos] == (int)EntityTypes.FedRabbit ||
                               matrix[numXPos, numYPos] == (int)EntityTypes.MovedRabbit ||
                               matrix[numXPos, numYPos] == (int)EntityTypes.Tree ||
                               distance != 1)
                        {
                            numXPos = random.Next(Math.Max(0, i - 1), Math.Min(length - 1, i + 1) + 1);
                            numYPos = random.Next(Math.Max(0, j - 1), Math.Min(breadth - 1, j + 1) + 1);
                            distance = Math.Sqrt((numXPos - i) * (numXPos - i) + (numYPos - j) * (numYPos - j));
                        }

                        // move to new postion
                        matrix[i, j] = (int)EntityTypes.Empty;
                        matrix[numXPos, numYPos] = (int)EntityTypes.MovedRabbit;
                    }
                }
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < breadth; j++)
                {
                    if (matrix[i, j] == (int)EntityTypes.MovedRabbit)
                        matrix[i, j] = (int)EntityTypes.Rabbit;
                }
            }
            return Task.FromResult(matrix);
        }
        public Task<int> DuplicateEntity(int length, int breadth, int[,] matrix, EntityTypes type)
        {
            int counter = 1;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < breadth; j++)
                {
                    if (matrix[i, j] == (int)EntityTypes.FedRabbit)
                        counter++;
                }
            }
            return Task.FromResult(counter);
        }
    }
}
