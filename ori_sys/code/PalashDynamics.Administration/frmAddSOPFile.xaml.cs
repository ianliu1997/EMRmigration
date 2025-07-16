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
using System.Windows.Browser;
using System.Windows.Media.Imaging;
using System.IO;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
//using PalashDynamics.Service.PalashServiceReferance;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Administration.Menu;
using PalashDynamics.ValueObjects.Administration;
using DataDrivenApplication;
using C1.Silverlight.Data;
using System.Reflection;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using C1.Silverlight.Pdf;
 

namespace PalashDynamics.Administration
{
    public partial class frmAddSOPFile : UserControl
    {
        private string filePath;
        
        byte[] data;
        FileInfo fi;
        public string msgTitle;
        public string msgText;
        public frmAddSOPFile()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        FillDesignation();
          //  LoadMenuList();
         
        }

        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser; //new clsUserVO();

        //   F:\Devidas\Live Project\manipal\PalashDynamics.Web\Reports\OperationTheatre\rptOTSchedulesDetails.rpt
        private void FillDesignation()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_MenuMaster;
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

                        cmbForm.ItemsSource = null;
                        cmbForm.ItemsSource = objList;
                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {
                            //for selecting unitid according to user login unit id
                            //for selecting unitid according to user login unit id
                            var res = from r in objList
                                      where r.ID == User.UserLoginInfo.UnitId
                                      select r;
                            ((MasterListItem)res.First()).Status = true;
                            cmbForm.SelectedItem = ((MasterListItem)res.First());
                            cmbForm.IsEnabled = false;
                        }
                        else
                            cmbForm.SelectedItem = objList[0];


                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

      
            //clsGetMenuGeneralDetailsBizActionVO objGetMenu = new clsGetMenuGeneralDetailsBizActionVO();
            ////client = new PalashServiceClient();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            ////client.ProcessCompleted += new EventHandler<ProcessCompletedEventArgs>(client_GetMenuGeneralDetailsProcessCompleted);
            //client.ProcessCompleted += new EventHandler<ProcessCompletedEventArgs>(client_GetMenuGeneralDetailsProcessCompleted);

            //client.ProcessAsync(objGetMenu, User); //user);
            //client.CloseAsync();

        
 
       
       

        private void cmdBrowse_Checked(object sender, RoutedEventArgs e)
        {

           OpenFileDialog openDialog = new OpenFileDialog();
              String appPath;
             long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            var pdf = new C1PdfDocument(PaperKind.A4);

            if (openDialog.ShowDialog() == true)
            {
                txtFile.Text = openDialog.File.Name;
                try
                {
                    appPath = "SOP_" + UnitID + "_" + DateTime.Today.ToString("dd-MMM-yyyy")+ txtFile.Text ;

                    Stream FileStream = new MemoryStream();
                    MemoryStream MemStrem = new MemoryStream();

                    pdf.Save(MemStrem);
                    FileStream.CopyTo(MemStrem);
                    //UploadSOPFile
                    
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    client.UploadSOPFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                           // WaitNew.Close();
                          // ViewPDFReport(appPath);
                        }
                    };
                    client.UploadSOPFileAsync(appPath, MemStrem.ToArray());
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


         

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
           // ((clsPatientVO)this.DataContext).SpouseDetails.Photo = data;
        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }
    }
}
