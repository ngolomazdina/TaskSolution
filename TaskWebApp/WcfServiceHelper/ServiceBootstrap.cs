using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Discovery;
using TaskWcf;
using System.ServiceModel;
using TaskCommon.Helpers;

namespace TaskWebApp.WcfServiceHelper
{
    public static class ServiceBootstrap
    {
        static ITaskService _taskWcfService;
        private static NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();

        public static ITaskService TaskWcfService
        {
            get
            {
                if (_taskWcfService == null)
                    InitializeService();

                return _taskWcfService;
            }
        }

        static void InitializeService()
        {
            try
            {
                DiscoveryClient client = new DiscoveryClient(new UdpDiscoveryEndpoint());

                FindCriteria crit = new FindCriteria(typeof(ITaskService));
                FindResponse resp = client.Find(crit);

                if (resp != null && resp.Endpoints.Count > 0)
                {
                    EndpointDiscoveryMetadata epaddrMtd = resp.Endpoints[0];
                    ChannelFactory<ITaskService> factory = new ChannelFactory<ITaskService>(new WSHttpBinding(), epaddrMtd.Address);

                    _taskWcfService = factory.CreateChannel();
                }

                client.Close();
            }
            catch(Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage($"При поиске wcf- сервиса возникла ошибка", ex));
            }
        }
    }
}