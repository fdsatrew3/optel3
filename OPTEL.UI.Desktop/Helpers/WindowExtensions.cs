﻿using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace OPTEL.UI.Desktop.Helpers
{
    internal static class WindowExtensions
    {
        // from winuser.h
        private const int GWL_STYLE = -16,
                          WS_MAXIMIZEBOX = 0x10000,
                          WS_MINIMIZEBOX = 0x20000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        internal static void HideMinimizeAndMaximizeButtons(this Window window)
        {
            window.SourceInitialized += (x, y) =>
            {

                IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
                var currentStyle = GetWindowLong(hwnd, GWL_STYLE);

                SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
            };
        }

        internal static void ShowModalDialog(this Window window, Window parentWindow = null)
        {
            window.HideMinimizeAndMaximizeButtons();
            if(parentWindow == null)
            {
                parentWindow = Application.Current.MainWindow;
            }
            window.Owner = parentWindow;
            window.ShowDialog();
        }
    }
}
