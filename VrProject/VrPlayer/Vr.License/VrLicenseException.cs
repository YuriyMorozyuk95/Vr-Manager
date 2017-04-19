using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vr.License
{
    public class VrLicenseException : Exception
    {
        public VrLicenseException(string message) : base(message)
        {
        }
    }
}
