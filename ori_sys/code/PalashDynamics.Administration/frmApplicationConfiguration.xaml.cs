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
using System.Windows.Browser;
using System.Reflection;
using CIMS;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Collections.ObjectModel;


namespace PalashDynamics.Administration
{
    public partial class frmApplicationConfiguration : UserControl
    {
        clsAppConfigVO myAppConfig;
        WaitIndicator Indicatior;
        ObservableCollection<clsAppConfigAutoEmailSMSVO> myAppConfigEmail = new ObservableCollection<clsAppConfigAutoEmailSMSVO>();

        public frmApplicationConfiguration()
        {
            InitializeComponent();
            myAppConfig = new clsAppConfigVO();
            myAppConfig = ((IApplicationConfiguration)App.Current).ApplicationConfigurations;
            this.DataContext = myAppConfig;
            if (myAppConfig.IsHO)
                chkAllowTransactionAtHO.IsEnabled = true;
            else
                chkAllowTransactionAtHO.IsEnabled = false;
            //cmbUnit.SelectedItem = myAppConfig.UnitID;
            FillDepartmentMaster(myAppConfig.UnitID);
            FillCashCounter(myAppConfig.UnitID);

            // InstalledFontCollection fonts = new InstalledFontCollection();

        }

        #region Variables
        public event RoutedEventHandler OnSaveButton_Click;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();

        #endregion

        #region Properties

        public byte[] AttachedFileContents { get; set; }
        byte[] data1;
        FileInfo Attachment1;
        public string fileName1;

        #endregion

        #region "Fill Combos"
        private void FillPrintFormat()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ReportPrintFormat;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }

                    cmbPrintFormat.ItemsSource = null;
                    cmbPrintFormat.ItemsSource = objList;
                    if (myAppConfig != null)
                    {
                        cmbPrintFormat.SelectedValue = myAppConfig.PrintFormatID;
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {


                throw;
            }

        }

        void FillPayMode()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ModeOfPayment;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbPayMode.ItemsSource = null;
                    cmbPayMode.ItemsSource = objList;
                    cmbPayMode.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();

        }


        private void FillRoles()
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.T_UserRoleMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {

                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);


                    }

                    cmbAdminRole.ItemsSource = null;
                    cmbAdminRole.ItemsSource = objList;

                    cmbDoctorRole.ItemsSource = null;
                    cmbDoctorRole.ItemsSource = objList;

                    cmbNurseRole.ItemsSource = null;
                    cmbNurseRole.ItemsSource = objList;

                    if (myAppConfig != null)
                    {
                        cmbAdminRole.SelectedValue = myAppConfig.AdminRoleID;
                        cmbDoctorRole.SelectedValue = myAppConfig.DoctorRoleID;
                        cmbNurseRole.SelectedValue = myAppConfig.NurseRoleID;

                    }
                    Indicatior.Close();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {

                // throw;
                Indicatior.Close();
                throw;
            }

        }

        private void FillTariff()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_TariffMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    }


                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objList;

                    if (myAppConfig != null)
                    {
                        cmbTariff.SelectedValue = myAppConfig.TariffID;
                    }
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();

                throw;
            }
        }

        private void FillCompany()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CompanyMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbSelfCompany.ItemsSource = null;
                    cmbSelfCompany.ItemsSource = objList;

                    if (myAppConfig != null)
                    {
                        cmbSelfCompany.SelectedValue = myAppConfig.SelfCompanyID;
                    }
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();

                throw;
            }
        }


        private void FillItemScrapCategoty()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ItemCategory;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbItemScrapCategory.ItemsSource = null;
                    cmbItemScrapCategory.ItemsSource = objList;

                    if (myAppConfig != null)
                    {
                        cmbItemScrapCategory.SelectedValue = myAppConfig.Accounts.ItemScrapCategory;
                    }
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();

                throw;
            }
        }


        private void FillRadiologyStore()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Store;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbRadiologyStore.ItemsSource = null;
                    cmbRadiologyStore.ItemsSource = objList;

                    cmbPathologyStore.ItemsSource = null;
                    cmbPathologyStore.ItemsSource = objList;

                    cmbOTStore.ItemsSource = null;
                    cmbOTStore.ItemsSource = objList;

                    cmbPharmacyStore.ItemsSource = null;
                    cmbPharmacyStore.ItemsSource = objList;

                    cmbIndentStore.ItemsSource = null;
                    cmbIndentStore.ItemsSource = objList;

                    if (myAppConfig != null)
                    {
                        cmbRadiologyStore.SelectedValue = myAppConfig.RadiologyStoreID;
                        cmbPathologyStore.SelectedValue = myAppConfig.PathologyStoreID;
                        cmbOTStore.SelectedValue = myAppConfig.OTStoreID;
                        cmbPharmacyStore.SelectedValue = myAppConfig.PharmacyStoreID;
                    }
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();

                throw;
            }
        }



        public enum PaymentTransactionType
        {
            None = 0,
            SelfPayment = 1,
            CompanyPayment = 2,
            AdvancePayment = 3,
            RefundPayment = 4

        }
        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();

        }
        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList, PaymentTransactionType sTransactionType)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.AdvancePayment || (PaymentTransactionType)sTransactionType == PaymentTransactionType.RefundPayment)
                {
                    if ((MaterPayModeList)Value == MaterPayModeList.PatientAdvance || (MaterPayModeList)Value == MaterPayModeList.CompanyAdvance ||
                         (MaterPayModeList)Value == MaterPayModeList.StaffFree)
                        break;
                }
                else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.SelfPayment)
                {
                    if ((MaterPayModeList)Value == MaterPayModeList.CompanyAdvance || (MaterPayModeList)Value == MaterPayModeList.PatientAdvance)
                        break;
                }
                else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.CompanyPayment)
                {
                    if ((MaterPayModeList)Value == MaterPayModeList.PatientAdvance || (MaterPayModeList)Value == MaterPayModeList.CompanyAdvance)
                        break;
                }
                else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.None)
                {
                    //Do Nothing
                }
                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);


                TheMasterList.Add(Item);
            }
        }

        private void FillPaymentMode()
        {
            try
            {
                List<MasterListItem> mlPaymode = new List<MasterListItem>();
                MasterListItem Default = new MasterListItem(0, "- Select -");
                mlPaymode.Insert(0, Default);
                EnumToList(typeof(MaterPayModeList), mlPaymode, PaymentTransactionType.None);
                cmbPaymentMode.ItemsSource = mlPaymode;
                // cmbPayMode.SelectedItem = Default;
                cmbPaymentMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;
            }
            catch (Exception)
            {
                ///Indicatior.Close();
                throw;
            }
        }

        private void FillUnit()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbUnit.ItemsSource = null;
                    cmbUnit.ItemsSource = objList;

                    if (myAppConfig != null)
                    {
                        cmbUnit.SelectedValue = myAppConfig.UnitID;
                        FillDepartmentMaster(myAppConfig.UnitID);
                        FillCashCounter(myAppConfig.UnitID);
                    }
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
                // FillCashCounter();
            }
            catch (Exception)
            {
                ///Indicatior.Close();
                throw;
            }
        }

        private void FillDepartmentMaster(long IUnitID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

                if (IUnitID > 0)
                    BizAction.Parent = new KeyValue { Key = IUnitID, Value = "UnitId" };
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;

                    cmbPathologyDepartment.ItemsSource = null;
                    cmbPathologyDepartment.ItemsSource = objList.DeepCopy();

                    cmbRadiologyDepartment.ItemsSource = null;
                    cmbRadiologyDepartment.ItemsSource = objList.DeepCopy();

                    if (myAppConfig != null)
                    {
                        cmbPathologyDepartment.SelectedValue = myAppConfig.PathologyDepartmentID;   //Set Department For Pathology
                        cmbRadiologyDepartment.SelectedValue = myAppConfig.RadiologyDepartmentID;   //Set Department For Radiology

                        cmbDepartment.SelectedValue = myAppConfig.DepartmentID;
                        FillDoctorList(IUnitID, myAppConfig.DepartmentID);
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                ///Indicatior.Close();

                throw;
            }

        }

        private void FillDoctorList(long iUnitID, long iDeptID)
        {
            try
            {
                clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                BizAction.UnitId = iUnitID;

                if ((MasterListItem)cmbDepartment.SelectedItem != null)
                {
                    BizAction.DepartmentId = iDeptID;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);
                    }
                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;

                    if (myAppConfig != null)
                    {
                        cmbDoctor.SelectedValue = myAppConfig.DoctorID;
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                ///Indicatior.Close();
                throw;
            }

        }

        private void FillDoctor()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_DepartmentMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        cmbDepartment.ItemsSource = null;
                        cmbDepartment.ItemsSource = objList;
                    }

                    if (myAppConfig != null)
                    {
                        cmbDepartment.SelectedValue = myAppConfig.SelfCompanyID;
                    }
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();

                throw;
            }
        }

        private void FillPathCompayType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CompanyTypeMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        cmbPathologyCompanyType.ItemsSource = null;
                        cmbPathologyCompanyType.ItemsSource = objList;
                    }

                    if (myAppConfig != null)
                    {
                        cmbPathologyCompanyType.SelectedValue = myAppConfig.PathologyCompanyTypeID;
                    }
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();

                throw;
            }
        }


        private void FillVisitType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_VisitTypeMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        cmbVisitType.ItemsSource = null;
                        cmbVisitType.ItemsSource = objList;

                        cmbFreeFollowupVisittype.ItemsSource = null;
                        cmbFreeFollowupVisittype.ItemsSource = objList;

                        cmbPharmacyVisitType.ItemsSource = null;
                        cmbPharmacyVisitType.ItemsSource = objList;

                        //added by rohini dated 11.4.16 as per disscussion with Pranav sir

                        cmbPathologyVisitType.ItemsSource = null;
                        cmbPathologyVisitType.ItemsSource = objList;

                    }

                    if (myAppConfig != null)
                    {
                        cmbVisitType.SelectedValue = myAppConfig.VisitTypeID;
                        cmbFreeFollowupVisittype.SelectedValue = myAppConfig.FreeFollowupVisitTypeID;
                        cmbPharmacyVisitType.SelectedValue = myAppConfig.PharmacyVisitTypeID;
                        cmbPathologyVisitType.SelectedValue = myAppConfig.PathologyVisitTypeID;
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                ///Indicatior.Close();
                throw;
            }
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
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }

                    cmbPathoSpecialization.ItemsSource = null;
                    cmbPathoSpecialization.ItemsSource = objList.DeepCopy();

                    cmbRadiologySpecialization.ItemsSource = null;
                    cmbRadiologySpecialization.ItemsSource = objList.DeepCopy();

                    cmbPharmacySpecialization.ItemsSource = null;
                    cmbPharmacySpecialization.ItemsSource = objList.DeepCopy();

                    cmbConsultationSpecialization.ItemsSource = null;
                    cmbConsultationSpecialization.ItemsSource = objList.DeepCopy();


                    if (myAppConfig != null)
                    {
                        cmbPathoSpecialization.SelectedValue = myAppConfig.PathoSpecializationID;
                        cmbRadiologySpecialization.SelectedValue = myAppConfig.RadiologySpecializationID;
                        cmbPharmacySpecialization.SelectedValue = myAppConfig.PharmacySpecializationID;
                        cmbConsultationSpecialization.SelectedValue = myAppConfig.ConsultationID;
                    }
                };


                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillPatientCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PatientCategoryMaster;
                BizAction.IsActive = true;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    }

                    cmbPatientCategory.ItemsSource = null;
                    cmbPatientCategory.ItemsSource = objList;
                    cmbPharmacyPatientCategory.ItemsSource = null;
                    cmbPharmacyPatientCategory.ItemsSource = objList;


                    if (myAppConfig != null)
                    {
                        cmbPatientCategory.SelectedValue = myAppConfig.PatientCategoryID;
                        cmbPharmacyPatientCategory.SelectedValue = myAppConfig.PharmacyPatientCategoryID;

                    }
                };


                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CategoryL1Master;
                BizAction.IsActive = true;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    }

                    cmbCategoryL.ItemsSource = null;
                    cmbCategoryL.ItemsSource = objList;


                    if (myAppConfig != null)
                    {
                        cmbCategoryL.SelectedValue = myAppConfig.CategoryID;

                    }
                    else
                    {
                        cmbCategoryL.SelectedValue = objList[0];
                    }
                };


                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillRelationMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_RelationMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    }

                    cmbRelation.ItemsSource = null;
                    cmbRelation.ItemsSource = objList;

                    if (myAppConfig != null)
                    {
                        cmbRelation.SelectedValue = myAppConfig.SelfRelationID;

                    }
                };


                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillEmailRelatedComboBox()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.T_EmailTemplate;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    }
                    cmdEmailTemplate.ItemsSource = null;
                    cmdEmailTemplate.ItemsSource = objList;
                    cmdEmailTemplate.SelectedValue = (long)0;

                    if (myAppConfig != null)
                    {
                        cmbPatientCategory.SelectedValue = myAppConfig.PatientCategoryID;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillSMSRelatedTemplateComboBox()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.T_SMSTemplate;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmdSMSTemplate.ItemsSource = null;
                    cmdSMSTemplate.ItemsSource = objList;
                    cmdSMSTemplate.SelectedValue = (long)0;


                    if (myAppConfig != null)
                    {
                        cmbPatientCategory.SelectedValue = myAppConfig.PatientCategoryID;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillPatientSource()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PatientSourceMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }

                    cmbPatientSource.ItemsSource = null;
                    cmbPatientSource.ItemsSource = objList.DeepCopy();

                    cmbCompanyPatientSource.ItemsSource = null;
                    cmbCompanyPatientSource.ItemsSource = objList.DeepCopy();

                    //cmbPackagePatientSource.ItemsSource = null;
                    //cmbPackagePatientSource.ItemsSource = objList.DeepCopy();

                    if (myAppConfig != null)
                    {
                        cmbPatientSource.SelectedValue = myAppConfig.PatientSourceID;
                        cmbCompanyPatientSource.SelectedValue = myAppConfig.CompanyPatientSourceID;
                        // cmbPackagePatientSource.SelectedValue = myAppConfig.PackagePatientSourceID;
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillFontCombo()
        {
            //     string path = "C:\Windows\Fonts";
            //     string[] ReturnValue;

            //ReturnValue = Directory.GetFiles(path);
            //     DirectoryInfo di = new DirectoryInfo(@"C:\Windows\Fonts");
            //     //FileInfo [] fontInfo = di.
            //     string[] files = Directory.(@"C:\Windows\Fonts");  

        }

        private void FillBank()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_BankMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    //cmbMaritalStatus.ItemsSource = null;
                    //cmbMaritalStatus.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    List<MasterListItem> objList = new List<MasterListItem>();

                    // objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbChequeDDBank.ItemsSource = null;
                    cmbChequeDDBank.ItemsSource = objList.DeepCopy();

                    cmbDebitCreditBankName.ItemsSource = null;
                    cmbDebitCreditBankName.ItemsSource = objList.DeepCopy();

                    cmbChequeDDBank.SelectedValue = myAppConfig.Accounts.ChequeDDBankID;
                    cmbDebitCreditBankName.SelectedValue = myAppConfig.Accounts.CrDBBankID;

                }


            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void fillEmbryologistAndAnesthetist()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Classification;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }

                    cmbEmbryologistClassification.ItemsSource = null;
                    cmbEmbryologistClassification.ItemsSource = objList.DeepCopy();

                    cmbAnesthetistClassification.ItemsSource = null;
                    cmbAnesthetistClassification.ItemsSource = objList.DeepCopy();

                    cmbRadiologist.ItemsSource = null;
                    cmbRadiologist.ItemsSource = objList.DeepCopy();

                    cmbGynecologist.ItemsSource = null;
                    cmbGynecologist.ItemsSource = objList.DeepCopy();

                    cmbPhysician.ItemsSource = null;
                    cmbPhysician.ItemsSource = objList.DeepCopy();

                    cmbAndrologist.ItemsSource = null;
                    cmbAndrologist.ItemsSource = objList.DeepCopy();

                    cmbBiologist.ItemsSource = null;
                    cmbBiologist.ItemsSource = objList.DeepCopy();

                    // creating Pathologist for MFC on 25.05.2016
                    cmbPathologist.ItemsSource = null;
                    cmbPathologist.ItemsSource = objList.DeepCopy();

                    if (myAppConfig != null)
                    {
                        cmbEmbryologistClassification.SelectedValue = myAppConfig.EmbryologistID;
                        cmbAnesthetistClassification.SelectedValue = myAppConfig.AnesthetistID;
                        cmbRadiologist.SelectedValue = myAppConfig.RadiologistID;
                        cmbGynecologist.SelectedValue = myAppConfig.GynecologistID;
                        cmbPhysician.SelectedValue = myAppConfig.PhysicianID;
                        cmbAndrologist.SelectedValue = myAppConfig.AndrologistID;
                        cmbBiologist.SelectedValue = myAppConfig.BiologistID;
                        cmbPathologist.SelectedValue = myAppConfig.PathologistID;
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillDateFormat()
        {
            List<MasterListItem> lstDateFormat = new List<MasterListItem>();
            lstDateFormat.Add(new MasterListItem() { ID = 0, Description = "--Select--", Status = true });
            lstDateFormat.Add(new MasterListItem() { ID = 1, Description = "dd/MM/yyyy", Status = true });
            lstDateFormat.Add(new MasterListItem() { ID = 2, Description = "dd-MMM-yy", Status = true });
            lstDateFormat.Add(new MasterListItem() { ID = 3, Description = "M/d/yyyy", Status = true });
            cmbDateFormat.ItemsSource = lstDateFormat;

            cmbDateFormat.SelectedItem = lstDateFormat[0];

            if (this.DataContext != null)
            {
                cmbDateFormat.SelectedValue = myAppConfig.DateFormatID;
            }
        }

        private void FillLab()
        {
            try
            {

                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_LaboratoryMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbLab.ItemsSource = null;
                        cmbLab.ItemsSource = objList.DeepCopy();
                        cmbLab.SelectedItem = objList[0];


                    }
                    if (myAppConfig != null)
                    {
                        cmbLab.SelectedValue = myAppConfig.InhouseLabID;

                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        private void FillPlannedTreatment()
        {
            try
            {


                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_IVFPlannedTreatment;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbOocyteDonotion.ItemsSource = null;
                        cmbOocyteDonotion.ItemsSource = objList.DeepCopy();
                        cmbOocyteDonotion.SelectedItem = objList[0];

                        cmbEmbryoReceipent.ItemsSource = objList.DeepCopy();
                        cmbEmbryoReceipent.SelectedItem = objList[0];

                        cmbOocyteReceipent.ItemsSource = objList.DeepCopy();
                        cmbOocyteReceipent.SelectedItem = objList[0];


                    }
                    if (myAppConfig != null)
                    {
                        cmbOocyteDonotion.SelectedValue = myAppConfig.OocyteDonationID;
                        cmbEmbryoReceipent.SelectedValue = myAppConfig.EmbryoReceipentID;
                        cmbOocyteReceipent.SelectedValue = myAppConfig.OocyteReceipentID;

                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private void FillDoctorType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DoctorTypeMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbDoctorType.ItemsSource = null;
                    cmbDoctorType.ItemsSource = objList;
                    cmbDoctorType.SelectedItem = objList[0];
                }
                if (myAppConfig != null)
                {
                    cmbDoctorType.SelectedValue = myAppConfig.DoctorTypeForReferral;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillIdentity()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IdentityMaster;
            BizAction.IsActive = true;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbIdentity.ItemsSource = null;
                    cmbIdentity.ItemsSource = objList;
                    cmbIdentity.SelectedItem = objList[0];

                }
                if (myAppConfig != null)
                {
                    cmbIdentity.SelectedValue = myAppConfig.IdentityForInternationalPatient;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillLevelsAuthorization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_RequestApprovalMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbForRefund.ItemsSource = null;
                    cmbForRefund.ItemsSource = objList;
                    cmbForRefund.SelectedItem = objList[0];

                    cmbForConcession.ItemsSource = null;
                    cmbForConcession.ItemsSource = objList;
                    cmbForConcession.SelectedItem = objList[0];

                    cmbForMRPAdjustment.ItemsSource = null;
                    cmbForMRPAdjustment.ItemsSource = objList;
                    cmbForMRPAdjustment.SelectedItem = objList[0];
                }
                if (myAppConfig != null)
                {
                    cmbForRefund.SelectedValue = myAppConfig.AuthorizationLevelForRefundID;
                    cmbForConcession.SelectedValue = myAppConfig.AuthorizationLevelForConcessionID;
                    cmbForMRPAdjustment.SelectedValue = myAppConfig.AuthorizationLevelForMRPAdjustmentID;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        #region Costing Divisions for Clinical & Pharmacy Billing

        //private void FillCostingDivisions()
        //{
        //    try
        //    {
        //        //Indicatior = new WaitIndicator();
        //        //Indicatior.Show();

        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        //BizAction.IsActive = true;
        //        BizAction.MasterTable = MasterTableNameList.M_IVFCostingDivision;
        //        BizAction.MasterList = new List<MasterListItem>();

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, e) =>
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "- Select -"));

        //            if (e.Error == null && e.Result != null)
        //            {

        //                objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);


        //            }

        //            cmbClinicalCostingDivision.ItemsSource = null;
        //            cmbClinicalCostingDivision.ItemsSource = objList.DeepCopy();

        //            cmbPharmacyCostingDivision.ItemsSource = null;
        //            cmbPharmacyCostingDivision.ItemsSource = objList.DeepCopy();

        //            if (myAppConfig != null)
        //            {
        //                cmbClinicalCostingDivision.SelectedValue = myAppConfig.ClinicalCostingDivisionID;
        //                cmbPharmacyCostingDivision.SelectedValue = myAppConfig.PharmacyCostingDivisionID;

        //            }
        //            //Indicatior.Close();
        //        };

        //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        Client.CloseAsync();

        //    }
        //    catch (Exception)
        //    {

        //        // throw;
        //        Indicatior.Close();
        //        throw;
        //    }

        //}


        private void FillCashCounter(long IUnitID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CashCounterMaster;
                BizAction.MasterList = new List<MasterListItem>();
                if (IUnitID > 0)
                    BizAction.Parent = new KeyValue { Key = IUnitID, Value = "ClinicID" };
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    if (e.Error == null && e.Result != null)
                    {
                        // List<MasterListItem> objList = new List<MasterListItem>();
                        //  objList.Add(new MasterListItem(0, "- Select -"));
                        // objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    }
                    cmbOPDCounter.ItemsSource = null;
                    cmbOPDCounter.ItemsSource = objList;

                    cmbIPDCounter.ItemsSource = null;
                    cmbIPDCounter.ItemsSource = objList.DeepCopy();

                    cmbPharmacyCounter.ItemsSource = null;
                    cmbPharmacyCounter.ItemsSource = objList.DeepCopy();

                    cmbLabCounter.ItemsSource = null;
                    cmbLabCounter.ItemsSource = objList.DeepCopy();

                    cmbRadiologyCounter.ItemsSource = null;
                    cmbRadiologyCounter.ItemsSource = objList.DeepCopy();

                    if (myAppConfig != null)
                    {
                        cmbOPDCounter.SelectedValue = myAppConfig.OPDCounterID;   //Set Department For Pathology
                        cmbIPDCounter.SelectedValue = myAppConfig.IPDCounterID;
                        cmbPharmacyCounter.SelectedValue = myAppConfig.PharmacyCounterID;
                        cmbLabCounter.SelectedValue = myAppConfig.LabCounterID;
                        cmbRadiologyCounter.SelectedValue = myAppConfig.RadiologyCounterID;

                    }

                };
                Client.ProcessAsync(BizAction, App.SessionUser);
                Client.CloseAsync();

            }

                //try
            //{
            //    clsGetCounterDetailsBizActionVO BizAction = new clsGetCounterDetailsBizActionVO();
            //    BizAction.CounterDetails = new List<clsCounterVO>();
            //    BizAction.ClinicID = ((MasterListItem)cmbUnit.SelectedValue).ID; 

                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                //    client.ProcessCompleted += (s, e) =>
            //    {
            //        List<clsCounterVO> objList = new List<clsCounterVO>();
            //        //objList.Add(new clsCounterVO(0, "- Select -"));

                //        //if (e.Error == null && e.Result != null)
            //        //{
            //        //    objList.AddRange(((clsGetCounterDetailsBizActionVO)e.Result).CounterDetails);
            //        //}
            //        cmbCashCounter.ItemsSource = null;
            //        cmbCashCounter.ItemsSource = objList;

                //        if (myAppConfig != null)
            //        {
            //            cmbCashCounter.SelectedValue = myAppConfig.DefaultCounterID;
            //        }
            //    };

                //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    client.CloseAsync();
            //}
            catch (Exception)
            {
                ///Indicatior.Close();
                throw;
            }

        }
        # endregion

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //myAppConfig.State = "MH";

            if (myAppConfig.IsHO == false)
            {
                cmbUnit.IsEnabled = false;
                chkIsHO.IsEnabled = false;
            }
            else
                cmbUnit.IsEnabled = true;
            List<MasterListItem> objList = new List<MasterListItem>();
            MasterListItem objM = new MasterListItem(0, "-- Select --");
            objList.Add(objM);
            txtCountry.ItemsSource = objList;
            txtCountry.SelectedItem = objM;
            txtState.ItemsSource = objList;
            txtState.SelectedItem = objM;
            txtCity.ItemsSource = objList;
            txtCity.SelectedItem = objM;
            txtArea.ItemsSource = objList;
            txtArea.SelectedItem = objM;

            FillPrintFormat();
            FillRoles();
            FillTariff();
            FillCompany();
            FillPaymentMode();
            FillUnit();
            FillVisitType();
            FillSpecialization();
            FillPatientCategory();
            FillCategory();
            FillRelationMaster();
            FillPatientSource();
            FillEmailRelatedComboBox();
            FillSMSRelatedTemplateComboBox();
            FillBank();
            FillItemScrapCategoty();
            FillRadiologyStore();
            fillEmbryologistAndAnesthetist();
            FillDateFormat();
            FillPayMode();
            FillLab();
            FillPlannedTreatment();
            FillDoctorType();
            FillIdentity();
            FillCountry();
            FillPathCompayType();
            // BY bHUSHAN..
            FillEmailSMSEvent();
            FillLevelsAuthorization();

            //Costing Divisions for Clinical & Pharmacy Billing
            //FillCostingDivisions();
            //FillCashCounter();

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

            if (IsValid())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Application Configuration ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }

        }


        private bool IsValid()
        {
            if (txtAppointmentSlot.Text != "")
            {
                if (Convert.ToInt16(txtAppointmentSlot.Text) < 5)
                {
                    txtAppointmentSlot.SetValidation("Appointment Slot can not be less than 5");
                    txtAppointmentSlot.RaiseValidationError();
                    txtAppointmentSlot.Focus();
                    return false;
                }
                else
                {
                    txtAppointmentSlot.ClearValidationError();
                }


            }

            return true;
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }

        private void Save()
        {
            try
            {
                WaitIndicator Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsUpdateAppConfigBizActionVO BizAction = new clsUpdateAppConfigBizActionVO();
                BizAction.AppConfig = myAppConfig;

                //rohinee

                if (cmbOPDCounter.SelectedItem != null)
                {
                    BizAction.AppConfig.OPDCounterID = ((MasterListItem)cmbOPDCounter.SelectedItem).ID;
                }
                if (cmbIPDCounter.SelectedItem != null)
                {
                    BizAction.AppConfig.IPDCounterID = ((MasterListItem)cmbIPDCounter.SelectedItem).ID;
                }

                if (cmbPharmacyCounter.SelectedItem != null)
                {
                    BizAction.AppConfig.PharmacyCounterID = ((MasterListItem)cmbPharmacyCounter.SelectedItem).ID;
                }

                if (cmbLabCounter.SelectedItem != null)
                {
                    BizAction.AppConfig.LabCounterID = ((MasterListItem)cmbLabCounter.SelectedItem).ID;
                }

                if (cmbRadiologyCounter.SelectedItem != null)
                {
                    BizAction.AppConfig.RadiologyCounterID = ((MasterListItem)cmbRadiologyCounter.SelectedItem).ID;
                }

                if (cmbCategoryL.SelectedItem != null)
                {
                    BizAction.AppConfig.CategoryID = ((MasterListItem)cmbCategoryL.SelectedItem).ID;
                }
                //

                //added by neena
                if (txtWebSite.Text != null)
                {
                    BizAction.AppConfig.WebSite = txtWebSite.Text;
                }
                //


                // By BHUSHAN ..
                BizAction.AppEmail = myAppConfigEmail.ToList();
                if (cmbPrintFormat.SelectedItem != null)
                {
                    BizAction.AppConfig.PrintFormatID = ((MasterListItem)cmbPrintFormat.SelectedItem).ID;
                }

                if (cmbAdminRole.SelectedItem != null)
                {
                    BizAction.AppConfig.AdminRoleID = ((MasterListItem)cmbAdminRole.SelectedItem).ID;
                }
                if (cmbDoctorRole.SelectedItem != null)
                {
                    BizAction.AppConfig.DoctorRoleID = ((MasterListItem)cmbDoctorRole.SelectedItem).ID;
                }
                if (chkAllowDischargeReq.IsChecked == true)
                {
                    BizAction.AppConfig.IsAllowDischargeRequest = true;
                }
                else
                {
                    BizAction.AppConfig.IsAllowDischargeRequest = false;
                }
                if (cmbNurseRole.SelectedItem != null)
                {
                    BizAction.AppConfig.NurseRoleID = ((MasterListItem)cmbNurseRole.SelectedItem).ID;
                }
                if (cmbTariff.SelectedItem != null)
                {
                    BizAction.AppConfig.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                }

                if (cmbRadiologyStore.SelectedItem != null)
                {
                    BizAction.AppConfig.RadiologyStoreID = ((MasterListItem)cmbRadiologyStore.SelectedItem).ID;
                }

                if (cmbPathologyStore.SelectedItem != null)
                {
                    BizAction.AppConfig.PathologyStoreID = ((MasterListItem)cmbPathologyStore.SelectedItem).ID;
                }

                if (cmbOTStore.SelectedItem != null)
                {
                    BizAction.AppConfig.OTStoreID = ((MasterListItem)cmbOTStore.SelectedItem).ID;
                }

                if (cmbPharmacyStore.SelectedItem != null)
                {
                    BizAction.AppConfig.PharmacyStoreID = ((MasterListItem)cmbPharmacyStore.SelectedItem).ID;
                }

                if (cmbSelfCompany.SelectedItem != null)
                {
                    BizAction.AppConfig.SelfCompanyID = ((MasterListItem)cmbSelfCompany.SelectedItem).ID;
                }

                if (cmbPaymentMode.SelectedItem != null)
                {
                    BizAction.AppConfig.PaymentModeID = ((MasterListItem)cmbPaymentMode.SelectedItem).ID;
                }

                if (txtRefAmt.Text != null)
                {
                    BizAction.AppConfig.RefundAmount = Convert.ToDouble(txtRefAmt.Text);
                }

                if (cmbPayMode.SelectedItem != null)
                {
                    BizAction.AppConfig.RefundPayModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
                }

                if (cmbUnit.SelectedItem != null)
                {
                    BizAction.AppConfig.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                }

                if (cmbDepartment.SelectedItem != null)
                {
                    BizAction.AppConfig.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
                }

                if (cmbDoctor.SelectedItem != null)
                {
                    BizAction.AppConfig.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                }
                if (cmbVisitType.SelectedItem != null)
                {
                    BizAction.AppConfig.VisitTypeID = ((MasterListItem)cmbVisitType.SelectedItem).ID;
                }
                if (cmbFreeFollowupVisittype.SelectedItem != null)
                {
                    BizAction.AppConfig.FreeFollowupVisitTypeID = ((MasterListItem)cmbFreeFollowupVisittype.SelectedItem).ID;
                }
                if (cmbPharmacyVisitType.SelectedItem != null)
                {
                    BizAction.AppConfig.PharmacyVisitTypeID = ((MasterListItem)cmbPharmacyVisitType.SelectedItem).ID;
                }
                //added by rohini dated 11.2.16
                if (cmbPathologyVisitType.SelectedItem != null)
                {
                    BizAction.AppConfig.PathologyVisitTypeID = ((MasterListItem)cmbPathologyVisitType.SelectedItem).ID;
                }
                if (cmbPathologyCompanyType.SelectedItem != null)
                {
                    BizAction.AppConfig.PathologyCompanyTypeID = ((MasterListItem)cmbPathologyCompanyType.SelectedItem).ID;
                }
                //
                if (cmbPathoSpecialization.SelectedItem != null)
                {
                    BizAction.AppConfig.PathoSpecializationID = ((MasterListItem)cmbPathoSpecialization.SelectedItem).ID;
                }
                if (cmbRadiologySpecialization.SelectedItem != null)
                {
                    BizAction.AppConfig.RadiologySpecializationID = ((MasterListItem)cmbRadiologySpecialization.SelectedItem).ID;
                }
                if (cmbPatientCategory.SelectedItem != null)
                {
                    BizAction.AppConfig.PatientCategoryID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                }
                if (cmbPharmacyPatientCategory.SelectedItem != null)
                {
                    BizAction.AppConfig.PharmacyPatientCategoryID = ((MasterListItem)cmbPharmacyPatientCategory.SelectedItem).ID;
                }
                if (cmbRelation.SelectedItem != null)
                {
                    BizAction.AppConfig.SelfRelationID = ((MasterListItem)cmbRelation.SelectedItem).ID;
                }
                if (cmbPatientSource.SelectedItem != null)
                {
                    BizAction.AppConfig.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
                }
                if (cmbCompanyPatientSource.SelectedItem != null)
                {
                    BizAction.AppConfig.CompanyPatientSourceID = ((MasterListItem)cmbCompanyPatientSource.SelectedItem).ID;
                }
                //if (cmbPackagePatientSource.SelectedItem != null)
                //{
                //    BizAction.AppConfig.PackagePatientSourceID = ((MasterListItem)cmbPackagePatientSource.SelectedItem).ID;
                //}


                if (cmbConsultationSpecialization.SelectedItem != null)
                {
                    BizAction.AppConfig.ConsultationID = ((MasterListItem)cmbConsultationSpecialization.SelectedItem).ID;
                }
                if (cmbPharmacySpecialization.SelectedItem != null)
                {
                    BizAction.AppConfig.PharmacySpecializationID = ((MasterListItem)cmbPharmacySpecialization.SelectedItem).ID;
                }

                if (cmbChequeDDBank.SelectedItem != null)
                {
                    BizAction.AppConfig.Accounts.ChequeDDBankID = ((MasterListItem)cmbChequeDDBank.SelectedItem).ID;
                }

                if (cmbDebitCreditBankName.SelectedItem != null)
                {
                    BizAction.AppConfig.Accounts.CrDBBankID = ((MasterListItem)cmbDebitCreditBankName.SelectedItem).ID;
                }

                if (cmbItemScrapCategory.SelectedItem != null)
                {
                    BizAction.AppConfig.Accounts.ItemScrapCategory = ((MasterListItem)cmbItemScrapCategory.SelectedItem).ID;
                }
                if (cmbEmbryologistClassification.SelectedItem != null)
                {
                    BizAction.AppConfig.EmbryologistID = ((MasterListItem)cmbEmbryologistClassification.SelectedItem).ID;
                }
                if (cmbAnesthetistClassification.SelectedItem != null)
                {
                    BizAction.AppConfig.AnesthetistID = ((MasterListItem)cmbAnesthetistClassification.SelectedItem).ID;
                }

                //Saily P

                if (cmbRadiologist.SelectedItem != null)
                {
                    BizAction.AppConfig.RadiologistID = ((MasterListItem)cmbRadiologist.SelectedItem).ID;
                }
                if (cmbGynecologist.SelectedItem != null)
                {
                    BizAction.AppConfig.GynecologistID = ((MasterListItem)cmbGynecologist.SelectedItem).ID;
                }
                if (cmbPhysician.SelectedItem != null)
                {
                    BizAction.AppConfig.PhysicianID = ((MasterListItem)cmbPhysician.SelectedItem).ID;
                }
                if (cmbAndrologist.SelectedItem != null)
                {
                    BizAction.AppConfig.AndrologistID = ((MasterListItem)cmbAndrologist.SelectedItem).ID;
                }
                if (cmbBiologist.SelectedItem != null)
                {
                    BizAction.AppConfig.BiologistID = ((MasterListItem)cmbBiologist.SelectedItem).ID;
                }
                //
                //if (cmbPatLoyalty.SelectedItem != null)
                //    BizAction.AppConfig.EmailConfig.PatLoyaltyCard = ((MasterListItem)cmbPatLoyalty.SelectedItem).ID;
                //if (cmbPatLoyaltySMS.SelectedItem != null)
                //    BizAction.AppConfig.EmailConfig.PatLoyaltyCardSMS = ((MasterListItem)cmbPatLoyaltySMS.SelectedItem).ID;
                //if (cmbLoyaltyCardExpiry.SelectedItem != null)
                //    BizAction.AppConfig.EmailConfig.LoyaltyCardExpiry = ((MasterListItem)cmbLoyaltyCardExpiry.SelectedItem).ID;
                //if (cmbLoyaltyCardExpirySMS.SelectedItem != null)
                //    BizAction.AppConfig.EmailConfig.LoyaltyCardExpirySMS = ((MasterListItem)cmbLoyaltyCardExpirySMS.SelectedItem).ID;


                if (cmbDateFormat.SelectedItem != null)
                {
                    BizAction.AppConfig.DateFormatID = ((MasterListItem)cmbDateFormat.SelectedItem).ID;
                }

                //if (cmbClinicalCostingDivision.SelectedItem != null)
                //{
                //    BizAction.AppConfig.ClinicalCostingDivisionID = ((MasterListItem)cmbClinicalCostingDivision.SelectedItem).ID;  //Costing Divisions for Clinical Billing
                //}

                //if (cmbPharmacyCostingDivision.SelectedItem != null)
                //{
                //    BizAction.AppConfig.PharmacyCostingDivisionID = ((MasterListItem)cmbPharmacyCostingDivision.SelectedItem).ID;  //Costing Divisions for Pharmacy Billing
                //}

                if (cmbPathologyDepartment.SelectedItem != null)
                {
                    BizAction.AppConfig.PathologyDepartmentID = ((MasterListItem)cmbPathologyDepartment.SelectedItem).ID;
                }

                if (cmbRadiologyDepartment.SelectedItem != null)
                {
                    BizAction.AppConfig.RadiologyDepartmentID = ((MasterListItem)cmbRadiologyDepartment.SelectedItem).ID;
                }
                if (cmbLab.SelectedItem != null)
                {
                    BizAction.AppConfig.InhouseLabID = ((MasterListItem)cmbLab.SelectedItem).ID;
                }
                if (cmbOocyteDonotion.SelectedItem != null)
                {
                    BizAction.AppConfig.OocyteDonationID = ((MasterListItem)cmbOocyteDonotion.SelectedItem).ID;
                }
                if (cmbEmbryoReceipent.SelectedItem != null)
                {
                    BizAction.AppConfig.EmbryoReceipentID = ((MasterListItem)cmbEmbryoReceipent.SelectedItem).ID;
                }
                if (cmbOocyteReceipent.SelectedItem != null)
                {
                    BizAction.AppConfig.OocyteReceipentID = ((MasterListItem)cmbOocyteReceipent.SelectedItem).ID;
                }

                if (cmbDoctorType.SelectedItem != null)
                {
                    BizAction.AppConfig.DoctorTypeForReferral = ((MasterListItem)cmbDoctorType.SelectedItem).ID;
                }
                if (cmbIdentity.SelectedItem != null)
                {
                    BizAction.AppConfig.IdentityForInternationalPatient = ((MasterListItem)cmbIdentity.SelectedItem).ID;
                }

                if (cmbForRefund.SelectedItem != null)
                {
                    BizAction.AppConfig.AuthorizationLevelForRefundID = ((MasterListItem)cmbForRefund.SelectedItem).ID;
                }
                if (cmbForConcession.SelectedItem != null)
                {
                    BizAction.AppConfig.AuthorizationLevelForConcessionID = ((MasterListItem)cmbForConcession.SelectedItem).ID;
                }

                if (cmbForMRPAdjustment.SelectedItem != null)
                {
                    BizAction.AppConfig.AuthorizationLevelForMRPAdjustmentID = ((MasterListItem)cmbForMRPAdjustment.SelectedItem).ID;
                }


                // creating Pathologist for MFC on 25.05.2016
                if (cmbPathologist.SelectedItem != null)
                {
                    BizAction.AppConfig.PathologistID = ((MasterListItem)cmbPathologist.SelectedItem).ID;
                }
                //

                if (chkIpd.IsChecked == true)
                {
                    BizAction.AppConfig.IsIPD = true;
                    BizAction.AppConfig.CreditLimitIPD = Convert.ToInt64(txtIpdCredit.Text);
                }
                else
                {
                    BizAction.AppConfig.IsIPD = false;
                    BizAction.AppConfig.CreditLimitIPD = 0;
                }
                if (chkOpd.IsChecked == true)
                {
                    BizAction.AppConfig.IsOPD = true;
                    BizAction.AppConfig.CreditLimitOPD = Convert.ToInt64(txtOpdCredit.Text);
                }
                else
                {
                    BizAction.AppConfig.IsOPD = false;
                    BizAction.AppConfig.CreditLimitOPD = 0;
                }

                BizAction.AppConfig.ItemExpiredIndays = Convert.ToInt64(txtItemExpiredIn.Text);   //By Umesh
                BizAction.AppConfig.DefaultCountryCode = Convert.ToString(txtDefaultCountryCode.Text.Trim());   //By Umesh
                //By Anjali..........................
                if (txtCountry.SelectedItem != null)
                {
                    BizAction.AppConfig.CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;
                }
                if (txtState.SelectedItem != null)
                {
                    BizAction.AppConfig.StateID = ((MasterListItem)txtState.SelectedItem).ID;
                }
                if (txtCity.SelectedItem != null)
                {
                    BizAction.AppConfig.CityID = ((MasterListItem)txtCity.SelectedItem).ID;
                }
                if (txtArea.SelectedItem != null)
                {
                    BizAction.AppConfig.RegionID = ((MasterListItem)txtArea.SelectedItem).ID;
                }
                //...................................
                //* Added by - Ajit Jadhav
                //* Added Date - 6/9/2016
                //* Comments - Save Billing Exceeds Limit    
                if (!string.IsNullOrEmpty(txtBillingExceedsLimit.Text)) //if (txtBillingExceedsLimit.Text != null)
                {
                    BizAction.AppConfig.BillingExceedsLimit = Convert.ToDouble(txtBillingExceedsLimit.Text);
                }
                //***//--------------

                BizAction.AppConfig.AttachmentFileName = fileName1;
                BizAction.AppConfig.Attachment = AttachedFileContents;

                if (cmbIndentStore.SelectedItem != null)
                {
                    BizAction.AppConfig.IndentStoreID = ((MasterListItem)cmbIndentStore.SelectedItem).ID;
                }

                BizAction.AppConfig.IsCentralPurchaseStore = (bool)chkCentralPurchase.IsChecked;

                //BY ROHINI
                if (txtDisclaimer.Text.Trim() != string.Empty)
                {
                    BizAction.AppConfig.Disclaimer = txtDisclaimer.Text;
                }
                //

                if (!string.IsNullOrEmpty(txtPatientDailyCashLimit.Text))
                {
                    BizAction.AppConfig.PatientDailyCashLimit = Convert.ToDecimal(txtPatientDailyCashLimit.Text);
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Application Configuration Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                    else
                    {
                        Indicatior.Close();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }

            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            myAppConfig = null;
            string strAction = "PalashDynamics.Administration.SystemConfiguration";

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "System Configuration";
            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance(strAction) as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(mydata);
        }

        private void cmbUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbUnit.SelectedItem != null)
            {
                //long id = ((MasterListItem)cmbUnit.SelectedItem).ID;

                FillCashCounter(((MasterListItem)cmbUnit.SelectedItem).ID);

            }

            clsGetAppConfigBizActionVO Obj = new clsGetAppConfigBizActionVO();
            //Obj.AppConfig = myAppConfig;
            //if (cmbUnit.SelectedItem != null && ((clsAppConfigVO)cmbUnit.SelectedItem).UnitID != myAppConfig.UnitID)
            if (cmbUnit.SelectedItem != null)
            {
                Obj.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        myAppConfig = null;
                        this.DataContext = null;
                        myAppConfig = ((clsGetAppConfigBizActionVO)arg.Result).AppConfig;
                        // BY BHUSHAN
                        myAppConfigEmail = new ObservableCollection<clsAppConfigAutoEmailSMSVO>();
                        List<clsAppConfigAutoEmailSMSVO> TempResult = new List<clsAppConfigAutoEmailSMSVO>();
                        TempResult = ((clsGetAppConfigBizActionVO)arg.Result).AppEmail;
                        if (TempResult != null)
                        {
                            foreach (clsAppConfigAutoEmailSMSVO item in TempResult)
                            {
                                myAppConfigEmail.Add(item);
                            }
                        }

                        this.DataContext = myAppConfig;
                        this.DataContext = myAppConfig;

                        cmbAdminRole.SelectedValue = ((clsAppConfigVO)this.DataContext).AdminRoleID;
                        cmbDoctorRole.SelectedValue = ((clsAppConfigVO)this.DataContext).DoctorRoleID;
                        cmbNurseRole.SelectedValue = ((clsAppConfigVO)this.DataContext).NurseRoleID;
                        cmbSelfCompany.SelectedValue = ((clsAppConfigVO)this.DataContext).SelfCompanyID;
                        cmbTariff.SelectedValue = ((clsAppConfigVO)this.DataContext).TariffID;
                        cmbPaymentMode.SelectedValue = ((clsAppConfigVO)this.DataContext).PaymentModeID;
                        cmbRadiologyStore.SelectedValue = ((clsAppConfigVO)this.DataContext).RadiologyStoreID;
                        cmbPathologyStore.SelectedValue = ((clsAppConfigVO)this.DataContext).PathologyStoreID;
                        //By Changdeo S
                        cmbPharmacyStore.SelectedValue = ((clsAppConfigVO)this.DataContext).PharmacyStoreID;
                        //........................

                        //by rohinee for cash counter
                        cmbOPDCounter.SelectedValue = ((clsAppConfigVO)this.DataContext).LabCounterID;
                        cmbIPDCounter.SelectedValue = ((clsAppConfigVO)this.DataContext).IPDCounterID;
                        cmbPharmacyCounter.SelectedValue = ((clsAppConfigVO)this.DataContext).OPDCounterID;
                        cmbLabCounter.SelectedValue = ((clsAppConfigVO)this.DataContext).PharmacyCounterID;
                        cmbRadiologyCounter.SelectedValue = ((clsAppConfigVO)this.DataContext).RadiologyCounterID;


                        //
                        //rohini
                        cmbCategoryL.SelectedValue = ((clsAppConfigVO)this.DataContext).CategoryID;
                        //

                        //By Neena
                        txtWebSite.Text = ((clsAppConfigVO)this.DataContext).WebSite;
                        //
                        cmbOTStore.SelectedValue = ((clsAppConfigVO)this.DataContext).OTStoreID;
                        cmbDateFormat.SelectedValue = ((clsAppConfigVO)this.DataContext).DateFormatID;
                        cmbVisitType.SelectedValue = ((clsAppConfigVO)this.DataContext).VisitTypeID;
                        cmbDepartment.SelectedValue = ((clsAppConfigVO)this.DataContext).DepartmentID;
                        cmbDoctor.SelectedValue = ((clsAppConfigVO)this.DataContext).DoctorID;
                        cmbFreeFollowupVisittype.SelectedValue = ((clsAppConfigVO)this.DataContext).FreeFollowupVisitTypeID;
                        cmbPharmacyVisitType.SelectedValue = ((clsAppConfigVO)this.DataContext).PharmacyVisitTypeID;
                        //by rohini
                        cmbPathologyVisitType.SelectedValue = ((clsAppConfigVO)this.DataContext).PathologyVisitTypeID;
                        cmbPathologyCompanyType.SelectedValue = ((clsAppConfigVO)this.DataContext).PathologyCompanyTypeID;
                        //
                        cmbPathoSpecialization.SelectedValue = ((clsAppConfigVO)this.DataContext).PathoSpecializationID;
                        cmbConsultationSpecialization.SelectedValue = ((clsAppConfigVO)this.DataContext).ConsultationID;
                        cmbRadiologySpecialization.SelectedValue = ((clsAppConfigVO)this.DataContext).RadiologySpecializationID;
                        cmbPharmacySpecialization.SelectedValue = ((clsAppConfigVO)this.DataContext).PharmacySpecializationID;
                        cmbLocalLanSMS.SelectedValue = ((clsAppConfigVO)this.DataContext).ConftnMsgForAdd;
                        cmbEmbryologistClassification.SelectedValue = ((clsAppConfigVO)this.DataContext).EmbryologistID;
                        cmbAnesthetistClassification.SelectedValue = ((clsAppConfigVO)this.DataContext).AnesthetistID;
                        cmbRadiologist.SelectedValue = ((clsAppConfigVO)this.DataContext).RadiologistID;
                        cmbGynecologist.SelectedValue = ((clsAppConfigVO)this.DataContext).GynecologistID;
                        cmbPhysician.SelectedValue = ((clsAppConfigVO)this.DataContext).PhysicianID;
                        cmbAndrologist.SelectedValue = ((clsAppConfigVO)this.DataContext).AndrologistID;
                        cmbBiologist.SelectedValue = ((clsAppConfigVO)this.DataContext).BiologistID;
                        cmbPatientCategory.SelectedValue = ((clsAppConfigVO)this.DataContext).PatientCategoryID;
                        cmbPharmacyPatientCategory.SelectedValue = ((clsAppConfigVO)this.DataContext).PharmacyPatientCategoryID;
                        cmbRelation.SelectedValue = ((clsAppConfigVO)this.DataContext).SelfRelationID;
                        cmbPatientSource.SelectedValue = ((clsAppConfigVO)this.DataContext).PatientSourceID;
                        cmbCompanyPatientSource.SelectedValue = ((clsAppConfigVO)this.DataContext).CompanyPatientSourceID;
                        chkAllowDischargeReq.IsChecked = ((clsAppConfigVO)this.DataContext).IsAllowDischargeRequest;
                        txtRefAmt.Text = Convert.ToString(((clsAppConfigVO)this.DataContext).RefundAmount);
                        cmbPayMode.SelectedValue = ((clsAppConfigVO)this.DataContext).RefundPayModeID;
                        // Pathologist for MFC on 25.05.2016
                        cmbPathologist.SelectedValue = ((clsAppConfigVO)this.DataContext).PathologistID;
                        //

                        // BY BHSUAHN

                        //cmbClinicalCostingDivision.SelectedValue = ((clsAppConfigVO)this.DataContext).ClinicalCostingDivisionID;  //Costing Divisions for Clinical Billing
                        //cmbPharmacyCostingDivision.SelectedValue = ((clsAppConfigVO)this.DataContext).PharmacyCostingDivisionID;  //Costing Divisions for Pharmacy Billing

                        cmbPathologyDepartment.SelectedValue = ((clsAppConfigVO)this.DataContext).PathologyDepartmentID;  //Set Department For Pathology
                        cmbRadiologyDepartment.SelectedValue = ((clsAppConfigVO)this.DataContext).RadiologyDepartmentID;  //Set Department For Radiology

                        //By Anjali.......................
                        cmbLab.SelectedValue = ((clsAppConfigVO)this.DataContext).InhouseLabID;
                        cmbOocyteDonotion.SelectedValue = ((clsAppConfigVO)this.DataContext).OocyteDonationID;
                        cmbEmbryoReceipent.SelectedValue = ((clsAppConfigVO)this.DataContext).EmbryoReceipentID;
                        cmbOocyteReceipent.SelectedValue = ((clsAppConfigVO)this.DataContext).OocyteReceipentID;
                        cmbDoctorType.SelectedValue = ((clsAppConfigVO)this.DataContext).DoctorTypeForReferral;
                        cmbIdentity.SelectedValue = ((clsAppConfigVO)this.DataContext).IdentityForInternationalPatient;
                        cmbForRefund.SelectedValue = ((clsAppConfigVO)this.DataContext).AuthorizationLevelForRefundID;
                        cmbForConcession.SelectedValue = ((clsAppConfigVO)this.DataContext).AuthorizationLevelForConcessionID;

                        cmbForMRPAdjustment.SelectedValue = ((clsAppConfigVO)this.DataContext).AuthorizationLevelForMRPAdjustmentID;

                        txtCountry.SelectedValue = ((clsAppConfigVO)this.DataContext).CountryID;
                        txtState.SelectedValue = ((clsAppConfigVO)this.DataContext).StateID;
                        txtCity.SelectedValue = ((clsAppConfigVO)this.DataContext).CityID;
                        txtArea.SelectedValue = ((clsAppConfigVO)this.DataContext).RegionID;

                        chkCentralPurchase.IsChecked = ((clsAppConfigVO)this.DataContext).IsCentralPurchaseStore;
                        cmbIndentStore.SelectedValue = ((clsAppConfigVO)this.DataContext).IndentStoreID;

                        //By Umesh
                        txtIpdCredit.Text = ((clsAppConfigVO)this.DataContext).CreditLimitIPD.ToString();
                        if (!string.IsNullOrEmpty(txtIpdCredit.Text) && txtIpdCredit.Text != Convert.ToString(0))
                        {
                            chkIpd.IsChecked = true;
                        }
                        else
                        {
                            chkIpd.IsChecked = false;
                        }
                        txtOpdCredit.Text = ((clsAppConfigVO)this.DataContext).CreditLimitOPD.ToString();

                        txtPatientDailyCashLimit.Text = ((clsAppConfigVO)this.DataContext).PatientDailyCashLimit.ToString(); 

                        txtItemExpiredIn.Text = ((clsAppConfigVO)this.DataContext).ItemExpiredIndays.ToString();//By Umesh
                        txtDefaultCountryCode.Text = ((clsAppConfigVO)this.DataContext).DefaultCountryCode.ToString();//By Umesh
                        if (!string.IsNullOrEmpty(txtOpdCredit.Text) && txtOpdCredit.Text != Convert.ToString(0))
                        {
                            chkOpd.IsChecked = true;
                        }
                        else
                        {
                            chkOpd.IsChecked = false;
                        }

                        ClearAutoEmail();
                        if (myAppConfigEmail != null)
                        {
                            dgEmailSetting.ItemsSource = null;
                            dgEmailSetting.ItemsSource = myAppConfigEmail;
                            EventList = myAppConfigEmail;
                        }

                    }
                    else
                    {
                        Indicatior.Close();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(Obj, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            //}



        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                myAppConfig.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
                //myAppConfig.DoctorID = 0;
                FillDoctorList(myAppConfig.UnitID, myAppConfig.DepartmentID);
            }
        }

        #region Validation

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCountry.Text = txtCountry.Text.ToTitleCase();
        }

        private void txtDatabaseName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDatabaseName.Text = txtDatabaseName.Text.ToTitleCase();
        }

        private void txtState_LostFocus(object sender, RoutedEventArgs e)
        {
            txtState.Text = txtState.Text.ToTitleCase();
        }

        private void txtDistrict_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDistrict.Text = txtDistrict.Text.ToTitleCase();
        }

        private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCity.Text = txtCity.Text.ToTitleCase();
        }

        private void txtArea_LostFocus(object sender, RoutedEventArgs e)
        {
            txtArea.Text = txtArea.Text.ToTitleCase();
        }

        private void txtPincode_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtPincode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItNumber())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }

        }

        #endregion

        #region Commented
        //private void hblEmail_Click(object sender, RoutedEventArgs e)
        //{
        //    if (((MasterListItem)cmbEmail.SelectedItem).ID != 0)
        //    {
        //        cmbEmail.ClearValidationError();
        //        ChildWindow chform = new ChildWindow();
        //        chform.Title = "Email Template";
        //        ViewEmailTemplateDetails win = new ViewEmailTemplateDetails();

        //        win.TemplateId = ((MasterListItem)cmbEmail.SelectedItem).ID;
        //        chform.Content = win;
        //        chform.Show();
        //    }
        //    else
        //    {
        //        if ((MasterListItem)cmbEmail.SelectedItem == null)
        //        {
        //            cmbEmail.TextBox.SetValidation("Please select Email template");
        //            cmbEmail.TextBox.RaiseValidationError();
        //        }
        //        else if (((MasterListItem)cmbEmail.SelectedItem).ID == 0)
        //        {
        //            cmbEmail.TextBox.SetValidation("Please select Email template");
        //            cmbEmail.TextBox.RaiseValidationError();
        //        }
        //        else
        //            cmbEmail.ClearValidationError();
        //    }
        //}
        #endregion

        private void ViewSMSTemplate(long Id)
        {
            if (Id != 0)
            {
                ChildWindow chform = new ChildWindow();
                chform.Title = "SMS Template";
                PreviewSMSTemplate win = new PreviewSMSTemplate();

                win.TemplateId = Id;
                chform.Content = win;
                chform.Show();
            }
        }
        private void ViewEmailTemplate(long Id)
        {
            if (Id != 0)
            {

                ChildWindow chform = new ChildWindow();
                chform.Title = "Email Template";
                PreViewEmailTemplate win = new PreViewEmailTemplate();

                win.TemplateId = Id;
                chform.Content = win;
                chform.Show();
            }

        }

        private void chkIsHO_Click(object sender, RoutedEventArgs e)
        {
            if (chkIsHO.IsChecked == true)
            {
                chkAllowTransactionAtHO.IsEnabled = true;
                cmbUnit.IsEnabled = true;
                //chkAllowTransactionAtHO.IsChecked = true;
            }
            else
            {
                chkAllowTransactionAtHO.IsEnabled = false;
                chkAllowTransactionAtHO.IsChecked = false;
                cmbUnit.IsEnabled = false;
            }

        }


        private void txtAppointmentSlot_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtAppointmentSlot_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtFreeFolloupDays_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtFreeFolloupDays_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtRecentPatientInterval_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtRecentPatientInterval_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void chkAddLogoToAllReports_Checked(object sender, RoutedEventArgs e)
        {
            {
                if (chkAddLogoToAllReports.IsChecked == true)
                {
                    CmdBrowse.IsEnabled = true;
                }
                else
                {
                    CmdBrowse.IsEnabled = false;
                }
            }
        }

        #region File Browse
        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {

                txtlogo.Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        AttachedFileContents = new byte[stream.Length];
                        stream.Read(AttachedFileContents, 0, (int)stream.Length);
                        fileName1 = openDialog.File.Name;
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error while reading file.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }
        #endregion

        #region File View Event

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtlogo.Text))
            {
                if (AttachedFileContents != null)
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + txtlogo.Text.Trim() });
                            AttachedFileNameList.Add(txtlogo.Text.Trim());

                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", txtlogo.Text.Trim(), AttachedFileContents);
                }
            }
        }

        #endregion



        // BY BHUSHAN . . . .  FOR SMS/EMAIlL...

        private void FillEmailSMSEvent()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.Config_EmailSMSEventType;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmdEvent.ItemsSource = null;
                    cmdEvent.ItemsSource = objList;
                    cmdEvent.SelectedValue = (long)0;

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool chkValidations()
        {
            if (string.IsNullOrEmpty(txtEmailId.Text))
            {
                txtEmailId.SetValidation("Please Enter An Appropriate Email Address.");
                txtEmailId.RaiseValidationError();
                txtEmailId.Focus();
                return false;
            }
            else
                txtEmailId.ClearValidationError();

            if ((MasterListItem)cmdEvent.SelectedItem == null || ((MasterListItem)cmdEvent.SelectedItem).ID == 0)
            {
                cmdEvent.TextBox.SetValidation("Please Select An Event ");
                cmdEvent.TextBox.RaiseValidationError();
                cmdEvent.Focus();
                return false;
            }
            else
                cmdEvent.ClearValidationError();
            return true;
        }

        private bool chkDuplicacy()
        {
            clsAppConfigAutoEmailSMSVO obj1 = new clsAppConfigAutoEmailSMSVO();
            clsAppConfigAutoEmailSMSVO obj2 = new clsAppConfigAutoEmailSMSVO();

            ObservableCollection<clsAppConfigAutoEmailSMSVO> ItemList = new ObservableCollection<clsAppConfigAutoEmailSMSVO>();
            ItemList = (ObservableCollection<clsAppConfigAutoEmailSMSVO>)dgEmailSetting.ItemsSource;

            if (ItemList != null && ItemList.Count > 0)
            {
                foreach (clsAppConfigAutoEmailSMSVO item in ItemList)
                {
                    obj1 = ((ObservableCollection<clsAppConfigAutoEmailSMSVO>)dgEmailSetting.ItemsSource).FirstOrDefault(p => p.UnitID.Equals(((MasterListItem)cmbUnit.SelectedItem).ID) && p.ID != item.ID);
                    obj2 = ((ObservableCollection<clsAppConfigAutoEmailSMSVO>)dgEmailSetting.ItemsSource).FirstOrDefault(p => p.EventID.Equals(((MasterListItem)cmdEvent.SelectedItem).ID) && p.ID != item.ID);
                    if (obj1 != null && obj2 != null)
                        break;
                }
            }
            else
            {
                obj1 = ((ObservableCollection<clsAppConfigAutoEmailSMSVO>)dgEmailSetting.ItemsSource).FirstOrDefault(p => p.UnitID.Equals(((MasterListItem)cmbUnit.SelectedItem).ID));
                obj2 = ((ObservableCollection<clsAppConfigAutoEmailSMSVO>)dgEmailSetting.ItemsSource).FirstOrDefault(p => p.UnitID.Equals(((MasterListItem)cmdEvent.SelectedItem).ID));
            }
            if (obj1 != null && obj2 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Event Already Defined For This Unit!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ClearAutoEmail()
        {
            cmdEvent.SelectedValue = (long)0;
            cmdEmailTemplate.SelectedValue = (long)0;
            cmdSMSTemplate.SelectedValue = (long)0;
            txtEmailId.Text = "";
        }


        public ObservableCollection<clsAppConfigAutoEmailSMSVO> EventList { get; set; }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (chkValidations() && chkDuplicacy())
            {
                try
                {
                    if (EventList == null)
                        EventList = new ObservableCollection<clsAppConfigAutoEmailSMSVO>();

                    var item1 = from r in EventList
                                where (r.UnitID == ((MasterListItem)cmbUnit.SelectedItem).ID && r.EventID == ((MasterListItem)cmdEvent.SelectedItem).ID)

                                select new clsAppConfigAutoEmailSMSVO
                                {
                                    UnitID = r.UnitID,
                                    EventID = r.EventID,
                                    EventType = r.EventType,
                                    SendEmailId = r.SendEmailId,
                                    SMSTemplatName = r.SMSTemplatName,
                                    AppSMS = r.AppSMS,
                                    EmailTemplatName = r.EmailTemplatName,
                                    AppEmail = r.AppEmail
                                };
                    if (item1.ToList().Count > 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Event Already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    else
                    {
                        clsAppConfigAutoEmailSMSVO EventItem = new clsAppConfigAutoEmailSMSVO();
                        EventItem.AppEmail = ((MasterListItem)cmdEmailTemplate.SelectedItem).ID;
                        if (EventItem.AppEmail > 0)
                            EventItem.EmailTemplatName = ((MasterListItem)cmdEmailTemplate.SelectedItem).Description;
                        else
                            EventItem.EmailTemplatName = " ";
                        EventItem.EventID = ((MasterListItem)cmdEvent.SelectedItem).ID;
                        EventItem.EventType = ((MasterListItem)cmdEvent.SelectedItem).Description;
                        EventItem.AppSMS = ((MasterListItem)cmdSMSTemplate.SelectedItem).ID;
                        if (EventItem.AppSMS > 0)
                            EventItem.SMSTemplatName = ((MasterListItem)cmdSMSTemplate.SelectedItem).Description;
                        else
                            EventItem.SMSTemplatName = " ";
                        EventItem.SendEmailId = txtEmailId.Text.Trim();

                        // By BHUSHAN
                        EventItem.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        EventItem.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                        EventItem.AddedDateTime = DateTime.Now;
                        EventItem.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                        EventItem.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        EventItem.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                        EventItem.UpdatedDateTime = DateTime.Now;
                        EventItem.UpdatedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                        //   EventList.Add(EventItem);
                        myAppConfigEmail.Add(EventItem);
                        dgEmailSetting.ItemsSource = myAppConfigEmail;
                        //  dgEmailSetting.ItemsSource = EventList;
                        ClearAutoEmail();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EventList.Count > 0)
                {
                    var item1 = from r in EventList
                                where (r.UnitID == ((MasterListItem)cmbUnit.SelectedItem).ID && r.EventID == ((MasterListItem)cmdEvent.SelectedItem).ID
                                       && r.SendEmailId == txtEmailId.Text.Trim() && r.AppEmail == ((MasterListItem)cmdEmailTemplate.SelectedItem).ID
                                       && r.AppSMS == ((MasterListItem)cmdSMSTemplate.SelectedItem).ID)

                                select new clsAppConfigAutoEmailSMSVO
                                {
                                    UnitID = r.UnitID,
                                    EventID = r.EventID,
                                    EventType = r.EventType,
                                    SendEmailId = r.SendEmailId,
                                    SMSTemplatName = r.SMSTemplatName,
                                    AppSMS = r.AppSMS,
                                    EmailTemplatName = r.EmailTemplatName,
                                    AppEmail = r.AppEmail
                                };
                    if (item1.ToList().Count == 0)
                    {
                        int var = dgEmailSetting.SelectedIndex;
                        if (EventList.Count > 0)
                            EventList.RemoveAt(dgEmailSetting.SelectedIndex);
                        EventList.Insert(var, new clsAppConfigAutoEmailSMSVO
                        {
                            EventID = ((MasterListItem)cmdEvent.SelectedItem).ID,
                            EventType = ((MasterListItem)cmdEvent.SelectedItem).Description,
                            SendEmailId = txtEmailId.Text.Trim(),
                            AppEmail = ((MasterListItem)cmdEmailTemplate.SelectedItem).ID,
                            EmailTemplatName = ((MasterListItem)cmdEmailTemplate.SelectedItem).Description,
                            AppSMS = ((MasterListItem)cmdSMSTemplate.SelectedItem).ID,
                            SMSTemplatName = ((MasterListItem)cmdSMSTemplate.SelectedItem).Description,
                        });
                        foreach (var item in EventList)
                        {
                            if (item.AppEmail == 0)
                                item.EmailTemplatName = " ";
                            if (item.AppSMS == 0)
                                item.SMSTemplatName = " ";
                        }

                        myAppConfigEmail = new ObservableCollection<clsAppConfigAutoEmailSMSVO>();
                        myAppConfigEmail = (ObservableCollection<clsAppConfigAutoEmailSMSVO>)EventList;

                        dgEmailSetting.ItemsSource = null;
                        dgEmailSetting.ItemsSource = myAppConfigEmail;
                        ClearAutoEmail();
                        cmdEdit.IsEnabled = false;
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Event Already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ViewImageEmail_Click(object sender, RoutedEventArgs e)
        {
            long SelectedId;
            SelectedId = ((clsAppConfigAutoEmailSMSVO)dgEmailSetting.SelectedItem).AppEmail; //((MasterListItem)cmdE.SelectedItem).ID;
            ViewEmailTemplate(SelectedId);
        }

        private void ViewImageSMS_Click(object sender, RoutedEventArgs e)
        {
            long SelectedId;
            SelectedId = ((clsAppConfigAutoEmailSMSVO)dgEmailSetting.SelectedItem).AppSMS; //((MasterListItem)cmdE.SelectedItem).ID;
            ViewSMSTemplate(SelectedId);
        }

        private void CCTO_Click(object sender, RoutedEventArgs e)
        {
            clsAppEmailCCToBizActionVo BizAction = new clsAppEmailCCToBizActionVo();
            //  long EUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
            long EUnitID = ((clsAppConfigAutoEmailSMSVO)dgEmailSetting.SelectedItem).UnitID;
            long EconfigAutoEmailID = ((clsAppConfigAutoEmailSMSVO)dgEmailSetting.SelectedItem).ID;

            BizAction.UnitID = EUnitID;
            BizAction.ConfigAutoEmailID = EconfigAutoEmailID;
            frmApplicatonConfigEmailCCinCCTo chform = null;
            chform = new frmApplicatonConfigEmailCCinCCTo(EUnitID, EconfigAutoEmailID);
            chform.Title = "Add in CCTO";
            chform.OnCloseButton_Click += new RoutedEventHandler(Email_ClosedButton);
            chform.Show();

        }
        void Email_ClosedButton(object sender, RoutedEventArgs e)
        {
            int i = 0;
        }

        private void hlbDeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgEmailSetting.SelectedItem != null)
                {
                    string msgText = "Are You Sure \n You Want To Delete The Selected Event ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            EventList.RemoveAt(dgEmailSetting.SelectedIndex);
                        }
                    };
                    msgWD.Show();
                }
            }
            catch
            {
                throw;
            }
        }

        private void cmdViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmailSetting.SelectedItem != null)
            {
                cmdEdit.IsEnabled = true;
                cmdEvent.SelectedValue = ((clsAppConfigAutoEmailSMSVO)dgEmailSetting.SelectedItem).EventID;
                cmdEmailTemplate.SelectedValue = ((clsAppConfigAutoEmailSMSVO)dgEmailSetting.SelectedItem).AppEmail;
                cmdSMSTemplate.SelectedValue = ((clsAppConfigAutoEmailSMSVO)dgEmailSetting.SelectedItem).AppSMS;
                txtEmailId.Text = ((clsAppConfigAutoEmailSMSVO)dgEmailSetting.SelectedItem).SendEmailId;
            }
        }

        private void dgEmailSetting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtConfirmPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                txtConfirmPassword.SetValidation("Password & Conform Password Are Not Matching..");
                txtConfirmPassword.RaiseValidationError();
                txtConfirmPassword.Focus();
            }
            else
            {
                txtConfirmPassword.ClearValidationError();
            }
        }

        private void txtSMSConfirmPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSMSPassword.Password != txtSMSConfirmPassword.Password)
            {
                txtSMSConfirmPassword.SetValidation("Password & Conform Password Are Not Matching..");
                txtSMSConfirmPassword.RaiseValidationError();
                txtSMSConfirmPassword.Focus();
            }
            else
            {
                txtSMSConfirmPassword.ClearValidationError();
            }
        }

        private void txtSMSPassword_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Length > 0)
            {
                if (txtEmail.Text.IsEmailValid())
                    txtEmail.ClearValidationError();
                else
                {
                    txtEmail.SetValidation("Please Enter Valid Email");
                    txtEmail.RaiseValidationError();
                }
            }
        }

        private void txtEmailId_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void chkOpd_Checked(object sender, RoutedEventArgs e)
        {
            if (chkOpd.IsChecked == true)
            {
                txtOpdCredit.IsEnabled = true;
            }
        }

        private void chkIpd_Checked(object sender, RoutedEventArgs e)
        {
            if (chkIpd.IsChecked == true)
            {
                txtIpdCredit.IsEnabled = true;
            }
        }

        private void chkOpd_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkOpd.IsChecked == false)
            {
                txtOpdCredit.Text = "0";
                txtOpdCredit.IsEnabled = false;
                //Save();
            }
        }

        private void chkIpd_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkIpd.IsChecked == false)
            {
                txtIpdCredit.Text = "0";
                txtIpdCredit.IsEnabled = false;
                // Save();
            }
        }


        public void FillCountry()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
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
                    txtCountry.ItemsSource = null;
                    txtCountry.ItemsSource = objList.DeepCopy();
                    // txtCountry.SelectedItem = objList[0];


                }
                if (this.DataContext != null)
                {
                    txtCountry.SelectedValue = myAppConfig.CountryID;

                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        List<clsRegionVO> RegionList;
        public void FillCountry(long CountryID, long StateID, long CityID, long RegionID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
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
                        txtCountry.ItemsSource = null;
                        txtCountry.ItemsSource = objList.DeepCopy();

                    }
                    if (this.DataContext != null)
                    {
                        txtCountry.SelectedValue = myAppConfig.CountryID;

                    }
                    FillState(CountryID, StateID, CityID, RegionID);
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }
        public void FillState(long CountryID, long StateID, long CityID, long RegionID)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
            BizAction.CountryId = CountryID;
            BizAction.ListStateDetails = new List<clsStateVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                    {
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                        {
                            foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtState.ItemsSource = null;
                    txtState.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        txtState.SelectedValue = myAppConfig.StateID;
                    }
                    FillCity(StateID, CityID, RegionID);
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillCity(long StateID, long CityID, long RegionID)
        {
            clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
            BizAction.StateId = StateID;
            BizAction.ListCityDetails = new List<clsCityVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListCityDetails != null)
                    {
                        if (BizAction.ListCityDetails.Count > 0)
                        {
                            foreach (clsCityVO item in BizAction.ListCityDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = objList.DeepCopy();



                    if (this.DataContext != null)
                    {
                        txtCity.SelectedValue = myAppConfig.CityID;

                    }
                    FillRegion(CityID);
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillState(long CountryID)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
            BizAction.CountryId = CountryID;
            BizAction.ListStateDetails = new List<clsStateVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                    {
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                        {
                            foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }

                    txtState.ItemsSource = null;
                    txtState.ItemsSource = objList.DeepCopy();



                    if (this.DataContext != null)
                    {
                        txtState.SelectedValue = myAppConfig.StateID;

                    }
                    else
                    {
                        txtState.SelectedItem = objM;
                    }

                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillCity(long StateID)
        {
            clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
            BizAction.StateId = StateID;
            BizAction.ListCityDetails = new List<clsCityVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListCityDetails != null)
                    {
                        if (BizAction.ListCityDetails.Count > 0)
                        {
                            foreach (clsCityVO item in BizAction.ListCityDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = objList.DeepCopy();



                    if (this.DataContext != null)
                    {
                        txtCity.SelectedValue = myAppConfig.CityID;

                    }
                    else
                    {
                        txtCity.SelectedItem = objM;

                    }

                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillRegion(long CityID)
        {
            clsGetRegionDetailsByCityIDBizActionVO BizAction = new clsGetRegionDetailsByCityIDBizActionVO();
            BizAction.CityId = CityID;
            BizAction.ListRegionDetails = new List<clsRegionVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.ListRegionDetails = ((clsGetRegionDetailsByCityIDBizActionVO)args.Result).ListRegionDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListRegionDetails != null)
                    {
                        if (BizAction.ListRegionDetails.Count > 0)
                        {
                            RegionList = new List<clsRegionVO>();
                            RegionList = BizAction.ListRegionDetails;
                            foreach (clsRegionVO item in BizAction.ListRegionDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = objList.DeepCopy();



                    if (this.DataContext != null)
                    {
                        txtArea.SelectedValue = myAppConfig.RegionID;

                    }
                    else
                    {
                        txtArea.SelectedItem = objM;

                    }
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void txtCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtCountry.SelectedItem != null && txtCountry.SelectedValue != null)
                if (((MasterListItem)txtCountry.SelectedItem).ID > 0)
                {
                    myAppConfig.CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtState.ItemsSource = objList;
                    txtState.SelectedItem = objM;
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                    txtArea.ItemsSource = objList;
                    txtArea.SelectedItem = objM;
                    FillState(((MasterListItem)txtCountry.SelectedItem).ID);
                }
        }

        private void txtState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    myAppConfig.StateID = ((MasterListItem)txtState.SelectedItem).ID;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                    txtArea.ItemsSource = objList;
                    txtArea.SelectedItem = objM;
                    FillCity(((MasterListItem)txtState.SelectedItem).ID);
                }
        }


        private void txtCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    if (((MasterListItem)txtCity.SelectedItem).ID > 0)
                        myAppConfig.CityID = ((MasterListItem)txtCity.SelectedItem).ID;

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    txtArea.ItemsSource = null;
                    txtArea.SelectedItem = objM;
                    FillRegion(((MasterListItem)txtCity.SelectedItem).ID);
                }
        }

        private void txtArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)txtArea.SelectedItem).ID > 0)
            {
                myAppConfig.RegionID = ((MasterListItem)txtArea.SelectedItem).ID;
                myAppConfig.Pincode = (RegionList.Where(t => t.Id == ((MasterListItem)txtArea.SelectedItem).ID).Select(t => t.PinCode).SingleOrDefault());
            }
            else
            {
                myAppConfig.Pincode = "";
            }
        }

        private void txtWebSite_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtWebSite.Text.Length > 0)
            {
                if (txtWebSite.Text.IsEmailValid())
                    txtWebSite.ClearValidationError();
                else
                {
                    txtWebSite.SetValidation("Please enter valid WebSite");
                    txtWebSite.RaiseValidationError();
                }
            }
        }

        private void txtMobileCountryCode_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtMobileCountryCode_OnTextChanged(object sender, RoutedEventArgs e)
        {
            //if (IsPageLoded)
            //{
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsValidCountryCode() && textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((TextBox)sender).Text.Length > 4)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
            // }
        }

        private void txtPatientDailyCashLimit_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            //{
            //    ((TextBox)sender).Text = textBefore;
            //    ((TextBox)sender).SelectionStart = selectionStart;
            //    ((TextBox)sender).SelectionLength = selectionLength;
            //    textBefore = "";
            //    selectionStart = 0;
            //    selectionLength = 0;
            //}
        }

        private void txtPatientDailyCashLimit_KeyDown(object sender, KeyEventArgs e)
        {
            //textBefore = ((TextBox)sender).Text;
            //selectionStart = ((TextBox)sender).SelectionStart;
            //selectionLength = ((TextBox)sender).SelectionLength;
        }
    }
}