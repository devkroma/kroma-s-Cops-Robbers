using GTANetworkServer;

namespace kroma_cnr.Player.Camera
{
    class LoginCamera : Script
    {
        public static void setPlayerLoginCamera(Client player)
        {
            API.shared.triggerClientEvent(player, "setPlayerLoginCamera");
        }

        public static void resetPlayerLoginCamera(Client player)
        {
            API.shared.triggerClientEvent(player, "resetPlayerLoginCamera");
        }
    }
}