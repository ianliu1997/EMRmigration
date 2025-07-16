using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.CompoundDrug;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using System.ComponentModel;
using System.Windows.Resources;
using System.Reflection;
using System.Xml.Linq;
using System.IO;
using System.Net;

namespace PalashDynamics.Pharmacy
{
    public partial class CompoundItem : UserControl
    {
        #region DataMembers
        private SwivelAnimation objAnimation;
        private ObservableCollection<clsCompoundDrugDetailVO> _ocCompoundDrugDetails;
        private ObservableCollection<clsCompoundDrugMasterVO> _ocCompoundDrug;
        public PagedSortableCollectionView<clsCompoundDrugMasterVO> DataList { get; private set; }
        WaitIndicator indicator = new WaitIndicator();
        Boolean IsPageLoded = false;
        string textBefore = string.Empty;
        int selectionStart = 0;
        int selectionLength = 0;
        private clsCompoundDrugMasterVO _ItemSelected;
        private string _strSearchByCode;
        private string _strSearchByDescription;
        public string Action { get; set; }
        public string ModuleName { get; set; }
        #endregion DataMembers

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Properties
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                OnPropertyChanged("DataListPageSize");
            }
        }
        public string SearchByCode { get { return _strSearchByCode; } }
        public string SearchByDescription { get { return _strSearchByDescription; } }
        public ObservableCollection<clsCompoundDrugDetailVO> ocCompoundDrugDetails { get; set; }
        public ObservableCollection<clsCompoundDrugMasterVO> ocCompoundDrug { get; set; }

        public clsCompoundDrugMasterVO ItemSelected
        {
            get { return _ItemSelected; }
            set { _ItemSelected = value; }
        }
        #endregion Properties

        #region Constructor
        public CompoundItem()
        {
            InitializeComponent();
            this.DataContext = new clsCompoundDrugMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsCompoundDrugMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 6;
            //======================================================
        }
        #endregion Constructor

        #region Events
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsCompoundDrugMasterVO();
                FetchCompoundDrugList();
                ocCompoundDrugDetails = new ObservableCollection<clsCompoundDrugDetailVO>();
                dgCompoundDrugDetailList.ItemsSource = null;
                dgCompoundDrugDetailList.ItemsSource = ocCompoundDrugDetails;
                SetCommandButtonState("New");
                ValidateCompoundDrug();
                IsPageLoded = true;

            }
        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Forward);
            if (ocCompoundDrugDetails != null)
                ocCompoundDrugDetails.Clear();
            ClearControls();
            ValidateCompoundDrug();
            this.DataContext = new clsCompoundDrugMasterVO();
            SetCommandButtonState("ClickNew");
        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// This event is used to fetch all the record matching all filter criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (ocCompoundDrugDetails != null)
                ocCompoundDrugDetails.Clear();

            DataList = new PagedSortableCollectionView<clsCompoundDrugMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 6;
            dgCompoundDrugDetailList.ItemsSource = null;
            dgCompoundDrugList.ItemsSource = null;
            dgMoleculeItemList.ItemsSource = null;
            dpgrCompoundDrugList.DataContext = DataList;
            _strSearchByCode = txtCode.Text == String.Empty ? null : txtCode.Text.Trim();
            _strSearchByDescription = txtDescriptions.Text == String.Empty ? null : txtDescriptions.Text.Trim();
            FetchCompoundDrugList();
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateCompoundDrug())
            {
                //clsAddCompoundDrugBizActionVO objBizActionVO = new clsAddCompoundDrugBizActionVO();
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Save Compound Drug ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();

            }
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (ocCompoundDrugDetails != null)
                ocCompoundDrugDetails.Clear();
            objAnimation.Invoke(RotationType.Backward);
        }

        private void CmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmDrugSelectionList frmDrug = new frmDrugSelectionList();

                frmDrug.OnAddButton_Click += new RoutedEventHandler(WinDrug_OnAddButton_Click);
                frmDrug.Show();
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }

        private void dgCompoundDrugList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCompoundDrugList.SelectedItem != null)
            {
                this.ItemSelected = (clsCompoundDrugMasterVO)(dgCompoundDrugList.SelectedItem);
                FillCompoundDrugDetails(this.ItemSelected.ID, this.ItemSelected.UnitID);
            }
        }

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateCompoundDrug())
            {

                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Modify Compound Drug ?";

                MessageBoxControl.MessageBoxChildWindow msgWin2 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);


                msgWin2.OnMessageBoxClosed += (args) =>
                    {
                        if (args == MessageBoxResult.Yes)
                        {
                            clsUpdateCompoundDrugBizActionVO objBizActionVO = new clsUpdateCompoundDrugBizActionVO();
                            objBizActionVO.CompoundDrug = new clsCompoundDrugMasterVO();
                            objBizActionVO.CompoundDrug = ((clsCompoundDrugMasterVO)this.DataContext);
                            objBizActionVO.CompoundDrugDetailList = new List<clsCompoundDrugDetailVO>();
                            try
                            {
                                objBizActionVO.CompoundDrugDetailList = ocCompoundDrugDetails.ToList();
                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client.ProcessCompleted += (s, arg) =>
                                    {
                                        if (arg.Error == null && arg.Result != null)
                                        {
                                            if (((clsUpdateCompoundDrugBizActionVO)arg.Result).SuccessStatus == 1)
                                            {
                                                FetchCompoundDrugList();
                                                MessageBoxControl.MessageBoxChildWindow msgWin =
                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                                                SetCommandButtonState("New");
                                                msgWin.Show();
                                            }
                                            else if (((clsUpdateCompoundDrugBizActionVO)arg.Result).SuccessStatus == 2)
                                            {

                                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be update because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                                msgWindow.Show();
                                            }
                                            else if (((clsUpdateCompoundDrugBizActionVO)arg.Result).SuccessStatus == 3)
                                            {

                                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be update because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                                msgWindow.Show();
                                            }

                                        }
                                        objAnimation.Invoke(RotationType.Backward);
                                    };
                                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client.CloseAsync();
                            }
                            catch (Exception Ex)
                            {
                                throw Ex;
                            }
                        }
                    };
                msgWin2.Show();
            }
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
            ClearControls();
            SetCommandButtonState("New");
            if (FrontPanel.Visibility == Visibility.Visible)
            {
                ModuleName = "PalashDynamics.Administration";
                Action = "PalashDynamics.Administration.frmInventoryConfiguration";
                UserControl rootPage = Application.Current.RootVisual as UserControl;

                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Inventory Configuration";

                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
                FetchCompoundDrugList();

        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance(Action) as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw;
            }



        }

        private void LnkUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ItemSelected = (clsCompoundDrugMasterVO)dgCompoundDrugList.SelectedItem;
                if (this.ItemSelected != null)
                {
                    this.DataContext = this.ItemSelected;

                    ((clsCompoundDrugMasterVO)this.DataContext).ID = this.ItemSelected.ID;
                    ((clsCompoundDrugMasterVO)this.DataContext).UnitID = this.ItemSelected.UnitID;

                    FillCompoundDrugDetails(this.ItemSelected.ID, this.ItemSelected.UnitID);

                    objAnimation.Invoke(RotationType.Forward);
                    SetCommandButtonState("Modify");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ocCompoundDrugDetails != null)
                    ocCompoundDrugDetails.Clear();

                DataList = new PagedSortableCollectionView<clsCompoundDrugMasterVO>();
                DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                DataListPageSize = 6;
                dgCompoundDrugDetailList.ItemsSource = null;
                dgCompoundDrugList.ItemsSource = null;
                dgMoleculeItemList.ItemsSource = null;
                dpgrCompoundDrugList.DataContext = DataList;
                _strSearchByCode = txtCode.Text == String.Empty ? null : txtCode.Text.Trim();
                _strSearchByDescription = txtDescriptions.Text == String.Empty ? null : txtDescriptions.Text.Trim();
                FetchCompoundDrugList();
            }
        }

        private void hlnkDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ocCompoundDrugDetails != null && ocCompoundDrugDetails.Count > 2)
            {
                int iRowIndex = DataGridRow.GetRowContainingElement(sender as FrameworkElement).GetIndex();
                ocCompoundDrugDetails.RemoveAt(iRowIndex);
                dgCompoundDrugDetailList.UpdateLayout();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Compound drug must contain atleast two drug.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        #endregion Events

        #region HandlerMethod
        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchCompoundDrugList();
        }
        void WinDrug_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmDrugSelectionList)sender).DialogResult == true)
            {
                if (ocCompoundDrugDetails == null)
                    ocCompoundDrugDetails = new System.Collections.ObjectModel.ObservableCollection<clsCompoundDrugDetailVO>();
                foreach (var item in ((frmDrugSelectionList)sender).SelectedDrugList)
                {
                    clsCompoundDrugDetailVO objCompoundDrugDetails = new clsCompoundDrugDetailVO();
                    objCompoundDrugDetails.ItemID = item.ItemID;
                    objCompoundDrugDetails.ItemUnitID = item.ItemUnitID;
                    objCompoundDrugDetails.ItemCode = item.ItemCode;
                    objCompoundDrugDetails.ItemName = item.ItemName;
                    if (!ocCompoundDrugDetails.Where(z => z.ItemID == item.ItemID && z.ItemUnitID == item.ItemUnitID).Any())
                        ocCompoundDrugDetails.Add(objCompoundDrugDetails);
                }
                dgMoleculeItemList.ItemsSource = null;
                dgMoleculeItemList.ItemsSource = ocCompoundDrugDetails;
                dgMoleculeItemList.UpdateLayout();
            }
        }

        protected void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strText = ((TextBox)sender).Text;
            if ((textBefore != null && !String.IsNullOrEmpty(strText)) && (!strText.IsPositiveNumber() || !strText.IsValueDouble()))
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = string.Empty;
                selectionStart = selectionLength = 0;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        #endregion HandlerMethod

        #region "Private Methods"

        private void ClearControls()
        {
            txtCode.Text = String.Empty;
            txtDescriptions.Text = string.Empty;
            txtItemCode.Text = String.Empty;
            txtItemDescriptions.Text = String.Empty;
            txtLaborChargeAmt.Text = String.Empty;
            txtLaborChargePer.Text = String.Empty;
        }

        private void FillCompoundDrugDetails(long lngCompoundDrugID, long lngCompoundDrugUnitID)
        {
            clsGetCompoundDrugDetailsBizActionVO objBizActionVO = new clsGetCompoundDrugDetailsBizActionVO();
            clsCompoundDrugMasterVO objCompoundDrug = (clsCompoundDrugMasterVO)dgCompoundDrugList.SelectedItem;
            objBizActionVO.CompoundDrug = new clsCompoundDrugMasterVO();
            try
            {
                objBizActionVO.CompoundDrug.ID = lngCompoundDrugID;
                objBizActionVO.CompoundDrug.UnitID = lngCompoundDrugUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {


                            clsGetCompoundDrugDetailsBizActionVO objDetails = (clsGetCompoundDrugDetailsBizActionVO)arg.Result;
                            List<clsCompoundDrugDetailVO> lstCompoundDrugDetails = objDetails.CompoundDrugDetailList;
                            ocCompoundDrugDetails = new ObservableCollection<clsCompoundDrugDetailVO>();
                            if (objDetails.CompoundDrugDetailList.Count > 0)
                            {
                                foreach (clsCompoundDrugDetailVO item in objDetails.CompoundDrugDetailList)
                                {
                                    ocCompoundDrugDetails.Add(item);
                                }
                            }
                            dgCompoundDrugDetailList.ItemsSource = null;
                            dgCompoundDrugDetailList.ItemsSource = ocCompoundDrugDetails;
                            dgCompoundDrugDetailList.UpdateLayout();
                            dgCompoundDrugList.UpdateLayout();
                            dgCompoundDrugDetailList.Focus();

                            dgMoleculeItemList.ItemsSource = null;
                            dgMoleculeItemList.ItemsSource = ocCompoundDrugDetails;
                            dgMoleculeItemList.UpdateLayout();

                        }
                    };
                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// This method is used to fetch all the Compound drugs.
        /// </summary>
        private void FetchCompoundDrugList()
        {
            indicator.Show();
            clsGetCompoundDrugBizActionVO objBizActionVO = new clsGetCompoundDrugBizActionVO();
            // These parameters are used for the searching purpose.
          //  objBizActionVO.SearchByCode = txtCode.Text == String.Empty ? null : txtCode.Text.Trim();
          //  objBizActionVO.SearchByDescription = txtDescriptions.Text == String.Empty ? null : txtDescriptions.Text.Trim();
            objBizActionVO.SearchByCode = SearchByCode;
            objBizActionVO.SearchByDescription = SearchByDescription;
            // End
            // These parameters are used for the Pagination
            objBizActionVO.PagingEnabled = true;
            objBizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            objBizActionVO.MaximumRows = DataList.PageSize == 0 ? 6 : DataList.PageSize;
            // End
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetCompoundDrugBizActionVO objCompoundDrug = ((clsGetCompoundDrugBizActionVO)arg.Result);
                            dgCompoundDrugList.ItemsSource = null;
                            dpgrCompoundDrugList.Source = null;
                            dgCompoundDrugDetailList.ItemsSource = null;
                            if (objCompoundDrug.CompoundDrugList != null)
                            {
                                DataList.Clear();
                                DataList.TotalItemCount = objCompoundDrug.TotalRowCount;
                                foreach (var item in objCompoundDrug.CompoundDrugList)
                                {
                                    DataList.Add(item);
                                }

                                dgCompoundDrugList.ItemsSource = DataList;
                                dpgrCompoundDrugList.Source = DataList;
                                dgCompoundDrugList.SelectedItem = DataList[0];

                            }
                            indicator.Close();
                        }
                        else
                            indicator.Close();
                    }
                    else
                    {
                        System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while loading compound drug.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to Validate the Compound Drug before saving. 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateCompoundDrug()
        {
            bool isValid = true;
            if (String.IsNullOrEmpty(txtItemCode.Text))
            {
                txtItemCode.SetValidation("Please enter code.");
                txtItemCode.RaiseValidationError();
                txtItemCode.Focus();
                isValid = false;
                return isValid;
            }
            else
            {
                txtItemCode.ClearValidationError();
            }

            if (string.IsNullOrEmpty(txtItemDescriptions.Text) || string.IsNullOrEmpty(txtItemDescriptions.Text.Trim()))
            {
                txtItemDescriptions.SetValidation("Please enter description.");
                txtItemDescriptions.RaiseValidationError();
                txtItemDescriptions.Focus();
                isValid = false;
                return isValid;
            }
            else
                txtItemDescriptions.ClearValidationError();

            // Both TextBoxes are Empty
            if (((String.IsNullOrEmpty(txtLaborChargeAmt.Text) && String.IsNullOrEmpty(txtLaborChargeAmt.Text.Trim())) && (String.IsNullOrEmpty(txtLaborChargePer.Text) && String.IsNullOrEmpty(txtLaborChargePer.Text.Trim()))) || ((String.IsNullOrEmpty(txtLaborChargeAmt.Text) || Convert.ToDouble(txtLaborChargeAmt.Text.Trim()) <= 0) && (string.IsNullOrEmpty(txtLaborChargePer.Text) || Convert.ToDouble(txtLaborChargePer.Text.Trim()) <= 0)))
            {
                txtLaborChargeAmt.SetValidation("Please enter labour charge in amount or Percentage");
                txtLaborChargePer.SetValidation("Please enter labour charge in amount or Percentage");
                txtLaborChargeAmt.RaiseValidationError();
                txtLaborChargePer.RaiseValidationError();
                txtLaborChargePer.Focus();
                isValid = false;
                return isValid;
            }
            else
            {
                txtLaborChargePer.ClearValidationError();
                txtLaborChargeAmt.ClearValidationError();
            }
            // Both Are Filled
            if ((!String.IsNullOrEmpty(txtLaborChargeAmt.Text) && !String.IsNullOrEmpty(txtLaborChargeAmt.Text.Trim()) && Convert.ToDouble(txtLaborChargeAmt.Text.Trim()) > 0) && (!String.IsNullOrEmpty(txtLaborChargePer.Text) && !String.IsNullOrEmpty(txtLaborChargePer.Text.Trim()) && Convert.ToDouble(txtLaborChargePer.Text.Trim()) > 0))
            {
                txtLaborChargeAmt.SetValidation("Please enter labour charge either in amount or Percentage");
                txtLaborChargeAmt.RaiseValidationError();
                txtLaborChargePer.RaiseValidationError();
                txtLaborChargeAmt.Focus();
                isValid = false;
                return isValid;
            }
            else
            {
                txtLaborChargePer.ClearValidationError();
                txtLaborChargeAmt.ClearValidationError();
            }

            //if ((String.IsNullOrEmpty(txtLaborChargeAmt.Text) || String.IsNullOrEmpty(txtLaborChargeAmt.Text.Trim())) && (String.IsNullOrEmpty(txtLaborChargePer.Text) || String.IsNullOrEmpty(txtLaborChargePer.Text.Trim())))
            //{

            //    txtLaborChargeAmt.SetValidation("Please enter labour charge in amount or Percentage");
            //    txtLaborChargeAmt.RaiseValidationError();
            //    txtLaborChargePer.RaiseValidationError();
            //    txtLaborChargeAmt.Focus();
            //    isValid = false;
            //    return isValid;
            //}
            //else
            //{
            //    txtLaborChargePer.ClearValidationError();
            //    txtLaborChargeAmt.ClearValidationError();
            //}
            if (!String.IsNullOrEmpty(txtLaborChargePer.Text) && Convert.ToDouble(txtLaborChargePer.Text) > 100)
            {
                txtLaborChargePer.SetValidation("Value must not exceed 100");
                txtLaborChargePer.RaiseValidationError();
                txtLaborChargePer.Focus();
                isValid = false;
                return isValid;
            }
            else
                txtLaborChargePer.ClearValidationError();

            if (IsPageLoded)
            {
                if (ocCompoundDrugDetails != null && ocCompoundDrugDetails.Count <= 1)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select at least 2 Items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.Show();
                    isValid = false;
                    return isValid;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        private void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddCompoundDrugBizActionVO objBizactionVO = new clsAddCompoundDrugBizActionVO();
                objBizactionVO.CompoundDrug = new clsCompoundDrugMasterVO();

                objBizactionVO.CompoundDrug = (clsCompoundDrugMasterVO)this.DataContext;

                objBizactionVO.CompoundDrugDetailList = ocCompoundDrugDetails.ToList();

                try
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddCompoundDrugBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                FetchCompoundDrugList();


                                MessageBoxControl.MessageBoxChildWindow msgWin =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                                msgWin.Show();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddCompoundDrugBizActionVO)args.Result).SuccessStatus == 2)
                            {

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddCompoundDrugBizActionVO)args.Result).SuccessStatus == 3)
                            {

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }


                    };
                    client.ProcessAsync(objBizactionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion "Private Methods"

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    break;

                case "Save":
                    cmdModify.IsEnabled = false;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;

                case "Modify":
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    break;
                case "ClickNew":
                    cmdModify.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

    }
}
