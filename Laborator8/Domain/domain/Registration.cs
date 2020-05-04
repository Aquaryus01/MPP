using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.domain
{
    public class Registration : Entity<string>
    {

        public Registration(string participantId, string sampleId)
        {

            this.ParticipantId = participantId;
            this.SampleId = sampleId;
        }

        [XmlAttribute]
        public string Id { get; set; }
        public string ParticipantId { get; set; }
        public string SampleId { get; set; }

      

        public override bool Equals(object obj)
        {
            var registration = obj as Registration;
            return registration != null &&
                   Id == registration.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[Registration-> participantId:{0}, sampleId:{1}]", ParticipantId, SampleId);
        }

        public static bool operator !=(Registration r1, Registration r2)
        {
            return (r1.Id != r2.Id);
        }

        public static bool operator ==(Registration r1, Registration r2)
        {
            return (r1.Id == r2.Id);
        }
    }
}
