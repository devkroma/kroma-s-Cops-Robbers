using GTANetworkServer;
using MongoDB.Bson;
using System.Collections.Generic;

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
            API.shared.freezePlayer(player, false);
            API.shared.setEntityPosition(player.handle, kroma_cnr.Main.spawnPos);
            kroma_cnr.Player.Camera.LoginCamera.resetPlayerLoginCamera(player);
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
