using System;
using System.Collections.Generic;
using System.Text;
using static Ecosystem.Engine.Constants;

namespace Ecosystem.Engine.Entities.Creatures
{
    public class Rabbit : Consumer
    {
        public Rabbit()
        {
            this.Id = (int)EntityTypes.Rabbit;
            this.Lifetime = 1;
            this.Speed = 1;
            this.SensoryRange = 1;
        }
    }
}
