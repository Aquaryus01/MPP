using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class LoginForm : Form
    {
        private ClientController clientController;
        public LoginForm(ClientController clientController)
        {
            InitializeComponent();
            this.clientController = clientController;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string user = usernameTextField.Text;
            string password = passwordTextField.Text;
            try
            {
                clientController.login(user, password);
                MainForm mainForm = new MainForm(clientController);
                mainForm.Text = "Window for " + user;
                mainForm.Show();
                this.Hide();
            }
            catch(Exception ex)
            {

                MessageBox.Show(this, "Login error" + ex.Message + " " + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
        }

    }
    }
}
