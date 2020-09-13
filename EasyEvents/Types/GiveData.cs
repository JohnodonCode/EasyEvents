namespace EasyEvents.Types
{
    public class GiveData
    {
        public ItemType item;
        public RoleInfo role;

        public GiveData(ItemType item, RoleInfo role)
        {
            this.item = item;
            this.role = role;
        }
    }
}