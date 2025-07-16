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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;
using CIMS;
using System.Text;


namespace PalashDynamics.Administration //GST Details added by Ashish Z. on dated 24062017
{
    public partial class frmServiceTaxLinking : ChildWindow
    {
        #region Variable Declarations
        public clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();  //set from PalashDynamics.Administration.TariffServiceMaster.xaml.cs
        List<clsServiceTaxVO> ServiceTaxClassList = new List<clsServiceTaxVO>();
        List<clsServiceTaxVO> ServiceTaxClassSelectedList = new List<clsServiceTaxVO>();
        List<MasterListItem> TaxTypeList = new List<MasterListItem>();
        bool IsModify = false;
        #endregion

        #region Constructor and Loaded
        public frmServiceTaxLinking()
        {
            InitializeComponent();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (objServiceMasterVO != null)
                lblServiceName.Text = ": " + objServiceMasterVO.ServiceName.ToString();
            //lblTariffName.Text = ": " + objServiceMasterVO.TariffName.ToString();
            //lblClassName.Text = ": " + objServiceMasterVO.ClassName.ToString();
            GetServiceTaxlist();
            //FillUnitList();
            FillTax();
            FillTaxType();
            //FillServiceClasses();
        }
        #endregion

        #region Fill Combobox

        //private void FillServiceClasses()
        //{
        //    try
        //    {
        //        clsGetServiceMasterListBizActionVO BizActionObj = new clsGetServiceMasterListBizActionVO();
        //        BizActionObj.ServiceMaster = new clsServiceMasterVO();
        //        if (objServiceMasterVO != null)
        //            BizActionObj.ServiceMaster.ID = objServiceMasterVO.ServiceID;
        //        BizActionObj.ServiceMaster.UnitID = 1;// objServiceMasterVO.UnitID;
        //        BizActionObj.GetAllServiceMasterDetailsForID = true;
        //        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
        //        //BizActionObj.IsPagingEnabled = true;
        //        //BizActionObj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
        //        //BizActionObj.MaximumRows = DataList.PageSize;
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                clsGetServiceMasterListBizActionVO result = args.Result as clsGetServiceMasterListBizActionVO;
        //                if (result.ServiceList != null)
        //                {
        //                    foreach (var item in result.ServiceList)
        //                    {
        //                        clsServiceTaxVO clsVO = new clsServiceTaxVO();
        //                        clsVO.ServiceId = item.ServiceID;
        //                        clsVO.ClassId = item.ClassID;
        //                        clsVO.ClassName = item.ClassName;
        //                        ServiceTaxClassList.Add(clsVO);
        //                    }
        //                    dgClassList.ItemsSource = null;
        //                    dgClassList.ItemsSource = ServiceTaxClassList.ToList();
        //                }
        //            }
        //        };
        //        client.ProcessAsync(BizActionObj, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        List<MasterListItem> objUnitList = new List<MasterListItem>();
        private void FillUnitList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    objUnitList.Add(new MasterListItem(0, "-- Select --"));
                    objUnitList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbUnit.ItemsSource = null;
                    cmbUnit.ItemsSource = objUnitList;
                    cmbUnit.SelectedItem = objUnitList[0];

                }
            };
            client.ProcessAsync(BizAction, null); //((IApplicationConfiguration)App.Current).CurrentUser
            client.CloseAsync();
        }

        private void FillTax()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_TaxMaster;
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
                        cmbTax.ItemsSource = null;
                        cmbTax.ItemsSource = objList;
                        cmbTax.SelectedItem = objList[0];
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


        private void FillTaxType()
        {
            List<MasterListItem> EnumList = new List<MasterListItem>();
            EnumToList(typeof(TaxType), EnumList);
            TaxTypeList.Add(new MasterListItem(0, "-- Select --"));
            TaxTypeList.AddRange(EnumList);

            cmbTaxtype.ItemsSource = null;
            cmbTaxtype.ItemsSource = TaxTypeList;
            cmbTaxtype.SelectedItem = TaxTypeList[0];
        }

        private static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                if (Value > 0)
                {
                    string Display = Enum.GetName(EnumType, Value);
                    MasterListItem Item = new MasterListItem(Value, Display);
                    TheMasterList.Add(Item);
                }
            }
        }

        private static object[] GetValues(Type enumType)
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

        #endregion

        #region Clicked Events
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow msgWD =
            new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Save the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    if (!Validations(true))
                    {
                        SaveTaxDetails();
                    }
                }
            };
            msgWD.Show();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow msgWD =
      new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Modify the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    if (!Validations(false))
                    {
                        try
                        {
                            clsAddUpdateServiceTaxBizActionVO BizAction = new clsAddUpdateServiceTaxBizActionVO();
                            BizAction.OperationType = 2;
                            BizAction.ServiceTaxDetailsVO = new clsServiceTaxVO();
                            BizAction.ServiceTaxDetailsVO.ID = (dgServiceTaxList.SelectedItem as clsServiceTaxVO).ID;
                            BizAction.ServiceTaxDetailsVO.UnitId = 0;//(cmbUnit.SelectedItem as MasterListItem).ID;
                            BizAction.ServiceTaxDetailsVO.ServiceId = (dgServiceTaxList.SelectedItem as clsServiceTaxVO).ServiceId;
                            BizAction.ServiceTaxDetailsVO.ClassId = 0; //(dgServiceTaxList.SelectedItem as clsServiceTaxVO).ClassId;
                            BizAction.ServiceTaxDetailsVO.TaxID = (cmbTax.SelectedItem as MasterListItem).ID;
                            BizAction.ServiceTaxDetailsVO.Percentage = Convert.ToDecimal(txtPercentage.Text);
                            BizAction.ServiceTaxDetailsVO.TaxType = Convert.ToInt32((cmbTaxtype.SelectedItem as MasterListItem).ID);
                            BizAction.ServiceTaxDetailsVO.IsTaxLimitApplicable = Convert.ToBoolean(chkIsTaxLimitApplicable.IsChecked);
                            if (!string.IsNullOrEmpty(txtTaxLimt.Text))
                                BizAction.ServiceTaxDetailsVO.TaxLimit = Convert.ToDecimal(txtTaxLimt.Text);
                            BizAction.ServiceTaxDetailsVO.status = true;
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    clsAddUpdateServiceTaxBizActionVO objVO = new clsAddUpdateServiceTaxBizActionVO();
                                    objVO = args.Result as clsAddUpdateServiceTaxBizActionVO;
                                    if (objVO != null && objVO.SuccessStatus == 1)
                                    {
                                        ClearData();
                                        cmdSave.IsEnabled = true;
                                        cmdModify.IsEnabled = false;
                                        MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Record is already added in List.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                                        msgW.Show();
                                    }
                                    else if (objVO != null)
                                    {
                                        ClearData();
                                        GetServiceTaxlist();
                                        MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Record Modified Sucessfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW.Show();
                                        cmdSave.IsEnabled = true;
                                        cmdModify.IsEnabled = false;
                                    }
                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                        catch (Exception Ex)
                        {
                            throw;
                        }
                    }
                }
            };
            msgWD.Show();
        }

        private void chkIsTaxLimitApplicable_Click(object sender, RoutedEventArgs e)
        {
            if (chkIsTaxLimitApplicable.IsChecked == true)
            {
                txtTaxLimt.IsEnabled = true;
            }
            else if (chkIsTaxLimitApplicable.IsChecked == false)
            {
                txtTaxLimt.IsEnabled = false;
                txtTaxLimt.Text = string.Empty;
            }
        }
        #endregion

        private void GetServiceTaxlist()
        {
            try
            {
                clsGetServiceTaxDetailsBizActionVO BizAction = new clsGetServiceTaxDetailsBizActionVO();
                BizAction.ServiceTaxDetailsVO = new clsServiceTaxVO();
                BizAction.ServiceTaxDetailsVO.ServiceId = objServiceMasterVO.ServiceID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetServiceTaxDetailsBizActionVO objVO = new clsGetServiceTaxDetailsBizActionVO();
                        objVO = (args.Result as clsGetServiceTaxDetailsBizActionVO);
                        if (objVO.ServiceTaxDetailsVOList == null)
                            objVO.ServiceTaxDetailsVOList = new List<clsServiceTaxVO>();

                        if (objVO.ServiceTaxDetailsVOList != null && objVO.ServiceTaxDetailsVOList.Count > 0)
                        {
                            foreach (var item in objVO.ServiceTaxDetailsVOList.ToList())
                            {
                                item.TaxTypeName = TaxTypeList.SingleOrDefault(z => z.ID == item.TaxType).Description.ToString();
                            }
                        }
                        dgServiceTaxList.ItemsSource = null;
                        dgServiceTaxList.ItemsSource = objVO.ServiceTaxDetailsVOList;
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveTaxDetails()
        {
            try
            {
                clsAddUpdateServiceTaxBizActionVO BizAction = new clsAddUpdateServiceTaxBizActionVO();
                BizAction.OperationType = 1;
                BizAction.ServiceTaxDetailsVO = new clsServiceTaxVO();
                BizAction.ServiceTaxDetailsVOList = new List<clsServiceTaxVO>();

                BizAction.ServiceTaxDetailsVO.ServiceId = objServiceMasterVO.ServiceID;
                BizAction.ServiceTaxDetailsVO.UnitId = 0; //(cmbUnit.SelectedItem as MasterListItem).ID;
                BizAction.ServiceTaxDetailsVO.TaxID = (cmbTax.SelectedItem as MasterListItem).ID;
                BizAction.ServiceTaxDetailsVO.TariffId = 0;
                BizAction.ServiceTaxDetailsVO.TaxType = Convert.ToInt32((cmbTaxtype.SelectedItem as MasterListItem).ID);
                BizAction.ServiceTaxDetailsVO.Percentage = Convert.ToDecimal(txtPercentage.Text);
                BizAction.ServiceTaxDetailsVO.IsTaxLimitApplicable = Convert.ToBoolean(chkIsTaxLimitApplicable.IsChecked);
                if (!string.IsNullOrEmpty(txtTaxLimt.Text))
                    BizAction.ServiceTaxDetailsVO.TaxLimit = Convert.ToDecimal(txtTaxLimt.Text);
                BizAction.ServiceTaxDetailsVO.status = true;


                //foreach (var item in ServiceTaxClassSelectedList.ToList())
                //{
                //    item.ServiceId = objServiceMasterVO.ServiceID;
                //    item.UnitId = 0; //(cmbUnit.SelectedItem as MasterListItem).ID;
                //    item.TaxID = (cmbTax.SelectedItem as MasterListItem).ID;
                //    item.TariffId = 0;
                //    item.TaxType = Convert.ToInt32((cmbTaxtype.SelectedItem as MasterListItem).ID);
                //    item.Percentage = Convert.ToDecimal(txtPercentage.Text);
                //    item.IsTaxLimitApplicable = Convert.ToBoolean(chkIsTaxLimitApplicable.IsChecked);
                //    if (!string.IsNullOrEmpty(txtTaxLimt.Text))
                //        item.TaxLimit = Convert.ToDecimal(txtTaxLimt.Text);
                //    item.status = true;
                //}
                //BizAction.ServiceTaxDetailsVOList = ServiceTaxClassSelectedList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsAddUpdateServiceTaxBizActionVO objVO = new clsAddUpdateServiceTaxBizActionVO();
                        objVO = args.Result as clsAddUpdateServiceTaxBizActionVO;
                        if (objVO != null && objVO.SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record is already added in List.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgW.Show();
                        }
                        else if (objVO != null)
                        {
                            ClearData();
                            GetServiceTaxlist();
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record Saved Sucessfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.Show();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool Validations(bool IsFromSave)
        {
            bool reasult = false;

            //List<clsServiceTaxVO> objServiceTaxList = new List<clsServiceTaxVO>();
            //objServiceTaxList = dgServiceTaxList.ItemsSource as List<clsServiceTaxVO>;
            //if (dgServiceTaxList.ItemsSource != null)
            //{

            //    if (objServiceTaxList != null && objServiceTaxList.Count > 0)
            //    {
            //        StringBuilder strError = new StringBuilder();
            //        var item1 = from r in objServiceTaxList
            //                    where r.TaxType == Convert.ToInt32((cmbTaxtype.SelectedItem as MasterListItem).ID)
            //                    select new clsServiceTaxVO
            //                    {
            //                        TaxType = r.TaxType
            //                    };
            //        if (item1.ToList().Count > 0)
            //        {
            //            reasult = true;
            //            if (strError.ToString().Length > 0)
            //                strError.Append(",");

            //            if (!string.IsNullOrEmpty(strError.ToString()))
            //            {
            //                string strMsg = "TaxType is Different : " + strError.ToString();

            //                MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                msgW1.Show();
            //            }
            //        }
            //    }
            //}

            if (cmbTaxtype.SelectedItem != null && (cmbTaxtype.SelectedItem as MasterListItem).ID > 0 && dgServiceTaxList.ItemsSource != null && (dgServiceTaxList.ItemsSource as List<clsServiceTaxVO>).Count > 0)
            {
                foreach (var item in dgServiceTaxList.ItemsSource as List<clsServiceTaxVO>)
                {
                    if (item.TaxType != (cmbTaxtype.SelectedItem as MasterListItem).ID)
                    {
                        reasult = true;
                        string msg = "Please select same Tax Type";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow("", msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgW.Show();
                        break;
                    }
                }
            }

            //if (ServiceTaxClassSelectedList.Count > 0)
            //{
            //    foreach (var item in ServiceTaxClassSelectedList.ToList())
            //    {
            //        StringBuilder strError = new StringBuilder();
            //        var item1 = from r in ServiceTaxClassSelectedList
            //                    where r.ServiceId == item.ServiceId && r.ClassId == item.ClassId && r.UnitId == item.UnitId && r.TaxID == item.TaxID && r.TaxType == item.TaxType
            //                    select new clsServiceTaxVO
            //                    {
            //                        ClassId = r.ClassId,
            //                        ClassName = r.ClassName
            //                    };
            //        if (item1.ToList().Count > 0)
            //        {
            //            reasult = true;
            //            if (strError.ToString().Length > 0)
            //                strError.Append(",");
            //            strError.Append(item.TaxTypeName);

            //            if (!string.IsNullOrEmpty(strError.ToString()))
            //            {
            //                string strMsg = "TaxType is Different : " + strError.ToString();

            //                MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                msgW1.Show();
            //            }
            //            break;
            //        }
            //    }
            //}

            //if (IsFromSave == true && (ServiceTaxClassSelectedList == null || ServiceTaxClassSelectedList.Count() == 0))
            //{
            //    reasult = true;
            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //            new MessageBoxControl.MessageBoxChildWindow("", "Please select Class!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
            //    msgW.Show();
            //}


            string msg1 = string.Empty;
            if (objServiceMasterVO != null && objServiceMasterVO.ServiceID == 0)
            {
                reasult = true;
                msg1 = "Please select Service!";
                MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("", msg1, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgW.Show();
            }
            //else if (objServiceMasterVO != null && objServiceMasterVO.TariffID == 0)
            //{
            //    //reasult = true;
            //    //msg = "Please select Tariff!";
            //    //MessageBoxControl.MessageBoxChildWindow msgW =
            //    //        new MessageBoxControl.MessageBoxChildWindow("", msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
            //    //msgW.Show();
            //}


            //if (objServiceMasterVO != null && objServiceMasterVO.ClassID == 0)
            //{
            //    reasult = true;
            //    msg = "Please select Class!";
            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //            new MessageBoxControl.MessageBoxChildWindow("", msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
            //    msgW.Show();
            //}


            //if (cmbUnit.SelectedItem != null && (cmbUnit.SelectedItem as MasterListItem).ID == 0)
            //{
            //    reasult = true;
            //    cmbUnit.TextBox.SetValidation("Please Select Unit");
            //    cmbUnit.TextBox.RaiseValidationError();
            //    cmbUnit.Focus();
            //}
            //else
            //{
            //    cmbUnit.TextBox.ClearValidationError();
            //}

            if (cmbTax.SelectedItem != null && (cmbTax.SelectedItem as MasterListItem).ID == 0)
            {
                reasult = true;
                cmbTax.TextBox.SetValidation("Please Select Tax");
                cmbTax.TextBox.RaiseValidationError();
                cmbTax.Focus();
            }
            else
            {
                cmbTax.TextBox.ClearValidationError();
            }

            if (cmbTaxtype.SelectedItem != null && (cmbTaxtype.SelectedItem as MasterListItem).ID == 0)
            {
                reasult = true;
                cmbTaxtype.TextBox.SetValidation("Please Select Tax Type");
                cmbTaxtype.TextBox.RaiseValidationError();
                cmbTaxtype.Focus();
            }
            else
            {
                cmbTaxtype.TextBox.ClearValidationError();
            }

            if (string.IsNullOrEmpty(txtPercentage.Text))
            {
                reasult = true;
                txtPercentage.SetValidation("Please add Tax Percentage(%)");
                txtPercentage.RaiseValidationError();
                txtPercentage.Focus();
            }
            else
            {
                txtPercentage.ClearValidationError();
            }

            if (chkIsTaxLimitApplicable.IsChecked == true && string.IsNullOrEmpty(txtTaxLimt.Text))
            {
                reasult = true;
                txtTaxLimt.SetValidation("Please add Tax Limit");
                txtTaxLimt.RaiseValidationError();
                txtTaxLimt.Focus();
            }
            else
            {
                txtTaxLimt.ClearValidationError();
            }
            return reasult;
        }

        private void ClearData()
        {
            //List<MasterListItem> ClearList = new List<MasterListItem>();
            //MasterListItem Default = new MasterListItem(0, "-- Select --");
            //ClearList.Insert(0, Default);

            //cmbUnit.SelectedValue = (long)0; //ClearList[0];
            cmbTax.SelectedValue = (long)0; // ClearList[0];
            cmbTaxtype.SelectedValue = (long)0; // ClearList[0];
            txtPercentage.Text = "";
            chkIsTaxLimitApplicable.IsChecked = false;
            txtTaxLimt.Text = "";
        }

        //private void chkClasses_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dgClassList.SelectedItem != null)
        //    {
        //        clsServiceTaxVO objVO = (clsServiceTaxVO)dgServiceTaxList.SelectedItem;
        //        clsServiceTaxVO objTaxVO = dgClassList.SelectedItem as clsServiceTaxVO;
        //        if (ServiceTaxClassSelectedList == null)
        //            ServiceTaxClassSelectedList = new List<clsServiceTaxVO>();
        //        CheckBox chk = (CheckBox)sender;
        //        StringBuilder strError = new StringBuilder();
        //        if (chk.IsChecked == true)
        //        {
        //            if (ServiceTaxClassSelectedList.Count > 0)
        //            {
        //                var item = from r in ServiceTaxClassSelectedList
        //                           where r.ClassId == objTaxVO.ClassId
        //                           select new clsServiceTaxVO
        //                           {
        //                               ClassId = r.ClassId,
        //                               ClassName = r.ClassName
        //                           };
        //                if (item.ToList().Count > 0)
        //                {
        //                    if (strError.ToString().Length > 0)
        //                        strError.Append(",");
        //                    strError.Append(objTaxVO.ClassName);

        //                    if (!string.IsNullOrEmpty(strError.ToString()))
        //                    {
        //                        string strMsg = "Class already Selected : " + strError.ToString();

        //                        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                        msgW1.Show();
        //                    }
        //                }
        //                else
        //                {
        //                    ServiceTaxClassSelectedList.Add(objTaxVO);
        //                }
        //            }
        //            else
        //            {
        //                ServiceTaxClassSelectedList.Add(objTaxVO);
        //            }
        //        }
        //        else
        //        {
        //            clsServiceTaxVO obj;
        //            if (objVO != null)
        //            {
        //                obj = ServiceTaxClassSelectedList.Where(z => z.ClassId == objVO.ClassId).FirstOrDefault();
        //                ServiceTaxClassSelectedList.Remove(obj);

        //            }
        //            else if (objTaxVO != null)
        //            {
        //                obj = ServiceTaxClassSelectedList.Where(z => z.ClassId == objTaxVO.ClassId).FirstOrDefault();
        //                ServiceTaxClassSelectedList.Remove(obj);
        //            }
        //        }

        //        //dgServiceTaxList.ItemsSource = null;
        //        //dgServiceTaxList.ItemsSource = ServiceTaxClassSelectedList;
        //        //dgServiceTaxList.UpdateLayout();
        //        //dgServiceTaxList.Focus();

        //    }

        //}

        //private void chkSelectedClass_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void hlkModify_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceTaxList.SelectedItem != null)
            {
                clsServiceTaxVO objTaxVO = new clsServiceTaxVO();
                objTaxVO = dgServiceTaxList.SelectedItem as clsServiceTaxVO;
                //cmbUnit.SelectedValue = objTaxVO.UnitId;
                cmbTax.SelectedValue = objTaxVO.TaxID;

                cmbTaxtype.SelectedItem = TaxTypeList[objTaxVO.TaxType];

                txtPercentage.Text = objTaxVO.Percentage.ToString();
                chkIsTaxLimitApplicable.IsChecked = objTaxVO.IsTaxLimitApplicable;
                txtTaxLimt.Text = objTaxVO.TaxLimit.ToString();

                IsModify = true;
                cmdSave.IsEnabled = false;
                cmdModify.IsEnabled = true;
            }
        }

        private void cmdDeleteTax_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceTaxList.SelectedItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
      new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Delete the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        try
                        {
                            clsAddUpdateServiceTaxBizActionVO BizAction = new clsAddUpdateServiceTaxBizActionVO();
                            BizAction.OperationType = 3;
                            BizAction.ServiceTaxDetailsVO = new clsServiceTaxVO();
                            BizAction.ServiceTaxDetailsVO.ID = (dgServiceTaxList.SelectedItem as clsServiceTaxVO).ID;
                            BizAction.ServiceTaxDetailsVO.status = false;
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    GetServiceTaxlist();
                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Record Deleted Sucessfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW.Show();
                                    cmdSave.IsEnabled = true;
                                    cmdModify.IsEnabled = false;
                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                        catch (Exception Ex)
                        {
                            throw;
                        }
                    }
                };
                msgWD.Show();
            }
        }
    }
}

