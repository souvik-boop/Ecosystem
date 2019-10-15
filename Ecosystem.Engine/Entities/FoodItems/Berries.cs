using System;
using System.Collections.Generic;
using System.Text;
using static Ecosystem.Engine.Constants;

namespace Ecosystem.Engine.Entities.FoodItems
{
    public class Berries : FoodSource
    {
        public Berries()
        {
            this.Id = (int)EntityTypes.Berries;
            this.Lifetime = 1;
            this.Quantity = 1;
        }
    }
}
