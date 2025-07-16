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

namespace EMR
{
    public partial class frmDiagnosisSelection : ChildWindow
    {
        #region Public Variable

        public event RoutedEventHandler OnCancelButtonClick;
        public event RoutedEventHandler OnAddButton_Click;
        public double WindowHeight { get; set; }
        public double WindowWidth { get; set; }
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

        public frmDiagnosisSelection()
        {
            InitializeComponent();
            //this.Title = LocalizationManager.resourceManager.GetString("ttlDiagnosisList");
            this.Title = "Diagnosis List";
            DataListDiagnosis = new PagedSortableCollectionView<clsEMRDiagnosisVO>();
            DataListDiagnosis.OnRefresh += new EventHandler<RefreshEventArgs>(DataListDiagnosis_OnRefresh);
            DataListPageSizeSer = 15;
            this.Loaded += new RoutedEventHandler(FrmDiagnosisSelectionList_Loaded);
        }

        void FrmDiagnosisSelectionList_Loaded(object sender, RoutedEventArgs e)
        {
            FillDiagnosis();
        }

        void DataListDiagnosis_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillDiagnosis();
        }

        private void FillDiagnosis()
        {
            clsGetRSIJDiagnosisMasterBizactionVO BizAction = new clsGetRSIJDiagnosisMasterBizactionVO();
            BizAction.DiagnosisDetails = new List<clsEMRDiagnosisVO>();
            WaitIndicator indicator = new WaitIndicator();
            try
            {
                indicator.Show();
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
                        if (((clsGetRSIJDiagnosisMasterBizactionVO)arg.Result).DiagnosisDetails != null)
                        {
                            clsGetRSIJDiagnosisMasterBizactionVO result = arg.Result as clsGetRSIJDiagnosisMasterBizactionVO;

                            if (result.DiagnosisDetails != null)
                            {
                                DataListDiagnosis.Clear();
                                DataListDiagnosis.TotalItemCount = ((clsGetRSIJDiagnosisMasterBizactionVO)arg.Result).TotalRows;

                                foreach (clsEMRDiagnosisVO item in result.DiagnosisDetails)
                                {
                                    if (DiagnosisList != null && DiagnosisList.Count > 0 ) //Added by AJ Date 25/11/2016 
                                    {
                                        foreach (var Serviceitem in DiagnosisList)
                                        {
                                            if (Serviceitem.ID == item.ID)
                                            {
                                                item.SelectDiagnosis = true;
                                                break;
                                            }
                                        }
                                    }
                                    //***// ------------------
                                   // item.SelectDiagnosis = false;
                                    DataListDiagnosis.Add(item);
                                }

                            }
                            dgDiagnosisList.ItemsSource = null;
                            dgDiagnosisList.ItemsSource = DataListDiagnosis;
                            DataPagerDoc.Source = null;
                            DataPagerDoc.PageSize = BizAction.MaximumRows;
                            DataPagerDoc.Source = DataListDiagnosis;
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                    indicator.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {

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
            if (DataListDiagnosis == null || DiagnosisList == null || DiagnosisList.Count <= 0)
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
                            //var item = from r in DiagnosisList
                            //           where r.Categori == ((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem).Categori
                            //           select new clsEMRDiagnosisVO
                            //           {
                            //               Status = r.Status,
                            //               ID = r.ID,
                            //               Code = r.Code,
                            //               Diagnosis = r.Diagnosis,
                            //               ICDId = r.ICDId,
                            //               DiagnosisTypeId = DiagnosisTypeId
                            //           };

                            var item = from r in DiagnosisList
                                       where r.ID == ((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem).ID
                                       select new clsEMRDiagnosisVO
                                       {
                                           Status = r.Status,
                                           ID = r.ID,
                                           Code = r.Code,
                                           Diagnosis = r.Diagnosis,
                                           //ICDId = r.ICDId,
                                           DiagnosisTypeId = DiagnosisTypeId
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
                    {
                        DiagnosisList.Remove((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem);
                        //Added by AJ Date 25/11/2016
                        foreach (var item in DiagnosisList.ToList())
                        {
                            if (((clsEMRDiagnosisVO)dgDiagnosisList.SelectedItem).ID == item.ID)
                            {
                                DiagnosisList.Remove(item);
                                break;
                            }
                        }
                        //***//---------------------
                    }
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

    }
}



