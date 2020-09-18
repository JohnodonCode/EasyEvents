using System.Collections.Generic;
using Exiled.API.Features;
using System.Linq;

namespace EasyEvents
{
    public class CustomRole
    {
        public List<string> members = new List<string>();
        public string id = null;
        public int classId = -1;
        
        public CustomRole(string id, int classId)
        {
            this.id = id;
            this.classId = classId;
        }

        public List<Player> GetMembers()
        {
            return this.id == "all" ? Player.List.ToList() : Player.List.Where(p => members.Contains(p.UserId)).ToList();
        }
    }
}