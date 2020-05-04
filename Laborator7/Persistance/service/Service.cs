using Domain.domain;
using Persistance.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.service
{
    public class Service
    {

        public OfficeRepository Office { get; set; }
        public SampleRepository Sample { get; set; }
        public RegistrationRepository Registration { get; set; }
        public ParticipantRepository Participant { get; set; }

        public Service(OfficeRepository office, SampleRepository sample, RegistrationRepository registration, ParticipantRepository participant)
        {
            Office = office;
            Sample = sample;
            Registration = registration;
            Participant = participant;
        }

        public List<Office> getAllOffices()
        {
            return this.Office.findAll();
        }

        public void AddOffice(Office office)
        {
            this.Office.save(office);
        }


        public void AddParticipant(Participant participant)
        {
            this.Participant.save(participant);
        }

        public void AddSample(Sample sample)
        {
            this.Sample.save(sample);
        }


        public void AddRegistration(Registration registration)
        {
            this.Registration.save(registration);
        }

        public bool LocalLogin(String user, String pass)
        {
            return this.Office.Login(user, pass);
        }

        public List<SampleDTO> GroupBySample()
        {
            return this.Sample.GroupByNumberOfSample();
        }



        public List<RegistrationDTO> findAllRegistrationDTO(Distance distance, Style style)
        {
            List<RegistrationDTO> result = new List<RegistrationDTO>();
            List<String> rezId = new List<String>();
            rezId = Registration.findBySample(distance, style);
            foreach (String s in rezId)
            {
                foreach (RegistrationDTO r in Registration.findByParticipant(s))
                {
                    result.Add(r);
                }
            }
            return result;
        }

        public void addRegistration(string firstName, string lastName, int age, Distance distance, Style style)
        {

            string id = Participant.FindMaxId();
            string idSample = Sample.findIdSample(distance, style);
            if (idSample == null)
            {
                Sample sample = new Sample(distance, style);
                sample.Id = Sample.FindMaxId();
                Sample.save(sample);
                idSample = Sample.findIdSample(distance, style);

            }
            //String idInscriere = Registration.findIdRegistration(id, idSample);
            //if (idInscriere == null)
            //{
            Participant participant = new Participant(firstName, lastName, age);
            participant.Id = id;
            Participant.save(participant);
            Registration registration = new Registration(id, idSample);
            registration.Id = Registration.FindMaxId();
            Registration.save(registration);
            //}


        }





    }
}
