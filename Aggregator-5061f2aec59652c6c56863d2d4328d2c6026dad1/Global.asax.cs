using System;
using Aggregator.Business.Startup;

namespace Aggregator
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            InitializeService.Initialize();
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }
       
        protected void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            InitializeService.Settings.LoggingSettings.AccommodationJobHost.Stop(true);
        }
    }
}