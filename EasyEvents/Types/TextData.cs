namespace EasyEvents.Types
{
    public class TextData
    {
        public string message;
        public RoleInfo role;

        public TextData(string message, RoleInfo role)
        {
            this.message = message;
            this.role = role;
        }
    }
}