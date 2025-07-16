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
using System.Xml.Linq;
using System.Windows.Data;
using System.Reflection;
using CIMS.Forms;
using System.IO;
using System.Windows.Resources;
using CIMS;
using System.Windows.Browser;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
//using PalashDynamics.UserInforServiceRef;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Administration;
using System.ComponentModel;
using PalashDynamics.Forms;
using System.Windows.Media.Imaging;
using PalashDynamics.IVF.DashBoard;
using MessageBoxControl;

namespace PalashDynamics
{
    public partial class MainPage : UserControl
    {
        //public class MenuItem
        //{
        //    public string Id { get; set; }
        //    public string Title { get; set; }
        //    public string Parent { get; set; }
        //    public string ImagePath { get; set; }
        //    public string Tooltip { get; set; }
        //    public string Header { get; set; }
        //    public string Configuration { get; set; }
        //    public string Module { get; set; }
        //    public string Action { get; set; }
        //    public string Mode { get; set; }
        //} 
        bool IsPasswordChanged = true;
        public bool isCRMMIS = false;
        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            dt.Interval = new TimeSpan(0, 0, 0, 1);
            dt.Tick += new EventHandler(dt_Tick);
            dt.Start();
        }
        void dt_Tick(object sender, EventArgs e)
        {
            App application = (App)Application.Current;
            int finalval = Convert.ToInt32(application.SampleVal1);
            if (App.CounterI > finalval)
            {
                MainGrid.Visibility = Visibility.Visible;
                dt.Tick -= new EventHandler(dt_Tick);
            }

            App.CounterI = App.CounterI + 1;
        }
        private void FillChildMenu(List<clsMenuVO> Items)
        {
            if (isCRMMIS == false)
                MenuTree.Children.Clear();
            foreach (var item in Items)
            {

                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsProcessingUnit == false && item.Action == " PalashDynamics.Pathology.FrmExternalRegistration")
                {
                }
                else
                {
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    Image img = new Image();
                    //img.Source = new BitmapImage(new Uri(item.ImageSrc, UriKind.Relative));
                    img.SetBinding(Image.SourceProperty, new Binding("ImagePath"));
                    img.Margin = new Thickness(4, 4, 0, 4);
                    img.Height = 16;
                    sp.Children.Add(img);
                    HyperlinkButton hb = new HyperlinkButton();
                    hb.SetBinding(HyperlinkButton.ContentProperty, new Binding("Title"));
                    hb.IsTabStop = false;
                    hb.TargetName = "_blank";
                    hb.VerticalAlignment = VerticalAlignment.Center;
                    hb.HorizontalAlignment = HorizontalAlignment.Right;
                    hb.Margin = new Thickness(4);
                    hb.Foreground = new SolidColorBrush(Colors.Black);
                    hb.Click += new RoutedEventHandler(Menu_Click);
                    sp.Children.Add(hb);
                    sp.DataContext = item;
                    MenuTree.Children.Add(sp);
                }
            }
        }

        public UserControl PatientView { get; set; }
        HyperlinkButton Selectedhb;

        void Menu_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            HyperlinkButton hb = (HyperlinkButton)sender;
            Selectedhb = hb;
            clsMenuVO Item = (clsMenuVO)((StackPanel)hb.Parent).DataContext;
            SampleSubHeader.Text = ":" + Item.Title;
            SampleHeader.Text = Item.Header;

            ////Added by Ramesh 8Mar2017
            //if (Item.SOPFileName != null)
            //{
            //    SOPFileLink.Visibility = Visibility.Visible;
            //    SOPFileLink.Content = Item.SOPFileName;
            //}

            if (ModuleTypeList.ContainsKey(Item.Action))
            {
                UIElement myData = null;
                myData = (UIElement)Activator.CreateInstance(ModuleTypeList[Item.Action]);
                ActivateUIElement(myData, Item.Parent, Item.Mode);
            }
            else
            {
                if (Item.Module != null && Item.Module != string.Empty && Item.Action != null && Item.Action != string.Empty)
                {
                    WebClient c = new WebClient();
                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                    c.OpenReadAsync(new Uri(Item.Module + ".xap", UriKind.Relative));
                }
            }
        }

        Dictionary<String, Type> ModuleTypeList = new Dictionary<string, Type>();
        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                clsMenuVO Item = (clsMenuVO)((StackPanel)Selectedhb.Parent).DataContext;
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == (Item.Module + ".dll"))
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Item.Action) as UIElement;
                if (myData != null)
                { 
                    ModuleTypeList.Add(Item.Action, myData.GetType());
                    ActivateUIElement(myData, Item.Parent, Item.Mode);
                    if (Item.Title == "EMR")
                    {
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null )
                        {
                            PART_MaximizeToggle.IsChecked = true;
                            DesignBar.Visibility = Visibility.Collapsed;
                            PART_MaximizeToggle.Visibility = Visibility.Collapsed;
                            CmdGraphicalAnalysis.Visibility = Visibility.Collapsed;
                            CmdcryoBank.Visibility = Visibility.Collapsed;
                            CmdGotoHome.Visibility = Visibility.Collapsed;
                            cmdChangePassword.Visibility = Visibility.Collapsed;
                            AddSOPFile.Visibility = Visibility.Collapsed;
                        }
                    }
                   
                  
                }
            }
            catch (Exception ex)
            {
                HtmlPage.Window.Alert("Not Implemented yet"); //Remove this message box letter
            }
        }

        private void ActivateUIElement(UIElement myElement, string ElementTitle, string Mode)
        {
            //if (myElement is IInitiateCIMS)
            //{
            //    ((IInitiateCIMS)myElement).Initiate(Mode);
            //}

            //if (myElement is UserControl)
            //{
            //    MainRegion.Content = myElement;
            //}
            //else if (myElement is ChildWindow)
            //{
            //    ((ChildWindow)myElement).Show();
            //}
            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                {
                    if (ElementTitle == "Surrogacy")
                        ((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate = true;
                    else
                        ((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate = false;
                }
            }
            catch (Exception)
            {
                
                //throw;
            }
           

            if (ElementTitle == "Surrogacy" && myElement is IInitiateCIMSIVF)
            {
                clsMenuVO Item = (clsMenuVO)((StackPanel)Selectedhb.Parent).DataContext;
                ((IInitiateCIMSIVF)myElement).Initiate(Item);
            }
            else if (myElement is IInitiateCIMS)
            {
                ((IInitiateCIMS)myElement).Initiate(Mode);
                if (myElement.ToString() == "EMR.frmEMR")
                {
                    //UserControl rootPage = Application.Current.RootVisual as UserControl;
                    PART_MaximizeToggle.IsChecked = false;
                }
            }

            if (myElement is UserControl)
            {
                MainRegion.Content = myElement;
                if (myElement.ToString() == "EMR.frmEMR")
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                    {
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                        {
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            PART_MaximizeToggle.IsChecked = true;
                            DesignBar.Visibility = Visibility.Collapsed;
                            PART_MaximizeToggle.Visibility = Visibility.Collapsed;
                            CmdGraphicalAnalysis.Visibility = Visibility.Collapsed;
                            CmdcryoBank.Visibility = Visibility.Collapsed;
                            CmdGotoHome.Visibility = Visibility.Collapsed;
                            cmdChangePassword.Visibility = Visibility.Collapsed;
                            cmdIVFDashBoard.Visibility = Visibility.Collapsed;
                            AddSOPFile.Visibility = Visibility.Collapsed;

                        }
                    }
                }
                else
                {
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    PART_MaximizeToggle.IsChecked = false;
                }
            }
            else if (myElement is ChildWindow)
            {
                ((ChildWindow)myElement).Show();
            }
        }

        List<clsMenuVO> MenuList = new List<clsMenuVO>();
       
        public void LoadMenuList()
        {
            clsGetMenuListBizActionVO BizActionObject = new clsGetMenuListBizActionVO() { ID = 0 };
            BizActionObject.MenuList = new List<clsMenuVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    if (((clsGetMenuListBizActionVO)ea.Result).MenuList != null)
                    {
                        MenuList = ((clsGetMenuListBizActionVO)ea.Result).MenuList;
                        SampleHeader.Text = "Home";
                        UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.Home") as UIElement;
                        MainRegion.Content = myData;
                        App.Configuration = new ConfigureDashboard();
                        RequestXML("Home");
                        cmdShortcutHeader.Content = "Home";

                        //MenuList = ((clsGetMenuListBizActionVO)ea.Result).MenuList;
                        //SampleHeader.Text = "Home_New";
                        //UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.Home_New") as UIElement;
                        //MainRegion.Content = myData;
                        //App.Configuration = new ConfigureDashboard();
                        //RequestXML("Home_New");
                        //cmdShortcutHeader.Content = "Home_New";
                    }
                }
            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        public Image myImage = null;
        public void GetUserMenu()
        {
            MenuList = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.RoleDetails.MenuList;

            for (int i = 0; i < MenuList.Count; i++)
            {
                if (MenuList[i].ParentId == 0)
                {
                    if (MenuList[i].Title == "Operation Theatre")
                    {
                        cmdOT.Visibility = Visibility.Visible;
                    }

                    if (MenuList[i].Title == "Find Patient")
                    {
                        cmdFindPatient.Visibility = Visibility.Visible;
                    }

                    if (MenuList[i].Title == "Appointment")
                    {
                        cmdAppointment.Visibility = Visibility.Visible;
                    }

                    if (MenuList[i].Title == "Queue Management")
                    {
                        cmdQueueManagment.Visibility = Visibility.Visible;
                    }

                    if (MenuList[i].Title == "Pathology")  //if (MenuList[i].Title == "Lab Diagnostic")
                    {
                        cmdPathology.Visibility = Visibility.Visible;
                    }

                    if (MenuList[i].Title == "Billing")
                    {
                        BillingToggleButton.Visibility = Visibility.Visible;
                    }

                    if (MenuList[i].Title == "MIS")
                    {
                        cmdMIS.Visibility = Visibility.Visible;
                    }

                    if (MenuList[i].Title == "Administration")
                    {
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                            cmdAdministration.Visibility = Visibility.Visible;
                        else
                            cmdAdministration.Visibility = Visibility.Collapsed;
                    }

                    if (MenuList[i].Title == "Inventory")
                    {
                        cmdPharmacy.Visibility = Visibility.Visible;
                    }
                    if (MenuList[i].Title == "Procedures")
                    {
                        cmdOT.Visibility = Visibility.Visible;
                    }
                    if (MenuList[i].Title == "Radiology")
                    {
                        cmdRadiology.Visibility = Visibility.Visible;
                    }
                    if (MenuList[i].Title == "IVF Lab")
                    {
                        cmdIVFWork.Visibility = Visibility.Visible;
                    }
                    if (MenuList[i].Title == "Cryo")
                    {
                        cmdCryo.Visibility = Visibility.Visible;
                    }
                    if (MenuList[i].Title == "Andrology")
                    {
                        cmdClinical.Visibility = Visibility.Visible;
                    }
                    if (MenuList[i].Title == "PRO")
                    {
                        cmdPRO.Visibility = Visibility.Visible;
                    }
                    if (MenuList[i].Title == "Surrogacy")
                    {
                        cmdSurrogacy.Visibility = Visibility.Visible;
                    }
                    if (MenuList[i].Title == "Donor")
                    {
                        cmdDonor.Visibility = Visibility.Visible;
                    }
                    if (MenuList[i].Title == "ANC")
                    {
                        cmdANC.Visibility = Visibility.Visible;
                    }
                    if (MenuList[i].Title == "IVFDashboard")
                    {
                        cmdIVFDashBoard.Visibility = Visibility.Visible;

                        myImage = new Image();
                        BitmapImage bmi = new BitmapImage();
                        bmi.UriSource = new Uri(@"../Icons/category.png", UriKind.Relative);
                        myImage.Source = bmi;
                        cmdIVFDashBoard.Content = myImage;

                    }
                    if (MenuList[i].Title == "IPD")
                    {
                        cmdIPD.Visibility = Visibility.Visible;
                    }

                    #region For Nursing 16022017

                    if (MenuList[i].Title == "Nursing")
                    {
                        cmdNursing.Visibility = Visibility.Visible;
                    }

                    #endregion

                }
            }
            //commented by rohini 22/10/2015
            //SampleHeader.Text = "Home";
            //UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.Home") as UIElement;
            //MainRegion.Content = myData;
            //App.Configuration = new ConfigureDashboard();
            //RequestXML("Home");
            //cmdShortcutHeader.Content = "Home";


            SampleHeader.Text = "Home";
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.Home_New") as UIElement;
            MainRegion.Content = myData;
            App.Configuration = new ConfigureDashboard();
            RequestXML("Home");
            cmdShortcutHeader.Content = "Home";
        }
        //private XDocument xDoc = null;
        //public void RequestXML(string Parent)
        //{
        //    if (xDoc == null)
        //    {
        //        WebClient _Client = new WebClient();
        //        _Client.DownloadStringAsync(new Uri(@"Data\Menus.xml", UriKind.RelativeOrAbsolute));
        //        _Client.DownloadStringCompleted += (s, e) =>
        //            {
        //                if (e.Error == null)
        //                {
        //                    xDoc = XDocument.Parse(e.Result);
        //                    var books = from r in xDoc.Descendants("MenuItem")
        //                                where r.Element("Parent").Value == Parent
        //                                select new MenuItem
        //                                {
        //                                    Title = r.Element("Title").Value,
        //                                    ImagePath = r.Element("Image").Value,
        //                                    Tooltip = r.Element("Title").Value,
        //                                    Parent = r.Element("Parent").Value,
        //                                    Header = r.Element("Header").Value,
        //                                    Module = r.Element("Module").Value,
        //                                    Action = r.Element("Action").Value,
        //                                    Configuration = r.Element("Configuration").Value,
        //                                    Mode = r.Element("Mode").Value
        //                                };
        //                    FillChildMenu(books.ToList());
        //                }
        //            };
        //    }
        //    else
        //    {
        //        var books = from r in xDoc.Descendants("MenuItem")
        //                    where r.Element("Parent").Value == Parent
        //                    select new MenuItem
        //                    {
        //                        Title = r.Element("Title").Value,
        //                        ImagePath = r.Element("Image").Value,
        //                        Tooltip = r.Element("Title").Value,
        //                        Parent = r.Element("Parent").Value,
        //                        Header = r.Element("Header").Value,
        //                        Module = r.Element("Module").Value,
        //                        Action = r.Element("Action").Value,
        //                        Configuration = r.Element("Configuration").Value,
        //                        Mode = r.Element("Mode").Value
        //                    };
        //        FillChildMenu(books.ToList());
        //    }
        //}

        public void RequestXML(string Parent)
        {
            if (MenuList.Count > 0)
            {
                SampleHeader.Text = Parent;
                SampleSubHeader.Text = "";
                var Menus = from r in MenuList
                            where r.Parent == Parent && r.Status == true
                            orderby r.MenuOrder
                            select new clsMenuVO
                            {
                                Status = r.Status,
                                Title = r.Title,
                                ImagePath = r.ImagePath,
                                Tooltip = r.Title,
                                Parent = r.Parent,
                                Header = r.Header,
                                Module = r.Module,
                                Action = r.Action,
                                Configuration = r.Configuration,
                                Mode = r.Mode,
                                MenuOrder = r.MenuOrder
                            };
                FillChildMenu(Menus.ToList());
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.DateTime.Now.ToShortDateString() != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ApplicationDateTime.ToShortDateString())
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Server date and system date are not matching change the system date and try again.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    ((IApplicationConfiguration)App.Current).CurrentUser = null;
                    Uri addressLogout = new Uri(Application.Current.Host.Source, "../Logout.aspx");
                    HtmlPage.Window.Navigate(addressLogout);
                };
                msgW1.Show();
            }
            else
            {
                //Add this code 
                App.MainPage = this;
                ConfigureSeprator.Visibility = Visibility.Collapsed;
                if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.FirstPasswordChanged == false)
                {
                    OpenChangePassword();
                }
                else
                {
                    // LoadMenuList();
                    // GetParentMenu();
                    //string username = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UserName;
                    //if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.RoleDetails.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AdminRoleID)
                    //    lnkMyProfile.Visibility = System.Windows.Visibility.Visible;

                    if (((IApplicationConfiguration)App.Current).CurrentUser.ID == 1)
                        cmdChangePassword.Visibility = Visibility.Collapsed;

                    GetUserMenu();
                    txtUserName.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
                    txtUserUnit.Text = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitName;
                    txtUnitCounter.Text = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterName;
                }
            }
        }

        private void OpenChangePassword()
        {
            App.MainPage = this;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            SampleHeader.Text = "";
            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.frmFirstChangePassword") as UIElement;
            App.Configuration = new ConfigureDashboard();
            frmFirstChangePassword win = new frmFirstChangePassword();
            win = (frmFirstChangePassword)mydata;
            win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
            win.Show();
        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            GetUserMenu();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                PART_MaximizeToggle.IsChecked = false;
            }
        }

        private void FindPatientToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            SampleHeader.Text = "Patients";
            SampleSubHeader.Text = "";
            cmdShortcutHeader.Content = "Find Patient";
            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
            ((IInitiateCIMS)mydata).Initiate("REG");
            MainRegion.Content = mydata;
            App.Configuration = new ConfigureDashboard();

            RequestXML("Find Patient");
            cmdShortcutHeader.Content = "Find Patient";
        }

        private void AppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainPage = this;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            SampleHeader.Text = "Appointment List";
            SampleSubHeader.Text = "";
            try
            {
                WebClient ac = new WebClient();
                ac.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("OPDModule.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);
                        string Action = "CIMS.Forms.AppointmentList";
                        myData = asm.CreateInstance(Action) as UIElement;
                        if (myData != null)
                        {
                            //ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            RequestXML("Appointment List");
                            cmdShortcutHeader.Content = "Appointment List";
                        }
                    }
                    catch (Exception ex)
                    {
                        HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
                    }
                };
                ac.OpenReadAsync(new Uri("OPDModule.xap", UriKind.Relative));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void QueueManagement_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;
            SampleHeader.Visibility = Visibility.Visible;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            SampleHeader.Text = "Patient Queue List";
            SampleSubHeader.Text = "";

            try
            {
                WebClient ac1 = new WebClient();
                ac1.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("OPDModule.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);
                        string Action = "CIMS.Forms.QueueManagement";
                        myData = asm.CreateInstance(Action) as UIElement;
                        //ModuleTypeList.Add(Action, myData.GetType());
                        if (myData != null)
                        {
                            //  ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            RequestXML("Queue Management");
                            cmdShortcutHeader.Content = "Queue";
                            SampleSubHeader.Text = "";
                            SampleHeader.Text = "Patient Queue List";
                        }
                    }
                    catch (Exception ex)
                    {
                        HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
                    }
                };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
                ac1.OpenReadAsync(new Uri("OPDModule.xap", UriKind.Relative));

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Investigations_Click(object sender, RoutedEventArgs e)
        {
            //App.MainPage = this;
            ////CmdSave.Visibility = Visibility.Collapsed;
            ////CmdClose.Visibility = Visibility.Collapsed;
            //ConfigureSeprator.Visibility = Visibility.Collapsed;
            //SampleHeader.Text = "Pathology";
            //cmdShortcutHeader.Content = "Pathology";
            //PatientList p = new PatientList();
            //MainRegion.Content = p;
            ////UIElement mydata = null;
            //MainRegion.Content = p;
            //RequestXML("Pathology");

            #region Commented by Saily P

            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
            App.MainPage = this;
            SampleHeader.Visibility = Visibility.Visible;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            SampleHeader.Text = "Clinical Work Order";
            SampleSubHeader.Text = "";

            try
            {
                WebClient ac1 = new WebClient();
                ac1.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("PalashDynamics.Pathology.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);

                        string Action = "PalashDynamics.Pathology.PathologyForms.NewPathologyWorkOrderGeneration";

                        myData = asm.CreateInstance(Action) as UIElement;
                        //ModuleTypeList.Add(Action, myData.GetType());

                        if (myData != null)
                        {
                            //  ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            RequestXML("Pathology");
                            cmdShortcutHeader.Content = "Pathology";
                            SampleSubHeader.Text = "";
                            SampleHeader.Text = "Clinical Work Order List";
                        }

                    }
                    catch (Exception ex)
                    {
                        HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
                    }
                };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
                ac1.OpenReadAsync(new Uri("PalashDynamics.Pathology.xap", UriKind.Relative));

            }
            catch (Exception ex)
            {
                throw;
            }

            #endregion

            //NewSaveSeprator.Visibility = Visibility.Collapsed;
            //ConfigureSeprator.Visibility = Visibility.Collapsed;
            //SampleHeader.Text = "Lab Diagnostic";
            //cmdShortcutHeader.Content = "Lab Diagnostic";
            //SampleSubHeader.Text = "";

            //UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
            //((IInitiateCIMS)myData).Initiate("REG");
            //MainRegion.Content = myData;
            //App.Configuration = new ConfigureDashboard();
            //RequestXML("Pathology");
        }

        bool IsBilling = false;
        private void Billing_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;
            //CmdSave.Visibility = Visibility.Collapsed;
            //CmdClose.Visibility = Visibility.Collapsed;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            SampleHeader.Text = "Billing";
            cmdShortcutHeader.Content = "Billing";
            // UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
            DatePicker mydtp = new DatePicker();
            DatePicker mydtp1 = new DatePicker();
            PatientList p = new PatientList();
            // PatientList p = new PatientList(DateTime.Now.Date, DateTime.Now.Date);
            //p.dtpFromDate.SelectedDate = DateTime.Now.Date;
            //p.dtpToDate.SelectedDate = DateTime.Now.Date;
            MainRegion.Content = p;
            RequestXML("Billing");


            #region BHUSHAN For Dash Board
            //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            //App.MainPage = this;
            //SampleHeader.Visibility = Visibility.Visible;
            //ConfigureSeprator.Visibility = Visibility.Collapsed;
            //SampleHeader.Text = "Biling Dashboard";
            //SampleSubHeader.Text = "";
            //cmdShortcutHeader.Content = "Billing";

            //try
            //{
            //    WebClient ac1 = new WebClient();
            //    ac1.OpenReadCompleted += (s, arg) =>
            //    {
            //        try
            //        {
            //            UIElement myData = null;
            //            string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

            //            XElement deploy = XDocument.Parse(appManifest).Root;
            //            List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
            //                                    where (assemblyParts.Attribute("Source").Value == ("OPDModule.dll"))
            //                                    select assemblyParts).ToList();
            //            Assembly asm = null;
            //            AssemblyPart asmPart = new AssemblyPart();
            //            StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
            //            asm = asmPart.Load(streamInfo.Stream);

            //            string Action = "OPDModule.DashBoard.BillingDashBoard";

            //            myData = asm.CreateInstance(Action) as UIElement;
            //            //ModuleTypeList.Add(Action, myData.GetType());

            //            if (myData != null)
            //            {
            //                //ActivateUIElement(myData, Item.Title, Item.Mode);
            //                MainRegion.Content = myData;
            //                RequestXML("Billing");
            //                SampleHeader.Text = "Billing Dashboard";
            //                SampleSubHeader.Text = "";
            //                cmdShortcutHeader.Content = "Billing";
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
            //        }
            //    };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
            //    ac1.OpenReadAsync(new Uri("OPDModule.xap", UriKind.Relative));

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            #endregion
        }

        private void NewPatientButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void EditPatientToggleButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CmdHome_Click(object sender, RoutedEventArgs e)
        {
            //CmdSave.Visibility = Visibility.Collapsed;
            //CmdClose.Visibility = Visibility.Collapsed;
            //PatientNewSeprator.Visibility = Visibility.Collapsed;
            NewSaveSeprator.Visibility = Visibility.Collapsed;
            ConfigureSeprator.Visibility = Visibility.Collapsed;


            //by rohini           
            SampleHeader.Text = "Home";
            cmdShortcutHeader.Content = "Home";
            SampleSubHeader.Text = "";

            //UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.Home") as UIElement;
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.Home_New") as UIElement;
            MainRegion.Content = myData;
            App.Configuration = new ConfigureDashboard();         
            RequestXML("Home");
        }

        private void CmdSkin_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CmdConfigure_Click(object sender, RoutedEventArgs e)
        {
            if (App.Configuration != null)
            {
                App.Configuration.Show();
            }
        }

        object temp;
        private void PART_MaximizeToggle_Checked(object sender, RoutedEventArgs e)
        {
            object u = MainContainer2.Content;
            MainContainer2.Content = null;
            temp = MainContainer.Content;
            MainContainer.Content = u;
        }

        private void PART_MaximizeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            object u = MainContainer.Content;
            MainContainer.Content = temp;
            MainContainer2.Content = u;
        }

        private void PART_MaximizeToggle_Click(object sender, RoutedEventArgs e)
        {

        }

        public void OpenMainRegion(string Action)
        {
            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance(Action) as UIElement;

            if (mydata != null)
            {
                SampleSubHeader.Text = "";
                MainRegion.Content = mydata;
                App.Configuration = new ConfigureDashboard();
            }
        }

        public void GlobalOpenMainRegion(UIElement UIelement)
        {
            if (UIelement != null)
            {
                SampleSubHeader.Text = "";
                MainRegion.Content = UIelement;
                App.Configuration = new ConfigureDashboard();
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {

            App.MainPage = this;
            //CmdSave.Visibility = Visibility.Collapsed;
            //CmdClose.Visibility = Visibility.Collapsed;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            cmdShortcutHeader.Content = "Administration";
            SampleHeader.Text = "Administration";
            SampleSubHeader.Text = "";
            UIElement mydata = null;
            MainRegion.Content = mydata;
            RequestXML("Administration");

            //UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
            #region commented

            //try
            //{
            //    WebClient ac1 = new WebClient();
            //    ac1.OpenReadCompleted += (s, arg) =>
            //    {
            //        try
            //        {
            //            UIElement myData = null;
            //            string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

            //            XElement deploy = XDocument.Parse(appManifest).Root;
            //            List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
            //                                    where (assemblyParts.Attribute("Source").Value == ("PalashDynamics.Administration.dll"))
            //                                    select assemblyParts).ToList();
            //            Assembly asm = null;
            //            AssemblyPart asmPart = new AssemblyPart();
            //            StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
            //            asm = asmPart.Load(streamInfo.Stream);

            //            //string Action = "PalashDynamics.Administration.SystemConfiguration";
            //            string Action = "CIMS.Forms.frmAddUser";
            //            myData = asm.CreateInstance(Action) as UIElement;
            //            //ModuleTypeList.Add(Action, myData.GetType());

            //            if (myData != null)
            //            {
            //                //  ActivateUIElement(myData, Item.Title, Item.Mode);
            //                MainRegion.Content = myData;
            //                App.Configuration = new ConfigureDashboard();
            //                RequestXML("Administration");
            //                SampleHeader.Text = "System Configuration";
            //                cmdShortcutHeader.Content = "Administration";
            //                SampleSubHeader.Text = "";                            
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
            //        }
            //    };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
            //    ac1.OpenReadAsync(new Uri("PalashDynamics.Administration.xap", UriKind.Relative));

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

            ////MainRegion.Content = mydata;
            ////App.Configuration = new ConfigureDashboard();
            ////RequestXML("Administration");
            #endregion
        }

        private void cmdPharmacy_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;
            SampleHeader.Visibility = Visibility.Visible;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            SampleHeader.Text = "Inventory Dashboard";
            SampleSubHeader.Text = "";
            cmdShortcutHeader.Content = "Inventory";

            try
            {
                WebClient ac1 = new WebClient();
                ac1.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("PalashDynamics.Pharmacy.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);

                        string Action = "PalashDynamics.Pharmacy.Inventory_DashBoard.InventoryDashBoard_New";//"PalashDynamics.Pharmacy.InventoryDashBoard_New";

                        myData = asm.CreateInstance(Action) as UIElement;
                        //ModuleTypeList.Add(Action, myData.GetType());

                        if (myData != null)
                        {
                            //ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            RequestXML("Inventory");
                            SampleHeader.Text = "Inventory Dashboard";
                            SampleSubHeader.Text = "";
                            cmdShortcutHeader.Content = "Inventory";
                        }

                    }
                    catch (Exception ex)
                    {
                        HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
                    }
                };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
                ac1.OpenReadAsync(new Uri("PalashDynamics.Pharmacy.xap", UriKind.Relative));

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void cmdCRM_Click(object sender, RoutedEventArgs e)
        {
            App.MainPage = this;
            //CmdSave.Visibility = Visibility.Collapsed;
            //CmdClose.Visibility = Visibility.Collapsed;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            cmdShortcutHeader.Content = "MIS";
            SampleSubHeader.Text = "";
            SampleHeader.Text = "MIS";
            //UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.Home") as UIElement;
            UIElement mydata = null;
            MainRegion.Content = mydata;
            RequestXML("CRM");
            isCRMMIS = true;
            RequestXML("MIS");
            isCRMMIS = false;
        }

        private void cmdMIS_Click(object sender, RoutedEventArgs e)
        {
            //App.MainPage = this;
            //ConfigureSeprator.Visibility = Visibility.Collapsed;
            //cmdShortcutHeader.Content = "MIS";
            //SampleHeader.Text = "MIS";
            //SampleSubHeader.Text = "";
            //UIElement mydata = null;
            //MainRegion.Content = mydata;
            //RequestXML("MIS");
            App.MainPage = this;
            //CmdSave.Visibility = Visibility.Collapsed;
            //CmdClose.Visibility = Visibility.Collapsed;

            ConfigureSeprator.Visibility = Visibility.Collapsed;
            cmdShortcutHeader.Content = "MIS";
            SampleSubHeader.Text = "";
            SampleHeader.Text = "MIS";
            //UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.Home") as UIElement;
            UIElement mydata = null;
            MainRegion.Content = mydata;
            RequestXML("CRM");
            isCRMMIS = true;
            RequestXML("MIS");
            isCRMMIS = false;

        }

        private void cmdChangePassword_Click(object sender, RoutedEventArgs e)
        {
            frmChangePassword myfrm = new frmChangePassword();
            ChildWindow mywin = new ChildWindow();
            //mywin.HasCloseButton = false;
            mywin.Title = "Change Password";
            //mywin.Height = 400;
            //mywin.Width = 475;
            mywin.Content = myfrm;
            mywin.Show();

        }

        private void UpdateAudtiTrail()
        {
            clsUserVO User = null;
            User = ((IApplicationConfiguration)App.Current).CurrentUser;
            clsUpdateAuditTrailBizActionVO BizAction = new clsUpdateAuditTrailBizActionVO();
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            client1.ProcessCompleted += (s, ea) =>
            {
                ((IApplicationConfiguration)App.Current).CurrentUser = null;
                Uri address11 = new Uri(Application.Current.Host.Source, "../Logout.aspx");
                HtmlPage.Window.Navigate(address11);
            };
            client1.ProcessAsync(BizAction, User);// new clsUserVO());
            client1.CloseAsync();
            client1 = null;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.SetSessionUserCompleted += (s, ea) =>
            {
                ////UpdateAudtiTrail();

                clsUserVO User = null;
                User = ((IApplicationConfiguration)App.Current).CurrentUser;
                clsUpdateAuditTrailBizActionVO BizAction = new clsUpdateAuditTrailBizActionVO();

                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

                client1.ProcessCompleted += (s1, ea1) =>
                {
                    ((IApplicationConfiguration)App.Current).CurrentUser = null;

                    Uri address11 = new Uri(Application.Current.Host.Source, "../Logout.aspx");
                    HtmlPage.Window.Navigate(address11);
                };
                client1.ProcessAsync(BizAction, User);// new clsUserVO());
                client1.CloseAsync();
                client1 = null;
                //((IApplicationConfiguration)App.Current).CurrentUser = null;
                //Uri address1 = new Uri(Application.Current.Host.Source, "../Logout.aspx");
                //HtmlPage.Window.Navigate(address1);
            };

            client.SetSessionUserAsync("USER", null);
            client.CloseAsync();
        }

        private void cmdFindPatient_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Radiology_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder = null;

            App.MainPage = this;
            SampleHeader.Visibility = Visibility.Visible;
            ConfigureSeprator.Visibility = Visibility.Collapsed;

            cmdShortcutHeader.Content = "Radiology";
            SampleHeader.Text = "Radiology Work Order";
            SampleSubHeader.Text = "";
            RequestXML("Radiology");
            UIElement mydata = null;
            MainRegion.Content = mydata;

            try
            {
                WebClient ac1 = new WebClient();
                ac1.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("PalashDynamics.Radiology.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);

                        //  string Action = "PalashDynamics.Radiology.WorkOrderGeneration";//Commented By Yogesh k

                        string Action = "PalashDynamics.Radiology.New_RadiologyWorkOrderGeneration";

                        myData = asm.CreateInstance(Action) as UIElement;
                        //ModuleTypeList.Add(Action, myData.GetType());

                        if (myData != null)
                        {
                            //  ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            RequestXML("Radiology");
                            cmdShortcutHeader.Content = "Radiology";
                            SampleSubHeader.Text = "";
                            SampleHeader.Text = "Radiology Work Order List";
                        }

                    }
                    catch (Exception ex)
                    {
                        HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
                    }
                };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
                ac1.OpenReadAsync(new Uri("PalashDynamics.Radiology.xap", UriKind.Relative));

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void cmdOT_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;
            SampleHeader.Visibility = Visibility.Visible;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            cmdShortcutHeader.Content = "Operation Theatre";
            SampleHeader.Text = "OT Booking List";
            SampleSubHeader.Text = "";

            try
            {
                WebClient ac1 = new WebClient();
                ac1.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("PalashDynamics.OperationTheatre.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);
                        string Action = "PalashDynamics.OperationTheatre.frmOTScheduling";
                        myData = asm.CreateInstance(Action) as UIElement;
                        //ModuleTypeList.Add(Action, myData.GetType());
                        if (myData != null)
                        {
                            //  ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            RequestXML("Operation Theatre");
                            cmdShortcutHeader.Content = "Operation Theatre";
                            SampleSubHeader.Text = "";
                            SampleHeader.Text = "OT Booking List";
                        }
                    }
                    catch (Exception ex)
                    {
                        HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
                    }
                };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
                ac1.OpenReadAsync(new Uri("PalashDynamics.OperationTheatre.xap", UriKind.Relative));

            }
            catch (Exception ex)
            {
                throw;
            }

            ////try
            ////{
            ////    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ////    App.MainPage = this;
            ////    //CmdSave.Visibility = Visibility.Collapsed;
            ////    //CmdClose.Visibility = Visibility.Collapsed;
            ////    ConfigureSeprator.Visibility = Visibility.Collapsed;

            ////    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
            ////    MainRegion.Content = mydata;
            ////    App.Configuration = new ConfigureDashboard();

            ////    RequestXML("OT");

            ////    SampleHeader.Text = "Operation Theatre :";
            ////    SampleSubHeader.Text = "Find Patients";
            ////    cmdShortcutHeader.Content = "Operation Theatre";
            ////}
            ////catch (Exception ex)
            ////{
            ////    throw;
            ////}

            //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();

            //App.MainPage = this;
            //SampleHeader.Visibility = Visibility.Visible;
            //ConfigureSeprator.Visibility = Visibility.Collapsed;

            //cmdShortcutHeader.Content = "Procedures";
            //SampleHeader.Text = "Procedures Booking List";
            //SampleSubHeader.Text = "";
            //RequestXML("Operation Theatre");

            //UIElement mydata = null;
            //MainRegion.Content = mydata;

            //try
            //{
            //    WebClient ac1 = new WebClient();
            //    ac1.OpenReadCompleted += (s, arg) =>
            //    {
            //        try
            //        {
            //            // UIElement myData = null;
            //            string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

            //            XElement deploy = XDocument.Parse(appManifest).Root;
            //            List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
            //                                    where (assemblyParts.Attribute("Source").Value == ("PalashDynamics.OperationTheatre.dll"))
            //                                    select assemblyParts).ToList();
            //            Assembly asm = null;
            //            AssemblyPart asmPart = new AssemblyPart();
            //            StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
            //            asm = asmPart.Load(streamInfo.Stream);

            //            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
            //            ((IInitiateCIMS)myData).Initiate("REG");
            //            MainRegion.Content = myData;
            //            App.Configuration = new ConfigureDashboard();

            //            //string Action = "PalashDynamics.OperationTheatre.OTScheduling";
            //            //// string Action = "CIMS.Forms.PatientList";
            //            //myData = asm.CreateInstance(Action) as UIElement;
            //            ////ModuleTypeList.Add(Action, myData.GetType());

            //            //if (myData != null)
            //            //{
            //            //    //  ActivateUIElement(myData, Item.Title, Item.Mode);
            //            //    MainRegion.Content = myData;
            //            //    RequestXML("Operation Theatre");
            //            //    cmdShortcutHeader.Content = "Procedures";
            //            //    SampleSubHeader.Text = "";
            //            //    SampleHeader.Text = "Procedures Booking List";
            //            //}

            //        }
            //        catch (Exception ex)
            //        {
            //            HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
            //        }
            //    };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
            //    ac1.OpenReadAsync(new Uri("PalashDynamics.OperationTheatre.xap", UriKind.Relative));

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

        }

        private void cmdIVFWork_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;
            //CmdSave.Visibility = Visibility.Collapsed;
            //CmdClose.Visibility = Visibility.Collapsed;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            SampleHeader.Text = "Patients";
            SampleSubHeader.Text = "";
            cmdShortcutHeader.Content = "IVF Lab";
            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
            ((IInitiateCIMS)mydata).Initiate("REG");
            MainRegion.Content = mydata;
            App.Configuration = new ConfigureDashboard();

            //App.MainPage = this;
            ////CmdSave.Visibility = Visibility.Collapsed;
            ////CmdClose.Visibility = Visibility.Collapsed;
            //ConfigureSeprator.Visibility = Visibility.Collapsed;
            //cmdShortcutHeader.Content = "IVF Lab Work";
            //SampleHeader.Text = "IVF Lab Work";
            //SampleSubHeader.Text = "";
            //UIElement mydata = null;
            //MainRegion.Content = mydata;
            RequestXML("IVF Lab Work");
        }

        private void lnkMyProfile_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                clsGetClientBizActionVO BizAction = new clsGetClientBizActionVO();
                BizAction.Details = new clsClientDetailsVO();

                Uri Address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", Address.AbsoluteUri);

                Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null && ((clsGetClientBizActionVO)arg.Result).Details != null)
                        {
                            long ID = ((clsGetClientBizActionVO)arg.Result).Details.ID;
                            //Testing
                            // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "http://localhost/SHSCloud.Web/ClientControlPanel.aspx?ID=" + ID.ToString()), "_blank");                            
                            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "http://182.18.151.32/SHSCloud.Web/ClientControlPanel.aspx?ID=" + ID.ToString()), "_blank");
                        }
                        else
                        {
                            //MessageBox wd = new MessageBox( );
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

        private void cmdIPD_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;
            SampleHeader.Visibility = Visibility.Visible;
            ConfigureSeprator.Visibility = Visibility.Collapsed;

            cmdShortcutHeader.Content = "IPD";
            SampleHeader.Text = " ";
            SampleSubHeader.Text = "";
            RequestXML("IPD");

            UIElement mydata = null;
            MainRegion.Content = mydata;

            try
            {
                WebClient ac1 = new WebClient();
                ac1.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("PalashDynamics.IPD.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);

                        string Action = "PalashDynamics.IPD.Forms.frmAdmissionList";

                        myData = asm.CreateInstance(Action) as UIElement;
                        //ModuleTypeList.Add(Action, myData.GetType());

                        if (myData != null)
                        {
                            //  ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            RequestXML("IPD");
                            cmdShortcutHeader.Content = "IPD";
                            SampleSubHeader.Text = ": Admission List";
                            SampleHeader.Text = "Admission List ";
                        }
                    }
                    catch (Exception ex)
                    {
                        HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
                    }
                };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
                ac1.OpenReadAsync(new Uri("PalashDynamics.IPD.xap", UriKind.Relative));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdClinical_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;
            SampleHeader.Visibility = Visibility.Visible;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            SampleHeader.Text = "Andrology Queue List";
            SampleSubHeader.Text = "";

            try
            {
                WebClient ac1 = new WebClient();
                ac1.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("OPDModule.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);
                        string Action = "CIMS.Forms.QueueManagement";
                        myData = asm.CreateInstance(Action) as UIElement;
                        ((IInitiateCIMS)myData).Initiate("IsAndrology");
                        //ModuleTypeList.Add(Action, myData.GetType());
                        if (myData != null)
                        {
                            //  ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            //RequestXML("Clinical");
                            RequestXML("Andrology");
                            cmdShortcutHeader.Content = "Andrology";
                            SampleSubHeader.Text = "";
                            SampleHeader.Text = "Andrology Queue List";
                        }
                    }
                    catch (Exception ex)
                    {
                        HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
                    }
                };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
                ac1.OpenReadAsync(new Uri("OPDModule.xap", UriKind.Relative));

            }
            catch (Exception ex)
            {
                throw;
            }
            //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            //App.MainPage = this;

            //ConfigureSeprator.Visibility = Visibility.Collapsed;
            //SampleHeader.Text = "QueueManagement";
            //cmdShortcutHeader.Content = "QueueManagement";

            //ModuleName = "OPDModule";
            //Action = "CIMS.Forms.QueueManagement";
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //WebClient c = new WebClient();
            //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_Clinical);
            //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

            //RequestXML("Clinical");
            //PART_MaximizeToggle.IsChecked = true;
        }
        void c_OpenReadCompleted_Clinical(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("IsAndrology");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData as UIElement);
                SampleHeader.Text = "Queue Management";
                cmdShortcutHeader.Content = "Queue eManagement";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void cmdCryo_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();

            App.MainPage = this;
            SampleHeader.Visibility = Visibility.Visible;
            ConfigureSeprator.Visibility = Visibility.Collapsed;

            cmdShortcutHeader.Content = "Cryo";
            SampleHeader.Text = "Cryo";
            SampleSubHeader.Text = "";
            RequestXML("Cryo");
            UIElement mydata = null;
            MainRegion.Content = mydata;
            try
            {
                WebClient ac1 = new WebClient();
                ac1.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("PalashDynamics.IVF.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);

                        string Action = "PalashDynamics.IVF.frmCryoBankSummary";

                        myData = asm.CreateInstance(Action) as UIElement;
                        //ModuleTypeList.Add(Action, myData.GetType());

                        if (myData != null)
                        {
                            //  ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            RequestXML("Cryo");
                            cmdShortcutHeader.Content = "Cryo";
                            SampleSubHeader.Text = "";
                            SampleHeader.Text = "Cryo";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
                ac1.OpenReadAsync(new Uri("PalashDynamics.IVF.xap", UriKind.Relative));

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdPRO_Click(object sender, RoutedEventArgs e)
        {
            App.MainPage = this;
            //CmdSave.Visibility = Visibility.Collapsed;
            //CmdClose.Visibility = Visibility.Collapsed;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            cmdShortcutHeader.Content = "PRO";
            SampleHeader.Text = "PRO";
            SampleSubHeader.Text = "";
            UIElement mydata = null;
            MainRegion.Content = mydata;
            RequestXML("PRO");
        }

        // By BHUSHAN FOr Surrogacy,.... 
        private void cmdSurrogacy_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            cmdShortcutHeader.Content = "Surrogacy";
            SampleHeader.Text = "Surrogacy";
            SampleSubHeader.Text = "";
            //UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
            //MainRegion.Content = mydata;
            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Forms.PatientView.PatientListForSurrogacy") as UIElement;
            ((IInitiateCIMS)mydata).Initiate("REG");
            MainRegion.Content = mydata;
            RequestXML("Surrogacy");
        }

        private void cmdANC_Click(object sender, RoutedEventArgs e)
        {
            App.MainPage = this;
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            cmdShortcutHeader.Content = "ANC ";
            SampleHeader.Text = "ANC ";
            SampleSubHeader.Text = "";
            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
            ((IInitiateCIMS)mydata).Initiate("REG");
            MainRegion.Content = mydata;
            App.Configuration = new ConfigureDashboard();
            RequestXML("ANC ");

        }
        public string Action { get; set; }
        public string ModuleName { get; set; }
        private void CmdIVFDASHBOARD_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;

            ConfigureSeprator.Visibility = Visibility.Collapsed;
            SampleHeader.Text = "ART Dashboard";
            cmdShortcutHeader.Content = "ART Dashboard";

            ModuleName = "PalashDynamics.IVF";
            Action = "PalashDynamics.IVF.DashBoard.PatientARTAndLabDayDetails";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompletedNew);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

            RequestXML("Clinical");
            PART_MaximizeToggle.IsChecked = true;
        }
        void c_OpenReadCompletedNew(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("New");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData as UIElement);
                SampleHeader.Text = "ART Dashboard";
                cmdShortcutHeader.Content = "ART Dashboard";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CmdgraphocalAnalysis_Click(object sender, RoutedEventArgs e)
        {
            AnalysisDashBoard win = new AnalysisDashBoard();
            win.Show();
        }

        private void SOP_Click(object sender, RoutedEventArgs e)
        {
            string URL = "../SOP/SOP.pdf";
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

            //AddSOPFile win = new AddSOPFile();
            //win.Show();
        }
        



        private void CmdCryoBank_Click(object sender, RoutedEventArgs e)
        {
            CryoBankForDashboard win = new CryoBankForDashboard();
            win.Show();
            //ModuleName = "PalashDynamics.IVF";
            //Action = "PalashDynamics.IVF.DashBoard.CryoBankForDashboard";
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //WebClient c = new WebClient();
            //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompletedCryoBank);
            //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

        }

        private void cmdDonor_Click(object sender, RoutedEventArgs e)
        {
            ConfigureSeprator.Visibility = Visibility.Collapsed;
            cmdShortcutHeader.Content = "Donor";
            SampleSubHeader.Text = "";
            SampleHeader.Text = "Donor";
            //UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.Home") as UIElement;
            UIElement mydata = null;
            MainRegion.Content = mydata;
            RequestXML("Donor");

        }

        private void cmdAddmissionList_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();

            App.MainPage = this;
            SampleHeader.Visibility = Visibility.Visible;
            ConfigureSeprator.Visibility = Visibility.Collapsed;

            cmdShortcutHeader.Content = "Admission List";
            SampleHeader.Text = " ";
            SampleSubHeader.Text = "";
            RequestXML("Admission List");

            UIElement mydata = null;
            MainRegion.Content = mydata;

            try
            {
                WebClient ac1 = new WebClient();
                ac1.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("PalashDynamics.IPD.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);

                        string Action = "PalashDynamics.IPD.Forms.frmAdmissionList";

                        myData = asm.CreateInstance(Action) as UIElement;
                        //ModuleTypeList.Add(Action, myData.GetType());

                        if (myData != null)
                        {
                            //  ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            RequestXML("Admission List");
                            cmdShortcutHeader.Content = "Admission List";
                            //SampleSubHeader.Text = "";
                            //SampleHeader.Text = " ";
                            SampleSubHeader.Text = ": Admission List";
                            SampleHeader.Text = "Admission List ";
                        }

                    }
                    catch (Exception ex)
                    {
                        HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
                    }
                };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
                ac1.OpenReadAsync(new Uri("PalashDynamics.IPD.xap", UriKind.Relative));

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region For Nursing 16022017

        private void cmdNursing_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            App.MainPage = this;
            SampleHeader.Visibility = Visibility.Visible;
            ConfigureSeprator.Visibility = Visibility.Collapsed;

            cmdShortcutHeader.Content = "IPD";
            SampleHeader.Text = " ";
            SampleSubHeader.Text = "";
            RequestXML("IPD");

            UIElement mydata = null;
            MainRegion.Content = mydata;

            try
            {
                WebClient ac1 = new WebClient();
                ac1.OpenReadCompleted += (s, arg) =>
                {
                    try
                    {
                        UIElement myData = null;
                        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(arg.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                        XElement deploy = XDocument.Parse(appManifest).Root;
                        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                                where (assemblyParts.Attribute("Source").Value == ("PalashDynamics.IPD.dll"))
                                                select assemblyParts).ToList();
                        Assembly asm = null;
                        AssemblyPart asmPart = new AssemblyPart();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(arg.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                        asm = asmPart.Load(streamInfo.Stream);

                        string Action = "PalashDynamics.IPD.Forms.frmAdmissionList";

                        myData = asm.CreateInstance(Action) as UIElement;
                        //ModuleTypeList.Add(Action, myData.GetType());

                        if (myData != null)
                        {
                            //  ActivateUIElement(myData, Item.Title, Item.Mode);
                            MainRegion.Content = myData;
                            RequestXML("Nursing");      //RequestXML("IPD");
                            cmdShortcutHeader.Content = "Nursing";
                            SampleSubHeader.Text = ": Admission List";
                            SampleHeader.Text = "Admission List ";
                        }
                    }
                    catch (Exception ex)
                    {
                        HtmlPage.Window.Alert(ex.Message); //Remove this message box letter
                    }
                };//new OpenReadCompletedEventHandler(ac_OpenReadCompleted);
                ac1.OpenReadAsync(new Uri("PalashDynamics.IPD.xap", UriKind.Relative));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public void FormCall()
        {
            ModuleName = "OPDModule";
            Action = "OPDModule.Forms.PatientSearch";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_PatientOpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }
        void c_PatientOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
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
                if (myData is IInitiateCIMS)
                {
                    ((IInitiateCIMS)myData).Initiate(PatientTypes.All.ToString());
                }
                ChildWindow cw = new ChildWindow();
                cw = (ChildWindow)myData;
                cw.Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void MainPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App application = (App)Application.Current;
            int finalval = Convert.ToInt32(application.SampleVal1);
            if (App.CounterI > finalval)
            {
                MainGrid.Visibility = Visibility.Collapsed;
                App.CounterI = 0;
                Logout();
            }
            else
            {
                MainGrid.Visibility = Visibility.Collapsed;
                App.CounterI = 0;
            }
       

        }
        private void Logout()
        {
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.SetSessionUserCompleted += (s, ea) =>
            {
                clsUserVO User = null;
                User = ((IApplicationConfiguration)App.Current).CurrentUser;
                clsUpdateAuditTrailBizActionVO BizAction = new clsUpdateAuditTrailBizActionVO();
               // BizAction.IsLoginFlag = false;
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client1.ProcessCompleted += (s1, ea1) =>
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Session is expired", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();

                    ((IApplicationConfiguration)App.Current).CurrentUser = null;
                 
                    Uri address11 = new Uri(Application.Current.Host.Source, "../session.aspx");
                    HtmlPage.Window.Navigate(address11);
                };
                client1.ProcessAsync(BizAction, User);// new clsUserVO());
                client1.CloseAsync();
                client1 = null;
            };
            client.SetSessionUserAsync("USER", null);
            client.CloseAsync();
        }

        
        //void c_OpenReadCompletedCryoBank(object sender, OpenReadCompletedEventArgs e)
        //{
        //    try
        //    {
        //        UIElement myData = null;
        //        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
        //        XElement deploy = XDocument.Parse(appManifest).Root;
        //        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
        //                                where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
        //                                select assemblyParts).ToList();
        //        Assembly asm = null;
        //        AssemblyPart asmPart = new AssemblyPart();
        //        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
        //        asm = asmPart.Load(streamInfo.Stream);
        //        myData = asm.CreateInstance(Action) as UIElement;
        //       // ((IInitiateCIMS)myData).Initiate("New");
        //        ((IApplicationConfiguration)App.Current).OpenMainContent(myData as UIElement);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
