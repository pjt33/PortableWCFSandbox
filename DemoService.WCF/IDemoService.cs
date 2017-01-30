using System.ServiceModel;
using System.ServiceModel.Web;

namespace Foo
{
    [ServiceContract(Namespace = Namespaces.Demo)]
    public interface IDemoService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        string Reverse(string str);
    }
}