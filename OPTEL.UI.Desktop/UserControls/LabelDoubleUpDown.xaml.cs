using System.Windows;
using System.Windows.Controls;

namespace OPTEL.UI.Desktop.UserControls
{
    /// <summary>
    /// Логика взаимодействия для LabelDoubleUpDown.xaml
    /// </summary>
    public partial class LabelDoubleUpDown : UserControl
    {
        public LabelDoubleUpDown()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(LabelDoubleUpDown), new FrameworkPropertyMetadata(default(string), OnTitleChanged));
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
            LabelDoubleUpDown control = d as LabelDoubleUpDown;
            if (d != null)
            {
                control.BaseLabel.Content = e.NewValue;
            }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(LabelDoubleUpDown), new FrameworkPropertyMetadata(default(double), onValueChanged));

        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        private static void onValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabelDoubleUpDown control = d as LabelDoubleUpDown;
            if (d != null)
            {
                control.BaseDoubleUpDown.Value = e.NewValue as double?;
            }
        }
    }
}
