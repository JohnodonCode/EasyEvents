using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Exiled.Events.EventArgs;
using EasyEvents.Types;
using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace EasyEvents
{
    public static class ScriptActions
    {
        public static bool eventRan = false;
        
        private static List<SpawnData> classIds = null;
        private static RoleInfo finalClass = null;

        private static List<TeleportData> teleportIds = null;

        public static bool detonate = false;

        public static List<RoleInfo> clearItems = new List<RoleInfo>();
        
        public static List<GiveData> giveData = new List<GiveData>();

        public static List<InfectData> infectData = new List<InfectData>();
        
        public static void SetCustomSpawn(List<SpawnData> _classIds, RoleInfo _finalClass, int line)
        {
            if (classIds != null) throw new CommandErrorException("Error running command \"spawn\" at line "+line+": Custom spawns have already been set. Only run the \"spawn\" command once.");

            classIds = _classIds;
            finalClass = _finalClass;
        }

        public static void SetTeleport(List<TeleportData> _teleportIds, int line)
        {
            if (teleportIds != null) throw new CommandErrorException("Error running command \"teleport\" at line "+line+": A teleport rule has already been set. Only run the \"teleport\" command once.");

            teleportIds = _teleportIds;
        }

        public static void AddEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Player.Died += OnKill;
        }

        public static void RemoveEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Player.Died -= OnKill;
            classIds = null;
            teleportIds = null;
            detonate = false;
            eventRan = false;
            CustomRoles.roles = new Dictionary<string, CustomRole>();
            CustomRoles.users = new Dictionary<string, CustomRole>();
            finalClass = null;
            clearItems = new List<RoleInfo>();
            giveData = new List<GiveData>();
            infectData = new List<InfectData>();
        }
        
        private static void OnRoundStarted()
        { 
            Timing.RunCoroutine(RoundStart());
        }

        private static void OnKill(DiedEventArgs ev)
        {
            if (ev.Killer == null) return;
            
            Timing.RunCoroutine(Infect(ev));
        }

        private static IEnumerator<float> Infect(DiedEventArgs ev)
        {
            yield return Timing.WaitForSeconds(1f);

            foreach (var data in infectData)
            {
                if (!data.killedBy.Equals(ev.Killer.GetRole())) continue;

                Vector3 oldPos = ev.Target.Position;
                    
                ev.Target.SetRole(data.newRole.GetRole());

                if (data.newRole.role != null)
                {
                    CustomRoles.users[ev.Target.UserId] = data.newRole.role;
                }

                yield return Timing.WaitForSeconds(1f);
                    
                if(data.soft) ev.Target.GameObject.GetComponent<PlayerMovementSync>().OverridePosition(oldPos, 0f, false);
                else
                {
                    foreach (var teleportdata in teleportIds)
                    {
                        if (!teleportdata.role.Equals(data.newRole)) continue;

                        if (!PlayerMovementSync.FindSafePosition(teleportdata.door.transform.position, out var pos))
                        {
                            throw new EventRunErrorException("No safe position could be found for door \""+teleportdata.door.DoorName+"\".");
                        }
                        
                        ev.Target.GameObject.GetComponent<PlayerMovementSync>().OverridePosition(pos, 0f, false);
                    }
                }

                foreach (var clearItemsData in clearItems)
                {
                    if (!clearItemsData.Equals(data.newRole)) continue;
                        
                    ev.Target.ClearInventory();
                }
                    
                foreach (var itemData in giveData)
                {
                    if (!itemData.role.Equals(data.newRole)) continue;

                    ev.Target.Inventory.AddNewItem(itemData.item);
                }
            }
        }
        
        private static IEnumerator<float> RoundStart()
        {
            yield return Timing.WaitForSeconds(1f);
            
            if (classIds != null)
            {
                SetRoles();
                yield return Timing.WaitForSeconds(1f);
            }
            
            ClearItems();
            GiveItems();

            if (teleportIds != null)
            {
                Teleport();
            }

            if (detonate)
            {
                AlphaWarheadController.Host.StartDetonation();
            }
        }

        private static void SetRoles()
        {
            var players = Player.List.ToList();
            players.Shuffle();
            
            foreach (var data in classIds)
            {

                for (var i = 0; i < players.Count; i++)
                {
                    if ((i != 0) && (i * data.chance / 100) <= ((i - 1) * data.chance / 100)) continue;
                    
                    players[i].SetRole((RoleType) data.role.classId);
                    if (data.role.role != null)
                    {
                        data.role.role.members.Add(players[i]);
                        CustomRoles.users[players[i].UserId] = data.role.role;
                    }
                    players.RemoveAt(i);
                }
                
                players.Shuffle();
            }

            if (players.Count > 0 && finalClass.classId != -1)
            {
                var role = (RoleType) finalClass.classId;
                
                foreach (var player in players)
                {
                    player.SetRole(role);
                }

                if (finalClass.role != null)
                {
                    finalClass.role.members = players;
                }
            }
        }

        private static void Teleport()
        {
            foreach (var data in teleportIds)
            {
                if (!PlayerMovementSync.FindSafePosition(data.door.transform.position, out var pos))
                {
                    throw new EventRunErrorException("No safe position could be found for door \""+data.door.DoorName+"\".");
                }
                
                var players = data.role.GetMembers();
                
                foreach (var player in players)
                {
                    player.GameObject.GetComponent<PlayerMovementSync>().OverridePosition(pos, 0f, false);
                }
            }
        }

        private static void ClearItems()
        {
            foreach (var clearItemsData in clearItems)
            {
                var list = clearItemsData.GetMembers();

                foreach (var player in list)
                {
                    player.ClearInventory();
                }
            }
        }

        private static void GiveItems()
        {
            foreach (var itemData in giveData)
            {
                var list = itemData.role.GetMembers();

                foreach (var player in list)
                {
                    player.Inventory.AddNewItem(itemData.item);
                }
            }
        }
    }
}