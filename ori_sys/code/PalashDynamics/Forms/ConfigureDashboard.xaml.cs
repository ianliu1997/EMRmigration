using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics;
using System.Windows.Browser;

namespace CIMS.Forms
{
    public partial class ConfigureDashboard : ChildWindow
    {
        public ConfigureDashboard()
        {
            InitializeComponent();
        }

        public bool isValid = true;
        public string msgText;
        public string msgTitle = "PALASHDYNAMICS";

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            isValid = true;
          
            msgText = "";
            //ClearallFlags();

            //if (OnSaveButton_Click != null)
            //    OnSaveButton_Click(this, new RoutedEventArgs());
            if (isValid)
            {
                clsUserVO objLogin = CreateFormData();

                    msgText = "Are You Sure to Reset the Dashboard Configurations?";

                    MessageBoxControl.MessageBoxChildWindow msgW = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
               
            }
        }


        private clsUserVO CreateFormData()
        {
            clsUserVO objUserVO = new clsUserVO();
            objUserVO.ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            
            return objUserVO;
        }

        private void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }
        }

        void Save()
        {
            this.DialogResult = true;
            clsUserVO objLogin = CreateFormData();
            clsUserRoleVO objRole = CreateRoleObjectFromFormData();
            //objRole.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


            clsUpdateDashboardBizActionVO objPasswordChange = new clsUpdateDashboardBizActionVO();
            objPasswordChange.ID = objLogin.ID;
            objPasswordChange.DashBoardList = objRole.DashBoardList;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {

                    //clsUserVO objChangePassword = ((clsChangePasswordFirstTimeBizActionVO)ea.Result).Details;
                    //this.DataContext = objChangePassword;

                    msgText = "Record is successfully submitted!";

                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();


                    //Uri address1 = new Uri(Application.Current.Host.Source, "../login.aspx");
                    //HtmlPage.Window.Navigate(address1);
                }
            };
            client.ProcessAsync(objPasswordChange, new clsUserVO());
            client.CloseAsync();
            client = null;

        }

        //void Save()
        //{
        //    this.DialogResult = true;
        //    clsUserVO objLogin = CreateFormData();
        //    clsUserRoleVO objRole = CreateRoleObjectFromFormData();

        //    clsChangePasswordFirstTimeBizActionVO objPasswordChange = new clsChangePasswordFirstTimeBizActionVO();
        //    objPasswordChange.Details = objLogin;
        //    objPasswordChange.DashBoardList = objRole.DashBoardList;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, ea) =>
        //    {
        //        if (ea.Result != null && ea.Error == null)
        //        {
        //            clsUserVO objChangePassword = ((clsChangePasswordFirstTimeBizActionVO)ea.Result).Details;
        //            this.DataContext = objChangePassword;

        //            msgText = "Record is successfully submitted!";

        //            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            msgWindow.Show();


        //            Uri address1 = new Uri(Application.Current.Host.Source, "../login.aspx");
        //            HtmlPage.Window.Navigate(address1);
        //        }
        //    };
        //    client.ProcessAsync(objPasswordChange, new clsUserVO());
        //    client.CloseAsync();
        //    client = null;
        //}

        private clsUserRoleVO CreateRoleObjectFromFormData()
        {
            clsUserRoleVO objRoleVO = new clsUserRoleVO();

            objRoleVO.Status = true;
            objRoleVO.DashBoardList = (List<clsDashBoardVO>)lstItems.ItemsSource;

            return objRoleVO;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
         
            GetUserDashBoardDetails();
        }

        void GetUserDashBoardDetails()
        {
            clsGetLoginNamePasswordBizActionVO objBizVO = new clsGetLoginNamePasswordBizActionVO();
            // objBizVO.ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            // objBizVO.ID= ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
            //objBizVO.UserType = 0;
            objBizVO.ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsUserVO objRoleDetails = ((clsGetLoginNamePasswordBizActionVO)ea.Result).LoginDetails;
                    //List<clsDashBoardVO> lList = new List<clsDashBoardVO>();
                    lstItems.ItemsSource = ((clsGetLoginNamePasswordBizActionVO)ea.Result).DashBoardList;
                }
            };

            // client.ProcessCompleted += new EventHandler<ProcessCompletedEventArgs>(client_GetUserDashBoardDetails);
            client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        

       
    }
}

