using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;

namespace EasyEvents
{
    public class RoleInfo
    {
        public CustomRole role;
        public int classId;

        public RoleInfo(CustomRole role, int classId)
        {
            this.role = role;
            this.classId = classId;
        }

        public static RoleInfo parseRole(string arg, string cmd, int line, int argNum)
        {
            var classId = -1;
            CustomRole role = null;

            if (arg.Trim().ToLower().StartsWith("g:") && CustomRoles.roles.ContainsKey(arg.Trim().ToLower()))
            {
                if(!CustomRoles.roles.TryGetValue(arg.Trim().ToLower(), out role)) throw new InvalidArgumentException("Invalid argument for command \""+cmd+"\" on line "+line+", argument "+argNum+". Expected \"(0-17)\" but got \""+arg+"\".");
                classId = role.classId;
            }
            else
            {
                if(!Enum.TryParse<RoleType>(arg.Trim(), true, out var roleId)) throw new InvalidArgumentException("Invalid argument for command \""+cmd+"\" on line "+line+", argument "+argNum+". Expected \"(0-17)\" but got \""+arg+"\".");
                classId = (int) roleId;
            }
            
            return new RoleInfo(role, classId);
        }

        public List<Player> GetMembers()
        {
            return this.role == null ? Player.List.Where(player => player.Role == (RoleType) this.classId).ToList() : this.role.members;
        }
    }
}