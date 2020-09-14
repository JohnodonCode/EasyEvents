namespace EasyEvents.Types
{
    public class InfectData
    {
        public RoleInfo killedBy;
        public RoleInfo newRole;
        public bool soft;

        public InfectData(RoleInfo killedBy, RoleInfo newRole, bool soft)
        {
            this.killedBy = killedBy;
            this.newRole = newRole;
            this.soft = soft;
        }
    }
}