using System;
using System.Collections.Generic;
using System.Text;

namespace Ecosystem.Engine.Entities
{
    public abstract class FoodSource
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public int Lifetime { get; set; }
    }
}
