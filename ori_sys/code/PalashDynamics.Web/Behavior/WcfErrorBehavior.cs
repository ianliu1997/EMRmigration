using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Diagnostics;

namespace WcfExceptionExample.Web.Behavior
{
    /// <summary>
    /// Allows an implementer to control the fault message returned to the caller and optionally perform custom error processing such as logging.
    /// </summary>
    public class WcfErrorBehavior : IErrorHandler
    {
  
        void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            try
            {
                if (error is FaultException)
                { //let WCF perform normal behavior 
                }
                else 
                {
                // Add code here to build faultreason for client based on exception
                FaultReason faultReason = new FaultReason(error.Message);
                ExceptionDetail exceptionDetail = new ExceptionDetail(error);

                // For security reasons you can also decide to not give the ExceptionDetail back to the client or change the message, etc
                FaultException<ExceptionDetail> faultException = new FaultException<ExceptionDetail>(exceptionDetail, faultReason, FaultCode.CreateSenderFaultCode(new FaultCode("0")));

                MessageFault messageFault = faultException.CreateMessageFault();
                fault = Message.CreateMessage(version, messageFault, faultException.Action);
                }
              }
            catch
            {
                // Todo log error
            }   
        }
     
        /// <summary>
        /// Handle all WCF Exceptions
        /// </summary>
        bool IErrorHandler.HandleError(Exception ex)
        {
            try
            {
                // Add logging of exception here!
                Debug.WriteLine(ex.ToString());
            }
            catch
            {
                // Todo log error
            }
            
            // return true means we handled the error.
            return true;
        }


    }

    public sealed class WcfErrorBehaviorAttribute : WcfBehaviorAttributeBase
    {
        public WcfErrorBehaviorAttribute()
            : base(typeof(WcfErrorBehavior))
        {
        }
    }
}
