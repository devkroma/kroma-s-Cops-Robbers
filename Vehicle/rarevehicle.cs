using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace kroma_cnr.Vehicle
{
    class rarevehicle : Script
    {
        private static int playersRequired = 1; // Players required for rare vehicle to spawn
        private static int randomChance = 2; // Higher number = less chance (e.g. 1000 = 1 in 1000 chance)
        private static int timerDuration = 60000; // Timer value in ms
        public static bool pickedRandomPlayer = false;
        public static Client randomRareVehiclePlayer;

        private static Timer rareVehicleTimer = new Timer();

        public static Vector3 vehicleGaragePos = new Vector3(952.3259, -1548.246, 30.33348);
        public static Vector3 vehicleSpawnPos = new Vector3(3596.973, 3661.665, 33.87174);
        public static Vector3 vehicleSpawnRot = new Vector3(0.0, 0.0, 84.27094);

        public static Blip rareVehicleSpawnBlip;
        public static Blip rareVehicleGarageBlip;
        public static NetHandle rareVehicleGarageMarker;
        public static NetHandle rareVehicle;

        public static List<string> rareVehicles = new List<string>();

        public static void Main()
        {
            rareVehicleTimer.Elapsed += new ElapsedEventHandler(onRareVehicleTimer);
            rareVehicleTimer.Interval = timerDuration;
            rareVehicleTimer.Enabled = true;
            API.shared.consoleOutput("[RAREVEHICLE] Rare Vehicle timer started.");
            loadRareVehiclesToList();
            API.shared.consoleOutput("[RAREVEHICLE] Rare Vehicle list loaded.");
        }

        private static void loadRareVehiclesToList()
        {
            rareVehicles = File.ReadAllLines("resources/kroma-cnr/Vehicle/rareVehicles.txt").ToList();
        }

        private static int getRandomColour(int colour)
        {
            if(colour >= 995)
            {
                return 159;
            }
            else if(colour >= 990)
            {
                return 158;
            }
            else if(colour >= 950)
            {
                return 120;
            }
            else
            {
                return 111;
            }
        }

        private static void onRareVehicleTimer(object source, ElapsedEventArgs e)
        {
            if(!pickedRandomPlayer)
            {
                int pCount = 0;
                foreach(Client i in API.shared.getAllPlayers())
                {
                    if(kroma_cnr.Player.playerid.GetPlayerId(i) != -1)
                    {
                        pCount++;
                    }
                }
                if(pCount >= playersRequired)
                {
                    Random rnd = new Random();
                    int randomNumber = rnd.Next(randomChance);
                    if(randomNumber == 1)
                    {
                        List<Client> allPlayers = new List<Client>();
                        foreach(Client i in API.shared.getAllPlayers())
                        {
                            if(kroma_cnr.Player.playerid.GetPlayerId(i) != -1)
                            {
                                allPlayers.Add(i);
                            }
                        }
                        int randomPlayer = rnd.Next(allPlayers.Count);
                        int randomVehicle = rnd.Next(rareVehicles.Count);
                        int randomColour = rnd.Next(1000);
                        pickedRandomPlayer = true;
                        randomRareVehiclePlayer = allPlayers[randomPlayer];
                        rareVehicleSpawnBlip = API.shared.createBlip(vehicleSpawnPos);
                        //rareVehicleGarageBlip = API.shared.createBlip(vehicleGaragePos);
                        //rareVehicleGarageMarker = API.shared.createMarker(1, vehicleGaragePos, vehicleGaragePos, new Vector3(0.0, 0.0, 0.0), new Vector3(1.0, 1.0, 1.0), 255, 255, 0, 0);
                        API.shared.setBlipSprite(rareVehicleSpawnBlip, 488);
                        //API.shared.setBlipSprite(rareVehicleGarageBlip, 357);
                        API.shared.setBlipColor(rareVehicleSpawnBlip, 11);
                        //API.shared.setBlipColor(rareVehicleGarageBlip, 11);
                        API.shared.consoleOutput("[RAREVEHICLE] Spawning a {0} for {1}.", rareVehicles[randomVehicle], randomRareVehiclePlayer.name);
                        rareVehicle = API.shared.createVehicle(API.shared.vehicleNameToModel(rareVehicles[randomVehicle]), vehicleSpawnPos, vehicleSpawnRot, getRandomColour(randomColour), getRandomColour(randomColour));
                        API.shared.sendChatMessageToPlayer(randomRareVehiclePlayer, "~#ffd700~", "A rare vehicle has spawned for you! Head to the rare vehicle marker for the chance to own it.");
                        if(randomColour >= 950)
                        {
                            API.shared.sendChatMessageToPlayer(randomRareVehiclePlayer, "~#ffd700~", "Apparently, this vehicle has a nice paintjob on it too...");
                        }
                        API.shared.triggerClientEvent(randomRareVehiclePlayer, "createGarageMarker", vehicleGaragePos);
                        API.shared.triggerClientEvent(randomRareVehiclePlayer, "setRareVehicle", rareVehicle);
                        foreach(Client i in API.shared.getAllPlayers())
                        {
                            if(i != randomRareVehiclePlayer)
                            {
                                API.shared.sendChatMessageToPlayer(i, "~#ff5700~", String.Format("{0} is on their way to try and find a rare vehicle! Stop them doing so and you'll win some money.", randomRareVehiclePlayer.name));
                            }
                        }
                    }
                }
            }
        }

        public static void onPlayerEnterRareVehicle(Client player)
        {
            API.shared.sendNotificationToAll(String.Format("{0} has found the rare vehicle! Stop them from reaching the garage.", player.name));
            API.shared.triggerClientEvent(player, "createGarageWaypoint");
        }

        public static void onRareVehicleDelivered(Client player)
        {
            API.shared.sendNotificationToAll(String.Format("{0} has delivered the rare vehicle successfully!", player.name));
            API.shared.sendChatMessageToPlayer(player, "~#ffd700~", "Well done on delivering the vehicle! It's now yours to keep.");
            pickedRandomPlayer = false;
            randomRareVehiclePlayer = null;
            API.shared.deleteEntity(rareVehicleSpawnBlip.handle);
        }

        [Command("delivervehicle", Alias = "dv")]
        public void commandDeliverVehicle(Client player)
        {
            if(randomRareVehiclePlayer == player)
            {
                if(API.getPlayerVehicle(player) == rareVehicle)
                {
                    if(player.position.DistanceTo(vehicleGaragePos) <= 10.0f)
                    {
                        onRareVehicleDelivered(player);
                        API.triggerClientEvent(player, "onRareVehicleDelivered");
                    }
                    else
                    {
                        API.sendChatMessageToPlayer(player, "~#808080~", "You aren't close enough to the garage.");
                    }
                }
                else
                {
                    API.sendChatMessageToPlayer(player, "~#808080~", "You aren't in a rare vehicle.");
                }
            }
            else
            {
                API.sendChatMessageToPlayer(player, "~#808080~", "You aren't delivering a rare vehicle.");
            }
        }
    }
}
