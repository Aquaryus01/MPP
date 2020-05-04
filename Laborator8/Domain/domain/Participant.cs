using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace Domain.domain
{
    public class Participant : Entity<string>
    {
        public Participant(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }


        [XmlAttribute]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        

        public override bool Equals(object obj)
        {
            var participant = obj as Participant;
            return participant != null && Id == participant.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[Participant-> id:{0}, firstName:{1}, lastName:{2}, age:{3}]",
                Id, FirstName, LastName, Age);
        }

        public static bool operator !=(Participant p1, Participant p2)
        {
            return p1.Id != p2.Id;
        }

        public static bool operator ==(Participant p1, Participant p2)
        {
            return p1.Id == p2.Id;
        }




    }
}
