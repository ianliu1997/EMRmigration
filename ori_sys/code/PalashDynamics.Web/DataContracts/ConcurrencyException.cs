using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfExceptionExample.Web.DataContracts
{
    [DataContract]
    public class ConcurrencyException
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Details { get; set; }
    }

    

}
