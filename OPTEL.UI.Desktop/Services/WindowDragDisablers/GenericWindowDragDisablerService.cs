using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using System;
using System.Windows;
using System.Windows.Interop;

namespace OPTEL.UI.Desktop.Services.WindowClosers
{
    public class GenericWindowDragDisablerService : IWindowDragDisablerService
    {
        private Window _parent;

        public GenericWindowDragDisablerService(Window parent)
        {
            _parent = parent;
            _parent.SourceInitialized += OnSourceInitialized;
        }
        public void OnSourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(_parent);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
