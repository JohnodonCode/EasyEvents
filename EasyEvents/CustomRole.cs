using System.Collections.Generic;
using Exiled.API.Features;

namespace EasyEvents
{
    public class CustomRole
    {
        public List<Player> members = new List<Player>();
        public string id = null;
        public int classId = -1;
        
        public CustomRole(string id, int classId)
        {
            this.id = id;
            this.classId = classId;
        }
    }
}