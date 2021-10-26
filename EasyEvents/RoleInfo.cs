using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;

namespace EasyEvents
{
    public class RoleInfo
    {
        public string roleID;
        public int classId;

        public RoleInfo(string roleID, int classId)
        {
            this.roleID = roleID;
            this.classId = classId;
        }

        public CustomRole GetCustomRole()
        {
            if (roleID == null) return null;
            if (!CustomRoles.roles.ContainsKey(roleID)) return null;
            if (!CustomRoles.roles.TryGetValue(roleID, out var role)) return null;
            return role;
        }

        public static RoleInfo parseRole(string arg, string cmd, int line, int argNum)
        {
            var classId = -1;
            CustomRole role = null;

            if (arg.Trim().ToLower().StartsWith("g:") && CustomRoles.roles.ContainsKey(arg.Trim().ToLower()))
            {
                if(!CustomRoles.roles.TryGetValue(arg.Trim().ToLower(), out role)) throw new InvalidArgumentException("Invalid argument for command \""+cmd+"\" on line "+line+", argument "+argNum+". Expected \"g:classname\" but got \""+arg+"\".");
                classId = role.classId;
            }
            else if (arg.Trim().ToLower() == "all")
            {
                if(!CustomRoles.roles.TryGetValue("all", out role)) throw new InvalidArgumentException("Invalid argument for command \""+cmd+"\" on line "+line+", argument "+argNum+". You really shouldn't be getting this error, and if you are, RUN.");
                classId = role.classId;
            }
            else
            {
                if(!Enum.TryParse<RoleType>(arg.Trim(), true, out var roleId)) throw new InvalidArgumentException("Invalid argument for command \""+cmd+"\" on line "+line+", argument "+argNum+". Expected \"(0-17)\" but got \""+arg+"\".");
                classId = (int) roleId;
            }
            return new RoleInfo(role?.id, classId);
        }

        public List<Player> GetMembers()
        {
            return this.GetCustomRole() == null ? Player.List.Where(player => player.Role == (RoleType) this.classId).ToList() : this.GetCustomRole().GetMembers();
        }

        public bool Equals(RoleInfo other)
        {
            if (this.classId == -1 || other.classId == -1)
            {
                Log.Error("A role's classId is -1. This should never be the case and will cause issues.");
                return false;
            }
            if (this.roleID == "all" || other.roleID == "all") return true;
            if (this.classId != other.classId) return false;
            if (this.GetCustomRole() == null || other.GetCustomRole() == null) return true;
            return this.roleID == other.roleID;
        }

        public RoleType GetRole()
        {
            return (RoleType) this.classId;
        }
        
        public static RoleInfo all
        {
            get => new RoleInfo("all", 2);
            private set {}
        }
    }
}