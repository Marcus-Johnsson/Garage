using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_1._1
{
    internal class Car : Vehicle
    {
        
        public bool Eletric { get; set; }

        public Car() : base ("Car")
        {
            int eletricChance = rnd.Next(1, 3);
            Eletric = (eletricChance == 1);
        }

    }

}
