namespace EasyEvents.Types
{
    public class InfectData
    {
        public RoleInfo oldRole;
        public RoleInfo newRole;
        public bool soft;

        public InfectData(RoleInfo oldRole, RoleInfo newRole, bool soft)
        {
            this.oldRole = oldRole;
            this.newRole = newRole;
            this.soft = soft;
        }
    }
}