using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.domain
{
    [Serializable]
    public class InfoSubmitDTO
    {

        public InfoSubmitDTO(string firstName, string lastName, int age, Distance distance, Style style)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
            this.Distance = distance;
            this.Style = style;
        }

        public string FirstName { set; get; }
        public string LastName { set; get; }
        public int Age { set; get; }

        public Distance Distance { set; get; }
        public Style Style { set; get; }
    }
}
