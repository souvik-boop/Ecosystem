namespace Ecosystem.Engine
{
    public class Constants
    {
        public enum EntityTypes
        {
            //Environmental entities
            Empty = 0,

            //Obstacle entities
            Tree = 1000,

            //Consumer entities
            Rabbit = 2000,
            FedRabbit = 2001,
            MovedRabbit = 2002,
            MatedRabbit = 2003,
            MovedAndFedRabbit = 2004,

            //Food Source entities
            Berries = 3000,
        }
    }
}
