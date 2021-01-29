using Autofac;
using Microsoft.Extensions.Configuration;

namespace Deploy.Appliction.Config
{
    public class AppConfig
    {
        public static readonly AppConfig Default = new AppConfig();

        internal IConfiguration Configuration { get; set; }
        public DeployOption Deploy { get; set; }

        public AppConfig()
        {
            GetDeployConfig();
        }

        protected void GetDeployConfig()
        {
            Configuration = Appliction.Extensions.Utils.Current.Resolve<IConfiguration>();
            var section = Configuration.GetSection(nameof(Deploy));
            Deploy = section.Exists() 
                ? section.Get<DeployOption>() 
                : new DeployOption();
        }
    }
}