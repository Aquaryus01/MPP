using Networking.rpcProtocol;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class StartClient
    {

        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //IServices server = new ClientProxy("127.0.0.1", 55555);
            //ClientController ctrl = new ClientController(server);
            //LoginForm loginForm = new LoginForm(ctrl);
            //Application.Run(loginForm);
            
            //.NET REMOTING
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
            serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();
            IDictionary props = new Hashtable();

            props["port"] = 0;
            TcpChannel channel = new TcpChannel(props, clientProv, serverProv);
            ChannelServices.RegisterChannel(channel, false);
            IServices services =
                 (IServices)Activator.GetObject(typeof(IServices), "tcp://localhost:55555/Swimming");

            ClientController ctrl = new ClientController(services);
            LoginForm win = new LoginForm(ctrl);
            Application.Run(win);
        }

    }

   
}

