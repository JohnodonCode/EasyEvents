namespace EasyEvents.Types
{
    public class TextData
    {
        public string message;
        public RoleInfo role;
        public int duration;

        public TextData(string message, RoleInfo role, int duration)
        {
            this.message = message;
            this.role = role;
            this.duration = duration;
        }
    }
}