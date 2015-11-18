using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysTrayApp
{
    static class Program
    {

        public class SysTrayApp : Form
        {
            
            [STAThread]
            public static void Main()
            {
                Application.Run(new SysTrayApp());
            }

            private NotifyIcon trayIcon;
            private ContextMenu trayMenu;
            private Timer timer;

            public SysTrayApp()
            {
                // Create a simple tray menu with only one item.

                trayMenu = new ContextMenu();
                trayMenu.MenuItems.Add("Exit", OnExit);

                // Create a tray icon. In this example we use a
                // standard system icon for simplicity, but you
                // can of course use your own custom icon too.
                trayIcon = new NotifyIcon();
                trayIcon.Text = "OKRestart";
                trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

                // Add menu to tray icon and show it.
                trayIcon.ContextMenu = trayMenu;
                trayIcon.Visible = true;

                timer = new Timer();
                timer.Interval = 1000;
                timer.Tick += Timer_Tick;
                timer.Start();

            }

            int pressCount = 0;

            private void Timer_Tick(object sender, EventArgs e)
            {
                int lbutton = GetAsyncKeyState(Keys.LButton);
                int enter = GetAsyncKeyState(Keys.Enter);
                
                if(lbutton == 0 || enter == 0)
                {
                    pressCount = 0;
                }
                if (lbutton == Int16.MinValue || enter == Int16.MinValue) //Use constants (0x8000 and -32768 is Int16.MaxValue)
                {
                    pressCount++;
                    if (pressCount >= 5)
                    {
                        pressCount = 0;
                        Process.Start("shutdown", "/r /t 0 /f");
                    }
                }

            }

            protected override void OnLoad(EventArgs e)
            {
                Visible = false; // Hide form window.
                ShowInTaskbar = false; // Remove from taskbar.
              
                base.OnLoad(e);
            }

            [DllImport("User32.dll")]
            private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // Keys enumeration

            [DllImport("User32.dll")]
            private static extern short GetAsyncKeyState(System.Int32 vKey);


            private void OnExit(object sender, EventArgs e)
            {
              // HotKey.UnregisterHotKey(Handle, 100);
                Application.Exit();
            }

            protected override void Dispose(bool isDisposing)
            {
                if (isDisposing)
                {
                    // Release the icon resource.
                    trayIcon.Dispose();
                }

                base.Dispose(isDisposing);
            }

        }
    }
}
