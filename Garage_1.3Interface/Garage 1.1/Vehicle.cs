namespace Garage_1._1
{
    public interface VehicleInfo
    {

        string TypeOfVehicle { get; set; }

        string PlateDone { get; set; }

        string Color { get; set; }

        int ParkTime { get; set; }

        int ParkManuallPay { get; set; }
    }
    class Vehicle : VehicleInfo
    {
        public Vehicle(string typeOfVehicle)
        {
            TypeOfVehicle = typeOfVehicle;
            PlateDone = Helpers.NumberPlateGenerate(typeOfVehicle);
            Color = Helpers.ColorGenerate(typeOfVehicle);
            ParkManuallPay = 0;
            ParkTime = rnd.Next(3, 8);
        }

        public string TypeOfVehicle { get; set; }

        public string PlateDone { get; set; }

        public string Color { get; set; }

        public int ParkTime { get; set; }

        public int ParkManuallPay { get; set; }


        public static Random rnd;

        static Vehicle()
        {
            rnd = new Random();
        }
        public static List<Vehicle> CreateCars(string type)
        {
            int whatVehicle = 3;
            List<Vehicle> CarsToPark = new List<Vehicle>();
            if (type == "RANDOM")
            {
                whatVehicle = rnd.Next(3);
                switch (whatVehicle)
                {
                    case 0:
                        {


                            CarsToPark.Add(new Car());
                            break;
                        }
                    case 1:
                        {


                            CarsToPark.Add(new Motorbike());
                            break;
                        }
                    case 2:
                        {


                            CarsToPark.Add(new Buss());
                            break;
                        }
                    case 3:
                        {
                            break;
                        }
                }
            }
            else if (type == "CAR")
            {
                CarsToPark.Add(new Car());
            }
            else if (type == "MOTORBIKE")
            {
                CarsToPark.Add(new Motorbike());
            }
            else if (type == "BUSS")
            {
                CarsToPark.Add(new Buss());
            }

            return CarsToPark;
        }
    }


}


