using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.domain
{
    [Serializable]
    public class OfficeDTO
    {

        public OfficeDTO(string user, string pass)
        {
            this.User = user;
            this.Password = pass;
        }


        public string User { set; get; }
        public string Password { set; get; }

        public override string ToString()
        {
            return "user:" + User + "pass:" + Password;
        }
    }

}

