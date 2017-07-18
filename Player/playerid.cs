using GTANetworkServer;
using kroma_cnr.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace kroma_cnr.Player
{
    class playerid : Script
    {
        public static Dictionary<int, PlayerAccount> PlayerAccount = new Dictionary<int, PlayerAccount>();

        public static void Main()
        {
            API.shared.consoleOutput("[PLAYERID] Player ID system loaded.");
            for(int i = 0; i < kroma_cnr.Main.maxPlayers; i++)
            {
                PlayerAccount.Add(i, null);
            }
        }

        public static void onPlayerLoadData(Client player, PlayerAccount data)
        {
            AddToPlayerAccount(player, data);
        }

        public static void PlayeridOnPlayerDisconnected(Client player, string reason)
        {
            RemoveFromPlayerAccount(player);
        }

        public static void AddToPlayerAccount(Client player, PlayerAccount data)
        {
            bool isFree = false;
            foreach(KeyValuePair<int, PlayerAccount> pair in PlayerAccount)
            {
                if(pair.Value == null)
                {
                    PlayerAccount[pair.Key] = data;
                    isFree = true;
                    API.shared.consoleOutput("[PLAYERID] {0} has logged in and been assigned playerid {1}", player.name, pair.Key);
                    Login.SpawnPlayer(player);
                    break;
                }
            }
            if(!isFree)
            {
                API.shared.sendChatMessageToPlayer(player, "There weren't any free dictionaries to add you to.");
            }
        }

        public static void RemoveFromPlayerAccount(Client player)
        {
            foreach(var key in PlayerAccount.Keys.ToList())
            {
                if(PlayerAccount[key] == null) continue;
                else if(PlayerAccount[key].Name == player.name)
                {
                    PlayerAccount[key] = null;
                    API.shared.consoleOutput("[PLAYERID] {0} has disconnected and been removed from playerid {1}", player.name, key);
                }
            }
        }

        public static int GetPlayerId(Client player)
        {
            foreach(var key in PlayerAccount.Keys.ToList())
            {
                if(PlayerAccount[key] == null) continue;
                else if(PlayerAccount[key].Name == player.name)
                {
                    return key;
                }
            }
            return -1;
        }

        public static Client GetClientFromPlayerId(int id)
        {
            Client c = null;
            foreach(Client client in API.shared.getAllPlayers())
            {
                if(GetPlayerId(client) == id)
                {
                    c = client;
                    break;
                }
            }
            return c;
        }

        [Command("myid")]
        public void commandMyId(Client player)
        {
            API.sendChatMessageToPlayer(player, String.Format("Your playerid is {0}.", GetPlayerId(player)));
        }

        [Command("mydb")]
        public void commandMyDb(Client player)
        {
            
            API.sendChatMessageToPlayer(player, String.Format("Your database id is {0}.", PlayerAccount[GetPlayerId(player)].Id));
        }
    }
}
