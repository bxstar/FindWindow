using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FindWindow
{
    public class WinAPIuser32
    {
        /// <summary>
        /// 根据句柄得到控件ID
        /// </summary>
        /// <param name="hwndCtl"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetDlgCtrlID(IntPtr hwndCtl);
        /// <summary>
        /// 由控件ID获得控件窗口句柄
        /// </summary>
        /// <param name="hDlg"></param>
        /// <param name="nIDDlgItem"></param>
        /// <returns></returns>
        [DllImport("user32.dll ", EntryPoint = "GetDlgItem")]
        public static extern IntPtr GetDlgItem(
        IntPtr hDlg,
        int nIDDlgItem
        );
        /// <summary>
        ///  查找句柄
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// 查找句柄
        /// </summary>
        /// <param name="hwndParent"></param>
        /// <param name="hwndChildAfter"></param>
        /// <param name="lpszClass">类名</param>
        /// <param name="lpszWindow">标题</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        public static extern int FindWindowEx(int hwndParent, int hwndChildAfter, string lpszClass, string lpszWindow);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);
        [DllImport("User32.dll")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern int SendTextMessage(
            IntPtr hWnd, // handle to destination window 
            int Msg, // message 
            int wParam, // first message parameter 
            string lParam
            // int  lParam // second message parameter 
        );
        /// <summary>
        /// 获取焦点
        /// </summary>
        /// <param name="hwnd"></param>
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern int SetWindowText(int hwnd, string lpString);

        [DllImport("user32.dll")]
        public static extern int SetFocus(int hWnd);

        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(int hWndParent, CallBack lpfn, int lParam);
        /// <summary>
        /// 回调业务
        /// </summary>
        public delegate void CallBusiness(IntPtr hwnd);
        public delegate bool CallBack(IntPtr hwnd, int lParam);
        /// <summary>
        /// 遍历子窗体的父窗体句柄
        /// </summary>
        public static CallBack callBackEnumChildWindows = new CallBack(ChildWindowProcess);
        /// <summary>
        /// 委托业务,需要客户端添加
        /// </summary>
        public static CallBusiness CallFuntion;
        /// <summary>
        /// 遍历子窗体或控件
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static bool EnumChildWindows(IntPtr hWnd, int lParam)
        {
            EnumChildWindows(hWnd.ToInt32(), callBackEnumChildWindows, 0);
            return true;
        }
        /// <summary>
        /// 获取类名字
        /// </summary>
        /// <param name="hwnd">需要获取类名的句柄</param>
        /// <param name="lpClassName">类名(执行完成以后查看)</param>
        /// <param name="nMaxCount">缓冲区</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(
            IntPtr hwnd,
            StringBuilder lpClassName,
            int nMaxCount
        );
        /// <summary>
        /// 遍历子控件
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static bool ChildWindowProcess(IntPtr hwnd, int lParam)
        {
            if (CallFuntion != null)
            {
                CallFuntion(hwnd);
            }
            return true;
        }
    }
}
