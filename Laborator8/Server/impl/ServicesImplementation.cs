using Domain.domain;
using Persistance.service;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.impl
{
    public class ServicesImplementation: MarshalByRefObject, IServices
    {
        private Service service;
        private readonly IDictionary<string, Observer> loggedOffices;

        public ServicesImplementation(Service service)
        {
            this.service = service;
            loggedOffices = new Dictionary<string, Observer>();
        }

        public SampleDTO[] getCurrentSamples()
        {
            List<SampleDTO> samples = this.service.GroupBySample();
            return samples.ToArray();
        }

        public void login(OfficeDTO officeDTO, Observer client)
        {
            bool isOffice = this.service.LocalLogin(officeDTO.User, officeDTO.Password);
            if(isOffice)
            {
                if(loggedOffices.ContainsKey(officeDTO.User))
                {
                    throw new ServerException("Office is already logged in. Sorry...");
                }
                loggedOffices[officeDTO.User] = client;
            }
            else
            {
                throw new ServerException("Invalid password or user! Please try again..");
            }

        }

        public void logout(OfficeDTO officeDTO, Observer client)
        {
            Observer observer = loggedOffices[officeDTO.User];
            if(observer == null)
            {
                throw new ServerException("User is not logged in");
            }
            loggedOffices.Remove(officeDTO.User);
        }

        public RegistrationDTO[] searchBySample(StyleDTO styleDTO)
        {
            List<RegistrationDTO> registrationDTOs = this.service.findAllRegistrationDTO(styleDTO.Distance, styleDTO.Style);
            return registrationDTOs.ToArray();
        }

        public void submitRegistration(InfoSubmitDTO infoSubmitDTO)
        {
            try
            {
                this.service.addRegistration(infoSubmitDTO.FirstName, infoSubmitDTO.LastName, infoSubmitDTO.Age, infoSubmitDTO.Distance, infoSubmitDTO.Style);
                Console.WriteLine("REgistration saved in database");
                notifyOfficeSubmited();

            }
            catch(ServerException)
            {
                throw new ServerException("Could not submit this registration..");
            }


        }

        public void notifyOfficeSubmited()
        {
            List<Office> offices = this.service.getAllOffices();
            List<SampleDTO> newSamples = this.service.GroupBySample();
            SampleDTO[] result = newSamples.ToArray();
            Console.WriteLine("Notifiying all offices about the new submit");
            foreach (Office office in offices)
            {
                if (loggedOffices.ContainsKey(office.Username))
                {
                    Observer employee = loggedOffices[office.Username];
                    Task.Run(() => employee.OfficeSubmitted(result));
                }
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }


    }
}
