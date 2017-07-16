using GTANetworkServer;
using GTANetworkShared;
using kroma_cnr.Database;
using System.Collections.Generic;

namespace kroma_cnr.Player
{
    class playerid : Script
    {
        public static Dictionary<NetHandle, PlayerAccount> PlayerAccount = new Dictionary<NetHandle, PlayerAccount>();

        public static void Main()
        {
            API.shared.consoleOutput("[PLAYERID] Player ID system loaded.");
        }

        public static void onPlayerLoadData(Client player, PlayerAccount data)
        {
            PlayerAccount.Add(player.handle, data);
            API.shared.consoleOutput("Player {0} has logged in and been added to PlayerAccount dictionary.", player.name);
        }

        public static void PlayeridOnPlayerDisconnected(Client player, string reason)
        {
            if(PlayerAccount.ContainsKey(player.handle))
            {
                PlayerAccount.Remove(player.handle);
                API.shared.consoleOutput("Player {0} has logged out and been removed from PlayerAccount dictionary.", player.name);
            }
        }
    }
}
