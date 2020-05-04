using Networking.utils;
using Persistance.repository;
using Persistance.service;
using Server.impl;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class StartServer
    {
        static void Main()
        {
            //ParticipantRepository participantRepository = new ParticipantRepository();
            //OfficeRepository officeRepository = new OfficeRepository();
            //RegistrationRepository registrationRepository = new RegistrationRepository();
            //SampleRepository sampleRepository = new SampleRepository();
            //Service service = new Service(officeRepository, sampleRepository, registrationRepository, participantRepository);

            //IServices serviceImpl = new ServicesImplementation(service);

            //SerialServer server = new SerialServer("127.0.0.1", 55555, serviceImpl);

            //Console.WriteLine("Server started ...");
            //Console.WriteLine(service.LocalLogin("admin", "admin"));
            //server.Start();
            //Console.ReadLine();

            //.NET Remote

            BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
            serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();
            IDictionary props = new Hashtable();

            props["port"] = 55555;
            TcpChannel channel = new TcpChannel(props, clientProv, serverProv);
            ChannelServices.RegisterChannel(channel, false);

            ParticipantRepository participantRepository = new ParticipantRepository();
            OfficeRepository officeRepository = new OfficeRepository();
            RegistrationRepository registrationRepository = new RegistrationRepository();
            SampleRepository sampleRepository = new SampleRepository();

            Service service = new Service(officeRepository, sampleRepository, registrationRepository, participantRepository);

            var server = new ServicesImplementation(service);

            RemotingServices.Marshal(server, "Swimming");


            Console.WriteLine("Server started ...");
            Console.WriteLine("Press <enter> to exit...");
            Console.ReadLine();
        } 

    }
   

}
