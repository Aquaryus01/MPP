using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.domain
{
    [Serializable]
    public class SampleDTO
    {


        public SampleDTO(Distance distance, Style style, int numberParticipants)
        {
            this.Distance = distance;
            this.Style = style;
            this.NumberParticipants = numberParticipants;
        }

        [XmlAttribute]

        public Distance Distance { get; set; }
        public Style Style { get; set; }
        public int NumberParticipants { get; set; }

        public string Id { get; set; }

       

        public override bool Equals(object obj)
        {
            var sample = obj as SampleDTO;
            return sample != null && Id == sample.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[SampleDTO-> id:{0}, distance:{1}, style:{2}, numberOfParticipants{3}]",
                Id, Distance, Style, NumberParticipants);
        }

        public static bool operator !=(SampleDTO p1, SampleDTO p2)
        {
            return p1.Id != p2.Id;
        }

        public static bool operator ==(SampleDTO p1, SampleDTO p2)
        {
            return p1.Id == p2.Id;
        }

    }

}
