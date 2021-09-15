using System.Collections.Generic;
using EasyEvents.Types;

namespace EasyEvents
{
    public class ScriptActionsStore
    {
        public bool eventRan = false;

        public bool lastRan = false;
        
        public List<SpawnData> classIds = null;
        public RoleInfo finalClass = null;
        
        public List<TeleportData> teleportIds = new List<TeleportData>();
        
        public bool detonate = false;
        
        public List<RoleInfo> clearItems = new List<RoleInfo>();
        
        public List<GiveData> giveData = new List<GiveData>();
        
        public List<InfectData> infectData = new List<InfectData>();
        
        public List<HPData> hpData = new List<HPData>();

        public List<SizeData> sizeData = new List<SizeData>();
        
        public List<DoorData> doorData = new List<DoorData>();

        public bool disableDecontamination = false;

        public List<RoleInfo> last = new List<RoleInfo>();

        public List<TextData> cassie = new List<TextData>();
        public List<TextData> broadcast = new List<TextData>();
        public List<TextData> hint = new List<TextData>();
        
        public List<LightData> lights = new List<LightData>();

        public bool disableNuke = false;
        
        public List<RoleInfo> escape = new List<RoleInfo>();

        public ScriptActionsStore()
        {
            
        }

        public void Add(ScriptActionsStore data)
        {
            this.teleportIds.AddRange(data.teleportIds);
            this.broadcast.AddRange(data.broadcast);
            this.hint.AddRange(data.hint);
            this.lights.AddRange(data.lights);
        }
    }
}