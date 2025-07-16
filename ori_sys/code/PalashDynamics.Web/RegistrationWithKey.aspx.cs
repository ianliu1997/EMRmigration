using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Management;
using System.Windows.Forms;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects;
using System.IO;

namespace PalashDynamics.Web
{
    public partial class RegistrationWithKey : System.Web.UI.Page
    {
        #region Public Variables
        
        public string PIDName;
        public string PIDValue;
        public string HDDSerialNo;
        public string Name;
        public string area;
        public string pincode;
        public string date;
        public string time;
        public string RegKey;
        public string EmailId;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InsertInfo("Win32_Processor", true);
                HDDSerialNo = GetHDDSerialNumber("C");
                generation();
                lblKey.Text = RegKey;
            }
        }

        private void generation()
        {
            clsUnitMasterVO objUnit = new clsUnitMasterVO();
            clsGetLicenseDetailsBizActionVO objAddLicense = new clsGetLicenseDetailsBizActionVO();
            objAddLicense.UnitDetails = objUnit;
            PalashDynamicsWeb service = new PalashDynamicsWeb();
            objAddLicense = (clsGetLicenseDetailsBizActionVO)service.Process(objAddLicense, new clsUserVO());

            if (objAddLicense.UnitDetails.Description != null || objAddLicense.UnitDetails.Description != " ")
            {
                Name = objAddLicense.UnitDetails.Description;
                area = objAddLicense.UnitDetails.Area;
                pincode = objAddLicense.UnitDetails.Pincode;
                date = objAddLicense.sDate;
                time = objAddLicense.sTime;
                EmailId = objAddLicense.UnitDetails.Email;
                LKGeneration();
                LKInserttoDB();
                writeToFile();                
                //Response.Redirect("PalashRegistration.aspx");
            }
            else
            {
                //error in retrieving registration data.
            }
        }

        private void InsertInfo(string Key, bool DontInsertNull)
        {
            System.Windows.Forms.ListView lst = new System.Windows.Forms.ListView(); ;
            lst.Items.Clear();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + Key);
            try
            {
                foreach (ManagementObject share in searcher.Get())
                {
                    ListViewGroup grp;
                    try
                    {
                        grp = lst.Groups.Add(share["Name"].ToString(), share["Name"].ToString());
                    }
                    catch
                    {
                        grp = lst.Groups.Add(share.ToString(), share.ToString());
                    }

                    if (share.Properties.Count <= 0)
                    {
                        MessageBox.Show("No Information Available", "No Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    foreach (PropertyData PC in share.Properties)
                    {

                        System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(grp);

                        item.Text = PC.Name;
                        PIDName = PC.Name;
                        if (PIDName == "ProcessorId")
                        {
                            if (PC.Value != null && PC.Value.ToString() != "")
                            {
                                PIDValue = PC.Value.ToString();
                            }
                            else
                            {
                                if (!DontInsertNull)
                                    item.SubItems.Add("No Information available");
                            }
                        }
                        lst.Items.Add(item);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("can't get data because of the followeing error \n" + exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public string GetHDDSerialNumber(string drive)
        {
            if (drive == "" || drive == null)
            {
                drive = "C";
            }
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + drive + ":\"");
            disk.Get();
            return disk["VolumeSerialNumber"].ToString();
        }

        public void LKInserttoDB()
        {
            clsdbDetailsBizActionVO objAddtoDB = new clsdbDetailsBizActionVO();
            objAddtoDB.Value = RegKey;
            PalashDynamicsWeb service = new PalashDynamicsWeb();
            objAddtoDB = (clsdbDetailsBizActionVO)service.Process(objAddtoDB, new clsUserVO()); 
        }

        public void LKGeneration()
        {
            string key = "";
            string PinKey = "";           
            key = area;
            PinKey = pincode;

            string TempArea;
            string [] keyTemp = new string[4];

            if (key[2].ToString() == null)            
                keyTemp[0] = "P";            
            else
                keyTemp[0] = key[2].ToString();
                        
            if (key[4].ToString() == null)
                keyTemp[1] = "a";
            else
                keyTemp[1] = key[4].ToString();

            if (key[3].ToString() == null)
                keyTemp[2] = "l";
            else
                keyTemp[2] = key[3].ToString();

            TempArea = keyTemp[0] + keyTemp[1] + keyTemp[0] + keyTemp[2];
            //TempArea=key[2].ToString() + key[4].ToString() + key[2].ToString() + key[3].ToString();
            string TempPin;           
            TempPin = PinKey[3].ToString() + PinKey[4].ToString() + PinKey[5].ToString();
            RegKey = Name + '|' + TempArea + '|' + TempPin + '|' + HDDSerialNo + '|' + PIDValue + '|' + date + '|' + time;            
        }

        protected void btnOnline_Click(object sender, EventArgs e)
        {
        }

        protected void btnSHS_Click(object sender, EventArgs e)
        {            
            string fromEmail, ToEmail, Subject, Body;
            fromEmail=EmailId;
            ToEmail= "sailyp@seedhealthcare.com";
            Subject= "LicenseKey";            
            EmailService service = new EmailService();
            service.SendEmail(fromEmail, ToEmail, Subject, RegKey);
            Response.Redirect("RegistrationCompleted.aspx");
        }

        public void writeToFile()
        {
            try
            {
                StreamWriter sw = new StreamWriter("D:\\SailyP\\Razi Clinic\\ActivationKeyNew.txt");
                //Write a line of text
                sw.WriteLine(RegKey);
                //Close the file
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}