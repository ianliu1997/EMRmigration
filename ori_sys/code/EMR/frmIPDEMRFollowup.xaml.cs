using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Resources;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

using PalashDynamics.ValueObjects;

using System.Windows.Media;
using System.Windows.Browser;
using System.Windows.Data;

using PalashDynamics.ValueObjects.Administration;

namespace EMR
{
    using System.Windows.Controls.Primitives;

    //public class ReadonlyDatePicker : DatePicker
    //{
    //    public override void OnApplyTemplate()
    //    {
    //        base.OnApplyTemplate();
    //        var textBox = this.GetTemplateChild("TextBox") as DatePickerTextBox;
    //        textBox.IsReadOnly = true;
    //    }
    //}
    public partial class frmIPDEMRFollowup : UserControl
    {
        #region Data Member
        public Boolean IsEnableControl { get; set; }
        public clsVisitVO CurrentVisit { get; set; }
        #endregion

        public frmIPDEMRFollowup()
        {
            InitializeComponent();
            dpFollowUpDate.DisplayDateStart = DateTime.Now;
            this.Loaded += new RoutedEventHandler(frmEMRFollowup_Loaded);
        }
        void frmEMRFollowup_Loaded(object sender, RoutedEventArgs e)
        {

            FillFollowUpDetails();
            if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
                this.IsEnableControl = false;
            }
            else
            {
                if (CurrentVisit.ISIPDDischarge)
                {
                    this.IsEnableControl = false;
                }
                spSpecDoctor.Visibility = Visibility.Visible;
            }
            //else if (this.CurrentVisit.VisitTypeID == 1)
            //{
            //    spSpecDoctor.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    spSpecDoctor.Visibility = Visibility.Visible;
                FillSpecialization();
                FillDoctor();
           // }
            cmdSave.IsEnabled = IsEnableControl;
            cmdSchedule.IsEnabled = IsEnableControl;
        }

        #region Fill DOCTER AND SPLIZATION COMBO
        private void FillDoctor()
        {
            //clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = new clsGetRSIJDoctorDepartmentDetailsBizActionVO();
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem("0", "-- Select --"));
            //        objList.AddRange(((clsGetRSIJDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();

            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(this.CurrentVisit.DoctorCode, this.CurrentVisit.Doctor));
            cmbDoctor.ItemsSource = null;
            cmbDoctor.ItemsSource = objList;
            cmbDoctor.SelectedItem = objList[0];
            cmbDoctor.IsEnabled = false;
        }
        private void FillSpecialization()
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem("0", this.CurrentVisit.DoctorSpecialization));
            cmbSpecialization.ItemsSource = null;
            cmbSpecialization.ItemsSource = objList;
            cmbSpecialization.SelectedItem = objList[0];
            cmbSpecialization.IsEnabled = false;
        }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        private void cmdSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = CurrentVisit.PatientId;
            ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = CurrentVisit.UnitId;

            UserControl rootPage = Application.Current.RootVisual as UserControl;

            WebClient c2 = new WebClient();
            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
            c2.OpenReadAsync(new Uri("OPDModule" + ".xap", UriKind.Relative));

        }

        void c2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;

                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == "OPDModule.dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance("CIMS.Forms.PatientAppointment") as UIElement;

                if (myData != null)
                {
                    ((IInitiateCIMS)myData).Initiate("FollowUp");

                    ((ChildWindow)myData).Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            //string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
            //MessageBoxControl.MessageBoxChildWindow msgW =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
            //msgW.Show();
            SaveFollowUp();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //string msgText = DefaultValues.ResourceManager.GetString("DiscardChanges");
            //MessageBoxControl.MessageBoxChildWindow msgWinCancel =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgWinCancel.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinCancel_OnMessageBoxClosed);
            //msgWinCancel.Show();
            NavigateToDashBoard();
        }

        void msgWinCancel_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                NavigateToDashBoard();
            }
        }

        private void NavigateToDashBoard()
        {
            this.Content = null;
            ((((((((this.Parent) as ContentControl).Parent as Border).Parent as DockPanel).Parent as DockPanel).FindName("tvPatientEMR") as TreeView)).Items[0] as TreeViewItem).IsSelected = true;
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveFollowUp();
            }

        }

        private void SaveFollowUp()
        {
            clsAddUpdateFollowUpDetailsBizActionVO BizAction = new clsAddUpdateFollowUpDetailsBizActionVO();
            BizAction.VisitID = this.CurrentVisit.ID;
            BizAction.PatientID = this.CurrentVisit.PatientId;
            BizAction.DoctorCode = this.CurrentVisit.DoctorCode;
            BizAction.PatientUnitID = this.CurrentVisit.PatientUnitId;
            BizAction.IsOPDIPD = this.CurrentVisit.OPDIPD;
            BizAction.FollowUpRemark = txtFollowUpRemark.Text;
            BizAction.FollowupDate = dpFollowUpDate.SelectedDate;
            BizAction.Advice = txtFollowUpAdvice.Text;
            BizAction.DoctorCode = this.CurrentVisit.DoctorCode;
            BizAction.DepartmentCode = this.CurrentVisit.DepartmentCode;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                clsAddUpdateFollowUpDetailsBizActionVO objBizAction = args.Result as clsAddUpdateFollowUpDetailsBizActionVO;
                string strSaveMsg = DefaultValues.ResourceManager.GetString("RecordSavePrompt");
                if (objBizAction.SuccessStatus == 1)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //            new MessageBoxControl.MessageBoxChildWindow("Palash", strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    //{
                    //    this.Content = null;
                    //    NavigateToNextMenu();
                    //};
                    //msgW1.Show();
                   // NavigateToNextMenu();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //                new MessageBoxControl.MessageBoxChildWindow("Palash", strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    //{
                    //    NavigateToDashBoard();
                    //};
                    //msgW1.Show();
                  //  NavigateToDashBoard();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #region Private MEthods

        private void FillFollowUpDetails()
        {
            clsGetPatientFollowUpDetailsBizActionVO BizAction = new clsGetPatientFollowUpDetailsBizActionVO();
            BizAction.VisitID = this.CurrentVisit.ID;
            BizAction.PatientID = this.CurrentVisit.PatientId;
            BizAction.PatientUnitID = this.CurrentVisit.PatientUnitId;
            BizAction.Isopdipd = this.CurrentVisit.OPDIPD;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Result != null && args.Error == null)
                {
                    clsGetPatientFollowUpDetailsBizActionVO ObjBizAction = args.Result as clsGetPatientFollowUpDetailsBizActionVO;
                    txtFollowUpAdvice.Text = ObjBizAction.FollowUpAdvice == null ? string.Empty : ObjBizAction.FollowUpAdvice;
                    txtFollowUpRemark.Text = ObjBizAction.FollowUpRemark == null ? String.Empty : ObjBizAction.FollowUpRemark;
                    dpFollowUpDate.DisplayDate = Convert.ToDateTime(ObjBizAction.FollowupDate);
                    dpFollowUpDate.SelectedDate = ObjBizAction.FollowupDate;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

       

        #endregion
    }
}
