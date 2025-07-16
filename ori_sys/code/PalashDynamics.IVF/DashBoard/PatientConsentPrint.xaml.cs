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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Windows.Browser;
using PalashDynamics.IVF;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Patient;

namespace OPDModule.Forms
{
    public partial class PatientConsentPrint : ChildWindow
    {
        long PatientID;
        long PatientUnitID;
        
        public PatientConsentPrint( long patientid , long patientunitid)
        {
            InitializeComponent();
            PatientID = patientid;
            PatientUnitID = patientunitid;
        }
    
        public clsPatientConsentVO ConsentDetails{get;set;}
        public bool IsSaved { get; set; }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            richTextEditor.Html = ((clsPatientConsentVO)this.DataContext).Template;
            FillDepartmentList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
            if (IsSaved == true)
            {
                cmdSave.IsEnabled = false;
                cmdPrint.IsEnabled = true;
            }
            else
            {
                cmdSave.IsEnabled = true;
                cmdPrint.IsEnabled = false;
            }
        }

        private void FillDepartmentList(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbDepartment.SelectedValue = ((clsPatientConsentVO)this.DataContext).DepartmentID;
                    }
                }
                GetDetailsifPatientIsCouple();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        } 
        
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (richTextEditor.Html != "")
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n You Want To Save & Print The Patient Consent ?";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                msgW1.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Template", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }
        
        private void Save()
        {
            clsAddPrintedPatientConscentBizActionVO BizAction = new clsAddPrintedPatientConscentBizActionVO();
            try
            {
                BizAction.ConsentDetails = (clsPatientConsentVO)this.DataContext;
                BizAction.ConsentDetails.ConsentID = ((clsPatientConsentVO)this.DataContext).ID;
                BizAction.ConsentDetails.ID = 0;
                BizAction.ConsentDetails.Template = richTextEditor.Html;
                //BizAction.ConsentDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                //BizAction.ConsentDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.ConsentDetails.PatientID =PatientID;
                BizAction.ConsentDetails.PatientUnitID = PatientUnitID;
                BizAction.ConsentDetails.Template = GetConsentfields();
                //string str = richTextEditor.Html;
                //string str2 = null;
                //string str3 = null;

                //if (str.Contains("{Patient Name}") && str.Contains("{MR NO}"))
                //{
                //   string PN = str.Replace("{Patient Name}", ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + ' ' + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + ' ' + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName);
                //   string MR = PN.Replace("{MR NO}", ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);
                //   str3 = MR;
                //}
                //else if (str.Contains("{Patient Name}"))
                //{
                //    str2 = str.Replace("{Patient Name}", ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + ' ' + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + ' ' + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName);
                //}
                //else if (str.Contains("{MR NO}"))
                //{
                //    str2 = str.Replace("{MR NO}", ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);                    
                //}

                //if (str2 != null)
                //{
                //    BizAction.ConsentDetails.Template = str2;
                //}
                //else if (str3 != null)
                //    BizAction.ConsentDetails.Template = str3;
                //else
                //    BizAction.ConsentDetails.Template = richTextEditor.Html;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Patient Consent Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.OnMessageBoxClosed += (re) =>
                                    {
                                        if (re == MessageBoxResult.OK)
                                        {
                                            if (((clsAddPrintedPatientConscentBizActionVO)args.Result).ConsentDetails.ID > 0)
                                            {
                                                PrintConsent(((clsAddPrintedPatientConscentBizActionVO)args.Result).ConsentDetails.ID);
                                            }
                                        }
                                    };
                        msgW1.Show();
                        this.DialogResult = false;

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = false;

        }

        private void PrintConsent(long iID)
      {
          if (iID > 0)
          {
              long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
              string URL = "../Reports/OPD/PatientConsent.aspx?ID=" + iID + "&UnitID=" + UnitID; ;
              HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
          }
      }
        
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintConsent(((clsPatientConsentVO)this.DataContext).ID);
        }
          private string GetConsentfields()
        {
            string str = richTextEditor.Html;
            if (str.Contains("{Patient First Name}"))
            {
                str = str.Replace("{Patient First Name}", ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName);
            }
            if (str.Contains("{Patient Last Name}"))
            {
                str = str.Replace("{Patient Last Name}", ((IApplicationConfiguration)App.Current).SelectedPatient.LastName);
            }
             if (str.Contains("{MR No}"))
            {
                str = str.Replace("{MR No}", ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);
            }
              if (str.Contains("{DOB}"))
            {
                str = str.Replace("{DOB}", (((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth.Value).ToString("dd/MMM/yyyy"));
            }
              if (str.Contains("{Patient Middle Name}"))
            {
                str = str.Replace("{Patient Middle Name}", ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName);
            }
              if (str.Contains("{Patient title}"))
            {
                str = str.Replace("{Patient title}", ((IApplicationConfiguration)App.Current).SelectedPatient.Gender);
            }
              if (str.Contains("{Location}"))
            {
                str = str.Replace("{Location}", ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitName);
            }
              if (str.Contains("{Female Patient Last Name}") || str.Contains("{Female Patient First Name}") || str.Contains("{Female Patient Middle Name}"))
            {
                 if(IsCouple==true)
                 {
                     str = str.Replace("{Female Patient Last Name}", CoupleDetails.FemalePatient.LastName);
                     str = str.Replace("{Female Patient First Name}", CoupleDetails.FemalePatient.FirstName);
                     str = str.Replace("{Female Patient Middle Name}", CoupleDetails.FemalePatient.MiddleName);
                 }
                 else
                 {
                     str = str.Replace("{Female Patient Last Name}", " ");
                     str = str.Replace("{Female Patient First Name}", " ");
                     str = str.Replace("{Female Patient Middle Name}", " ");
                 }
            }
            return str;

        }
        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }
       bool IsCouple=false;
        private void GetDetailsifPatientIsCouple()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = PatientID;
            BizAction.PatientUnitID = PatientUnitID;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                    BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                    CoupleDetails.MalePatient = new clsPatientGeneralVO();
                    CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                    CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                    if (CoupleDetails.CoupleId != 0)
                    {
                        IsCouple = true;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
    }
  
}

