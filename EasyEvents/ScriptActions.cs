using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Exiled.Events.EventArgs;
using EasyEvents.Types;
using Exiled.API.Features;
using LightContainmentZoneDecontamination;
using MEC;
using UnityEngine;
using Random = System.Random;

namespace EasyEvents
{
    public static class ScriptActions
    {
        private static Random random = new Random();
        
        public static bool eventRan = false;
        
        private static List<SpawnData> classIds = null;
        private static RoleInfo finalClass = null;

        private static List<TeleportData> teleportIds = null;

        public static bool detonate = false;

        public static List<RoleInfo> clearItems = new List<RoleInfo>();
        
        public static List<GiveData> giveData = new List<GiveData>();

        public static List<InfectData> infectData = new List<InfectData>();
        
        public static List<HPData> hpData = new List<HPData>();
        
        public static List<SizeData> sizeData = new List<SizeData>();

        public static bool disableDecontamination = false;

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
            Exiled.Events.Handlers.Map.AnnouncingDecontamination += OnDeconText;
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
            CustomRoles.users = new Dictionary<string, string>();
            finalClass = null;
            clearItems = new List<RoleInfo>();
            giveData = new List<GiveData>();
            infectData = new List<InfectData>();
            hpData = new List<HPData>();
            sizeData = new List<SizeData>();
            disableDecontamination = false;
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

        private static void OnDeconText(AnnouncingDecontaminationEventArgs ev)
        {
            ev.IsAllowed = !disableDecontamination;
        }

        private static void OnDecon(DecontaminatingEventArgs ev)
        {
            ev.IsAllowed = !disableDecontamination;
        }

        private static IEnumerator<float> Infect(DiedEventArgs ev)
        {
            yield return Timing.WaitForSeconds(1f);

            foreach (var data in infectData)
            {
                if (!data.killedBy.Equals(ev.Killer.GetRole())) continue;

                Vector3 oldPos = ev.Target.Position;
                    
                ev.Target.SetRole(data.newRole.GetRole());

                CustomRoles.ChangeRole(ev.Target, data.newRole.GetCustomRole());

                yield return Timing.WaitForSeconds(1f);

                if (data.soft) ev.Target.Position = oldPos;
                else
                {
                    foreach (var teleportdata in teleportIds)
                    {
                        if (!teleportdata.role.Equals(data.newRole)) continue;

                        if (!PlayerMovementSync.FindSafePosition(teleportdata.door.transform.position, out var pos))
                        {
                            throw new EventRunErrorException("No safe position could be found for door \""+teleportdata.door.DoorName+"\".");
                        }

                        ev.Target.Position = pos;
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
                
                foreach (var sizedata in sizeData)
                {
                    if (!sizedata.role.Equals(data.newRole)) continue;
                    
                    ev.Target.Scale = sizedata.scale;
                }
                
                foreach (var healthData in hpData)
                {
                    if (!healthData.role.Equals(data.newRole)) continue;
                    
                    ev.Target.Health = healthData.amount;
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
            SetHP();
            SetSize();

            if (teleportIds != null)
            {
                Teleport();
            }

            if (detonate)
            {
                AlphaWarheadController.Host.StartDetonation();
            }

            if (disableDecontamination)
            {
                DecontaminationController.Singleton._disableDecontamination = true;
                DecontaminationController.Singleton._stopUpdating = true;
            }
        }

        private static void SetRoles()
        {
            var players = Player.List.ToList();
            players.Shuffle();
            
            foreach (var data in classIds)
            {
                var num = 0;
                
                for (var i = 0; i < players.Count; i++)
                {
                    if (random.Next(0, 101) > data.chance && num > data.min) continue;
                    
                    players[i].SetRole(data.role.GetRole());
                    CustomRoles.ChangeRole(players[i], data.role.GetCustomRole());
                    players.RemoveAt(i);
                    num++;
                }
                
                players.Shuffle();
            }

            if (players.Count > 0 && finalClass.classId != -1)
            {
                var role = finalClass.GetRole();
                var customRole = finalClass.GetCustomRole();
                
                foreach (var player in players)
                {
                    player.SetRole(role);
                    CustomRoles.ChangeRole(player, customRole);
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
                    player.Position = pos;
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

        private static void SetHP()
        {
            foreach (var data in hpData)
            {
                var list = data.role.GetMembers();

                foreach (var player in list)
                {
                    player.Health = data.amount;
                }
            }
        }
        
        private static void SetSize()
        {
            foreach (var data in sizeData)
            {
                var list = data.role.GetMembers();

                foreach (var player in list)
                {
                    player.Scale = data.scale;
                }
            }
        }
    }
}