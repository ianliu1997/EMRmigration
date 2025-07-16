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
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.IO;
using PalashDynamics.ValueObjects;

namespace DataDrivenApplication
{
    public partial class FormEditor : ChildWindow
    {
        public FormEditor()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FormEditor_Loaded);
        }

        void FormEditor_Loaded(object sender, RoutedEventArgs e)
        {
            //added by rohini dated 15.1.2016
            FillTempType();

            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.GetFileListCompleted += (s, args) =>
            {
                if (args.Error == null)
                {
                    lstProtocol.ItemsSource = (System.Collections.ObjectModel.ObservableCollection<string>)args.Result;
                    lstFlowChart.ItemsSource = (System.Collections.ObjectModel.ObservableCollection<string>)args.Result;                    
                }                
            };
            client.GetFileListAsync();

            if (this.DataContext != null)
            {
                FillTempType();
                tbTitle.Text = string.IsNullOrEmpty(((FormDetail)this.DataContext).Title) ? "" : ((FormDetail)this.DataContext).Title;
                tbDescription.Text = string.IsNullOrEmpty(((FormDetail)this.DataContext).Description) ? "" : ((FormDetail)this.DataContext).Description;
                tbProtocol.Text = string.IsNullOrEmpty(((FormDetail)this.DataContext).ProtocolUrl) ? "" : ((FormDetail)this.DataContext).ProtocolUrl;
                tbFlowChart.Text = string.IsNullOrEmpty(((FormDetail)this.DataContext).FlowChartUrl) ? "" : ((FormDetail)this.DataContext).FlowChartUrl;
                if (((FormDetail)this.DataContext).IsPhysicalExam)
                {
                    optYes.IsChecked = true;
                }
                else
                {
                    optNo.IsChecked = true;
                }
                if (((FormDetail)this.DataContext).IsForOT)
                {
                    optYesOT.IsChecked = true;
                }
                else
                {
                    optNoOT.IsChecked = true;
                }

                //ADDED BY ROHINI DATED 15.1.2016               
                //cmbTempType.SelectedItem = string.IsNullOrEmpty(((FormDetail)this.DataContext).TemplateType) ? "" : ((FormDetail)this.DataContext).TemplateType;
                if (((FormDetail)this.DataContext).TemplateTypeID != 0)
                    ((MasterListItem)cmbTempType.SelectedItem).ID = ((FormDetail)this.DataContext).TemplateTypeID;
                //cmbTempSubtype.SelectedItem = string.IsNullOrEmpty(((FormDetail)this.DataContext).TemplateSubtype) ? "" : ((FormDetail)this.DataContext).TemplateSubtype;
                if (((FormDetail)this.DataContext).TemplateSubtypeID != 0)
                    ((MasterListItem)cmbTempSubtype.SelectedItem).ID = ((FormDetail)this.DataContext).TemplateSubtypeID;
                
              

            }
            else
            {
                this.DataContext = new FormDetail();
            }
          

        }
        # region added by rohini 15.1.2016
            private void FillTempType()
            {
                List<MasterListItem> list1 = new List<MasterListItem>();
                MasterListItem newItem = new MasterListItem();
                newItem = new MasterListItem(1, "EMR");
                list1.Add(newItem);
                newItem=new MasterListItem(2,"Reporting");
                list1.Add(newItem);
                cmbTempType.ItemsSource = null;
                cmbTempType.ItemsSource = list1;
                //cmbTempType.SelectedValue = (long)1;

                if (((FormDetail)this.DataContext) != null)
                {
                    if (((FormDetail)this.DataContext).TemplateTypeID != 0)
                    {
                        foreach (MasterListItem item in list1)
                        {
                            if (item.ID == ((FormDetail)this.DataContext).TemplateTypeID)
                            {
                                cmbTempType.SelectedItem = item;
                            }
                        }
                    }
                }
                else
                {
                    cmbTempType.SelectedItem = list1[1];
                }
               
               

            }
            private void cmbTempType_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {              

                if (cmbTempType.SelectedItem != null || cmbTempType.SelectedValue != null)
                {
                    if (((MasterListItem)cmbTempType.SelectedItem).ID == 2)
                    {
                        //List<MasterListItem> objList = new List<MasterListItem>();
                        //MasterListItem objM = new MasterListItem(0, "-- Select --");
                        //objList.Add(objM);
                        //cmbTempSubtype.ItemsSource = objList;
                        //cmbTempSubtype.SelectedItem = objM;
                        FillTempSubType();
                    }
                    else
                    {
                        //FillTempSubType();
                        List<MasterListItem> objList = new List<MasterListItem>();
                        MasterListItem objM = new MasterListItem(0, "-- Select --");
                        objList.Add(objM);
                        cmbTempSubtype.ItemsSource = objList;
                        cmbTempSubtype.SelectedItem = objM;
                    }
               }


            }

            private void FillTempSubType()
            {
                List<MasterListItem> list1 = new List<MasterListItem>();
                MasterListItem newItem = new MasterListItem();
                newItem = new MasterListItem(0, "-- Select --");
                list1.Add(newItem);
                newItem = new MasterListItem(1, "Procedure");
                list1.Add(newItem);
                newItem = new MasterListItem(2, "Pathology");
                list1.Add(newItem);
                newItem = new MasterListItem(2, "Radiology");
                list1.Add(newItem);
                cmbTempSubtype.ItemsSource = list1;
                //cmbTempSubtype.SelectedValue = (long)1; 
                if (((FormDetail)this.DataContext) != null)
                {
                    if (((FormDetail)this.DataContext).TemplateSubtypeID != 0)
                    {
                        foreach (MasterListItem item in list1)
                        {
                            if (item.ID == ((FormDetail)this.DataContext).TemplateSubtypeID)
                            {
                                cmbTempSubtype.SelectedItem = item;
                            }
                        }
                    }
                }
                else
                {
                    cmbTempSubtype.SelectedItem = list1[0];
                }
              
            }
      #endregion 
        public event RoutedEventHandler OnOkButtonClick;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnCreateForm_Click(object sender, RoutedEventArgs e)
        {

        }
        string msgTitle = "";
        string msgText = "";
        public Boolean Validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(tbTitle.Text.Trim()))
            {
                //tbDescription.SetValidation("Please Enter Title");
                //tbDescription.RaiseValidationError();
                //tbDescription.Focus();
                //result = false;

                msgText = "Please Enter Template Title";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                result = false;

            }
            else
            {
                //tbDescription.ClearValidationError();
                result = true;
            }
            //if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            //{
            //    txtDescription.SetValidation("Please Enter Description");
            //    txtDescription.RaiseValidationError();
            //    txtDescription.Focus();
            //    result = false;
            //}
            //else
            //{
            //    txtDescription.ClearValidationError();

            //}
            return result;
        }
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                ((FormDetail)this.DataContext).Title = tbTitle.Text;
                ((FormDetail)this.DataContext).Description = tbDescription.Text;
                ((FormDetail)this.DataContext).ProtocolUrl = tbProtocol.Text;
                ((FormDetail)this.DataContext).FlowChartUrl = tbFlowChart.Text;
                //ADDED BY ROHINI DAYED 15.1.2016
                if ((MasterListItem)cmbTempType.SelectedItem != null)
                {
                    ((FormDetail)this.DataContext).TemplateTypeID = ((MasterListItem)cmbTempType.SelectedItem).ID;
                    ((FormDetail)this.DataContext).TemplateType = ((MasterListItem)cmbTempType.SelectedItem).Description;
                }
                if ((MasterListItem)cmbTempSubtype.SelectedItem != null)
                {
                    ((FormDetail)this.DataContext).TemplateSubtypeID = ((MasterListItem)cmbTempSubtype.SelectedItem).ID;
                    ((FormDetail)this.DataContext).TemplateSubtype = ((MasterListItem)cmbTempSubtype.SelectedItem).Description;
                }
                //
                #region for physical exam
                if (optYes.IsChecked == true)
                    ((FormDetail)this.DataContext).IsPhysicalExam = true;
                else
                    ((FormDetail)this.DataContext).IsPhysicalExam = false;
                #endregion

                #region for OT
                if (optYesOT.IsChecked == true)
                    ((FormDetail)this.DataContext).IsForOT = true;
                else
                    ((FormDetail)this.DataContext).IsForOT = false;
                #endregion

                #region Added by Saily P for including Applicable to
                if (optBoth.IsChecked == true)
                    ((FormDetail)this.DataContext).ApplicableTo = (int)EMR_Template_Applicable_Criteria.Both;
                else if (optFemale.IsChecked == true)
                    ((FormDetail)this.DataContext).ApplicableTo = (int)EMR_Template_Applicable_Criteria.Female;
                else if (optMale.IsChecked == true)
                    ((FormDetail)this.DataContext).ApplicableTo = (int)EMR_Template_Applicable_Criteria.Male;
                else if (optNotApplicable.IsChecked == true)
                    ((FormDetail)this.DataContext).ApplicableTo = (int)EMR_Template_Applicable_Criteria.NotApplicable;

                #endregion

                if (OnOkButtonClick != null)
                {
                    OnOkButtonClick(((FormDetail)this.DataContext), e);
                }
                this.DialogResult = true;
            }
        }

        private void lstProtocol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstProtocol.SelectedItem != null)
                tbProtocol.Text = (string)lstProtocol.SelectedItem;
        }

        private void lstFlowChart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFlowChart.SelectedItem != null)
                tbFlowChart.Text = (string)lstFlowChart.SelectedItem;
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    bool exist = lstProtocol.Items.Any(i => i.ToString() == openDialog.File.Name);
                    bool flag = true;
                    if (exist == true)
                    {
                        if (MessageBox.Show("File is already exist. Do you want to overwrite it?", "File Alreadt Exist", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            flag = true;
                        else
                            flag = false;
                    }
                    if (flag == true)
                    {
                        using (Stream stream = openDialog.File.OpenRead())
                        {
                            // Don't allow really big files (more than 5 MB).
                            if (true)
                            {
                                byte[] data = new byte[stream.Length];
                                stream.Read(data, 0, (int)stream.Length);

                                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                                client.UploadFileCompleted += (s, args) =>
                                {
                                    if (args.Error == null)
                                    {
                                        //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                                        DataTemplateHttpsServiceClient client1 = new DataTemplateHttpsServiceClient("CustomBinding_DataTemplateService", address.AbsoluteUri);
                                        client1.GetFileListCompleted += (s1, args1) =>
                                        {
                                            if (args1.Error == null)
                                            {
                                                lstProtocol.ItemsSource = (System.Collections.ObjectModel.ObservableCollection<string>)args1.Result;
                                                lstFlowChart.ItemsSource = (System.Collections.ObjectModel.ObservableCollection<string>)args1.Result;
                                            }
                                        };

                                        client1.GetFileListAsync();
                                    }
                                };

                                client.UploadFileAsync(openDialog.File.Name, data);
                            }
                            else
                            {
                                MessageBox.Show("File must be less than 5 MB");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while reading file.");
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void optNotApplicable_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void optMale_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void optBoth_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void optFemale_Checked(object sender, RoutedEventArgs e)
        {

        }

        
    }
}

