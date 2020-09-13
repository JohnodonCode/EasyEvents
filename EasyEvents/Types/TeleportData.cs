using EasyEvents.Commands;

namespace EasyEvents.Types
{
    public class TeleportData
    {
        public int classId = -1;
        public Door door = null;
        public CustomRole role = null;

        public TeleportData(int c, Door d, CustomRole r)
        {
            classId = c;
            door = d;
            role = r;
        }
    }
}