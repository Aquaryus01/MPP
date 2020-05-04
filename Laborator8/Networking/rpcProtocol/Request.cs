using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.rpcProtocol
{
    [Serializable]
    public class Request
    {
        public RequestType type { set; get; }
        public Object data { set; get; }

        public Request(RequestType Type, Object data1)
        {
            this.type = Type;
            this.data = data1;
        }
        public Request(RequestType Type)
        {
            this.type = Type;
        }

    }
}
