using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_1._1
{
    internal class Motorbike : Vehicle
    {
        public string Model { get; set; }

        public Motorbike() : base("Motorbike")
        {
            Model = Helpers.ModelGenerate(Model);
        }
    }
}
