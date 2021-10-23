namespace src.Helpers
{
    public class AppSettingsSection
    {
        public string Secret { get; set; }
    }

    public class ConnectionStringsSection
    {
        public string LocalDB { get; set; }
    }

    public class AppSettingsAccessor
    {
        public AppSettingsSection AppSettings { get; set; }
        public ConnectionStringsSection ConnectionStrings { get; set; }
    }

}