/* kroma's Cops & Robbers
 * https://github.com/devkroma/kroma-s-Cops-Robbers
 * Use this without permission idk mate it's not really all that
 * Creed is a cool guy */

using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Globalization;

namespace kroma_cnr
{
    public class Main : Script
    {
        public static int maxPlayers = 50;
        private Vector3 upInAir = new Vector3(350.0, 200.0, 170.0);
        public static Vector3 skinPos = new Vector3(464.0632, -990.294, 24.91485);
        public static Vector3 skinRot = new Vector3(0.0, 0.0, -9.998462);
        public static Vector3 spawnPosPolice = new Vector3(426.935, -980.9301, 30.71);
        public static Vector3 spawnPosCriminal = new Vector3(363.2693, -595.5392, 28.67362);

        public Main()
        {
            kroma_cnr.Database.Mongo.Main();
            kroma_cnr.Player.playerid.Main();
            kroma_cnr.Admin.admincommands.Main();
            kroma_cnr.Vehicle.vehicle.Main();
            kroma_cnr.Player.Skin.skin.Main();
            kroma_cnr.Admin.Anticheat.weapon.Main();
            API.onResourceStart += MainOnResourceStart;
            API.onPlayerConnected += MainOnPlayerConnected;
            API.onPlayerDisconnected += MainOnPlayerDisconnected;
            API.onPlayerFinishedDownload += MainOnPlayerFinishedDownload;
            API.onChatCommand += MainOnChatCommand;
            API.onClientEventTrigger += MainOnClientEventTrigger;
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

        public void MainOnChatCommand(Client player, string command, CancelEventArgs e)
        {
            if(kroma_cnr.Player.playerid.GetPlayerId(player) == -1)
            {
                if(!(command.ToLower().StartsWith("/register") || command.ToLower().StartsWith("/login")))
                {
                    API.sendChatMessageToPlayer(player, "~#808080~", "You must log in before using any commands.");
                    e.Cancel = true;
                }
            }
            else
            {
                if(command.ToLower().StartsWith("/register") || command.ToLower().StartsWith("/login"))
                {
                    API.sendChatMessageToPlayer(player, "~#808080~", "You can't use this command whilst logged in.");
                    e.Cancel = true;
                }
            }
        }

        public void MainOnClientEventTrigger(Client player, string eventName, params object[] arguments)
        {
            if(eventName == "setPlayerSpawnSkin")
            {
                kroma_cnr.Player.Skin.skin.setPlayerSpawnSkin(player, arguments[0].ToString());
            }
        }
    }
}
