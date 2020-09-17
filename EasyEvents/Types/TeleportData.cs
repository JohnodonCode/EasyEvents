using EasyEvents.Commands;
using UnityEngine;

namespace EasyEvents.Types
{
    public class TeleportData
    {
        public Vector3 pos;
        public RoleInfo role;

        public TeleportData(Vector3 p, RoleInfo r)
        {
            pos = p;
            role = r;
        }
    }
}