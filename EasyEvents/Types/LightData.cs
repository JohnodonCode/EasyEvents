namespace EasyEvents.Types
{
    public class LightData
    {
        public bool HCZOnly;
        public int time;

        public LightData(bool hczOnly, int time)
        {
            HCZOnly = hczOnly;
            this.time = time;
        }
    }
}