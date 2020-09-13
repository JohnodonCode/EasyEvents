using EasyEvents.Commands;

namespace EasyEvents.Types
{
    public class TeleportData
    {
        public Door door;
        public RoleInfo role;

        public TeleportData(Door d, RoleInfo r)
        {
            door = d;
            role = r;
        }
    }
}