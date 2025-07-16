using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PalashDynamics.ValueObjects;
using PalashDynamics.BusinessLayer;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer;
using System.Configuration;
//using PalashDynamics.Web;
using System.Data;
using PalashDynamics.BusinessLayer.User;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using System.Management;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Drawing;
using PalashDynamics.Web;




namespace PalashDynamics
{
    public partial class frmLogin : System.Web.UI.Page
    {
        #region Public Variables
                
        public bool isChangedFirstPassword = false;
        public bool Validated = false;
        public static int count;
        public int ThresholdValue;
        public bool FillUnitFlag = false;
        public string defaultUnit;
        public bool ChkAccessKey = false;
        public string PIDName;
        public string PIDValue;
        public string HDDSerialNo;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
           // Session["AccessKeyStatus"] = ChkAccessKey;                       
            if (!IsPostBack)
            {
               chkRegistration();            
                if (Validated == true)
                {
                    ViewState["counter"] = 0;
                    chkHeadOffice();
                    ddlUnits.Enabled = true;
                    //ddlUnits.Enabled = false;
                    txtLoginName.Focus();
                    count = 0;
                }
                else
                {
                    Response.Redirect("ValidationError.aspx");
                }                
            }
            else
            {
            }
        }
       
        private void chkRegistration()
        {
            try
            {
                string s;
                DataTemplateService ReadFile = new DataTemplateService();
                string FileName = "CIMS.txt";
                s = ReadFile.GetActivationFile(FileName);
                if (s == "Development")
                {
                    Validated = true;
                }
                else
                {
                    InsertInfo("Win32_Processor", true);
                    HDDSerialNo = GetHDDSerialNumber("C");

                    clsUnitMasterVO objUnit = new clsUnitMasterVO();

                    clsGetLicenseDetailsBizActionVO objAddLicense = new clsGetLicenseDetailsBizActionVO();
                    objAddLicense.UnitDetails = objUnit;

                    PalashDynamicsWeb service = new PalashDynamicsWeb();
                    objAddLicense = (clsGetLicenseDetailsBizActionVO)service.Process(objAddLicense, new clsUserVO());

                    if (objAddLicense.UnitDetails.Description == null || objAddLicense.UnitDetails.Description == " ")
                    {
                        Response.Redirect("PalashRegistration.aspx");
                    }
                    else if (objAddLicense.K2 == "True")
                    {
                        Response.Redirect("PalashActivation.aspx");
                    }
                    else if (objAddLicense.K2 == "False")
                    {
                        ReadActivationKey();
                    }
                }            
            }
            catch (Exception ex)
            {
                Response.Write("Service Unavailable. Please Contact Your Vendor.");
                throw ex;
            }           
        }

        private void ReadActivationKey()
        {
            try
            {
                string s;
                string[] TempArray;
                DataTemplateService ReadFile = new DataTemplateService();
                string FileName = "Activation.txt";
                s = ReadFile.GetActivationFile(FileName);
                string ActivationKey;
                ActivationKey = EncryptDecryptUserKey(s, true);
                //Continue to read until you reach end of file
                if (ActivationKey != null)
                {
                    //chk if the activation key is valid.
                    TempArray = ActivationKey.Split('|');

                    string strPath = "Software\\Microsoft\\CIMS";
                    string strKey;
                  //  RegistryKey regKeyAppRoot = Registry.CurrentUser.CreateSubKey(strPath);
                    RegistryKey regKeyAppRoot = Registry.ClassesRoot.CreateSubKey(strPath); //.CurrentUser.CreateSubKey(strPath);
                    strKey = (string)regKeyAppRoot.GetValue("CIMSv1.0");

                    if (strKey == s)
                    {
                        if (TempArray[4] == HDDSerialNo && TempArray[5] == PIDValue && TempArray[0] == "True")
                            Validated = true;
                        else
                            Validated = false;
                    }
                    else
                    {
                        Response.Redirect("PalashRegistration.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Service Unavailable. Please Contact Your Vendor.");
                throw ex;
            }           
        }

        private void GetRegistryKey()
        {
            //label1.Text = strKey;
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
                                    item.SubItems.Add("No Information Available");
                            }
                        }
                        lst.Items.Add(item);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Can't Get Data Because Of The Followeing Error :-  \n" + exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);                
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

        public static string EncryptDecryptUserKey(string sUserKey, bool bFlag = false)
        {
            int i = 0;
            string s = null;
           // string DbTp = null;
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

        private void chkHeadOffice()
        {
            try
            {
                clsAppConfigVO objConfig = new clsAppConfigVO();
                clsGetAppConfigBizActionVO objGetconfig = new clsGetAppConfigBizActionVO();
                  objGetconfig.AppConfig = objConfig;              
  
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                objGetconfig = (clsGetAppConfigBizActionVO)service.Process(objGetconfig, new clsUserVO());

                if ( objGetconfig.Error == null && objGetconfig.AppConfig != null )
                {
                    //if (objGetconfig.AppConfig.SubsciptionEndDate == null)
                    //{
                    //    Response.Write("Your subscription has not been started yet, Please contact Administration");
                    //    ddlUnits.Enabled = false;
                    //    btn.Enabled = false;
                    //    lnkForgotPassword.Enabled = false;
                    //}
                    //else if (DateTime.Now.Date > objGetconfig.AppConfig.SubsciptionEndDate)
                    //{
                    //    Response.Write("Your subscription has been ended, Please renew your subscrition");
                    //    ddlUnits.Enabled = false;
                    //    btn.Enabled = false;
                    //    lnkForgotPassword.Enabled = false;
                    //}
                    //else
                        if (objGetconfig.AppConfig.IsHO == true)
                    {
                        ddlUnits.Enabled = true;
                        defaultUnit = objGetconfig.AppConfig.UnitName;
                        FillUnits();
                    }
                    else
                    {
                        defaultUnit = objGetconfig.AppConfig.UnitName;
                        FillUnitFlag = true;
                        FillUnits();
                        ddlUnits.Enabled = false;
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
       
        private void DefaultUnit()
        { 
        }
        
        private void FillUnits()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            try
            {                
                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                BizAction = (clsGetMasterListBizActionVO)service.Process(BizAction, new clsUserVO());
                ddlUnits.DataSource = null;
                if (BizAction.MasterList != null)
                {
                    ddlUnits.DataSource = BizAction.MasterList;
                    ddlUnits.DataTextField = "Description";
                    ddlUnits.DataValueField = "ID";
                    ddlUnits.DataBind();
                    
                    if (FillUnitFlag == false)
                    {
                         //ddlUnits.SelectedValue = BizAction.MasterList[0].Description;
                        var result = from r in ((List<MasterListItem>)ddlUnits.DataSource)
                                     where r.Description == defaultUnit
                                     select r;

                        if (result != null)
                        {
                            ddlUnits.SelectedValue = ((MasterListItem)result.First()).ID.ToString();
                        }
                    }
                    else
                    {
                        var result = from r in ((List<MasterListItem>)ddlUnits.DataSource)  where r.Description == defaultUnit
                                     select r;

                        if (result != null)
                        {
                            ddlUnits.SelectedValue = ((MasterListItem)result.First()).ID.ToString();
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(BizAction.Error);
               // throw ex;                            
            }
        }

        protected void OK_Click(object sender, EventArgs e)
        {            
            bool IsValid = true;
            clsUserVO user = null;
            clsUserVO LockedUser = null;
            clsUserVO getAuthenticUser = null;
            List<MasterListItem> SelectedUnit = null;
            clsGetUserListBizActionVO obj = null;

            if (txtLoginName.Text.Trim() == "")
            {
                Error.Text = "Login Name Cannot Be Blank";
                txtLoginName.Focus();
                IsValid = false;
            }
            else if (txtPassword.Text.Trim() == "")
            {
                Error.Text = "Password Cannot Be Blank";
                txtPassword.Focus();
                IsValid = false;
            }
            else if ( ddlUnits.SelectedValue == "") 
            //else if (Convert.ToInt64(ddlUnits.SelectedValue) == 0)
            {
                Error.Text = "Unit Not Selected";
                IsValid = false;
            }            
            if (IsValid)
            {
               // long SelectedUnitId = ddlUnits.SelectedIndex;
                //string SelectedUnit = ddlUnits.SelectedValue;
                long SelectedUnitId = Convert.ToInt64(ddlUnits.SelectedValue);
                //SelectedUnit = (List<MasterListItem>)ddlUnits.SelectedItem; //(int64)ddlUnits.SelectedValue;
                user = UserAuthenticationBizAction.GetInstance().AuthenticateUser(txtLoginName.Text.Trim(), txtPassword.Text.Trim());
                    if (user != null)
                    { //Login Name is Valid.
                        getAuthenticUser = LoginAuthenticationBizAction.GetInstance().LoginAuthenticate(txtLoginName.Text.Trim(), txtPassword.Text.Trim(), SelectedUnitId);
                        //if (string.Compare(txtPassword.Text.Trim(), getAuthenticUser.Password) == 0)
                        if (getAuthenticUser != null)
                        {
                            //if(txtPassword.Text.Trim().Equals(getAuthenticUser.Password)
                            //if (txtLoginName.Text.Trim() == "sa" && txtPassword.Text.Trim() == "sa")
                            if (txtLoginName.Text.Trim() == "sa" && txtPassword.Text.Trim().Equals(getAuthenticUser.Password))
                            {
                                string UserMachineName = "";
                                string UserWindowsLogin = "";
                                #region Commented to check the problem in UserInformation Service
                                ////UserInformation userInfo = new UserInformation();

                                ////UserMachineName = userInfo.GetClientMachineName();
                                ////UserWindowsLogin = userInfo.GetClientUserName();

                                ////getAuthenticUser.UserLoginInfo.Name = user.LoginName;
                                ////getAuthenticUser.UserLoginInfo.UnitName = ddlUnits.SelectedItem.ToString();
                                ////getAuthenticUser.UserLoginInfo.UnitId = Convert.ToInt64(ddlUnits.SelectedValue);
                                ////getAuthenticUser.UserLoginInfo.MachineName = UserMachineName;
                                ////getAuthenticUser.UserLoginInfo.WindowsUserName = UserWindowsLogin;
                                #endregion

                                getAuthenticUser.UserLoginInfo.Name = txtLoginName.Text.Trim();
                                getAuthenticUser.UserLoginInfo.UnitName = ddlUnits.SelectedItem.ToString();
                                getAuthenticUser.UserLoginInfo.UnitId = Convert.ToInt64(ddlUnits.SelectedValue);
                                getAuthenticUser.UserLoginInfo.MachineName = System.Environment.MachineName;
                                getAuthenticUser.UserLoginInfo.WindowsUserName = System.Environment.UserName;
                                Error.Text = "";
                                ViewState["counter"] = 0;
                                Session["AuditId"] = getAuthenticUser.UserLoginInfo.RetunUnitId;
                                getAuthenticUser.UserLoginInfo.UnitId = SelectedUnitId;
                                Session["USER"] = getAuthenticUser;
                                Session["Report"] = "Test";
                                //Response.Write("<script language=javascript>");
                                //Response.Write("window.open('PalashDynamicsTestPage.aspx', '', 'resizable=no, toolbar=no, menubar=no, left=0, top=0, height=710, width=1500');");
                                //Response.Write("window.opener=null; window.close();");
                                //Response.Write("</script>");
                                Response.Redirect("PalashDynamicsTestPage.aspx");
                            }
                            else
                            {
                                if (getAuthenticUser.UserGeneralDetailVO.Locked == false && getAuthenticUser.UserGeneralDetailVO.Status == true)
                                {
                                    for (int i = 0; i < getAuthenticUser.UserGeneralDetailVO.UnitDetails.Count; i++)
                                    {
                                         //getAuthenticUser.UserGeneralDetailVO.UnitDetails[i].IsDefault == true)
                                        if (ddlUnits.SelectedValue == getAuthenticUser.UserGeneralDetailVO.UnitDetails[i].UnitID.ToString()) // && getAuthenticUser.UserGeneralDetailVO.UnitDetails[i].IsDefault == true)
                                        {
                                            string UserMachineName = "";
                                            string UserWindowsLogin = "";

                                            #region Commented to check the problem in UserInformation Service
                                            ////UserInformation userInfo = new UserInformation();

                                            ////UserMachineName = userInfo.GetClientMachineName();
                                            ////UserWindowsLogin = userInfo.GetClientUserName();

                                            ////getAuthenticUser.UserLoginInfo.Name = user.LoginName;
                                            ////getAuthenticUser.UserLoginInfo.UnitName = ddlUnits.SelectedItem.ToString();
                                            ////getAuthenticUser.UserLoginInfo.UnitId = Convert.ToInt64(ddlUnits.SelectedValue);
                                            ////getAuthenticUser.UserLoginInfo.MachineName = UserMachineName;
                                            ////getAuthenticUser.UserLoginInfo.WindowsUserName = UserWindowsLogin;
                                            #endregion

                                            getAuthenticUser.UserLoginInfo.Name = user.LoginName;
                                            getAuthenticUser.UserLoginInfo.UnitName = ddlUnits.SelectedItem.ToString();
                                            getAuthenticUser.UserLoginInfo.UnitId = getAuthenticUser.UserGeneralDetailVO.UnitDetails[i].UnitID;
                                            getAuthenticUser.UserLoginInfo.MachineName = Environment.MachineName;
                                            getAuthenticUser.UserLoginInfo.WindowsUserName = Environment.UserName;
                                            Error.Text = "";
                                            ViewState["counter"] = 0;
                                            Session["USER"] = getAuthenticUser;
                                            Session["Report"] = "Test";
                                            //Response.Write("<script language=javascript>");
                                            //Response.Write("window.open('PalashDynamicsTestPage.aspx', '', 'resizable=no, toolbar=no, menubar=no, left=0, top=0, height=710, width=1500');");
                                            //Response.Write("window.opener=null; window.close();");
                                            //Response.Write("</script>");
                                            Response.Redirect("PalashDynamicsTestPage.aspx");
                                        }
                                        else
                                        {
                                            Error.Text = "User Not Assigned For The Unit.";
                                            Error.Visible = true;
                                        }
                                    }
                                }
                                else if (getAuthenticUser.UserGeneralDetailVO.Locked == true)
                                {
                                    Error.Text = "User Is Locked. Please Contact Your Administrator.";
                                    Error.Visible = true;
                                }
                                else if (getAuthenticUser.UserGeneralDetailVO.Status == false)
                                {
                                    Error.Text = "User Is Deactivated. Please Contact Your Administrator.";
                                    Error.Visible = true;
                                }
                            }                            
                        }
                        else
                        {
                            ViewState["counter"] = Convert.ToInt32(ViewState["counter"]) + 1;
                            ThresholdValue = user.PassConfig.AccountLockThreshold;
                            Error.Text = "Invalid Password.";
                            txtPassword.Text = "";
                            txtPassword.Focus();
                            if (Convert.ToInt32(ViewState["counter"]) >= ThresholdValue)
                            {
                                ViewState["counter"] = 0;
                                Error.Text = "Your Account Has Been Locked. Please Contact Your Admin.";
                                LockedUser = LockUserBizAction.GetInstance().LockUser(user.ID);
                            }
                        }
                    }
                    else
                    {
                        Error.Text = "Invalid User.";
                        txtLoginName.Text = "";
                        txtPassword.Text = "";
                        txtLoginName.Focus();
                    }                                                       
            }
            else
            {
            }
        }      
    }
}
