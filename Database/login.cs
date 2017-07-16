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

        [Command("login", SensitiveInfo = true, GreedyArg = true)]
        public void commandLogin(Client player, string password)
        {
            if(API.getEntityData(player.handle, "LoggedIn") >= 1)
            {
                API.sendChatMessageToPlayer(player, "You're already logged in.");
            }
            else
            {
                Mongo.CheckAccountExistsToLogin(player, password);
            }
        }
    }
}
