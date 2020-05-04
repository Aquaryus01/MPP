using Networking.utils;
using Persistance.repository;
using Persistance.service;
using Server.impl;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class StartServer
    {
        static void Main()
        {
            ParticipantRepository participantRepository = new ParticipantRepository();
            OfficeRepository officeRepository = new OfficeRepository();
            RegistrationRepository registrationRepository = new RegistrationRepository();
            SampleRepository sampleRepository = new SampleRepository();
            Service service = new Service(officeRepository,sampleRepository,registrationRepository,participantRepository);

            IServices serviceImpl = new ServicesImplementation(service);

            SerialServer server = new SerialServer("127.0.0.1", 55555, serviceImpl);

            Console.WriteLine("Server started ...");
            Console.WriteLine(service.LocalLogin("admin", "admin"));
            server.Start();
            Console.ReadLine();
        } 

    }
   

}
