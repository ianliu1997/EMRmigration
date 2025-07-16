using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace WcfExceptionExample.Web.Behavior
{
    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/dd470096(VS.96).aspx
    /// </summary>
    public class WcfSilverlightFaultBehavior : IDispatchMessageInspector
    {
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (reply.IsFault)
            {
                HttpResponseMessageProperty property = new HttpResponseMessageProperty();

                // Here the response code is changed to 200.
                property.StatusCode = System.Net.HttpStatusCode.OK;

                reply.Properties[HttpResponseMessageProperty.Name] = property;
            }
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // Do nothing to the incoming message.
            return null;
        }
    }

    public sealed class WcfSilverlightFaultBehaviorAttribute : WcfBehaviorAttributeBase
    {
        public WcfSilverlightFaultBehaviorAttribute()
            : base(typeof(WcfSilverlightFaultBehavior))
        {
        }
    }
}
