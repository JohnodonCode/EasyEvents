using System;
using System.Collections.Generic;
using System.Linq;
using EasyEvents.Integration;
using Exiled.Events.EventArgs;
using EasyEvents.Types;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using Random = System.Random;
using Exiled.Loader;

namespace EasyEvents
{
    public static class ScriptActions
    {
        private static Random random = new Random();
        
        public static ScriptActionsStore scriptData = new ScriptActionsStore();
        
        private static Dictionary<int, ScriptActionsStore> delays = new Dictionary<int, ScriptActionsStore>();
        
        private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

        public static ScriptActionsStore GetDelay(int delay)
        {
            if(!delays.ContainsKey(delay)) delays[delay] = new ScriptActionsStore();
            return delays[delay];
        }

        private static IEnumerator<float> DoDelayedAction(int delay)
        {
            yield return Timing.WaitForSeconds(delay);

            coroutines.Add(Timing.RunCoroutine(RoundStart(delays[delay], false)));
        }

        public static void SetCustomSpawn(List<SpawnData> _classIds, RoleInfo _finalClass, int line, ScriptActionsStore data)
        {
            if (data.classIds != null) throw new CommandErrorException("Error running command \"spawn\" at line "+line+": Custom spawns have already been set. Only run the \"spawn\" command once.");

            DebugLog($"Spawning... Class IDs: {_classIds}");
            data.classIds = _classIds;
            data.finalClass = _finalClass;
            foreach (var item in _classIds)
            {
                Log.Info(item);
            }
            foreach (var item in _classIds)
            {
                Log.Info(item);
            }
        }

        public static void AddEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Player.Died += OnKill;
            Exiled.Events.Handlers.Map.Decontaminating += OnDecon;
            Exiled.Events.Handlers.Player.ChangingRole += OnRoleChange;
            Exiled.Events.Handlers.Warhead.Starting += OnNukeStart;
            Exiled.Events.Handlers.Player.Escaping += OnEscape;
        }

        public static void RemoveEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Player.Died -= OnKill;
            Exiled.Events.Handlers.Map.Decontaminating -= OnDecon;
            Exiled.Events.Handlers.Player.ChangingRole -= OnRoleChange;
            Exiled.Events.Handlers.Warhead.Starting -= OnNukeStart;
            Exiled.Events.Handlers.Player.Escaping -= OnEscape;
        }

        public static void Reset()
        {
            foreach (var coroutine in coroutines)
            {
                Timing.KillCoroutines(coroutine);
            }
            
            CustomRoles.roles = new Dictionary<string, CustomRole>();
            CustomRoles.users = new Dictionary<string, string>();

            CustomRoles.roles.Add("all", new CustomRole("all", 2));

            scriptData = new ScriptActionsStore();
            delays = new Dictionary<int, ScriptActionsStore>();

            Timing.CallDelayed(10f, () =>
            {
                try
                {
                    AdvancedSubclassing.PopulateCustomRoles();
                    
                    if (EasyEvents.Singleton.Config.Events.Count > 0)
                    {
                        var selected = EasyEvents.Singleton.Config.Events.PickRandom().Trim().ToLower().Replace(" ", "");

                        if (selected == "none") return;
                        if (!ScriptStore.Scripts.ContainsKey(selected)) throw new EventNotFoundException("The event \"" + selected + "\" was not found while attempting to automatically run an event.");
                        ScriptHandler.RunScript(ScriptStore.Scripts[selected]);

                        scriptData.eventRan = true;
                        Loader.Plugins.FirstOrDefault(pl => pl.Name == "SCPStats")?.Assembly?.GetType("SCPStats.EventHandler")?.GetField("PauseRound")?.SetValue(null, true);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            });
        }
        
        private static void OnRoundStarted()
        {
            DebugLog("Round Started");
            Timing.RunCoroutine(RoundStart(scriptData, true));
        }

        private static void OnKill(DiedEventArgs ev)
        {
            if (ev.Killer == null) return;

            Timing.RunCoroutine(Infect(ev));
        }

        private static void OnRoleChange(ChangingRoleEventArgs ev)
        {
            Timing.RunCoroutine(CheckLast());
        }

        private static void OnDecon(DecontaminatingEventArgs ev)
        {
            ev.IsAllowed = !scriptData.disableDecontamination;
        }

        private static void OnNukeStart(StartingEventArgs ev)
        {
            ev.IsAllowed = !scriptData.disableNuke;
        }

        private static void OnEscape(EscapingEventArgs ev)
        {
            Timing.RunCoroutine(LastEscape(ev));
        }

        private static void DebugLog(object message)
        {
            if (!EasyEvents.Singleton.Config.Debug) return;
            Log.Debug(message);
        }

        private static IEnumerator<float> LastEscape(EscapingEventArgs ev)
        {
            RoleInfo r = ev.Player.GetRole();
            
            yield return Timing.WaitForSeconds(1f);

            foreach (var role in scriptData.escape)
            {
                if (role.Equals(r))
                {
                    scriptData.lastRan = true;

                    foreach (var player in Player.List.Where(p => p.Id != ev.Player.Id))
                    {
                        player.SetRole(RoleType.Spectator);
                        CustomRoles.ChangeRole(player, null);
                    }
                    
                    ev.Player.SetRole(RoleType.Tutorial);
                    CustomRoles.ChangeRole(ev.Player, null);
                }
            }
        }

        private static IEnumerator<float> CheckLast()
        {
            yield return Timing.WaitForSeconds(1.5f);

            if (scriptData.lastRan) yield break;

            foreach (var role in scriptData.last)
            {

                if (Player.List.Count(p => p.GetRole().Equals(role)) < 2)
                {
                    scriptData.lastRan = true;
                    
                    foreach (var player in Player.List.Where(p => !p.GetRole().Equals(role)))
                    {
                        player.SetRole(RoleType.Spectator);
                        CustomRoles.ChangeRole(player, null);
                    }

                    foreach (var player in Player.List.Where(p => p.GetRole().Equals(role)))
                    {
                        player.SetRole(RoleType.Tutorial);
                        CustomRoles.ChangeRole(player, null);
                    }
                    
                    break;
                }
            }
        }
        
        private static readonly List<DamageTypes.DamageType> DisallowedDamage = new List<DamageTypes.DamageType>()
        {
            DamageTypes.Contain, 
            DamageTypes.Decont, 
            DamageTypes.None, 
            DamageTypes.Nuke, 
            DamageTypes.Lure, 
            DamageTypes.Recontainment, 
            DamageTypes.FriendlyFireDetector, 
            DamageTypes.Contain, 
            DamageTypes.Decont, 
            DamageTypes.None, 
            DamageTypes.Nuke, 
            DamageTypes.Lure,
            DamageTypes.Recontainment,
            DamageTypes.FriendlyFireDetector,
            DamageTypes.Wall,
            DamageTypes.RagdollLess
        };
        private static IEnumerator<float> Infect(DiedEventArgs ev)
        {
            if (DisallowedDamage.Contains(ev.HitInformations.Tool)) yield break;

            var role = ev.Target.GetRole();
            
            DebugLog("Infect waiting");
            yield return Timing.WaitForSeconds(2f);
            DebugLog("Infect done waiting");

            if (scriptData.lastRan)
            {
                DebugLog("");
                yield break;
            }

            var ran = false;

            DebugLog("Infect running checks");
            
            foreach (var data in scriptData.infectData)
            {
                if (!data.oldRole.Equals(role))
                {
                    DebugLog("Role " + data.oldRole.classId + "," + data.oldRole.roleID + " is not equal to " + role.classId + "," + role.roleID + ".");
                    continue;
                }
                
                DebugLog("Role " + data.oldRole.classId + "," + data.oldRole.roleID + " is equal to " + role.classId + "," + role.roleID + ".");

                ran = true;

                Vector3 oldPos = ev.Target.Position;
                    
                ev.Target.SetRole(data.newRole.GetRole());

                CustomRoles.ChangeRole(ev.Target, data.newRole.GetCustomRole());

                DebugLog("Done setting roles");

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

                    ev.Target.AddItem(itemData.item);
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

                foreach (var broadcastData in scriptData.broadcast)
                {
                    if (!broadcastData.role.Equals(data.newRole) || broadcastData.role.roleID == "all") continue;
                    
                    ev.Target.Broadcast((ushort) broadcastData.duration, broadcastData.message);
                }

                foreach (var hintData in scriptData.hint)
                {
                    if (!hintData.role.Equals(data.newRole) || hintData.role.roleID == "all") continue;
                    
                    ev.Target.ShowHint(hintData.message, hintData.duration);
                }

                DebugLog("Infect done.");
            }

            if (!ran)
            {
                DebugLog("Infect not ran. Resetting player.");
                CustomRoles.ChangeRole(ev.Target, null);
            }
        }
        
        private static IEnumerator<float> RoundStart(ScriptActionsStore dataObj, bool main)
        {
            DebugLog("Corutine Started");
            yield return Timing.WaitForSeconds(1f);
            
            if (dataObj.classIds != null && dataObj.classIds.Count > 0)
            {
                DebugLog("classIds not null, spawning");
                SetRoles(dataObj);
                yield return Timing.WaitForSeconds(1f);
            }
            
            try
            {
                Teleport(dataObj);
                ClearItems(dataObj);
                GiveItems(dataObj);
                SetHP(dataObj);
                SetSize(dataObj);
                RunCassie(dataObj);
                RunBroadcasts(dataObj);
                RunHints(dataObj);
                RunLights(dataObj);
                RunDoors(dataObj);

                if (dataObj.detonate)
                {
                    AlphaWarheadController.Host.StartDetonation();
                }

                if (main)
                {
                    foreach (var delay in delays.Keys)
                    {
                        Timing.RunCoroutine(DoDelayedAction(delay));
                    }
                }
                else
                {
                    scriptData.Add(dataObj);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private static void SetRoles(ScriptActionsStore dataObj)
        {
            var players = Player.List.ToList();
            players.Shuffle();

            var playersTemp = players.Clone();
            
            foreach (var data in dataObj.classIds)
            {
                var num = 0;
                
                for (var i = 0; i < playersTemp.Count; i++)
                {
                    if (random.Next(0, 101) > data.chance || num > data.min) continue;
                    
                    playersTemp[i].SetRole(data.role.GetRole());
                    CustomRoles.ChangeRole(playersTemp[i], data.role.GetCustomRole());
                    players.RemoveAll(player => player.Id == playersTemp[i].Id);
                    num++;
                }
                
                players.Shuffle();
                playersTemp = players.Clone();
            }
            
            playersTemp = players.Clone();

            var spawnData = dataObj.classIds.Where(data => data.chance == 100).ToList();
            
            if (spawnData.Count > 0)
            {
                for (var i = 0; i < playersTemp.Count; i++)
                {
                    playersTemp[i].SetRole(spawnData[0].role.GetRole());
                    DebugLog(playersTemp[i]);
                    DebugLog(spawnData[0].role.GetCustomRole());
                    CustomRoles.ChangeRole(playersTemp[i], spawnData[0].role.GetCustomRole());
                    players.RemoveAll(player => player.Id == playersTemp[i].Id);
                }
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
                    player.AddItem(itemData.item);
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
                    player.Broadcast((ushort) broadcastData.duration, broadcastData.message);
                }
            }
        }
        
        private static void RunHints(ScriptActionsStore dataObj)
        {
            foreach (var hintData in dataObj.hint)
            {
                foreach (var player in hintData.role.GetMembers())
                {
                    player.ShowHint(hintData.message, hintData.duration);
                }
            }
        }

        private static void RunLights(ScriptActionsStore dataObj)
        {
            foreach (var lightData in dataObj.lights)
            {
                ZoneType zonetype = ZoneType.Unspecified;
                if (lightData.HCZOnly) zonetype = ZoneType.HeavyContainment;
                Map.TurnOffAllLights(lightData.time, zonetype);
            }
        }

        private static void RunDoors(ScriptActionsStore dataObj)
        {
            foreach (var doorData in dataObj.doorData)
            {
                foreach(Door door in doorData.doors)
                {
                    switch (doorData.action)
                    {
                        case "break":
                        case "destroy":
                            door.BreakDoor();
                            break;
                        case "open":
                            door.IsOpen = true;
                            break;
                        case "close":
                            door.IsOpen = false;
                            break;
                        case "lock":
                            door.ChangeLock(DoorLockType.DecontLockdown);
                            break;
                        case "unlock":
                            door.ChangeLock(DoorLockType.None);
                            break;
                        default:
                            throw new InvalidArgumentException("Invalid argument for command \"door\" on line " + doorData.i + ", argument 1. \"" + doorData.action + "\" is not a valid door action.");
                    }
                }
                
            }
        }
    }
}
