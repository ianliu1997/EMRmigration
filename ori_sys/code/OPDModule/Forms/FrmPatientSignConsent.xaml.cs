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
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using System.Collections.ObjectModel;
using System.IO;
using PalashDynamics.ValueObjects;
using MessageBoxControl;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.Animations;
using System.Reflection;
using System.Windows.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;


namespace OPDModule.Forms
{
    public partial class FrmPatientSignConsent : ChildWindow
    {
        long SpatientID, SConsentID;
        long SPatientUnitID, SConsentUnitID;

        #region Variable Declaration

        byte[] data;
        FileInfo fi;
        public string msgTitle;
        public string msgText;
        clsPatientSignConsentVO SelectedDetails { get; set; }
        ObservableCollection<clsPatientSignConsentVO> lstFile { get; set; }
         
        bool IsNew = false;
        bool IsCancel = true;
        public PagedSortableCollectionView<clsPatientSignConsentVO> MasterList { get; private set; }
        #endregion

        public FrmPatientSignConsent(long PatientID, long PatientUnitID, long ConsentID, long ConsentUnitID)
        {
            InitializeComponent();

            SpatientID = PatientID;
            SPatientUnitID = PatientUnitID;
            SConsentID = ConsentID;
            SConsentUnitID = ConsentUnitID;

            MasterList = new PagedSortableCollectionView<clsPatientSignConsentVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            this.dgDataPager_back.DataContext = MasterList;
            dgReport_back.DataContext = MasterList;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RptRcdDate.SelectedDate = DateTime.Now;
            RptRcdTime.Value = DateTime.Now;
            GetData();
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void GetData ()
        {
              WaitIndicator win = new WaitIndicator();
               win.Show();
               try
               {
                   clsGetPatientSignConsentBizActionVO bizActionVO = new clsGetPatientSignConsentBizActionVO();
                    bizActionVO.sortExpression = ""; //txtSearch.Text;
                    bizActionVO.IsPagingEnabled = true;
                    bizActionVO.MaximumRows = MasterList.PageSize;
                    bizActionVO.StartIndex = MasterList.PageIndex * MasterList.PageSize;

                   bizActionVO.ConsentID =SConsentID;
                   bizActionVO.ConsentUnitID = SConsentUnitID;
                   bizActionVO.Status = true;              

                    bizActionVO.SignPatientList = new List<clsPatientSignConsentVO>();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            bizActionVO.SignPatientList = (((clsGetPatientSignConsentBizActionVO)args.Result).SignPatientList);
                            MasterList.Clear();
                            if (bizActionVO.SignPatientList.Count > 0)
                            {
                                
                                MasterList.TotalItemCount = (int)(((clsGetPatientSignConsentBizActionVO)args.Result).TotalRows);
                                foreach (var item in bizActionVO.SignPatientList)
                                {
                                    MasterList.Add(item);
                                }
                                PagedCollectionView collection = new PagedCollectionView(MasterList);
                                dgDataPager_back.Source = null;
                                dgDataPager_back.Source  = MasterList;
                              
                                dgReport_back.DataContext = collection;
                                win.Close();                               
                            }
                        }                       
                        win.Close();
                    };
                    client.ProcessAsync(bizActionVO, new clsUserVO());
                    client.CloseAsync();
                   }
                   catch (Exception)
                   {
                       throw;
                   }    
        }

        string DocumentName = null;
        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            openDialog.Filter = "Image Files (*.bmp, *.jpg, *.gif)|*.bmp;*.jpg;*.gif;|Pdf Files(*.pdf)|*.pdf; |Text Files (*.txt)|*.txt; |Excel Files (*.xls, *.xlsx)|*.xls; *.xlsx; |Word Files (*.doc, *.docx)|*.doc;*.docx; |All files (*.*)|*.*;";
            openDialog.FilterIndex = 1;
            DocumentName = null;
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
                        DocumentName = txtDocumentName.Text;

                    }
                }
                catch (Exception ex)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error While Reading File.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    
                    msgWindow.Show();
                    throw ex;
                }
            }
        }

        private bool CheckValidations()
        {
            bool result = true;
            try
            {
                if (TxtReportPath.Text == "" || TxtReportPath.Text == null)
                {
                    TxtReportPath.SetValidation("Consent Path is required.");
                    TxtReportPath.RaiseValidationError();
                    TxtReportPath.Focus();
                    result = false;
                }
                else
                    TxtReportPath.ClearValidationError();

                if (txtDocumentName.Text == "" || txtDocumentName.Text == null)
                {
                    txtDocumentName.SetValidation("Consent Name is required.");
                    txtDocumentName.RaiseValidationError();
                    txtDocumentName.Focus();
                    result = false;
                }
                else
                    txtDocumentName.ClearValidationError();

                if (RptRcdTime.Value == null)
                {
                    RptRcdTime.SetValidation("Consent Received Time is required.");
                    RptRcdTime.RaiseValidationError();
                    RptRcdTime.Focus();
                    result = false;
                }
                else
                    RptRcdTime.ClearValidationError();
                
                if (RptRcdDate.SelectedDate == null)
                {
                    RptRcdDate.SetValidation("Consent Received Date is required.");
                    RptRcdDate.RaiseValidationError();
                    RptRcdDate.Focus();
                    result = false;
                }
                else
                    RptRcdDate.ClearValidationError();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void ClearUI()
        {
            txtDocumentName.Text = "";           
            TxtRemarks.Text = "";
            TxtReportPath.Text = "";
            RptRcdDate.SelectedDate = DateTime.Now;
            RptRcdTime.Value = DateTime.Now;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator win = new WaitIndicator();
             win.Show ();
             try
             {
                 bool valid = CheckValidations();
                 if (valid)
                 {
                     clsADDPatientSignConsentBizActionVO BizAction = new clsADDPatientSignConsentBizActionVO();
                     //BizAction.UploadDetails = new List<clsPatientSignConsentVO>();
                     clsPatientSignConsentVO ObjFile = new clsPatientSignConsentVO();
                                         
                     ObjFile.Time = Convert.ToDateTime(RptRcdTime.Value);
                     ObjFile.Date = Convert.ToDateTime(RptRcdDate.SelectedDate);

                     ObjFile.SourceURL = fi.Name;
                     ObjFile.Report = data;
                     ObjFile.Remarks = TxtRemarks.Text;
                     ObjFile.DocumentName = txtDocumentName.Text;
                     ObjFile.Path = TxtReportPath.Text;

                     ObjFile.PatientID = SpatientID;
                     ObjFile.PatientUnitID = SPatientUnitID;
                     ObjFile.ConsentID = SConsentID;
                     ObjFile.ConsentUnitID = SConsentUnitID;
                     BizAction.signConsentDetails = ObjFile;

                     Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                     PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                     client.ProcessCompleted += (s, arg) =>
                     {
                         if (arg.Error == null)
                         {
                             if (arg.Result != null)
                             {
                                 MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Consent Is Uploaded Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                 mgbx.Show();

                                 GetData();
                                 ClearUI();
                                 win.Close();
                             }
                         }
                     };
                     client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                     client.CloseAsync();
                 }
                 else
                 {
                     win.Close();
                     MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Atleast one file is required.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                     mgbx.Show();
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
        }
          
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
      
        private void ViewImage_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsPatientSignConsentVO)dgReport_back.SelectedItem).SourceURL))
            {
                if (((clsPatientSignConsentVO)dgReport_back.SelectedItem).Report != null)
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);


                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsPatientSignConsentVO)dgReport_back.SelectedItem).SourceURL });
                            AttachedFileNameList.Add(((clsPatientSignConsentVO)dgReport_back.SelectedItem).SourceURL);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsPatientSignConsentVO)dgReport_back.SelectedItem).SourceURL, ((clsPatientSignConsentVO)dgReport_back.SelectedItem).Report);
                }
            }
        }

        private void cmdDeleteAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                        clsDeletePatientSignConsentBizActionVO BizAction = new clsDeletePatientSignConsentBizActionVO();

                        clsPatientSignConsentVO objBizAction = new clsPatientSignConsentVO();
                        objBizAction.ID = ((clsPatientSignConsentVO)this.dgReport_back.SelectedItem).ID;
                        objBizAction.UnitID = ((clsPatientSignConsentVO)this.dgReport_back.SelectedItem).UnitID;
                        BizAction.DeleteVO = objBizAction;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        client.ProcessCompleted += (s, args) =>
                        {
                        if (args.Error == null && args.Result != null)
                        {
                        msgText = "Record is successfully Deleted!";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                        GetData();
                        }
                        else
                        {
                        msgText = "Error During Deleting Test !";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();                        
                        }
           
                        };
                        client.ProcessAsync(BizAction, new clsUserVO());
                        client.CloseAsync();
                        }
            catch (Exception)
            {                
                throw;
            }
          
            }       
    }
}

