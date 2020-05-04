using Networking.rpcProtocol;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class StartClient
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IServices server = new ClientProxy("127.0.0.1", 55555);
            ClientController ctrl = new ClientController(server);
            LoginForm loginForm = new LoginForm(ctrl);
            Application.Run(loginForm);
        }

    }

   
}

