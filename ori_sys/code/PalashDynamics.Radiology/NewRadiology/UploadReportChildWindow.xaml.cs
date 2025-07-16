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
using System.IO;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;




namespace PalashDynamics.Radiology
{
    public partial class UploadReportChildWindow : ChildWindow
    {
        byte[] data;
        FileInfo fi;
        List<string> pdfFiles = new List<string>();
        bool IsUpload = false;
        public string msgTitle;
        public string msgText = "";
        public long ResultID;
        public bool IsResultEntry { get; set; }
        public bool OrderID { get; set; }
        public clsRadOrderBookingDetailsVO selectedTest { get; set; }
        public List<clsRadOrderBookingDetailsVO> ObjList = new List<clsRadOrderBookingDetailsVO>();
        clsRadResultEntryVO ResultEntryDetails { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public UploadReportChildWindow()
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

        private void UploadReportChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
         //  GetResultEntry();
            if (ObjList == null)
                ObjList = new List<clsRadOrderBookingDetailsVO>();
            RptRcdDate.SelectedDate = (DateTime?)DateTime.Now;
            RptRcdTime.Value = (DateTime?)DateTime.Now;
            CheckValidations();
            TxtReportPath.Focus();
            TxtReportPath.RaiseValidationError();
        }

        private bool CheckValidations()
        {
            bool result = true;
            try
            {
                if (TxtReportPath.Text == "" || TxtReportPath.Text == null)
                {
                    msgText = "Report File Path is required";
                    TxtReportPath.SetValidation(msgText);
                    TxtReportPath.RaiseValidationError();
                    TxtReportPath.Focus();
                    result = false;
                }
                else
                    TxtReportPath.ClearValidationError();


                if (RptRcdTime.Value == null)
                {
                    msgText = "Report Received Time is required";
                    RptRcdTime.SetValidation(msgText);
                    RptRcdTime.RaiseValidationError();
                    RptRcdTime.Focus();
                    result = false;
                }
                else
                    RptRcdTime.ClearValidationError();


                if (RptRcdDate.SelectedDate == null)
                {
                    msgText = "Report Received Date is required";
                    RptRcdDate.SetValidation(msgText);
                    RptRcdDate.RaiseValidationError();
                    RptRcdDate.Focus();
                    result = false;
                }
                else
                    RptRcdDate.ClearValidationError();
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        public event RoutedEventHandler OnSaveButtonClick;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        //private void GetResultEntry()
        //{

        //   // clsRadOrderBookingDetailsVO ob = (clsRadOrderBookingDetailsVO)this.DataContext;


        //    clsGetRadResultEntryBizActionVO BizAction = new clsGetRadResultEntryBizActionVO();
        //    //BizAction.ID = selectedTest.RadOrderID;
        //    //BizAction.DetailID = selectedTest.ID;
        //    //BizAction.UnitID = selectedTest.UnitID;
        // //  BizAction.ID = ((clsRadOrderBookingVO)dgtest.SelectedItem).ID;
        //    //    BizAction.DetailID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ID;
        //    //    BizAction.UnitID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).UnitID;
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null)
        //        {
        //            if (((clsGetRadResultEntryBizActionVO)arg.Result).ResultDetails != null)
        //            {
        //                clsRadResultEntryVO ObjDetails;
        //                ObjDetails = ((clsGetRadResultEntryBizActionVO)arg.Result).ResultDetails;

        //                if (ObjDetails != null)
        //                {
        //                    ResultEntryDetails = ObjDetails;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            msgText = "Error Occurred while processing";
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                   new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //            msgW1.Show();
        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}

        //private void GetResultEntry()
        //{

        //    //  GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
        //    clsGetRadResultEntryBizActionVO BizAction = new clsGetRadResultEntryBizActionVO();
        //    BizAction.ID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ID;
        //    BizAction.DetailID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ID;
        //    BizAction.UnitID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).UnitID;
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && ((clsGetRadResultEntryBizActionVO)arg.Result).ResultDetails != null)
        //        {
        //            clsRadResultEntryVO ObjDetails;
        //            ObjDetails = ((clsGetRadResultEntryBizActionVO)arg.Result).ResultDetails;
        //            if (ObjDetails != null)
        //            {
        //                this.DataContext = ObjDetails;
        //                cmbFilm.SelectedValue = ObjDetails.FilmID;
        //                cmbRadiologist.SelectedValue = ObjDetails.RadiologistID1;
        //                cmbResult.SelectedValue = ObjDetails.TemplateResultID;
        //                cmbTemplate.SelectedValue = ObjDetails.TemplateID;
        //                richTextBox.Html = ObjDetails.FirstLevelDescription;
        //                ChkReferDoctorSignature.IsChecked = ObjDetails.IsDigitalSignatureRequired;
        //                if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
        //                    richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html + "[%DOCTORINFO%]";
        //                txtReferenceDoctor.Text = ObjDetails.ReferredBy;
        //                txtPatientName.Text = ObjDetails.PatientName;

        //            }
        //            if (ObjDetails.TestItemList != null)
        //            {
        //                List<clsRadItemDetailMasterVO> ObjItem;
        //                ObjItem = ((clsGetRadResultEntryBizActionVO)arg.Result).TestItemList; ;
        //                foreach (var item4 in ObjItem)
        //                {
        //                    cmbStore.SelectedValue = item4.StoreID;
        //                    ItemList.Add(item4);
        //                }
        //                dgItemDetailsList.ItemsSource = null;
        //                dgItemDetailsList.ItemsSource = ItemList;
        //            }

        //        }
        //        else
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //            msgW1.Show();
        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}
        int ClickedFlag = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            Uploadfile();

            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                try
                {
                    bool valid = CheckValidations();

                    if (valid)
                    {
                     
                        DateTime dt = new DateTime();
                        clsRadOrderBookingDetailsVO ob = (clsRadOrderBookingDetailsVO)this.DataContext;
        
                      clsRadUploadReportBizActionVO BizAction = new clsRadUploadReportBizActionVO();
                        BizAction.UploadReportDetails = new clsRadPatientReportVO();
                       // BizAction.UploadReportDetails.SampleNo = (string)ob.SampleNo;
                        BizAction.UploadReportDetails.SourceURL = "OrderDetailID_" + ob.ID + "PatientId_" + ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.PatientID + "UnitID_" + ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.UnitID;
                        BizAction.UploadReportDetails.Report = data;
                    //  BizAction.UploadReportDetails.RadOrderDetailID = ob.RadOrderDetailID;
                    //  BizAction.UploadReportDetails.RadPatientReportID = ob.RadPatientReportID;

                        BizAction.UploadReportDetails.RadOrderDetailID = ob.ID;
                        BizAction.UploadReportDetails.RadPatientReportID = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.PatientID;

                       string reportfileName="TestID"+ob.TestID;
                       //reportfileName = BizAction.UploadReportDetails.SourceURL + reportfileName;

                       BizAction.UploadReportDetails.ReportPath ="OrderDetailID" + ob.ID + "PatientId" + ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.PatientID +"TestID"+ob.TestID+ TxtReportPath.Text;
                           
                           //TxtReportPath.Text + BizAction.UploadReportDetails.SourceURL + " TestID_" + ob.TestID;

                      BizAction.UploadReportDetails.TestID = ob.TestID;//Added By Yogesh k 06-06-16
                      BizAction.UploadReportDetails.RadiologistID1 = ob.RadiologySpecilizationID;
                      BizAction.UploadReportDetails.ReferredBy = ob.ReferredBy;
                        BizAction.UploadReportDetails.Time = RptRcdTime.Value;
                        BizAction.UploadReportDetails.Notes = TxtNotes.Text;
                        BizAction.UploadReportDetails.Remarks = TxtRemarks.Text;
                        BizAction.IsResultEntry = IsResultEntry;
                       // if (IsResultEntry == false)
                            BizAction.UploadReportDetails.RadOrderID = ob.RadOrderID;

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
                                       // OnSaveButtonClick((clsRadOrderBookingDetailsVO)(this.DataContext), new RoutedEventArgs());
                                    }
                                    this.DialogResult = true;

                                }
                            }
                        };

                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                }
                catch (Exception)
                {
                    ClickedFlag = 0;
                    throw;
                }


            







            //bool valid = CheckValidations();
            //if (valid)
            //{
            //  //  DateTime dt = new DateTime();
            //    DateTime dt = new DateTime();

            //    clsRadOrderBookingDetailsVO ob = (clsRadOrderBookingDetailsVO)this.DataContext;
            //   // clsRadiologyVO ob1 = (clsRadiologyVO)this.DataContext;
            //    clsAddRadResultEntryBizActionVO BizAction = new clsAddRadResultEntryBizActionVO();
            //    if (ResultEntryDetails != null)
            //    {
            //        BizAction.ResultDetails = ResultEntryDetails;
            //        //BizAction.IsUpload = false;
            //        BizAction.ResultDetails.IsFinalized = selectedTest.IsFinalized;
            //    }
            //    else
            //    {
            //        BizAction.ResultDetails = new clsRadResultEntryVO();
            //        //BizAction.IsUpload = true;
            //    }
            //    BizAction.IsUpload = true;
            //    BizAction.ResultDetails.ID = selectedTest.ResultEntryID;
            //    BizAction.ResultDetails.RadOrderID = selectedTest.RadOrderID;
            //    BizAction.ResultDetails.BookingDetailsID = selectedTest.ID;
            //    BizAction.ResultDetails.TestID = selectedTest.TestID;
            //    BizAction.ResultDetails.SourceURL = "Order Details ID" + selectedTest.ID + "Order Id" + selectedTest.RadOrderID + "UnitID" + selectedTest.UnitID + "Result Entry ID " + selectedTest.ResultEntryID + fi.Name;
            //    BizAction.ResultDetails.Report = data;
            //    BizAction.ResultDetails.Time = (DateTime)RptRcdTime.Value;
            //    BizAction.ResultDetails.Notes = TxtNotes.Text;
            //    BizAction.ResultDetails.Remarks = TxtRemarks.Text;
            //    BizAction.ResultDetails.IsCompleted = IsUpload;
            //   // BizAction.IsResultEntry = false;
            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null)
            //        {
            //            if (arg.Result != null)
            //            {
            //                if (OnSaveButtonClick != null)
            //                {
            //                    OnSaveButtonClick((clsRadOrderBookingDetailsVO)(this.DataContext), new RoutedEventArgs());
            //                }
            //                this.DialogResult = true;
            //            }
            //        }
            //    };
            //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    client.CloseAsync();
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            RptRcdDate.ClearValidationError();
            RptRcdTime.ClearValidationError();
            TxtReportPath.ClearValidationError();
            this.DialogResult = false;
        }

        private void ViewLink_Click(object sender, RoutedEventArgs e)
        {
            if (ResultID > 0)
            {
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                string URL = "../Reports/Radiology/RadiologyResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Multiselect = true;
            //openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";        

            //if (openFileDialog.ShowDialog() == true)//System.Windows.Forms.DialogResult.OK
            //{
            //    TxtReportPath.Text = openFileDialog.File.Name;
            //    pdfFiles = new List<string>();
            //    //foreach (string fileName in openFileDialog.File)
            //    pdfFiles.Add(openFileDialog.File.Name);
            //}
            //string installedPath = @"C:\Users\yogeshk\Documents\docs";             
                   
            //if (!Directory.Exists(installedPath))
            //{
             
            // Directory.CreateDirectory(installedPath);
              
            //}
          
            //foreach (string sourceFileName in pdfFiles)
            //{
            //    string destinationFileName = System.IO.Path.Combine(installedPath, System.IO.Path.GetFileName(sourceFileName));
            //    System.IO.File.Copy(sourceFileName, destinationFileName);
            //}


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
                        IsUpload = true;
                    }
                }
                catch (Exception ex)
                {
                    msgText = "Error while reading file";
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        private void btnUpload_Click(string str)
        {
           
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnUpload_Click_1(object sender, RoutedEventArgs e)
        {
           
        }


        private void Uploadfile()
        {
            clsRadOrderBookingDetailsVO ob = (clsRadOrderBookingDetailsVO)this.DataContext;

            clsRadUploadReportBizActionVO BizAction = new clsRadUploadReportBizActionVO();
            BizAction.UploadReportDetails = new clsRadPatientReportVO();
            // BizAction.UploadReportDetails.SampleNo = (string)ob.SampleNo;

            BizAction.UploadReportDetails.SourceURL = "OrderDetailID_" + ob.ID + "PatientId_" + ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.PatientID + "UnitID_" + ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.UnitID;

            string str;
            str = "OrderDetailID" + ob.ID + "PatientId" + ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.PatientID+"TestID"+ob.TestID + TxtReportPath.Text;
               
                //TxtReportPath.Text;
            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.UploadReportFileForRadiologyCompleted += (s, args) =>
            {
                if (args.Error == null)
                {
                    if (OnSaveButtonClick != null)
                    {
                         OnSaveButtonClick((clsRadOrderBookingDetailsVO)(this.DataContext), new RoutedEventArgs());
                    }
                     this.DialogResult = true;
                }
            };
            client.UploadReportFileForRadiologyAsync(str, data);
        }

       

       
    }
}

