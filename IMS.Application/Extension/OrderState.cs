﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Extension
{
    public static class OrderState
    {
        public const string Canceled = "Canceled";
        public const string Processing = "Processing";
        public const string Delivered = "Delivered";
        public const string Delivering = "Delivering";
    }
}
