using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
//using WindowsInput;
//using WindowsInput.Native;

namespace GuacamoleAssistant
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int HOTKEY_ID1 = 31197; //Any number to use to identify the hotkey instance
        const int HOTKEY_ID2 = 31198; //Any number to use to identify the hotkey instance
        const int WM_HOTKEY = 0x0312;

        [DllImport("shell32.dll", EntryPoint = "#61", CharSet = CharSet.Unicode)]
        public static extern int RunFileDlg(
        [In] IntPtr hWnd,
        [In] IntPtr icon,
        [In] string path,
        [In] string title,
        [In] string prompt,
        [In] uint flags);

        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        // send AltTab
        KeyModifiers userKeyModifiers1 = KeyModifiers.Control;
        Keys userKey1 = Keys.OemQuestion;

        // send Win
        KeyModifiers userKeyModifiers2 = KeyModifiers.Control | KeyModifiers.Alt;
        Keys userKey2 = Keys.M;

        //InputSimulator inps;

        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WM_HOTKEY:
                    Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF);
                    KeyModifiers modifier = (KeyModifiers)((int)message.LParam & 0xFFFF);
                    //MessageBox.Show("HotKey Pressed :" + modifier.ToString() + " " + key.ToString());

                    if ((userKeyModifiers1) == modifier && userKey1 == key)
                    {
                        // Send Alt-Tab 
                        SendKeys.Send("%{Tab}");
                    }

                    if ((userKeyModifiers2) == modifier && userKey2 == key)
                    {
                        RunFileDlg(IntPtr.Zero, IntPtr.Zero, null, null, null, 0);
                        //KeyboardSend.KeyDown(Keys.LWin);
                        //KeyboardSend.KeyDown(Keys.R);
                        //KeyboardSend.KeyUp(Keys.R);
                        //KeyboardSend.KeyUp(Keys.LWin);

                        //Thread.Sleep(100);
                        // InputSimulator
                        //inps.Keyboard.KeyPress(VirtualKeyCode.LWIN, VirtualKeyCode.VK_E);
                        //inps.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_E);
                    }

                    break;
            }
            base.WndProc(ref message);
        }
        

        private void Main_Load(object sender, EventArgs e)
        {
            //inps = new InputSimulator();
            RegisterHotKey(this.Handle, HOTKEY_ID1, userKeyModifiers1, userKey1);
            RegisterHotKey(this.Handle, HOTKEY_ID2, userKeyModifiers2, userKey2);

            notifyIcon.Visible = true;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID1);
            UnregisterHotKey(this.Handle, HOTKEY_ID2);
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendKeys.Send("%{Tab}");
        }
    }
}
