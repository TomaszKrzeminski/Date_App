﻿using System;
using System.Collections.Generic;

namespace DateApp.Models
{
    public partial class QrtzCalendars
    {
        public string SchedName { get; set; }
        public string CalendarName { get; set; }
        public byte[] Calendar { get; set; }
    }
}
