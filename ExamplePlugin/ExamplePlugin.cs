using MCAdmin.Commands;
namespace MCAdmin.Plugins
{
    class ExamplePlugin : Plugin
    {
        public override void Load()
        {
            Program.AddRTLine(System.Drawing.Color.Black, "Loaded ExamplePlugin\r\n", true);
        }
        public override void Unload()
        {
            Program.AddRTLine(System.Drawing.Color.Black, "Unloaded ExamplePlugin\r\n", true);
        }
    }
}
