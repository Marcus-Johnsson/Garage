using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_1._1
{
    internal class Buss : Vehicle
    {
        public int Passenger { get; set; }

        public Buss() : base ("Buss")
        {
            Passenger = rnd.Next(10,30);
        }
    }
}
