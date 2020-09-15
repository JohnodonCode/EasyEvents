using UnityEngine;

namespace EasyEvents.Types
{
    public class SizeData
    {
        public RoleInfo role;
        public Vector3 scale;

        public SizeData(RoleInfo role, Vector3 scale)
        {
            this.role = role;
            this.scale = scale;
        }
    }
}