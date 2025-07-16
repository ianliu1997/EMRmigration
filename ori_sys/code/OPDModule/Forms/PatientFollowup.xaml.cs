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
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.UserControls;
using PalashDynamics.Animations;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Windows.Media.Imaging;
using System.IO;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using System.Windows.Controls.Primitives;

namespace OPDModule.Forms
{
    public partial class PatientFollowup : ChildWindow
    {
        clsPatientGeneralVO SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
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
        WaitIndicator wait = new WaitIndicator();
        private SwivelAnimation objAnimation;
        public event RoutedEventHandler OnCancelButton_Click;
        public event RoutedEventHandler OnSaveButton_Click;
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        public PatientFollowup()
        {

            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmEMR_Loaded);
            FillReasonList();
            FillDoctor();
            if (((IApplicationConfiguration)App.Current).SelectedPatient.FollowUpID > 0)
            {
                CmdSave.Visibility = Visibility.Collapsed;
                CmdModify.Visibility = Visibility.Visible;
            }

        }



        private void frmEMR_Loaded(object sender, RoutedEventArgs e)
        {
            if (!SelectedPatient.IsFromAppointment)
            {
                if (SelectedPatient != null)
                {
                    if (SelectedPatient.MRNo != null)
                    {
                        if (SelectedPatient != null && SelectedPatient.MRNo.Length > 1)
                        {
                            fillCoupleDetails();
                        }
                    }
                }
            }
        }

        private void fillCoupleDetails()
        {
            try
            {
                clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();

                BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;

                BizAction.IsAllCouple = false;
                BizAction.CoupleDetails = new clsCoupleVO();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                        {
                            BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                            BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                            CoupleDetails.MalePatient = new clsPatientGeneralVO();
                            CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                            CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                            //.........................................
                            ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = CoupleDetails;

                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.ImageName != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.ImageName.Length > 0)
                            {
                                imgPhoto13.Source = new BitmapImage(new Uri(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.ImageName, UriKind.Absolute));

                                imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                                imgP1.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            else if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
                            {
                                byte[] imageBytes = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo;
                                BitmapImage img = new BitmapImage();
                                img.SetSource(new MemoryStream(imageBytes, false));
                                imgPhoto13.Source = img;

                                imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                                imgP1.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            else
                            {
                                imgP1.Visibility = System.Windows.Visibility.Visible;
                                imgPhoto13.Visibility = System.Windows.Visibility.Collapsed;
                            }

                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.ImageName != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.ImageName.Length > 0)
                            {
                                imgPhoto12.Source = new BitmapImage(new Uri(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.ImageName, UriKind.Absolute));

                                imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                                imgP2.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            else if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
                            {
                                byte[] imageBytes = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo;
                                BitmapImage img = new BitmapImage();
                                img.SetSource(new MemoryStream(imageBytes, false));
                                imgPhoto12.Source = img;

                                imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                                imgP2.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            else
                            {
                                imgP2.Visibility = System.Windows.Visibility.Visible;
                                imgPhoto12.Visibility = System.Windows.Visibility.Collapsed;
                            }

                            Maleborder.Visibility = Visibility.Visible;
                            Femaleborder.Visibility = Visibility.Visible;

                            if (CoupleDetails.FemalePatient != null)
                            {
                                clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                                FemalePatientDetails = CoupleDetails.FemalePatient;
                                Female.DataContext = FemalePatientDetails;
                                txtReferralDoctor.Text = FemalePatientDetails.DoctorName;
                                txtSourceofReference.Text = FemalePatientDetails.SourceofReference;
                                txtCamp.Text = FemalePatientDetails.Camp;

                            }

                            if (CoupleDetails.MalePatient != null)
                            {
                                clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                                MalePatientDetails = CoupleDetails.MalePatient;
                                Male.DataContext = MalePatientDetails;
                            }

                        }
                        else
                        {
                            LoadPatientHeader();
                        }
                        wait.Close();
                    }
                    else
                    {
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }



        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            string msgText = "Are you sure you want to Save Follow Up ?";
            MessageBoxControl.MessageBoxChildWindow msgWD =
                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWD.OnMessageBoxClosed += (args) =>
            {
                if (args == MessageBoxResult.Yes)
                {
                    SaveFollowUp(false);
                }
            };
            msgWD.Show();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());
        }

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            SaveFollowUp(false);
        }

        private void LoadPatientHeader()
        {
            clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
            BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();
            BizAction.PatientDetails.GeneralDetails = (clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.PatientDetails.GeneralDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                    BizAction.PatientDetails.SpouseDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.SpouseDetails;

                    if (((clsGetPatientBizActionVO)args.Result).PatientDetails.GenderID == 2)
                    {
                        Female.DataContext = BizAction.PatientDetails.GeneralDetails;
                        CoupleDetails.FemalePatient = BizAction.PatientDetails.GeneralDetails;
                        CoupleDetails.FemalePatient.ImageName = BizAction.PatientDetails.ImageName;
                        if (BizAction.PatientDetails.GeneralDetails.ContactNO1 == null)
                        {
                            CoupleDetails.FemalePatient.ContactNO1 = ((clsGetPatientBizActionVO)args.Result).PatientDetails.ContactNo1;
                            CoupleDetails.FemalePatient.Email = ((clsGetPatientBizActionVO)args.Result).PatientDetails.Email;
                            CoupleDetails.FemalePatient.AddressLine1 = ((clsGetPatientBizActionVO)args.Result).PatientDetails.AddressLine1;
                        }

                        Maleborder.Visibility = Visibility.Collapsed;
                        Femaleborder.Visibility = Visibility.Visible;
                        Male.DataContext = null;
                        CoupleDetails.MalePatient = null;

                        txtReferralDoctor.Text = BizAction.PatientDetails.GeneralDetails.DoctorName;
                        txtSourceofReference.Text = BizAction.PatientDetails.GeneralDetails.SourceofReference;
                        txtCamp.Text = BizAction.PatientDetails.GeneralDetails.Camp;

                        if (((clsGetPatientBizActionVO)args.Result).PatientDetails.ImageName != null && ((clsGetPatientBizActionVO)args.Result).PatientDetails.ImageName.Length > 0)
                        {
                            imgPhoto13.Source = new BitmapImage(new Uri(((clsGetPatientBizActionVO)args.Result).PatientDetails.ImageName, UriKind.Absolute));

                            imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                            imgP1.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (CoupleDetails.FemalePatient.Photo != null)
                        {
                            byte[] imageBytes = CoupleDetails.FemalePatient.Photo;
                            BitmapImage img = new BitmapImage();
                            img.SetSource(new MemoryStream(imageBytes, false));
                            imgPhoto13.Source = img;

                            imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                            imgP1.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            imgP1.Visibility = System.Windows.Visibility.Visible;
                            imgPhoto13.Visibility = System.Windows.Visibility.Collapsed;
                        }

                        CoupleDetails.CoupleId = 0;

                    }
                    else if (((clsGetPatientBizActionVO)args.Result).PatientDetails.GenderID == 1)
                    {
                        CoupleDetails.CoupleId = 0;
                        Male.DataContext = BizAction.PatientDetails.GeneralDetails;
                        CoupleDetails.MalePatient = BizAction.PatientDetails.GeneralDetails;
                        CoupleDetails.MalePatient.ImageName = BizAction.PatientDetails.ImageName;
                        Femaleborder.Visibility = Visibility.Collapsed;
                        Maleborder.Visibility = Visibility.Visible;
                        Female.DataContext = null;
                        CoupleDetails.FemalePatient = null;

                        if (BizAction.PatientDetails.GeneralDetails.ContactNO1 == null)
                        {
                            CoupleDetails.MalePatient.ContactNO1 = ((clsGetPatientBizActionVO)args.Result).PatientDetails.ContactNo1;
                            CoupleDetails.MalePatient.Email = ((clsGetPatientBizActionVO)args.Result).PatientDetails.Email;
                            CoupleDetails.MalePatient.AddressLine1 = ((clsGetPatientBizActionVO)args.Result).PatientDetails.AddressLine1;
                        }

                        txtReferralDoctor.Text = BizAction.PatientDetails.GeneralDetails.DoctorName;
                        txtSourceofReference.Text = BizAction.PatientDetails.GeneralDetails.SourceofReference;
                        txtCamp.Text = BizAction.PatientDetails.GeneralDetails.Camp;

                        if (((clsGetPatientBizActionVO)args.Result).PatientDetails.ImageName != null && ((clsGetPatientBizActionVO)args.Result).PatientDetails.ImageName.Length > 0)
                        {
                            imgPhoto12.Source = new BitmapImage(new Uri(((clsGetPatientBizActionVO)args.Result).PatientDetails.ImageName, UriKind.Absolute));

                            imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                            imgP2.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (CoupleDetails.MalePatient.Photo != null)
                        {
                            byte[] imageBytes = CoupleDetails.MalePatient.Photo;
                            BitmapImage img = new BitmapImage();
                            img.SetSource(new MemoryStream(imageBytes, false));
                            imgPhoto12.Source = img;

                            imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                            imgP2.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            imgP2.Visibility = System.Windows.Visibility.Visible;
                            imgPhoto12.Visibility = System.Windows.Visibility.Collapsed;
                        }                                      

                    }
                    else
                    {
                        Male.DataContext = null;
                        Female.DataContext = null;
                        CoupleDetails = null;
                        Femaleborder.Visibility = Visibility.Visible;
                        Maleborder.Visibility = Visibility.Visible;

                    }

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbFollowupReason.ItemsSource = null;
                    cmbFollowupReason.SelectedItem = objList[0];
                    cmbFollowupReason.ItemsSource = objList;

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.FollowUpReasonID > 0)
                    {
                        cmbFollowupReason.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.FollowUpReasonID;
                        txtRemarks.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.FollowUpRemark;
                        dtpFollowupdate.SelectedDate = ((IApplicationConfiguration)App.Current).SelectedPatient.FollowUpDate;
                    }


                    //((IApplicationConfiguration)App.Current).SelectedPatient.FollowUpID = objVO.FollowUpID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void FillDoctor()
        {
            wait.Show();
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            BizAction.UnitId = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbDoctor.ItemsSource = null;

                    if (((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbDoctor.ItemsSource = objList;
                    cmbDoctor.SelectedValue = objList[0].ID;
                    cmbDoctor.SelectedItem = objList[0];


                    if (((IApplicationConfiguration)App.Current).SelectedPatient.DoctorID > 0)
                    {
                        cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.DoctorID;
                    }

                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            wait.Close();
        }

       
        private void SaveFollowUp(Boolean WithAppoinment)
        {
            // wait.Show();
            if (Validation())
            {
                clsAddUpdateFollowUpDetailsBizActionVO BizAction = new clsAddUpdateFollowUpDetailsBizActionVO();

                BizAction.VisitID = (((IApplicationConfiguration)App.Current).SelectedPatient).VisitID;
                BizAction.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                if ((MasterListItem)cmbDoctor.SelectedItem != null)
                    BizAction.DoctorCode = Convert.ToString(((MasterListItem)cmbDoctor.SelectedItem).ID);

                if ((MasterListItem)cmbFollowupReason.SelectedItem != null)
                    BizAction.AppoinmentReson = ((MasterListItem)cmbFollowupReason.SelectedItem).ID;

                BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientUnitID;
                BizAction.IsOPDIPD = false;
                BizAction.FollowUpRemark = txtRemarks.Text;
                BizAction.FollowupDate = dtpFollowupdate.SelectedDate;
                BizAction.FolloWUPRequired = false;
                BizAction.DepartmentCode = Convert.ToString((((IApplicationConfiguration)App.Current).SelectedPatient).DepartmentID);
                BizAction.ISFollowUpNewQueueList = true;
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    clsAddUpdateFollowUpDetailsBizActionVO objBizAction = args.Result as clsAddUpdateFollowUpDetailsBizActionVO;

                    if (objBizAction.SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        this.DialogResult = true;
                        if (OnSaveButton_Click != null)
                            OnSaveButton_Click(this, new RoutedEventArgs());
                        wait.Close();                      
                        
                    }

                    if (objBizAction.SuccessStatus == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Update Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        this.DialogResult = true;
                        if (OnSaveButton_Click != null)
                            OnSaveButton_Click(this, new RoutedEventArgs());
                        wait.Close();  
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }

          
        public bool Validation()
        {
            bool IsValid = false;

            if (dtpFollowupdate.SelectedDate == null)
            {
                IsValid = false;
                dtpFollowupdate.SetValidation("Please Enter Follow Up Date");
                dtpFollowupdate.RaiseValidationError();
                dtpFollowupdate.Focus();
                return IsValid;               
            }
            else
            {
                dtpFollowupdate.ClearValidationError();
                IsValid = true;
            }

            if (cmbFollowupReason.SelectedItem == null)
            {
                IsValid = false;
                cmbFollowupReason.TextBox.SetValidation("Please select Follow up Reason");
                cmbFollowupReason.TextBox.RaiseValidationError();
                cmbFollowupReason.Focus();
                return IsValid;
            }
            else if (((MasterListItem)cmbFollowupReason.SelectedItem).ID == 0)
            {
                IsValid = false;
                cmbFollowupReason.TextBox.SetValidation("Please select Follow up Reason");
                cmbFollowupReason.TextBox.RaiseValidationError();
                cmbFollowupReason.Focus();
                return IsValid;
            }
            else
            {
                cmbFollowupReason.TextBox.ClearValidationError();
                IsValid = true;
            }

            if (txtRemarks.Text == null && txtRemarks.Text.Trim().Length < 0)
            {
                IsValid = false;
                txtRemarks.SetValidation("Please Enter Remark");
                txtRemarks.RaiseValidationError();
                txtRemarks.Focus();
                return IsValid;
            }

            else
            {
                txtRemarks.ClearValidationError();
                IsValid = true;
            }

            if (cmbDoctor.SelectedItem == null)
            {
                IsValid = false;
                cmbDoctor.TextBox.SetValidation("Please select Doctor");
                cmbDoctor.TextBox.RaiseValidationError();
                cmbDoctor.Focus();
                return IsValid;
            }
            else if (((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
            {
                IsValid = false;
                cmbDoctor.TextBox.SetValidation("Please select Doctor");
                cmbDoctor.TextBox.RaiseValidationError();
                cmbDoctor.Focus();
                return IsValid;
            }
            else
            {
                cmbDoctor.TextBox.ClearValidationError();
                IsValid = true;
            }

            return IsValid;

        }


    }
}

