using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using System.Management;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
//using PalashDynamics.Web.PalashActivationKey;
using Microsoft.VisualBasic;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Web.PalashActivationRef;
using PalashDynamics.ValueObjects.Administration;


namespace PalashDynamics.Web
{
    public partial class PalashRegistration : System.Web.UI.Page
    {
        #region Public Variables
        
        public bool saveFlag = false;
        public bool FormValid = true;
        public string PIDName;
        public string PIDValue;
        public string HDDSerialNo;
        public string Name;
        public string area;
        public string pincode;
        public string date;
        public string time;
        public string RegKey;
       
        public DateTime DateToStatic;
        public long Id;
        public string TempString;
        public string Key;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {           
            if (!IsPostBack)
            {
                disable();
                FillUnits();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            checkValidation();
            if (FormValid == true)
            {
                saveFlag = true;
                lblError.Text = "";
                lblError.Visible = false;
                //clsUnitMasterVO objUnit = new clsUnitMasterVO();
                CreateFormData();
                if ((bool)Session["ISHO"] == true)
                {
                    clsAddUnitMasterBizActionVO BizAction = new clsAddUnitMasterBizActionVO();
                    BizAction.UnitDetails = (clsUnitMasterVO)Session["SelectedUnit"];
                    // BizAction.UnitDetails.UnitID = ((clsUnitMasterVO)grdMaster.SelectedItem).UnitID;
                    PalashDynamicsWeb service = new PalashDynamicsWeb();
                    BizAction = (clsAddUnitMasterBizActionVO)service.Process(BizAction, new clsUserVO());

                    if (BizAction.SuccessStatus == 0 && BizAction.UnitDetails != null)
                    {
                        GenerateLicense();
                    }
                    else
                    {
                        lblError.Text = "Error while updating Clinic data";
                        lblError.Visible = true;
                    }                   
                }
                else
                    GenerateLicense();
            }
            else
            {
                lblError.Text = "";
                lblError.Text = "Data Is Not Complete. Cannot Submit Data";
                lblError.Visible = true;
            }               
        }

        private void GenerateLicense()
        {

            DateToStatic = DateTime.Now;
            SelectedUnit = (clsUnitMasterVO)(Session["SelectedUnit"]);
            
            clsAddLicenseToBizActionVO objAddLicense = new clsAddLicenseToBizActionVO();
            objAddLicense.UnitDetails = SelectedUnit;
            objAddLicense.K1 = "False";
            objAddLicense.K2 = "True";
            objAddLicense.sDate = DateToStatic.ToShortDateString();
            objAddLicense.sTime = DateToStatic.ToShortTimeString();

            try
            {
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                objAddLicense = (clsAddLicenseToBizActionVO)service.Process(objAddLicense, new clsUserVO());

                if (objAddLicense.SuccessStatus == 0)
                {
                    InsertInfo("Win32_Processor", true);
                    HDDSerialNo = GetHDDSerialNumber("C");
                    generation();
                    SendToSHS();
                }
                else
                {
                    lblError.Text = "Error Occured While Processing";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {            
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        private void checkValidation()
        {
            FormValid = true;
            //if (string.IsNullOrEmpty(txtAdress1.Text))
            //{
            //    FormValid = false;
            //    txtAdress1.Focus();
            //}
            if (string.IsNullOrEmpty(txtArea.Text))
            {
                FormValid = false;
                txtArea.Focus();
            }
            //if (string.IsNullOrEmpty(txtCity.Text))
            //{
            //    FormValid = false;
            //    txtCity.Focus();
            //}
            if (string.IsNullOrEmpty(txtClinicEmailId.Text))
            {
                FormValid = false;
                txtClinicEmailId.Focus();
            }         
            //if (string.IsNullOrEmpty(txtContactNo.Text))
            //{
            //    FormValid = false;
            //    txtContactNo.Focus();
            //}
            //if (string.IsNullOrEmpty(txtCountry.Text))
            //{
            //    FormValid = false;
            //    txtCountry.Focus();
            //}
            //if (string.IsNullOrEmpty(txtCountryCode.Text))
            //{
            //    FormValid = false;
            //    txtCountryCode.Focus();
            //}

            if ((bool)Session["ISHO"] == true)
            {
                if (string.IsNullOrEmpty(txtHOName.Text))
                {
                    FormValid = false;
                    txtCountryCode.Focus();
                }
            }
            long? SelectedUnitId = Convert.ToInt64(ddlUnits.SelectedValue);

            if (SelectedUnitId != null && SelectedUnitId > 0)
            {
            }
            else
            {
                FormValid = false;
                ddlUnits.Focus();
            }
            //if (string.IsNullOrEmpty(txtDistrict.Text))
            //{
            //    FormValid = false;
            //    txtDistrict.Focus();
            //}
            if (string.IsNullOrEmpty(txtPinCode.Text))
            {
                FormValid = false;
                txtPinCode.Focus();
            }
            //if(string.IsNullOrEmpty(txtState.Text))
            //{
            //    FormValid=false;
            //    txtState.Focus();
            //}
            //if (string.IsNullOrEmpty(txtSTDCode.Text))
            //{
            //    FormValid = false;
            //    txtSTDCode.Focus();
            //}
            //if (string.IsNullOrEmpty(txtTaluka.Text))
            //{
            //    FormValid = false;
            //    txtTaluka.Focus();
            //}           
        }

        private void CreateFormData()
        {
            clsUnitMasterVO objUnitDetails = new clsUnitMasterVO();
            if ((bool)Session["ISHO"] == true)
            {
                objUnitDetails = (clsUnitMasterVO)Session["SelectedUnit"];
                objUnitDetails.Description = txtHOName.Text.Trim();
                objUnitDetails.ResiNoCountryCode = int.Parse(txtCountryCode.Text.Trim());
                objUnitDetails.ResiSTDCode = int.Parse(txtSTDCode.Text.Trim());
                objUnitDetails.ContactNo = txtContactNo.Text.Trim();
                objUnitDetails.FaxNo = txtClinicFaxNo.Text.Trim();
                objUnitDetails.Email = txtClinicEmailId.Text.Trim();
                objUnitDetails.Country = txtCountry.Text.Trim();
                objUnitDetails.State = txtState.Text.Trim();
                objUnitDetails.District = txtDistrict.Text.Trim();
                objUnitDetails.Taluka = txtTaluka.Text.Trim();
                objUnitDetails.City = txtCity.Text.Trim();
                objUnitDetails.Area = txtArea.Text.Trim();
                objUnitDetails.Pincode = txtPinCode.Text.Trim();
                objUnitDetails.AddressLine1 = txtAdress1.Text.Trim();
                objUnitDetails.AddressLine2 = txtAddressLine2.Text.Trim();
                objUnitDetails.AddressLine3 = txtAddressLine3.Text.Trim();
                Session["SelectedUnit"] = objUnitDetails;
            }             
        }

        private void generation()
        {
            LKGeneration();            
            writeToFile();
            if (lblError.Visible == false)
            {
                try
                {
                    Service1 ActivationService = new Service1();
                    SelectedUnit = (clsUnitMasterVO)Session["SelectedUnit"];
                    Id = ActivationService.RegisterClient(SelectedUnit.Description.Trim(), SelectedUnit.AddressLine1, SelectedUnit.AddressLine2.Trim(), SelectedUnit.AddressLine3.Trim(), SelectedUnit.Country.Trim(),
                        SelectedUnit.State.Trim(), SelectedUnit.District.Trim(), SelectedUnit.Taluka.Trim(), SelectedUnit.City.Trim(), SelectedUnit.Area.Trim(), SelectedUnit.Pincode.Trim(), SelectedUnit.ResiNoCountryCode, SelectedUnit.ResiSTDCode,
                        SelectedUnit.ContactNo.Trim(), SelectedUnit.FaxNo.Trim(), SelectedUnit.Email.Trim(), DateToStatic, RegKey);
                    clsUnitMasterVO objUnit = new clsUnitMasterVO();
                    clsUpdateLicenseToBizActionVO objUpdateLicense = new clsUpdateLicenseToBizActionVO();
                    objUpdateLicense.UnitDetails = objUnit;
                    objUpdateLicense.Id = Id;
                    PalashDynamicsWeb service = new PalashDynamicsWeb();

                    objUpdateLicense = (clsUpdateLicenseToBizActionVO)service.Process(objUpdateLicense, new clsUserVO());

                    if (objUpdateLicense.SuccessStatus == 0)
                    {
                        Service1 GenerateKey = new Service1();
                        Key = GenerateKey.GenerateActivationKey(TempString);
                        GenerateKey.UpdateClient(Key, Id);
                        disable();
                        lblKey.Text = Key;
                        lblActivationKey.Visible = true;
                        lblKey.Visible = true;
                        lblKeyNote.Visible = true;
                        btnActivation.Visible = true;
                        //disable();
                    }
                    else
                    {
                        lblError.Text = "Error occured in Registration.";
                        lblError.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    disable();
                    lblError.Text = "Activation Service Unavailable. Please Try After Some Time Or Contact Your Vendor.";
                    lblError.Visible = true;
                }        
            }                        
        }

        private void disable()
        {
            txtAddressLine2.Enabled = false;
            txtAddressLine3.Enabled = false;
            txtAdress1.Enabled = false;
            txtArea.Enabled = false;
            txtCity.Enabled = false;
            txtClinicEmailId.Enabled = false;
            txtClinicFaxNo.Enabled = false;
            //txtCode.Enabled = false;
            txtContactNo.Enabled = false;
            txtCountry.Enabled = false;
            txtCountryCode.Enabled = false;
            //txtDescription.Enabled = false;
            ddlUnits.Enabled = false;
            txtDistrict.Enabled = false;
            txtPinCode.Enabled = false;
            txtState.Enabled = false;
            txtSTDCode.Enabled = false;
            txtTaluka.Enabled = false;
            btnSave.Enabled = false;
            txtHOName.Enabled = false;
            lblActivationKey.Visible = false;
            lblKey.Visible = false;
            lblKeyNote.Visible = false;
            btnActivation.Visible = false;           
        }

        private clsAddLicenseToBizActionVO CreateData()
        {
            clsAddLicenseToBizActionVO obj = new clsAddLicenseToBizActionVO();
            return obj;
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
                MessageBox.Show("Can't Get Data Because Of The Followeing Error :- \n" + exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        //public void LKInserttoDB()
        //{
        //    clsdbDetailsBizActionVO objAddtoDB = new clsdbDetailsBizActionVO();
        //    objAddtoDB.Value = RegKey;

        //    PalashDynamicsWeb service = new PalashDynamicsWeb();
        //    objAddtoDB = (clsdbDetailsBizActionVO)service.Process(objAddtoDB, new clsUserVO());

        //}

        private void FillUnits()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            try
            {
                #region

                #endregion

                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                BizAction = (clsGetMasterListBizActionVO)service.Process(BizAction, new clsUserVO());
                ddlUnits.DataSource = null;
                if (BizAction.MasterList != null)
                {
                    //List<MasterListItem> myList = new List<MasterListItem>();
                    ddlUnits.DataSource = BizAction.MasterList;
                    BizAction.MasterList.Insert(0,new MasterListItem(0,"-- Select --"));
                    ddlUnits.DataTextField = "Description";
                    ddlUnits.DataValueField = "ID";
                    ddlUnits.DataBind();                                     
                    ddlUnits.SelectedValue = BizAction.MasterList[0].Description;
                    ddlUnits.Enabled = true;                       
                }
            }
            catch (Exception ex)
            {
                Response.Write(BizAction.Error);
                throw ex;                 
            }
        }

        public void LKGeneration()
        {           
            string key = "";
            string PinKey = "";
            SelectedUnit = (clsUnitMasterVO)Session["SelectedUnit"];
            key = SelectedUnit.Area;  //txtArea.Text.Trim(); // S
            PinKey = SelectedUnit.Pincode; // txtPinCode.Text.Trim(); //
            Name = SelectedUnit.Description;  //ddlUnits.Text.Trim();  //SelectedUnit.Description;//
            date = DateToStatic.ToShortDateString();
            time = DateToStatic.ToShortTimeString();
            string TempArea;
            string[] keyTemp = new string[4];

            if (key[2] == null)
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

            string TempPin;
            string TempVar = "False";
            TempPin = PinKey[3].ToString() + PinKey[4].ToString() + PinKey[5].ToString();
            
            RegKey = TempVar + '|' + Name + '|' + TempArea + '|' + TempPin + '|' + HDDSerialNo + '|' + PIDValue + '|' + date + '|' + time;
          
            TempString = EncryptDecryptUserKey(RegKey);

        }

        public void SendToSHS()
        {
            string fromEmail, ToEmail, Subject, Body;

            fromEmail = txtClinicEmailId.Text.Trim();
            ToEmail = "nileshi@seedhealthcare.com";
            Subject = "LicenseKey";
            Body = TempString + ' ' + Key;

            EmailService service = new EmailService();
            service.SendEmail(fromEmail, ToEmail, Subject, Body);
        }

        public void writeToFile()
        {
            string WriteStatus;
            try
            {
                DataTemplateService WritToFile = new DataTemplateService();
                byte[] TempArray;

                WriteStatus =  WritToFile.ActivationFile(TempString);
                if (WriteStatus == "Error")
                {
                    lblError.Text = "Unable to Write to File.";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnActivation_Click(object sender, EventArgs e)
        {
            Response.Redirect("PalashActivation.aspx");
        }

        public static string EncryptDecryptUserKey(string sUserKey, bool bFlag = false)
        {
            int i = 0;
            string s = null;
      //     string DbTp = null;
            s = "";
            // for (i = 1; i <= Strings.Len(Strings.Trim(sUserKey)); i++)
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

        protected void ddlUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            long? SelectedUnitId = Convert.ToInt64(ddlUnits.SelectedValue);

            if (SelectedUnitId != null && SelectedUnitId > 0)
            {
                FillData(SelectedUnitId.Value);
            }
            else
            {
                SelectedUnit = new clsUnitMasterVO();
                Session["SelectedUnit"] = SelectedUnit;
            }
        }

        public clsUnitMasterVO SelectedUnit { get; set; }
        private void FillData(long iUnitID)
        {
            clsGetUnitDetailsByIDBizActionVO BizAction = new clsGetUnitDetailsByIDBizActionVO();
            BizAction.Details = new clsUnitMasterVO();
            BizAction.UnitID = iUnitID;
            PalashDynamicsWeb service = new PalashDynamicsWeb();
            BizAction = (clsGetUnitDetailsByIDBizActionVO)service.Process(BizAction, new clsUserVO());

            //clsUnitMasterVO UnitVO = new clsUnitMasterVO();
            if (BizAction.Details != null)
            {
                SelectedUnit = BizAction.Details;
                Session["SelectedUnit"] = SelectedUnit;
                txtCountryCode.Text = SelectedUnit.ResiNoCountryCode.ToString();
                txtSTDCode.Text = SelectedUnit.ResiSTDCode.ToString();
                txtContactNo.Text = SelectedUnit.ContactNo;

                txtAdress1.Text = SelectedUnit.AddressLine1;
                txtAddressLine2.Text = SelectedUnit.AddressLine2;
                txtAddressLine3.Text = SelectedUnit.AddressLine3;
                txtCountry.Text = SelectedUnit.Country;
                txtState.Text = SelectedUnit.State;
                txtDistrict.Text = SelectedUnit.District;
                txtTaluka.Text = SelectedUnit.Taluka;
                txtArea.Text = SelectedUnit.Area;
                txtPinCode.Text = SelectedUnit.Pincode;
                txtClinicEmailId.Text = SelectedUnit.Email;
                txtClinicFaxNo.Text = SelectedUnit.FaxNo;
                txtCity.Text = SelectedUnit.City;               
                btnSave.Enabled = true;
                chkHeadOffice();                
            }
        }


        private void chkHeadOffice()
        {
            try
            {
                clsAppConfigVO objConfig = new clsAppConfigVO();
                clsGetAppConfigBizActionVO objGetconfig = new clsGetAppConfigBizActionVO();
                objGetconfig.AppConfig = objConfig;
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                objGetconfig = (clsGetAppConfigBizActionVO)service.Process(objGetconfig, new clsUserVO());

                if (objGetconfig.AppConfig != null && objGetconfig.Error == null)
                {                   
                     SelectedUnit = (clsUnitMasterVO)Session["SelectedUnit"] ;
                     Session["ISHO"] = objGetconfig.AppConfig.IsHO;
                     if (objGetconfig.AppConfig.IsHO == true && SelectedUnit.UnitID == objGetconfig.AppConfig.UnitID)
                    {                       
                        Enable();
                        txtHOName.Text = SelectedUnit.Description;
                        lblHOName.Visible = true;
                        txtHOName.Visible = true;
                    }
                    else
                    {
                        disable();
                        btnSave.Enabled = true;
                        txtHOName.Text = "";
                        ddlUnits.Enabled = true;
                        lblHOName.Visible = false;
                        txtHOName.Visible = false;
                    }
                }
                else
                {
                    Response.Write(objGetconfig.Error);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void Enable()
        {
            txtAddressLine2.Enabled = true;
            txtAddressLine3.Enabled = true;
            txtAdress1.Enabled = true;
            txtArea.Enabled = true;
            txtCity.Enabled = true;
            txtClinicEmailId.Enabled = true;
            txtClinicFaxNo.Enabled = true;
            //txtCode.Enabled = false;
            txtContactNo.Enabled = true;
            txtCountry.Enabled = true;
            txtCountryCode.Enabled = true;
            //txtDescription.Enabled = false;
            ddlUnits.Enabled = true;
            txtDistrict.Enabled = true;
            txtPinCode.Enabled = true;
            txtState.Enabled = true;
            txtSTDCode.Enabled = true;
            txtTaluka.Enabled = true;
            btnSave.Enabled = true;
            txtHOName.Enabled = true;
            lblActivationKey.Visible = false;
            lblKey.Visible = false;
            lblKeyNote.Visible = false;
            btnActivation.Visible = false;
        }  
    }
}