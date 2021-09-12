namespace EasyEvents.Types
{
    public class HPData
    {
        public RoleInfo role;
        public int amount;

        public HPData(RoleInfo role, int amount)
        {
            this.role = role;
            this.amount = amount;
        }
    }
}