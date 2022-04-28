﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;
using MEC;
using EasyEvents.Handlers;
using EasyEvents.API.EventArgs;

namespace EasyEvents
{
    public class EasyEvents : Plugin<Config>
    {
        public override string Name => "EasyEvents";
        public override string Author => "Johnodon";
        public override Version Version => new Version(4, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(5, 1, 3);

        public static EasyEvents Singleton;

        public override void OnEnabled()
        {
            Singleton = this;
            ScriptStore.LoadScripts();
            ScriptActions.AddEvents();
            Exiled.Events.Handlers.Server.RestartingRound += ScriptActions.Reset;
            Exiled.Events.Handlers.Server.ReloadedConfigs += OnConfigUpdate;
            ScriptActions.Reset();
            base.OnEnabled();
        }
        public static void StartingEvent(StartingEventEventArgs ev)
        {
            Log.Debug(ev.eventData.EventText);
        }

        public override void OnDisabled()
        {
            Singleton = null;
            ScriptStore.Scripts = new Dictionary<string, string>();
            ScriptActions.RemoveEvents();
            Exiled.Events.Handlers.Server.RestartingRound -= ScriptActions.Reset;
            Exiled.Events.Handlers.Server.ReloadedConfigs -= OnConfigUpdate;
            base.OnDisabled();
        }

        private static void OnConfigUpdate()
        {
            Timing.CallDelayed(1f, ScriptStore.LoadScripts);
        }
    }
}
