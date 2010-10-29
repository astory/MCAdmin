namespace MCAdmin
{
    public class Plugin
    {
        public Plugin()
        {
            this.Load();
        }
        ~Plugin()
        {
            this.Unload();
        }

        public virtual void Load() { }
        public virtual void Unload() { }
    }
}
