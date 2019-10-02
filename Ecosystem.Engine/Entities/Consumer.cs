using System;
using System.Collections.Generic;
using System.Text;

namespace Ecosystem.Engine.Entities
{
    public abstract class Consumer
    {
        public int id { get; set; }
        public int SensoryRange { get; set; }
        public int Speed { get; set; }
        public int Lifetime { get; set; }
    }
}
