using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace NetStandardClientLib
{
    public class DemoServiceProxy
    {
        private readonly ChannelFactory<FrameworkClient.Bar.IDemoService> _ChanSrc;

        public DemoServiceProxy()
        {
            var binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpsTransportBindingElement());
            var addr = new EndpointAddress("https://demoservicewcf.azurewebsites.net/DemoService.svc");
            _ChanSrc = new ChannelFactory<FrameworkClient.Bar.IDemoService>(binding, addr);
        }

        public string Reverse(string str)
        {
            var svc = _ChanSrc.CreateChannel();

            try
            {
                return svc.Reverse("ABCDEF");
            }
            finally
            {
                try
                {
                    var client = svc as ICommunicationObject;
                    if (client != null)
                    {
                        if (client.State == CommunicationState.Faulted) client.Abort();
                        else client.Close();
                    }
                }
                catch (Exception)
                {
                    System.Diagnostics.Debug.WriteLine("WCF cleanup threw exception - not helpful!");
                }
            }
        }

        public async Task<string> ReverseAsync(string str)
        {
            var svc = _ChanSrc.CreateChannel();

            try
            {
                return await svc.ReverseAsync("ABCDEF").ConfigureAwait(false);
            }
            finally
            {
                try
                {
                    var client = svc as ICommunicationObject;
                    if (client != null)
                    {
                        if (client.State == CommunicationState.Faulted) client.Abort();
                        else client.Close();
                    }
                }
                catch (Exception)
                {
                    System.Diagnostics.Debug.WriteLine("WCF cleanup threw exception - not helpful!");
                }
            }
        }
    }
}
