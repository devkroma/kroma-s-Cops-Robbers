using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using GTANetworkServer;

namespace kroma_cnr.Database
{
    public static class DatabaseClass
    {
        static public MongoClient CnRClient = new MongoClient("mongodb://localhost:27017");
        static public IMongoDatabase CnRDatabase { get; set; }
    }
    class Mongo
    {
        public static void Main()
        {
            DatabaseClass.CnRDatabase = DatabaseClass.CnRClient.GetDatabase("kroma-cnr");
            API.shared.consoleOutput("[MONGO] Starting database.");
        }
        public static async void CheckAccountExistsAsync(Client player)
        {
            var collection = DatabaseClass.CnRDatabase.GetCollection<PlayerAccount>("accounts");
            var filter = Builders<PlayerAccount>.Filter.Eq("Name", player.name);
            var result = await collection.Find(filter).ToListAsync();
            if (result.Count() >= 1)
            {
                kroma_cnr.Main.OnAccountRegistered(player);
            }
            else
            {
                 kroma_cnr.Main.OnAccountNotRegistered(player);
            }
        }

        public static async void CheckAccountExistsToRegister(Client player, string password)
        {
            var collection = DatabaseClass.CnRDatabase.GetCollection<PlayerAccount>("accounts");
            var filter = Builders<PlayerAccount>.Filter.Eq("Name", player.name);
            var result = await collection.Find(filter).ToListAsync();
            if (result.Count() >= 1)
            {
                Register.AccountAlreadyRegistered(player);
            }
            else
            {
                 Register.RegisterAccountAsync(player, password);
            }
        }

        public static void LoadAccount(string[] name)
        {

        }
    }

    public class PlayerAccount
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
