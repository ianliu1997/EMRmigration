using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects
{
    public interface IBizActionValueObject:IValueObject
    {
        string GetBizAction();
    }
}
