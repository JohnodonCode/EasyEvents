using EasyEvents.Commands;

namespace EasyEvents.Types
{
    public class TeleportData
    {
        public int classId = -1;
        public Door door = null;

        public TeleportData(int c, Door d)
        {
            classId = c;
            door = d;
        }
    }
}