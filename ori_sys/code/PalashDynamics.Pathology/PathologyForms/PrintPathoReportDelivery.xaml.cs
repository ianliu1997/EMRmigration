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
using PalashDynamics.ValueObjects.Pathology;
using System.Text;
using System.IO;
//using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Printing;
using C1.Silverlight.RichTextBox.PdfFilter;
using CIMS;
using C1.Silverlight.Pdf;
using C1.Silverlight;
using C1.Silverlight.RichTextBox;
using PalashDynamics.Service.PalashTestServiceReference;
using C1.Silverlight.RichTextBox.Documents;
using System.Windows.Browser;
using System.Windows.Media.Imaging;
using System.Globalization;


//using System.Drawing;
//using System.Drawing.Imaging;


namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class PrintPathoReportDelivery : ChildWindow
    {
        public long? IsOpdIpd { get; set; }
        public long OrderUnitID { get; set; }
        public long ResultId { get; set; }
        public bool ISFinalize { get; set; }
        public long UnitID { get; set; }
        public bool IsFromReportDelivery { get; set; }
        public bool IsDuplicate = false;
        public clsPathOrderBookingDetailVO ObjDetails { get; set; }
        public clsPathOrderBookingVO OrderDetails { get; set; }
        StringBuilder strPatInfo;
        StringBuilder strDoctorPathInfo;
        List<clsPathOrderBookingDetailVO> SelectedTestList { get; set; }

        long PrintID = 0;
        public String TemplateContent = String.Empty;

        public PrintPathoReportDelivery()
        {
            InitializeComponent();
            SelectedTestList = new List<clsPathOrderBookingDetailVO>();
            this.Loaded += new RoutedEventHandler(PrintPathoReportDelivery_Loaded);
        }

        void PrintPathoReportDelivery_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsFromReportDelivery == true)
            {
                SavePDF.Visibility = Visibility.Visible;
            }
            else
            {
                SavePDF.Visibility = Visibility.Collapsed;
            }
            if (ObjDetails != null)
            {
                SelectedTestList.Add(ObjDetails);
            }
            GetPatientPathoDetailsInHtml(ResultId, ISFinalize);
        }
        clsPathoResultEntryPrintDetailsVO PatientResultEntry = new clsPathoResultEntryPrintDetailsVO();
        private void GetPatientPathoDetailsInHtml(long ResultId, bool IsFinalize)
        {
            clsPathoResultEntryPrintDetailsBizActionVO BizAction = new clsPathoResultEntryPrintDetailsBizActionVO();

            BizAction.ID = ResultId;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.IsDelivered = 0;
            BizAction.IsOpdIpd = IsOpdIpd; //by bhushan and rohinee for ipd
            BizAction.OrderUnitID = OrderUnitID;

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsPathoResultEntryPrintDetailsBizActionVO)arg.Result).ResultDetails != null)
                        {
                            PatientResultEntry = new clsPathoResultEntryPrintDetailsVO();
                            PatientResultEntry = ((clsPathoResultEntryPrintDetailsBizActionVO)arg.Result).ResultDetails;


                            strPatInfo = new StringBuilder();

                            // strPatInfo.Append(PatientResultEntry.PatientInfoHTML);

                            //strPatInfo = strPatInfo.Replace("[PatName]", "    :" + PatientResultEntry.PatientName.ToString());
                            //strPatInfo = strPatInfo.Replace("[MRNO]", "    :" + PatientResultEntry.MRNo.ToString());
                            //strPatInfo = strPatInfo.Replace("[OrderDate]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy hh:mm tt"));


                            //strPatInfo = strPatInfo.Replace("[Age]", "   :" + PatientResultEntry.AgeYear.ToString() + " Yrs " + PatientResultEntry.AgeMonth.ToString() + " Mth " + PatientResultEntry.AgeDate.ToString() + " Dys");
                            //strPatInfo = strPatInfo.Replace("[Sex]", "    :" + PatientResultEntry.Gender.ToString());

                            //strPatInfo = strPatInfo.Replace("[RefDoc]", "    :" + "Dr. " + PatientResultEntry.ReferredBy.ToString());

                            //strPatInfo = strPatInfo.Replace("[PatientCategory1]", "    :" + PatientResultEntry.PatientCategory.ToString());
                            //strPatInfo = strPatInfo.Replace("[PatientCategory2]", "    :" + PatientResultEntry.PatientSource.ToString());
                            //strPatInfo = strPatInfo.Replace("[CompanyName]", "    :" + PatientResultEntry.Company.ToString());
                            //strPatInfo = strPatInfo.Replace("[PatientContactNo]", "    :" + PatientResultEntry.ContactNo.ToString());
                            //strPatInfo = strPatInfo.Replace("[DSCode]", "    :" + PatientResultEntry.DonarCode.ToString());
                            //strPatInfo = strPatInfo.Replace("[ReferenceNo]", "    :" + PatientResultEntry.ReferenceNo.ToString());
                            //strPatInfo = strPatInfo.Replace("[BillNo]", "    :" + PatientResultEntry.SampleNo.ToString());
                            //strPatInfo = strPatInfo.Replace("[RPTDATE]", "    :" + PatientResultEntry.SampleCollectionTime.Value.ToString("dd MMM yyyy hh:mm tt"));
                            //strPatInfo = strPatInfo.Replace("[RPTTIME]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy hh:mm tt"));
                            //strPatInfo = strPatInfo.Replace("[TemplateTestName]", PatientResultEntry.Test.ToString());


                            //if (IsDuplicate == true)
                            //{
                            //    strPatInfo = strPatInfo.Replace("[NOTFINALIZED]", "DUPLICATE REPORT");
                            //}
                            //else
                            //{
                            //    strPatInfo = strPatInfo.Replace("[NOTFINALIZED]", "");
                            //}

                            //strPatInfo = strPatInfo.Replace("[ClinicName]", "    " + PatientResultEntry.UnitName.Trim());
                            //strPatInfo = strPatInfo.Replace("[AddressLine1]", "    " + PatientResultEntry.AdressLine1.Trim());
                            //strPatInfo = strPatInfo.Replace("[AddressLine2]", "    " + PatientResultEntry.AdressLine1.Trim());
                            //strPatInfo = strPatInfo.Replace("[EmailId]", "     " + PatientResultEntry.Email.Trim());
                            //strPatInfo = strPatInfo.Replace("[TinNo]", "    : " + PatientResultEntry.TinNo.Trim());
                            //strPatInfo = strPatInfo.Replace("[RegNo]", "    : " + PatientResultEntry.RegNo.Trim());
                            //strPatInfo = strPatInfo.Replace("[MobNo]", "    " + PatientResultEntry.UnitContactNo.Trim());

                            //byte[] imageBytes = null;
                            //string imageBase64 = string.Empty;
                            //string imageSrc = string.Empty;

                            //imageBytes = PatientResultEntry.UnitLogo;
                            //imageBase64 = Convert.ToBase64String(imageBytes);
                            //imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);
                            //strPatInfo.Replace("[%Signature2%]", imageSrc);


                            //strPatInfo = strPatInfo.Replace("[Signature2]", "    " + PatientResultEntry.UnitLogo);

                            strDoctorPathInfo = new StringBuilder();
                            strDoctorPathInfo.Append(PatientResultEntry.DoctorInfoHTML);

                            strDoctorPathInfo = strDoctorPathInfo.Replace("[ApprovedDateTime]", "    :" + PatientResultEntry.ApprovedDateTime.Value.ToString("dd MMM yyyy hh:mm tt"));
                            strDoctorPathInfo = strDoctorPathInfo.Replace("[GeneratedDateTime]", "    :" + PatientResultEntry.GeneratedDateTime.Value.ToString("dd MMM yyyy hh:mm tt"));
                            //strDoctorPathInfo = strDoctorPathInfo.Replace("[SubOptimalRemark]", "    :" + PatientResultEntry.SubOptimalRemark.ToString());

                            if (PatientResultEntry.Authorizedby.ToString() != null)
                                PatientResultEntry.Authorizedby = "Dr." + PatientResultEntry.Authorizedby.ToString();

                            strDoctorPathInfo = strDoctorPathInfo.Replace("[Authorizedby]", "    :" + PatientResultEntry.Authorizedby.ToString());
                            strDoctorPathInfo = strDoctorPathInfo.Replace("[Disclaimer]", "    :" + PatientResultEntry.Disclaimer.ToString());

                            //strDoctorPathInfo = strDoctorPathInfo.Replace("[%IsSubOptimal%]", "    :" + PatientResultEntry.IsSubOptimal.ToString());

                            if (PatientResultEntry.IsSubOptimal == true)
                            {
                                strDoctorPathInfo = strDoctorPathInfo.Replace("[SubOptimalRemark]", "    :" + PatientResultEntry.SubOptimalRemark.ToString());
                            }
                            else
                            {
                                strDoctorPathInfo = strDoctorPathInfo.Replace("Sub Optimal",  "  ");
                                strDoctorPathInfo = strDoctorPathInfo.Replace("[SubOptimalRemark]","  ");
                            }

                            strDoctorPathInfo = strDoctorPathInfo.Replace("[ClinicName]", "    " + PatientResultEntry.UnitName.Trim());
                            strDoctorPathInfo = strDoctorPathInfo.Replace("[AddressLine1]", "    " + PatientResultEntry.AdressLine1.Trim());
                            strDoctorPathInfo = strDoctorPathInfo.Replace("[AddressLine2]", "    " + PatientResultEntry.AdressLine1.Trim());
                            strDoctorPathInfo = strDoctorPathInfo.Replace("[EmailId]", "     " + PatientResultEntry.Email.Trim());
                            strDoctorPathInfo = strDoctorPathInfo.Replace("[TinNo]", "    : " + PatientResultEntry.TinNo.Trim());
                            strDoctorPathInfo = strDoctorPathInfo.Replace("[RegNo]", "    : " + PatientResultEntry.RegNo.Trim());
                            strDoctorPathInfo = strDoctorPathInfo.Replace("[MobNo]", "    " + PatientResultEntry.UnitContactNo.Trim());



                            byte[] imageBytesDis = null;
                            string imageBase64Dis = string.Empty;
                            string imageSrcDis = string.Empty;

                            imageBytesDis = PatientResultEntry.DisclaimerImg;
                            imageBase64Dis = Convert.ToBase64String(imageBytesDis);
                            imageSrcDis = string.Format("data:image/jpg;base64,{0}", imageBase64Dis);
                            strDoctorPathInfo.Replace("[%DisclImg%]", imageSrcDis);

                            //byte[] imageBytes = null;
                            //string imageBase64 = string.Empty;
                            //string imageSrc = string.Empty;

                            //if (PatientResultEntry.Pathologist1 != null)
                            //{
                            //    if (PatientResultEntry.Signature1 != null)
                            //    {
                            //        imageBytes = PatientResultEntry.Signature1;

                            //        imageBase64 = Convert.ToBase64String(imageBytes);
                            //        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);
                            //        if (imageSrc != null && imageSrc.Length > 0)
                            //        {
                            //            //if (PatientResultEntry.Pathologist == PatientResultEntry.PathoDoctorid1)  // && ReferDoctorSignature.IsChecked == true)
                            //            //{
                            //            strDoctorPathInfo.Replace("[%Signature4%]", imageSrc);
                            //            //}
                            //        }
                            //    }

                            //    strDoctorPathInfo.Replace("[%Pathalogist4%]", PatientResultEntry.Pathologist1 + " " + '(' + PatientResultEntry.Education1 + ')');
                            //    strDoctorPathInfo.Replace("[%Education4%]", PatientResultEntry.Roles);
                            //    //strDoctorPathInfo.Replace("[%Education4%]", PatientResultEntry.Education1);
                            //    ////if (!string.IsNullOrEmpty(PatientResultEntry.Education1))
                            //    ////{
                            //    ////    string[] parts = PatientResultEntry.Education1.Split(',');
                            //    ////    StringBuilder result = new StringBuilder();
                            //    ////    if (parts.Length == 3)
                            //    ////    {
                            //    ////        for (int i = 0; i < parts.Length; i++)
                            //    ////        {
                            //    ////            result.Append(parts[i]);
                            //    ////            if (i % 3 != 0)
                            //    ////            {
                            //    ////                result.Append("<br />");
                            //    ////            }
                            //    ////            else
                            //    ////            {
                            //    ////                result.Append(',');
                            //    ////            }
                            //    ////        }
                            //    ////        strDoctorPathInfo.Replace("[%Education4%]", result.ToString()); //PatientResultEntry.Education1);
                            //    ////    }
                            //    ////    else
                            //    ////    {
                            //    ////        strDoctorPathInfo.Replace("[%Education4%]", PatientResultEntry.Education1);
                            //    ////    }
                            //    ////}
                            //}
                            //else
                            //{
                            //    strDoctorPathInfo.Replace("[%Pathalogist4%]", string.Empty);
                            //    strDoctorPathInfo.Replace("[%Education4%]", string.Empty);
                            //}
                            ////-------------------------------------------------------------------------------------------

                            //if (PatientResultEntry.Pathologist2 != null)
                            //{
                            //    if (PatientResultEntry.Signature2 != null)
                            //    {
                            //        imageBytes = PatientResultEntry.Signature2;
                            //        imageBase64 = Convert.ToBase64String(imageBytes);
                            //        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                            //        if (PatientResultEntry.Pathologist == PatientResultEntry.PathoDoctorid2)  // && ReferDoctorSignature.IsChecked == true)
                            //        {
                            //            strDoctorPathInfo.Replace("[%Signature3%]", imageSrc);
                            //        }
                            //    }
                            //    strDoctorPathInfo.Replace("[%Pathalogist3%]", PatientResultEntry.Pathologist3 + " " + '(' + PatientResultEntry.Education3 + ')');
                            //    strDoctorPathInfo.Replace("[%Education3%]", PatientResultEntry.Roles);
                            //    //strDoctorPathInfo.Replace("[%Education3%]", PatientResultEntry.Education2);
                            //    ////if (!string.IsNullOrEmpty(PatientResultEntry.Education2))
                            //    ////{
                            //    ////    string[] parts = PatientResultEntry.Education1.Split(',');
                            //    ////    StringBuilder result = new StringBuilder();
                            //    ////    if (parts.Length == 3)
                            //    ////    {
                            //    ////        for (int i = 0; i < parts.Length; i++)
                            //    ////        {
                            //    ////            result.Append(parts[i]);
                            //    ////            if (i % 3 != 0)
                            //    ////            {
                            //    ////                result.Append("<br />");
                            //    ////            }
                            //    ////            else
                            //    ////            {
                            //    ////                result.Append(',');
                            //    ////            }
                            //    ////        }
                            //    ////        strDoctorPathInfo.Replace("[%Education4%]", result.ToString()); //PatientResultEntry.Education1);
                            //    ////    }
                            //    ////    else
                            //    ////    {
                            //    ////        strDoctorPathInfo.Replace("[%Education4%]", PatientResultEntry.Education2);
                            //    ////    }
                            //    ////}
                            //}
                            //else
                            //{
                            //    strDoctorPathInfo.Replace("[%Pathalogist3%]", string.Empty);
                            //    strDoctorPathInfo.Replace("[%Education3%]", string.Empty);
                            //}
                            ////-------------------------------------------------------------------------------------------

                            //if (PatientResultEntry.Pathologist3 != null)
                            //{
                            //    if (PatientResultEntry.Signature3 != null)
                            //    {
                            //        imageBytes = PatientResultEntry.Signature3;
                            //        imageBase64 = Convert.ToBase64String(imageBytes);
                            //        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                            //        if (PatientResultEntry.Pathologist == PatientResultEntry.PathoDoctorid3)  // && ReferDoctorSignature.IsChecked == true)
                            //        {
                            //            strDoctorPathInfo.Replace("[%Signature2%]", imageSrc);
                            //        }
                            //    }


                            //    strDoctorPathInfo.Replace("[%Pathalogist2%]", PatientResultEntry.Pathologist2 + " " + '(' + PatientResultEntry.Education2 + ')');
                            //    strDoctorPathInfo.Replace("[%Education2%]", PatientResultEntry.Roles);
                            //    //strDoctorPathInfo.Replace("[%Education2%]", PatientResultEntry.Education3);
                            //    ////if (!string.IsNullOrEmpty(PatientResultEntry.Education3))
                            //    ////{
                            //    ////    string[] parts = PatientResultEntry.Education1.Split(',');
                            //    ////    StringBuilder result = new StringBuilder();
                            //    ////    if (parts.Length == 3)
                            //    ////    {
                            //    ////        for (int i = 0; i < parts.Length; i++)
                            //    ////        {
                            //    ////            result.Append(parts[i]);
                            //    ////            if (i % 3 != 0)
                            //    ////            {
                            //    ////                result.Append("<br />");
                            //    ////            }
                            //    ////            else
                            //    ////            {
                            //    ////                result.Append(',');
                            //    ////            }
                            //    ////        }
                            //    ////        strDoctorPathInfo.Replace("[%Education4%]", result.ToString()); //PatientResultEntry.Education1);
                            //    ////    }
                            //    ////    else
                            //    ////    {
                            //    ////        strDoctorPathInfo.Replace("[%Education4%]", PatientResultEntry.Education3);
                            //    ////    }
                            //    ////}
                            //}
                            //else
                            //{
                            //    strDoctorPathInfo.Replace("[%Pathalogist2%]", string.Empty);
                            //    strDoctorPathInfo.Replace("[%Education2%]", string.Empty);
                            //}
                            ////-------------------------------------------------------------------------------------------

                            //if (PatientResultEntry.Pathologist4 != null)
                            //{
                            //    if (PatientResultEntry.Signature4 != null)
                            //    {
                            //        imageBytes = PatientResultEntry.Signature4;
                            //        imageBase64 = Convert.ToBase64String(imageBytes);
                            //        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                            //        if (PatientResultEntry.Pathologist == PatientResultEntry.PathoDoctorid4)  // && ReferDoctorSignature.IsChecked == true)
                            //        {
                            //            strDoctorPathInfo.Replace("[%Signature1%]", imageSrc);
                            //        }
                            //    }

                            //    strDoctorPathInfo.Replace("[%Pathalogist1%]", PatientResultEntry.Pathologist1);
                            //    strDoctorPathInfo.Replace("[%Pathalogist1%]", PatientResultEntry.Pathologist1 + " " + '(' + PatientResultEntry.Education1 + ')');
                            //    strDoctorPathInfo.Replace("[%Education1%]", PatientResultEntry.Roles);
                            //    //strDoctorPathInfo.Replace("[%Education1%]", PatientResultEntry.Education4);
                            //    //////if (!string.IsNullOrEmpty(PatientResultEntry.Education4))
                            //    //////{
                            //    //////    string[] parts = PatientResultEntry.Education1.Split(',');
                            //    //////    StringBuilder result = new StringBuilder();
                            //    //////    if (parts.Length == 3)
                            //    //////    {
                            //    //////        for (int i = 0; i < parts.Length; i++)
                            //    //////        {
                            //    //////            result.Append(parts[i]);
                            //    //////            if (i % 3 != 0)
                            //    //////            {
                            //    //////                result.Append("<br />");
                            //    //////            }
                            //    //////            else
                            //    //////            {
                            //    //////                result.Append(',');
                            //    //////            }
                            //    //////        }
                            //    //////        strDoctorPathInfo.Replace("[%Education4%]", result.ToString()); //PatientResultEntry.Education1);
                            //    //////    }
                            //    //////    else
                            //    //////    {
                            //    //////        strDoctorPathInfo.Replace("[%Education4%]", PatientResultEntry.Education4);
                            //    //////    }
                            //    //////}
                            //}
                            //else
                            //{
                            //    strDoctorPathInfo.Replace("[%Pathalogist1%]", string.Empty);
                            //    strDoctorPathInfo.Replace("[%Education1%]", string.Empty);
                            //}




                            richTextBox.Html = PatientResultEntry.Template;
                            //richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());
                            PrintReport1(ResultId, PatientResultEntry, strPatInfo.ToString(), strDoctorPathInfo.ToString());

                            //richTextBox.Html = PatientResultEntry.Template;
                            //richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", Convert.ToString(strPatInfo));
                            //richTextBox.Html = richTextBox.Html.Replace("[%DOCTORINFO%]", Convert.ToString(strDoctorPathInfo));                           
                            //PrintReport(ResultId, ISFinalize, strPatInfo.ToString(), strDoctorPathInfo.ToString());


                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();

        }
        public PaperKind PaperKind { get; set; }
        public Thickness PrintMargin { get; set; }




        private void PrintReport1(long PrintID, clsPathoResultEntryPrintDetailsVO PatientResultEntry, string strPatInfo, string strDoctorPathInfo)
        {
            try
            {
                richTextBox.Html = PatientResultEntry.Template;
                string rtbstring = string.Empty;
                string styleString = string.Empty;

                rtbstring = richTextBox.Html;

                if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                {
                    rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "[%PATIENTINFO%]");
                    richTextBox.Html = rtbstring;
                    TemplateContent = rtbstring;
                }

                if (!(richTextBox.Html.Contains("[%DOCTORINFO%]")))
                {
                    rtbstring = rtbstring.Insert(rtbstring.IndexOf("</body>"), "[%DOCTORINFO%]");
                    richTextBox.Html = rtbstring;
                    TemplateContent = rtbstring;
                }

                richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());
                richTextBox.Html = richTextBox.Html.Replace("[%DOCTORINFO%]", strDoctorPathInfo.ToString());

              //  richTextBox.Document.Margin = new Thickness(5, 5, 5, 5);
                richTextBox.Document.Margin = new Thickness(5, 83, 5, 5);
                //richTextBox.Document.Margin = new Thickness(5, 40, 5, 80);

                PrintMargin = new Thickness(5, 178, 5, 100);
                //PrintMargin = new Thickness(5, 190, 5, 50);

                //Printing
                var viewManager = new C1RichTextViewManager
                {
                    Document = richTextBox.Document,
                    PresenterInfo = richTextBox.ViewManager.PresenterInfo
                };
                var print = new PrintDocument();
                int presenter = 0;

                print.PrintPage += (s, printArgs) =>
                {

                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //  new MessageBoxControl.MessageBoxChildWindow("", "Print.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //msgW1.Show();

                    var element = (FrameworkElement)HeaderTemplate.LoadContent();
                    Grid grd = (Grid)element.FindName("PatientDetails");

                    if (grd != null)
                    {
                        grd.Visibility = Visibility.Visible;
                        grd.DataContext = PatientResultEntry;

                    }


                    element.DataContext = viewManager.Presenters[presenter];
                    printArgs.PageVisual = element;
                    printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;
                };

                var pdf = new C1PdfDocument(PaperKind.A4);
                
                PdfFilter.PrintDocument(richTextBox.Document, pdf, PrintMargin);
                
                var di = pdf.DocumentInfo;
                var font1 = new Font("Arial", 8, PdfFontStyle.Bold);
                
                var Rpfont = new Font("Verdana", 10, PdfFontStyle.Bold);
                var Rpfontdata = new Font("Verdana", 10, PdfFontStyle.Regular);

                var fmt = new StringFormat();
                fmt.Alignment = HorizontalAlignment.Left;
                fmt.LineAlignment = VerticalAlignment.Top;
                //fmt.LineAlignment = VerticalAlignment.Bottom;
                fmt.LineSpacing = -1.4; 

                Font titleFont = new Font("Tahoma", 20, PdfFontStyle.Bold);
                //Font headerFont = new Font("Arial", 10, PdfFontStyle.Bold);
                //Font bodyFont = new Font("Times New Roman", 4);


                var bkmk = new List<string[]>();

                Rect rcPage = PdfUtils.PageRectangle(pdf);
                Rect rc = PdfUtils.RenderParagraph(pdf, pdf.DocumentInfo.Title, titleFont, rcPage, rcPage, false);
                
                var ftr = new StringFormat();
                ftr.Alignment = HorizontalAlignment.Center;
                ftr.LineAlignment = VerticalAlignment.Bottom;             

                int PageCnt = pdf.Pages.Count;

                string name = string.Empty;
                string mrno = string.Empty;

                string age = string.Empty;
                string orderDate = string.Empty;

                string gender = string.Empty;
                string repoTime = string.Empty;

                string contno = string.Empty;
                string company = string.Empty;

                string patientCategory = string.Empty;
                string referenceNo = string.Empty;

                string patientSource = string.Empty;
                string sampleCollectionTime = string.Empty;

                string referredBy = string.Empty;
                string donarCode = string.Empty;

                if (PatientResultEntry.Salutation.ToString() != null)
                    name = PatientResultEntry.Salutation.ToString().Trim();
                else
                    name = "";

                if (PatientResultEntry.PatientName.ToString() != null)
                    name = name + PatientResultEntry.PatientName.ToString().Trim();
                else
                    name = "                                     ";

                if (PatientResultEntry.MRNo.ToString() != null)
                    mrno = PatientResultEntry.MRNo.ToString().Trim();
                else
                    mrno = "                                     ";

                if (PatientResultEntry.AgeYear.ToString() != null)
                    age = PatientResultEntry.AgeYear.ToString().Trim() + " Years " + PatientResultEntry.AgeMonth.ToString().Trim() + " Month(s) " + PatientResultEntry.AgeDate.ToString().Trim() + " Days";
                else
                    age = "                                     ";

                if (PatientResultEntry.ResultAddedDateTime.Value.ToString() != null)
                { //orderDate = PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy hh:mm tt").Trim();   String.Format("{0:MM/dd/yyyy}", 
                    orderDate = PatientResultEntry.ResultAddedDateTime.Value.ToString("dd/MM/yyyy").Trim();
                }
                else
                    orderDate = "                                     ";

                if (PatientResultEntry.Gender.ToString() != null)
                    gender = PatientResultEntry.Gender.ToString().Trim();
                else
                    gender = "                                     ";

                if (PatientResultEntry.ResultAddedDateTime.Value.ToString() != null)
                {
                    repoTime = PatientResultEntry.ResultAddedDateTime.Value.ToString("t").Trim();
                    //repoTime = PatientResultEntry.ResultAddedDateTime.Value.ToString("hh:mm").Trim();
                }
                else
                    repoTime = "                                     ";

                if (PatientResultEntry.ContactNo.ToString() != null)
                    contno = PatientResultEntry.ContactNo.ToString().Trim();
                else
                    contno = "                                     ";

                if (PatientResultEntry.Company.ToString() != null)
                    company = PatientResultEntry.Company.ToString().Trim();
                else
                    company = "                                     ";


                if (PatientResultEntry.PatientCategory.ToString() != null)
                    patientCategory = PatientResultEntry.PatientCategory.ToString().Trim();
                else
                    patientCategory = "                                     ";

                if (PatientResultEntry.ReferenceNo.ToString() != null)
                    referenceNo = PatientResultEntry.ReferenceNo.ToString().Trim();
                else
                    referenceNo = "                                     ";


                if (PatientResultEntry.PatientSource.ToString() != null)
                    patientSource = patientSource = PatientResultEntry.PatientSource.ToString().Trim();
                else
                    patientSource = "                                     ";

                if (PatientResultEntry.SampleCollectionTime.Value.ToString() != null)
                    sampleCollectionTime = ": " + PatientResultEntry.SampleCollectionTime.Value.ToString("dd MMM yyyy hh:mm tt").Trim();
                else
                    sampleCollectionTime = "                                     ";

                if (PatientResultEntry.ReferredBy.ToString() != null)
                    referredBy = PatientResultEntry.ReferredBy.ToString().Trim();
                else
                    referredBy = "                                     ";

                if (PatientResultEntry.DonarCode.ToString() != null)
                    donarCode = PatientResultEntry.DonarCode.ToString().Trim();
                else
                    donarCode = "                                     ";


                string pathocategory = string.Empty;
                string testname = string.Empty;

                string sampleNo = string.Empty;

                if (PatientResultEntry.SampleNo.ToString() != null)
                    sampleNo =": "+PatientResultEntry.SampleNo.ToString().Trim();
                else
                    sampleNo = "                                     ";

                

                if (PatientResultEntry.PathoCategory.ToString() != null)
                    pathocategory = PatientResultEntry.PathoCategory.ToString().Trim();

                if (PatientResultEntry.Test.ToString() != null)
                    testname = PatientResultEntry.Test.ToString().Trim();

                    for (int page = 0; page < pdf.Pages.Count; page++)
                    {
                        
                        //byte[] imageBytes = null;
                        //imageBytes = PatientResultEntry.UnitLogo;
                        
                       // Stream imgstream = new MemoryStream(imageBytes);
                       // BitmapImage bi = new BitmapImage();
                       // bi.SetSource(imgstream);
                       // var wb = new WriteableBitmap(bi);                        
                       //// pdf.DrawImage(wb, PdfUtils.Inflate(pdf.PageRectangle, 0, 10), ContentAlignment.TopCenter, Stretch.Uniform);//Commented By YK 080317
                       // pdf.DrawImage(wb, PdfUtils.Inflate(pdf.PageRectangle, 0, 10), ContentAlignment.TopLeft, Stretch.None);//Added By YK
                        

                        //Pen pen1 = new Pen(Colors.Gray, 2.0f);
                        //pdf.DrawRectangle(pen1, Hrect2);

                        #region commentCode                        
                            //string h1 = "Name                 "+"    :"+name.ToString()+"                                             "+"MRNO                "+"    :"+mrno.ToString()+"                        "+"\n";
                            //string h2 = "Age                  "+"    :"+age.ToString()+"                                              "+"OrderDate           "+"    :"+orderDate.ToString()+"                   "+"\n";
                            //string h3 = "Gender               "+"    :"+gender.ToString()+"                                           "+"Reported Time       "+"    :"+orderDate.ToString()+"                   "+"\n";
                            //string h4 = "Patient Contact No"+"    :"+contno.ToString()+"                                           "+"Company Name        "+"    :"+company.ToString()+"                     "+"\n";
                            //string h5 = "Patient Category 1"+"    :"+patientCategory.ToString()+"                                  "+"Reference No        "+"    :"+referenceNo.ToString() + "               "+"\n";
                            //string h6 = "Patient Category 2"+"    :"+patientSource.ToString()+"                                    "+"Collection Time     "+"    :"+sampleCollectionTime.ToString()+"        "+"\n";
                            //string h7 = "Referred By       "+"    :"+referredBy.ToString()+"                                       "+"D / S Code          "+"    :"+donarCode.ToString()+"                   "+"\n";

                       
                            //string h1 = "Name                          :"+name.ToString() +"\n";                                           
                            //string h2 = "Age                             :"+age.ToString() +"\n";                                            
                            //string h3 = "Gender                       :"+gender.ToString() +"\n";                                         
                            //string h4 = "Patient Contact No    :"+contno.ToString() +"\n";                                          
                            //string h5 = "Patient Category 1    :"+patientCategory.ToString()  +"\n";
                            //string h6 = "Patient Category 2    :"+patientSource.ToString() +"\n";                                   
                            //string h7 = "Referred By               :"+referredBy.ToString() +"\n";                                       

                            //string h8 ="MRNO                      :"+mrno.ToString()+"\n";
                            //string h9 ="Date                       :"+orderDate.ToString()+"\n";
                            //string h10 ="Reported Time        :"+orderDate.ToString()+"\n";
                            //string h11 ="Company Name      :"+company.ToString()+"\n";
                            //string h12 ="Reference No          :"+referenceNo.ToString()+"\n";
                            //string h13 ="Collection Time      :"+sampleCollectionTime.ToString()+"\n";
                            //string h14 ="D / S Code               :" + donarCode.ToString()+ "\n";
                        #endregion
                        
                        string h1 = "Name                  "+"\n";           
                        string h2 = "Age                   "+"\n";     
                        string h3 = "Gender                "+"\n";
                        string h4 = "Patient Contact No    "+"\n";
                        //string h5 = "Patient Category 1    "+"\n";
                        //string h6 = "Patient Category 2    "+"\n";
                        string h7 = "Referred By           "+"\n";

                        string d1 =": "+name.ToString() + "\n";
                        string d2 =": "+age.ToString() + "\n";
                        string d3 =": "+gender.ToString() + "\n";
                        string d4 =": "+contno.ToString() + "\n";
                        //string d5 =": "+patientCategory.ToString() + "\n";
                        //string d6 =": "+patientSource.ToString() + "\n";
                        string d7 =": "+referredBy.ToString() + "\n";

                        string h8 = "MR No                  "+"\n";        
                        string h9 = "Date                   "+"\n";        
                        string h10 = "Reported Time         "+"\n";   
                        //string h11 = "Company Name          "+"\n";   
                        string h12 = "Reference No          "+"\n";
                        //string h13 = "D / S Code            " + "\n"; 
                        //string h13 = "Collection Time       "+"\n";   
                        //string h14 = "D / S Code            "+"\n";      

                        string d8 =": " + mrno.ToString() + "\n";
                        string d9 =": " + orderDate.ToString() + "\n";
                        string d10 =": " + repoTime.ToString() + "\n";
                        //string d11 =": "+company.ToString() + "\n";
                        string d12 =": " + referenceNo.ToString() + "\n";
                        //string d13 = ": " + donarCode.ToString() + "\n";
                        //string d13 =": "+ sampleCollectionTime.ToString() + "\n";
                        //string d14 =": "+ donarCode.ToString() + "\n";

                        string header = string.Empty;
                        string headerdata = string.Empty;
                        string header1 = string.Empty;
                        string headerdata1 = string.Empty;

                        string f1 = PatientResultEntry.AdressLine1.Trim().ToString() + "\t" + "\t" + "\n"; ;
                        string f2 = PatientResultEntry.Email.Trim().ToString() + "\t" + "\t" + PatientResultEntry.UnitContactNo.Trim().ToString() + "\n";

                        string Footer = string.Empty;

                        Footer = string.Format("{0}", f1 + f2);
                        var f = string.Format("{0}", Footer);

                        //Pen pen = new Pen(Colors.Red);
                        Pen pen = new Pen(Colors.Gray, 2.0f);
                        int i = pdf.CurrentPage;

                        if (pdf.CurrentPage == 0)
                        {
                            //header = string.Format("{0}", h1 + h2 + h3 + h4 + h5 + h6 + h7);
                            header = string.Format("{0}", h1 + h2 + h3 + h4  + h7);
                            header = string.Format("{0}", h1 + h2 + h3 + h4  + h7);
                            var a = string.Format("{0}", header);

                            headerdata = string.Format("{0}", d1 + d2 + d3 + d4  + d7);
                            var adata = string.Format("{0}", headerdata);

                            //header1 = string.Format("{0}", h8 + h9 + h10 + h11 + h12 + h13 + h14);
                            header1 = string.Format("{0}", h8 + h9 + h10  + h12  );
                            var a1 = string.Format("{0}", header1);

                            //headerdata1 = string.Format("{0}", d8 + d9 + d10 + d11 + d12 + d13 + d14);
                            headerdata1 = string.Format("{0}", d8 + d9 + d10  + d12 );
                            var adata1 = string.Format("{0}", headerdata1);

                            int X1 = 10;                           
                           // int Y1 = 70;//old
                            int Y1 = 145;
                            int X2 = 570;
                            //  int Y2 = 70;//old
                            int Y2 = 145;
                            pdf.DrawLine(pen, X1, Y1, X2, Y2);

                            int x = 10;
                            //int y = 50;
                            // int y = 72;//old
                            int y = 147;
                            int width = 350;
                            int height = 80;
                            Rect Hrect1 = new Rect(x, y, width, height);
                            pdf.DrawString(a, Rpfont, Colors.Black, Hrect1, fmt);
                            //pdf.DrawRectangle(pen, Hrect1);

                            //Rect Hrect1data = new Rect(110, 50, 450, 130);
                           // Rect Hrect1data = new Rect(110, 72, 350, 80);//Old code comment by yk 72 to 102
                            Rect Hrect1data = new Rect(110, 147, 350, 80);
                            pdf.DrawString(adata, Rpfontdata, Colors.Black, Hrect1data, fmt);

                            //Rect Hrect2 = new Rect(310, 50, 450, 130);
                            // Rect Hrect2 = new Rect(310, 72, 350, 80);//Old code comment by yk 72 to 102
                            Rect Hrect2 = new Rect(310, 147, 350, 80);
                            pdf.DrawString(a1, Rpfont, Colors.Black, Hrect2, fmt);

                            //Rect Hrect2data = new Rect(400, 72, 350, 80);
                            Rect Hrect2data = new Rect(400, 147, 350, 80);//Old code comment by yk 72 to 102
                            pdf.DrawString(adata1, Rpfontdata, Colors.Black, Hrect2data, fmt);

                            //pdf.DrawRectangle(pen, Hrect2);

                            int XX1 = 10;
                          //  int YY1 = 150;
                            int YY1 = 222;
                            int XX2 = 570;
                            //int YY2 = 150;
                            int YY2 = 222;

                            pdf.DrawLine(pen, XX1, YY1, XX2, YY2);

                            var apatho = string.Format("{0}", pathocategory);

                            var R1f = new Font("Verdana", 12, PdfFontStyle.Bold);
                          //  Rect R1 = new Rect(10, 158, 350, 100);
                            Rect R1 = new Rect(10, 222, 350, 100);
                            pdf.DrawString(apatho, R1f, Colors.Black, R1, fmt);                            

                            var atestname = string.Format("{0}", testname);
                            var R2f = new Font("Verdana", 11, PdfFontStyle.Bold);
                            //Rect R2 = new Rect(10, 172, 350, 100);
                            Rect R2 = new Rect(10, 237, 350, 100);
                            //pdf.DrawString(atestname, R2f, Colors.Purple, R2, fmt);   
                            pdf.DrawString(atestname, R2f, Color.FromArgb(0xff, 97, 48, 144), R2, fmt);


                            //// For Sample No And Collection Time

                            ////string sa1 = "Sample No                  " + "\n";
                            ////var asampleNo = string.Format("{0}", sa1);

                            ////var R3f = new Font("Verdana", 9, PdfFontStyle.Bold);
                            ////Rect R3 = new Rect(400, 160, 350, 100);
                            ////pdf.DrawString(asampleNo, R3f, Colors.Black, R3, fmt);

                            ////var adatasampleNo = string.Format("{0}", sampleNo);
                            ////var R4f = new Font("Verdana", 9, PdfFontStyle.Bold);
                            ////Rect R4 = new Rect(475, 160, 350, 100);
                            ////pdf.DrawString(adatasampleNo, R4f, Colors.Black, R4, fmt);


                            ////string col1 = "Collection Time      " + "\n";
                            ////var acol1 = string.Format("{0}", col1);

                            ////var R5f = new Font("Verdana", 9, PdfFontStyle.Bold);
                            ////Rect R5 = new Rect(400, 170, 350, 100);
                            ////pdf.DrawString(acol1, R5f, Colors.Black, R5, fmt);

                            ////var adataColTime = string.Format("{0}", sampleCollectionTime.ToString());
                            ////var R6f = new Font("Arial", 10, PdfFontStyle.Regular);
                            ////Rect R6 = new Rect(475, 170, 350, 100);
                            ////pdf.DrawString(adataColTime, R6f, Colors.Black, R6, fmt);

                            //// For Sample No And Collection Time                            
                            //Footer
                            //Rect Frect1 = new Rect(10, 750, 500, 50);
                            //pdf.DrawString(f, font, Colors.Black, Frect1, ftr);                            
                        }
                        else
                        {                            
                            header = string.Format("{0}",h1 + h2 + h3);
                            var a = string.Format("{0}", header);

                            headerdata = string.Format("{0}", d1 + d2 + d3);
                            var adata = string.Format("{0}", headerdata);

                            header1 = string.Format("{0}", h8 + h9 + h10);
                            var a1 = string.Format("{0}", header1);

                            headerdata1 = string.Format("{0}", d8 + d9 + d10);
                            var adata1 = string.Format("{0}", headerdata1);
                                                       
                            int X1 = 10;
                            //int Y1 = 70;//old code commented 70 to 102
                            int Y1 = 145;
                            int X2 = 570;
                            //  int Y2 = 70;//old code commented 70 to 102
                            int Y2 = 145;
                            pdf.DrawLine(pen, X1, Y1, X2, Y2);

                            int x = 10;
                            //int y = 55;
                           // int y = 72;//old code commented 70 to 102
                            int y = 147;
                            int width = 350;
                            int height = 80;
                            Rect Hrect1 = new Rect(x, y, width, height);
                            pdf.DrawString(a, Rpfont, Colors.Black, Hrect1, fmt);
                            //pdf.DrawRectangle(pen, Hrect1);

                            //Rect Hrect1data = new Rect(110, 55, 450, 80);
                            //Rect Hrect1data = new Rect(110, 72, 350, 80);//old commented yk 72 to 102
                            Rect Hrect1data = new Rect(110, 147, 350, 80);
                            pdf.DrawString(adata, Rpfontdata, Colors.Black, Hrect1data, fmt);


                            //Rect Hrect2 = new Rect(310, 55, 450, 80);
                            //Rect Hrect2 = new Rect(310, 72, 350, 80);//old commented yk 72 to 102
                            Rect Hrect2 = new Rect(310, 147, 350, 80);
                            pdf.DrawString(a1, Rpfont, Colors.Black, Hrect2, fmt);

                            //Rect Hrect2data = new Rect(400, 55, 450, 80);
                          //  Rect Hrect2data = new Rect(400, 72, 350, 80);
                            Rect Hrect2data = new Rect(400, 147, 350, 80);//old commented yk 72 to 102
                            pdf.DrawString(adata1, Rpfontdata, Colors.Black, Hrect2data, fmt);

                            //pdf.DrawRectangle(pen, Hrect2);


                            int XX1 = 10;
                            int YY1 = 188; //Second page second Draw line value
                            int XX2 = 570;
                            int YY2 = 188;  //Second page second Draw line value
                            pdf.DrawLine(pen, XX1, YY1, XX2, YY2);

                            //var apatho = string.Format("{0}", pathocategory);
                            //var R1f = new Font("Verdana", 12, PdfFontStyle.Bold);
                            //Rect R1 = new Rect(10, 70, 450, 130);
                            //pdf.DrawString(apatho, R1f, Colors.Black, R1, fmt);

                            //var atestname = string.Format("{0}", testname);
                            //var R2f = new Font("Verdana", 11, PdfFontStyle.Bold);
                            //Rect R2 = new Rect(10, 80, 450, 130);
                            //pdf.DrawString(atestname, R2f, Colors.Black, R2, fmt);   
                            
                            //Footer
                            //Rect Frect1 = new Rect(10, 750, 500, 50);
                            //pdf.DrawString(f, font, Colors.Black, Frect1, ftr);                            
                        }                      

                        var pn = new StringFormat();
                        pn.Alignment = HorizontalAlignment.Right;
                        pn.LineAlignment = VerticalAlignment.Bottom;

                        pdf.CurrentPage = page;
                        var text = string.Format("Page {1} of {2}", di.Title, page + 1, pdf.Pages.Count);
                        pdf.DrawString(text, font1, Colors.Black, PdfUtils.Inflate(pdf.PageRectangle, -72, -36), pn);


                        if (page == pdf.Pages.Count - 1) //print to last page only
                        {
                            int D11 = 10;
                          //  int D12 = 700;
                            int D12 = 600;  //Value change from 695 to 600
                            int D13 = 64;
                            int D14 = 80;

                            var R11f = new Font("Verdana", 10, PdfFontStyle.Regular);
                            Rect Hrect31 = new Rect(D11, D12, D13, D14);
                            pdf.DrawString("disclaimer :", R11f, Colors.Black, Hrect31, fmt);
                            //pdf.DrawRectangle(pen, Hrect31);

                            int D1 = 67;
                          //  int D2 = 700;
                            int D2 = 600;//Value change from 695 to 600
                            int D3 = 530;
                            int D4 = 80;

                            //var R11f = new Font("Verdana", 10, PdfFontStyle.Regular);
                            Rect Hrect21 = new Rect(D1, D2, D3, D4);
                            pdf.DrawString(PatientResultEntry.Disclaimer.ToString(), R11f, Colors.Black, Hrect21, fmt);
                            //pdf.DrawRectangle(pen, Hrect21);


                            int L1 = 10;
                            int L2 = 700; //Value Change 770 to 700
                            int L3 = 100;
                            int L4 = 40;

                            Rect Hrect22 = new Rect(L1, L2, L3, L4);
                            string LL1 = "-------------------------" + "\n";
                            string LL2 = "Lab Technician" + "\n";
                            pdf.DrawString(LL1 + LL2, Rpfont, Colors.Black, Hrect22, fmt);
                            //pdf.DrawRectangle(pen, Hrect22);

                            int A1 = 420;
                            int A2 = 700;//Value Change 770 to 700
                            int A3 = 150;
                            int A4 = 40;

                            Rect Hrect23 = new Rect(A1, A2, A3, A4);
                            string AA1 = "-------------------------------" + "\n";
                            string AA2 = "Authorised Signatory " + "\n";
                            pdf.DrawString(AA1 + AA2, Rpfont, Colors.Black, Hrect23, fmt);
                            //pdf.DrawRectangle(pen, Hrect23);
                        }                       
                    }

                pdf.CurrentPage = pdf.Pages.Count - 1;
                String appPath;

                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                appPath = "PathoReport" + UnitID + "_" + PrintID + ".pdf";

                Stream FileStream = new MemoryStream();
                MemoryStream MemStrem = new MemoryStream();

                pdf.Save(MemStrem);
                FileStream.CopyTo(MemStrem);

                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                client.UploadReportFileCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        // WaitNew.Close();
                        //ViewPDFReport(appPath);
                    }
                };
                client.UploadReportFileAsync(appPath, MemStrem.ToArray());
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                // WaitNew.Close();
            }
        }

        //rc = PdfUtils.RenderParagraph(pdf, header, headerFont, rcPage, rc, true, true);
        //string header = string.Format("{0}", h1+h2+h3+h4+h5+h6);
        //var a = string.Format("{0}", header);
        //string header = string.Format("{0}", page + 1, BuildRandomTitle(PatientResultEntry));                   
        // rc = PdfUtils.RenderParagraph(pdf, header, headerFont, PdfUtils.Inflate(pdf.PageRectangle, 20, 20), rc, true, true);
        //int x = 10;
        //int y = -700;
        //pdf.DrawString(a, font, Colors.Black, PdfUtils.Offset(pdf.PageRectangle, x, y), fmt);
        //rc.X = 10;
        //rc.Y = 180;
        //rc = PdfUtils.RenderParagraph(pdf, header, headerFont, rcPage, rc, true, true);

        //rc = PdfUtils.RenderParagraph(pdf, header, headerFont, PdfUtils.Inflate(pdf.PageRectangle, 140, 140), rc, true, true);
        // pdf.DrawRectangle(pen, rc);
        // pdf.DrawString(header, font, Colors.Black, PdfUtils.Inflate(pdf.PageRectangle, 40, 40), fmt);

        static string BuildRandomTitle(clsPathoResultEntryPrintDetailsVO PatientResultEntry)
        {
            string a1 = "Name" + "    :" + PatientResultEntry.PatientName.ToString() + "MRNO" + PatientResultEntry.MRNo.ToString();
            //string[] a2 = "Music|Tennis|Golf|Zen|Diving|Modern Art|Gardening|Architecture|Mathematics|Investments|.NET|Java".Split('|');
            //string[] a3 = "Quickly|Painlessly|The Hard Way|Slowly|Painfully|With Panache".Split('|');
            //return string.Format("{0} {1} {2}", a1[_rnd.Next(a1.Length - 1)], a2[_rnd.Next(a2.Length - 1)], a3[_rnd.Next(a3.Length - 1)]);

            return string.Format("{0}", a1);
        }


        //static string BuildRandomTitle()
        //{
        //    string[] a1 = "Learning|Explaining|Mastering|Forgetting|Examining|Understanding|Applying|Using|Destroying".Split('|');
        //    string[] a2 = "Music|Tennis|Golf|Zen|Diving|Modern Art|Gardening|Architecture|Mathematics|Investments|.NET|Java".Split('|');
        //    string[] a3 = "Quickly|Painlessly|The Hard Way|Slowly|Painfully|With Panache".Split('|');
        //    return string.Format("{0} {1} {2}", a1[_rnd.Next(a1.Length - 1)], a2[_rnd.Next(a2.Length - 1)], a3[_rnd.Next(a3.Length - 1)]);
        //}

        static Random _rnd = new Random();

        private void PrintReport(long ResultID, bool IsFinalize, clsPathoResultEntryPrintDetailsVO PatientResultEntry, string strPatInfo, string DoctorInfoString)
        {

            richTextBox.Html = null;

            richTextBox.Html = PatientResultEntry.Template;
            richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", Convert.ToString(strPatInfo));
            richTextBox.Html = richTextBox.Html.Replace("[%DOCTORINFO%]", Convert.ToString(DoctorInfoString));
            //richTextBox.Html = richTextBox.Html.Remove(0);
            //Printing
            var viewManager = new C1RichTextViewManager
            {
                Document = richTextBox.Document,

                PresenterInfo = richTextBox.ViewManager.PresenterInfo
            };
            var print = new PrintDocument();
            int presenter = 0;

            int count = 0;
            long printpagecount = 0;
            long Count = 0;
            print.PrintPage += (s, printArgs) =>
            {
                var element = (FrameworkElement)HeaderTemplate.LoadContent();

                ////if (count == viewManager.Presenters.Count)
                ////{
                ////    Grid grd1 = new Grid();
                ////    grd1 = (Grid)element.FindName("PatientDetails");

                ////    if (grd1 != null)
                ////    {
                ////        grd1.Visibility = Visibility.Visible;
                ////        grd1.DataContext = PatientResultEntry;

                ////    }
                ////}
                ////else
                ////{
                ////    if (viewManager.Presenters.Count == 1)
                ////    {
                ////        Grid grd1 = new Grid();
                ////        grd1 = (Grid)element.FindName("PatientDetails");

                ////        if (grd1 != null)
                ////        {
                ////            grd1.Visibility = Visibility.Visible;
                ////            grd1.DataContext = PatientResultEntry;

                ////        }
                ////    }
                ////    else
                ////    {
                ////        Grid grd1 = new Grid();
                ////        grd1 = (Grid)element.FindName("PatientDetails");
                ////        grd1.Visibility = Visibility.Collapsed;
                ////    }
                ////}
                element.DataContext = viewManager.Presenters[presenter];
                printArgs.PageVisual = element;
                printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;
                count = presenter;
                count++;
            };

            long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            string URL = "../Reports/Pathology/ResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&IsFinalized=" + IsFinalize;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            //print.Print("A Christmas Carol");

            //var pdf = new C1PdfDocument(PaperKind.A4);

            //PdfFilter.PrintDocument(richTextBox.Document, pdf, PrintMargin);
            //pdf.CurrentPage = pdf.Pages.Count - 1;
            //String appPath;

            //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            //appPath = "DischargeCard" + UnitID + "_" + ResultID + ".pdf";

            //Stream FileStream = new MemoryStream();
            //MemoryStream MemStrem = new MemoryStream();

            //pdf.Save(MemStrem);
            //FileStream.CopyTo(MemStrem);

            //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

            //client.UploadReportFileForDischargeCompleted += (s, args) =>
            //{
            //    if (args.Error == null)
            //    {
            //        //WaitNew.Close();
            //        ViewPDFReport(appPath);
            //    }
            //};
            //client.UploadReportFileForDischargeAsync(appPath, MemStrem.ToArray());
            //client.CloseAsync();

            //print.BeginPrint += new EventHandler<BeginPrintEventArgs>(print_BeginPrint);
            //print.PrintPage += new EventHandler<PrintPageEventArgs>(print_PrintPage);
            //print.Print("A Christmas Carol");

            //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //string URL = "../Reports/Pathology/ResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&IsFinalized=" + IsFinalize;
            //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

            //print.Print("A Christmas Carol");

            //var pdf = new C1PdfDocument(PaperKind);

            //PdfFilter.PrintDocument(richTextBox.Document, pdf, PrintMargin);

            //String appPath;

            //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            //appPath = UnitID + "_" + ResultID + ".pdf";

            //Stream FileStream = new MemoryStream();

            //MemoryStream MemStrem = new MemoryStream();

            ////pdf.Save(appPath);
            //pdf.Save(MemStrem);


            //FileStream.CopyTo(MemStrem);

            //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

            //client.UploadReportFileForRadiologyCompleted += (s, args) =>
            //{
            //    if (args.Error == null)
            //    {
            //        //HtmlPage.Window.Invoke("OpenReport", new string[] { ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL });
            //        //listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
            //    }
            //};
            //client.UploadReportFileForRadiologyAsync(appPath, MemStrem.ToArray());
            //client.CloseAsync();

        }




        private void ViewPDFReport(string FileName)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../PatientPathTestReportDocuments");
            string fileName1 = address.ToString();
            fileName1 = fileName1 + "/" + FileName;
            // HtmlPage.Window.Invoke("Open", fileName1);
            HtmlPage.Window.Invoke("open", new object[] { fileName1, "", "" });
        }

        void print_BeginPrint(object sender, BeginPrintEventArgs e)
        {
            //PrintDocument pd = ((PrintDocument)sender);
            //pd.
        }

        void print_PrintPage(object sender, PrintPageEventArgs e)
        {
            //Thickness margin = new Thickness
            //{
            //    Left = Math.Max(0, 96 - e.PageMargins.Left),
            //    Top = Math.Max(0, 96 - e.PageMargins.Top),
            //    Right = Math.Max(0, 96 - e.PageMargins.Right),
            //    Bottom = Math.Max(0, 96 - e.PageMargins.Bottom)
            //};
            //Ellipse ellipse = new Ellipse
            //{
            //    Fill = new SolidColorBrush(Color.FromArgb(255, 255, 192, 192)),
            //    Stroke = new SolidColorBrush(Color.FromArgb(255, 192, 192, 255)),
            //    StrokeThickness = 24,   // 1/4 inch 
            //    Margin = margin
            //};
            //Border border = new Border();
            //border.Child = ellipse;
            //e.PageVisual = border; 

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //PrintReport(ResultId, ISFinalize, PatientResultEntry, strPatInfo.ToString(), strDoctorPathInfo.ToString());


            string sPath = "PathoReport" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId + "_" + ResultId + ".pdf";
            ViewPDFReport(sPath);

            //string sPath = "DischargeCard" + ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).UnitID + "_" + ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).ID + ".pdf";
            //ViewPDFReport(sPath);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var oldItem = e.RemovedItems.OfType<C1TabItem>().FirstOrDefault();
                if (oldItem == null) return; // richTextBoxTab and the others are null the first time around because InitializeComponent is running.

                if (oldItem == richTextBoxTab)
                {
                    htmlBox.Text = richTextBox.Html;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                }
                else if (oldItem == htmlTab)
                {
                    richTextBox.Html = htmlBox.Text;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                }
                else if (oldItem == rtfTab)
                {
                    richTextBox.Document = new RtfFilter().ConvertToDocument(rtfBox.Text);
                    htmlBox.Text = richTextBox.Html;
                }
            }
            catch { }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SavePDF_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var print = new PrintDocument();
            //    var pdf = new C1PdfDocument(PaperKind.A4);
            //    PrintMargin = new Thickness(0, 152, 0, 0);
            //    PdfFilter.PrintDocument(richTextBox.Document, pdf, PrintMargin);
            //    String appPath;
            //    long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //    appPath = "Milan_" + OrderDetails.FirstName + "_" + OrderDetails.LastName + "_" + UnitID + "_" + ObjDetails.PathPatientReportID + ".pdf";

            //    Uri address2 = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            //    DataTemplateServiceClient client1 = new DataTemplateServiceClient("CustomBinding_DataTemplateService", address2.AbsoluteUri);
            //    client1.DeletePathReportFileCompleted += (s1, args1) =>
            //    {
            //        if (args1.Error == null)
            //        {
            //            Stream FileStream = new MemoryStream();
            //            MemoryStream MemStrem = new MemoryStream();
            //            pdf.Save(MemStrem);
            //            FileStream.CopyTo(MemStrem);
            //            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            //            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            //            client.UploadReportFileForPathologyCompleted += (p, args) =>
            //            {
            //                if (args.Error == null)
            //                {
            //                    Stream FileStream1 = new MemoryStream();
            //                    MemoryStream MemStrem1 = new MemoryStream();
            //                    FileStream = FileStream1;
            //                    MemStrem = MemStrem1;
            //                    Uri address3 = new Uri(Application.Current.Host.Source, "../PatientPathTestReportDocuments");
            //                    string fileName1 = address3.ToString();
            //                    fileName1 = fileName1 + "/" + appPath;
            //                    //HtmlPage.Window.Invoke("open", new object[] { fileName1, "", "" });
            //                    richTextBox.Html = null;
            //                    richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html + "[%DOCTORINFO%]";
            //                    richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", "");
            //                    richTextBox.Html = richTextBox.Html.Replace("[%DOCTORINFO%]", "");
            //                    richTextBox.Html = strDoctorPathInfo.ToString();//PreviewPatientResultEntry.FirstLevelDescription;
            //                    this.Close();

            //                    //frmSendEmail w = new frmSendEmail();
            //                    //w.SelectedTestList = SelectedTestList;
            //                    //w.IsTemplateBasedTest = true;
            //                    //w.UnitID = SelectedTestList[0].UnitId;
            //                    //w.ResultID = SelectedTestList[0].ID;
            //                    //w.SelectedDetails = OrderDetails;
            //                    //w.IsPDFSave = true; // By BHUSHAN
            //                    //w.appPath = appPath; // By BHUSHAN
            //                    //w.Show();
            //                }
            //            };
            //            client.UploadReportFileForPathologyAsync(appPath, MemStrem.ToArray());
            //            client.CloseAsync();
            //        }
            //    };
            //    client1.DeletePathReportFileAsync(appPath);
            //    client1.CloseAsync();
            //}
            //catch
            //{

            //}
        }
    }
}


public static class PdfUtils
{

    public static Rect Inflate(this Rect rc, double dx, double dy)
    {
        rc.X -= dx;
        rc.Y -= dy;
        rc.Width += 2 * dx;
        rc.Height += 2 * dy;
        return rc;
    }

    public static Rect Offset(this Rect rc, double dx, double dy)
    {
        rc.X += dx;
        rc.Y += dy;
        return rc;
    }

    public static Stream GetStream(string resName)
    {
        var asm = typeof(PdfUtils).Assembly;
        resName = string.Format("{0}.Resources.{1}", asm.FullName.Split(',')[0].Trim(), resName);
        return asm.GetManifestResourceStream(resName);
    }

    public static Rect PageRectangle(this C1PdfDocument pdf)
    {
        return PageRectangle(pdf, new Thickness(72));
    }

    public static Rect PageRectangle(this C1PdfDocument pdf, Thickness pageMargins)
    {
        Rect rc = pdf.PageRectangle;
        double left = Math.Min(rc.Width, rc.Left + pageMargins.Left);
        double top = Math.Min(rc.Height, rc.Top + pageMargins.Top);
        double width = Math.Max(0, rc.Width - (pageMargins.Left + pageMargins.Right));
        double height = Math.Max(0, rc.Height - (pageMargins.Top + pageMargins.Bottom));
        return new Rect(left, top, width, height);
    }

    public static Rect RenderParagraph(this C1PdfDocument doc, string text, Font font, Rect rcPage, Rect rc, bool outline)
    {
        return RenderParagraph(doc, text, font, rcPage, rc, outline, false);
    }

    public static Rect RenderParagraph(this C1PdfDocument doc, string text, Font font, Rect rcPage, Rect rc)
    {
        return RenderParagraph(doc, text, font, rcPage, rc, false, false);
    }

    public static Rect RenderParagraph(this C1PdfDocument pdf, string text, Font font, Rect rcPage, Rect rc, bool outline, bool linkTarget)
    {
        // if it won't fit this page, do a page break
        rc.Height = pdf.MeasureString(text, font, rc.Width).Height;
        if (rc.Bottom > rcPage.Bottom)
        {
            pdf.NewPage();
            rc.Y = rcPage.Top;
        }

        // draw the string
        pdf.DrawString(text, font, Colors.Black, rc);

        // show bounds (to check word wrapping)
        //var p = Pen.GetPen(Colors.Orange);
        //pdf.DrawRectangle(p, rc);

        // add headings to outline
        if (outline)
        {
            pdf.DrawLine(Colors.Black, rc.X, rc.Y, rc.Right, rc.Y);
            pdf.AddBookmark(text, 0, rc.Y);
        }

        // add link target
        if (linkTarget)
        {
            pdf.AddTarget(text, rc);
        }

        // update rectangle for next time
        rc = Offset(rc, 0, rc.Height);
        return rc;
    }
}