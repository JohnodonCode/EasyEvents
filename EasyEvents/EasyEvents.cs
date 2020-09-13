using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace EasyEvents
{
    public class EasyEvents : Plugin<Config>
    {
        public override string Name => "EasyEvents";
        public override string Author => "PintTheDragon";
        public override Version Version => new Version("1.0.0");
        public override PluginPriority Priority => PluginPriority.Highest;

        public static EasyEvents Singleton;

        public override void OnEnabled()
        {
            base.OnEnabled();
            Singleton = this;
            ScriptStore.LoadScripts();
            ScriptActions.AddEvents();
            Exiled.Events.Handlers.Server.RestartingRound += EventHandlers.onRoundRestart;
        }
        
        public override void OnDisabled()
        {
            base.OnDisabled();
            Singleton = null;
            ScriptActions.RemoveEvents();
            Exiled.Events.Handlers.Server.RestartingRound -= EventHandlers.onRoundRestart;
        }
    }
}