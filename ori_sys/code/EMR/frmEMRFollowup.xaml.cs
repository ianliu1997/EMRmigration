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
    using PalashDynamics.ValueObjects.Master;
    using PalashDynamics.UserControls;

    public class ReadonlyDatePicker : DatePicker
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var textBox = this.GetTemplateChild("TextBox") as DatePickerTextBox;
            textBox.IsReadOnly = true;
        }
    }
    public partial class frmEMRFollowup : UserControl
    {
        #region Data Member
        public Boolean IsEnableControl { get; set; }
        public clsVisitVO CurrentVisit { get; set; }
        WaitIndicator Indicatior = null;
        #endregion


        #region Constructor
        
        public frmEMRFollowup()
        {
            InitializeComponent();
            dpFollowUpDate.DisplayDateStart = DateTime.Now;
            this.Loaded += new RoutedEventHandler(frmEMRFollowup_Loaded);
        }

        void frmEMRFollowup_Loaded(object sender, RoutedEventArgs e)
        {
            FillReasonList();
            FillFollowUpDetails();
            if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
                this.IsEnableControl = false;
            }
            else if (this.CurrentVisit.VisitTypeID == 1)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
            }
            else
            {
               //spSpecDoctor.Visibility = Visibility.Visible;
                FillSpecialization();
                FillDoctor();
            }
            //DateTime d = CurrentVisit.Date;
            //if (d.ToString("d") != DateTime.Now.ToString("d"))
            //{
            //    cmdSave.IsEnabled = false;
            //    cmdSaveandAppoinment.IsEnabled = false;
            //}

            // EMR Changes Added by Ashish Z. on dated 02062017
            if (CurrentVisit.EMRModVisitDate <= DateTime.Now)
            {
                cmdSave.IsEnabled = false;
                cmdSaveandAppoinment.IsEnabled = false;
            }
            //End

                //cmdSave.IsEnabled = IsEnableControl;
                //cmdSchedule.IsEnabled = IsEnableControl;
        }
        
        #endregion
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            lblFollowUpAdvice1.Visibility = Visibility.Collapsed;
            cmbAppointmentType12.Visibility = Visibility.Collapsed;
            lblFollowUpAdvice.Visibility = Visibility.Collapsed;
            dpFollowUpDate.Visibility = Visibility.Collapsed;
            cmdSaveandAppoinment.IsEnabled = false;
        }
        private void RadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            if (week1.IsChecked==true)
            {
              dpFollowUpDate.SelectedDate=dt.AddDays(7);
            }
            else if (week2.IsChecked == true)
            {
                dpFollowUpDate.SelectedDate = dt.AddDays(14);
            }
            else if (week3.IsChecked == true)
            {
                dpFollowUpDate.SelectedDate = dt.AddDays(21);
            }
            else if (month.IsChecked == true)
            {
                dpFollowUpDate.SelectedDate = dt.AddMonths(1);
            }

        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            lblFollowUpAdvice1.Visibility = Visibility.Visible;
            cmbAppointmentType12.Visibility = Visibility.Visible;
            lblFollowUpAdvice.Visibility = Visibility.Visible;
            dpFollowUpDate.Visibility = Visibility.Visible;
            cmdSaveandAppoinment.IsEnabled = true;
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
            SaveFollowUp(false);
        }

        private void cmdSaveandAppoinment_Click(object sender, RoutedEventArgs e)
        {
            SaveFollowUp(true);
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
                SaveFollowUp(false);
            }

        }

        private void SaveFollowUp(Boolean WithAppoinment)
        {
            if (followupreq.IsChecked == false)
            {
                if (dpFollowUpDate.SelectedDate == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Follow Up Date !!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    return;
                }
            }
            if (WithAppoinment)
            {
                if (((MasterListItem)cmbAppointmentType12.SelectedItem).ID == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Reason !!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    return;
                }
            }
            if (txtFollowUpRemark.Text.Length <= 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter the Remark!!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return;
            }
            WaitIndicator IndicatiorDiag = new WaitIndicator();
            IndicatiorDiag.Show();
            clsAddUpdateFollowUpDetailsBizActionVO BizAction = new clsAddUpdateFollowUpDetailsBizActionVO();
            Boolean FollowUpRequired=false;
            if (followupreq.IsChecked==true)
            {
                FollowUpRequired = true;
            }
            
            BizAction.VisitID = this.CurrentVisit.ID;
            BizAction.PatientID = this.CurrentVisit.PatientId;
            BizAction.DoctorCode = this.CurrentVisit.DoctorCode;
            BizAction.PatientUnitID = this.CurrentVisit.PatientUnitId;
            BizAction.IsOPDIPD = this.CurrentVisit.OPDIPD;
            BizAction.FollowUpRemark = txtFollowUpRemark.Text;
            BizAction.FollowupDate = dpFollowUpDate.SelectedDate;
            BizAction.Advice = txtFollowUpAdvice.Text;
            BizAction.FolloWUPRequired = FollowUpRequired;
            if ((MasterListItem)cmbAppointmentType12.SelectedItem != null)
                BizAction.AppoinmentReson = ((MasterListItem)cmbAppointmentType12.SelectedItem).ID;
            BizAction.DepartmentCode = this.CurrentVisit.DepartmentCode;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                clsAddUpdateFollowUpDetailsBizActionVO objBizAction = args.Result as clsAddUpdateFollowUpDetailsBizActionVO;
                string strSaveMsg = DefaultValues.ResourceManager.GetString("RecordSavePrompt");
                if (objBizAction.SuccessStatus == 1)
                {
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //            new MessageBoxControl.MessageBoxChildWindow("Palash", strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    //{
                    //    this.Content = null;
                    //    NavigateToNextMenu();
                    //};
                    //msgW1.Show();
                    if (WithAppoinment == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    else
                    {
                        CIMS.Forms.PatientAppointment winDisplay = new CIMS.Forms.PatientAppointment();
                        if ((MasterListItem)cmbAppointmentType12.SelectedItem != null)
                            winDisplay.FollowUpAppoinment = ((MasterListItem)cmbAppointmentType12.SelectedItem).ID;

                        winDisplay.IsFromEMR = true;//Added By YK 08042017
                        //winDisplay.FirstName = objBizAction.PatientID.ToString();
                        winDisplay.FollowUpDate = (DateTime)dpFollowUpDate.SelectedDate;
                        winDisplay.FollowUpDoctorID = CurrentVisit.DoctorID;
                        winDisplay.FollowUPDepartment =Convert.ToInt64(CurrentVisit.DepartmentCode);
                        winDisplay.FollowUPRemark = txtFollowUpRemark.Text;
                        
                        winDisplay.IsFromEMR = true;//Added By YK 08042017
                        winDisplay.MrNo = this.CurrentVisit.MRNO;
                       //winDisplay.GenderID=this.CurrentVisit.g

                        winDisplay.Initiate("FollowUp");
                        winDisplay.Show();
                    }
                  //  NavigateToNextMenu();
                    IndicatiorDiag.Close();
                }
                else
                {
                    if (WithAppoinment == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    else
                    {
                        CIMS.Forms.PatientAppointment winDisplay = new CIMS.Forms.PatientAppointment();
                        if ((MasterListItem)cmbAppointmentType12.SelectedItem != null)
                            winDisplay.FollowUpAppoinment = ((MasterListItem)cmbAppointmentType12.SelectedItem).ID;
                        winDisplay.FollowUpDate = (DateTime)dpFollowUpDate.SelectedDate;
                        winDisplay.FollowUpDoctorID = CurrentVisit.DoctorID;
                        winDisplay.FollowUPDepartment = Convert.ToInt64(CurrentVisit.DepartmentCode);

                        winDisplay.IsFromEMR = true;//Added By YK 08042017
                        winDisplay.MrNo = this.CurrentVisit.MRNO;


                        winDisplay.FollowUPRemark = txtFollowUpRemark.Text;
                        winDisplay.Initiate("FollowUp");
                        winDisplay.Show();
                    }
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //                new MessageBoxControl.MessageBoxChildWindow("Palash", strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    //{
                    //    NavigateToDashBoard();
                    //};
                    //msgW1.Show();
                  //  NavigateToDashBoard();
                   IndicatiorDiag.Close();
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
                    dpFollowUpDate.DisplayDate =Convert.ToDateTime( ObjBizAction.FollowupDate);
                    dpFollowUpDate.SelectedDate = ObjBizAction.FollowupDate;
                    if (ObjBizAction.NoFollowReq)
                    {
                        followupreq.IsChecked = true;
                    }
                    if (ObjBizAction.AppoinmentReson != 0)
                    {
                        cmbAppointmentType12.SelectedValue = ObjBizAction.AppoinmentReson;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void NavigateToNextMenu()
        {
            EMR.frmEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            TreeView tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            TreeViewItem SelectedItem = tvEMR.SelectedItem as TreeViewItem;
            clsMenuVO objMenu = SelectedItem.DataContext as clsMenuVO;
            if (SelectedItem.HasItems == true)
            {
                (SelectedItem.Items[0] as TreeViewItem).IsSelected = true;
            }
            else if (objMenu.Parent.Trim() == "Patient EMR")
            {
                int iCount = tvEMR.Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (objMenu.MenuOrder < iCount)
                {
                    if ((tvEMR.Items[iMenuIndex] as TreeViewItem).HasItems == true)
                    {
                        ((tvEMR.Items[iMenuIndex] as TreeViewItem).Items[0] as TreeViewItem).IsSelected = true;
                    }
                    else
                        (tvEMR.Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
            }
            else
            {
                int iCount = (SelectedItem.Parent as TreeViewItem).Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (iCount > objMenu.MenuOrder)
                {
                    ((SelectedItem.Parent as TreeViewItem).Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
                else
                {
                    objMenu = (SelectedItem.Parent as TreeViewItem).DataContext as clsMenuVO;
                    int iIndex = Convert.ToInt32(objMenu.MenuOrder);
                    (tvEMR.Items[iIndex] as TreeViewItem).IsSelected = true;
                }
            }
        }

        private void FillReasonList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_AppointmentReasonMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbAppointmentType12.ItemsSource = null;
                    cmbAppointmentType12.SelectedItem = objList[0];
                    cmbAppointmentType12.ItemsSource = objList;
                }
                //if (this.DataContext != null)
                //{
                //    cmbAppointmentType.SelectedValue = ((clsAppointmentVO)this.DataContext).AppointmentReasonId;
                //}
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion
    
    }
}
