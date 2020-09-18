using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.Events.EventArgs;
using EasyEvents.Types;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using Random = System.Random;

namespace EasyEvents
{
    public static class ScriptActions
    {
        private static Random random = new Random();
        
        public static ScriptActionsStore scriptData = new ScriptActionsStore();
        
        private static Dictionary<int, ScriptActionsStore> delays = new Dictionary<int, ScriptActionsStore>();

        public static ScriptActionsStore GetDelay(int delay)
        {
            if(!delays.ContainsKey(delay)) delays[delay] = new ScriptActionsStore();
            return delays[delay];
        }

        private static IEnumerator<float> DoDelayedAction(int delay)
        {
            yield return Timing.WaitForSeconds(delay);

            Timing.RunCoroutine(RoundStart(delays[delay]));
        }

        public static void SetCustomSpawn(List<SpawnData> _classIds, RoleInfo _finalClass, int line, ScriptActionsStore data)
        {
            if (data.classIds != null) throw new CommandErrorException("Error running command \"spawn\" at line "+line+": Custom spawns have already been set. Only run the \"spawn\" command once.");

            data.classIds = _classIds;
            data.finalClass = _finalClass;
        }

        public static void AddEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Player.Died += OnKill;
            Exiled.Events.Handlers.Map.AnnouncingDecontamination += OnDeconText;
            Exiled.Events.Handlers.Map.Decontaminating += OnDecon;
        }

        public static void RemoveEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Player.Died -= OnKill;
            Exiled.Events.Handlers.Map.AnnouncingDecontamination -= OnDeconText;
            Exiled.Events.Handlers.Map.Decontaminating -= OnDecon;
        }

        public static void Reset()
        {
            CustomRoles.roles = new Dictionary<string, CustomRole>();
            CustomRoles.users = new Dictionary<string, string>();
            
            CustomRoles.roles.Add("all", new CustomRole("all", 2));
            
            scriptData = new ScriptActionsStore();
            delays = new Dictionary<int, ScriptActionsStore>();
        }
        
        private static void OnRoundStarted()
        { 
            Timing.RunCoroutine(RoundStart(scriptData));
            
            foreach (var delay in delays.Keys)
            {
                Timing.RunCoroutine(DoDelayedAction(delay));
            }
        }

        private static void OnKill(DiedEventArgs ev)
        {
            Timing.RunCoroutine(CheckLast());
            
            if (ev.Killer == null) return;
            
            Timing.RunCoroutine(Infect(ev));
        }

        private static void OnDeconText(AnnouncingDecontaminationEventArgs ev)
        {
            ev.IsAllowed = !scriptData.disableDecontamination;
        }

        private static void OnDecon(DecontaminatingEventArgs ev)
        {
            ev.IsAllowed = !scriptData.disableDecontamination;
        }

        private static IEnumerator<float> CheckLast()
        {
            yield return Timing.WaitForSeconds(1f);

            foreach (var role in scriptData.last)
            {
                if (Player.List.Count(p => p.GetRole().Equals(role)) == 1)
                {
                    foreach (var player in Player.List.Where(p => !p.GetRole().Equals(role)))
                    {
                        player.Kill();
                    }
                }

                break;
            }
        }
        
        private static IEnumerator<float> Infect(DiedEventArgs ev)
        {
            yield return Timing.WaitForSeconds(1f);

            foreach (var data in scriptData.infectData)
            {
                if (!data.killedBy.Equals(ev.Killer.GetRole())) continue;

                Vector3 oldPos = ev.Target.Position;
                    
                ev.Target.SetRole(data.newRole.GetRole());

                CustomRoles.ChangeRole(ev.Target, data.newRole.GetCustomRole());

                yield return Timing.WaitForSeconds(1f);

                if (data.soft) ev.Target.Position = oldPos;
                else
                {
                    foreach (var teleportdata in scriptData.teleportIds)
                    {
                        if (!teleportdata.role.Equals(data.newRole)) continue;

                        ev.Target.Position = teleportdata.pos;
                    }
                }

                foreach (var clearItemsData in scriptData.clearItems)
                {
                    if (!clearItemsData.Equals(data.newRole)) continue;
                        
                    ev.Target.ClearInventory();
                }
                    
                foreach (var itemData in scriptData.giveData)
                {
                    if (!itemData.role.Equals(data.newRole)) continue;

                    ev.Target.Inventory.AddNewItem(itemData.item);
                }
                
                foreach (var sizedata in scriptData.sizeData)
                {
                    if (!sizedata.role.Equals(data.newRole)) continue;
                    
                    ev.Target.Scale = sizedata.scale;
                }
                
                foreach (var healthData in scriptData.hpData)
                {
                    if (!healthData.role.Equals(data.newRole)) continue;
                    
                    ev.Target.Health = healthData.amount;
                }
            }
        }
        
        private static IEnumerator<float> RoundStart(ScriptActionsStore dataObj)
        {
            yield return Timing.WaitForSeconds(1f);
            
            if (dataObj.classIds != null && dataObj.classIds.Count > 0)
            {
                SetRoles(dataObj);
                yield return Timing.WaitForSeconds(1f);
            }
            
            Teleport(dataObj);
            ClearItems(dataObj);
            GiveItems(dataObj);
            SetHP(dataObj);
            SetSize(dataObj);
            RunCassie(dataObj);
            RunBroadcasts(dataObj);
            RunHints(dataObj);

            if (dataObj.detonate)
            {
                AlphaWarheadController.Host.StartDetonation();
            }
        }

        private static void SetRoles(ScriptActionsStore dataObj)
        {
            var players = Player.List.ToList();
            players.Shuffle();
            
            foreach (var data in dataObj.classIds)
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

            if (dataObj.finalClass != null && players.Count > 0 && dataObj.finalClass.classId != -1)
            {
                var role = dataObj.finalClass.GetRole();
                var customRole = dataObj.finalClass.GetCustomRole();
                
                foreach (var player in players)
                {
                    player.SetRole(role);
                    CustomRoles.ChangeRole(player, customRole);
                }
            }
        }

        private static void Teleport(ScriptActionsStore dataObj)
        {
            foreach (var data in dataObj.teleportIds)
            {
                var players = data.role.GetMembers();
                
                foreach (var player in players)
                {
                    player.Position = data.pos;
                }
            }
        }

        private static void ClearItems(ScriptActionsStore dataObj)
        {
            foreach (var clearItemsData in dataObj.clearItems)
            {
                var list = clearItemsData.GetMembers();

                foreach (var player in list)
                {
                    player.ClearInventory();
                }
            }
        }

        private static void GiveItems(ScriptActionsStore dataObj)
        {
            foreach (var itemData in dataObj.giveData)
            {
                var list = itemData.role.GetMembers();

                foreach (var player in list)
                {
                    player.Inventory.AddNewItem(itemData.item);
                }
            }
        }

        private static void SetHP(ScriptActionsStore dataObj)
        {
            foreach (var data in dataObj.hpData)
            {
                var list = data.role.GetMembers();

                foreach (var player in list)
                {
                    player.Health = data.amount;
                }
            }
        }
        
        private static void SetSize(ScriptActionsStore dataObj)
        {
            foreach (var data in dataObj.sizeData)
            {
                var list = data.role.GetMembers();

                foreach (var player in list)
                {
                    player.Scale = data.scale;
                }
            }
        }

        private static void RunCassie(ScriptActionsStore dataObj)
        {
            foreach (var cassieData in dataObj.cassie)
            {
                Cassie.Message(cassieData.message, false, false);
                
                if (cassieData.role.roleID == "all")
                {
                    dataObj.cassie.Remove(cassieData);
                }
            }
        }
        
        private static void RunBroadcasts(ScriptActionsStore dataObj)
        {
            foreach (var broadcastData in dataObj.broadcast)
            {
                foreach (var player in broadcastData.role.GetMembers())
                {
                    player.Broadcast(5, broadcastData.message);
                }
                
                if (broadcastData.role.roleID == "all")
                {
                    dataObj.broadcast.Remove(broadcastData);
                }
            }
        }
        
        private static void RunHints(ScriptActionsStore dataObj)
        {
            foreach (var hintData in dataObj.hint)
            {
                foreach (var player in hintData.role.GetMembers())
                {
                    player.ShowHint(hintData.message, 5);
                }

                if (hintData.role.roleID == "all")
                {
                    dataObj.hint.Remove(hintData);
                }
            }
        }
    }
}