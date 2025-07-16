using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Log
{
    public class LogInfo
    {
        public Guid guid { get; set; }
        public long UserId { get; set; }
        public DateTime TimeStamp  { get; set; }
        public string ClassName { get; set; }
        public string MethodName  { get; set; }
        public string Message { get; set; }
    }
}
