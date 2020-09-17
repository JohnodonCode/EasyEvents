using System.Collections.Generic;
using EasyEvents.Types;

namespace EasyEvents
{
    public class ScriptActionsStore
    {
        public bool eventRan = false;
        
        public List<SpawnData> classIds = null;
        public RoleInfo finalClass = null;
        
        public List<TeleportData> teleportIds = null;
        
        public bool detonate = false;
        
        public List<RoleInfo> clearItems = new List<RoleInfo>();
        
        public List<GiveData> giveData = new List<GiveData>();
        
        public List<InfectData> infectData = new List<InfectData>();
        
        public List<HPData> hpData = new List<HPData>();

        public List<SizeData> sizeData = new List<SizeData>();
        
        public bool disableDecontamination = false;
        
        public ScriptActionsStore()
        {
            
        }
    }
}