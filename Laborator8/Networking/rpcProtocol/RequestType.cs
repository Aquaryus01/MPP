using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.rpcProtocol
{
    [Serializable]
    public enum RequestType
    {
        LOGIN,
        LOGOUT,
        SUBMIT_REGISTRATION,
        SEARCH_BY_SAMPLE,
        GET_CURRENT_SAMPLES
    }
}
