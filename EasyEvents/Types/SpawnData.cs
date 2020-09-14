namespace EasyEvents.Types
{
    public class SpawnData
    {
        public int chance = -1;
        public int min = -1;
        public RoleInfo role = null;

        public SpawnData(int c, int m, RoleInfo r)
        {
            chance = c;
            role = r;
            min = m;
        }
    }
}