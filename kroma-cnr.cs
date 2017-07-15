using GTANetworkServer;
using System;

namespace kroma_cnr
{
    public class Main : Script
    {
        public Main()
        {
            kroma_cnr.Database.Mongo.Main();
            API.onResourceStart += MainOnResourceStart;
            API.onPlayerConnected += MainOnPlayerConnected;
        }

        public void MainOnResourceStart()
        {
            API.consoleOutput("[KROMA-CNR] kroma's Cops & Robbers is starting.");
        }

        private void MainOnPlayerConnected(Client player)
        {
            API.consoleOutput("[KROMA-CNR] {0} has connected to the server.", player.name);
            kroma_cnr.Database.Mongo.CheckAccountExistsAsync(player);
        }

        public static void OnAccountNotRegistered(Client player)
        {
            API.shared.sendChatMessageToPlayer(player, "This account isn't registered!");
        }

        public static void OnAccountRegistered(Client player)
        {
            API.shared.sendChatMessageToPlayer(player, "This account is already registered!");
        }
    }
}
