using Autofac;
using Autofac.Integration.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.ServiceModel.Discovery;
using TaskWcf;
using TaskWcf.Infrastructure;
using TaskCommon.Helpers;

namespace TaskWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        ServiceHost svcHost = new ServiceHost(typeof(TaskService), new Uri("http://localhost:8001/TaskService"));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();
            builder.RegisterType<Repository>().As<IRepository>();
            builder.Register(c => new TaskService
                            (c.Resolve<IRepository>())).As<ITaskService>();
            var container = builder.Build();
            
            try
            {
                ServiceMetadataBehavior smb = svcHost.Description.Behaviors.Find<ServiceMetadataBehavior>();

                if (smb == null)
                    smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                svcHost.Description.Behaviors.Add(smb);
                svcHost.Description.Behaviors.Add(new ServiceDiscoveryBehavior());

                svcHost.AddServiceEndpoint(
                  ServiceMetadataBehavior.MexContractName,
                  MetadataExchangeBindings.CreateMexHttpBinding(),
                  "mex"
                );
                
                svcHost.AddServiceEndpoint(typeof(ITaskService), new WSHttpBinding(), "");
                svcHost.AddServiceEndpoint(new UdpDiscoveryEndpoint());

                svcHost.AddDependencyInjectionBehavior<ITaskService>(container);

                svcHost.Open();
                
            }
            catch (CommunicationException commProblem)
            {
                _log.Error(LogHelper.GetErrorMessage($"Не удалось запустить wcf-сервис. Ошибка: {commProblem.Message}", commProblem));               
            }
        }

        protected void Application_Stop()
        {
            svcHost.Close();            
        }
    }

}
