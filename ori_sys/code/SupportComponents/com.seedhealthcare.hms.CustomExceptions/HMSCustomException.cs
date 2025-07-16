using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace com.seedhealthcare.hms.CustomExceptions
{
    /// <summary>
    /// This exception is thrown when the client encounters an error code from the dictionary server
    /// it connects to. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class HmsApplicationException : ApplicationException
    {
        private int errorCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="HmsApplicationException" /> class with no arguments.
        /// </summary>
        public HmsApplicationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HmsApplicationException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public HmsApplicationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HmsApplicationException" /> class with a specified error message and 
        /// a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If the <em>innerException</em>
        /// parameter is not a null reference, the current exception is raised in a <strong>catch</strong> block that handles 
        /// the inner exception.</param>
        public HmsApplicationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HmsApplicationException" /> class with a 
        /// raw error code coming from a dictionary server and a specified error message.
        /// </summary>
        /// <param name="errorCode">Raw error code. See <see cref="HmsApplicationException.ErrorCode">ErrorCode</see>.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public HmsApplicationException(int errorCode, string message)
            : base(message)
        {
            this.errorCode = errorCode;
        }

        /// <exclude />
        private HmsApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.errorCode = info.GetInt32("ErrorCode");
        }

        /// <exclude />
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ErrorCode", this.errorCode);
            base.GetObjectData(info, context);
        }


        /// <summary>
        /// <p>The original error code returned by the dictionary server. The RFC 2229 defines error
        /// codes in the range of 400-599:</p>
        /// <list type="table">
        /// <item><term>4yz</term><description>Transient Negative Completion reply</description></item>
        /// <item><term>5yz</term><description>Permanent Negative Completion reply</description></item>
        /// </list>
        /// </summary>
        public int ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
    } 
}

