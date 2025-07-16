using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Radiology;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Text;
using C1.Silverlight.RichTextBox;
using System.Windows.Printing;
using C1.Silverlight;
using C1.Silverlight.RichTextBox.Documents;
using C1.Silverlight.Pdf;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using C1.Silverlight.RichTextBox.PdfFilter;
using System.Windows.Browser;

namespace PalashDynamics.Radiology
{
    public partial class PrintRportDelivery : ChildWindow
    {

        public long ResultId { get; set; }
        public bool ISFinalize { get; set; }
        public long UnitID { get; set; }
        public string TestName { get; set; }
        public long Opd_Ipd_External { get; set; }
        public bool IsFromReportDelivery { get; set; }
        public clsRadOrderBookingVO SelectedOrder { get; set; }
        public clsRadOrderBookingDetailsVO ObjDetails { get; set; }

        StringBuilder strPatInfo;
        StringBuilder strDoctorPathInfo;
        public String TemplateContent = String.Empty;

        clsRadResultEntryPrintDetailsVO PatientResultEntry = new clsRadResultEntryPrintDetailsVO();

        public PrintRportDelivery()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PrintRportDelivery_Loaded);
        }

        void PrintRportDelivery_Loaded(object sender, RoutedEventArgs e)
        {
            GetPatientDetailsInHtml(ResultId, ISFinalize);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void GetPatientDetailsInHtml(long ResultId, bool IsFinalize)
        {
            clsRadResultEntryPrintDetailsBizActionVO BizAction = new clsRadResultEntryPrintDetailsBizActionVO();

            BizAction.ResultID = ResultId;
            BizAction.UnitID = UnitID;
            BizAction.OPDIPD = Opd_Ipd_External;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        //clsRadResultEntryPrintDetailsVO PatientResultEntry = new clsRadResultEntryPrintDetailsVO();
                        //PatientResultEntry = ((clsRadResultEntryPrintDetailsBizActionVO)arg.Result).ResultDetails;

                        //strPatInfo = new StringBuilder();



                        PatientResultEntry = new clsRadResultEntryPrintDetailsVO();
                        PatientResultEntry = ((clsRadResultEntryPrintDetailsBizActionVO)arg.Result).ResultDetails;


                        strPatInfo = new StringBuilder();

                        ///////////Commented Code

                        //strPatInfo.Append(PatientResultEntry.PatientInfoHTML);

                        //strPatInfo = strPatInfo.Replace("[MRNO]", "    :" + PatientResultEntry.MRNo.ToString());
                        //strPatInfo = strPatInfo.Replace("[OrderDate]", "    :" + PatientResultEntry.OrderDate.Value.ToString("dd MMM yyyy"));
                        ////strPatInfo = strPatInfo.Replace("[Salutation]", "    :" + PatientResultEntry.Salutation.ToString());
                        //strPatInfo = strPatInfo.Replace("[PatName]", "    :" + PatientResultEntry.PatientName.ToString());
                        //strPatInfo = strPatInfo.Replace("[Age]", "    :" + PatientResultEntry.AgeYear.ToString() + " Yrs " + PatientResultEntry.AgeMonth.ToString() + " Mnt " + PatientResultEntry.AgeDate.ToString() + " Dys");
                        ////strPatInfo = strPatInfo.Replace("[AgeMonth]", "    " + PatientResultEntry.AgeMonth.ToString() + " Mnt");
                        ////strPatInfo = strPatInfo.Replace("[AgeDate]", "    " + PatientResultEntry.AgeDate.ToString() + " Dys");
                        //strPatInfo = strPatInfo.Replace("[Sex]", "    :" + PatientResultEntry.Gender.ToString());

                        //if (PatientResultEntry.ReferredDoctor != "")
                        //{
                        //    strPatInfo = strPatInfo.Replace("[RefBy]", "    :" + "Dr. " + PatientResultEntry.ReferredDoctor.ToString());
                        //}
                        //else
                        //{
                        //    strPatInfo = strPatInfo.Replace("[RefBy]", "    :");
                        //}

                        //strPatInfo = strPatInfo.Replace("[BillNo]", "    :" + PatientResultEntry.BillNo.ToString());
                        //strPatInfo = strPatInfo.Replace("[RPTDATE]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy"));
                        //strPatInfo = strPatInfo.Replace("[RPTTIME]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToShortTimeString());
                        //strPatInfo = strPatInfo.Replace("[TemplateTestName]", PatientResultEntry.PrintTestName.ToString());

                        //if (IsFinalize == false)
                        //{
                        //    strPatInfo = strPatInfo.Replace("[NOTFINALIZED]", "RADIOLOGY REPORT");
                        //}
                        //else
                        //{
                        //    strPatInfo = strPatInfo.Replace("[NOTFINALIZED]", "");
                        //}

                        ///////////////////Commented Code END//////////////////////

                        /////////////For Logo/////////////////
                        //byte[] imageBytesNew = null;
                        //string imageBase64New = string.Empty;
                        //string imageSrcNew = string.Empty;

                        //imageBytesNew = PatientResultEntry.Signature2;

                        //imageBase64New = Convert.ToBase64String(imageBytesNew);
                        //imageSrcNew = string.Format("data:image/jpg;base64,{0}", imageBase64New);


                        //strPatInfo.Replace("[%Signature2%]", imageSrcNew);
                        
                        

                        /////////////END///////////////////



                        strDoctorPathInfo = new StringBuilder();
                     //   strDoctorPathInfo.Append(PatientResultEntry.DoctorInfoHTML);// commented code for removing footer info

                        byte[] imageBytes = null;
                        string imageBase64 = string.Empty;
                        string imageSrc = string.Empty;

                        if (PatientResultEntry.Radiologist1 != null)
                        {
                            if (PatientResultEntry.Signature1 != null)
                            {
                                imageBytes = PatientResultEntry.Signature1;

                                imageBase64 = Convert.ToBase64String(imageBytes);
                                imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                                if (PatientResultEntry.Radiologist == PatientResultEntry.RadioDoctorid1)
                                {
                                    //strDoctorPathInfo.Replace("[%Signature3%]", imageSrc);
                                    //strDoctorPathInfo.Replace("[%Image2%]","true");
                                }
                            }

                          //  strDoctorPathInfo.Replace("[Radiologist]", PatientResultEntry.Radiologist1);
                           // strDoctorPathInfo.Replace("[Degree]", PatientResultEntry.Education1);
                        }
                        else
                        {
                         //   strDoctorPathInfo.Replace("[%Radiologist4%]", string.Empty);
                           // strDoctorPathInfo.Replace("[%Education4%]", string.Empty);
                        }
                        ////-------------------------------------------------------------------------------------------

                        //if (PatientResultEntry.Radiologist2 != null)
                        //{
                        //    if (PatientResultEntry.Signature2 != null)
                        //    {
                        //        imageBytes = PatientResultEntry.Signature2;
                        //        imageBase64 = Convert.ToBase64String(imageBytes);
                        //        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                        //        if (PatientResultEntry.Radiologist == PatientResultEntry.RadioDoctorid2)
                        //        {
                        //            strDoctorPathInfo.Replace("[%Signature3%]", imageSrc);
                        //        }
                        //    }

                        //    strDoctorPathInfo.Replace("[%Radiologist3%]", PatientResultEntry.Radiologist2);
                        //    strDoctorPathInfo.Replace("[%Education3%]", PatientResultEntry.Education2);
                        //}
                        //else
                        //{
                        //    strDoctorPathInfo.Replace("[%Radiologist3%]", string.Empty);
                        //    strDoctorPathInfo.Replace("[%Education3%]", string.Empty);
                        //}
                        ////-------------------------------------------------------------------------------------------

                        //if (PatientResultEntry.Radiologist3 != null)
                        //{
                        //    if (PatientResultEntry.Signature3 != null)
                        //    {
                        //        imageBytes = PatientResultEntry.Signature3;
                        //        imageBase64 = Convert.ToBase64String(imageBytes);
                        //        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                        //        if (PatientResultEntry.Radiologist == PatientResultEntry.RadioDoctorid3)
                        //        {
                        //            strDoctorPathInfo.Replace("[%Signature2%]", imageSrc);
                        //        }
                        //    }

                        //    strDoctorPathInfo.Replace("[%Radiologist2%]", PatientResultEntry.Radiologist3);
                        //    strDoctorPathInfo.Replace("[%Education2%]", PatientResultEntry.Education3);
                        //}
                        //else
                        //{
                        //    strDoctorPathInfo.Replace("[%Radiologist2%]", string.Empty);
                        //    strDoctorPathInfo.Replace("[%Education2%]", string.Empty);
                        //}
                        ////-------------------------------------------------------------------------------------------

                        //if (PatientResultEntry.Radiologist4 != null)
                        //{
                        //    if (PatientResultEntry.Signature4 != null)
                        //    {
                        //        imageBytes = PatientResultEntry.Signature4;
                        //        imageBase64 = Convert.ToBase64String(imageBytes);
                        //        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                        //        if (PatientResultEntry.Radiologist == PatientResultEntry.RadioDoctorid4)
                        //        {
                        //            strDoctorPathInfo.Replace("[%Signature1%]", imageSrc);
                        //        }
                        //    }

                        //    strDoctorPathInfo.Replace("[%Radiologist1%]", PatientResultEntry.Radiologist4);
                        //    strDoctorPathInfo.Replace("[%Education1%]", PatientResultEntry.Education4);
                        //}
                        //else
                        //{
                        //    //strDoctorPathInfo.Replace("[%Radiologist1%]", string.Empty);Commented By Yogesh K 21112016
                        //    //strDoctorPathInfo.Replace("[%Education1%]", string.Empty);Commented By Yogesh K 21112016
                        //}


                        //richTextBox.Html = PatientResultEntry.PatientInfoHTML;
                        richTextBox.Html = PatientResultEntry.FirstLevelDescription;

                       // richTextBox.Html = PatientResultEntry.Radiologist1;
                        
                      
                        ///////////////Commented Code//////////////////////
                        //if (!richTextBox.Html.Contains("[%PATIENTINFO%]"))
                        //{
                        //    string strPTag = "<body><p class=" + "c0" + ">[%PATIENTINFO%]</p>";
                        //    richTextBox.Html = richTextBox.Html.Replace("<body>", strPTag);
                        //}

                        //richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());

                        //if (!richTextBox.Html.Contains("[%DOCTORINFO%]"))
                        //{
                        //    string strDTag = "<body><p class=" + "c0" + ">[%DOCTORINFO%]</p>";//Added By YK
                        //    //richTextBox.Html = richTextBox.Html.Replace("</body>", "<p class=" + "c0" + ">[%DOCTORINFO%]</p></body>");
                        //    richTextBox.Html = richTextBox.Html.Replace("</body>", strDTag);
                        //}

                        //richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());
                        //richTextBox.Html = richTextBox.Html.Replace("[%DOCTORINFO%]", strDoctorPathInfo.ToString());//Added By YK

                        ////////////////Commented End//////////////////

                     

                        PrintReport1(ResultId, PatientResultEntry, strPatInfo.ToString(), strDoctorPathInfo.ToString());
                        //PrintReport(ResultId, ISFinalize, strPatInfo.ToString());


                    }
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();

        }

        public PaperKind PaperKind { get; set; }
        public Thickness PrintMargin { get; set; }
        //PrintDocument pd;
        //void pd_PrintPage(object sender, PrintPageEventArgs e)
        //{

        //    e.PageVisual = dpC1RTB;


        //}
        private void PrintReport(long ResultID, bool IsFinalize, string PatientInfoString)
        {
            try
            {

                ////////////////////Added by yk new code for print docs///////////

                //pd = new PrintDocument();
                //pd.PrintPage += new EventHandler<PrintPageEventArgs>(pd_PrintPage);
                //pd.Print("Radiology");
                ///////////////////////END////////////////////



              

                richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", PatientInfoString);

                var viewManager = new C1RichTextViewManager
                {
                    Document = richTextBox.Document,
                    PresenterInfo = richTextBox.ViewManager.PresenterInfo
                };
                var print = new PrintDocument();
                int presenter = 0;
                print.PrintPage += (s, printArgs) =>
                {
                    var element = (FrameworkElement)printTemplate.LoadContent();
                    //element.Margin = new Thickness(0, 0, 30, 0);
                    element.DataContext = viewManager.Presenters[presenter];
                    //  element.DataContext = viewManager.Presenters[0];
                    printArgs.PageVisual = element;
                    printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;
                    //printArgs.PageMargins.Left = 4;
                };

                print.BeginPrint += new EventHandler<BeginPrintEventArgs>(print_BeginPrint);
                print.PrintPage += new EventHandler<PrintPageEventArgs>(print_PrintPage);
                print.Print("A Christmas Carol");

               

            }
            catch (Exception) { }


        }
        


        //private void PrintReport1(long PrintID, clsRadResultEntryPrintDetailsVO PatientResultEntry, string strPatInfo, string strDoctorPathInfo)
        //{
        //    try
        //    {
        //        richTextBox.Html = PatientResultEntry.PatientInfoHTML;
        //        string rtbstring = string.Empty;
        //        string styleString = string.Empty;

        //        rtbstring = richTextBox.Html;

        //        //if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
        //        //{
        //        //    rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "[%PATIENTINFO%]");
        //        //    richTextBox.Html = rtbstring;
        //        //    TemplateContent = rtbstring;
        //        //}

        //        //if (!(richTextBox.Html.Contains("[%DOCTORINFO%]")))
        //        //{
        //        //    rtbstring = rtbstring.Insert(rtbstring.IndexOf("</body>"), "[%DOCTORINFO%]");
        //        //    richTextBox.Html = rtbstring;
        //        //    TemplateContent = rtbstring;
        //        //}

        //        //richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());
        //        //richTextBox.Html = richTextBox.Html.Replace("[%DOCTORINFO%]", strDoctorPathInfo.ToString());

        //        richTextBox.Document.Margin = new Thickness(5, 5, 5, 5);
        //        //richTextBox.Document.Margin = new Thickness(5, 40, 5, 80);

        //        PrintMargin = new Thickness(5, 230, 5, 100);
        //        //PrintMargin = new Thickness(5, 190, 5, 50);

        //        //Printing
        //        var viewManager = new C1RichTextViewManager
        //        {
        //            Document = richTextBox.Document,
        //            PresenterInfo = richTextBox.ViewManager.PresenterInfo
        //        };
        //        var print = new PrintDocument();
        //        int presenter = 0;

        //        print.PrintPage += (s, printArgs) =>
        //        {

        //            //MessageBoxControl.MessageBoxChildWindow msgW1 =
        //            //  new MessageBoxControl.MessageBoxChildWindow("", "Print.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            //msgW1.Show();

        //            var element = (FrameworkElement)printTemplate.LoadContent();
        //            Grid grd = (Grid)element.FindName("PatientDetails");

        //            if (grd != null)
        //            {
        //                grd.Visibility = Visibility.Visible;
        //                grd.DataContext = PatientResultEntry;

        //            }


        //            element.DataContext = viewManager.Presenters[presenter];
        //            printArgs.PageVisual = element;
        //            printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;
        //        };

        //        var pdf = new C1PdfDocument(PaperKind.A4);




        //        PdfFilter.PrintDocument(richTextBox.Document, pdf, PrintMargin);


        //        var di = pdf.DocumentInfo;
        //        var font1 = new Font("Arial", 8, PdfFontStyle.Bold);

        //        var Rpfont = new Font("Verdana", 10, PdfFontStyle.Bold);
        //        var Rpfontdata = new Font("Verdana", 10, PdfFontStyle.Regular);

        //        var fmt = new StringFormat();
        //        fmt.Alignment = HorizontalAlignment.Left;
        //        fmt.LineAlignment = VerticalAlignment.Top;
        //        //fmt.LineAlignment = VerticalAlignment.Bottom;
        //        fmt.LineSpacing = -1.4;

        //        Font titleFont = new Font("Tahoma", 24, PdfFontStyle.Bold);
        //        //Font headerFont = new Font("Arial", 10, PdfFontStyle.Bold);
        //        //Font bodyFont = new Font("Times New Roman", 4);


        //        var bkmk = new List<string[]>();

        //        Rect rcPage = PdfUtils.PageRectangle(pdf);
        //        Rect rc = PdfUtils.RenderParagraph(pdf, pdf.DocumentInfo.Title, titleFont, rcPage, rcPage, false);


        //        var ftr = new StringFormat();
        //        ftr.Alignment = HorizontalAlignment.Center;
        //        ftr.LineAlignment = VerticalAlignment.Bottom;


        //        int PageCnt = pdf.Pages.Count;

        //        string name = string.Empty;
        //        string mrno = string.Empty;

        //        string age = string.Empty;
        //        string orderDate = string.Empty;

        //        string gender = string.Empty;
        //        string repoTime = string.Empty;

        //        string contno = string.Empty;
        //        string company = string.Empty;

        //        string patientCategory = string.Empty;
        //        string referenceNo = string.Empty;

        //        string patientSource = string.Empty;
        //        string sampleCollectionTime = string.Empty;

        //        string referredBy = string.Empty;
        //        string donarCode = string.Empty;

        //        //if (PatientResultEntry.Salutation.ToString() != null)
        //        //    name = PatientResultEntry.Salutation.ToString().Trim();
        //        //else
        //        //    name = "";

        //        if (PatientResultEntry.PatientName.ToString() != null)
        //            name = name + PatientResultEntry.PatientName.ToString().Trim();
        //        else
        //            name = "                                     ";

        //        if (PatientResultEntry.MRNo.ToString() != null)
        //            mrno = PatientResultEntry.MRNo.ToString().Trim();
        //        else
        //            mrno = "                                     ";

        //        if (PatientResultEntry.AgeYear.ToString() != null)
        //            age = PatientResultEntry.AgeYear.ToString().Trim() + " Years " + PatientResultEntry.AgeMonth.ToString().Trim() + " Month(s) " + PatientResultEntry.AgeDate.ToString().Trim() + " Days";
        //        else
        //            age = "                                     ";

        //        if (PatientResultEntry.ResultAddedDateTime.Value.ToString() != null)
        //        { //orderDate = PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy hh:mm tt").Trim();   String.Format("{0:MM/dd/yyyy}", 
        //            orderDate = PatientResultEntry.ResultAddedDateTime.Value.ToString("dd/MM/yyyy").Trim();
        //        }
        //        else
        //            orderDate = "                                     ";

        //        if (PatientResultEntry.Gender.ToString() != null)
        //            gender = PatientResultEntry.Gender.ToString().Trim();
        //        else
        //            gender = "                                     ";

        //        if (PatientResultEntry.ResultAddedDateTime.Value.ToString() != null)
        //        {
        //            repoTime = PatientResultEntry.ResultAddedDateTime.Value.ToString("t").Trim();
        //            //repoTime = PatientResultEntry.ResultAddedDateTime.Value.ToString("hh:mm").Trim();
        //        }
        //        else
        //            repoTime = "                                     ";

        //        //if (PatientResultEntry.ContactNo.ToString() != null)
        //        //    contno = PatientResultEntry.ContactNo.ToString().Trim();
        //        //else
        //        //    contno = "                                     ";

        //        //if (PatientResultEntry.Company.ToString() != null)
        //        //    company = PatientResultEntry.Company.ToString().Trim();
        //        //else
        //        //    company = "                                     ";


        //        //if (PatientResultEntry.PatientCategory.ToString() != null)
        //        //    patientCategory = PatientResultEntry.PatientCategory.ToString().Trim();
        //        //else
        //        //    patientCategory = "                                     ";

        //        //if (PatientResultEntry.ReferenceNo.ToString() != null)
        //        //    referenceNo = PatientResultEntry.ReferenceNo.ToString().Trim();
        //        //else
        //        //    referenceNo = "                                     ";


        //        //if (PatientResultEntry.PatientSource.ToString() != null)
        //        //    patientSource = patientSource = PatientResultEntry.PatientSource.ToString().Trim();
        //        //else
        //        //    patientSource = "                                     ";

        //        //if (PatientResultEntry.SampleCollectionTime.Value.ToString() != null)
        //        //    sampleCollectionTime = ":" + PatientResultEntry.SampleCollectionTime.Value.ToString("dd MMM yyyy hh:mm tt").Trim();
        //        //else
        //        //    sampleCollectionTime = "                                     ";

        //        //if (PatientResultEntry.ReferredDoctor.ToString() != null)
        //        //    referredBy = PatientResultEntry.ReferredDoctor.ToString().Trim();
        //        //else
        //        //    referredBy = "                                     ";

        //        //if (PatientResultEntry.DonarCode.ToString() != null)
        //        //    donarCode = PatientResultEntry.DonarCode.ToString().Trim();
        //        //else
        //        //    donarCode = "                                     ";



        //        string pathocategory = string.Empty;
        //        string testname = string.Empty;

        //        string sampleNo = string.Empty;

        //        //if (PatientResultEntry.SampleNo.ToString() != null)
        //        //    sampleNo = ":" + PatientResultEntry.SampleNo.ToString().Trim();
        //        //else
        //        //    sampleNo = "                                     ";



        //        //if (PatientResultEntry.PathoCategory.ToString() != null)
        //        //    pathocategory = PatientResultEntry.PathoCategory.ToString().Trim();

        //        if (PatientResultEntry.TestTemplate.ToString() != null)
        //            testname = PatientResultEntry.TestTemplate.ToString().Trim();

        //        for (int page = 0; page < pdf.Pages.Count; page++)
        //        {

        //            byte[] imageBytes = null;
        //            imageBytes = PatientResultEntry.Signature2;

        //            Stream imgstream = new MemoryStream(imageBytes);
        //            BitmapImage bi = new BitmapImage();
        //            bi.SetSource(imgstream);
        //            var wb = new WriteableBitmap(bi);
        //            // pdf.DrawImage(wb, PdfUtils.Inflate(pdf.PageRectangle, 0, 10), ContentAlignment.TopCenter, Stretch.Uniform);//Commented By YK 080317
        //            pdf.DrawImage(wb, PdfUtils.Inflate(pdf.PageRectangle, 0, 10), ContentAlignment.TopLeft, Stretch.None);//Added By YK

        //            //string h1 = "Name                 "+"    :"+name.ToString()+"                                             "+"MRNO                "+"    :"+mrno.ToString()+"                        "+"\n";
        //            //string h2 = "Age                  "+"    :"+age.ToString()+"                                              "+"OrderDate           "+"    :"+orderDate.ToString()+"                   "+"\n";
        //            //string h3 = "Gender               "+"    :"+gender.ToString()+"                                           "+"Reported Time       "+"    :"+orderDate.ToString()+"                   "+"\n";
        //            //string h4 = "Patient Contact No"+"    :"+contno.ToString()+"                                           "+"Company Name        "+"    :"+company.ToString()+"                     "+"\n";
        //            //string h5 = "Patient Category 1"+"    :"+patientCategory.ToString()+"                                  "+"Reference No        "+"    :"+referenceNo.ToString() + "               "+"\n";
        //            //string h6 = "Patient Category 2"+"    :"+patientSource.ToString()+"                                    "+"Collection Time     "+"    :"+sampleCollectionTime.ToString()+"        "+"\n";
        //            //string h7 = "Referred By       "+"    :"+referredBy.ToString()+"                                       "+"D / S Code          "+"    :"+donarCode.ToString()+"                   "+"\n";


        //            //string h1 = "Name                          :"+name.ToString() +"\n";                                           
        //            //string h2 = "Age                             :"+age.ToString() +"\n";                                            
        //            //string h3 = "Gender                       :"+gender.ToString() +"\n";                                         
        //            //string h4 = "Patient Contact No    :"+contno.ToString() +"\n";                                          
        //            //string h5 = "Patient Category 1    :"+patientCategory.ToString()  +"\n";
        //            //string h6 = "Patient Category 2    :"+patientSource.ToString() +"\n";                                   
        //            //string h7 = "Referred By               :"+referredBy.ToString() +"\n";                                       

        //            //string h8 ="MRNO                      :"+mrno.ToString()+"\n";
        //            //string h9 ="Date                       :"+orderDate.ToString()+"\n";
        //            //string h10 ="Reported Time        :"+orderDate.ToString()+"\n";
        //            //string h11 ="Company Name      :"+company.ToString()+"\n";
        //            //string h12 ="Reference No          :"+referenceNo.ToString()+"\n";
        //            //string h13 ="Collection Time      :"+sampleCollectionTime.ToString()+"\n";
        //            //string h14 ="D / S Code               :" + donarCode.ToString()+ "\n";


        //            string h1 = "Name                  " + "\n";
        //            string h2 = "Age                   " + "\n";
        //            string h3 = "Gender                " + "\n";
        //            string h4 = "Patient Contact No    " + "\n";
        //            // string h5 = "Patient Category 1    "+"\n";
        //            // string h6 = "Patient Category 2    "+"\n";
        //            string h7 = "Referred By           " + "\n";

        //            string d1 = ":" + name.ToString() + "\n";
        //            string d2 = ":" + age.ToString() + "\n";
        //            string d3 = ":" + gender.ToString() + "\n";
        //            string d4 = ":" + contno.ToString() + "\n";
        //            // string d5 =":"+patientCategory.ToString() + "\n";
        //            // string d6 =":"+patientSource.ToString() + "\n";
        //            string d7 = ":" + referredBy.ToString() + "\n";

        //            string h8 = "MR No                  " + "\n";
        //            string h9 = "Date                   " + "\n";
        //            string h10 = "Reported Time         " + "\n";
        //            // string h11 = "Company Name          "+"\n";   
        //            string h12 = "Reference No          " + "\n";
        //            //  string h13 = "D / S Code            " + "\n"; 
        //            //string h13 = "Collection Time       "+"\n";   
        //            //string h14 = "D / S Code            "+"\n";      

        //            string d8 = ":" + mrno.ToString() + "\n";
        //            string d9 = ":" + orderDate.ToString() + "\n";
        //            string d10 = ":" + repoTime.ToString() + "\n";
        //            // string d11 =":"+company.ToString() + "\n";
        //            string d12 = ":" + referenceNo.ToString() + "\n";
        //            // string d13 = ":" + donarCode.ToString() + "\n";
        //            //string d13 =":"+ sampleCollectionTime.ToString() + "\n";
        //            //string d14 =":"+ donarCode.ToString() + "\n";

        //            string header = string.Empty;
        //            string headerdata = string.Empty;
        //            string header1 = string.Empty;
        //            string headerdata1 = string.Empty;

        //            string f1 = string.Empty;   // PatientResultEntry.AdressLine1.Trim().ToString() + "\t" + "\t" + "\n"; ;
        //            string f2 = string.Empty;                //PatientResultEntry.Email.Trim().ToString() + "\t" + "\t" + PatientResultEntry.UnitContactNo.Trim().ToString() + "\n";

        //            string Footer = string.Empty;

        //            Footer = string.Format("{0}", f1 + f2);
        //            var f = string.Format("{0}", Footer);

        //            //Pen pen = new Pen(Colors.Red);
        //            Pen pen = new Pen(Colors.Gray, 2.0f);
        //            int i = pdf.CurrentPage;

        //            if (pdf.CurrentPage == 0)
        //            {
        //                //header = string.Format("{0}", h1 + h2 + h3 + h4 + h5 + h6 + h7);
        //                header = string.Format("{0}", h1 + h2 + h3 + h4 + h7);
        //                header = string.Format("{0}", h1 + h2 + h3 + h4 + h7);
        //                var a = string.Format("{0}", header);

        //                headerdata = string.Format("{0}", d1 + d2 + d3 + d4 + d7);
        //                var adata = string.Format("{0}", headerdata);

        //                //header1 = string.Format("{0}", h8 + h9 + h10 + h11 + h12 + h13 + h14);
        //                header1 = string.Format("{0}", h8 + h9 + h10 + h12);
        //                var a1 = string.Format("{0}", header1);

        //                //headerdata1 = string.Format("{0}", d8 + d9 + d10 + d11 + d12 + d13 + d14);
        //                headerdata1 = string.Format("{0}", d8 + d9 + d10 + d12);
        //                var adata1 = string.Format("{0}", headerdata1);

        //                int X1 = 10;
        //                int Y1 = 91;
        //                int X2 = 570;
        //                int Y2 = 91;

        //                //Comment Code 22 3 17
        //              //  pdf.DrawLine(pen, X1, Y1, X2, Y2);
        //                //END

        //                int x = 10;
        //                //int y = 50;
        //                int y = 93;
        //                int width = 450;
        //                int height = 100;
        //                Rect Hrect1 = new Rect(x, y, width, height);
        //                pdf.DrawString(a, Rpfont, Colors.Black, Hrect1, fmt);
        //                //pdf.DrawRectangle(pen, Hrect1);

        //                //Rect Hrect1data = new Rect(110, 50, 450, 130);
        //                Rect Hrect1data = new Rect(110, 93, 450, 100);
        //                pdf.DrawString(adata, Rpfontdata, Colors.Black, Hrect1data, fmt);

        //                //Rect Hrect2 = new Rect(310, 50, 450, 130);
        //                Rect Hrect2 = new Rect(310, 93, 450, 100);
        //                pdf.DrawString(a1, Rpfont, Colors.Black, Hrect2, fmt);

        //                Rect Hrect2data = new Rect(400, 93, 450, 100);
        //                pdf.DrawString(adata1, Rpfontdata, Colors.Black, Hrect2data, fmt);

        //                //pdf.DrawRectangle(pen, Hrect2);

        //                int XX1 = 10;
        //                int YY1 = 195;
        //                int XX2 = 570;
        //                int YY2 = 195;
        //                pdf.DrawLine(pen, XX1, YY1, XX2, YY2);

        //                var apatho = string.Format("{0}", pathocategory);

        //                var R1f = new Font("Verdana", 12, PdfFontStyle.Bold);
        //                Rect R1 = new Rect(10, 200, 450, 130);
        //                pdf.DrawString(apatho, R1f, Colors.Black, R1, fmt);



        //                var atestname = string.Format("{0}", testname);
        //                var R2f = new Font("Verdana", 11, PdfFontStyle.Bold);
        //                Rect R2 = new Rect(10, 220, 450, 130);
        //                //pdf.DrawString(atestname, R2f, Colors.Purple, R2, fmt);   
        //                pdf.DrawString(atestname, R2f, Color.FromArgb(0xff, 97, 48, 144), R2, fmt);


        //                //// For Sample No And Collection Time

        //                //string sa1 = "Sample No                  " + "\n";
        //                //var asampleNo = string.Format("{0}", sa1);

        //                //var R3f = new Font("Verdana", 9, PdfFontStyle.Bold);
        //                //Rect R3 = new Rect(400, 220, 450, 130);
        //                //pdf.DrawString(asampleNo, R3f, Colors.Black, R3, fmt);

        //                //var adatasampleNo = string.Format("{0}", sampleNo);
        //                //var R4f = new Font("Verdana", 9, PdfFontStyle.Bold);
        //                //Rect R4 = new Rect(475, 220, 450, 130);
        //                //pdf.DrawString(adatasampleNo, R4f, Colors.Black, R4, fmt);


        //                //string col1 = "Collection Time      " + "\n";
        //                //var acol1 = string.Format("{0}", col1);

        //                //var R5f = new Font("Verdana", 9, PdfFontStyle.Bold);
        //                //Rect R5 = new Rect(400, 230, 450, 130);
        //                //pdf.DrawString(acol1, R5f, Colors.Black, R5, fmt);

        //                //var adataColTime = string.Format("{0}", sampleCollectionTime.ToString());
        //                //var R6f = new Font("Arial", 10, PdfFontStyle.Regular);
        //                //Rect R6 = new Rect(475, 230, 450, 130);
        //                //pdf.DrawString(adataColTime, R6f, Colors.Black, R6, fmt);

        //                //// For Sample No And Collection Time

        //                //Footer
        //                //Rect Frect1 = new Rect(10, 750, 500, 50);
        //                //pdf.DrawString(f, font, Colors.Black, Frect1, ftr);  

        //            }
        //            else
        //            {

        //                header = string.Format("{0}", h1 + h2 + h3);
        //                var a = string.Format("{0}", header);

        //                headerdata = string.Format("{0}", d1 + d2 + d3);
        //                var adata = string.Format("{0}", headerdata);

        //                header1 = string.Format("{0}", h8 + h9 + h10);
        //                var a1 = string.Format("{0}", header1);

        //                headerdata1 = string.Format("{0}", d8 + d9 + d10);
        //                var adata1 = string.Format("{0}", headerdata1);

        //                int X1 = 10;
        //                int Y1 = 91;
        //                int X2 = 570;
        //                int Y2 = 91;
        //                //Code Commented
        //            //    pdf.DrawLine(pen, X1, Y1, X2, Y2);
        //                //END


        //                int x = 10;
        //                //int y = 55;
        //                int y = 93;
        //                int width = 450;
        //                int height = 80;
        //                Rect Hrect1 = new Rect(x, y, width, height);
        //                pdf.DrawString(a, Rpfont, Colors.Black, Hrect1, fmt);
        //                //pdf.DrawRectangle(pen, Hrect1);

        //                //Rect Hrect1data = new Rect(110, 55, 450, 80);
        //                Rect Hrect1data = new Rect(110, 93, 450, 80);
        //                pdf.DrawString(adata, Rpfontdata, Colors.Black, Hrect1data, fmt);


        //                //Rect Hrect2 = new Rect(310, 55, 450, 80);
        //                Rect Hrect2 = new Rect(310, 93, 450, 80);
        //                pdf.DrawString(a1, Rpfont, Colors.Black, Hrect2, fmt);

        //                //Rect Hrect2data = new Rect(400, 55, 450, 80);
        //                Rect Hrect2data = new Rect(400, 93, 450, 80);
        //                pdf.DrawString(adata1, Rpfontdata, Colors.Black, Hrect2data, fmt);

        //                //pdf.DrawRectangle(pen, Hrect2);


        //                int XX1 = 10;
        //                int YY1 = 140;
        //                int XX2 = 570;
        //                int YY2 = 140;
        //                pdf.DrawLine(pen, XX1, YY1, XX2, YY2);

        //                //var apatho = string.Format("{0}", pathocategory);
        //                //var R1f = new Font("Verdana", 12, PdfFontStyle.Bold);
        //                //Rect R1 = new Rect(10, 70, 450, 130);
        //                //pdf.DrawString(apatho, R1f, Colors.Black, R1, fmt);

        //                //var atestname = string.Format("{0}", testname);
        //                //var R2f = new Font("Verdana", 11, PdfFontStyle.Bold);
        //                //Rect R2 = new Rect(10, 80, 450, 130);
        //                //pdf.DrawString(atestname, R2f, Colors.Black, R2, fmt);   

        //                //Footer
        //                //Rect Frect1 = new Rect(10, 750, 500, 50);
        //                //pdf.DrawString(f, font, Colors.Black, Frect1, ftr);                            
        //            }

        //            var pn = new StringFormat();
        //            pn.Alignment = HorizontalAlignment.Right;
        //            pn.LineAlignment = VerticalAlignment.Bottom;

        //            pdf.CurrentPage = page;
        //            var text = string.Format("Page {1} of {2}", di.Title, page + 1, pdf.Pages.Count);
        //            pdf.DrawString(text, font1, Colors.Black, PdfUtils.Inflate(pdf.PageRectangle, -72, -36), pn);

        //        }




        //        pdf.CurrentPage = pdf.Pages.Count - 1;
        //        String appPath;

        //        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

        //        appPath = "RadioReport" + UnitID + "_" + PrintID + ".pdf";

        //        Stream FileStream = new MemoryStream();
        //        MemoryStream MemStrem = new MemoryStream();

        //        pdf.Save(MemStrem);
        //        FileStream.CopyTo(MemStrem);

        //        Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
        //        DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

        //        client.UploadReportFileForRadiologyCompleted += (s, args) =>
        //        {
        //            if (args.Error == null)
        //            {
        //                // WaitNew.Close();
        //                //ViewPDFReport(appPath);
        //            }
        //        };
        //        client.UploadReportFileForRadiologyAsync(appPath, MemStrem.ToArray());
        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    finally
        //    {
        //        // WaitNew.Close();
        //    }
        //}



        private void PrintReport1(long PrintID, clsRadResultEntryPrintDetailsVO PatientResultEntry, string strPatInfo, string strDoctorPathInfo)
        {
            try
            {
                //richTextBox.Html = PatientResultEntry.PatientInfoHTML;
                richTextBox.Html = PatientResultEntry.FirstLevelDescription;

                string rtbstring = string.Empty;
                string styleString = string.Empty;

                rtbstring = richTextBox.Html;

                if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                {
                    rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "[%PATIENTINFO%] </br>"); //Change By Bhushanp 29052017
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

                richTextBox.Document.Margin = new Thickness(5, 5, 5, 5);
                //richTextBox.Document.Margin = new Thickness(5, 40, 5, 80);

                PrintMargin = new Thickness(5, 200, 5, 100);// 230 to 180
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

                Font titleFont = new Font("Tahoma", 24, PdfFontStyle.Bold);
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

                string testname = string.Empty;

                string UnitName = string.Empty;
                string UnitAddress = string.Empty;
                string UnitContact = string.Empty;
                string UnitContact1 = string.Empty;
                string UnitMobileNO = string.Empty;

               
                string UnitEmail = string.Empty;
                string UnitWebsite = string.Empty;

                //if (PatientResultEntry.Salutation.ToString() != null || PatientResultEntry.Salutation=="")
                //    name = PatientResultEntry.Salutation.ToString().Trim();
                //else
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

                //if (PatientResultEntry.ContactNo.ToString() != null)
                //    contno = PatientResultEntry.ContactNo.ToString().Trim();
                //else
                    contno = "                                     ";

                //if (PatientResultEntry.Company.ToString() != null)
                //    company = PatientResultEntry.Company.ToString().Trim();
                //else
                    company = "                                     ";


                //if (PatientResultEntry.PatientCategory.ToString() != null)
                //    patientCategory = PatientResultEntry.PatientCategory.ToString().Trim();
                //else
                    patientCategory = "                                     ";

                    //if (PatientResultEntry.ReferenceNo.ToString() != null)
                    //    referenceNo = PatientResultEntry.ReferenceNo.ToString().Trim();
                    //else
                    referenceNo = "                                     ";


                //if (PatientResultEntry.PatientSource.ToString() != null)
                //    patientSource = patientSource = PatientResultEntry.PatientSource.ToString().Trim();
                //else
                    patientSource = "                                     ";

                //if (PatientResultEntry.SampleCollectionTime.Value.ToString() != null)
                //    sampleCollectionTime = ":" + PatientResultEntry.SampleCollectionTime.Value.ToString("dd MMM yyyy hh:mm tt").Trim();
                //else
                    sampleCollectionTime = "                                     ";

                    if (PatientResultEntry.ReferredDoctor.ToString() != null)
                        referredBy = PatientResultEntry.ReferredDoctor.ToString().Trim();
                    else
                    referredBy = "                                     ";

                /////////////////////////////////////Unit Details////////////////////////////////////

                    if (PatientResultEntry.UnitName.ToString() != null)
                        UnitName = PatientResultEntry.UnitName.ToString().Trim();
                    else
                        UnitName = "                                     ";

                    if (PatientResultEntry.UnitAddress.ToString() != null)
                        UnitAddress = PatientResultEntry.UnitAddress.ToString().Trim();
                    else
                        UnitAddress = "                                     ";

                    if (PatientResultEntry.UnitContact.ToString() != null || PatientResultEntry.UnitContact1.ToString() != null || PatientResultEntry.UnitMobileNo.ToString() != null)
                    {
                        UnitContact = PatientResultEntry.UnitContact.ToString().Trim();
                        UnitContact1 = "Ph : "+ UnitContact +","+ PatientResultEntry.UnitContact1.ToString().Trim();

                        UnitMobileNO = UnitContact1 + " Mob : " + PatientResultEntry.UnitMobileNo.ToString().Trim();
                    }
                    else
                        UnitContact1 = "                                     ";


                    //if (PatientResultEntry.UnitMobileNo.ToString() != null)
                    //{
                    //    UnitMobileNO = UnitContact1+ "Mob: "+ PatientResultEntry.UnitMobileNo.ToString().Trim();
                      
                    //}
                    //else
                    //    UnitMobileNO = "                                     ";


                    if (PatientResultEntry.UnitEmail.ToString() != null)
                    {
                        UnitEmail = "Email : " + PatientResultEntry.UnitEmail.ToString().Trim();

                    }
                    else
                        UnitEmail = "                                     ";

                    if (PatientResultEntry.UnitWebsite.ToString() != null)
                    {
                        UnitWebsite = "Website : " + PatientResultEntry.UnitWebsite.ToString().Trim();

                    }
                    else
                        UnitWebsite = "                                     ";


                    //if (PatientResultEntry.uni.ToString() != null)
                    //    UnitAddress = PatientResultEntry.UnitAddress.ToString().Trim();
                    //else
                    //    UnitAddress = "                                     ";
                ////////////////////////////////////////END///////////////////////////////////////

                //if (PatientResultEntry.DonarCode.ToString() != null)
                //    donarCode = PatientResultEntry.DonarCode.ToString().Trim();
                //else
                    donarCode = "                                     ";



                string pathocategory = string.Empty;
                //string testname = string.Empty;

                string sampleNo = string.Empty;

                //if (PatientResultEntry.SampleNo.ToString() != null)
                //    sampleNo = ":" + PatientResultEntry.SampleNo.ToString().Trim();
                //else
                   sampleNo = "                                     ";



                //if (PatientResultEntry.PathoCategory.ToString() != null)
                //    pathocategory = PatientResultEntry.PathoCategory.ToString().Trim();

                if (PatientResultEntry.TestTemplate.ToString() != null)
                    testname = PatientResultEntry.TestTemplate.ToString().Trim();

                for (int page = 0; page < pdf.Pages.Count; page++)
                {

                    byte[] imageBytes = null;
                    imageBytes = PatientResultEntry.Signature2;

                    Stream imgstream = new MemoryStream(imageBytes);
                    BitmapImage bi = new BitmapImage();
                    bi.SetSource(imgstream);
                    var wb = new WriteableBitmap(bi);
                    // pdf.DrawImage(wb, PdfUtils.Inflate(pdf.PageRectangle, 0, 10), ContentAlignment.TopCenter, Stretch.Uniform);//Commented By YK 080317
                  
                    
                    ///commented by yk 03/04/2017
                  //  pdf.DrawImage(wb, PdfUtils.Inflate(pdf.PageRectangle, 0, 10), ContentAlignment.TopLeft, Stretch.None);//Added By YK
                    //end


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


                    string h1 = "Name                  " + "\n";
                    string h2 = "Age                   " + "\n";
                    string h3 = "Gender                " + "\n";
                  //  string h4 = "Patient Contact No    " + "\n";//
                    // string h5 = "Patient Category 1    "+"\n";
                    // string h6 = "Patient Category 2    "+"\n";
                 //   string h7 = "Referred By           " + "\n";//

                    string d1 = ":" + name.ToString() + "\n";
                    string d2 = ":" + age.ToString() + "\n";
                    string d3 = ":" + gender.ToString() + "\n";
                  //  string d4 = ":" + contno.ToString() + "\n";//
                    // string d5 =":"+patientCategory.ToString() + "\n";
                    // string d6 =":"+patientSource.ToString() + "\n";
                 //   string d7 = ":" + referredBy.ToString() + "\n";//

                    string h8 = "MR No                  " + "\n";
                    string h9 = "Date                   " + "\n";
                    string h10 = "Reported Time         " + "\n";
                    // string h11 = "Company Name          "+"\n";   
                  //  string h12 = "Reference No          " + "\n";//
                    //  string h13 = "D / S Code            " + "\n"; 
                    //string h13 = "Collection Time       "+"\n";   
                    //string h14 = "D / S Code            "+"\n";   
                    
                    string hu1 = "Name                  " + "\n";
                    string du1 = "" + UnitName.ToString() + "\n";

                    

                    string hu2 = "Address                  " + "\n";
                    string du2 = "" + UnitAddress.ToString() + "\n";

                    string du3 = "" + UnitMobileNO.ToString() + "\n";
                    string du4 = "" + UnitEmail.ToString() + "\n";
                    string du5 = "" + UnitWebsite.ToString() + "\n";
                  

                    string d8 = ":" + mrno.ToString() + "\n";
                    string d9 = ":" + orderDate.ToString() + "\n";
                    string d10 = ":" + repoTime.ToString() + "\n";
                    // string d11 =":"+company.ToString() + "\n";
                //    string d12 = ":" + referenceNo.ToString() + "\n";//
                    // string d13 = ":" + donarCode.ToString() + "\n";
                    //string d13 =":"+ sampleCollectionTime.ToString() + "\n";
                    //string d14 =":"+ donarCode.ToString() + "\n";

                    string header = string.Empty;
                    string headerdata = string.Empty;
                    string header1 = string.Empty;
                    string headerdata1 = string.Empty;

                    string HeaderUnitData = string.Empty;

                    //string f1 = PatientResultEntry.AdressLine1.Trim().ToString() + "\t" + "\t" + "\n"; ;
                    //string f2 = PatientResultEntry.Email.Trim().ToString() + "\t" + "\t" + PatientResultEntry.UnitContactNo.Trim().ToString() + "\n";

                    string Footer = string.Empty;

                 //   Footer = string.Format("{0}", f1 + f2);
                    var f = string.Format("{0}", Footer);

                    //Pen pen = new Pen(Colors.Red);
                    Pen pen = new Pen(Colors.Gray, 2.0f);
                    int i = pdf.CurrentPage;
                    HeaderUnitData = string.Format("{0}", du1 + du2 + du3 + du4 + du5);//added by yk
                    var unitData = string.Format("{0}", HeaderUnitData);
                    if (pdf.CurrentPage == 0)
                    {
                        //header = string.Format("{0}", h1 + h2 + h3 + h4 + h5 + h6 + h7);
                        header = string.Format("{0}", h1 + h2 + h3 );//h4 h7
                        header = string.Format("{0}", h1 + h2 + h3);
                        var a = string.Format("{0}", header);

                        headerdata = string.Format("{0}", d1 + d2 + d3 );
                        var adata = string.Format("{0}", headerdata);

                        //header1 = string.Format("{0}", h8 + h9 + h10 + h11 + h12 + h13 + h14);

                      

                        header1 = string.Format("{0}", h8 + h9 + h10 );
                        var a1 = string.Format("{0}", header1);

                        //headerdata1 = string.Format("{0}", d8 + d9 + d10 + d11 + d12 + d13 + d14);
                        headerdata1 = string.Format("{0}", d8 + d9 + d10 );
                        var adata1 = string.Format("{0}", headerdata1);

                        int X1 = 10;
                        int Y1 = 150;
                        int X2 = 570;
                        int Y2 = 150;
                        //int X1 = 10;
                        //int Y1 = 141;
                        //int X2 = 520;
                        //int Y2 = 141;
                        pdf.DrawLine(pen, X1, Y1, X2, Y2);
                        
                        ////////////////////////////////////////////For Unit details/////////////////
                        int ux = 290;//310 to 290
                        //int y = 50;
                        int uy = 35;//93
                        // int y = 141;
                        int uwidth = 250;
                        int uheight = 100;
                        Rect uHrect1 = new Rect(ux, uy, uwidth, uheight);
                        pdf.DrawString(unitData, Rpfontdata, Colors.Black, uHrect1, fmt);
                        ///////////////////////////////////////END////////////////////////////////

                        //int x = 100;
                        int x = 10;
                        //int y = 50;
                        int y = 155;//93
                       // int y = 141;
                        int width = 450;
                        int height = 100;
                        Rect Hrect1 = new Rect(x, y, width, height);
                        pdf.DrawString(a, Rpfont, Colors.Black, Hrect1, fmt);
                        //pdf.DrawRectangle(pen, Hrect1);

                        //Rect Hrect1data = new Rect(110, 50, 450, 130);
                        Rect Hrect1data = new Rect(110, 155, 450, 100);
                        pdf.DrawString(adata, Rpfontdata, Colors.Black, Hrect1data, fmt);

                        //Rect Hrect2 = new Rect(310, 50, 450, 130);
                        Rect Hrect2 = new Rect(310, 155, 450, 100);
                        pdf.DrawString(a1, Rpfont, Colors.Black, Hrect2, fmt);

                        Rect Hrect2data = new Rect(400, 155, 450, 100);
                        pdf.DrawString(adata1, Rpfontdata, Colors.Black, Hrect2data, fmt);

                        //pdf.DrawRectangle(pen, Hrect2);

                        int XX1 = 10;
                        //int YY1 = 195;
                        int YY1 = 205;
                        int XX2 = 570;
                       // int YY2 = 195;
                        int YY2 = 205;
                        pdf.DrawLine(pen, XX1, YY1, XX2, YY2);

                        var apatho = string.Format("{0}", pathocategory);

                        var R1f = new Font("Verdana", 12, PdfFontStyle.Bold);
                        Rect R1 = new Rect(10, 125, 450, 130);
                        pdf.DrawString(apatho, R1f, Colors.Black, R1, fmt);



                        var atestname = string.Format("{0}", testname);
                        var R2f = new Font("Verdana", 11, PdfFontStyle.Bold);
                        Rect R2 = new Rect(10, 150, 450, 130);
                        //pdf.DrawString(atestname, R2f, Colors.Purple, R2, fmt);   
                        pdf.DrawString(atestname, R2f, Color.FromArgb(0xff, 97, 48, 144), R2, fmt);


                        //// Commented For Sample No And Collection Time 23 3 17

                        //string sa1 = "Sample No                  " + "\n";
                        //var asampleNo = string.Format("{0}", sa1);

                        //var R3f = new Font("Verdana", 9, PdfFontStyle.Bold);
                        //Rect R3 = new Rect(400, 220, 450, 130);
                        //pdf.DrawString(asampleNo, R3f, Colors.Black, R3, fmt);

                        //var adatasampleNo = string.Format("{0}", sampleNo);
                        //var R4f = new Font("Verdana", 9, PdfFontStyle.Bold);
                        //Rect R4 = new Rect(475, 220, 450, 130);
                        //pdf.DrawString(adatasampleNo, R4f, Colors.Black, R4, fmt);


                        //string col1 = "Collection Time      " + "\n";
                        //var acol1 = string.Format("{0}", col1);

                        //var R5f = new Font("Verdana", 9, PdfFontStyle.Bold);
                        //Rect R5 = new Rect(400, 230, 450, 130);
                        //pdf.DrawString(acol1, R5f, Colors.Black, R5, fmt);

                        //var adataColTime = string.Format("{0}", sampleCollectionTime.ToString());
                        //var R6f = new Font("Arial", 10, PdfFontStyle.Regular);
                        //Rect R6 = new Rect(475, 230, 450, 130);
                        //pdf.DrawString(adataColTime, R6f, Colors.Black, R6, fmt);

                        ////END For Sample No And Collection Time

                        //Footer
                        //Rect Frect1 = new Rect(10, 750, 500, 50);
                        //pdf.DrawString(f, font, Colors.Black, Frect1, ftr);  

                    }
                    else
                    {

                        header = string.Format("{0}", h1 + h2 + h3);
                        var a = string.Format("{0}", header);

                        headerdata = string.Format("{0}", d1 + d2 + d3);
                        var adata = string.Format("{0}", headerdata);

                        header1 = string.Format("{0}", h8 + h9 + h10);
                        var a1 = string.Format("{0}", header1);

                        headerdata1 = string.Format("{0}", d8 + d9 + d10);
                        var adata1 = string.Format("{0}", headerdata1);

                        int X1 = 10;
                        int Y1 = 130;//91 to 150
                        int X2 = 570;
                        int Y2 = 130;
                        pdf.DrawLine(pen, X1, Y1, X2, Y2);
                        ////////////////////////////////////////////For Unit details/////////////////
                        int ux = 290;
                        //int y = 50;
                        int uy = 35;//93
                        // int y = 141;
                        int uwidth = 250;
                        int uheight = 100;
                        Rect uHrect1 = new Rect(ux, uy, uwidth, uheight);
                        pdf.DrawString(unitData, Rpfontdata, Colors.Black, uHrect1, fmt);
                        ///////////////////////////////////////END////////////////////////////////

                        int x = 10;
                        //int y = 55;
                        int y = 135;
                        int width = 450;
                        int height = 70;
                        Rect Hrect1 = new Rect(x, y, width, height);
                        pdf.DrawString(a, Rpfont, Colors.Black, Hrect1, fmt);
                        //pdf.DrawRectangle(pen, Hrect1);

                        //Rect Hrect1data = new Rect(110, 55, 450, 80);
                        Rect Hrect1data = new Rect(110, 135, 450, 70);
                        pdf.DrawString(adata, Rpfontdata, Colors.Black, Hrect1data, fmt);


                        //Rect Hrect2 = new Rect(310, 55, 450, 80);
                        Rect Hrect2 = new Rect(310, 135, 450, 70);
                        pdf.DrawString(a1, Rpfont, Colors.Black, Hrect2, fmt);

                        //Rect Hrect2data = new Rect(400, 55, 450, 80);
                        Rect Hrect2data = new Rect(400, 135, 450, 70);
                        pdf.DrawString(adata1, Rpfontdata, Colors.Black, Hrect2data, fmt);

                        //pdf.DrawRectangle(pen, Hrect2);


                        int XX1 = 10;
                        int YY1 = 180;
                        int XX2 = 570;
                        int YY2 = 180;
                        pdf.DrawLine(pen, XX1, YY1, XX2, YY2);
                        // Added By New
                        //string Space1 = string.Format("{0}", "" + "" + "");
                        //var abc = string.Format("{0}", Space1);

                        //string Space2 = string.Format("{0}", d1 + d2 + "");
                        //var abcd = string.Format("{0}", Space2);
                        ////Rect Hrect2 = new Rect(310, 55, 450, 80);
                        //Rect SpaceRect1 = new Rect(310, 155, 450, 80);
                        //pdf.DrawString(Space1, Rpfont, Colors.Black, Hrect2, fmt);

                        ////Rect Hrect2data = new Rect(400, 55, 450, 80);
                        //Rect SpaceRect2 = new Rect(400, 155, 450, 80);
                        //pdf.DrawString(Space2, Rpfontdata, Colors.Black, Hrect2data, fmt);

                         //Pen pen1 = new Pen(Colors.Red, 2.0f);
                         //int XX11 = 10;
                         //int YY11 = 220;
                         //int XX21 = 570;
                         //int YY21 = 220;
                         //pdf.DrawLine(pen1, XX11, YY11, XX21, YY21);

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

                }




                pdf.CurrentPage = pdf.Pages.Count - 1;
                String appPath;

                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                appPath = "RadioReport" + UnitID + "_" + PrintID + ".pdf";

                Stream FileStream = new MemoryStream();
                MemoryStream MemStrem = new MemoryStream();

                pdf.Save(MemStrem);
                FileStream.CopyTo(MemStrem);

                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                client.UploadReportFileForRadiologyCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        // WaitNew.Close();
                        //ViewPDFReport(appPath);
                    }
                };
                client.UploadReportFileForRadiologyAsync(appPath, MemStrem.ToArray());
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
            ////////OLD commented 22 3 17/////////////
           // PrintReport(ResultId, ISFinalize, strPatInfo.ToString());
            //////////END/////////////////

            string sPath = "RadioReport" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId + "_" + ResultId + ".pdf";
            ViewPDFReport(sPath);

        }
        private void ViewPDFReport(string FileName)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../PatientRadTestReportDocuments");
            string fileName1 = address.ToString();
            fileName1 = fileName1 + "/" + FileName;
            // HtmlPage.Window.Invoke("Open", fileName1);
            HtmlPage.Window.Invoke("open", new object[] { fileName1, "", "" });
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
            // this.Close();
            this.DialogResult = false;

        }

    }
}

