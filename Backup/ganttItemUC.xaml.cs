using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCADAServis.Controls
{
    /// <summary>
    /// Interaction logic for ganttItemUC.xaml
    /// </summary>
    public partial class ganttItemUC : UserControl
    {
        private GanttItem data;
        public GanttItem Data
        {
            set
            {
                data = value;
                refreshToolTip();
            }
            get { return data; }
        }
        public string Caption { set { lCaption.Text = value; } }

        public ganttItemUC()
        {
            InitializeComponent();
            VisualStateManager.GoToState(this, "IsMouseOut", false);
            this.MouseLeave += (o, e) => VisualStateManager.GoToState(this, "IsMouseOut", true);
            this.MouseEnter += (o, e) => VisualStateManager.GoToState(this, "IsMouseIn", true);
        }


        private void refreshToolTip()
        {
            ttCaption.Text = data.Caption;
            ttFrom.Text = data.Start.ToString("dd.MM.yyyy (ddd) hh:mm");
            ttTo.Text = data.End.ToString("dd.MM.yyyy (ddd) hh:mm");

            Dictionary<string, string> shownData = new Dictionary<string, string>();
            if (data.Data != null)
                foreach (var row in data.Data)
                {
                    shownData.Add(row.Key, row.Value.ToString());
                }
        }



    }
}
