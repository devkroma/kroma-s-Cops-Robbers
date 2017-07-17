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
        public void commandSpawnCar(Client player, string modelname, int colour1, int colour2)
        {
            if(getAdminLevel(player) >= 3)
            {
                if(kroma_cnr.Vehicle.vehicle.vehicleNames.Contains(modelname))
                {
                    API.sendChatMessageToPlayer(player, "~#00cb00~", "Correct vehicle name.");
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
    }
}
