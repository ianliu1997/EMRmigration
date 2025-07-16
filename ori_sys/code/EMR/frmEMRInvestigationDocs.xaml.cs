using System;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.RSIJ;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;
namespace EMR
{
    public partial class frmEMRInvestigationDocs : UserControl
    {
        #region DataMembers
        public TreeView TreeViewImage = null;
        private double angle = 90;
        private int trackZoom = 0;
        #endregion
        #region Constructor
        public frmEMRInvestigationDocs()
        {
            InitializeComponent();
            scrollViewer_1.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer_1.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            this.Loaded += new RoutedEventHandler(frmEMRInvestigationDocs_Loaded);
        }
        #endregion
        #region Loaded
        private void frmEMRInvestigationDocs_Loaded(object sender, RoutedEventArgs e)
        {
            TreeViewImage = new TreeView();
            LoadVisitAndAdmission(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);           
        }
        #endregion
        //private void PreviewReport_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dgFiles.SelectedItem != null)
        //    {
        //        clsDMSFileVO objFile = dgFiles.SelectedItem as clsDMSFileVO;
        //        string sPath = objFile.ServerAddress.Substring(objFile.ServerAddress.IndexOf("DMSImages"));
        //        HtmlPage.Window.Navigate(new Uri("http://192.168.10.29/" + sPath + "/" + objFile.DocumentName, UriKind.Absolute), "_blank");
        //    }
        //}
        #region DMS Viewer
        private void LoadVisitAndAdmission(string sMRNO)
        {
            ClsGetVisitAdmissionBizActionVO BizAction = new ClsGetVisitAdmissionBizActionVO();
            BizAction.MRNO = sMRNO;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    if (((ClsGetVisitAdmissionBizActionVO)e.Result).DMSViewerVisitAdmissionList != null)
                    {
                        AttachOPDIPDTree(((ClsGetVisitAdmissionBizActionVO)e.Result).DMSViewerVisitAdmissionList);
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void AttachOPDIPDTree(List<clsDMSViewerVisitAdmissionVO> list)
        {
            var rootelements = list.Where(S => S.ParentID.Equals(0));
            foreach (var element in rootelements)
            {
                TreeViewItem rootItem = new TreeViewItem();
                rootItem.Header = element.DoctorName; // title of root element
                rootItem.DataContext = element;
                BuildMenu(list, rootItem, element.ID); // find child menu and add to tree view
                TreeViewImage.Items.Add(rootItem); // add tree view item to tree view
            }
            TreeViewImage.FontSize = 15;
            ImageTree.Children.Add(TreeViewImage);
        }
        private void BuildMenu(List<clsDMSViewerVisitAdmissionVO> list, TreeViewItem rootItem, long ID)
        {
            // find immediate child of parent elemt
            List<clsDMSViewerVisitAdmissionVO> ChildList = (from obj in list
                                                            where obj.ParentID.Equals(ID)
                                                            select obj).ToList();
            foreach (clsDMSViewerVisitAdmissionVO _obj in ChildList)
            {
                TreeViewItem _rootItem = new TreeViewItem(); // add root elemt to tree 
                if (_obj.ParentID.Equals(1))
                    _rootItem.Header = _obj.Date.ToString("dd/MM/yyyy") + " " + _obj.DepartmentName; // add title to item
                else if (_obj.ParentID.Equals(2))
                    _rootItem.Header = _obj.Date.ToString("dd/MM/yyyy") + " " + _obj.DoctorName;
                else
                    _rootItem.Header = _obj.DoctorName;
                _rootItem.DataContext = _obj;
                _rootItem.Selected += new RoutedEventHandler(rootItem_Selected);
                List<clsDMSViewerVisitAdmissionVO> list1 = list.Where(S => S.ParentID.Equals(_obj.ID)).ToList<clsDMSViewerVisitAdmissionVO>();
                if (list.Count > 0)
                    BuildMenu(list, _rootItem, _obj.ID); // recursive call 
                rootItem.Items.Add(_rootItem); // add item to parent item
            }
        }
        void rootItem_Selected(object sender, RoutedEventArgs e)
        {
            clsDMSViewerVisitAdmissionVO objDMSViewer = ((clsDMSViewerVisitAdmissionVO)((TreeViewItem)sender).DataContext);
            string sPath = objDMSViewer.ImgPath.Substring(objDMSViewer.ImgPath.IndexOf("DMSImages"));
            sPath = sPath.Replace(@"\", "/");
            DisplayImg.Source = new BitmapImage(new Uri(@"http://192.168.10.29/" + sPath + "/" + objDMSViewer.ImgName, UriKind.RelativeOrAbsolute));
        }
        private void CmdZoomPlus_Click(object sender, RoutedEventArgs e)
        {
            if (trackZoom != 10)
            {
                /* Increase Height And Width Of Image */
                DisplayImg.Height = DisplayImg.Height + 50;
                DisplayImg.Width = DisplayImg.Width + 100;

                /* Increase Height And Width Of Image */
                viewBox_Image.Height = viewBox_Image.Height + 50;
                viewBox_Image.Width = viewBox_Image.Width + 100;

                trackZoom = trackZoom + 1;
            }
        }
        private void cmbZoomMinus_Click(object sender, RoutedEventArgs e)
        {
            if (trackZoom != 0)
            {
                /* Decrease Height And Width Of Image */
                DisplayImg.Height = DisplayImg.Height - 50;
                DisplayImg.Width = DisplayImg.Width - 100;

                /* Decrease Height And Width Of ViewBox */
                viewBox_Image.Height = viewBox_Image.Height - 50;
                viewBox_Image.Width = viewBox_Image.Width - 100;

                trackZoom = trackZoom - 1;
            }

        }
        private void cmbRotation_Click(object sender, RoutedEventArgs e)
        {
            /* Set And Reset Angle Of Rotator */
            Rotator.Angle = angle;
            angle = angle + 90;

            if (angle > 360)
            {
                angle = 90; /* Reset Angle To 90 */
            }
        }
        private void mainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /* Set Height And Width Of Image To The Actual Height Of Main Grid */
            DisplayImg.Height = mainGrid.ActualHeight;
            DisplayImg.Width = mainGrid.ActualHeight;

            /* Similarly Set Height And Width Of ViewBox To The Actual Height Of Main Grid */
            viewBox_Image.Height = mainGrid.ActualHeight;
            viewBox_Image.Width = mainGrid.ActualWidth;

        }
        #endregion
    }
}

