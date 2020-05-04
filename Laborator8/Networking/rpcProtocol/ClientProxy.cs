using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Domain.domain;

namespace Networking.rpcProtocol
{
     public class ClientProxy: IServices
    {
        private int port;
        private string host;

        private Observer client;
        private NetworkStream stream;
        private IFormatter formatter;
        private TcpClient connection;

        private Queue<Response> responses;
        private volatile bool finished;
        private EventWaitHandle waitHandle;


        public ClientProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
            responses = new Queue<Response>();
        }


        private void initializeConnection()
        {

            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                finished = false;
                waitHandle = new AutoResetEvent(false);
                startReader();
            }
            catch(ServerException e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }

        private void sendRequest(Request request)
        {
            try
            {
                formatter.Serialize(stream, request);
                stream.Flush();
            }
            catch(ServerException er)
            {
                throw new ServerException("Couldnt send request right now" + er);
            }
        }


        private Response readResponse()
        {
            Response response = null;
            try
            {
                waitHandle.WaitOne();
                lock(responses)
                {
                    response = responses.Dequeue();
                }
            }
            catch(ServerException er)
            {
                Console.WriteLine(er.StackTrace);
            }

            return response;
        }

        private void startReader()
        {
            Thread thread = new Thread(run);
            thread.Start();
        }

        private void closeConnection()
        {
            finished = true;
            try
            {
                stream.Close();
                connection.Close();
                waitHandle.Close();
                client = null;
            }
            catch(ServerException er)
            {
                Console.WriteLine(er.StackTrace);
            }
        }


        private bool isUpdate(Response response)
        {
            return response.type == ResponseType.NEW_SUBMIT;
        }

        private void handleUpdate(Response response)
        {
            SampleDTO[] samples = (SampleDTO[])response.data;
            Console.WriteLine("Office submited from handleUpdate invoked");
            try
            {
                client.OfficeSubmitted(samples);
            }catch(ServerException e)
            {
                Console.WriteLine(e.StackTrace);
            }
                
        }


        public virtual void login(OfficeDTO officeDTO, Observer client)
        {
            initializeConnection();
            Request request = new Request(RequestType.LOGIN, officeDTO);
            Console.WriteLine(request.data.ToString());
            sendRequest(request);
            Response response = readResponse();
            if(response.type == ResponseType.OK)
            {
                this.client = client;
                return;
            }

            if(response.type ==  ResponseType.ERROR)
            {
                string error = response.data.ToString();
                closeConnection();
                throw new ServerException("Error at login " + error);
            }
        }

        public virtual void logout(OfficeDTO officeDTO, Observer client)

        {
            Request request = new Request(RequestType.LOGOUT, officeDTO);
            sendRequest(request);
            Response response = readResponse();
            closeConnection();
            if(response.type ==  ResponseType.ERROR)
            {
                string error = response.data.ToString();
                throw new ServerException("Error at logout " + error);
            }
        }


        public virtual SampleDTO[] getCurrentSamples()
        {
            Request request = new Request(RequestType.GET_CURRENT_SAMPLES);
            sendRequest(request);
            Response response = readResponse();
            if(response.type == ResponseType.ERROR)
            {
                string errror = response.data.ToString();
                throw new ServerException("Error gettin all samples " + errror);
            }
            return (SampleDTO[])response.data;
        }

        public virtual RegistrationDTO[] searchBySample(StyleDTO styleDTO)
        {
            Request request = new Request(RequestType.SEARCH_BY_SAMPLE, styleDTO);
            sendRequest(request);
            Response respone = readResponse();
            if( respone.type == ResponseType.ERROR)
            {
                string error = respone.data.ToString();
                throw new ServerException("Error getting samples by searching after one " + error);
            }
            return (RegistrationDTO[])respone.data;
        }

        public virtual void submitRegistration(InfoSubmitDTO infoSubmitDTO)
        {
            Request request = new Request(RequestType.SUBMIT_REGISTRATION, infoSubmitDTO);
            sendRequest(request);
            Response response = readResponse();
            if(response.type == ResponseType.OK)
            {
                Console.WriteLine("Succesfully submited");
            }
            if(response.type == ResponseType.ERROR)
            {
                string error = response.data.ToString();
                throw new ServerException("Error at submiting " + error);
            }

        }

        public virtual void run()
        {
            while(!finished)
            {
                try
                {
                    object response = formatter.Deserialize(stream);
                    Console.WriteLine("Response recieved " + response);
                    if(isUpdate((Response) response))
                    {
                        handleUpdate((Response)response);
                    }
                    else
                    {
                        lock(responses)
                        {
                            responses.Enqueue((Response)response);
                        }
                        waitHandle.Set();
                    }
                }catch(ServerException err)
                {
                    Console.WriteLine("Readin error " + err);
                }
                catch(System.IO.IOException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                catch (System.Runtime.Serialization.SerializationException e)
                {
                    Console.WriteLine(e.StackTrace);
                }

            }
        }

    }
}
