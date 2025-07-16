using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.UserRights
{
    public class clsUserRightsVO
    {
        
        public long UserID { get; set; }
        public long UnitID { get; set; }
        public long CreditLimit { get; set; }
        public bool IsIpd { get; set; }
        public bool IsOpd { get; set; }
        public bool IsDirectPurchase { get; set; }
        public bool Status { get; set; }
        public long AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long IpdAuthLvl { get; set; }
        public long OpdAuthLvl { get; set; }
        public int ResultStatus { get; set; }
        
        public Decimal IpdBillingPercentage{get;set;}
        public Decimal IpdSettlePercentage { get; set; }
        public Decimal IpdBillingAmmount { get; set; }
        public Decimal IpdSettleAmmount { get; set; }
        public long IpdBillAuthLvlID { get; set; }
        public Decimal OpdBillingPercentage { get; set; }
        public Decimal OpdSettlePercentage { get; set; }
        public Decimal OpdBillingAmmount { get; set; }
        public Decimal OpdSettleAmmount { get; set; }
        public long OpdBillAuthLvlID { get; set; }
        public bool IsCrossAppointment { get; set; }
        public bool IsDailyCollection { get; set; } //***//
        public bool IsDirectIndent { get; set; }//***//
        public bool IsInterClinicIndent { get; set; }//***//

        //by Anjali..........
        public string AuthLevelForRefundOPD { get; set; }
        public string AuthLevelForConcenssionOPD { get; set; }
        public long OPDAuthLvtForConcessionID { get; set; }
        public string AuthLevelForRefundIPD { get; set; }
        public string AuthLevelForConcenssionIPD { get; set; }

        // Added By CDS
        public long UserGRNCountForMonth { get; set; }
        public Decimal MaxPurchaseAmtPerTrans { get; set; }
        public long FrequencyPerMonth { get; set; }

        public bool IsCentarlPurchase { get; set; }

        public long POApprovalLvlID { get; set; }
        public string POApprovalLvl { get; set; }
        public bool Isfinalized { get; set; }//Added By Yogesh 170516
        public bool IsEditAfterFinalized { get; set; }//Added By rohinee 170516

        public bool IsRefundSerAfterSampleCollection { get; set; }

        public long PatientAdvRefundAuthLvlID { get; set; }//Added By bhushanp 31062017
        public decimal PatientAdvRefundAmmount { get; set; } //Added By bhushanp 31062017

        public long CompanyAdvRefundAuthLvlID { get; set; }//Added By bhushanp 31062017
        public decimal CompanyAdvRefundAmmount { get; set; } //Added By bhushanp 31062017

        public bool IsMRPAdjustmentAuth { get; set; }
        public long MRPAdjustmentAuthLvlID { get; set; }

        public bool IsRCEditOnFreeze { get; set; } //Added By Prashant Channe 16/10/2018
    }
}
