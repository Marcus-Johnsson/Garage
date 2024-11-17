using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Garage_1._1
{
    interface Garage
    {
        
    }
    class ParkingSlots : Garage
    {
        public int SlotNumber { get;}
        public List<Vehicle> ParkedVehicles { get; }




        public ParkingSlots(int slotNumber)
        {
            SlotNumber = slotNumber;

            ParkedVehicles = new List<Vehicle>();
        }
        public static List<ParkingSlots> CreateSlots(int numberOfSlots = 15)
        {
            
            List<ParkingSlots> slots = new List<ParkingSlots>();

            for (int i = 0; i < numberOfSlots; i++)
            {
                slots.Add(new ParkingSlots(i + 1)); // Assign slot numbers starting from 1
            }

            return slots;
        }
    }
}


