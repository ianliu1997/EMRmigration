using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1.RegisrationService;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //


        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            PatientRegistrationSoapClient client = new PatientRegistrationSoapClient();
            string strMRNO = "";

            strMRNO = client.SaveRegistration(1, DateTime.Now, "FirstName", "", "LastName", "LastName", 1, 1, Convert.ToDateTime("11/25/1982"), 1, 1, 1, "", 0, 0, "", "", 0, "",
                                    "","","","","","","","","","","");
             
            MessageBox.Show(strMRNO);


        }

    }
}
