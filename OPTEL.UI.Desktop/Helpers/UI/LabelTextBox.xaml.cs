using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

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

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(LabelTextBox), new FrameworkPropertyMetadata(default(string), OnTitleChanged));
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }
        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabelTextBox control = d as LabelTextBox;
            if (d != null)
            {
                control.BaseLabel.Content = e.NewValue;
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LabelTextBox), new FrameworkPropertyMetadata(default(string), onTextChanged));

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        private static void onTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabelTextBox control = d as LabelTextBox;
            if (d != null)
            {
                control.BaseTextBox.Text = e.NewValue as string;
            }
        }
    }
}
