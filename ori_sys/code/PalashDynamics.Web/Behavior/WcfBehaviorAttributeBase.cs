using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WcfExceptionExample.Web.Behavior
{
    public abstract class WcfBehaviorAttributeBase : Attribute, IServiceBehavior
    {
        private Type _behaviorType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeBehavior">IDispatchMessageInspector, IErrorHandler of IParameterInspector</param>
        public WcfBehaviorAttributeBase(Type typeBehavior)
        {
            _behaviorType = typeBehavior;
        }

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
                object behavior;
                try
                {
                    behavior = Activator.CreateInstance(_behaviorType);
                }
                catch (MissingMethodException e)
                {
                    throw new ArgumentException("The Type specified in the BehaviorAttribute constructor must have a public empty constructor.", e);
                }
                catch (InvalidCastException e)
                {
                    throw new ArgumentException("The Type specified in the BehaviorAttribute constructor must implement IDispatchMessageInspector, IParamaterInspector of IErrorHandler", e);
                }

                foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
                {
                    if (behavior is IParameterInspector)
                    {
                        foreach (EndpointDispatcher epDisp in channelDispatcher.Endpoints)
                        {
                            foreach (DispatchOperation op in epDisp.DispatchRuntime.Operations)
                                op.ParameterInspectors.Add((IParameterInspector)behavior);
                        }
                    }
                    else if (behavior is IErrorHandler)
                    {
                        channelDispatcher.ErrorHandlers.Add((IErrorHandler)behavior);
                    }
                    else if (behavior is IDispatchMessageInspector)
                    {
                        foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                        {
                            endpointDispatcher.DispatchRuntime.MessageInspectors.Add((IDispatchMessageInspector)behavior);
                        }
                    }
                }

        }

        void IServiceBehavior.Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }
    }

}
