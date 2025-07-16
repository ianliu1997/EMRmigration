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
using MessageBoxControl;
using CIMS;
using PalashDynamics.Service.EmailServiceReference;
using PalashDynamics.ValueObjects.Pathology;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Text;

namespace PalashDynamics.Pathology
{
    public partial class SendMailChildWindow : ChildWindow
    {
        public clsPathOrderBookingVO SelectedDetails { get; set; }
        public clsPathOrderBookingDetailVO SelectedTestDetails { get; set; }

        public string Email { get; set; }
        string ContactNo = string.Empty;
        public event RoutedEventHandler OnSaveButtonClick;

        string PatientMailID = string.Empty;
        string DrMailIDMailID = string.Empty;

        public SendMailChildWindow()
        {
            InitializeComponent();
        }

        private void SendMailChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TxtPatientMailId.Text = SelectedDetails.PatientEmailId;
            PatientMailID = SelectedDetails.PatientEmailId;
            Email = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Email;

            FillSendMailTo();
            FillDoctorList();
            FillContactNo();
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

        private void FillContactNo()
        {
            clsGetUnitContactNoBizActionVO BizAction = new clsGetUnitContactNoBizActionVO();
            BizAction.UnitID = SelectedDetails.UnitId;  // iUnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    ContactNo = ((clsGetUnitContactNoBizActionVO)e.Result).ContactNo;

                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FillDoctorList()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = SelectedDetails.UnitId;  // iUnitID;

            //if ((MasterListItem)cmbDepartment.SelectedItem != null)
            //{
            BizAction.DepartmentId = 0;  // iDeptID;
            //}

            //if ((MasterListItem)cmbVisitType.SelectedItem != null && (MasterListItem)cmbDepartment.SelectedItem != null)
            //{
            //    //if (((MasterListItem)cmbDepartment.SelectedItem).ID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID && ((MasterListItem)cmbDepartment.SelectedItem).ID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID)
            //    //{
            //    BizAction.ServiceId = ((MasterListItem)cmbVisitType.SelectedItem).FilterID;
            //    //}
            //}

            //BizAction.IsServiceWiseDoctorList = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);

                    //if (iDeptID == 0)
                    //{
                    //    txtReferenceDoctor.ItemsSource = null;
                    //    txtReferenceDoctor.ItemsSource = objList;
                    //}
                    //else
                    //{
                    cmbDoctors.ItemsSource = null;

                    cmbDoctors.ItemsSource = objList;


                    cmbDoctors.SelectedItem = ((List<MasterListItem>)cmbDoctors.ItemsSource).First(dd => dd.ID == SelectedDetails.DoctorID); //SelectedDetails.ReferredDoctorID

                    //    if (this.DataContext != null)
                    //    {
                    //        if (UsePrevDoctorID == true)
                    //        {
                    //            cmbDoctor.SelectedValue = ((clsVisitVO)this.DataContext).DoctorID;
                    //            FillDoctorScheduleWise();
                    //            UsePrevDoctorID = false;

                    //            if (((clsVisitVO)this.DataContext).DoctorID == 0)
                    //            {
                    //                cmbDoctor.TextBox.SetValidation("Please select the Doctor");
                    //                cmbDoctor.TextBox.RaiseValidationError();
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (objList.Count > 0)
                    //            {

                    //                cmbDoctor.SelectedValue = objList[0].ID;

                    //                FillDoctorScheduleWise();

                    //                if (cmbDoctor.SelectedItem != null && ((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
                    //                {
                    //                    cmbDoctor.TextBox.SetValidation("Please select the Doctor");
                    //                    cmbDoctor.TextBox.RaiseValidationError();
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

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

                if (SendMail)
                {
                    //if (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder.ReferredByEmailID != null)
                    //{

                    Uri address = new Uri(Application.Current.Host.Source, "../EmailService.svc"); // this url will work both in dev and after deploy
                    EmailServiceClient EmailClient = new EmailServiceClient("CustomBinding_EmailService", address.AbsoluteUri);

                    //EmailServiceClient EmailClient = new EmailServiceClient();

                    EmailClient.SendEmailwithAttachmentForPathologyCompleted += (sa, arg1) =>
                    {
                        if (arg1.Error == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Email send successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW3.Show();

                            if (OnSaveButtonClick != null)
                            {
                                OnSaveButtonClick(sender, e);
                            }
                            this.DialogResult = true;
                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Email sending fail", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW2.Show();
                        }
                    };
                    string Doctor = "Dr." + ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder.ReferredBy;
                    string Patient = SelectedDetails.PatientName; //((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientName;

                    StringBuilder emailBody = new StringBuilder();
                    emailBody.Append("<p><span class='Normal'>Dear </span><span style='font-weight:bold;'>" + Patient + ",</span></p>");
                    emailBody.Append("<p><span class='Normal'>Please find attached your investigation reports, as requested, with this email. </span></p>");
                    emailBody.Append("<p><span class='Normal'>Please visit or call us to discuss the same with our doctor or address any concerns – we will be happy to help the best we can. </span></p>");
                    emailBody.Append("<br/>");
                    emailBody.Append("<p><span class='Normal'>Take Care</span></p>");
                    emailBody.Append("<p><span class='Normal'>Healthspring " + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitName + "</span></p>");
                    emailBody.Append("<p><span class='Normal'>Phone Number : " + ContactNo + "</span></p>");
                    emailBody.Append("<p><span class='Normal'></span></p>");

                    //string emailBody = "Please find an attachment of Pathology Report Of " + Patient;

                    ObservableCollection<string> AttachmentList = new ObservableCollection<string>();
                    //long BillID;
                    long UnitID = SelectedDetails.UnitId;  // ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;

                    //if (TabTemplateDetails.Visibility == Visibility.Visible && PrintTemplate == true)
                    //{
                    //    BillID = ((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.TemplateDetails.ID;
                    //}
                    //else
                    //{
                    //    BillID = ((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.ID;
                    //}

                    //BillID =  ResultID;

                    //AttachmentList.Add(UnitID + "_" + BillID + ".pdf");

                    //Uri address = new Uri(Application.Current.Host.Source, "EmailTemplateAttachment");
                    //string url = address.ToString() + "\\" + SelectedTestDetails.SourceURL;

                    AttachmentList.Add(SelectedTestDetails.SourceURL);

                    //string ToEmail = "";

                    //if(TxtPatientMailId.Text.Trim() != "")
                    //{
                    //    ToEmail = TxtPatientMailId.Text.Trim();
                    //}

                    //if(ToEmail == "")
                    //{
                    //    if(TxtDoctorMailId.Text.Trim() != "")
                    //    {
                    //        ToEmail = TxtDoctorMailId.Text.Trim();
                    //    }
                    //}
                    //else
                    //{
                    //    if(TxtDoctorMailId.Text.Trim() != "")
                    //    {
                    //        ToEmail = ToEmail + "," + TxtDoctorMailId.Text.Trim();
                    //    }
                    //}



                    // EmailClient.SendEmailAsync(Email, ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder.ReferredByEmailID, " Pathology Report", Subject);
                    //EmailClient.SendEmailwithAttachmentAsync(Email, ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder.ReferredByEmailID, " Pathology Report", Subject, AttachmentList.Count(), AttachmentList);

                    //EmailClient.SendEmailwithAttachmentAsync(Email, ToEmail, " Pathology Report", Subject, AttachmentList.Count(), AttachmentList);
                    EmailClient.SendEmailwithAttachmentForPathologyAsync(Email, ToEmail, " Pathology Report", emailBody.ToString(), AttachmentList.Count(), AttachmentList, SelectedDetails.UnitId);
                    EmailClient.CloseAsync();
                    //}

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
            //if (OnCancelButtonClick != null)
            //{
            //    OnCancelButtonClick(sender, e);
            //}
            this.DialogResult = false;
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

