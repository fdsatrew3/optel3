using OPTEL.Data;
using System;

namespace OPTEL.UI.Desktop.Services.GanttChartManagers.Base
{
    public interface IGanttChartManagerService
    {
        public void SetProductionPlan(ProductionPlan plan);
        public void UpdateChart();
        public void SetDesiredInterval(DateTime start, DateTime end);
    }
}
