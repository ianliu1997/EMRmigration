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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using CIMS;

using System.Collections.ObjectModel;
namespace PalashDynamics.Pharmacy
{
    public partial class ItemSupplier : ChildWindow
    {
        Boolean IsPageLoded = false;
        clsItemMasterVO objMasterVO = null;
        Boolean IsRowSelected = false;

        public List<clsItemSupllierVO> ItemSupplierList;
        public List<clsItemSupllierVO> _ItemSupplier;
        public ItemSupplier()
        {
            InitializeComponent();
            this.DataContext = new clsItemSupllierVO();
        }
        public void GetItemDetails(clsItemMasterVO objItemMasterVO)
        {

            objMasterVO = objItemMasterVO;
        }
        

        public clsAddItemSupplierbizActionVO BizActionobj;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {


            try
            {

               
                BizActionobj = new clsAddItemSupplierbizActionVO();
                BizActionobj.ItemSupplier = new clsItemSupllierVO();
                BizActionobj.ItemSupplierList = (List<clsItemSupllierVO>)dgItemSupplier.ItemsSource;
                BizActionobj.ItemSupplier.CreatedUnitID = 1;
                BizActionobj.ItemSupplier.UpdatedUnitID = 1;
                BizActionobj.ItemSupplier.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID; ;
                BizActionobj.ItemSupplier.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                BizActionobj.ItemSupplier.AddedDateTime = DateTime.Now;

                BizActionobj.ItemSupplier.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                BizActionobj.ItemSupplier.ItemID = objMasterVO.ID;
                BizActionobj.ItemSupplier.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                string msgTitle = "";

                string msgText = "";

                if (IsEditMode == false)
                {
                    msgText = "Are you sure you want to Save the  Details?";

                }
                MessageBoxControl.MessageBoxChildWindow msgW =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();




             

            }
            catch (Exception)
            {

                throw;
            }
        }


    




        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {

        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {




                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessAsync(BizActionobj, new clsUserVO());



                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {

                            //IF DATA IS SAVED
                            if (((PalashDynamics.ValueObjects.Inventory.clsAddItemSupplierbizActionVO)(arg.Result)).SuccessStatus == 1)
                            {
                                if (IsEditMode == true)
                                {
                                    string msgTitle = "";
                                    string msgText = "Item Supplier Updated Successfully";

                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


                                    msgW.Show();
                                    IsEditMode = false;
                                    pkID = 0;
                                }
                                else
                                {
                                    string msgTitle = "";
                                    string msgText = "Item Supplier Saved Successfully";

                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


                                    msgW.Show();

                                }
                                this.Close();
                             


                            }


                        }


                    };


                    client.CloseAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }


        }





        private void EmptyUI()
        {
         
            try
            {
                clsItemMasterVO obj = new clsItemMasterVO();
             
            }
            catch (Exception)
            {

                throw;
            }



        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        public Boolean IsEditMode { get; set; }
        public long pkID { get; set; }

        private void dgItemSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

         
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                lblItemName.Text = "";
                if (objMasterVO.ItemName != null)
                {
                    lblItemName.Text = objMasterVO.ItemName.ToString();
                }
                FillDataGrid();
            }
            IsPageLoded = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }


        private void FillDataGrid()
        {


            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
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
                        objList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        ItemSupplierList = new List<clsItemSupllierVO>();
                        clsItemSupllierVO obj = new clsItemSupllierVO();

                        foreach (var item in objList)
                        {

                            ItemSupplierList.Add(new clsItemSupllierVO { ID = item.ID, SupplierName = item.Description, HPLevelList = obj.HPLevelList, status = item.Status });
                        }


                        dgItemSupplier.ItemsSource = null;
                        dgItemSupplier.ItemsSource = ItemSupplierList;
                        GetItemSupplierlist();




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

        private void GetItemSupplierlist()
        {
            clsGetItemSupplierBizActionVO objBizActionVO = new clsGetItemSupplierBizActionVO();
            objBizActionVO.ItemSupplier = new clsItemSupllierVO();

            objBizActionVO.ItemSupplier.ItemID = objMasterVO.ID;

          
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    _ItemSupplier = null;
                    _ItemSupplier = new List<clsItemSupllierVO>();
                    _ItemSupplier = ((clsGetItemSupplierBizActionVO)args.Result).ItemSupplierList;
                    CheckForExistingSupplier();

                }

            };

            client.ProcessAsync(objBizActionVO, new clsUserVO());
            client.CloseAsync();

        }

        private void CheckForExistingSupplier()
        {
            List<clsItemSupllierVO> objList = (List<clsItemSupllierVO>)dgItemSupplier.ItemsSource;
           
               
                if (objList != null && objList.Count > 0)
                {
                    if (_ItemSupplier != null && _ItemSupplier.Count > 0)
                    {
                        foreach (var item in _ItemSupplier)
                        {
                            foreach (var items in objList)
                            {
                                if (items.ID == item.ID)
                                {
                                    items.status = item.status;
                                    items.SelectedHPLevel = item.SelectedHPLevel;
                                
                                }
                             
                            }
                        }

                    }
                    dgItemSupplier.ItemsSource = objList.OrderByDescending(u=>u.status).ToList();
                   
               }
             
            //this.DataContext = objList;
        }


        private void FillSupplier()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
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

        private void rdbLavelOne_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rdbLevelTwo_Checked(object sender, RoutedEventArgs e)
        {
            clsItemMasterVO obj;
            obj = new clsItemMasterVO();

        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            var chk = sender as CheckBox;
            //var datagridrow = DataGridRow.GetRowContainingElement(chk);
            List<clsItemSupllierVO> obj = new List<clsItemSupllierVO>();
            obj = (List<clsItemSupllierVO>)dgItemSupplier.ItemsSource;
            int ind= dgItemSupplier.SelectedIndex;
             
            //for (int i = 0; i < obj.Count-1; i++)
            //{
            //    if (obj[i].status==chk.IsChecked)
            //    {
                    
            //    }
            //}
            //foreach (var item in obj)
            //{
            //    if (item.status==chk.IsChecked)
            //    {
                    
            //    }  
            //}
        }

        private void cboHPLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            AutoCompleteBox cbo = (AutoCompleteBox)sender;
            if (cbo.SelectedItem != null)
            {
                if (dgItemSupplier.SelectedItem != null)
                {
                    ((clsItemSupllierVO)dgItemSupplier.SelectedItem).SelectedHPLevel =  ((MasterListItem)cbo.SelectedItem);
                }
            }
        }

        private void cboHPLevel_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;
            if (cbo.SelectedItem != null)
            {
                if (dgItemSupplier.SelectedItem != null)
                {
                    ((clsItemSupllierVO)dgItemSupplier.SelectedItem).SelectedHPLevel = ((MasterListItem)cbo.SelectedItem);
                }
            }
        }

        //private void GetGridRowColumnIndex(Point pt, DataGrid grid, out int rowIndex, out int colIndex, out object dataContext)
        //{
        //    rowIndex = -1;
        //    colIndex = -1;
        //    dataContext = null;
        //    var elements = VisualTreeHelper.FindElementsInHostCoordinates(pt, grid);
        //    if (null == elements ||
        //       elements.Count() == 0)
        //    {
        //        return;
        //    }

        //    // Get the rows and columns.
        //    var rowQuery = from gridRow in elements where gridRow is DataGridRow select gridRow as DataGridRow;
        //    var cellQuery = from gridCell in elements where gridCell is DataGridCell select gridCell as DataGridCell;
        //    var cells = cellQuery.ToList<DataGridCell>();
        //    if (cells.Count == 0)
        //    {
        //        return;
        //    }

        //    foreach (var row in rowQuery)
        //    {
        //        dataContext = row.DataContext;
        //        rowIndex = row.GetIndex();
        //        foreach (DataGridColumn col in grid.Columns)
        //        {
        //            var colContent = col.GetCellContent(row);
        //            var parent = GetParent(colContent, typeof(DataGridCell));
        //            if (parent != null)
        //            {
        //                var thisCell = (DataGridCell)parent;
        //                if (object.ReferenceEquals(thisCell, cells[0]))
        //                {
        //                    colIndex = col.DisplayIndex;
        //                }
        //            }
        //        }
        //    }
        //}

        //private void ProductsGrid_MouseMove(object sender, MouseEventArgs e)
        //{
        //    int rowIndex, colIndex;
        //    object dataContext;
        //    GetGridRowColumnIndex(e.GetPosition(null), ProductsGrid, out rowIndex,
        //      out colIndex, out dataContext);
        //    SelectedRow.Text = string.Format("[Page={0}], [Row={1}] ", ProductsPager.PageIndex, rowIndex);
        //    SelectedColumn.Text = string.Format(" [Cell={0}] ", colIndex);
        //    if (null != dataContext)
        //    {
        //        var prod = dataContext as Product;
        //        ProductInfo.Text = string.Format("[{0}, {1}] ", prod.Name, prod.Color);
        //    }
        //}

    }
}

