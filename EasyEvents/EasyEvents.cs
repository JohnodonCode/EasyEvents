using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace EasyEvents
{
    public class EasyEvents : Plugin<Config>
    {
        public static EasyEvents Singleton;
        
        public override void OnEnabled()
        {
            base.OnEnabled();

            Singleton = this;

            ScriptStore.LoadScripts();
        }
        
        public override void OnDisabled()
        {
            base.OnDisabled();

            Singleton = null;
            
            ScriptActions.RemoveEvents();
        }
    }
}