using System;
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using EasyEvents.API.EventArgs;

namespace EasyEvents
{
    public class EasyEvents : Plugin<Config>
    {
        public override string Name { get; } = "EasyEvents";
        public override string Author { get; } = "Johnodon";
        public override Version Version { get; } = new Version(4, 0, 1);
        public override Version RequiredExiledVersion { get; } = new Version(5, 2, 1);

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
            ScriptStore.Scripts = new Dictionary<string, string>();
            ScriptActions.RemoveEvents();
            Exiled.Events.Handlers.Server.RestartingRound -= ScriptActions.Reset;
            Exiled.Events.Handlers.Server.ReloadedConfigs -= OnConfigUpdate;
            Singleton = null;
            base.OnDisabled();
        }

        private static void OnConfigUpdate()
        {
            Timing.CallDelayed(1f, ScriptStore.LoadScripts);
        }
    }
}
