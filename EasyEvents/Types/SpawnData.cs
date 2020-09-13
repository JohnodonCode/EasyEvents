namespace EasyEvents.Types
{
    public class SpawnData
    {
        public int chance = -1;
        public RoleInfo role = null;

        public SpawnData(int c, RoleInfo r)
        {
            chance = c;
            role = r;
        }
    }
}