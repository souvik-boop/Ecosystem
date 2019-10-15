using System;
using System.Collections.Generic;
using System.Text;
using static Ecosystem.Engine.Constants;

namespace Ecosystem.Engine.Entities.Trees
{
    public class Pine : Obstacle
    {
        public Pine()
        {
            this.Id = (int)EntityTypes.Tree;
            this.Lifetime = int.MaxValue;
        }
    }
}
