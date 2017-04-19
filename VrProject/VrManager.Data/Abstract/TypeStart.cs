using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrManager.Data.Abstract
{
    public enum TypeStartFocus
    {
        /// <summary>
        /// In Cybser Space
        /// </summary>
         FocusedInFullScreen,
        /// <summary>
        /// In Rollercoaster
        /// </summary>
        FocusToMainWnd,
        /// <summary>
        /// In Helix
        /// </summary>
        FocusToTitle
    }
}
