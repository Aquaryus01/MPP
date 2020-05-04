using Domain.domain;
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
    public partial class MainForm : Form
    {
        private readonly ClientController clientContoller;
        private List<SampleDTO> samples;
        private List<RegistrationDTO> registrations;

        public MainForm(ClientController client)
        {
            InitializeComponent();
            this.clientContoller = client;
            this.samples = clientContoller.getCurrentSamples();
            initialize();
            clientContoller.updateEvent += Update;
        }

        private void setComboBox()
        {
            comboBoxDistance.DataSource = Enum.GetValues(typeof(Distance));
            comboBoxStyle.DataSource = Enum.GetValues(typeof(Style));

        }


        private void initialize()
        {

            setComboBox();
            dataGridViewSamples.DataSource = clientContoller.getCurrentSamples();
            //Distance distance = (Distance)Enum.Parse(typeof(Distance), comboBoxDistance.Text);
            //Style style = (Style)Enum.Parse(typeof(Style), comboBoxStyle.Text);
            //StyleDTO styleDTO = new StyleDTO(distance, style);
            //dataGridViewRegistration.DataSource = clientContoller.searchBySample(styleDTO);

        }

        private void updateTabelList(DataGridView dataGridView, List<SampleDTO> newdata)
        {

            dataGridView.DataSource = null;
            dataGridView.DataSource = newdata;


        }

        public delegate void UpdateTabelListCallback(DataGridView dataGridView, List<SampleDTO> data);

        public void Update(object sender, UserEventArgs e)
        {
            if (e.EventType == Event.NEW_CLIENT)
            {
                SampleDTO[] dTOBJCursa = (SampleDTO[])e.Data;
                this.samples.Clear();
                foreach (SampleDTO c in dTOBJCursa)
                {
                    this.samples.Add(c);
                }
                dataGridViewSamples.BeginInvoke(new UpdateTabelListCallback(this.updateTabelList), new Object[] { dataGridViewSamples, this.samples });
            }
        }

        private void handleSearch(object sender, EventArgs e)
        {
            Distance distance = (Distance)Enum.Parse(typeof(Distance), comboBoxDistance.Text);
            Style style = (Style)Enum.Parse(typeof(Style), comboBoxStyle.Text);
            Console.WriteLine(distance + " " +style);
            StyleDTO styleDTO = new StyleDTO(distance, style);
            styleDTO.Distance = distance;
            styleDTO.Style = style;
            Console.WriteLine(styleDTO.ToString());
           // dataGridViewRegistration.DataSource = clientContoller.searchBySample(styleDTO);
            this.registrations = clientContoller.searchBySample(styleDTO); ;
            this.dataGridViewRegistration.DataSource = this.registrations;
        }

        private void handleSubmit(object sender, EventArgs e)
        {
            String firstName = this.firstNameField.Text;
            string lastName = this.lastNameField.Text;
            int age = Int32.Parse(this.ageField.Text);
            Distance distance = (Distance)Enum.Parse(typeof(Distance), comboBoxDistance.Text);
            Style style = (Style)Enum.Parse(typeof(Style), comboBoxStyle.Text);

            InfoSubmitDTO infoSubmit = new InfoSubmitDTO(firstName, lastName, age, distance, style);
            clientContoller.submitRegistration(infoSubmit);
            MessageBox.Show("Registration saved succesfully");
            this.firstNameField.Text = "";
            this.lastNameField.Text = "";
            this.ageField.Text = "";
        }

        private void LogoutBttnClick(object sender, EventArgs e)
        {
            clientContoller.logout();
            Application.Exit();
        }

    }
}
