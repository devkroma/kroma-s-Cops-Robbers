using GTANetworkServer;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace kroma_cnr.Admin.Anticheat
{
    class weapon : Script
    {
        public static List<string> weaponNames;

        public static void Main()
        {
            loadWeaponNamesToList();
            API.shared.consoleOutput("[WEAPON] Weapon Anticheat loaded.");
        }

        public static void loadWeaponNamesToList()
        {
            weaponNames = File.ReadAllLines("resources/kroma-cnr/Admin/Anticheat/weaponNames.txt").ToList();
        }

        public static void giveWeaponToPlayer(Client player, string weaponName, int ammo)
        {
            int playerid = kroma_cnr.Player.playerid.GetPlayerId(player);
            if(playerid != -1)
            {
                if(!kroma_cnr.Player.playerid.PlayerAccount[playerid].Weapons.Contains(weaponName))
                {
                    kroma_cnr.Player.playerid.PlayerAccount[playerid].Weapons.Add(weaponName);
                }
                API.shared.givePlayerWeapon(player, API.shared.weaponNameToModel(weaponName), ammo, false, true);
            }
        }
    }
}
