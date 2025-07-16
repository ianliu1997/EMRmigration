using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects;
using PalashDynamics.Web.PalashActivationRef;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using PalashDynamics.ValueObjects.Administration.Menu;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Web
{
    public partial class PalashActivation : System.Web.UI.Page
    {
        public long Code;
        public string ActivationKey;
        string TempString;

        protected void Page_Load(object sender, EventArgs e)
        {                           
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                clsUnitMasterVO objUnit = new clsUnitMasterVO();
                clsGetLicenseDetailsBizActionVO objAddLicense = new clsGetLicenseDetailsBizActionVO();
                objAddLicense.UnitDetails = objUnit;
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                objAddLicense = (clsGetLicenseDetailsBizActionVO)service.Process(objAddLicense, new clsUserVO());
                Code = objAddLicense.Id;
                Service1 ActivationService = new Service1();
                ActivationKey = ActivationService.GetActivationKey(Code);
                if (!string.IsNullOrEmpty(txtKey1.Text) && !string.IsNullOrEmpty(txtKey2.Text) && !string.IsNullOrEmpty(txtKey3.Text) && !string.IsNullOrEmpty(TxtKey4.Text))
                {
                    string concateKey, key1, key2, key3, key4;
                    key1 = txtKey1.Text.ToString();
                    key2 = txtKey2.Text.ToString();
                    key3 = txtKey3.Text.ToString();
                    key4 = TxtKey4.Text.ToString();
                    concateKey = key1 + ' ' + key2 + ' ' + key3 + ' ' + key4;

                    if (concateKey == ActivationKey)
                    {
                        AppendToFile(true);
                        SetRegistryKey();                     
                        lblError.Text = "";
                        txtKey1.Enabled = false;
                        txtKey2.Enabled = false;
                        txtKey3.Enabled = false;
                        TxtKey4.Enabled = false;
                        lblSucess.Visible = true;
                        lnkLogin.Visible = true;
                    }
                    else 
                    {
                        lblError.Text = "Invalid Activation Key."; //actually incomplete.
                        lblError.Visible = true;
                    }                    
                }
                else
                {
                    lblError.Text = "Invalid Activation Key."; //actually incomplete.
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {                
                lblError.Text = "Activation Service Unavailable. Please Try After Some Time Or Contact Your Vendor."; //actually incomplete.
                lblError.Visible = true;
                throw ex;
            }          
        }

        private void SetRegistryKey()
        {
            try
            {
                string strPath = "Software\\Microsoft\\CIMS";
               // RegistryKey regKeyAppRoot = Registry.CurrentUser.CreateSubKey(strPath);
                RegistryKey regKeyAppRoot = Registry.ClassesRoot.CreateSubKey(strPath);  //.CurrentUser.CreateSubKey(strPath);
                regKeyAppRoot.SetValue("CIMSv1.0", TempString);
                UpdateDB();               
                //label1.Text = "Key set";
            }
            catch (Exception ex)
            {
                lblError.Text = "Error Occured with System Registry. Administrator Rights Not Assigned To The User. Cannot Complete Activation Process.";
                lblError.Visible = true;
                AppendToFile(false);
                throw ex;
            }            
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
           // Response.Redirect("PalashErrorForm.aspx");           
        }

        private void chkActivationKey()
        {
        }
              
        private void UpdateDB()
        {
            try
            {
                long UpdateActivationDate;
                clsAddLicenseToBizActionVO objAddLicense = new clsAddLicenseToBizActionVO();
                objAddLicense.K1 = "True";
                objAddLicense.K2 = "False";
                DateTime ActivationDate = DateTime.Now;
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                objAddLicense = (clsAddLicenseToBizActionVO)service.Process(objAddLicense, new clsUserVO());

                Service1 ActivationService = new Service1();
                UpdateActivationDate = ActivationService.UpdateClientActivationTime(ActivationDate, Code);                              
            }
            catch (Exception ex)
            {
                Response.Write("Activation Service Unavailable. Please Try After Some Time Or Contact Your Vendor.");
                throw ex;
            }
        }
            
        public void AppendToFile(bool Trans)
        {
            string WriteStatus;
            try
            {
                // Read.
                string s;
                DataTemplateService ReadFile = new DataTemplateService();
                string FileName = "Activation.txt";
                s = ReadFile.GetActivationFile(FileName);
                //Append.                              
                if (Trans == true)
                {
                    TempString = EncryptDecryptUserKey(s, true);
                    string[] TempVar = TempString.Split('|');
                    TempVar[0] = "True";
                    TempString = TempVar[0] + '|' + TempVar[1] + '|' + TempVar[2] + '|' + TempVar[3] + '|' + TempVar[4] + '|' + TempVar[5] + '|' + TempVar[6] + '|' + TempVar[7];
                    TempString = EncryptDecryptUserKey(TempString);
                    WriteStatus = ReadFile.ActivationFile(TempString);
                   if (WriteStatus == "Error")
                   {
                       lblError.Text = "Unable To Write To File.";
                       lblError.Visible = true;
                   }
                }
                else 
                {
                    TempString = EncryptDecryptUserKey(s, true);
                    string[] TempVar = TempString.Split('|');
                    TempVar[0] = "False";
                    TempString = TempVar[0] + '|' + TempVar[1] + '|' + TempVar[2] + '|' + TempVar[3] + '|' + TempVar[4] + '|' + TempVar[5] + '|' + TempVar[6] + '|' + TempVar[7];
                    TempString = EncryptDecryptUserKey(TempString);

                    WriteStatus = ReadFile.ActivationFile(TempString);
                   if (WriteStatus == "Error")
                   {
                       lblError.Text = "Unable To Write To File.";
                       lblError.Visible = true;
                   }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string EncryptDecryptAccessKey(string sAccessKey, bool bFlag = false)
            {
                int i = 0;
                string s = null;
                s = "";
                for (i = 1; i <= sAccessKey.Length; i++)
                {
                    if (bFlag == false)
                    {
                        s = s + Strings.Chr((Strings.Asc(Strings.Mid(sAccessKey, i, 1) + 100)) / 2 + i);
                    }
                    else
                    {
                        int a = Convert.ToInt32(Strings.Mid(sAccessKey, i, 1));
                        string first = Convert.ToString(a - i);
                        s = s + Strings.Chr((Strings.Asc(first) * 2 - 100));
                    }
                }
                return s;
            }

        public static string EncryptDecryptUserKey(string sUserKey, bool bFlag = false)
            {
                int i = 0;
                string s = null;
                s = "";
                for (i = 1; i <= sUserKey.Length; i++)
                {
                    if (bFlag == false)
                    {
                        s = s + Strings.Chr(Strings.Asc(Strings.Mid(sUserKey, i, 1)) * 2 - 5);
                        // 97  43
                    }
                    else
                    {
                        s = s + Strings.Chr((Strings.Asc(Strings.Mid(sUserKey, i, 1)) + 5) / 2);
                    }
                }
                return s;
            }    
    }
}