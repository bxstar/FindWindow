using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;

namespace FindWindow
{
    public partial class Frm联通 : Form
    {
        #region 常量定义
        //设置鼠标位置
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        //模拟鼠标
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// 查找句柄
        /// </summary> 
        [DllImport("User32.DLL")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        //发送按键消息必用PostMessage,SendMessage有时会不起作用
        [DllImport("user32.dll", EntryPoint = "PostMessage", SetLastError = true)]
        private extern static bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        //获取指定句柄的相对屏幕的位置和矩形区域
        [DllImport("user32")]
        public static extern int GetWindowRect(int hwnd, ref Rectangle lpRect);

        [DllImport("kernel32")]
        public static extern IntPtr GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);

        [DllImport("user32.dll")]
        private static extern IntPtr GetFocus();

        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; //模拟鼠标左键按下参数
        private const int MOUSEEVENTF_LEFTUP = 0x0004; //模拟鼠标左键抬起参数

        //SendMessage参数

        private const int WM_KEYDOWN = 0X100;
        private const int WM_KEYUP = 0X101;
        private const int WM_SYSCHAR = 0X106;
        private const int WM_SYSKEYUP = 0X105;
        private const int WM_SYSKEYDOWN = 0X104;
        private const int WM_CHAR = 0X102;
        /// <summary>
        /// 设置文本
        /// </summary>
        private const int WM_SETTEXT = 0xC;

        private const int WM_GETTEXT = 0x0D;
        #endregion

        public Frm联通()
        {
            InitializeComponent();
        }

        private void Frm联通_Load(object sender, EventArgs e)
        {
            txtWindowName.Text = "outWidget";
            txtUserCode.Text = "15901925614";
            txtMsg.Text = "我是一只小小鸟";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //沃友无法通过标题获取窗口句柄，因此只能根据进程来获取句柄
            int pid = Win32Helper.GetPidByProcessName("WoYou");
            IntPtr wndFx = new Win32Helper().GetMainWindowHandle(pid);
            Rectangle rc = new Rectangle();
            GetWindowRect(wndFx.ToInt32(), ref rc);

            //移动到短信按钮，并点击
            SetCursorPos(rc.X + 170, rc.Y + 570);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {

        }
    }
}
