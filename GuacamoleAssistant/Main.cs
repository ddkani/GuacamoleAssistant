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

        [DllImport("user32")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int HOTKEY_ID1 = 31197; //Any number to use to identify the hotkey instance
        const int HOTKEY_ID2 = 31198; //Any number to use to identify the hotkey instance
        const int HOTKEY_ID3 = 31199;
        const int HOTKEY_ID4 = 31200;
        const int HOTKEY_ID5 = 31201;
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
            Windows = 8,
            NoRepeat = 16384
        }

        //http://ldg119.tistory.com/234

        int VK_HANGULE = 0x15;
        int VK_LCONTROL = 0xA2;
        int VK_LEFT = 0x25;
        int VK_RIGHT = 0x27;
        int VK_LWIN = 0x5C;

        void ToggleKey(int key)
        {
            KeyboardSend.KeyDown(key);
            KeyboardSend.KeyUp(key);
        }

        private void ShowToolTip(object sender, string message)
        {
            //new ToolTip().Show(message, this, Cursor.Position.X - this.Location.X, Cursor.Position.Y - this.Location.Y, 1000);
            new ToolTip().Show(message, this, Screen.PrimaryScreen.WorkingArea.Width, 
                Screen.PrimaryScreen.WorkingArea.Height, 500);
        }

        // send alttab
        KeyModifiers userKeyModifiers1 = KeyModifiers.Control;
        Keys userKey1 = Keys.OemQuestion;

        // HanEng switch
        KeyModifiers userKeyModifiers2 = KeyModifiers.Shift;
        Keys userKey2 = Keys.Space;

        // left vd
        KeyModifiers userKeyModifiers3 = KeyModifiers.Control;
        Keys userKey3 = Keys.F11;

        // right vd
        KeyModifiers userKeyModifiers4 = KeyModifiers.Control;
        Keys userKey4 = Keys.F12;

        KeyModifiers userKeyModifiers5 = KeyModifiers.Control | KeyModifiers.Shift;
        Keys userKey5 = Keys.OemQuestion;

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
                        //ShowToolTip(this, "Show Alt+Tab Window");
                    }

                    if ((userKeyModifiers2) == modifier && userKey2 == key)
                    {
                        ToggleKey(VK_HANGULE);
                        //ShowToolTip(this, "Switch IME (Han/Eng)");
                    }

                    if ((userKeyModifiers3) == modifier && userKey3 == key)
                    {
                        // Send {Control down}{LWin down}{Left}{Control up}{LWin up}
                        KeyboardSend.KeyDown(VK_LCONTROL);
                        KeyboardSend.KeyDown(VK_LWIN);
                        KeyboardSend.KeyDown(VK_LEFT);
                        KeyboardSend.KeyUp(VK_LCONTROL);
                        KeyboardSend.KeyUp(VK_LWIN);
                        KeyboardSend.KeyUp(VK_LEFT);
                        //ShowToolTip(this, "Move Virtual Desktop to LEFT");

                        //MessageBox.Show("");

                    }

                    if ((userKeyModifiers4) == modifier && userKey4 == key)
                    {
                        // Send {Control down}{LWin down}{Left}{Control up}{LWin up}
                        KeyboardSend.KeyDown(VK_LCONTROL);
                        KeyboardSend.KeyDown(VK_LWIN);
                        KeyboardSend.KeyDown(VK_RIGHT);
                        KeyboardSend.KeyUp(VK_LCONTROL);
                        KeyboardSend.KeyUp(VK_LWIN);
                        KeyboardSend.KeyUp(VK_RIGHT);
                        //ShowToolTip(this, "Move Virtual Desktop to RIGHT");

                        //MessageBox.Show("");
                    }

                    if ((userKeyModifiers5) == modifier && userKey5 == key)
                    {
                        RunFileDlg(IntPtr.Zero, IntPtr.Zero, null, null, null, 0);
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
            RegisterHotKey(this.Handle, HOTKEY_ID3, userKeyModifiers3, userKey3);
            RegisterHotKey(this.Handle, HOTKEY_ID4, userKeyModifiers4, userKey4);
            RegisterHotKey(this.Handle, HOTKEY_ID4, userKeyModifiers5, userKey5);

            notifyIcon.Visible = true;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID1);
            UnregisterHotKey(this.Handle, HOTKEY_ID2);
            UnregisterHotKey(this.Handle, HOTKEY_ID3);
            UnregisterHotKey(this.Handle, HOTKEY_ID4);
            UnregisterHotKey(this.Handle, HOTKEY_ID5);
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
            //SendKeys.Send("%{Tab}");
            //ToggleKey(VK_LWIN);
            KeyboardSend.KeyDown(VK_LCONTROL);
            KeyboardSend.KeyDown(VK_LWIN);
            KeyboardSend.KeyDown(VK_RIGHT);
            KeyboardSend.KeyUp(VK_LCONTROL);
            KeyboardSend.KeyUp(VK_LWIN);
            KeyboardSend.KeyUp(VK_RIGHT);

        }
    }
}
