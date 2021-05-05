using System.Windows.Controls;

namespace OPTEL.UI.Desktop.Helpers.UI
{
    /// <summary>
    /// Логика взаимодействия для LabelTextBox.xaml
    /// </summary>
    public partial class LabelTextBox : UserControl
    {
        public LabelTextBox()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return BaseLabel.Content.ToString(); }
            set
            {
                BaseLabel.Content = value;
            }
        }

        public string Text
        {
            get { return BaseTextBox.Text; }
            set
            {
                BaseTextBox.Text = value;
            }
        }
    }
}
