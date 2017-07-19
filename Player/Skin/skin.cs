using GTANetworkServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace kroma_cnr.Player.Skin
{
    class skin : Script
    {
        public static List<string> policeSkins;

        public static List<string> criminalSkins;

        public static void Main()
        {
            loadPoliceSkinsToList();
            loadCriminalSkinsToList();
            API.shared.consoleOutput("[SKIN] Skins loaded.");
        }

        public static void loadPoliceSkinsToList()
        {
            policeSkins = File.ReadAllLines("resources/kroma-cnr/Player/Skin/policeSkins.txt").ToList();
        }

        public static void loadCriminalSkinsToList()
        {
            criminalSkins = File.ReadAllLines("resources/kroma-cnr/Player/Skin/criminalSkins.txt").ToList();
        }

        public static void showPlayerSkinSelection(Client player)
        {
            var stringPolice = String.Join(",", policeSkins);
            var stringCriminal = String.Join(",", criminalSkins);
            string stringSkins = stringPolice + "," + stringCriminal;
            API.shared.consoleOutput(stringSkins);
            API.shared.triggerClientEvent(player, "showPlayerSkinSelection", stringSkins);
        }

        public static void setPlayerSpawnSkin(Client player, string skin)
        {
            API.shared.setEntityDimension(player.handle, 0);
            API.shared.setPlayerSkin(player, API.shared.pedNameToModel(skin));
            API.shared.freezePlayer(player, false);
            if(policeSkins.Contains(skin))
            {
                API.shared.setEntityPosition(player.handle, kroma_cnr.Main.spawnPosPolice);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Nightstick", 1);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Flashlight", 1);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Pistol", 240);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Stungun", 1);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "CombatPDW", 600);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Pumpshotgun", 160);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Parachute", 1);
            }
            else
            {
                API.shared.setEntityPosition(player.handle, kroma_cnr.Main.spawnPosCriminal);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Knife", 1);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Knuckleduster", 1);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Pistol", 240);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Flaregun", 10);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "MicroSMG", 320);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Sawnoffshotgun", 160);
                kroma_cnr.Admin.Anticheat.weapon.giveWeaponToPlayer(player, "Parachute", 1);
            }
        }
    }
}
