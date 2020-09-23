using System.Collections.Generic;
using Exiled.API.Features;
using System.Linq;
using EasyEvents.Integration;

namespace EasyEvents
{
    public class CustomRole
    {
        public List<string> members = new List<string>();
        public string id = null;
        public int classId = -1;
        public bool isSubclass = false;
        
        public CustomRole(string id, int classId, bool isSubclass = false)
        {
            this.id = id;
            this.classId = classId;
            this.isSubclass = isSubclass;
        }

        public List<Player> GetMembers()
        {
            return this.id == "all" ? Player.List.ToList() : this.isSubclass ? AdvancedSubclassing.GetPlayers(id.Substring(2)) : Player.List.Where(p => members.Contains(p.UserId)).ToList();
        }
    }
}