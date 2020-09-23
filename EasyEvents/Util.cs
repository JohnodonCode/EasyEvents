using System;
 using System.Collections.Generic;
using System.Linq;
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
                var role = CustomRoles.GetRole(p);
                return new RoleInfo(role.id, role.classId);
            }

            return new RoleInfo(null, (int) p.Role);
        }
        
        public static void pop<T>(this List<T> list)
        {
            if (list.Any())
            {
                list.RemoveAt(list.Count - 1);
            }
        }
        
        public static List<T> Clone<T>(this List<T> listToClone)
        {
            return listToClone.Select(item => item).ToList();
        }
    }
}