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
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using CIMS;
using PalashDynamics.ValueObjects.IPD;
using C1.Silverlight;
//using EMR.IPD_EMR;
using System.Windows.Controls.Primitives;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Windows.Browser;

namespace EMR
{
    public partial class frmSelectionDoctorSpecliztion : ChildWindow
    {
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        #region Data Members
        public clsVisitVO CurrentVisit { get; set; }
        public RoutedEventHandler OnAddButton_Click;
       // public RoutedEventHandler OnCancelButton_Click;
        public bool IsWindowCloseForRound { get; set; }
        #endregion

        #region Constructor
        public frmSelectionDoctorSpecliztion()
        {
            InitializeComponent();  
        }
        #endregion

        #region Event

        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbSpecialization.SelectedItem != null && ((MasterListItem)cmbSpecialization.SelectedItem).Code != "0")
                    FillDoctor(((MasterListItem)cmbSpecialization.SelectedItem).Code);
            }
            catch (Exception)
            {
            }
        }
        
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Assign Doctor";//LocalizationManager.resourceManager.GetString("ttlAssignDoctor");
            FillSpecialization();
        }
        
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((MasterListItem)cmbSpecialization.SelectedItem).Code == "0")
                {
                    ShowMessageBox("Select Specialization", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    return;
                }
                if (((MasterListItem)cmbDoctor.SelectedItem).Code == "0")
                {
                    ShowMessageBox("Select Doctor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    return;
                }
                if (NewRound.IsChecked == false && ExistingRound.IsChecked == false)
                {
                    ShowMessageBox("Please select Round", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    return;
                }
                if (NewRound.IsChecked == true)
                {
                    clsIPDRoundDetailsBizactionVO bizAction = new clsIPDRoundDetailsBizactionVO();
                    bizAction.SpecName = ((MasterListItem)cmbSpecialization.SelectedItem).Description;
                    bizAction.SpecCode = ((MasterListItem)cmbSpecialization.SelectedItem).Code;
                    bizAction.DoctorName = ((MasterListItem)cmbDoctor.SelectedItem).Description;
                    bizAction.DoctorId = Convert.ToInt64(((MasterListItem)cmbDoctor.SelectedItem).Code);
                    bizAction.DoctorCode = ((MasterListItem)cmbDoctor.SelectedItem).Code;
                    bizAction.PatientID = this.CurrentVisit.PatientId;
                    bizAction.PatientUnitID = this.CurrentVisit.PatientUnitId;
                    bizAction.AdmisstionId = this.CurrentVisit.ID;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            bizAction.ID = ((clsIPDRoundDetailsBizactionVO)args.Result).ID;
                            this.DialogResult = false;
                            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                            this.OnAddButton_Click(bizAction, e);
                        }
                    };
                    client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                else
                {
                    clsIPDRoundDetailsBizactionVO bizAction = new clsIPDRoundDetailsBizactionVO();
                    bizAction.SpecName = ((MasterListItem)cmbSpecialization.SelectedItem).Description;
                    bizAction.SpecCode = ((MasterListItem)cmbSpecialization.SelectedItem).Code;
                    bizAction.DoctorName = ((MasterListItem)cmbDoctor.SelectedItem).Description;
                    bizAction.DoctorId = Convert.ToInt64(((MasterListItem)cmbDoctor.SelectedItem).Code);
                    bizAction.DoctorCode = ((MasterListItem)cmbDoctor.SelectedItem).Code;
                    bizAction.PatientID = CurrentVisit.PatientId;
                    bizAction.AdmisstionId = CurrentVisit.ID;
                    this.DialogResult = false;
                    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                    this.OnAddButton_Click(bizAction, e);
                }
            }
            catch (Exception p)
            {
            }
        }
     
        #endregion
      
        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Fill DOCTER AND SPLIZATION COMBO
        private void FillDoctor(string sDeptCode)
        {
            clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = new clsGetRSIJDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            if (cmbSpecialization.SelectedItem != null)
            {
                BizAction.IsForReferral = true;
                BizAction.SpecialCode = sDeptCode;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem("0", "-- Select --"));
                    objList.AddRange(((clsGetRSIJDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;
                    cmbDoctor.SelectedItem = objList[0];
                    if (this.DataContext != null)
                    {
                        cmbDoctor.SelectedValue = objList[0].ID;
                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                        {
                            cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
                            cmbDoctor.IsEnabled = false;
                        }
                        else
                            cmbDoctor.SelectedValue = "0";
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FillSpecialization()
        {
            try
            {
                clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Specialization;
                BizAction.CodeColumn = "ID";
                BizAction.DescriptionColumn = "Description";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem("0", "-- Select --"));
                        objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
                        cmbSpecialization.ItemsSource = null;
                        cmbSpecialization.ItemsSource = objList;
                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                        {
                            string sSpecCode = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorSpecCode;
                            cmbSpecialization.SelectedItem = objList.Where(z => z.Code == sSpecCode).FirstOrDefault();
                            cmbSpecialization.IsEnabled = false;
                        }
                        else
                        {
                            cmbSpecialization.SelectedItem = objList[0];
                        }
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
            }
            catch (Exception)
            {
            }
        }
        #endregion

        private void cmbDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ClsIPDCheckRoundExistsBizactionVO Bizaction = new ClsIPDCheckRoundExistsBizactionVO();
                Bizaction.PatientID = CurrentVisit.PatientId;
                Bizaction.VisitId = CurrentVisit.ID;
                Bizaction.IsOpdIpd = CurrentVisit.OPDIPD;
                Bizaction.DoctorCode = ((MasterListItem)cmbDoctor.SelectedItem).Code;
                Bizaction.DoctorID = Convert.ToInt64(((MasterListItem)cmbDoctor.SelectedItem).Code);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((ClsIPDCheckRoundExistsBizactionVO)args.Result).status == 0)
                        {
                            ExistingRound.Visibility = Visibility.Collapsed;
                            NewRound.IsChecked = true;
                        }
                        else
                        {
                            ExistingRound.Visibility = Visibility.Visible;
                            NewRound.IsChecked = false;
                        }
                    }
                };
                client.ProcessAsync(Bizaction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        private void cmdCancel_Click_1(object sender, RoutedEventArgs e)
        {
            if (IsWindowCloseForRound == true)
            {
                this.DialogResult = false;
                ModuleName = "PalashDynamics.IPD";
                Action = "PalashDynamics.IPD.Forms.frmAdmissionList";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                //this.Onclose_click(CurrentVisit,e);
                this.DialogResult = false;
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            }
        }
        void c_OpenReadCompleted_maleSemenSurvival(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       
    }
}

