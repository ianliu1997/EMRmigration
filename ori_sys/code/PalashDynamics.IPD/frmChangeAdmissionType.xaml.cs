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
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.IPD;
using System.Reflection;

namespace PalashDynamics.IPD
{
    public partial class frmChangeAdmissionType : ChildWindow, IInitiateCIMS
    {
        bool IsPatientExist = true;
        long VisitID, VisitUnitID, AdmissionTypeID;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        public long AdmId = 0;
        public long AdmUnitId = 0;
        public frmChangeAdmissionType()
        {
            InitializeComponent();
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                // ((IApplicationConfiguration)App.Current).SelectedIPDPatient = new clsIPDAdmissionVO();
                VisitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                VisitUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                AdmissionTypeID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionTypeID;
            }

            this.Loaded += new RoutedEventHandler(ChangeAdmissionTypeChildWindow_Loaded);
        }
        long iCount = 0;
        public void FillAdmissionType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_AdmissionType;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbAdmissionType.ItemsSource = null;
                    cmbAdmissionType.ItemsSource = objList;

                    if (this.AdmissionTypeID > 0)
                    {
                        cmbAdmissionType.SelectedItem = objList.SingleOrDefault(S => S.ID.Equals(this.AdmissionTypeID));
                        //iCount = ((MasterListItem)cmbAdmissionType.SelectedItem).ID;
                    }
                    else
                    {
                        cmbAdmissionType.SelectedItem = objList[0];
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();

                        IsPatientExist = false;
                        break;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.IsDischarge == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Already Discharged.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }
                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    break;
            }
        }

        #endregion
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                msgText = "Are you sure you want to update?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.OnMessageBoxClosed += (MessageBoxResult res)=>
                {
                    if(res == MessageBoxResult.Yes)
                    {
                        ChangeAdmissionType();
                        this.DialogResult = true;
                    }
                };
                msgWin.Show();
            }
        }

        private void ChangeAdmissionType()
        {
            clsUpdateAdmissionTypeBizActionVO Bizaction = new clsUpdateAdmissionTypeBizActionVO();
            Bizaction.UpdateAdmType = new clsIPDAdmissionVO();

            try
            {
                Bizaction.AdmID = AdmId;
                Bizaction.AdmUnitID = AdmUnitId;
                Bizaction.AdmTypeID = (((MasterListItem)cmbAdmissionType.SelectedItem).ID);
                Bizaction.UpdateAdmType.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                Bizaction.UpdateAdmType.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                Bizaction.UpdateAdmType.UpdatedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                Bizaction.UpdateAdmType.UpdatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsUpdateAdmissionTypeBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            msgText = "Record updated successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            this.DialogResult = true;
                            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IPD.Forms.frmAdmissionList") as UIElement;
                            ((IApplicationConfiguration)App.Current).OpenMainContent(mydata);
                        }
                    }
                };
                client.ProcessAsync(Bizaction, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                msgText = "Error occurred while processing.";

                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChangeAdmissionTypeChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
                this.DialogResult = false;
            else
            {
                if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
                {
                    txtPatientName.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName;
                    txtipdno.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.IPDNO;
                    cmbAdmissionType.SelectedItem = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionTypeID;
                    AdmUnitId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                    AdmId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                }
                FillAdmissionType();
                //OKButton.IsEnabled = false;
            }
        }
        public bool Validation()
        {
            if (((MasterListItem)cmbAdmissionType.SelectedItem).ID == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Please Select Admission Type", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                return false;
            }
            return true;
        }

        private void cmbAdmissionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (iCount != Convert.ToInt64(((MasterListItem)cmbAdmissionType.SelectedItem).ID))
            //{
            //    OKButton.IsEnabled = true;
            //}
        }
    }
}
