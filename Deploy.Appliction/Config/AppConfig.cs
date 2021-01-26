namespace Deploy.Appliction.Config
{
    public class AppConfig
    {
        public static AppConfig Default = new AppConfig();

        public DeployOption Deploy { get; set; } = new DeployOption();

        public AppConfig()
        {
        }
    }
}