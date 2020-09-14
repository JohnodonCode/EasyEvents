using System;
 using System.Collections.Generic;
using Exiled.API.Features;

namespace EasyEvents
{
    public static class Util
    {
        private static readonly Random Rng = new Random();  

        public static void Shuffle<T>(this IList<T> list)  
        {  
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = Rng.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }

        public static RoleInfo GetRole(this Player p)
        {
            if (CustomRoles.users.ContainsKey(p.UserId))
            {
                if(!CustomRoles.users.TryGetValue(p.UserId, out var role)) throw new CommandErrorException("Error getting role for user \""+p.UserId+"\".");
                return new RoleInfo(role, role.classId);
            }

            return new RoleInfo(null, (int) p.Role);
        }
    }
}