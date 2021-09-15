using Exiled.API.Features;
using System.Collections.Generic;

namespace EasyEvents.Types
{
    public class DoorData
    {
        public List<Door> doors;
        public string action;
        public int i;

        public DoorData(List<Door> doors, string action, int i)
        {
            this.doors = doors;
            this.action = action.ToLower();
            this.i = i;
        }
    }
}