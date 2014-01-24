using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FindWindow
{
    public partial class FrmBase : Form
    {
        //设置鼠标位置
        [DllImport("user32.dll")]
        protected static extern bool SetCursorPos(int X, int Y);

        //模拟鼠标
        [DllImport("user32.dll")]
        protected static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        protected static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// 查找句柄
        /// </summary> 
        [DllImport("User32.DLL")]
        protected static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        protected static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        //发送按键消息必用PostMessage,SendMessage有时会不起作用
        [DllImport("user32.dll", EntryPoint = "PostMessage", SetLastError = true)]
        protected extern static bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        //获取指定句柄的相对屏幕的位置和矩形区域
        [DllImport("user32")]
        protected static extern int GetWindowRect(int hwnd, ref Rectangle lpRect);

        [DllImport("kernel32")]
        protected static extern IntPtr GetCurrentThreadId();

        [DllImport("user32.dll")]
        protected static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        protected static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);

        [DllImport("user32.dll")]
        protected static extern IntPtr GetFocus();

        protected const int MOUSEEVENTF_LEFTDOWN = 0x0002; //模拟鼠标左键按下参数
        protected const int MOUSEEVENTF_LEFTUP = 0x0004; //模拟鼠标左键抬起参数

        //SendMessage参数

        protected const int WM_KEYDOWN = 0X100;
        protected const int WM_KEYUP = 0X101;
        protected const int WM_SYSCHAR = 0X106;
        protected const int WM_SYSKEYUP = 0X105;
        protected const int WM_SYSKEYDOWN = 0X104;
        protected const int WM_CHAR = 0X102;
        /// <summary>
        /// 设置文本
        /// </summary>
        protected const int WM_SETTEXT = 0xC;

        protected const int WM_GETTEXT = 0x0D;

        public FrmBase()
        {
            InitializeComponent();
        }
    }
}
