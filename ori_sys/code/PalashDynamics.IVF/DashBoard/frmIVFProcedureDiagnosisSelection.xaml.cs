using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.EMR;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using System.Text;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamics.Administration;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmIVFProcedureDiagnosisSelection : ChildWindow
    {
        #region Public Variable

        public event RoutedEventHandler OnCancelButtonClick;
        public event RoutedEventHandler OnAddButton_Click;

        public PagedSortableCollectionView<clsEMRDiagnosisVO> DataListDiagnosis { get; private set; }

        public int DataListPageSizeSer
        {
            get
            {
                return DataListDiagnosis.PageSize;
            }
            set
            {
                if (value == DataListDiagnosis.PageSize) return;
                DataListDiagnosis.PageSize = value;
            }
        }

        #endregion

        public frmIVFProcedureDiagnosisSelection()
        {
            InitializeComponent();
            DataListDiagnosis = new PagedSortableCollectionView<clsEMRDiagnosisVO>();
            DataListDiagnosis.OnRefresh += new EventHandler<RefreshEventArgs>(DataListDiagnosis_OnRefresh);
            DataListPageSizeSer = 15;
            this.Title = "Procedure List";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ttlProcedureList");
            this.Loaded += new RoutedEventHandler(frmProcedureDiagnosisSelection_Loaded);
        }

        void frmProcedureDiagnosisSelection_Loaded(object sender, RoutedEventArgs e)
        {
            FillSpecialization();
            FillDiagnosis();
        }
        void DataListDiagnosis_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillDiagnosis();
        }
        private void FillDiagnosis()
        {
            WaitIndicator indicator = new WaitIndicator();
            try
            {
                indicator.Show();
                clsGetRSIJDiagnosisMasterBizactionVO BizAction = new clsGetRSIJDiagnosisMasterBizactionVO();
                BizAction.DiagnosisDetails = new List<clsEMRDiagnosisVO>();
                BizAction.IsICD9 = true;
                if (txtDiagnosisCode.Text != null)
                {
                    BizAction.Code = txtDiagnosisCode.Text;
                }
                if (txtDiagnosisName.Text != null)
                {
                    BizAction.Diagnosis = txtDiagnosisName.Text;
                }
                BizAction.IsPagingEnabled = true;
                BizAction.StartRowIndex = DataListDiagnosis.PageIndex * DataListDiagnosis.PageSize;
                BizAction.MaximumRows = DataListDiagnosis.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        clsGetRSIJDiagnosisMasterBizactionVO result = arg.Result as clsGetRSIJDiagnosisMasterBizactionVO;
                        if (result.DiagnosisDetails != null)
                        {
                            DataListDiagnosis.Clear();
                            DataListDiagnosis.TotalItemCount = ((clsGetRSIJDiagnosisMasterBizactionVO)arg.Result).TotalRows;

                            foreach (clsEMRDiagnosisVO item in result.DiagnosisDetails)
                            {
                                item.SelectDiagnosis = false;
                                DataListDiagnosis.Add(item);
                            }
                            dgDiagnosisList.ItemsSource = null;
                            dgDiagnosisList.ItemsSource = DataListDiagnosis;
                            DataPagerDoc.Source = null;
                            DataPagerDoc.PageSize = BizAction.MaximumRows;
                            DataPagerDoc.Source = DataListDiagnosis;
                        }
                        else
                        {
                            string sMessage = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("NoRecordFound_Msg");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", sMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        string sErrMessage = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", sErrMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    indicator.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            if (DataListDiagnosis == null)
            {
                isValid = false;
            }
            else if (DataListDiagnosis.Count <= 0)
            {
                isValid = false;
            }

            if (isValid)
            {
                this.DialogResult = true;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
            else
            {
                string strMsg = "No Diagnosis Selected for Adding";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick((this.DataContext), e);
            }
            this.DialogResult = false;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            DataPagerDoc.PageIndex = 0;
            FillDiagnosis();
        }

        public ObservableCollection<clsEMRDiagnosisVO> DiagnosisList { get; set; }

        private void chkMultipleDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            bool IsValid = true;
            if (dgDiagnosisList.SelectedItem != null)
            {
                try
                {
                    if (DiagnosisList == null)
                        DiagnosisList = new ObservableCollection<clsEMRDiagnosisVO>();

                    CheckBox chk = (CheckBox)sender;
                    StringBuilder strError = new StringBuilder();

                    long DiagnosisTypeId = 0;

                    if (rdbPrimary.IsChecked == true)
                        DiagnosisTypeId = 1;

                    if (rdbSecondary.IsChecked == true)
                        DiagnosisTypeId = 2;

                    if (chk.IsChecked == true)
                    {
                        if (DiagnosisList.Count > 0)
                        {
                            var item = from r in DiagnosisList
                                       where r.Categori == ((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem).Categori
                                       select new clsEMRDiagnosisVO
                                       {
                                           Code = r.Code,
                                           Diagnosis = r.Diagnosis,
                                           DiagnosisTypeId = DiagnosisTypeId,
                                           ServiceCode = r.ServiceCode,
                                           Class = r.Class,
                                           //IsICOPIMHead = r.IsICOPIMHead
                                       };
                            if (item.ToList().Count > 0)
                            {
                                if (strError.ToString().Length > 0)
                                    strError.Append(",");
                                strError.Append(((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem).Diagnosis);

                                if (!string.IsNullOrEmpty(strError.ToString()))
                                {
                                    string strMsg = "Diagnosis already Selected : " + strError.ToString();

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    ((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem).SelectDiagnosis = false;


                                    IsValid = false;
                                }
                            }
                            else
                            {
                                ((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem).DiagnosisTypeId = DiagnosisTypeId;
                                DiagnosisList.Add((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem);
                            }


                        }
                        else
                        {
                            ((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem).DiagnosisTypeId = DiagnosisTypeId;
                            DiagnosisList.Add((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem);
                        }
                    }
                    else
                        DiagnosisList.Remove((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem);
                }
                catch (Exception)
                {

                }

            }

        }

        private void txtDiagnosisName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataPagerDoc.PageIndex = 0;
                FillDiagnosis();
            }
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }


        private void FillSpecialization()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Specialization;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                        cmbSpecilization.ItemsSource = null;
                        cmbSpecilization.ItemsSource = objList;
                        try
                        {
                            cmbSpecilization.SelectedItem = ((List<MasterListItem>)cmbSpecilization.ItemsSource).First(dd => dd.ID == 12);
                            //if (Pathology)
                            //    cmbSpecilization.SelectedItem = ((List<MasterListItem>)cmbSpecilization.ItemsSource).First(dd => dd.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID);
                            //else if (Radiology)
                            //    cmbSpecilization.SelectedItem = ((List<MasterListItem>)cmbSpecilization.ItemsSource).First(dd => dd.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID);
                            //else
                            //    cmbSpecilization.IsEnabled = true;
                            cmbSpecilization.IsEnabled = false;
                        }
                        catch (Exception)
                        {
                            cmbSpecilization.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
                        }
                    }

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception) { FillDiagnosis(); }

        }

        private void FillSubSpecialization(long iSupId)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
                if (iSupId > 0)
                    BizAction.Parent = new KeyValue { Key = iSupId, Value = "fkSpecializationID" };
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

                        cmbSubSpecilization.ItemsSource = null;
                        cmbSubSpecilization.ItemsSource = objList;

                        if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                            cmbSubSpecilization.SelectedItem = objList[0];
                        else
                            FillDiagnosis();

                        if (cmbSpecilization.SelectedItem != null)
                        {
                            FillDiagnosis();
                        }
                    }
                    else
                    {
                        FillDiagnosis();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                FillDiagnosis();
            }
        }

        private void cmbSpecilization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecilization.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cmbSpecilization.SelectedItem).ID);
            }
        }
    }
}




