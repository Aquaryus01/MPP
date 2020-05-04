using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.rpcProtocol
{

    [Serializable]
    public class Response
    {


        public ResponseType type { set; get; }
        public Object data { set; get; }

        public Response(ResponseType responseType, Object data1)
        {
            this.type = responseType;
            this.data = data1;
        }

        public Response(ResponseType eRROR)
        {
            this.type = eRROR;
        }
    }

}
