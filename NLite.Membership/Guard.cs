﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NLite.Membership
{
    internal static partial class Guard
    {
        [DebuggerStepThrough]
        public static void NotNull(object argumentValue,
                                         string argumentName)
        {
            if (argumentValue == null) throw new ArgumentNullException(argumentName);
        }


        [DebuggerStepThrough]
        public static void NotNullOrEmpty(string argumentValue,
                                                 string argumentName)
        {
            if (argumentValue == null || argumentValue.Length == 0) throw new ArgumentNullException(argumentName);
        }
    }
}
