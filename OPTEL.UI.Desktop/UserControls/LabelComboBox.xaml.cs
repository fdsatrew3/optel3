using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace OPTEL.UI.Desktop.UserControls
{
    /// <summary>
    /// Логика взаимодействия для LabelComboBox.xaml
    /// </summary>
    public partial class LabelComboBox : UserControl
    {
        public LabelComboBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(LabelComboBox), new FrameworkPropertyMetadata(default(string), OnTitleChanged));
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
            LabelComboBox control = d as LabelComboBox;
            if (d != null)
            {
                control.BaseLabel.Content = e.NewValue;
            }
        }

        public static readonly DependencyProperty EntriesProperty = DependencyProperty.Register("Entries", typeof(ObservableCollection<string>), typeof(LabelComboBox), new FrameworkPropertyMetadata(default(ObservableCollection<string>), onTextChanged));

        public ObservableCollection<string> Entries
        {
            get
            {
                return (ObservableCollection<string>)GetValue(EntriesProperty);
            }
            set
            {
                SetValue(EntriesProperty, value);
            }
        }

        private static void onTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabelComboBox control = d as LabelComboBox;
            if (d != null)
            {
                control.BaseComboBox.ItemsSource = (System.Collections.IEnumerable)e.NewValue;
            }
        }
    }
}
