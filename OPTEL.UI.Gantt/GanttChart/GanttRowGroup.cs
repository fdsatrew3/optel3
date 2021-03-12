﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace OPTEL.UI.Gantt.GanttChart
{
    public class GanttRowGroup
    {
        public GanttRowGroup()
        {
            Rows = new ObservableCollection<GanttRow>();
        }
        public ObservableCollection<GanttRow> Rows { get; set; }
    }
}
