using System.Runtime.InteropServices;
using static Garage_1._1.Helpers;

namespace Garage_1._1
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            
            string type = "";
            List<Vehicle> CarsToPark = Vehicle.CreateCars(type);
            List<ParkingSlots> slots = ParkingSlots.CreateSlots(15);
            List<Vehicle> ParkedVehicles = new List<Vehicle>();
            double houseMoney = 0;
            bool reduce;
            bool automatic = false;
            
            Console.SetWindowSize(200, 50);

            while (true)
            {
                Helpers.ParkingLogic(CarsToPark, slots, ParkedVehicles);
                WriteOutUI(houseMoney, slots);

                if (!automatic)
                {
                    while (true)
                    {
                        type = Console.ReadLine()?.ToUpper();
                        if (type == "CAR" || type == "BUSS" || type == "MOTORBIKE" || type == "RANDOM")
                        {
                            Console.Clear();
                            CarsToPark = Vehicle.CreateCars(type);

                            foreach (ParkingSlots slot in slots)
                            {
                                for (int i = slot.ParkedVehicles.Count - 1; i >= 0; i--)
                                {
                                    var vehicle = slot.ParkedVehicles[i];
                                    vehicle.ParkManuallPay++;
                                }    
                            }
                            break;
                        }
                        else if (type == "AUTO")
                        {
                            automatic = true;
                            break;
                        }
                        if (type != "AUTO")
                        {
                            Console.Clear();
                            Helpers.ChargeParkingManuall(slots, ParkedVehicles, houseMoney, automatic, type);
                            WriteOutUI(houseMoney, slots);
                            break;
                        }
                    }
                }
                if (automatic)
                {
                    while (automatic)
                    {
                        type = "RANDOM";
                        CarsToPark = Vehicle.CreateCars(type);
                        Console.Clear();
                        
                        Helpers.ParkingLogic(CarsToPark, slots, ParkedVehicles);

                        houseMoney = Helpers.ChargeParkingAutomatic(slots, ParkedVehicles, houseMoney, true);

                        DisplayGarage(slots);
                        Console.SetCursorPosition(37, 40);
                        Console.WriteLine("Yeild: " + houseMoney);
                        Console.SetCursorPosition(37, 41);
                        Console.WriteLine("To exit Auto press space (beta version takes some time to exit)");



                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey(intercept: true);
                            switch (key.KeyChar)
                            {
                                case ' ':
                                    {
                                        automatic = false;
                                        break;
                                    }
                            }
                        }
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                }
            }
        }
        public static void DisplayGarage(List<ParkingSlots> slots)
        {
            
            int slotWidth = 12;
            int typeWidth = 12;
            int plateWidth = 12;
            int vehicleInfoWidth = 13;
            int colorWidth = 10;
            int totalWidth = slotWidth + typeWidth + plateWidth + vehicleInfoWidth + colorWidth + 3 * 4;
            
            int consoleWidth = Console.WindowWidth;
            int leftMargin = (consoleWidth - totalWidth) / 2;
            if (leftMargin < 0) leftMargin = 0;

            // Initial Y position 
            int currentYPosition = 5;

            // Vehicle info
            Console.SetCursorPosition(leftMargin, currentYPosition++);
            Console.WriteLine("+-" + new string('-', totalWidth - 2) + "+");

            Console.SetCursorPosition(leftMargin, currentYPosition++);
            Console.WriteLine("| Slot Number ".PadRight(slotWidth + 1) +
                              "| Vehicle Type ".PadRight(typeWidth + 1) +
                              "| Plate Number ".PadRight(plateWidth + 1) +
                              "| Vehicle Info".PadRight(vehicleInfoWidth + 1) +
                              "| Color ".PadRight(colorWidth + 1) + "  |");

            Console.SetCursorPosition(leftMargin, currentYPosition++);
            Console.WriteLine("+-" + new string('-', totalWidth - 2) + "+");

            foreach (var slot in slots)
            {
                if (slot.ParkedVehicles.Count == 0)
                {
                    // write the list if empty
                    Console.SetCursorPosition(leftMargin, currentYPosition++);
                    Console.WriteLine("| " + slot.SlotNumber.ToString().PadRight(slotWidth) +
                                      " | " + "Empty".PadRight(typeWidth) +
                                      " |" + "".PadRight(plateWidth) +
                                      " |" + "".PadRight(vehicleInfoWidth) +
                                      "| " + "".PadRight(colorWidth) + " |");

                    Console.SetCursorPosition(leftMargin, currentYPosition++);
                    Console.WriteLine("+-" + new string('-', totalWidth - 2) + "+");
                }
                else
                {
                    foreach (var vehicle in slot.ParkedVehicles)
                    {
                        // write vehicle in list
                        string vehicleType = vehicle.TypeOfVehicle.PadRight(typeWidth);
                        string plateNumber = vehicle.PlateDone.PadRight(plateWidth);
                        string color = vehicle.Color.PadRight(colorWidth);

                        Console.SetCursorPosition(leftMargin, currentYPosition++);
                        Console.Write("| " + slot.SlotNumber.ToString().PadRight(slotWidth) + " | ");

                        string vehicleInfo = vehicle switch
                        {
                            Car car => car.Eletric ? "Electric Car".PadRight(vehicleInfoWidth) : "Fuel Car".PadRight(vehicleInfoWidth),
                            Motorbike motorbike => motorbike.Model.PadRight(vehicleInfoWidth),
                            Buss buss => buss.Passenger.ToString().PadRight(vehicleInfoWidth),
                            _ => "".PadRight(vehicleInfoWidth)
                        };

                        // Set vehicle type color
                        if (vehicle is Car)
                            Console.ForegroundColor = ConsoleColor.Blue;
                        else if (vehicle is Motorbike)
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        else if (vehicle is Buss)
                            Console.ForegroundColor = ConsoleColor.DarkGreen;

                        Console.Write(vehicleType);
                        Console.ResetColor();

                        // Print vehicle details
                        Console.WriteLine(" | " + plateNumber + "|" + vehicleInfo + "| " + color + " |");

                        Console.SetCursorPosition(leftMargin, currentYPosition++);
                        Console.WriteLine("+-" + new string('-', totalWidth - 2) + "+");
                    }
                }
            }
        }



        public static void ExitGarage(string vehicleType, string plateReg, string cash)
        {
            // The boxes on the right side

            Console.SetCursorPosition(137, UIsett.cursorYPosition);
            Console.ResetColor();
            Console.WriteLine("+------------------------------+");
            Console.ResetColor();
            Console.SetCursorPosition(137, UIsett.cursorYPosition - 1);
            Console.Write("| ");
            switch (vehicleType)
            {
                case "Motorbike":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "Car":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "Buss":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
            }
            Console.Write(vehicleType);
            Console.ResetColor();
            Console.Write(" " + plateReg + " payed " + cash);
            Console.ResetColor();

            Console.SetCursorPosition(168, UIsett.cursorYPosition - 1);
            Console.WriteLine("|");
            Console.SetCursorPosition(137, UIsett.cursorYPosition - 2);
            Console.ResetColor();
            Console.WriteLine("+------------------------------+");
            Console.ResetColor();

            UIsett.cursorYPosition += 4; // new location each time
        }
        public static void WriteVehicleStatus(string vehicleType, string status, int slot)
        {
            // The boxes on the left side

            Console.ForegroundColor = status == "Drove" ? ConsoleColor.Red : ConsoleColor.Green;
            Console.SetCursorPosition(37, UIsett.cursorYPosition);
            Console.WriteLine("+------------------------+");
            Console.SetCursorPosition(37, UIsett.cursorYPosition - 1);
            Console.Write("|  ");
            switch (vehicleType)
            {
                case "Motorbike":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "Car":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "Buss":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
            }
            Console.Write(vehicleType);
            Console.ForegroundColor = status == "Drove" ? ConsoleColor.Red : ConsoleColor.Green;
            Console.Write(status == "Drove" ? " drove away" : " parked in " + slot);
            Console.SetCursorPosition(62, UIsett.cursorYPosition - 1);
            Console.WriteLine("|");
            Console.SetCursorPosition(37, UIsett.cursorYPosition - 2);
            Console.WriteLine("+------------------------+");
            Console.ResetColor();

            UIsett.cursorYPosition += 4; // new location each timedon
        }
        public static void WriteOutUI(double houseMoney, List<ParkingSlots> slots)
        {
        DisplayGarage(slots);
        Console.SetCursorPosition(37, 40);
        Console.WriteLine("Yeild: " + houseMoney);
        Console.SetCursorPosition(37, 46);
        Console.WriteLine("Write a plate number to remove vehicle");
        Console.SetCursorPosition(37, 47);
        Console.WriteLine("To add a vehicle write what kind of vehicle you would like to add or random.");
        Console.SetCursorPosition(37, 48);
        Console.WriteLine("Don´t forget to try auto");
        Console.SetCursorPosition(37, 49);
        }
    }
}

// interface!!!

