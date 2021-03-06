﻿using GTANetworkServer;

namespace kroma_cnr.Database
{
    class Login : Script
    {
        public static void AccountNotRegistered(Client player)
        {
            API.shared.sendChatMessageToPlayer(player, "This account has not been registered. Try using /register [password] instead.");
        }

        public static void LoginAccount(Client player, string password)
        {
            API.shared.sendChatMessageToPlayer(player, "You've logged in successfully!");
        }

        public static void SpawnPlayer(Client player)
        {
            API.shared.setEntityPosition(player.handle, kroma_cnr.Main.skinPos);
            API.shared.setEntityDimension(player.handle, kroma_cnr.Player.playerid.GetPlayerId(player) + 1);
            API.shared.setEntityRotation(player.handle, kroma_cnr.Main.skinRot);
            kroma_cnr.Player.Camera.LoginCamera.resetPlayerLoginCamera(player);
            kroma_cnr.Player.Skin.skin.showPlayerSkinSelection(player);
        }

        [Command("login", SensitiveInfo = true, GreedyArg = true)]
        public void commandLogin(Client player, string password)
        {
            if(API.getEntityData(player.handle, "LoggedIn") >= 1)
            {
                API.sendChatMessageToPlayer(player, "You're already logged in.");
            }
            else
            {
                string hashedPassword = Mongo.GetStringSha256Hash(password);
                Mongo.CheckAccountExistsToLogin(player, hashedPassword);
            }
        }
    }
}
