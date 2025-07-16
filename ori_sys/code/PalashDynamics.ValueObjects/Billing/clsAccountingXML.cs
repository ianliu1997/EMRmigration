using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PalashDynamics.ValueObjects.Billing
{
    public class ALLEDGERENTRIES
    {
        public string LEDGERNAME { get; set; }

        [XmlElement(IsNullable = false)]
        public string ISDEEMEDPOSITIVE { get; set; }

        public double? AMOUNT { get; set; }

        [XmlElement("BILLALLOCATIONS.LIST")]
        public BILLALLOCATIONS BILLALLOCATIONS { get; set; }

        [XmlElement("CategoryAllocations.LIST")]
        public List<CategoryAllocations> CategoryAllocations { get; set; }
    }

    public class BILLALLOCATIONS
    {
        public string NAME { get; set; }

        public string BILLTYPE { get; set; }

        public double? AMOUNT { get; set; }
    }

    public class CategoryAllocations
    {
        public string Category { get; set; }

        [XmlElement("CostCentreAllocations.LIST")]
        public List<CostCentreAllocations> CostCentreAllocations { get; set; }
    }

    public class CostCentreAllocations
    {
        public string Name { get; set; }

        public double? AMOUNT { get; set; }
    }

    public class BODY
    {
        [XmlElement("DESC")]
        public Desc DESC { get; set; }

        [XmlElement("DATA")]
        public List<DATA> DATA { get; set; }
    }

    public class DATA
    {
        [XmlElement("TALLYMESSAGE")]
        public TALLYMESSAGE TALLYMESSAGE { get; set; }
    }

    public class Desc
    {
    }

    public class ENVELOPE
    {
        [XmlElement("HEADER")]
        public HEADER Header { get; set; }

        [XmlElement("BODY")]
        public BODY Body { get; set; }
    }

    public class HEADER
    {
        public int VERSION { get; set; }
        public string TALLYREQUEST { get; set; }
        public string TYPE { get; set; }
        public string ID { get; set; }
    }

    public class TALLYMESSAGE
    {
        [XmlElement("VOUCHER")]
        public VOUCHER Voucher { get; set; }
    }

    public class VOUCHER
    {
        [XmlAttribute("VCHTYPE")]
        public string VCHTYPE { get; set; }

        [XmlAttribute("ACTION")]
        public string ACTION { get; set; }

        public string DATE { get; set; }

        public string NARRATION { get; set; }

        public string VOUCHERTYPENAME { get; set; }

        public string EFFECTIVEDATE { get; set; }

        [XmlElement("ALLLEDGERENTRIES.LIST")]
        public List<ALLEDGERENTRIES> ALLEDGERENTRIES_LIST { get; set; }
    }
}
