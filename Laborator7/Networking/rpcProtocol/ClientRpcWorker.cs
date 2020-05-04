using Domain.domain;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking.rpcProtocol
{
    public class ClientRpcWorker : Observer
    {

        private IServices server;
        private TcpClient connection;
        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool connected;

        public ClientRpcWorker(IServices services, TcpClient conn)
        {
            this.server = services;
            this.connection = conn;
            try
            {
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                connected = true;
            }
            catch(ServerException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }


        public virtual void run()
        {
            while(connected)
            {
                try
                {
                    object request = formatter.Deserialize(stream);
                    object response = handleRequest((Request)request);
                    if(response != null)
                    {
                        sendResponse((Response)response);
                    }
                }
                catch(ServerException er)
                {
                    Console.WriteLine(er.StackTrace);

                }
                try
                {
                    Thread.Sleep(1000);
                }
                catch(ServerException er)
                {
                    Console.WriteLine(er.StackTrace);
                }
            }
            try
            {
                stream.Close();
                connection.Close();
            }
            catch(ServerException er)
            {
                Console.WriteLine(er.StackTrace);
            }
        }



        private void sendResponse(Response response)
        {
            Console.WriteLine("Sending response " + response.type);
            formatter.Serialize(stream, response);
            stream.Flush();
        }


        public virtual void OfficeSubmitted(SampleDTO[] samples)
        {
            Console.WriteLine("Office submitted invoked from Worker");
            try
            {
                sendResponse(new Response(ResponseType.NEW_SUBMIT, samples));
            }
            catch (ServerException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private Response handleRequest(Request request)
        {
            Response response = null;
            if (request.type == RequestType.LOGIN)
            {
                Console.WriteLine("Login request ");
                OfficeDTO officeDTO = (OfficeDTO)request.data;
                Console.WriteLine(officeDTO.User +  "---" + officeDTO.Password);
                try
                {
                    lock (server)
                    {
                        server.login(officeDTO, this);
                    }
                    return new Response(ResponseType.OK);
                }
                catch (ServerException e)
                {
                    connected = false;
                    return new Response(ResponseType.ERROR, e.Message);
                }
            }
            if (request.type == RequestType.LOGOUT)
            {
                Console.WriteLine("Logout request ");
                OfficeDTO officeDTO = (OfficeDTO)request.data;
                try
                {
                    lock (server)
                    {
                        server.logout(officeDTO, this);
                    }
                    connected = false;
                    return new Response(ResponseType.OK);
                }
                catch (ServerException e)
                {
                    return new Response(ResponseType.ERROR, e.Message);
                }
            }
            if (request.type == RequestType.GET_CURRENT_SAMPLES)
            {
                Console.WriteLine("Get current samples request");
                try
                {
                    lock (server)
                    {
                        SampleDTO[] samples = server.getCurrentSamples();
                        return new Response(ResponseType.GET_DISP_SAMPLE, samples);
                    }
                }
                catch (ServerException e)
                {
                    return new Response(ResponseType.ERROR, e.Message);
                }
            }
            if (request.type == RequestType.SEARCH_BY_SAMPLE)
            {
                Console.WriteLine("Search by sample request");
                StyleDTO styleDTO = (StyleDTO)request.data;
                try
                {
                    lock (server)
                    {
                        RegistrationDTO[] registrationDTOs = server.searchBySample(styleDTO);
                        return new Response(ResponseType.GET_SEARCH_RESULT, registrationDTOs);
                    }
                }
                catch (ServerException e)
                {
                    return new Response(ResponseType.ERROR, e.Message);
                }
            }
            if (request.type == RequestType.SUBMIT_REGISTRATION)
            {
                Console.WriteLine("Handling submit request");
                InfoSubmitDTO infoSubmitDTO = (InfoSubmitDTO)request.data;
                try
                {
                    lock (server)
                    {
                        server.submitRegistration(infoSubmitDTO);
                        return new Response(ResponseType.OK);
                    }
                }
                catch (ServerException e)
                {
                    return new Response(ResponseType.ERROR, e.Message);
                }

            }
            return response;
        }


    }
}
