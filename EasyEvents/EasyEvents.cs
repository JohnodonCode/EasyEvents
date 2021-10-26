using System;
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
        public override Version Version => new Version(2, 0, 1);
        public override Version RequiredExiledVersion { get; } = new Version(3, 0, 0);

        public static EasyEvents Singleton;

        public override void OnEnabled()
        {
            base.OnEnabled();
            Singleton = this;
            ScriptStore.LoadScripts();
            ScriptActions.AddEvents();
            Exiled.Events.Handlers.Server.RestartingRound += ScriptActions.Reset;
            Exiled.Events.Handlers.Server.ReloadedConfigs += OnConfigUpdate;
            ScriptActions.Reset();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            Singleton = null;
            ScriptStore.Scripts = new Dictionary<string, string>();
            ScriptActions.RemoveEvents();
            Exiled.Events.Handlers.Server.RestartingRound -= ScriptActions.Reset;
            Exiled.Events.Handlers.Server.ReloadedConfigs -= OnConfigUpdate;
        }

        private static void OnConfigUpdate()
        {
            Timing.CallDelayed(1f, ScriptStore.LoadScripts);
        }
    }
}
