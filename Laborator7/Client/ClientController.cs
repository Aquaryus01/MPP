using Domain.domain;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientController: Observer
    {
        public event EventHandler<UserEventArgs> updateEvent;
        private readonly IServices server;
        private OfficeDTO crtOffice;


        public ClientController(IServices services)
        {
            this.server = services;
        }

        protected virtual void OnUserEvent(UserEventArgs e)
        {
            if (updateEvent == null) return;
                updateEvent(this, e);
            Console.WriteLine("Update event called! ");
        }

        public void login(string user, string password)
        {
            OfficeDTO officeDTO = new OfficeDTO(user, password);
            Console.WriteLine(officeDTO.User, officeDTO.Password);
            this.server.login(officeDTO, this);
            Console.WriteLine("Login succesfuly!");
            crtOffice = officeDTO;
            Console.WriteLine("Curren Office {0}", crtOffice);
        }

        public void logout()
        {
            Console.WriteLine("Controller logged out");
            this.server.logout(crtOffice, this);
            crtOffice = null;
        }


        public List<SampleDTO> getCurrentSamples()
        {
            List<SampleDTO> sampleDTOs = new List<SampleDTO>();
            SampleDTO[] samples = this.server.getCurrentSamples();
            foreach(SampleDTO sample in samples)
            {
                sampleDTOs.Add(sample);
            }
            return sampleDTOs;
        }

        public List<RegistrationDTO> searchBySample(StyleDTO styleDTO)
        {
            List<RegistrationDTO> registrationDTOs = new List<RegistrationDTO>();
            RegistrationDTO[] registrations = this.server.searchBySample(styleDTO);
            foreach(RegistrationDTO registration in registrations)
            {
                registrationDTOs.Add(registration);
            }
            return registrationDTOs;
        }


        public void submitRegistration(InfoSubmitDTO infoSubmitDTO)
        {
            this.server.submitRegistration(infoSubmitDTO);
            SampleDTO[] samples = this.getCurrentSamples().ToArray();
            UserEventArgs userEventArgs = new UserEventArgs(Event.NEW_CLIENT, samples);
            OnUserEvent(userEventArgs);
        }

        public void OfficeSubmitted(SampleDTO[] samples)
        {
            UserEventArgs userEventArgs = new UserEventArgs(Event.NEW_CLIENT, samples);
            Console.WriteLine("Submitted recieveed!");
            OnUserEvent(userEventArgs);
        }


    }
}
