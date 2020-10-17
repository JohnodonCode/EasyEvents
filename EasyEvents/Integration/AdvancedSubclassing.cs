﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Loader;

namespace EasyEvents.Integration
{
    public static class AdvancedSubclassing
    {
        private static Type GetAPI()
        {
            try
            {
                return Loader.Plugins.FirstOrDefault(pl => pl.Name == "Subclass")?.Assembly.GetType("Subclass.API");
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Type GetSubclass()
        {
            try
            {
                return Loader.Plugins.FirstOrDefault(pl => pl.Name == "Subclass")?.Assembly.GetType("Subclass.SubClass");
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void PopulateCustomRoles()
        {
            var api = GetAPI();
            if (api == null) return;

            var subclasses = (Dictionary<string, object>) api.GetMethod("GetClasses")?.Invoke(null, null);
            if (subclasses == null || subclasses.Count < 1) return;

            var subclass = GetSubclass();
            if (subclass == null) return;
            
            foreach (var key in subclasses.Keys)
            {
                var role = subclass.GetField("SpawnsAs").GetValue(subclasses[key]);
                CustomRoles.roles.Add("g:"+key, new CustomRole("g:"+key, (int) role, true));
            }
        }

        public static List<Player> GetPlayers(string id)
        {
            var api = GetAPI();
            if (api == null) return new List<Player>();

            var subclasses = ((Dictionary<Player, object>) api.GetMethod("GetPlayersWithSubclasses")?.Invoke(null, null));
            if (subclasses == null || subclasses.Count < 1) return new List<Player>();
            
            var subclass = GetSubclass();
            if (subclass == null) return new List<Player>();

            return Player.List.Where(player =>
            {
                var val = subclasses.Keys.FirstOrDefault(p => p.Id == player.Id);
                if (val == null) return false;
                
                return (string) subclass.GetField("Name").GetValue(subclasses[val]) == id;
            }).ToList();
        }

        public static void RemovePlayer(Player p)
        {
            var api = GetAPI();
            if (api == null) return;

            api.GetMethod("RemoveClass")?.Invoke(null, new object[] {p});
        }

        public static void SetClass(Player p, string id)
        {
            var api = GetAPI();
            if (api == null) return;

            var classes = ((Dictionary<string, object>) api.GetMethod("GetClasses")?.Invoke(null, null));
            if (classes == null) return;
            if (!classes.ContainsKey(id)) return;
            
            api.GetMethod("GiveClass")?.Invoke(null, new object[] {p, classes[id]});
        }
    }
}