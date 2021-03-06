﻿using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using GTANetworkServer;
using System;
using System.Security.Cryptography;
using kroma_cnr.Player;
using GTANetworkShared;
using System.Collections.Generic;

namespace kroma_cnr.Database
{
    public static class DatabaseClass
    {
        static public MongoClient CnRClient = new MongoClient("mongodb://localhost:27017");
        static public IMongoDatabase CnRDatabase { get; set; }
    }
    public static class SHA
    {
        public static SHA256Managed sha = new SHA256Managed();
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
            var document = await collection.Find(filter).FirstAsync();
            if (document.Name == player.name)
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
            var document = await collection.Find(filter).FirstAsync();
            if (document.Name == player.name)
            {
                Register.AccountAlreadyRegistered(player);
            }
            else
            {
                 Register.RegisterAccountAsync(player, password);
            }
        }

        public static async void CheckAccountExistsToLogin(Client player, string password)
        {
            var collection = DatabaseClass.CnRDatabase.GetCollection<PlayerAccount>("accounts");
            var filter = Builders<PlayerAccount>.Filter.Eq("Name", player.name);
            var document = await collection.Find(filter).FirstAsync();
            if (document.Name == player.name)
            {
                if(document.Password == password)
                {
                    Login.LoginAccount(player, password);
                    playerid.onPlayerLoadData(player, document);
                }
                else
                {
                    API.shared.sendChatMessageToPlayer(player, "You've entered the wrong password.");
                }
            }
            else
            {
                Login.AccountNotRegistered(player);
            }
        }

        public static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] hash = SHA.sha.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }
    }

    public class PlayerAccount
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int AdminLevel { get; set; }
        public List<string> Weapons { get; set; }
    }

    public class VehicleVar
    {
        public ObjectId Id { get; set; }
        public NetHandle VehicleHandle { get; set; }
        public VehicleHash Model { get; set; }
        public int Colour1 { get; set; }
        public int Colour2 { get; set; }
        public Vector3 SpawnPos { get; set; }
        public Vector3 SpawnRot { get; set; }
        public int SpawnDim { get; set; }
        public ObjectId OwnerId { get; set; }
    }

    public class BuildingVar
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public Vector3 RobberyPos { get; set; }
        public int RobberyLevel { get; set; }
        public int LastRobbed { get; set; }
        public Blip MapBlip { get; set; }
        public NetHandle Marker { get; set; } 
        public TextLabel Label { get; set; }
    }
}
