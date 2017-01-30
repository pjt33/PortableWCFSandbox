using System.Linq;
using System.ServiceModel;

namespace Foo
{
    [ServiceBehavior(Namespace = Namespaces.Demo)]
    public class DemoService : IDemoService
    {
        public string Reverse(string str)
        {
			return new string(str.ToCharArray().Reverse().ToArray());
        }
    }
}