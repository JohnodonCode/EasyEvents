namespace EasyEvents.Types
{
    public class InfectData
    {
        public RoleInfo originalRole;
        public RoleInfo newRole;
        public bool soft;

        public InfectData(RoleInfo originalRole, RoleInfo newRole, bool soft)
        {
            this.originalRole = originalRole;
            this.newRole = newRole;
            this.soft = soft;
        }
    }
}