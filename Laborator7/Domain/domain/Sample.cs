using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.domain
{


    public class Sample : Entity<string>
    {

        public Sample(Distance distance, Style style)
        {

            Distance = distance;
            Style = style;
        }

        [XmlAttribute]

        public string Id { get; set; }
        public Distance Distance { get; set; }
        public Style Style { get; set; }


        public override bool Equals(object obj)
        {
            var sample = obj as Sample;
            return sample != null && Id == sample.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[Sample-> id:{0}, distance:{1}, style:{2}]",
                Id, Distance, Style);
        }

        public static bool operator !=(Sample p1, Sample p2)
        {
            return p1.Id != p2.Id;
        }

        public static bool operator ==(Sample p1, Sample p2)
        {
            return p1.Id == p2.Id;
        }


    }

}
