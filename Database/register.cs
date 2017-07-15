using GTANetworkServer;
using MongoDB.Bson;
using System;
using System.Security.Cryptography;
using System.Collections;

namespace kroma_cnr.Database
{
    class SHA
    {
        public static SHA256Managed sha = new SHA256Managed();
    }
    class Register : Script
    {
        public static void AccountAlreadyRegistered(Client player)
        {
            API.shared.sendChatMessageToPlayer(player, "This account has already been registered. Try using /login [password] instead.");
        }

        public static async void RegisterAccountAsync(Client player, string password)
        {
            API.shared.sendChatMessageToPlayer(player, "Hold on, we're registering your account now...");
            string hashedPassword = GetStringSha256Hash(password);
            var document = new BsonDocument
            {
                { "Name", player.name },
                { "Password", hashedPassword }
            };
            var collection = DatabaseClass.CnRDatabase.GetCollection<BsonDocument>("accounts");
            await collection.InsertOneAsync(document);
            API.shared.sendChatMessageToPlayer(player, "Account registered! You can now log in!");
        }

        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (SHA.sha)
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = SHA.sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        [Command("register", SensitiveInfo = true, GreedyArg = true)]
        public void commandRegister(Client player, string password)
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
