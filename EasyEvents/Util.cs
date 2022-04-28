using System;
 using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;

namespace EasyEvents
{
    public static class Util
    {
        private static readonly Random Rng = new Random();  

        public static List<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public static RoleInfo GetRole(this Player p)
        {
            if (CustomRoles.users.ContainsKey(p.UserId))
            {
                var role = CustomRoles.GetRole(p);
                return new RoleInfo(role.id, role.classId);
            }

            return new RoleInfo(null, (int) p.Role.Type);
        }
        
        public static void pop<T>(this List<T> list)
        {
            if (list.Any())
            {
                list.RemoveAt(list.Count - 1);
            }
        }
        
        public static List<T> Clone<T>(this IEnumerable<T> listToClone)
        {
            return listToClone.Select(item => item).ToList();
        }
        
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static List<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count).ToList();
        }
    }
}