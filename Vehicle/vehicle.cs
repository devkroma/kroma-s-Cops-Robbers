using GTANetworkServer;
using GTANetworkShared;
using kroma_cnr.Database;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace kroma_cnr.Vehicle
{
    class vehicle
    {
        public static List<string> vehicleNames;

        public static Dictionary<int, VehicleVar> VehicleVar = new Dictionary<int, VehicleVar>();

        public static void Main()
        {
            loadVehicleNamesToList();
            API.shared.consoleOutput("[VEHICLE] Vehicle script loaded.");
        }

        public static void loadVehicleNamesToList()
        {
            vehicleNames = File.ReadAllLines("resources/kroma-cnr/Vehicle/vehicleModels.txt").ToList();
        }

        public static void trySpawnNewVehicle(VehicleHash model, Vector3 pos, Vector3 rot, int colour1, int colour2, int dimension = 0)
        {

        }

        public static void trySpawnNewVehicleForPlayer(Client player, VehicleHash model, Vector3 pos, Vector3 rot, int colour1, int colour2, int dimension = 0)
        {
            VehicleVar newVehicle = new VehicleVar();
            newVehicle.Model = model;
            newVehicle.Colour1 = colour1;
            newVehicle.Colour2 = colour2;
            newVehicle.SpawnPos = pos;
            newVehicle.SpawnRot = rot;
            newVehicle.SpawnDim = dimension;
            bool isFinished = false;
            int i = 0;
            while(!isFinished)
            {
                if(VehicleVar.ContainsKey(i))
                {
                    if(VehicleVar[i] == null)
                    {
                        newVehicle.VehicleHandle = API.shared.createVehicle(model, pos, rot, colour1, colour2, dimension);
                        VehicleVar[i] = newVehicle;
                        isFinished = true;
                        API.shared.consoleOutput("[VEHICLE] A new vehicle has been created by {0} with Vehicle ID {1}", player.name, i);
                        API.shared.setPlayerIntoVehicle(player, newVehicle.VehicleHandle, -1);
                        break;
                    }
                }
                else
                {
                    newVehicle.VehicleHandle = API.shared.createVehicle(model, pos, rot, colour1, colour2, dimension);
                    VehicleVar.Add(i, newVehicle);
                    isFinished = true;
                    API.shared.consoleOutput("[VEHICLE] A new vehicle has been created by {0} with Vehicle ID {1}", player.name, i);
                    API.shared.setPlayerIntoVehicle(player, newVehicle.VehicleHandle, -1);
                    break;
                }
                i++;
                if(i > 500)
                {
                    API.shared.consoleOutput("[VEHICLE] Something went wrong in trySpawnNewVehicleForPlayer");
                    isFinished = true;
                    break;
                }
            }
        }
    }
}
