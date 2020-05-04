using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.domain
{


    public class Office : Entity<string>
    {

        public Office(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [XmlAttribute]
        public string Id { get; set; }
        public string Password { get; set; }

        public string Username { get; set; }


       

        public override string ToString()
        {
            return string.Format("[Office-> id:{0}, username:{1}, password:{2}]", Id, Username, Password);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var office = obj as Office;
            return office != null &&
                   Id == office.Id;
        }

        public static bool operator !=(Office o1, Office o2)
        {
            return o1.Id != o2.Id;
        }

        public static bool operator ==(Office o1, Office o2)
        {
            return o1.Id == o2.Id;
        }
    }

}
