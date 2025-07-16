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
//using PalashDynamics.Service.DataTemplateServiceRef;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.EMR;
using MessageBoxControl;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Service.DataTemplateServiceRef1;

namespace DataDrivenApplication
{
    public partial class ManagePatientCaseRecord : ChildWindow
    {
        public FormDetail Form { get; set; }
        public PatientCaseRecordRelation PCR_Relation { get; set; }
        public PatientCaseReferralRelation CaseReferral_Relation { get; set; }
        public ManagePatientCaseRecord(FormDetail Form)
        {
            if (Form == null)
            {
                throw new ArgumentNullException();
            }
            InitializeComponent();

            this.Form = Form;
            this.PCR_Relation = null;
            this.CaseReferral_Relation = null;
            this.Loaded += new RoutedEventHandler(ManagePatientCaseRecord_Loaded);
        }

        void ManagePatientCaseRecord_Loaded(object sender, RoutedEventArgs e)
        {
            if ((string)this.Tag == "PCR")
            {
                this.Title = "Manage Patient Case Record";
                this.FormTitle.Text = "Patient Case Record Details";
                dgTemplateList.ItemsSource = Form.PCRRelations;
            }
            else if ((string)this.Tag == "CaseReferral")
            {
                this.Title = "Manage Patient Case Referral";
                this.FormTitle.Text = "Patient Case Referral Details";
                dgTemplateList.ItemsSource = Form.CaseReferralRelations;
            }
        }

        public event RoutedEventHandler OnOkButtonClick;
        public event RoutedEventHandler OnDeleteRelClick;

        private void PreviewHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

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

        private void DeleteRelation_Click(object sender, RoutedEventArgs e)
        {
            if (dgTemplateList.SelectedItem != null)
            {
                if (OnDeleteRelClick != null)
                {
                    if ((string)this.Tag == "PCR")
                        OnDeleteRelClick(this.PCR_Relation, e);
                    else if ((string)this.Tag == "CaseReferral")
                        OnDeleteRelClick(this.CaseReferral_Relation, e);
                }
                this.DialogResult = true;
            }
        }

        private void AddRelation_Click(object sender, RoutedEventArgs e)
        {
            ListView.Visibility = Visibility.Collapsed;
            FormView.Visibility = Visibility.Visible;

            if ((string)this.Tag == "PCR")
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_EMR_PCR_SectionMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";

                BizAction.MasterList = new List<MasterListItem>();
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client1.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        cmbSourceSection.ItemsSource = null;
                        cmbSourceSection.ItemsSource = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                    }
                };
                client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client1.CloseAsync();
            }
            else if ((string)this.Tag == "CaseReferral")
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_EMR_REFERRAL_SectionMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";

                BizAction.MasterList = new List<MasterListItem>();
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client1.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        cmbSourceSection.ItemsSource = null;
                        cmbSourceSection.ItemsSource = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                    }
                };
                client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client1.CloseAsync();
            }

            //cmbSourceSection.ItemsSource = Form.SectionList;
            cmbTargetSection.ItemsSource = Form.SectionList;

        }

        private void CancelItemButton_Click(object sender, RoutedEventArgs e)
        {
            ListView.Visibility = Visibility.Visible;
            FormView.Visibility = Visibility.Collapsed;
        }

        private void SaveItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)this.Tag == "PCR")
                {
                    PatientCaseRecordRelation relation = new PatientCaseRecordRelation();
                    if (((MasterListItem)cmbSourceField.SelectedItem).ID == 13 && ((((FieldDetail)cmbTargetField.SelectedItem).DataType).Id != 9 && (((FieldDetail)cmbTargetField.SelectedItem).DataType).Id != 14))
                    {
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palash", "Only Medication field can be mapped with Medication. \nPlease select a medication field.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbx.Show();
                    }
                    else
                    {
                        relation.SourceSection = ((MasterListItem)cmbSourceSection.SelectedItem).Description;
                        relation.SourceField = (MasterListItem)cmbSourceField.SelectedItem;
                        relation.TargetSection = ((SectionDetail)cmbTargetSection.SelectedItem).Title;
                        relation.TargetField = ((FieldDetail)cmbTargetField.SelectedItem);
                        relation.TargetSectionId = ((SectionDetail)cmbTargetSection.SelectedItem).UniqueId.ToString();
                        relation.TargetFieldId = ((FieldDetail)cmbTargetField.SelectedItem).UniqueId.ToString();

                        if (OnOkButtonClick != null)
                        {
                            OnOkButtonClick(relation, e);
                        }
                    }
                }
                else if ((string)this.Tag == "CaseReferral")
                {
                    PatientCaseReferralRelation relation = new PatientCaseReferralRelation();

                    relation.SourceSection = ((MasterListItem)cmbSourceSection.SelectedItem).Description;
                    relation.SourceField = (MasterListItem)cmbSourceField.SelectedItem;
                    relation.TargetSection = ((SectionDetail)cmbTargetSection.SelectedItem).Title;
                    relation.TargetField = ((FieldDetail)cmbTargetField.SelectedItem);
                    relation.TargetSectionId = ((SectionDetail)cmbTargetSection.SelectedItem).UniqueId.ToString();
                    relation.TargetFieldId = ((FieldDetail)cmbTargetField.SelectedItem).UniqueId.ToString();

                    if (OnOkButtonClick != null)
                    {
                        OnOkButtonClick(relation, e);
                    }
                }

                this.DialogResult = true;
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void cmbSourceSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbSourceField.ItemsSource = null;
            if (cmbSourceSection.SelectedItem != null)
            {
                if ((string)this.Tag == "PCR")
                {
                    clsGetEMR_PCR_FieldListBizActionVO BizAction = new clsGetEMR_PCR_FieldListBizActionVO();
                    BizAction.SectionID = ((MasterListItem)cmbSourceSection.SelectedItem).ID;
                    BizAction.Status = true;
                    BizAction.PCR_FieldMasterList = new List<MasterListItem>();
                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    client1.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            cmbSourceField.ItemsSource = null;
                            cmbSourceField.ItemsSource = ((clsGetEMR_PCR_FieldListBizActionVO)args.Result).PCR_FieldMasterList;
                        }
                    };
                    client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client1.CloseAsync();
                }
                else if ((string)this.Tag == "CaseReferral")
                {
                    clsGetEMR_CaseReferral_FieldListBizActionVO BizAction = new clsGetEMR_CaseReferral_FieldListBizActionVO();
                    BizAction.SectionID = ((MasterListItem)cmbSourceSection.SelectedItem).ID;
                    BizAction.Status = true;
                    BizAction.CaseReferral_FieldMasterList = new List<MasterListItem>();
                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    client1.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            cmbSourceField.ItemsSource = null;
                            cmbSourceField.ItemsSource = ((clsGetEMR_CaseReferral_FieldListBizActionVO)args.Result).CaseReferral_FieldMasterList;
                        }
                    };
                    client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client1.CloseAsync();
                }
            }
        }


        private void cmbTargetSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbTargetField.ItemsSource = null;
            if (cmbTargetSection.SelectedItem != null)
            {
                cmbTargetField.ItemsSource = ((SectionDetail)cmbTargetSection.SelectedItem).FieldList;
            }
        }

        private void dgTemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((string)this.Tag == "PCR")
                    this.PCR_Relation = (PatientCaseRecordRelation)dgTemplateList.SelectedItem;
                else if ((string)this.Tag == "CaseReferral")
                    this.CaseReferral_Relation = (PatientCaseReferralRelation)dgTemplateList.SelectedItem;
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

