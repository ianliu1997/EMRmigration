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

namespace PalashDynamics.Web
{
    public partial class Loginold : System.Web.UI.Page
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

        public bool ChkHoLogin = false;

        public static bool IsCounterLogin;
       // public static long DefaultCounterID;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // Session["AccessKeyStatus"] = ChkAccessKey;                       
            if (!IsPostBack)
            {
                CashCout.Visible = false;
                CashCout.Disabled = true;

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
        //clsGetAppConfigBizActionVO objGetconfig1 = new clsGetAppConfigBizActionVO();
        private void chkHeadOffice()
        {
         
            try
            {
                clsAppConfigVO objConfig = new clsAppConfigVO();
                clsGetAppConfigBizActionVO objGetconfig = new clsGetAppConfigBizActionVO();
                objGetconfig.AppConfig = objConfig;

                PalashDynamicsWeb service = new PalashDynamicsWeb();
                objGetconfig = (clsGetAppConfigBizActionVO)service.Process(objGetconfig, new clsUserVO());

                if (objGetconfig.Error == null && objGetconfig.AppConfig != null)
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


                    //rohinee for set cash counter from app config
                    IsCounterLogin = objGetconfig.AppConfig.IsCounterLogin;
                    //DefaultCounterID = objGetconfig.AppConfig.DefaultCounterID;
                    //objGetconfig1 = (clsGetAppConfigBizActionVO)objGetconfig.AppConfig;
                    //......
                    ChkHoLogin = objGetconfig.AppConfig.IsHO;
                    if (objGetconfig.AppConfig.IsHO == true)
                    {
                        ddlUnits.Enabled = true;
                        defaultUnit = objGetconfig.AppConfig.UnitName;
                        FillUnits();
                        //FillCashCounter();

                    }
                    else
                    {
                        defaultUnit = objGetconfig.AppConfig.UnitName;
                        FillUnitFlag = true;
                        FillUnits();
//                       FillCashCounter();
                        ddlUnits.Enabled = false;
                        CashCout.Visible = true;
                        CashCout.Disabled = false;
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


        private void FillCashCounter(long CinicID)
        {
            clsGetCashCounterDetailsByClinicIDBizActionVO BizAction = new clsGetCashCounterDetailsByClinicIDBizActionVO();
            BizAction.ClinicID = CinicID;
            BizAction.ListCashCODetails = new List<clsCashCounterVO>();

            PalashDynamicsWeb service = new PalashDynamicsWeb();
            BizAction = (clsGetCashCounterDetailsByClinicIDBizActionVO)service.Process(BizAction, new clsUserVO());
            ddlCashCounter.DataSource = null;

            if (BizAction.ListCashCODetails != null)
            {
                List<clsCashCounterVO> objList = new List<clsCashCounterVO>();               

                if (BizAction.ListCashCODetails != null)
                {
                    //if (BizAction.ListCashCODetails.Count > 0)
                    //{
                    //    obj.Id = 0;
                    //    obj.Description = "-- Select --";
                    //    objList.Add(obj);

                    //    foreach (clsCashCounterVO item in BizAction.ListCashCODetails)
                    //    {
                    //        clsCashCounterVO obj = new clsCashCounterVO();
                    //        obj.Id = item.Id;
                    //        obj.Description = item.Description;
                    //        objList.Add(obj);
                    //    }
                    //}

                    for (int i = 0; i < BizAction.ListCashCODetails.Count; i++)
                    {
                        clsCashCounterVO obj = new clsCashCounterVO();
                        if (i == 0)
                        {
                            obj.Id = 0;
                            obj.Description = "-- Select --";
                            objList.Add(obj);
                        }
                        obj = BizAction.ListCashCODetails[i];
                        objList.Add(obj);
                    }
                }

                ddlCashCounter.DataSource = objList;

                //ddlCashCounter.DataSource = BizAction.ListCashCODetails;
                ddlCashCounter.DataTextField = "Description";
                ddlCashCounter.DataValueField = "ID";
                
                ddlCashCounter.DataBind();
            }            
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
                    Session["UnitList"] = BizAction.MasterList;         // For User rightswise fill Unit List 02032017
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
                        var result = from r in ((List<MasterListItem>)ddlUnits.DataSource)
                                     where r.Description == defaultUnit
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
           // string AccountLockTime ="";
            bool IsValid = true;
            clsUserVO user = null;
            clsUserVO LockedUser = null;
            clsUserVO getAuthenticUser = null;
            List<MasterListItem> SelectedUnit = null;
            //clsGetUserListBizActionVO obj = null;

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
            else if (ddlUnits.SelectedValue == "")
            //else if (Convert.ToInt64(ddlUnits.SelectedValue) == 0)
            {
                Error.Text = "Unit Not Selected";
                IsValid = false;
            }
            else if (ddlUnits.SelectedItem.ToString() != "Head Office")
            {
                if (ddlCashCounter.SelectedIndex == 0)
                {
                    Error.Text = "Cash Counter Not Selected";
                    IsValid = false;
                }
            }
            if (IsValid)
            {
                // long SelectedUnitId = ddlUnits.SelectedIndex;
                //string SelectedUnit = ddlUnits.SelectedValue;
                long SelectedUnitId = Convert.ToInt64(ddlUnits.SelectedValue);
                //SelectedUnit = (List<MasterListItem>)ddlUnits.SelectedItem; //(int64)ddlUnits.SelectedValue;
                user = UserAuthenticationBizAction.GetInstance().AuthenticateUser(txtLoginName.Text.Trim(), txtPassword.Text.Trim());
                # region //By Umesh

                DateTime date = Convert.ToDateTime("1/1/1900 12:00:00 AM");
                if (user != null && user.UserGeneralDetailVO.Locked == true && user.UserGeneralDetailVO.LokedDateTime != date)
                {
                    DateTime CurrentTime = DateTime.Now;
                    int AccLockDuration = Convert.ToInt32(user.PassConfig.AccountLockDuration);
                    var Diff = CurrentTime - user.UserGeneralDetailVO.LokedDateTime; //Convert.ToDateTime(ViewState["AccountLockTime"]);
                    if (Diff.Minutes >= AccLockDuration)
                    {
                        LockedUser = LockUserBizAction.GetInstance().AutoUnLockUser(user.ID);
                        Error.Text = "";
                    }
                }

                # endregion

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

                                        //Added By CDS
                                        if (ChkHoLogin == false && ddlUnits.SelectedItem.ToString() != "Head Office" && IsCounterLogin==true)// && DefaultCounterID == null)
                                        {
                                            getAuthenticUser.UserLoginInfo.CashCounterID = Convert.ToInt64(ddlCashCounter.SelectedValue);
                                            getAuthenticUser.UserLoginInfo.CashCounterName = Convert.ToString(ddlCashCounter.SelectedItem.ToString());
                                        }

                                        //else if (ChkHoLogin == false && ddlUnits.SelectedItem.ToString() != "Head Office" && IsCounterLogin == true && DefaultCounterID != null)
                                        //{
                                        //    getAuthenticUser.UserLoginInfo.CashCounterID = Convert.ToInt64(DefaultCounterID);
                                        //   // getAuthenticUser.UserLoginInfo.CashCounterName = Convert.ToString(ddlCashCounter.SelectedItem.ToString());
                                        //}

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
                        ViewState["LoginCount"] = ViewState["counter"];
                        ThresholdValue = user.PassConfig.AccountLockThreshold;
                        Error.Text = "Invalid Password.";
                        txtPassword.Text = "";
                        txtPassword.Focus();
                        if (Convert.ToInt32(ViewState["counter"]) >= ThresholdValue)
                        {
                            ViewState["counter"] = 0;
                            Error.Text = "Your Account Has Been Locked,Please Contact Your Admin Or Wait For " + Convert.ToInt32(user.PassConfig.AccountLockDuration) + " Mins.";
                          //  ViewState["AccountLockTime"]=DateTime.Now;
                          //  AccountLockTime = Convert.ToString(DateTime.Now);
                            LockedUser = LockUserBizAction.GetInstance().LockUser(user.ID);
                        }
                    }
                }
                else
                {
                    Error.Text = "Invalid Username Or Password";
                    txtLoginName.Text = "";
                    txtPassword.Text = "";
                    txtLoginName.Focus();
                }
            }
            else
            {
            }

           
        }

        //public void FillSearchState(long CountryID)
        //{
        //     clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    try
        //    {
        //        BizAction.IsActive = true;
        //        BizAction.MasterTable = MasterTableNameList.M_CashCounterMaster;
                
        //        BizAction.MasterList = new List<MasterListItem>();
        //        PalashDynamicsWeb service = new PalashDynamicsWeb();
        //        BizAction = (clsGetMasterListBizActionVO)service.Process(BizAction, new clsUserVO());
        //        ddlCashCounter.DataSource = null;
        //        if (BizAction.MasterList != null)
        //        {
        //            //List<MasterListItem> objList = new List<MasterListItem>();
        //            //BizAction.MasterList.Add(new MasterListItem(0, "-- Select --"));                  

        //            Int64 ID = Convert.ToInt64(ddlUnits.SelectedValue);

        //            var result1 = from r in ((List<MasterListItem>)ddlCashCounter.DataSource)
        //                         where r.FilterID == ID
        //                         select r;

        //            if (result1 != null)
        //            {
        //                ddlCashCounter.SelectedValue = ((MasterListItem)result1.First()).ID.ToString();
        //            }
                    

        //            ddlCashCounter.DataSource = BizAction.MasterList;

        //            ddlCashCounter.DataTextField = "Description";
        //            ddlCashCounter.DataValueField = "ID";
        //            ddlCashCounter.DataBind();

        //            if (FillUnitFlag == false)
        //            {
                       
        //                var result = from r in ((List<MasterListItem>)ddlCashCounter.DataSource)
        //                             where r.Description == defaultUnit
        //                             select r;

        //                if (result != null)
        //                {
        //                    ddlCashCounter.SelectedValue = ((MasterListItem)result.First()).ID.ToString();
        //                }
        //            }
        //            else
        //            {
        //                var result = from r in ((List<MasterListItem>)ddlCashCounter.DataSource)
        //                             where r.Description == defaultUnit
        //                             select r;

        //                if (result != null)
        //                {
        //                    ddlCashCounter.SelectedValue = ((MasterListItem)result.First()).ID.ToString();

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(BizAction.Error);
        //        // throw ex;                            
        //    }
        //}

        protected void ddlUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlUnits.SelectedItem != null)
                String Str=ddlUnits.SelectedItem.ToString();
            if (Convert.ToInt64(ddlUnits.SelectedValue) >0 )
            {
                if (ChkHoLogin == true || ddlUnits.SelectedItem.ToString() == "Head Office")
                {
                    // No Cdode 
                    CashCout.Visible = false;
                    CashCout.Disabled = true;
                    btn.Focus();
                }
                else
                {
                    CashCout.Visible = true;
                    CashCout.Disabled = false;

                    if (IsCounterLogin == true)// && DefaultCounterID == null)
                    {
                        FillCashCounter(Convert.ToInt64(ddlUnits.SelectedValue));
                        ddlCashCounter.Focus();
                    }

                    else
                    {
                        CashCout.Visible = false;
                        CashCout.Disabled = true;
                    }
                }
                
            }
        }

        #region For User rightswise fill Unit List

        protected void txtLoginName_TextChanged(object sender, EventArgs e)
        {
            List<MasterListItem> UnitList = ((List<MasterListItem>)Session["UnitList"]);
            if (!string.IsNullOrEmpty(txtLoginName.Text))
            {
                Error.Text = "";
                clsUserVO user = UserAuthenticationBizAction.GetInstance().AuthenticateUser(txtLoginName.Text.Trim(), txtPassword.Text.Trim());

                if (user != null)
                {
                    if (user.UserUnitList != null)
                    {
                        if (UnitList != null)
                        {
                            List<MasterListItem> list = new List<MasterListItem>();
                            foreach (clsUserUnitDetailsVO item in user.UserUnitList)
                            {
                                if (UnitList.SingleOrDefault(S => S.ID.Equals(item.UnitID)) != null)
                                    list.Add(UnitList.SingleOrDefault(S => S.ID.Equals(item.UnitID)));
                            }
                            ddlUnits.DataSource = list;
                            ddlUnits.DataTextField = "Description";
                            ddlUnits.DataValueField = "ID";
                            ddlUnits.DataBind();

                            if (user.UserUnitList.Count > 0)
                            {
                                long IsDefaultUnitID = 0;
                                if ((user.UserUnitList.SingleOrDefault(S => S.IsDefault.Equals(true))) != null)
                                {
                                    IsDefaultUnitID = (user.UserUnitList.SingleOrDefault(S => S.IsDefault.Equals(true))).UnitID;
                                }
                                else
                                    IsDefaultUnitID = user.UserUnitList[0].UnitID;

                                ddlUnits.SelectedValue = IsDefaultUnitID.ToString();
                                txtPassword.Focus();
                            }
                        }

                    }
                    else
                    {
                        ddlUnits.DataSource = UnitList;
                        ddlUnits.DataTextField = "Description";
                        ddlUnits.DataValueField = "ID";
                        ddlUnits.DataBind();
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
        }

        #endregion

    }
}