using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using CIMS;
using System.Windows.Browser;

namespace PalashDynamics.Radiology
{
    public partial class RadiologyUploadReportChildWindow : ChildWindow
    {
        byte[] data;
        FileInfo fi;
        public string msgTitle;
        public string msgText;
        public long ResultID;
        public bool IsResultEntry { get; set; }
        public bool OrderID { get; set; }

        public RadiologyUploadReportChildWindow()
        {
            InitializeComponent();
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    //do  nothing
                }
                else
                    cmdSave.IsEnabled = false;
            }

        }

        private bool CheckValidations()
        {
            bool result = true;
            try
            {
                if (TxtReportPath.Text == "" || TxtReportPath.Text == null)
                {
                    TxtReportPath.SetValidation("Report File Path is required.");
                    TxtReportPath.RaiseValidationError();
                    TxtReportPath.Focus();
                    result = false;
                }
                else
                    TxtReportPath.ClearValidationError();


                if (RptRcdTime.Value == null)
                {
                    RptRcdTime.SetValidation("Report Received Time is required.");
                    RptRcdTime.RaiseValidationError();
                    RptRcdTime.Focus();
                    result = false;
                }
                else
                    RptRcdTime.ClearValidationError();


                if (RptRcdDate.SelectedDate == null)
                {
                    RptRcdDate.SetValidation("Report Received Date is required.");
                    RptRcdDate.RaiseValidationError();
                    RptRcdDate.Focus();
                    result = false;
                }
                else
                    RptRcdDate.ClearValidationError();


            }
            catch (Exception ex)
            {

            }
            return result;
        }

        void UploadReportChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RptRcdDate.SelectedDate = (DateTime?)DateTime.Now;
            RptRcdTime.Value = (DateTime?)DateTime.Now;
            CheckValidations();
            TxtReportPath.Focus();
            TxtReportPath.RaiseValidationError();
        }

        public event RoutedEventHandler OnSaveButtonClick;

        public bool Validate()
        {
            if (RptRcdDate.SelectedDate == null)
            {
                msgText = "Report Received Date Cannot be left blank. Please Select Date.";

                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();

                return false;
            }
            if (RptRcdTime.Value == null)
            {
                msgText = "Report Received Time Cannot be left blank. Please Select Time.";

                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();

                return false;
            }
            if (TxtReportPath.Text == "")
            {
                msgText = "File Path Cannot be left blank. Please Select File.";

                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();

                return false;
            }
            return true;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool valid = CheckValidations();

            if (valid)
            {
                //DateTime dt=new DateTime();


                clsPathOrderBookingDetailVO ob = (clsPathOrderBookingDetailVO)this.DataContext;

                clsPathoUploadReportBizActionVO BizAction = new clsPathoUploadReportBizActionVO();
                BizAction.UploadReportDetails = new clsPathPatientReportVO();
                BizAction.UploadReportDetails.SampleNo = (string)ob.SampleNo;
                BizAction.UploadReportDetails.SourceURL = "Rpt_TestID" + ob.ID + fi.Extension;
                BizAction.UploadReportDetails.Report = data;
                BizAction.UploadReportDetails.PathOrderBookingDetailID = ob.ID;
                BizAction.UploadReportDetails.PathPatientReportID = ob.PathPatientReportID;

                BizAction.UploadReportDetails.Time = RptRcdTime.Value;
                BizAction.UploadReportDetails.Notes = TxtNotes.Text;
                BizAction.UploadReportDetails.Remarks = TxtRemarks.Text;
                BizAction.IsResultEntry = IsResultEntry;
                if (IsResultEntry == false)
                    BizAction.UploadReportDetails.OrderID = ob.OrderBookingID;

                if (BizAction.UnitID == 0)
                    BizAction.UnitID = ob.UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {

                            if (OnSaveButtonClick != null)
                            {
                                OnSaveButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), new RoutedEventArgs());
                            }
                            this.DialogResult = true;
                        }
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            RptRcdDate.ClearValidationError();
            RptRcdTime.ClearValidationError();
            TxtReportPath.ClearValidationError();
            this.DialogResult = false;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == true)
            {
                TxtReportPath.Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);
                        fi = openDialog.File;
                    }
                }
                catch (Exception ex)
                {
                    msgText = "Error while reading file.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();

                }
            }
        }

        private void btnUpload_Click(string str)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.UploadReportFileCompleted += (s, args) =>
            {
                if (args.Error == null)
                {
                    if (OnSaveButtonClick != null)
                    {
                        OnSaveButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), new RoutedEventArgs());
                    }
                    this.DialogResult = true;
                }
            };
            client.UploadReportFileAsync(str, data);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ViewLink_Click(object sender, RoutedEventArgs e)
        {
            if (ResultID > 0)
            {
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                string URL = "../Reports/Pathology/PathoResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
    }
}

