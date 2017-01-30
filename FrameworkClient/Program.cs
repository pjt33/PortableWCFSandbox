using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace FrameworkClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpsTransportBindingElement());
            var addr = new EndpointAddress("https://demoservicewcf.azurewebsites.net/DemoService.svc");
            var chanSrc = new ChannelFactory<Bar.IDemoService>(binding, addr);
            var svc = chanSrc.CreateChannel();

            try
            {
                Console.WriteLine(svc.Reverse("ABCDEF"));
                Console.WriteLine(svc.ReverseAsync("123456").Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                    Console.WriteLine("WCF cleanup threw exception - not helpful!");
                }
            }

            Console.ReadKey();
        }
    }
}
