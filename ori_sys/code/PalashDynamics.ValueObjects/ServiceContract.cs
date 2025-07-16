using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Reflection;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.ValueObjects
{
    // NOTE: If you change the interface name "IPalashService" here, you must also update the reference to "IPalashService" in Web.config.
    //[ServiceKnownType("GetKnownTypes", typeof(KnownTypesProvider))]

    [ServiceContract]
    [ServiceKnownType("GetKnownTypes", typeof(KnownTypesProvider))]
    public interface IPalashService
    {
        [OperationContract]
        IBizActionValueObject Process(IBizActionValueObject BizActionObject, clsUserVO UserInfo);
        [OperationContract]
        clsUserVO GetSessionUser(string sessionKey);


        [OperationContract]
        void SetSessionUser(string sessionKey, clsUserVO User);
        
    }


    static class KnownTypesProvider
    {
        static Type[] GetKnownTypes(ICustomAttributeProvider attributeTarget)
        {
            Type dataContractType = typeof(System.Runtime.Serialization.DataContractAttribute);
            Type serviceContractType = (Type)attributeTarget;
            Type[] exportedTypes = serviceContractType.Assembly.GetExportedTypes();
            List<Type> knownTypes = new List<Type>();
            foreach (Type type in exportedTypes)
            {
                if (type.IsEnum)
                {

                }

                if (type.IsClass || type.IsEnum)
                {
                    knownTypes.Add(type);
                }
            }
            return knownTypes.ToArray();

        }
    }
}
