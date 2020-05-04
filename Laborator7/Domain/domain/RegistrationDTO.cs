using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.domain
{
    [Serializable]
    public class RegistrationDTO
    {

        public RegistrationDTO(string firstName, string lastName, int age, Distance distance, Style style)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Distance = distance;
            Style = style;
        }

        [XmlAttribute]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public Distance Distance { get; set; }

        public Style Style { get; set; }

       
        public override bool Equals(object obj)
        {
            return obj is RegistrationDTO dTO &&
                   FirstName == dTO.FirstName &&
                   LastName == dTO.LastName &&
                   Age == dTO.Age &&
                   Distance == dTO.Distance &&
                   Style == dTO.Style;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[RegistrationDTO-> firstName:{0}, lastName:{1}, age:{2}, distance{3}, style{4}]",
                FirstName, LastName, Age, Distance, Style);
        }


    }

}
