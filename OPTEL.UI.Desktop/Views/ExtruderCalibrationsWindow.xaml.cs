using System.Windows;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ExtruderCalibrationsWindow.xaml
    /// </summary>
    public partial class ExtruderCalibrationsWindow : Window
    {
        public ExtruderCalibrationsWindow()
        {
            InitializeComponent();
            this.DataContext = UnitOfWork.instance.CalibrationChangeRepository;
        }
    }
}
