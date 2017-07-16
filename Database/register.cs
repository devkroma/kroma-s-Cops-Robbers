using GTANetworkServer;
using MongoDB.Bson;

namespace kroma_cnr.Database
{
    class Register : Script
    {
        public static void AccountAlreadyRegistered(Client player)
        {
            API.shared.sendChatMessageToPlayer(player, "This account has already been registered. Try using /login [password] instead.");
        }

        public static async void RegisterAccountAsync(Client player, string password)
        {
            API.shared.sendChatMessageToPlayer(player, "Hold on, we're registering your account now...");
            string hashedPassword = Mongo.GetStringSha256Hash(password);
            var document = new BsonDocument
            {
                { "Name", player.name },
                { "Password", hashedPassword }
            };
            var collection = DatabaseClass.CnRDatabase.GetCollection<BsonDocument>("accounts");
            await collection.InsertOneAsync(document);
            API.shared.sendChatMessageToPlayer(player, "Account registered! You can now log in!");
        }

        [Command("register", SensitiveInfo = true, GreedyArg = true)]
        public void commandRegister(Client player, string password)
        {
            if(API.getEntityData(player.handle, "LoggedIn") >= 1)
            {
                API.sendChatMessageToPlayer(player, "You're already logged in.");
            }
            else
            {
                if (password.Length >= 3)
                {
                    Mongo.CheckAccountExistsToRegister(player, password);
                }
                else
                {
                    API.shared.sendChatMessageToPlayer(player, "Your password must be at least 3 characters long!");
                }
            }
        }
    }
}
