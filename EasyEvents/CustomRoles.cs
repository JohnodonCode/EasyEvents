using System.Collections.Generic;
using EasyEvents.Integration;
using Exiled.API.Features;

namespace EasyEvents
{
    public static class CustomRoles
    {
        public static Dictionary<string, CustomRole> roles = new Dictionary<string, CustomRole>();
        public static Dictionary<string, string> users = new Dictionary<string, string>();

        public static CustomRole GetRole(Player p)
        {
            if(!users.TryGetValue(p.UserId, out var roleID)) throw new CommandErrorException("Error getting role for user \""+p.UserId+"\".");
            if(!roles.TryGetValue(roleID, out var role)) throw new CommandErrorException("Error getting role for user \""+p.UserId+"\".");
            return role;
        }
        
        public static void ChangeRole(Player p, CustomRole newRole)
        {
            if (p == null) return;
            if (newRole == null || !roles.ContainsKey(newRole.id))
            {
                if (users.ContainsKey(p.UserId)) users.Remove(p.UserId);
                return;
            }
            
            AdvancedSubclassing.RemovePlayer(p);
            
            if (!users.ContainsKey(p.UserId))
            {
                users[p.UserId] = newRole.id;
                roles[newRole.id].members.Add(p.UserId);
            }
            else
            {
                roles[users[p.UserId]].members.Remove(p.UserId);
                
                users[p.UserId] = newRole.id;
                roles[newRole.id].members.Add(p.UserId);
            }

            if (newRole.isSubclass)
            {
                AdvancedSubclassing.SetClass(p, newRole.id.Substring(2));
            }
        }
    }
}