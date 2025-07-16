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
using PalashDynamics;
using CIMS;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.User;
using System.Windows.Browser;
using System.Globalization;
using System.Threading;
using PalashDynamic.Localization;

namespace PalashDynamics
{
    public partial class App : Application,IApplicationConfiguration
    {
        private LocalizationManager _manager = new LocalizationManager();
        public ObservableResources<PalashDynamic.Localization.Localized> _resources = new ObservableResources<PalashDynamic.Localization.Localized>(new PalashDynamic.Localization.Localized());
        public static List<UIClassElement> ObjUIClass = new List<UIClassElement>();

        long Click = 0;
        public static int CounterI = 0;
        private bool idle;

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        public int SampleVal1 = 0;
        //Ramesh 9Mar2017 Multi User Session Management
        public int LiveTimeVal1 = 30; 

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IDictionary<string, string> SampleVal = e.InitParams;
            foreach (var item in SampleVal)
            {
                if (item.Key == "TimeForSession")
                {
                    SampleVal1 = Convert.ToInt32(item.Value);
                }

                //Ramesh 9Mar2017 Multi User Session Management
                if (item.Key == "LiveTimeSession")
                {
                    LiveTimeVal1 = Convert.ToInt32(item.Value);
                }
            }
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.GetSessionUserCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        _CurrentUser = ea.Result as clsUserVO;
                        _CurrentUser.UserLoginInfo.WindowsUserName = "Windows User";


                        clsGetAppConfigBizActionVO BizActionObject = new clsGetAppConfigBizActionVO();
                        BizActionObject.UnitID = _CurrentUser.UserLoginInfo.UnitId;
                        BizActionObject.AppConfig = new clsAppConfigVO();

                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

                        client1.ProcessCompleted += (s1, ea1) =>
                        {
                            if (ea1.Result != null && ea1.Error == null)
                            {
                                if (((clsGetAppConfigBizActionVO)ea1.Result).AppConfig != null)
                                {
                                    _AppConfig = ((clsGetAppConfigBizActionVO)ea1.Result).AppConfig;
                                    //_AppConfig=
                                    this.RootVisual = MainPage = new MainPage();
                                    //MainPage = new MainPage();
                                    //this.RootVisual = new RootPage();
                                    this.RootVisual.KeyDown += new System.Windows.Input.KeyEventHandler(RootVisual_KeyDown);
                                    this.RootVisual.MouseMove += new MouseEventHandler(RootVisual_MouseMove);
                                    //For Applying Date Format
                                    Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
                                    if (_AppConfig.DateFormatID > 0)
                                    {
                                        if (_AppConfig.DateFormat != null)
                                        {
                                            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = _AppConfig.DateFormat;
                                        }
                                        //Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                                    }
                                }
                            }
                        };
                        client1.ProcessAsync(BizActionObject, new clsUserVO());
                        client1.CloseAsync();  
                        
                    

                        #region Commented for checking the problem with UserInformation
                        ////Uri address1 = new Uri(Application.Current.Host.Source, "../UserInformation.asmx"); // this url will work both in dev and after deploy
                        ////UserInforServiceRef.UserInformationSoapClient client1 = new UserInforServiceRef.UserInformationSoapClient("UserInformationSoap", address1.AbsoluteUri);

                        ////client1.GetClientUserNameCompleted += (s1, e1) =>
                        ////{
                        ////    string winname = (string)e1.Result;

                        ////    _CurrentUser.UserLoginInfo.WindowsUserName = winname;

                        ////    this.RootVisual = MainPage = new MainPage();
                        ////};
                        ////client1.GetClientUserNameAsync();
                        ////client1.CloseAsync();
                        #endregion
                        
                        //CurrentUser = ((clsUserVO)ea.Result)e.InitParams["User"];
                                                
                        // UserMachineName = e.InitParams["UserMachineName"];
                        //long id = long.Parse(e.InitParams["UserloginUnitID"]);
                       
                        //IsFirstPasswordChanged = bool.Parse(e.InitParams["FirstPwdChanged"]);

                        //_LoggedUserId = userid;

                        //LoginName = (string)(e.InitParams["LoginName"]);
                        //LoggedinUnit = (string)(e.InitParams["UserloginUnitID"]);
                        //LoggedInUnitName = (string)(e.InitParams["UserloginUnitName"]);
                        //LoggedUserID = Convert.ToInt64(e.InitParams["UserID"]);
                    }
                    else
                    {
                        Uri address1 = new Uri(Application.Current.Host.Source, "../Logout.aspx");
                        HtmlPage.Window.Navigate(address1);
                    }
            };
            client.GetSessionUserAsync("USER");
            client.CloseAsync();

            }
            catch (Exception)
            {
                Uri address1 = new Uri(Application.Current.Host.Source, "../Logout.aspx");
                HtmlPage.Window.Navigate(address1);
            }            
        }

        void RootVisual_MouseMove(object sender, MouseEventArgs e)
        {
            idle = false;
            if (App.CounterI > SampleVal1)
            {

            }
            else
            {
                App.CounterI = 0;
            }
        }

        void RootVisual_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            idle = false;


            if (App.CounterI > SampleVal1)
            {

            }
            else
            {
                App.CounterI = 0;
            }
        }


        private void UpdateAudtiTrail()
        {
            try
            {
                clsUserVO User = null;
                User = ((IApplicationConfiguration)App.Current).CurrentUser;
                clsUpdateAuditTrailBizActionVO BizAction = new clsUpdateAuditTrailBizActionVO();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    ((IApplicationConfiguration)App.Current).CurrentUser = null;

                    Uri address1 = new Uri(Application.Current.Host.Source, "../Logout.aspx");
                    HtmlPage.Window.Navigate(address1);
                };
                client.ProcessAsync(BizAction, User);// new clsUserVO());
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                ((IApplicationConfiguration)App.Current).CurrentUser = null;

                Uri address1 = new Uri(Application.Current.Host.Source, "../Logout.aspx");
                HtmlPage.Window.Navigate(address1);
            }
            
        }

        private void Application_Exit(object sender, EventArgs e)
        {
           // UpdateAudtiTrail(); 

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.SetSessionUserCompleted += (s, ea) =>
            {
                UpdateAudtiTrail();
            };
            ((IApplicationConfiguration)App.Current).CurrentUser = null;
            client.SetSessionUserAsync("USER", null);
            client.CloseAsync();

            //Uri address1 = new Uri(Application.Current.Host.Source, "../Logout.aspx");
            //HtmlPage.Window.Navigate(address1);
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
              
        public static bool IsPatientMode { get; set; }
        //private static long _LoggedUserId;
        //public static long LoggedUserId { get { return _LoggedUserId; } }
        //private static long _UserRoleID;
        //public static long UserRoleID { get { return _UserRoleID; } }
        public static MainPage MainPage { get; set; }
        public static ChildWindow Configuration { get; set; }
       // public static ChildWindow FirstPasswordChange { get; set; }

        //public static bool IsFirstPasswordChanged { get; set; }
        private PalashDynamics.ValueObjects.Pathology.clsPathOrderBookingVO _SelectedPathologyWorkOrder;
        private PalashDynamics.ValueObjects.Radiology.clsRadOrderBookingVO _SelectedRadiologyWorkOrder;
        private PalashDynamics.ValueObjects.Patient.clsPatientGeneralVO _SelectedPatient;
        private PalashDynamics.ValueObjects.IPD.clsIPDAdmissionVO _SelectedIPDPatient;
        private PalashDynamics.ValueObjects.IVFPlanTherapy.clsCoupleVO _SelectedCoupleDetails;
        private PalashDynamics.ValueObjects.clsUserVO _CurrentUser;
        private PalashDynamics.ValueObjects.Administration.OTConfiguration.clsPatientProcedureScheduleVO _SelectedOTBooking;

        private PalashDynamics.ValueObjects.Patient.clsPatientGeneralVO _SelectedPatientForPediatric;
     
        #region IApplicationConfiguration Members

        public PalashDynamics.ValueObjects.Pathology.clsPathOrderBookingVO SelectedPathologyWorkOrder
        {
            get
            {
                return _SelectedPathologyWorkOrder;
            }
            set
            {
                _SelectedPathologyWorkOrder = value;
            }

        }

        public PalashDynamics.ValueObjects.Radiology.clsRadOrderBookingVO SelectedRadiologyWorkOrder
        {
            get
            {
                return _SelectedRadiologyWorkOrder;
            }
            set
            {
                _SelectedRadiologyWorkOrder = value;
            }
        }

        public PalashDynamics.ValueObjects.Patient.clsPatientGeneralVO SelectedPatient
        {
            get
            {
                return _SelectedPatient;
            }
            set
            {
                _SelectedPatient = value;
            }
        }

        public PalashDynamics.ValueObjects.IPD.clsIPDAdmissionVO SelectedIPDPatient
        {
            get
            {
                return _SelectedIPDPatient;
            }
            set
            {
                _SelectedIPDPatient = value;
            }
        }

        public PalashDynamics.ValueObjects.IVFPlanTherapy.clsCoupleVO SelectedCoupleDetails
        {
            get
            {
                return _SelectedCoupleDetails;
            }
            set
            {
                _SelectedCoupleDetails = value;
            }
        }

        public PalashDynamics.ValueObjects.Administration.OTConfiguration.clsPatientProcedureScheduleVO SelectedOTBooking
        {
            get
            {
                return _SelectedOTBooking;
            }
            set
            {
                _SelectedOTBooking = value;
            }
        }
        
        public clsUserVO LoginUserDetails { get; set; }
        
        public PalashDynamics.ValueObjects.clsUserVO CurrentUser
        {
            get { return _CurrentUser; }
            set { _CurrentUser = value; }
            //get
            //{
            //  //  return _CurrentUser;
            //    if (_CurrentUser == null) GetLoggedInUserDetails();
            //    return _CurrentUser;
            //}
            //set
            //{
            //    _CurrentUser = value;
            //}
        }

        public PalashDynamics.ValueObjects.Patient.clsPatientGeneralVO SelectedPatientForPediatric
        {
            get
            {
                return _SelectedPatientForPediatric;
            }
            set
            {
                _SelectedPatientForPediatric = value;
            }
        }
                          
        public void FillMenu(string Parent)
        {
            MainPage.RequestXML(Parent);
        }

        public void OpenMainContent(string Action)
        {
            MainPage.OpenMainRegion(Action);
        }

        public void  OpenMainContent(UIElement UIelement)
        {
            MainPage.GlobalOpenMainRegion(UIelement);
        }
             
        //public string UserMachineName  { get; set; }
        //public static string LoginName { get; set; }
        //public static string LoggedinUnit { get; set; }
        //public static string LoggedInUnitName { get; set; }
        //public static long LoggedUserID { get; set; }

        clsAppConfigVO _AppConfig = null ; //new PalashDynamics.ValueObjects.Administration.clsAppConfigVO();
              
        public clsAppConfigVO ApplicationConfigurations
        {
            get
            {
               // if (_AppConfig == null) GetApplicationConfigurations();
                return _AppConfig;
            }          
        }

        private void GetApplicationConfigurations()
        {
            clsGetAppConfigBizActionVO BizActionObject = new clsGetAppConfigBizActionVO();
            
            BizActionObject.AppConfig = new clsAppConfigVO();
            BizActionObject.UnitID = _CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    if (((clsGetAppConfigBizActionVO)ea.Result).AppConfig != null)
                    {
                        _AppConfig = ((clsGetAppConfigBizActionVO)ea.Result).AppConfig;                                            
                        //_AppConfig=
                    }
                }
            };
            client.ProcessAsync(BizActionObject, new clsUserVO());
            client.CloseAsync();           
        }

        public PalashDynamic.Localization.LocalizationManager LocalizedManager
        {
            get;
            set;
        }
        #endregion
       
    }

    public class UIClassElement
    {
        public long ID { get; set; }

        public string Tooltip { get; set; }

        public string Title { get; set; }

        public string ImagePath { get; set; }

        public string Parent { get; set; }

        public string Module { get; set; }

        public string Action { get; set; }

        public string Header { get; set; }

        public string Configuration { get; set; }

        public string Mode { get; set; }

        public bool Active { get; set; }

        public long? ParentId { get; set; }

        public int? MenuOrder { get; set; }




        public bool IsCreate { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsRead { get; set; }
        public bool IsPrint { get; set; }

        public long UnitRightRoleID { get; set; }
        public long UnitID { get; set; }

        public UIElement UIElementObj { get; set; }

    }

}
