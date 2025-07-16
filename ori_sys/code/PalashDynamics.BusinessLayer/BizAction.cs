using System;
using System.Collections.Generic;
using System.Text;
using PalashDynamics.ValueObjects;


namespace PalashDynamics.BusinessLayer
{
    //This is an Abstract class
    public abstract class BizAction
    {
        protected IValueObject PreProcess(IValueObject valueObject, clsUserVO objUserVO)
        {
            return valueObject;
        }

        protected IValueObject PostProcess(IValueObject valueObject, clsUserVO objUserVO)
        {
            return valueObject;
        }

        protected abstract IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO);

        public IValueObject Process(IValueObject valueObject, clsUserVO objUserVO)
        {
            PreProcess(valueObject, objUserVO);
            valueObject = ProcessRequest(valueObject, objUserVO);
            PostProcess(valueObject, objUserVO);

            return valueObject;
        }
    }
}
