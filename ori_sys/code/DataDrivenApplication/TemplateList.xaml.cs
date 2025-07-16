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
using DataDrivenApplication;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Windows.Printing;
using CIMS.Forms;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.EMR;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.Administration;

namespace DataDrivenApplication
{
    public partial class TemplateList : UserControl
    {

        public TemplateList()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(TemplateList_Loaded);
            //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            //client.GetTemplatesListCompleted += new EventHandler<GetTemplatesListCompletedEventArgs>(client_GetTemplatesListCompleted);
            //client.GetTemplatesListAsync();

            clsGetEMRTemplateListBizActionVO BizAction = new clsGetEMRTemplateListBizActionVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += new EventHandler<ProcessCompletedEventArgs>(client_ProcessCompleted);
            client.ProcessAsync(BizAction, new clsUserVO());
        }

        void client_ProcessCompleted(object sender, ProcessCompletedEventArgs e)
        {
            dgTemplateList.ItemsSource = ((clsGetEMRTemplateListBizActionVO)e.Result).objEMRTemplateList;
        }

        void client_GetTemplatesListCompleted(object sender, GetTemplatesListCompletedEventArgs e)
        {
            // Used with DataTemplateService
            // dgTemplateList.ItemsSource = (ObservableCollection<EMRTemplate>)e.Result;//App.FormTemplateList;

        }



        void TemplateList_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreviewHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgTemplateList.SelectedItem != null)
            {
                App.SelectedTemplate = (clsEMRTemplateVO)dgTemplateList.SelectedItem;
                
                // Used with
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                //client.GetTemplatesDetailsByTemplateTitleCompleted += (s, args) =>
                //{
                //    FormDetail Form = ((EMRTemplate)args.Result).Template.XmlDeserialize<FormDetail>();
                //    ((ContentControl)this.Parent).Content = new FormDesigner(Form);
                //    //App.MainWindow.MainRegion.Content = new FormDesigner(Form);
                //};
                //client.GetTemplatesDetailsByTemplateTitleAsync(((EMRTemplate)dgTemplateList.SelectedItem).TemplateId);

                clsGetEMRTemplateBizActionVO BizAction = new clsGetEMRTemplateBizActionVO();
                BizAction.objEMRTemplate.TemplateID = ((clsEMRTemplateVO)dgTemplateList.SelectedItem).TemplateID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    FormDetail Form = ((clsGetEMRTemplateBizActionVO)args.Result).objEMRTemplate.Template.XmlDeserialize<FormDetail>();
                    ((ContentControl)this.Parent).Content = new FormDesigner(Form);
                    //App.MainWindow.MainRegion.Content = new FormDesigner(Form);
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            FormEditor editor = new FormEditor();
            editor.OnOkButtonClick += new RoutedEventHandler(editor_OnOkButtonClick);
            editor.Show();
        }

        void editor_OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is FormDetail)
            {
                App.SelectedFormIndex = -1;
                App.SelectedTemplate = null;
                #region Comment by harish
                //App.MainWindow.MainRegion.Content = new FormDesigner((FormDetail)sender);
                #endregion
                if (((FormDetail)sender).Title != "" || ((FormDetail)sender).Title != null)
                {
                    // Used With DataTemplateService
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    //client.SaveTemplateCompleted += (s, args) =>
                    //{
                    //    App.MainWindow.MainRegion.Content = new TemplateList();
                    //};
                    //client.SaveTemplateAsync(t);


                    clsAddEMRTemplateBizActionVO BizAction = new clsAddEMRTemplateBizActionVO();

                    clsEMRTemplateVO t = new clsEMRTemplateVO();
                    t.Title = ((FormDetail)sender).Title;
                    t.Description = ((FormDetail)sender).Description;
                    t.Template = ((FormDetail)sender).XmlSerilze();
                    t.ApplicableCriteria = ((FormDetail)sender).ApplicableTo;
                    t.IsPhysicalExam = ((FormDetail)sender).IsPhysicalExam;
                    t.IsForOT = ((FormDetail)sender).IsForOT;

                    //BY ROHINI DATED 15.1.2016
                    t.TemplateTypeID = ((FormDetail)sender).TemplateTypeID;
                    t.TemplateType = ((FormDetail)sender).TemplateType;
                    t.TemplateSubtypeID = ((FormDetail)sender).TemplateSubtypeID;
                    t.TemplateSubtype = ((FormDetail)sender).TemplateSubtype;
                    //-----

                    BizAction.EMRTemplateDetails = t;

                    //User Related Values specified in DAL
                    BizAction.EMRTemplateDetails.Status = true;                    
                                        
                   
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        ((ContentControl)this.Parent).Content = new TemplateList();
                    };

                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                }

            }
        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.SelectedTemplate != null)
            {
                DeletionWindow dw = new DeletionWindow();
                dw.Message = "Are you sure you want to remove the Form (Template) '" + ((clsEMRTemplateVO)App.SelectedTemplate).Title + "' ?";
                dw.ID = 3;
                dw.FormTitle.Text = "Delete Form/Template";
                dw.OnOkButtonClick += new RoutedEventHandler(dw_OnOkButtonClick);
                dw.Show();
            }
        }

        void dw_OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            DeletionWindow dw = (DeletionWindow)sender;
            if (dw.ID == 3)
            {
                // Used with DataTemplateService
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                //client.DeleteTemplateCompleted += (s, args) =>
                //{
                //    App.MainWindow.MainRegion.Content = new TemplateList();
                //};
                //client.DeleteTemplateAsync(App.SelectedTemplate);

                clsUpdateEMRTemplateBizActionVO BizAction = new clsUpdateEMRTemplateBizActionVO();
                BizAction.EMRTemplateDetails = App.SelectedTemplate;
                BizAction.EMRTemplateDetails.Status = false;

                //User Related Values specified in DAL

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    ((ContentControl)this.Parent).Content = new TemplateList();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
        }

        private void dgTemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.SelectedFormIndex = dgTemplateList.SelectedIndex;
            App.SelectedTemplate = (clsEMRTemplateVO)dgTemplateList.SelectedItem;



            #region Added Tempararily
            String s = "";
            #endregion
        }

        private void DemoItemButton_Click(object sender, RoutedEventArgs e)
        {
            // Used with DataTemplateService and stand-alone Protocol Application
            App.MainWindow.MainRegion.Content = new TemplateAssignment();
        }

        private void VarianceItemButton_Click(object sender, RoutedEventArgs e)
        {
            // Used with DataTemplateService
            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.GetVarinceListCompleted += new EventHandler<GetVarinceListCompletedEventArgs>(client_GetVarinceListCompleted);
            client.GetVarinceListAsync();

            dgTemplateList.Visibility = Visibility.Collapsed;
            AddItemButton.Visibility = Visibility.Collapsed;
            DeleteItemButton.Visibility = Visibility.Collapsed;
            DemoItemButton.Visibility = Visibility.Collapsed;

            dgVarianceList.Visibility = Visibility.Visible;
            PrintVarianceItemButton.Visibility = Visibility.Visible;
            BackItemButton.Visibility = Visibility.Visible;
        }

        private void dgVarianceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void client_GetVarinceListCompleted(object sender, GetVarinceListCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            //Used With DataTemplate Service
            dgVarianceList.ItemsSource = (ObservableCollection<Variance>)e.Result;//App.FormTemplateList;
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        //used with DataTemplate Service
        IEnumerator<Variance> IEV;
        List<Variance> lstV;
        private void PrintVarianceItemButton_Click(object sender, RoutedEventArgs e)
        {
            //Used with DataTemplate Service
            IEV = (IEnumerator<Variance>)dgVarianceList.ItemsSource.GetEnumerator();
            lstV = new List<Variance>();
            PrintDocument document = new PrintDocument();

            document.PrintPage += new EventHandler<PrintPageEventArgs>(document_PrintPage);
            document.Print("Template");
        }


        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            //Used With DataTemplate Service
            PrintVarianceMonitoring PVM = new PrintVarianceMonitoring();

            while (IEV.MoveNext())
            {
                lstV.Add((Variance)IEV.Current);
                PVM.SetGridValue(lstV);
                PVM.Measure(new Size(e.PrintableArea.Width, e.PrintableArea.Height));
                //if (PVM.DesiredSize.Height > e.PrintableArea.Height)
                if (lstV.Count > 11)
                {
                    lstV.Clear();
                    e.HasMorePages = true;
                    break;
                }
            }

            e.PageVisual = PVM;
        }

        private void BackItemButton_Click(object sender, RoutedEventArgs e)
        {
            //Used With DataTemplate Service
            dgTemplateList.Visibility = Visibility.Visible;
            AddItemButton.Visibility = Visibility.Visible;
            DeleteItemButton.Visibility = Visibility.Visible;
            DemoItemButton.Visibility = Visibility.Visible;

            dgVarianceList.Visibility = Visibility.Collapsed;
            PrintVarianceItemButton.Visibility = Visibility.Collapsed;
            BackItemButton.Visibility = Visibility.Collapsed;
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            ImageWindow Win = new ImageWindow();
            Win.TemplateID = ((clsEMRTemplateVO)dgTemplateList.SelectedItem).TemplateID;
            Win.Show();
        }
    }
}
