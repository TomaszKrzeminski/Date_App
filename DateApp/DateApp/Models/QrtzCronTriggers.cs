﻿using System;
using System.Collections.Generic;

namespace DateApp.Models
{
    public partial class QrtzCronTriggers
    {
        public string SchedName { get; set; }
        public string TriggerName { get; set; }
        public string TriggerGroup { get; set; }
        public string CronExpression { get; set; }
        public string TimeZoneId { get; set; }

        public QrtzTriggers QrtzTriggers { get; set; }
    }
}
