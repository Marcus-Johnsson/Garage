namespace Garage_1._1
{
    internal class Helpers
    {
        public static Random rnd = new Random();

        public static class UIsett
        {
            public static int cursorYPosition;
        }

        public static string NumberPlateGenerate(string PlateDone)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string platePart1 = "";
            string platePart2 = "";

            for (int i = 0; i < 3; i++)
            {
                // Generate Char's for the plate
                int getChar = rnd.Next(chars.Length);
                platePart1 += chars[getChar];


                int getNumb = rnd.Next(0, 10);
                platePart2 += getNumb.ToString();
            }
            PlateDone = platePart1 + " " + platePart2;

            return PlateDone;
        }

        public static string ColorGenerate(string Color)
        {
            string[] models = ["red", "yellow", "black", "grey", "white", "blue", "green", "brown"];
            int getColor = rnd.Next(models.Length);
            Color = models[getColor];
            return Color;
        }

        public static string ModelGenerate(string Model)
        {
            string[] models = ["Ducati", "Honda", "Kawasaki", "BMW Motorrad", "KTM", "YAMAHA MOTOR", "SUZUKI", "Mario Kart"];
            int getModel = rnd.Next(models.Length);
            Model = models[getModel];
            return Model;
        }
        //public static void AcceptParking()
        //{
        //    slot.ParkedVehicles.Add(vehicle);
        //    Program.WriteVehicleStatus(vehicle.TypeOfVehicle, "Parked", slot.SlotNumber);
        //    CarsToPark.Remove(vehicle);
        //    isParked = true;


        //}   Om tiden finns


        public static List<Vehicle> ParkingLogic(List<Vehicle> CarsToPark, List<ParkingSlots> slots, List<Vehicle> ParkedVehicle)
        {
            UIsett.cursorYPosition = 24;

            foreach (var vehicle in CarsToPark.ToList())
            {
                bool isParked = false;

                if (vehicle is Motorbike motorbike || vehicle is Car car)
                {
                    if (vehicle is Motorbike)
                    {
                        foreach (var slot in slots)
                        {
                            if (slot.ParkedVehicles.Count == 1 && slot.ParkedVehicles[0] is Motorbike)
                            {
                                slot.ParkedVehicles.Add(vehicle);
                                Program.WriteVehicleStatus(vehicle.TypeOfVehicle, "Parked", slot.SlotNumber);
                                CarsToPark.Remove(vehicle);
                                isParked = true;
                                break;
                            }

                        }

                    }
                    if (!isParked)
                    {
                        foreach (var slot in slots)
                        {
                            var slot1 = slots[0];
                            var slot2 = slots[1];
                            // check if there's an gap slot availible and check slot 1 
                            if (IsGapSlot(slot, slots) || slot1.ParkedVehicles.Count == 0 && slot2.ParkedVehicles.Count > 0)
                            {
                                slot.ParkedVehicles.Add(vehicle);
                                Program.WriteVehicleStatus(vehicle.TypeOfVehicle, "Parked", slot.SlotNumber);
                                CarsToPark.Remove(vehicle);
                                isParked = true;
                                break;
                            }
                        }

                    }
                    if (!isParked)
                    {
                        foreach (var slot in slots)
                        {
                            int index = slots.IndexOf(slot);
                            var currentSlot = index >= 0 && index < slots.Count ? slots[index] : null;
                            var nextSlot = index >= 0 && index < slots.Count - 1 ? slots[index + 1] : null;
                            var thirdSlot = index >= 0 && index < slots.Count - 2 ? slots[index + 2] : null;

                            // Check if slots exist
                            if (currentSlot.ParkedVehicles.Count != null && nextSlot.ParkedVehicles.Count != null && thirdSlot.ParkedVehicles.Count != null)
                            {
                                // Check if empty
                                if (currentSlot.ParkedVehicles.Count == 0 && nextSlot.ParkedVehicles.Count == 0 && thirdSlot.ParkedVehicles.Count == 0)
                                {
                                    currentSlot.ParkedVehicles.Add(vehicle);
                                    Program.WriteVehicleStatus(vehicle.TypeOfVehicle, "Parked", currentSlot.SlotNumber);
                                    CarsToPark.Remove(vehicle);
                                    isParked = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!isParked)
                    {
                        foreach (var slot in slots)
                        {
                            // check if there's an slot availible 
                            if (slot.ParkedVehicles.Count == 0)
                            {
                                slot.ParkedVehicles.Add(vehicle);
                                Program.WriteVehicleStatus(vehicle.TypeOfVehicle, "Parked", slot.SlotNumber);
                                CarsToPark.Remove(vehicle);
                                isParked = true;
                                break;
                            }
                        }
                    }

                    if (!isParked)
                    {
                        CarsToPark.Remove(vehicle);

                        // Write out car drove away
                        Program.WriteVehicleStatus(vehicle.TypeOfVehicle, "Drove", 0);

                        break;
                    }
                }
                if (vehicle is Buss buss) //park busses
                {
                    for (int i = 0; i < slots.Count - 1; i++)
                    {
                        var nextSlot = slots[i + 1];
                        var slot = slots[i];

                        // step one: check list for two slots

                        if (IsTwoGapSlot(slot, slots))
                        {
                            Program.WriteVehicleStatus("Buss", "Parked", slot.SlotNumber);

                            slot.ParkedVehicles.Add(vehicle);
                            nextSlot.ParkedVehicles.Add(vehicle);
                            CarsToPark.Remove(vehicle);
                            isParked = true;
                            break;
                        }

                        if (!isParked)
                        {
                            { // step two: park in any two slots
                                if (slot.ParkedVehicles.Count == 0 && nextSlot.ParkedVehicles.Count == 0)
                                {
                                    Program.WriteVehicleStatus("Buss", "Parked", slot.SlotNumber);

                                    slot.ParkedVehicles.Add(vehicle);
                                    nextSlot.ParkedVehicles.Add(vehicle);
                                    CarsToPark.Remove(vehicle);
                                    isParked = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!isParked)
                    {
                        {
                            CarsToPark.Remove(vehicle);
                            // Write out buss drove away
                            Program.WriteVehicleStatus("Buss", "Drove", 0);
                            break;
                        }
                    }
                }
                break;
            }
            return ParkedVehicle;
        }
        private static bool IsGapSlot(ParkingSlots slot, List<ParkingSlots> slots)
        {
            int index = slots.IndexOf(slot);
            if (index < 0) return false;

            var prevSlot = index > 0 ? slots[index - 1] : null;
            var nextSlot = index < slots.Count - 1 ? slots[index + 1] : null;

            return (slot.ParkedVehicles.Count == 0) &&
                   (prevSlot != null && prevSlot.ParkedVehicles.Count > 0) &&
                   (nextSlot != null && nextSlot.ParkedVehicles.Count > 0);
        }

        private static bool IsTwoGapSlot(ParkingSlots slot, List<ParkingSlots> slots)
        {
            int index = slots.IndexOf(slot);
            if (index < 0 || index == slots.Count - 1) return false;

            var nextSlot = slots[index + 1];

            return slot.ParkedVehicles.Count == 0 && nextSlot.ParkedVehicles.Count == 0;
        }

        public static double ChargeParkingAutomatic(List<ParkingSlots> slots, List<Vehicle> ParkedVehicle, double houseMoney, bool reduce)
        {
            // sett base and reset the box position
            UIsett.cursorYPosition = 24;
            //-----------------------------------

            HashSet<Vehicle> sentVehicles = new HashSet<Vehicle>();
            foreach (ParkingSlots slot in slots)
            {
                int index = slots.IndexOf(slot);
                var prevSlot = index > 0 ? slots[index - 1] : null;

                if (slot.ParkedVehicles.Count > 0)
                {
                    for (int i = slot.ParkedVehicles.Count - 1; i >= 0; i--)
                    {
                        double payment = 0;
                        var vehicle = slot.ParkedVehicles[i];

                        if (reduce)
                        {
                            vehicle.ParkManuallPay++;
                        }

                        if (vehicle.ParkTime == vehicle.ParkManuallPay)
                        {

                            if (vehicle is Buss)
                            {
                                slot.ParkedVehicles.RemoveAt(i);
                                prevSlot.ParkedVehicles.Remove(vehicle);
                            }
                            else
                            {
                                slot.ParkedVehicles.RemoveAt(i);
                            }
                            //check 
                            int checkPark = vehicle.ParkManuallPay > vehicle.ParkTime ? vehicle.ParkTime : vehicle.ParkManuallPay;

                            // indentify Buss or not
                            double price = vehicle.TypeOfVehicle is Buss ? 1.8 : 1.2;
                            payment = checkPark * price;
                            string cash = payment.ToString();
                            Program.ExitGarage(vehicle.TypeOfVehicle, vehicle.PlateDone, cash);
                            houseMoney += payment;
                            continue;
                        }
                    }
                }
            }
            return houseMoney;
        }
        public static double ChargeParkingManuall(List<ParkingSlots> slots, List<Vehicle> ParkedVehicle, double houseMoney, bool automatic, string type)
        {


            // sett base and reset the box position
            UIsett.cursorYPosition = 24;
            //-----------------------------------

            HashSet<Vehicle> sentVehicles = new HashSet<Vehicle>();
            foreach (ParkingSlots slot in slots)
            {
                int index = slots.IndexOf(slot);
                var prevSlot = index > 0 ? slots[index - 1] : null;

                for (int i = slot.ParkedVehicles.Count - 1; i >= 0; i--)
                {
                    double payment = 0;

                    var vehicle = slot.ParkedVehicles[i];

                    if (type == vehicle.PlateDone)
                    {
                        vehicle.ParkManuallPay++;
                        if (vehicle is Buss)
                        {
                            // Remove from both the previous and current slots (if needed)
                            if (prevSlot != null && prevSlot.ParkedVehicles.Contains(vehicle))
                            {
                                prevSlot.ParkedVehicles.Remove(vehicle);

                                slot.ParkedVehicles.RemoveAt(i);
                                payment += vehicle.ParkManuallPay * 1.8;

                                string cash = payment.ToString();
                                Program.ExitGarage(vehicle.TypeOfVehicle, vehicle.PlateDone, cash);

                                houseMoney += payment;
                                break;
                            }
                        }
                        else
                        {
                            payment += vehicle.ParkManuallPay * 1.25;

                            slot.ParkedVehicles.RemoveAt(i);
                            string cash = payment.ToString();
                            Program.ExitGarage(vehicle.TypeOfVehicle, vehicle.PlateDone, cash);

                            houseMoney += payment;
                        }
                        break;
                    }
                    else
                    {
                        Console.SetCursorPosition(37, 50);
                        Console.WriteLine(type + " was a Invalid plate number");
                    }
                }
            }
            return houseMoney;
        }
    }
}
