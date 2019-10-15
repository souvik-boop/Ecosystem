using System;
using System.Collections.Generic;
using System.Text;

namespace Ecosystem.Engine.Entities
{
    public abstract class Obstacle
    {
        public int Id { get; set; }
        public int Lifetime { get; set; }
    }
}
