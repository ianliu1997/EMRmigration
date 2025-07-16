using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MessageBoxControl;
using CIMS;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.UserControls;

namespace PalashDynamics.Pathology
{
    public partial class frmChdSendMail : ChildWindow
    {
        public clsPathOrderBookingVO SelectedDetails { get; set; }
        //     public clsPathOrderBookingDetailVO SelectedTestDetails { get; set; }
        public List<clsPathOrderBookingDetailVO> SelectedTestlst = new List<clsPathOrderBookingDetailVO>();
        WaitIndicator indicator = new WaitIndicator();


        public string Email { get; set; }
        string ContactNo = string.Empty;
        public event RoutedEventHandler OnSaveButtonClick;
        public event RoutedEventHandler OnCancelButtonClick;

        string PatientMailID = string.Empty;
        string DrMailIDMailID = string.Empty;

        public frmChdSendMail()
        {
            InitializeComponent();
        }

        private void SendMailChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            indicator.Show();
            FillSendMailTo();
            TxtPatientMailId.Text = SelectedDetails.PatientEmailId;
            PatientMailID = SelectedDetails.PatientEmailId;
            Email = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Email;
            FillDoctorList();


        }


        private void FillSendMailTo()
        {

            List<MasterListItem> lst = new List<MasterListItem>();
            lst.Add(new MasterListItem() { ID = 0, Description = "--Select--", Status = true });
            lst.Add(new MasterListItem() { ID = 1, Description = "Patient", Status = true });
            lst.Add(new MasterListItem() { ID = 2, Description = "Doctor", Status = true });
            lst.Add(new MasterListItem() { ID = 3, Description = "Both", Status = true });
            cboMailSendTo.ItemsSource = lst;
            cboMailSendTo.SelectedItem = lst[0];
        }

        private void FillDoctorList()
        {
            try
            {
                clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                BizAction.UnitId = SelectedDetails.UnitId;

                BizAction.DepartmentId = 0;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);

                        cmbDoctors.ItemsSource = null;

                        cmbDoctors.ItemsSource = objList;
                        cmbDoctors.SelectedItem = ((List<MasterListItem>)cmbDoctors.ItemsSource).First(dd => dd.ID == SelectedDetails.DoctorID);

                    }
                    indicator.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                indicator.Close();
            }

        }



        private void cmdSend_Click(object sender, RoutedEventArgs e)
        {

            bool SendMail = true;

            string ToEmail = "";

            if (TxtDoctorMailId.Text.Trim().Length > 0 && (!TxtDoctorMailId.Text.IsEmailValid()))
            {
                TxtDoctorMailId.SetValidation("Please enter valid Email");
                TxtDoctorMailId.RaiseValidationError();
                TxtDoctorMailId.Focus();
            }
            else if (TxtPatientMailId.Text.Trim().Length > 0 && (!TxtPatientMailId.Text.IsEmailValid()))
            {
                TxtPatientMailId.SetValidation("Please enter valid Email");
                TxtPatientMailId.RaiseValidationError();
                TxtPatientMailId.Focus();
            }
            else
            {

                if (cboMailSendTo.SelectedItem == null || ((MasterListItem)cboMailSendTo.SelectedItem).ID == 0)
                {
                    SendMail = false;

                    MessageBoxChildWindow mgbx1 = new MessageBoxChildWindow("Palash", "Please Select Send Mail To.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx1.Show();
                }

                if (TxtPatientMailId.Text == "" && (((MasterListItem)cboMailSendTo.SelectedItem).ID == 1 || ((MasterListItem)cboMailSendTo.SelectedItem).ID == 3))
                {
                    SendMail = false;

                    MessageBoxChildWindow mgbx2 = new MessageBoxChildWindow("Palash", "Please Enter Patient mail id to send mail.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx2.Show();
                }
                else
                {
                    if (((MasterListItem)cboMailSendTo.SelectedItem).ID == 1 || ((MasterListItem)cboMailSendTo.SelectedItem).ID == 3)
                    {
                        if (SendMail == true)
                            ToEmail = TxtPatientMailId.Text.Trim();
                    }
                }

                if (TxtDoctorMailId.Text == "" && (((MasterListItem)cboMailSendTo.SelectedItem).ID == 2 || ((MasterListItem)cboMailSendTo.SelectedItem).ID == 3))
                {
                    SendMail = false;

                    MessageBoxChildWindow mgbx3 = new MessageBoxChildWindow("Palash", "Please Enter Doctor mail id to send mail.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx3.Show();
                }
                else
                {
                    if (((MasterListItem)cboMailSendTo.SelectedItem).ID == 2 || ((MasterListItem)cboMailSendTo.SelectedItem).ID == 3)
                    {
                        if (SendMail == true)
                        {
                            if (ToEmail == "")
                            {
                                if (TxtDoctorMailId.Text.Trim() != "")
                                {
                                    ToEmail = TxtDoctorMailId.Text.Trim();
                                }
                            }
                            else
                            {
                                if (TxtDoctorMailId.Text.Trim() != "")
                                {
                                    ToEmail = ToEmail + "," + TxtDoctorMailId.Text.Trim();
                                }
                            }
                        }
                    }
                }

                string Doctor = "Dr." + ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder.ReferredBy;
                string Patient = SelectedDetails.PatientName;
                if (SendMail)
                {
                    try
                    {
                        clsAddPathPatientReportDetailsForEmailSendingBizActionVO BizAction = new clsAddPathPatientReportDetailsForEmailSendingBizActionVO();

                        BizAction.PathOrderBookList = SelectedTestlst;
                        BizAction.PatientID = SelectedDetails.PatientID;
                        BizAction.PatientUnitID = SelectedDetails.PatientUnitID;
                        BizAction.PatientEmailID = TxtPatientMailId.Text;
                        BizAction.DoctorEmailID = TxtDoctorMailId.Text;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {

                            if (arg.Error == null)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Email request sent successfully to the server", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.OK)
                                    {
                                        this.DialogResult = true;

                                        if (OnSaveButtonClick != null)
                                            OnSaveButtonClick(this, new RoutedEventArgs());
                                    }
                                };
                                msgW1.Show();

                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while Sending Report.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();
                            }

                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }

                    catch (Exception)
                    {

                    }

                }
            }

        }

        private void cmbDoctors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDoctors.SelectedItem != null)
            {
                if (((MasterListItem)cmbDoctors.SelectedItem).ID > 0)
                {
                    TxtDoctorMailId.Text = ((MasterListItem)cmbDoctors.SelectedItem).Code;
                }
            }
        }

        private void cboMailSendTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cboMailSendTo.SelectedItem != null)
            {
                if (((MasterListItem)cboMailSendTo.SelectedItem).ID == 1)
                {
                    TxtPatientMailId.IsReadOnly = false;
                    cmbDoctors.IsEnabled = false;
                    TxtDoctorMailId.Text = "";
                    TxtDoctorMailId.IsReadOnly = true;
                    TxtPatientMailId.Text = PatientMailID;
                }
                else if (((MasterListItem)cboMailSendTo.SelectedItem).ID == 2)
                {

                    TxtDoctorMailId.IsReadOnly = false;
                    TxtPatientMailId.Text = PatientMailID;
                    TxtPatientMailId.IsReadOnly = true;
                    cmbDoctors.IsEnabled = true;
                    TxtPatientMailId.Text = "";
                    if (TxtDoctorMailId.Text == "")
                    {
                        cmbDoctors.SelectedItem = ((List<MasterListItem>)cmbDoctors.ItemsSource).First(dd => dd.ID == 0);
                        cmbDoctors.SelectedItem = ((List<MasterListItem>)cmbDoctors.ItemsSource).First(dd => dd.ID == SelectedDetails.DoctorID);

                    }

                }
                else if (((MasterListItem)cboMailSendTo.SelectedItem).ID == 3)
                {

                    TxtDoctorMailId.IsReadOnly = false;
                    TxtPatientMailId.IsReadOnly = false;
                    cmbDoctors.IsEnabled = true;
                    TxtPatientMailId.Text = PatientMailID;
                    if (TxtDoctorMailId.Text == "")
                    {
                        cmbDoctors.SelectedItem = ((List<MasterListItem>)cmbDoctors.ItemsSource).First(dd => dd.ID == 0);
                        cmbDoctors.SelectedItem = ((List<MasterListItem>)cmbDoctors.ItemsSource).First(dd => dd.ID == SelectedDetails.DoctorID);

                    }
                }
                else
                {
                    TxtDoctorMailId.IsReadOnly = false;
                    TxtPatientMailId.IsReadOnly = false;
                }
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCancelButtonClick != null)
                OnCancelButtonClick(this, new RoutedEventArgs());
        }

        private void TxtPatientMailId_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Text.Length > 0)
            {
                if (txt.Text.IsEmailValid())
                    txt.ClearValidationError();
                else
                {
                    txt.SetValidation("Please enter valid Email");
                    txt.RaiseValidationError();
                }
            }
        }

        private void TxtDoctorMailId_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Text.Length > 0)
            {
                if (txt.Text.IsEmailValid())
                    txt.ClearValidationError();
                else
                {
                    txt.SetValidation("Please enter valid Email");
                    txt.RaiseValidationError();
                }
            }
        }

    }
}

