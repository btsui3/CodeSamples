using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace project.Web.Enums
{
    [Flags]
    public enum CompanyDesignations
    {
        None = 0,
        SmallBusiness = 1,
        VeteranOwned = 2,
        MinorityOwned = 4,
        WomenOwned = 8
    }
}