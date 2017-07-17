/* kroma's Cops & Robbers
 * https://github.com/devkroma/kroma-s-Cops-Robbers
 * Use this without permission idk mate it's not really all that
 * Creed is a cool guy */

using GTANetworkServer;
using GTANetworkShared;
using System;

namespace kroma_cnr
{
    public class Main : Script
    {
        public static int maxPlayers = 50;
        private Vector3 upInAir = new Vector3(350.0, 200.0, 170.0);

        public Main()
        {
            kroma_cnr.Database.Mongo.Main();
            kroma_cnr.Player.playerid.Main();
            kroma_cnr.Admin.admincommands.Main();
            API.onResourceStart += MainOnResourceStart;
            API.onPlayerConnected += MainOnPlayerConnected;
            API.onPlayerDisconnected += MainOnPlayerDisconnected;
            API.onPlayerFinishedDownload += MainOnPlayerFinishedDownload;
        }

        public void MainOnResourceStart()
        {
            API.consoleOutput("[KROMA-CNR] kroma's Cops & Robbers is starting.");
        }

        private void MainOnPlayerConnected(Client player)
        {
            API.consoleOutput("[KROMA-CNR] {0} has connected to the server.", player.name);
            API.sendChatMessageToAll("~#00cd00~", String.Format("[JOIN] {0} has connected to the server.", player.name));
            kroma_cnr.Database.Mongo.CheckAccountExistsAsync(player);
        }

        public static void OnAccountNotRegistered(Client player)
        {
            API.shared.sendChatMessageToPlayer(player, "This account isn't registered! Use /register [password] to register it.");
        }

        public static void OnAccountRegistered(Client player)
        {
            API.shared.sendChatMessageToPlayer(player, "This account is already registered! Use /login [password] if you own this account.");
        }

        private void MainOnPlayerDisconnected(Client player, string reason)
        {
            API.sendChatMessageToAll("~#cd0000~", String.Format("[LEAVE] {0} has disconnected from the server ({1}).", player.name, reason));
            kroma_cnr.Player.playerid.PlayeridOnPlayerDisconnected(player, reason);
        }

        private void MainOnPlayerFinishedDownload(Client player)
        {
            API.setEntityPosition(player.handle, upInAir);
            API.freezePlayer(player, true);
            kroma_cnr.Player.Camera.LoginCamera.setPlayerLoginCamera(player);
        }
    }
}
