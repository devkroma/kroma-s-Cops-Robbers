using GTANetworkServer;
using System;

namespace kroma_cnr.Admin
{
    class admincommands : Script
    {
        public static void Main()
        {
            API.shared.consoleOutput("[ADMINCOMMANDS] Admin commands loaded.");
        }

        [Command("kickplayer", Alias = "pkick", GreedyArg = true)]
        public void commandKick(Client player, int targetid, string reason)
        {
            int playerid = kroma_cnr.Player.playerid.GetPlayerId(player);
            if(kroma_cnr.Player.playerid.PlayerAccount[playerid].AdminLevel >= 2)
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
    }
}
