using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComPortPackages.Core
{
    public static class Consts
    {
        public static readonly byte[] DoneBuffer = new byte[]
        {
            126,
            255,
            1,
            0,
            0,
            0,
            11,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            220,
            16
        };
    }
}