using GTANetworkServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kroma_cnr.Vehicle
{
    class vehicle
    {
        public static List<string> vehicleNames;

        public static void Main()
        {
            loadVehicleNamesToList();
            API.shared.consoleOutput("[VEHICLE] Vehicle script loaded.");
        }

        public static void loadVehicleNamesToList()
        {
            vehicleNames = File.ReadAllLines("resources/kroma-cnr/Vehicle/vehicleModels.txt").ToList();
        }
    }
}
