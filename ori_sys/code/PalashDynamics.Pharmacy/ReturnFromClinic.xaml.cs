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
using PalashDynamics.Animations;
namespace PalashDynamics.Pharmacy
{
    public partial class ReturnFromClinic : UserControl
    {
        private SwivelAnimation objAnimation;
        public ReturnFromClinic()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
        }
        public int ReturnNo { get; set; }
        public DateTime ReturnDate { get; set; }
        public string From_Clinic { get; set; }
        public string To_Clinic { get; set; }
        public int TotalItems { get; set; }
        public int TotAmt { get; set; }
        public int VatAmt { get; set; }
        public List<IssuedItemsDetail> ItemDetails { get; set; }

        public List<ReturnFromClinic> GetIssuedItemMaster()
        {
            List<ReturnFromClinic> retn = new List<ReturnFromClinic>();
            ReturnFromClinic newretn = null;
            newretn = new ReturnFromClinic();
            newretn.ReturnNo = 1005;
            newretn.ReturnDate = DateTime.Now;
            newretn.From_Clinic = "ABC";
            newretn.To_Clinic = "XYZ";
            newretn.TotAmt = 100;
            newretn.VatAmt = 10;
            newretn.ItemDetails= new List<IssuedItemsDetail>();
            newretn.ItemDetails.AddRange(
                
                  new IssuedItemsDetail[]
                  { 
                       new IssuedItemsDetail(){ItemName="XYZ",BatchCode="0005",Quantity=5,MRP=5,TotaAmt=25,VatPer=0,VatAmt=0,NetAmt=25,VatIE="E"},
                       new IssuedItemsDetail(){ItemName="ABC",BatchCode="0006",Quantity=25,MRP=3,TotaAmt=75,VatPer=0,VatAmt=0,NetAmt=25,VatIE="E"}

                  }
                );

            retn.Add(newretn);
            return retn;
          
 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdIssueListMaster.ItemsSource = GetIssuedItemMaster();
          
        }
        public class IssuedItemsDetail
        {
            public string ItemName { get; set; }
            public string BatchCode { get; set; }
            public int Quantity { get; set; }
            public int MRP { get; set; }
            public int TotaAmt { get; set; }
            public int VatPer { get; set; }
            public int VatAmt { get; set; }
            public int NetAmt { get; set; }
            public string VatIE { get; set; }

        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCan_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
        }

        private void cmdGetIssuedItems_Click_2(object sender, RoutedEventArgs e)
        {
            SearchIssue obj = new SearchIssue();
            obj.Show();
        }

        private void CmdCancelBaclPanel_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {

        }
        
    }
  
}
                                        