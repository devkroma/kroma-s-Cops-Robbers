using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Linq;

namespace kroma_cnr.Admin
{
    class admincommands : Script
    {
        public static void Main()
        {
            API.shared.consoleOutput("[ADMINCOMMANDS] Admin commands loaded.");
        }

        public static int getAdminLevel(Client player)
        {
            int playerid = kroma_cnr.Player.playerid.GetPlayerId(player);
            return kroma_cnr.Player.playerid.PlayerAccount[playerid].AdminLevel;
        }

        [Command("kickplayer", Alias = "pkick", GreedyArg = true)]
        public void commandKick(Client player, int targetid, string reason)
        {
            int playerid = kroma_cnr.Player.playerid.GetPlayerId(player);
            if(getAdminLevel(player) >= 2)
            {
                if(playerid == targetid)
                {
                    API.sendChatMessageToPlayer(player, "~#808080~", "You can't kick yourself.");
                }
                else
                {
                    if(kroma_cnr.Player.playerid.PlayerAccount[targetid] == null)
                    {
                        API.sendChatMessageToPlayer(player, "~#808080~", "That player isn't logged in.");
                    }
                    else
                    {
                        Client target = kroma_cnr.Player.playerid.GetClientFromPlayerId(targetid);
                        API.kickPlayer(target);
                        API.sendChatMessageToAll("~#cd0000~", String.Format("[KICK] {0} has been kicked by {1} ({2}).", target.name, player.name, reason));
                    }
                }
            }
            else
            {
                API.sendChatMessageToPlayer(player, "~#808080~", "You must be admin level 2 to use this command.");
            }
        }

        [Command("admin", Alias = "a", GreedyArg = true)]
        public void commandAdmin(Client player, string message)
        {
            if(getAdminLevel(player) >= 1)
            {
                foreach(var key in kroma_cnr.Player.playerid.PlayerAccount.Keys.ToList())
                {
                    if(kroma_cnr.Player.playerid.PlayerAccount[key] == null) continue;
                    else if(kroma_cnr.Player.playerid.PlayerAccount[key].AdminLevel >= 1)
                    {
                        Client sendTo = kroma_cnr.Player.playerid.GetClientFromPlayerId(key);
                        API.sendChatMessageToPlayer(sendTo, "~#66CDAA~", String.Format("[A] {0}~w~: {1}", player.name, message));
                    }
                }
            }
            else
            {
                API.sendChatMessageToPlayer(player, "~#808080~", "You must be admin level 1 to use this command.");
            }
        }

        [Command("spawncar", Alias = "sc")]
        public void commandSpawnCar(Client player, string modelname, int colour1 = 131, int colour2 = 131)
        {
            if(getAdminLevel(player) >= 3)
            {
                if(kroma_cnr.Vehicle.vehicle.vehicleNames.Contains(modelname.ToLower()))
                {
                    if(colour1 >= 0 && colour1 <= 159 && colour2 >= 0 && colour2 <= 159)
                    {
                        VehicleHash vehicle = API.vehicleNameToModel(modelname);
                        Vector3 spawnPos = API.getEntityPosition(player.handle);
                        Vector3 spawnRot = API.getEntityRotation(player.handle);
                        int spawnDim = API.getEntityDimension(player.handle);
                        API.sendChatMessageToPlayer(player, String.Format("Your {0} is attempting to spawn.", modelname));
                        kroma_cnr.Vehicle.vehicle.trySpawnNewVehicleForPlayer(player, vehicle, spawnPos, spawnRot, colour1, colour2, spawnDim);
                    }
                    else
                    {
                        API.sendChatMessageToPlayer(player, "~#808080~", "Valid vehicle colour IDs are between 0 and 159.");
                    }
                }
                else
                {
                    API.sendChatMessageToPlayer(player, "~#808080~", "You have selected an invalid vehicle name.");
                }
            }
            else
            {
                API.sendChatMessageToPlayer(player, "~#808080~", "You must be admin level 3 to use this command.");
            }
        }

        [Command("spawnweapon", Alias = "sw")]
        public void commandSpawnWeapon(Client player, string weaponname, int ammo)
        {
            if(getAdminLevel(player) >= 3)
            {
                if(ammo > 1 && ammo <= 999999)
                {
                    if(kroma_cnr.Admin.Anticheat.weapon.weaponNames.Contains(weaponname.ToLower()))
                    {
                        kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, weaponname, ammo);
                    }
                    else
                    {
                        API.sendChatMessageToPlayer(player, "~#808080~", "You have selected an invalid weapon name.");
                    }
                }
                else
                {
                    API.sendChatMessageToPlayer(player, "~#808080~", "You can only give yourself between 1 and 999999 ammo.");
                }
            }
            else
            {
                API.sendChatMessageToPlayer(player, "~#808080~", "You must be admin level 3 to use this command.");
            }
        }

        [Command("reloadvehiclelist")]
        public void commandReloadVehicleList(Client player)
        {
            if(getAdminLevel(player) >= 8)
            {
                kroma_cnr.Vehicle.vehicle.loadVehicleNamesToList();
                API.sendChatMessageToPlayer(player, "Vehicle list has been updated.");
            }
            else
            {
                API.sendChatMessageToPlayer(player, "~#808080~", "You must be admin level 8 to use this command.");
            }
        }

        [Command("createrobbery", Alias = "cr", GreedyArg = true)]
        public void commandCreateRobbery(Client player, int robberylevel, string buildingname)
        {
            if(getAdminLevel(player) >= 4)
            {
                if(robberylevel >= 0 && robberylevel <= 100)
                {
                    if(buildingname.Length > 3)
                    {
                        kroma_cnr.Database.buildings.trySpawnNewRobberyForPlayer(player, robberylevel, buildingname);
                    }
                    else
                    {
                        API.sendChatMessageToPlayer(player, "~#808080~", "You must choose a building name with over 3 characters.");
                    }
                }
                else
                {
                    API.sendChatMessageToPlayer(player, "~#808080~", "You must choose a robbery level between 0 and 100.");
                }
            }
            else
            {
                API.sendChatMessageToPlayer(player, "~#808080~", "You must be admin level 4 to use this command.");
            }
        }
    }
}
