namespace EasyEvents.Types
{
    public class ClearItemsData
    {
        public CustomRole role = null;
        public int classId = -1;

        public ClearItemsData(CustomRole role, int classId)
        {
            this.role = role;
            this.classId = classId;
        }
    }
}