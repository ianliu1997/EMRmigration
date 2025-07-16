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
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using System.Reflection;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using PalashDynamics.Collections;
using System.Windows.Media.Imaging;
using PalashDynamics.UserControls;
using System.Text;
namespace PalashDynamics.Administration
{

    public partial class CompanyMaster : UserControl
    {
        #region Validation
        private bool CheckValidation()
        {
            bool result = true;

            if (txtCode.Text == "")
            {

                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();

                result = false;
            }
            else
                txtCode.ClearValidationError();

            if (txtDescription.Text == "")
            {

                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();

                result = false;
            }
            else
                txtDescription.ClearValidationError();

            if (txtContactNum.Text == "")
            {

                txtContactNum.SetValidation("Please Enter Contact Number");
                txtContactNum.RaiseValidationError();
                txtContactNum.Focus();

                result = false;
            }
            else
                txtContactNum.ClearValidationError();

            if (txtEmail.Text.Trim().Length > 0)
            {
                if (txtEmail.Text.IsEmailValid())
                    txtEmail.ClearValidationError();
                else
                {
                    txtEmail.SetValidation("Please enter valid Email");
                    txtEmail.RaiseValidationError();
                    txtEmail.Focus();
                    result = false;
                }
            }

            //if (txtHeaderText.Text == string.Empty)
            //{

            //    txtHeaderText.SetValidation("Please Enter Header Text.");
            //    txtHeaderText.RaiseValidationError();
            //    txtHeaderText.Focus();

            //    result = false;
            //}
            //else
            //    txtHeaderText.ClearValidationError();

            //if (txtFooterText.Text == string.Empty)
            //{

            //    txtFooterText.SetValidation("Please Enter Footer Text.");
            //    txtFooterText.RaiseValidationError();
            //    txtFooterText.Focus();

            //    result = false;
            //}
            //else
            //    txtFooterText.ClearValidationError();

            return result;
        }

        private bool CheckDuplicasy()
        {
            clsCompanyVO CompanyItem;
            clsCompanyVO CompanyItem1;
            if (IsNew)
            {
                CompanyItem = ((PagedSortableCollectionView<clsCompanyVO>)grdCompany.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                CompanyItem1 = ((PagedSortableCollectionView<clsCompanyVO>)grdCompany.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper())); //p.CompanyName.ToUpper()

                //CompanyItem = ((List<clsCompanyVO>)grdCompany.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                //CompanyItem1 = ((List<clsCompanyVO>)grdCompany.ItemsSource).FirstOrDefault(p => p.CompanyName.ToUpper().Equals(txtDescription.Text.ToUpper()));

            }
            else
            {
                CompanyItem = ((PagedSortableCollectionView<clsCompanyVO>)grdCompany.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.Id != ((clsCompanyVO)grdCompany.SelectedItem).Id);
                CompanyItem1 = ((PagedSortableCollectionView<clsCompanyVO>)grdCompany.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()) && p.Id != ((clsCompanyVO)grdCompany.SelectedItem).Id); //p.CompanyName.ToUpper()


                //CompanyItem = ((List<clsCompanyVO>)grdCompany.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.Id != ((clsCompanyVO)grdCompany.SelectedItem).Id);
                //CompanyItem1 = ((List<clsCompanyVO>)grdCompany.ItemsSource).FirstOrDefault(p => p.CompanyName.ToUpper().Equals(txtDescription.Text.ToUpper()) && p.Id != ((clsCompanyVO)grdCompany.SelectedItem).Id);
            }
            if (CompanyItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (CompanyItem1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Description already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool chkTariff()
        {
            //clsTariffDetailsVO dept = ((List<clsTariffDetailsVO>)dgTariffSelectedList.ItemsSource).FirstOrDefault(p => p.Status == true); // ((List<clsTariffDetailsVO>)dgTariffList.ItemsSource).FirstOrDefault(p => p.Status == true);

            if (dgTariffSelectedList.ItemsSource != null && ((List<clsTariffDetailsVO>)dgTariffSelectedList.ItemsSource).Count > 0) //(dept != null)
            {
                return true;
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "Please Select Tariff";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW.Show();

                return false;
            }
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescription.Text = txtDescription.Text.ToTitleCase();
        }
        #endregion

        #region  Variables
        private SwivelAnimation objAnimation;

        private long CompanyId;

        bool IsPageLoded = false;
        public clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        List<clsTariffDetailsVO> TariffList { get; set; }
        bool IsNew = false;
        bool IsCancel = true;

        byte[] AttachedFileContents;
        string AttachedFileName = string.Empty;

        byte[] AttachedHeadImgFileContents;
        string AttachedHeadImgFileName = string.Empty;

        byte[] AttachedFootImgFileContents;
        string AttachedfootImgFileName = string.Empty;

        byte[] CompanyImage; //{ get; set; }
        byte[] CompFooterImage; //{ get; set; }
        byte[] CompHeaderImage;// { get; set; }
        private clsCompanyVO companyLogoInfo;
        WaitIndicator waitIndi;

        //public List<clsTariffDetailsVO> userTariffList { get; private set; }

        public PagedSortableCollectionView<clsTariffDetailsVO> userTariffList { get; private set; }
        public int DataListPageSize1
        {
            get
            {
                return userTariffList.PageSize;
            }
            set
            {
                if (value == userTariffList.PageSize) return;
                userTariffList.PageSize = value;
            }
        }
        private List<clsTariffDetailsVO> _OtherServiceSelected;
        private List<clsTariffDetailsVO> SelectedServiceList;
        #endregion

        #region FrontPannel Paging
        public PagedSortableCollectionView<clsCompanyVO> DataList { get; private set; }
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
            }
        }
        #endregion

        #region Constructor
        public CompanyMaster()
        {
            InitializeComponent();
            //userTariffList = new List<clsTariffDetailsVO>();
            waitIndi = new WaitIndicator();
            userTariffList = new PagedSortableCollectionView<clsTariffDetailsVO>();
            userTariffList.OnRefresh += new EventHandler<RefreshEventArgs>(userTariffList_OnRefresh);
            DataListPageSize1 = 15;
            _OtherServiceSelected = new List<clsTariffDetailsVO>();
            SelectedServiceList = new List<clsTariffDetailsVO>();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.DataContext = new clsCompanyVO();
            SetCommandButtonState("Load");
            FillTariffList();
            FillCompanyType();
            FillPatientCategory();

            DataList = new PagedSortableCollectionView<clsCompanyVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.dataGrid2Pager.DataContext = DataList;
            this.grdCompany.DataContext = DataList;
            FetchData();
            CheckValidation();

        }
        #endregion Constructor

        #region Loaded Event
        void CompanyMaster_Loaded(object sender, RoutedEventArgs e)
        {


        }
        #endregion Loaded Event

        #region Public Methods

        /// <summary>
        /// This Method Is Use For Two Purpose It Fill DataGrid (All Company Details) and 
        /// When We click On View Hyperlink Button Then It Will Get Details of Company on Which we Click  
        /// </summary>

        public void SetupPage()
        {
            if (waitIndi == null) waitIndi = new WaitIndicator();
            waitIndi.Show();
            clsGetCompanyDetailsBizActionVO bizActionVO = new clsGetCompanyDetailsBizActionVO();

            clsCompanyVO getCompanyinfo = new clsCompanyVO();
            bizActionVO.ItemMatserDetails = new List<clsCompanyVO>();
            //if (txtSearchDesc.Text != null)
            //    bizActionVO.Description = txtSearchDesc.Text;
            bizActionVO.CompanyId = CompanyId;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    waitIndi.Close();
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetCompanyDetailsBizActionVO ObjCompany = new clsGetCompanyDetailsBizActionVO();
                        ObjCompany = (clsGetCompanyDetailsBizActionVO)args.Result;
                        if (SelectedServiceList != null)
                            SelectedServiceList.Clear();
                        if (_OtherServiceSelected != null)
                            _OtherServiceSelected.Clear();

                        bizActionVO.ItemMatserDetails = (((clsGetCompanyDetailsBizActionVO)args.Result).ItemMatserDetails);
                        if (CompanyId > 0)  //&& bizActionVO.ItemMatserDetails.Count == 1
                        {
                            getCompanyinfo = bizActionVO.ItemMatserDetails[0];
                            //Added by Ashish Z.
                            this.companyLogoInfo = getCompanyinfo; //for View the uploaloaded logo file.
                            CompanyId = getCompanyinfo.Id;
                            txtCode.Text = getCompanyinfo.Code;
                            txtDescription.Text = getCompanyinfo.Description;
                            cmbCompanyType.SelectedValue = getCompanyinfo.CompanyTypeId;

                            //rohinee
                            cmbPatientCategory.SelectedValue = getCompanyinfo.PatientCatagoryID;
                            //

                            txtContPerName.Text = getCompanyinfo.ContactPerson;
                            txtContactNum.Text = Convert.ToString(getCompanyinfo.ContactNo);
                            txtEmail.Text = getCompanyinfo.CompanyEmail;
                            txtCompAddress.Text = getCompanyinfo.CompanyAddress;

                            txtTitle.Text = getCompanyinfo.Title;
                            if (getCompanyinfo.AttachedFileContent != null)
                            {
                                byte[] imageBytes = getCompanyinfo.AttachedFileContent;

                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(imageBytes, false));
                                //LogoImage.Source = img;
                                WriteableBitmap bmp = new WriteableBitmap((int)LogoImage.Width, (int)LogoImage.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                bmp.FromByteArray(imageBytes);
                                LogoImage.Source = bmp;
                            }
                            else
                            {
                                LogoImage.Source = null;
                            }
                            //txtFileName.Text = getCompanyinfo.AttachedFileName;

                            txtHeaderImageTitle.Text = getCompanyinfo.TitleHeaderImage;
                            if (getCompanyinfo.AttachedHeadImgFileContent != null)
                            {
                                byte[] imageBytes1 = getCompanyinfo.AttachedHeadImgFileContent;

                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(imageBytes, false));
                                //HeaderImage.Source = img;
                                WriteableBitmap bmp1 = new WriteableBitmap((int)HeaderImage.Width, (int)HeaderImage.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                bmp1.FromByteArray(imageBytes1);
                                HeaderImage.Source = bmp1;
                            }
                            else
                            {
                                HeaderImage.Source = null;
                            }
                            //txtHeaderImageFileName.Text = getCompanyinfo.AttachedHeadImgFileName;

                            txtFooterImageTitle.Text = getCompanyinfo.TitleFooterImage;
                            if (getCompanyinfo.AttachedFootImgFileContent != null)
                            {
                                byte[] imageBytes2 = getCompanyinfo.AttachedFootImgFileContent;

                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(imageBytes, false));
                                //FooterImage.Source = img;
                                WriteableBitmap bmp2 = new WriteableBitmap((int)FooterImage.Width, (int)FooterImage.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                bmp2.FromByteArray(imageBytes2);
                                FooterImage.Source = bmp2;
                            }
                            else
                            {
                                FooterImage.Source = null;
                            }
                            //txtFooterImageFileName.Text = getCompanyinfo.AttachedFootImgFileName;

                            txtHeaderText.Text = getCompanyinfo.HeaderText;
                            txtFooterText.Text = getCompanyinfo.FooterText;
                        }

                        bizActionVO.TariffDetails = (((clsGetCompanyDetailsBizActionVO)args.Result).TariffDetails);
                        if (CompanyId > 0 && bizActionVO.TariffDetails.Count > 0)
                        {
                            foreach (clsTariffDetailsVO item in bizActionVO.TariffDetails.ToList())
                            {
                                SelectedServiceList.Add(item);
                            }
                            _OtherServiceSelected = SelectedServiceList;
                            foreach (var item in _OtherServiceSelected)
                            {
                                item.SelectTariff = true;
                            }
                            dgTariffSelectedList.ItemsSource = null;
                            dgTariffSelectedList.ItemsSource = _OtherServiceSelected;
                            dgTariffSelectedList.UpdateLayout();
                        }
                        FillTariffList();
                    }
                   
                };
                client.ProcessAsync(bizActionVO, User); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                waitIndi.Close();
                throw;
            }

        }


        public void FetchData()
        {
            if (waitIndi == null) waitIndi = new WaitIndicator();
            waitIndi.Show();
            clsGetCompanyDetailsBizActionVO bizActionVO = new clsGetCompanyDetailsBizActionVO();
            bizActionVO.ItemMatserDetails = new List<clsCompanyVO>();
            bizActionVO.Description = txtSearch.Text.Trim();
            bizActionVO.CompanyId = 0;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
            {
                bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                bizActionVO.UnitID = 0;
            }

            bizActionVO.PagingEnabled = true;
            bizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            bizActionVO.MaximumRows = DataList.PageSize;


            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //grdCompany.ItemsSource = null;
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetCompanyDetailsBizActionVO result = args.Result as clsGetCompanyDetailsBizActionVO;
                        DataList.Clear();
                        if (result.ItemMatserDetails != null)
                        {

                            DataList.TotalItemCount = (int)((clsGetCompanyDetailsBizActionVO)args.Result).TotalRows;
                            foreach (clsCompanyVO item in result.ItemMatserDetails)
                            {
                                DataList.Add(item);

                            }
                            //grdCompany.ItemsSource = null;
                            //grdCompany.ItemsSource = DataList;
                            //dataGrid2Pager.Source = null;
                            //dataGrid2Pager.PageSize = bizActionVO.MaximumRows;
                            //dataGrid2Pager.Source = DataList;
                        }

                        //bizActionVO.ItemMatserDetails = (((clsGetCompanyDetailsBizActionVO)args.Result).ItemMatserDetails);
                        //grdCompany.ItemsSource = bizActionVO.ItemMatserDetails;
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                    waitIndi.Close();
                };
                client.ProcessAsync(bizActionVO, User); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                waitIndi.Close();
                throw;
            }
            finally
            {
                //waitIndi.Close();
            }

        }


        /// <summary>
        /// When We Click On Add Button All UI Must Empty
        /// </summary>
        public void ClearUI()
        {
            
            if (_OtherServiceSelected != null)
            {
                _OtherServiceSelected.Clear();
                _OtherServiceSelected = new List<clsTariffDetailsVO>();
            }
            dgTariffSelectedList.ItemsSource = null;
            if (SelectedServiceList != null)
            {
                SelectedServiceList.Clear();
                SelectedServiceList = new List<clsTariffDetailsVO>();
            }
            txtCode.Text = "";
            txtDescription.Text = "";
            cmbCompanyType.SelectedValue = (long)0;
            cmbPatientCategory.SelectedValue = (long)0;
            txtCompAddress.Text = string.Empty;
            txtContactNum.Text = string.Empty;
            txtContPerName.Text = string.Empty;
            txtEmail.Text = string.Empty;

            txtHeaderText.Text = string.Empty;
            txtFooterText.Text = string.Empty;
            txtFileName.Text = string.Empty;
            txtTitle.Text = string.Empty;
            LogoImage.Source = null;
            txtHeaderImageTitle.Text = string.Empty;
            HeaderImage.Source = null;
            txtFooterImageTitle.Text = string.Empty;
            FooterImage.Source = null;


            //if (dgTariffList.ItemsSource != null)
            //{
            //    //List<clsTariffDetailsVO> lList = (List<clsTariffDetailsVO>)dgTariffList.ItemsSource;
            //    foreach (var item in userTariffList)
            //    {
            //        item.SelectTariff = false;
            //        item.IsDefaultStatus = false;
            //        item.IsSelectedTariff = true;
            //    }
            //    dgTariffList.ItemsSource = null;
            //    dgTariffList.ItemsSource = userTariffList;//lList;
            //}
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        void userTariffList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillTariffList();
        }
        #endregion

        #region FillCombobox an Lists

        public void FillCompanyType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CompanyTypeMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbCompanyType.ItemsSource = null;
                    cmbCompanyType.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    cmbCompanyType.SelectedValue = ((clsCompanyVO)this.DataContext).CompanyTypeId;
                }


            };

            client.ProcessAsync(BizAction, User); // new clsUserVO());
            client.CloseAsync();
        }

        public void FillPatientCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PatientSourceMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbPatientCategory.ItemsSource = null;
                    cmbPatientCategory.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    cmbPatientCategory.SelectedValue = ((clsCompanyVO)this.DataContext).PatientCatagoryID;
                }


            };

            client.ProcessAsync(BizAction, User); // new clsUserVO());
            client.CloseAsync();
        }
        private void FillTariffList()
        {
            try
            {
                if (waitIndi == null) waitIndi = new WaitIndicator();
                waitIndi.Show();
                clsGetTariffDetailsListBizActionVO BizAction = new clsGetTariffDetailsListBizActionVO();
                BizAction.PatientSourceDetails = new List<clsTariffDetailsVO>();
                BizAction.IsFromCompanyMaster = true;

                BizAction.IsPagingEnabled = true;
                BizAction.StartRowIndex = userTariffList.PageIndex * userTariffList.PageSize;
                BizAction.MaximumRows = userTariffList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //dgTariffList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    waitIndi.Close();
                    if (arg.Error == null)
                    {
                        if (((clsGetTariffDetailsListBizActionVO)arg.Result).PatientSourceDetails != null)
                        {
                            BizAction.PatientSourceDetails = ((clsGetTariffDetailsListBizActionVO)arg.Result).PatientSourceDetails;
                            //List<clsTariffDetailsVO> userTariffList = new List<clsTariffDetailsVO>();
                            userTariffList.Clear();
                            userTariffList.TotalItemCount = (int)((clsGetTariffDetailsListBizActionVO)arg.Result).TotalRows;
                            if (_OtherServiceSelected != null && _OtherServiceSelected.Count > 0)
                            {
                                foreach (clsTariffDetailsVO item in _OtherServiceSelected.ToList())
                                {
                                    BizAction.PatientSourceDetails.ToList().ForEach(x =>
                                    {
                                        if (item.TariffID == x.TariffID)
                                            x.SelectTariff = true;
                                    });
                                }
                            }

                            foreach (var item in BizAction.PatientSourceDetails)
                            {
                                userTariffList.Add(item);
                                //userTariffList.Add(new clsTariffDetailsVO() { TariffID = item.TariffID, TariffCode = item.TariffCode, TariffDescription = item.TariffDescription, Status = false, IsDefaultStatus = false });
                            }

                            dgTariffList.ItemsSource = null;
                            dgTariffList.ItemsSource = userTariffList;
                            DataPagerDoc10.Source = null;
                            DataPagerDoc10.PageSize = BizAction.MaximumRows;
                            DataPagerDoc10.Source = userTariffList;

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception EX)
            {
                waitIndi.Close();
                throw;
            }
        }

        #endregion FillCombobox

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Button Click Events

        /// <summary>
        /// This Event is Call When We click on Add Button, and show Back Panel Which Have  Company Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            CheckValidation();
            this.SetCommandButtonState("New");
            this.DataContext = new clsCompanyVO();
            dgTariffSelectedList.ItemsSource = null;
            ClearUI();
            FillTariffList();
            IsNew = true;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Company Details";
            hlkPreview.IsEnabled = false;
            hlkPreviewHeaderImage.IsEnabled = false;
            hlkPreviewFooterImage.IsEnabled = false;
            tabCompDetails.IsSelected = true;



            objAnimation.Invoke(RotationType.Forward);
        }

        /// <summary>
        /// This Event is Call When We click on Save Button and Save  Company Details
        /// (For Add and Modify  Company Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool SaveCompany = true;
                SaveCompany = CheckValidation();
                if (SaveCompany == true && chkTariff())
                {

                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to save the Company Master?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {
                    Save();
                }
            }
        }

        private void Save()
        {
            try
            {
                if (waitIndi == null) waitIndi = new WaitIndicator();
                waitIndi.Show();
                clsAddUpdateCompanyDetailsBizActionVO BizActionVO = new clsAddUpdateCompanyDetailsBizActionVO();
                BizActionVO.ItemMatserDetails = (clsCompanyVO)this.DataContext;

                if (!string.IsNullOrEmpty(txtContactNum.Text))
                    BizActionVO.ItemMatserDetails.ContactNo = Convert.ToInt64(txtContactNum.Text);

                if (cmbCompanyType.SelectedItem != null)
                    BizActionVO.ItemMatserDetails.CompanyTypeId = ((MasterListItem)cmbCompanyType.SelectedItem).ID;

                //rohinee
                if (cmbPatientCategory.SelectedItem != null)
                    BizActionVO.ItemMatserDetails.PatientCatagoryID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                //

                BizActionVO.ItemMatserDetails.TariffDetails = (List<clsTariffDetailsVO>)dgTariffSelectedList.ItemsSource; //(List<clsTariffDetailsVO>)dgTariffList.ItemsSource;

                //** added by Ashish Z.
                WriteableBitmap bmp = new WriteableBitmap((int)LogoImage.Width, (int)LogoImage.Height);
                bmp.Render(LogoImage, new MatrixTransform());
                bmp.Invalidate();

                int[] p = bmp.Pixels;
                int len = p.Length * 4;
                byte[] result = new byte[len]; // ARGB
                Buffer.BlockCopy(p, 0, result, 0, len);
                BizActionVO.ItemMatserDetails.AttachedFileContent = result;
                // BizActionVO.ItemMatserDetails.AttachedFileContent = AttachedFileContents;
                //if (AttachedFileName == null)
                //    BizActionVO.ItemMatserDetails.AttachedFileName = txtFileName.Text.Trim();
                //else
                //    BizActionVO.ItemMatserDetails.AttachedFileName = AttachedFileName;
                BizActionVO.ItemMatserDetails.Title = txtTitle.Text.Trim();

                WriteableBitmap bmp1 = new WriteableBitmap((int)HeaderImage.Width, (int)HeaderImage.Height);
                bmp1.Render(HeaderImage, new MatrixTransform());
                bmp1.Invalidate();

                int[] p1 = bmp1.Pixels;
                int len1 = p1.Length * 4;
                byte[] result1 = new byte[len1]; // ARGB
                Buffer.BlockCopy(p1, 0, result1, 0, len1);
                BizActionVO.ItemMatserDetails.AttachedHeadImgFileContent = result1;
                //BizActionVO.ItemMatserDetails.AttachedHeadImgFileContent = AttachedHeadImgFileContents;
                //if (AttachedHeadImgFileName == null)
                //    BizActionVO.ItemMatserDetails.AttachedHeadImgFileName = txtHeaderImageFileName.Text.Trim();
                //else
                //    BizActionVO.ItemMatserDetails.AttachedHeadImgFileName = AttachedHeadImgFileName;
                BizActionVO.ItemMatserDetails.TitleHeaderImage = txtHeaderImageTitle.Text.Trim();

                WriteableBitmap bmp2 = new WriteableBitmap((int)FooterImage.Width, (int)FooterImage.Height);
                bmp2.Render(FooterImage, new MatrixTransform());
                bmp2.Invalidate();

                int[] p2 = bmp2.Pixels;
                int len2 = p2.Length * 4;
                byte[] result2 = new byte[len2]; // ARGB
                Buffer.BlockCopy(p2, 0, result2, 0, len2);
                BizActionVO.ItemMatserDetails.AttachedFootImgFileContent = result2;
                //BizActionVO.ItemMatserDetails.AttachedFootImgFileContent = AttachedFootImgFileContents;
                //if (AttachedfootImgFileName == null)
                //    BizActionVO.ItemMatserDetails.AttachedFootImgFileName = txtFooterImageFileName.Text.Trim();
                //else
                //    BizActionVO.ItemMatserDetails.AttachedFootImgFileName = AttachedfootImgFileName;
                BizActionVO.ItemMatserDetails.TitleFooterImage = txtFooterImageTitle.Text.Trim();
                //**
                if (!string.IsNullOrEmpty(txtHeaderText.Text))
                    BizActionVO.ItemMatserDetails.HeaderText = txtHeaderText.Text;
                if (!string.IsNullOrEmpty(txtFooterText.Text))
                    BizActionVO.ItemMatserDetails.FooterText = txtFooterText.Text;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        FetchData();
                        ClearUI();
                        objAnimation.Invoke(RotationType.Backward);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Company Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                    SetCommandButtonState("Save");
                    waitIndi.Close();
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                waitIndi.Close();
                throw;
            }

        }

        /// <summary>
        /// This Event is Call When We click on Modify Button and Update  Company Details
        /// (For Add and Modify  Company Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool ModifyCompany = true;
                ModifyCompany = CheckValidation();
                if (ModifyCompany == true && chkTariff())
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to Update the Company  Master?";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                    msgW1.Show();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {
                    Modify();
                }
            }

        }

        private void Modify()
        {
            try
            {
                if (waitIndi == null) waitIndi = new WaitIndicator();
                waitIndi.Show();
                clsAddUpdateCompanyDetailsBizActionVO BizActionVO = new clsAddUpdateCompanyDetailsBizActionVO();
                BizActionVO.ItemMatserDetails = new clsCompanyVO();

                BizActionVO.ItemMatserDetails.Id = CompanyId;
                BizActionVO.ItemMatserDetails.Code = txtCode.Text;
                BizActionVO.ItemMatserDetails.Description = txtDescription.Text;
                BizActionVO.ItemMatserDetails.ContactPerson = txtContPerName.Text;
                BizActionVO.ItemMatserDetails.ContactNo = Convert.ToInt64(txtContactNum.Text);
                BizActionVO.ItemMatserDetails.CompanyEmail = txtEmail.Text;
                BizActionVO.ItemMatserDetails.CompanyAddress = txtCompAddress.Text;

                if (cmbCompanyType.SelectedItem != null)
                    BizActionVO.ItemMatserDetails.CompanyTypeId = ((MasterListItem)cmbCompanyType.SelectedItem).ID;

                //rohinee
                if (cmbPatientCategory.SelectedItem != null)
                    BizActionVO.ItemMatserDetails.PatientCatagoryID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                //
                BizActionVO.ItemMatserDetails.TariffDetails = (List<clsTariffDetailsVO>)dgTariffSelectedList.ItemsSource; //(List<clsTariffDetailsVO>)dgTariffList.ItemsSource;

                //**Added by Ashish Z.
                //BizActionVO.ItemMatserDetails.AttachedFileContent = this.LogoImage.Source.DeepCopy(); //AttachedFileContents;
                //byte[] result = getimage((int)LogoImage.Width, (int)LogoImage.Height);
                WriteableBitmap bmp = new WriteableBitmap((int)LogoImage.Width, (int)LogoImage.Height);
                bmp.Render(LogoImage, new MatrixTransform());
                bmp.Invalidate();

                int[] p = bmp.Pixels;
                int len = p.Length * 4;
                byte[] result = new byte[len]; // ARGB
                Buffer.BlockCopy(p, 0, result, 0, len);
                BizActionVO.ItemMatserDetails.AttachedFileContent = result;

                //if (AttachedFileName == null)
                //    BizActionVO.ItemMatserDetails.AttachedFileName = "CompLogo" + txtFileName.Text.Trim();
                //else
                //    BizActionVO.ItemMatserDetails.AttachedFileName = "CompLogo" + AttachedFileName;
                BizActionVO.ItemMatserDetails.Title = txtTitle.Text.Trim();


                //byte[] result1 = getimage((int)HeaderImage.Width, (int)HeaderImage.Height);
                WriteableBitmap bmp1 = new WriteableBitmap((int)HeaderImage.Width, (int)HeaderImage.Height);
                bmp1.Render(HeaderImage, new MatrixTransform());
                bmp1.Invalidate();

                int[] p1 = bmp1.Pixels;
                int len1 = p1.Length * 4;
                byte[] result1 = new byte[len1]; // ARGB
                Buffer.BlockCopy(p1, 0, result1, 0, len1);
                BizActionVO.ItemMatserDetails.AttachedHeadImgFileContent = result1;
                //if (AttachedHeadImgFileName == null)
                //    BizActionVO.ItemMatserDetails.AttachedHeadImgFileName = "HeadImg" + txtHeaderImageFileName.Text.Trim();
                //else
                //    BizActionVO.ItemMatserDetails.AttachedHeadImgFileName = "HeadImg" + AttachedHeadImgFileName;
                BizActionVO.ItemMatserDetails.TitleHeaderImage = txtHeaderImageTitle.Text.Trim();

                //byte[] result2 = getimage((int)FooterImage.Width, (int)FooterImage.Height);
                WriteableBitmap bmp2 = new WriteableBitmap((int)FooterImage.Width, (int)FooterImage.Height);
                bmp2.Render(FooterImage, new MatrixTransform());
                bmp2.Invalidate();

                int[] p2 = bmp2.Pixels;
                int len2 = p2.Length * 4;
                byte[] result2 = new byte[len2]; // ARGB
                Buffer.BlockCopy(p2, 0, result2, 0, len2);
                BizActionVO.ItemMatserDetails.AttachedFootImgFileContent = result2;
                //if (AttachedfootImgFileName == null)
                //    BizActionVO.ItemMatserDetails.AttachedFootImgFileName = "FootImg" + txtFooterImageFileName.Text.Trim();
                //else
                //    BizActionVO.ItemMatserDetails.AttachedFootImgFileName = "FootImg" + AttachedfootImgFileName;
                BizActionVO.ItemMatserDetails.TitleFooterImage = txtFooterImageTitle.Text.Trim();

                if (!string.IsNullOrEmpty(txtHeaderText.Text))
                    BizActionVO.ItemMatserDetails.HeaderText = txtHeaderText.Text;
                if (!string.IsNullOrEmpty(txtFooterText.Text))
                    BizActionVO.ItemMatserDetails.FooterText = txtFooterText.Text;
                //**

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        FetchData();
                        ClearUI();
                        objAnimation.Invoke(RotationType.Backward);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Company Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                    SetCommandButtonState("Modify");
                    waitIndi.Close();
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                waitIndi.Close();
                throw;
            }
        }

        private byte[] getimage(int a, int b)
        {
            WriteableBitmap bmp = new WriteableBitmap(a, b);
            bmp.Render(LogoImage, new MatrixTransform());
            bmp.Invalidate();

            int[] p = bmp.Pixels;
            int len = p.Length * 4;
            byte[] result = new byte[len]; // ARGB
            Buffer.BlockCopy(p, 0, result, 0, len);
            return result;

        }

        /// <summary>
        /// This Event is Call When We click on Cancel Button, and show Front Panel On Which DataGrid Which
        /// Have All  Company List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //if (userTariffList != null)
            //{
            //    userTariffList.Clear();
            //    userTariffList = new PagedSortableCollectionView<clsTariffDetailsVO>();
            //}
            SetCommandButtonState("Cancel");
            this.DataContext = null;
            CompanyImage = new byte[0];
            CompHeaderImage = new byte[0];
            CompFooterImage = new byte[0];
            dgTariffSelectedList.ItemsSource = null;

            //var ls = from r in userTariffList //(List<clsTariffDetailsVO>)dgTariffList.ItemsSource
            //         where r.IsSelectedTariff = true
            //         select r;
            //ls.ToList();
            //dgTariffList.ItemsSource = ls.ToList();

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Billing Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        /// <summary>
        /// This Event is call When we Click On Hyperlink Button which is Present in DataGid 
        /// and Show Specific Company Details Which we Select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            CheckValidation();
            hlkPreview.IsEnabled = true;
            hlkPreviewHeaderImage.IsEnabled = true;
            hlkPreviewFooterImage.IsEnabled = true;
            SetCommandButtonState("View");
            IsNew = false;
            ClearUI();
            CompanyId = ((clsCompanyVO)grdCompany.SelectedItem).Id;
            if (grdCompany.SelectedItem != null)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + ((clsCompanyVO)grdCompany.SelectedItem).Description;
            }
            SetupPage();
            tabCompDetails.IsSelected = true;
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {


        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
            if (dataGrid2Pager.PageIndex != 0)
                dataGrid2Pager.PageIndex = 0;
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                FetchData();
                if (dataGrid2Pager.PageIndex != 0)
                    dataGrid2Pager.PageIndex = 0;
            }

        }


        private void txtContactNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = String.Empty;
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((TextBox)sender).Text.Length > 10)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = String.Empty;
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void WaterMarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtContactNo1_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
            {
                if (!((WaterMarkTextbox)sender).Text.IsNumberValid() && textBefore != null)
                {
                    ((WaterMarkTextbox)sender).Text = textBefore;
                    ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                    ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                    textBefore = String.Empty;
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((WaterMarkTextbox)sender).Text.Length > 10)
                {
                    ((WaterMarkTextbox)sender).Text = textBefore;
                    ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                    ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                    textBefore = String.Empty;
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void chkTariff_Click(object sender, RoutedEventArgs e)
        {
            if (dgTariffList.SelectedItem != null)
            {
                clsTariffDetailsVO objVO = (clsTariffDetailsVO)dgTariffSelectedList.SelectedItem;
                clsTariffDetailsVO objServiceVO = dgTariffList.SelectedItem as clsTariffDetailsVO;
                if (_OtherServiceSelected == null)
                    _OtherServiceSelected = new List<clsTariffDetailsVO>();
                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                if (chk.IsChecked == true)
                {
                    if (_OtherServiceSelected.Count > 0)
                    {
                        var item = from r in _OtherServiceSelected
                                   where r.TariffID == ((clsTariffDetailsVO)dgTariffList.SelectedItem).TariffID
                                   select new clsTariffDetailsVO
                                   {
                                       Status = r.Status,
                                       TariffID = r.TariffID,
                                       TariffDescription = r.TariffDescription
                                   };
                        if (item.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(((clsTariffDetailsVO)dgTariffList.SelectedItem).TariffDescription);

                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = "Services already Selected : " + strError.ToString();

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            _OtherServiceSelected.Add((clsTariffDetailsVO)dgTariffList.SelectedItem);
                        }
                    }
                    else
                    {
                        _OtherServiceSelected.Add((clsTariffDetailsVO)dgTariffList.SelectedItem);
                    }
                }
                else
                {
                    clsTariffDetailsVO obj;
                    if (objVO != null)
                    {
                        obj = _OtherServiceSelected.Where(z => z.TariffID == objVO.TariffID).FirstOrDefault();
                        _OtherServiceSelected.Remove(obj);
                        //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                        foreach (var item in userTariffList)
                        {
                            if (item.TariffID == obj.TariffID)
                            {
                                item.SelectTariff = false;
                            }
                        }
                        dgTariffList.ItemsSource = null;
                        dgTariffList.ItemsSource = userTariffList;
                        DataPagerDoc10.Source = null;
                        DataPagerDoc10.Source = userTariffList;
                        dgTariffList.UpdateLayout();
                    }
                    else if (objServiceVO != null)
                    {
                        obj = _OtherServiceSelected.Where(z => z.TariffID == objServiceVO.TariffID).FirstOrDefault();
                        _OtherServiceSelected.Remove(obj);
                        //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                        foreach (var item in userTariffList)
                        {
                            if (item.TariffID == obj.TariffID)
                            {
                                item.SelectTariff = false;
                            }
                        }
                        dgTariffList.ItemsSource = null;
                        dgTariffList.ItemsSource = userTariffList;
                        DataPagerDoc10.Source = null;
                        DataPagerDoc10.Source = userTariffList;
                        dgTariffList.UpdateLayout();
                    }
                }
                foreach (var item in _OtherServiceSelected)
                {
                    item.SelectTariff = true;
                }
                dgTariffSelectedList.ItemsSource = null;
                dgTariffSelectedList.ItemsSource = _OtherServiceSelected;
                dgTariffSelectedList.UpdateLayout();
                dgTariffSelectedList.Focus();
            }

        }

        #endregion Button Click Events

        #region Company Logo and Image Details
        //Added by Ashish Z.

        private void cmdBrowseLogo_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog OpenFile1 = new OpenFileDialog();
            //OpenFile1.Multiselect = false;
            //OpenFile1.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            //OpenFile1.FilterIndex = 1;
            //if (OpenFile1.ShowDialog() == true)
            //{
            //    try
            //    {
            //        BitmapImage imageSource1 = new BitmapImage();
            //        Stream stream1 = OpenFile1.File.OpenRead();
            //        BinaryReader binaryReader1 = new BinaryReader(stream1);
            //        imageSource1.SetSource(OpenFile1.File.OpenRead());
            //        LogoImage.Source = imageSource1;
            //        byte[] currentImageInBytes1 = new byte[0];
            //        currentImageInBytes1 = binaryReader1.ReadBytes((int)stream1.Length);
            //        stream1.Position = 0;
            //        imageSource1.SetSource(stream1);


            //        CompanyImage = currentImageInBytes1.DeepCopy();
            //    }
            //    catch (Exception)
            //    {
            //        HtmlPage.Window.Alert("Error Loading File");
            //    }
            //}
            try
            {
                OpenFileDialog openDialog1 = new OpenFileDialog();
                openDialog1.Multiselect = false;
                openDialog1.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
                if (openDialog1.ShowDialog() == true)
                {
                    //AttachedFileName = openDialog.File.Name;
                    //txtFileName.Text = openDialog.File.Name;
                    try
                    {
                        //using (Stream stream = openDialog.File.OpenRead())
                        //{
                        BitmapImage imageSource1 = new BitmapImage();
                        imageSource1.SetSource(openDialog1.File.OpenRead());
                        LogoImage.Source = null;
                        LogoImage.Source = imageSource1;

                        //AttachedFileContents = new byte[stream.Length];
                        //stream.Read(AttachedFileContents, 0, (int)stream.Length);

                        //}
                    }
                    catch (Exception ex)
                    {
                        string msgText = "Error while reading file.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        private void CmdBrowseFooterImage_Click(object sender, RoutedEventArgs e)
        {

            //OpenFileDialog OpenFile2 = new OpenFileDialog();
            //OpenFile2.Multiselect = false;
            //OpenFile2.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            //OpenFile2.FilterIndex = 1;
            //if (OpenFile2.ShowDialog() == true)
            //{
            //    try
            //    {
            //        BitmapImage imageSource2 = new BitmapImage();
            //        Stream stream2 = OpenFile2.File.OpenRead();
            //        BinaryReader binaryReader2 = new BinaryReader(stream2);
            //        imageSource2.SetSource(OpenFile2.File.OpenRead());
            //        FooterImage.Source = imageSource2;
            //        byte[] currentImageInBytes2 = new byte[0];
            //        currentImageInBytes2 = binaryReader2.ReadBytes((int)stream2.Length);
            //        stream2.Position = 0;
            //        imageSource2.SetSource(stream2);


            //        CompFooterImage = currentImageInBytes2.DeepCopy();
            //    }
            //    catch (Exception)
            //    {
            //        HtmlPage.Window.Alert("Error Loading File");
            //    }
            //}
            try
            {
                OpenFileDialog openDialog2 = new OpenFileDialog();
                openDialog2.Multiselect = false;
                openDialog2.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
                if (openDialog2.ShowDialog() == true)
                {
                    //AttachedfootImgFileName = openDialog.File.Name;
                    //txtFooterImageFileName.Text = openDialog.File.Name;
                    try
                    {
                        //using (Stream stream = openDialog1.File.OpenRead())
                        //{
                        BitmapImage imageSource2 = new BitmapImage();
                        imageSource2.SetSource(openDialog2.File.OpenRead());
                        FooterImage.Source = null;
                        FooterImage.Source = imageSource2;

                        //AttachedFootImgFileContents = new byte[stream.Length];
                        //stream.Read(AttachedFootImgFileContents, 0, (int)stream.Length);
                        //}
                    }
                    catch (Exception ex)
                    {
                        string msgText = "Error while reading file.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        private void CmdBrowseHeaderImage_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog OpenFile3 = new OpenFileDialog();
            //OpenFile3.Multiselect = false;
            //OpenFile3.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            //OpenFile3.FilterIndex = 1;
            //if (OpenFile3.ShowDialog() == true)
            //{
            //    try
            //    {
            //        BitmapImage imageSource3 = new BitmapImage();
            //        Stream stream3 = OpenFile3.File.OpenRead();
            //        BinaryReader binaryReader3 = new BinaryReader(stream3);
            //        imageSource3.SetSource(OpenFile3.File.OpenRead());
            //        HeaderImage.Source = imageSource3;
            //        byte[] currentImageInBytes3 = new byte[0];
            //        currentImageInBytes3 = binaryReader3.ReadBytes((int)stream3.Length);
            //        stream3.Position = 0;
            //        imageSource3.SetSource(stream3);

            //        CompHeaderImage = currentImageInBytes3.DeepCopy();
            //    }
            //    catch (Exception)
            //    {
            //        HtmlPage.Window.Alert("Error Loading File");
            //    }

            try
            {
                OpenFileDialog openDialog3 = new OpenFileDialog();
                openDialog3.Multiselect = false;
                openDialog3.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
                if (openDialog3.ShowDialog() == true)
                {
                    //AttachedHeadImgFileName = openDialog.File.Name;
                    //txtHeaderImageFileName.Text = openDialog.File.Name;
                    try
                    {
                        //using (Stream stream = openDialog3.File.OpenRead())
                        //{
                        BitmapImage imageSource3 = new BitmapImage();
                        imageSource3.SetSource(openDialog3.File.OpenRead());
                        HeaderImage.Source = null;
                        HeaderImage.Source = imageSource3;

                        //AttachedHeadImgFileContents = new byte[stream.Length];
                        //stream.Read(AttachedHeadImgFileContents, 0, (int)stream.Length);
                        //}
                    }
                    catch (Exception ex)
                    {
                        string msgText = "Error while reading file.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void hlkPreview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsCompanyVO CompanyLogoDetailsVO = this.companyLogoInfo;
                if (CompanyLogoDetailsVO.AttachedFileContent != null)
                {
                    byte[] imageBytes = CompanyLogoDetailsVO.AttachedFileContent;

                    BitmapImage img = new BitmapImage();
                    img.SetSource(new MemoryStream(imageBytes, false));
                    LogoImage.Source = img;
                }
                else
                {
                    LogoImage.Source = null;
                }



                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                //client.UploadReportFileForMLCaseCompleted += (s, args) =>
                //{
                //    if (args.Error == null)
                //    {
                //        Uri address1 = new Uri(Application.Current.Host.Source, "../PatientMLCaseDocuments");
                //        string strFileName = address1.ToString();
                //        strFileName = strFileName + "/" + CompanyLogoDetailsVO.AttachedFileName;
                //        HtmlPage.Window.Invoke("open", new object[] { strFileName, "", "" });
                //    }
                //};
                //client.UploadReportFileForMLCaseAsync(CompanyLogoDetailsVO.AttachedFileName, CompanyLogoDetailsVO.AttachedFileContent);
                //client.CloseAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void hlkPreviewFooterImage_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    clsCompanyVO CompanyLogoDetailsVO = this.companyLogoInfo;
            //    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            //    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            //    client.UploadReportFileForMLCaseCompleted += (s, args) =>
            //    {
            //        if (args.Error == null)
            //        {
            //            Uri address1 = new Uri(Application.Current.Host.Source, "../PatientMLCaseDocuments");
            //            string strFileName = address1.ToString();
            //            strFileName = strFileName + "/" + CompanyLogoDetailsVO.AttachedFootImgFileName;
            //            HtmlPage.Window.Invoke("open", new object[] { strFileName, "", "" });
            //        }
            //    };
            //    client.UploadReportFileForMLCaseAsync(CompanyLogoDetailsVO.AttachedFootImgFileName, CompanyLogoDetailsVO.AttachedFootImgFileContent);
            //    client.CloseAsync();

            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        private void hlkPreviewHeaderImage_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    clsCompanyVO CompanyLogoDetailsVO = this.companyLogoInfo;
            //    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            //    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            //    client.UploadReportFileForMLCaseCompleted += (s, args) =>
            //    {
            //        if (args.Error == null)
            //        {
            //            Uri address1 = new Uri(Application.Current.Host.Source, "../PatientMLCaseDocuments");
            //            string strFileName = address1.ToString();
            //            strFileName = strFileName + "/" + CompanyLogoDetailsVO.AttachedHeadImgFileName;
            //            HtmlPage.Window.Invoke("open", new object[] { strFileName, "", "" });
            //        }
            //    };
            //    client.UploadReportFileForMLCaseAsync(CompanyLogoDetailsVO.AttachedHeadImgFileName, CompanyLogoDetailsVO.AttachedHeadImgFileContent);
            //    client.CloseAsync();

            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
        //
        #endregion
    }
}
