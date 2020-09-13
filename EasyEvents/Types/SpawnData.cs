namespace EasyEvents.Types
{
    public class SpawnData
    {
        public int classId = -1;
        public int chance = -1;
        public CustomRole role = null;

        public SpawnData(int id, int c, CustomRole r)
        {
            classId = id;
            chance = c;
            role = r;
        }
    }
}