using System.Collections.Generic;
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

        public static readonly DependencyProperty EntriesProperty = DependencyProperty.Register("Entries", typeof(IEnumerable<object>), typeof(LabelComboBox), new FrameworkPropertyMetadata(default(IEnumerable<object>), onEntriesChanged));

        public IEnumerable<object> Entries
        {
            get
            {
                return (IEnumerable<object>)GetValue(EntriesProperty);
            }
            set
            {
                SetValue(EntriesProperty, value);
            }
        }

        private static void onEntriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabelComboBox control = d as LabelComboBox;
            if (d != null)
            {
                control.BaseComboBox.ItemsSource = (System.Collections.IEnumerable)e.NewValue;
            }
        }

        public static readonly DependencyProperty SelectedEntryProperty = DependencyProperty.Register("SelectedEntry", typeof(object), typeof(LabelComboBox), new FrameworkPropertyMetadata(default(object), onSelectedEntryChanged));

        public object SelectedEntry
        {
            get
            {
                return (object)GetValue(SelectedEntryProperty);
            }
            set
            {
                SetValue(SelectedEntryProperty, value);
            }
        }

        private static void onSelectedEntryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabelComboBox control = d as LabelComboBox;
            if (d != null)
            {
                control.BaseComboBox.SelectedItem = e.NewValue;
            }
        }
    }
}
