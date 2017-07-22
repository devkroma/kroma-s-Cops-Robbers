using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;

namespace kroma_cnr.Database
{
    class buildings
    {
        public static Dictionary<int, BuildingVar> BuildingVar = new Dictionary<int, BuildingVar>();

        public static void trySpawnNewRobberyForPlayer(Client player, int robberyLevel, string robberyName)
        {
            BuildingVar newBuilding = new BuildingVar();
            Vector3 pos = API.shared.getEntityPosition(player.handle);
            newBuilding.Name = robberyName;
            newBuilding.RobberyPos = pos;
            newBuilding.RobberyLevel = robberyLevel;
            newBuilding.LastRobbed = 0;
            bool isFinished = false;
            int i = 0;
            while(!isFinished)
            {
                if(BuildingVar.ContainsKey(i))
                {
                    if(BuildingVar[i] == null)
                    {
                        var blip = API.shared.createBlip(pos);
                        API.shared.setBlipName(blip, robberyName);
                        API.shared.setBlipScale(blip, 1.0f);
                        API.shared.setBlipSprite(blip, 52);
                        API.shared.setBlipColor(blip, 4);
                        newBuilding.MapBlip = blip;
                        newBuilding.Label = API.shared.createTextLabel(robberyName + "~n~Level " + robberyLevel, pos, 20.0f, 0.5f);
                        BuildingVar[i] = newBuilding;
                        isFinished = true;
                        API.shared.consoleOutput("[BUILDINGS] A new building has been created by {0} with Building ID {1}", player.name, i);
                        API.shared.sendChatMessageToPlayer(player, String.Format("You've created a new building with Building ID {0}", i));
                        break;
                    }
                }
                else
                {
                    var blip = API.shared.createBlip(pos);
                    API.shared.setBlipName(blip, robberyName);
                    API.shared.setBlipScale(blip, 1.0f);
                    API.shared.setBlipSprite(blip, 52);
                    API.shared.setBlipColor(blip, 4);
                    newBuilding.MapBlip = blip;
                    newBuilding.Label = API.shared.createTextLabel(robberyName + "~n~Level " + robberyLevel, pos, 20.0f, 0.5f);
                    BuildingVar.Add(i, newBuilding);
                    isFinished = true;
                    API.shared.consoleOutput("[BUILDINGS] A new building has been created by {0} with Building ID {1}", player.name, i);
                    API.shared.sendChatMessageToPlayer(player, String.Format("You've created a new building with Building ID {0}", i));
                    break;
                }
                i++;
                if(i > 500)
                {
                    API.shared.consoleOutput("[VEHICLE] Something went wrong in trySpawnNewBuildingForPlayer");
                    isFinished = true;
                    break;
                }
            }
        }
    }
}
