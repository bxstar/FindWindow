using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;
using log4net;

namespace FindWindow
{
    public partial class Frm移动 : Form
    {
        //日志类
        ILog logger = LogManager.GetLogger("Logger");
        /// <summary>
        /// 信息数
        /// </summary>
        private int MsgCount = 0;

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

        public Frm移动()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            txtWindowName.Text = "飞信2013";
            txtUserCode.Text = "15901925614";
            txtMsg.Text = "我是一只小小鸟";

            //SetCursorPos(1024, 768); //设置鼠标位置
            //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

            //System.Threading.Thread.Sleep(1000);


            //IntPtr hwndCalc = FindWindow(null, this.txtWindowName.Text);
            //if (hwndCalc != null)
            //{
            //    List<IntPtr> list = new List<IntPtr>();
            //    WinAPIuser32.CallFuntion = delegate(IntPtr enumIntPtr)
            //    {
            //        StringBuilder s = new StringBuilder(2000);
            //        WinAPIuser32.GetClassName(enumIntPtr, s, 255);
            //        if (s.ToString() == "FxRichEdit")
            //        {
            //            list.Add(enumIntPtr);
            //        }
            //    };
            //    WinAPIuser32.EnumChildWindows(hwndCalc, 0);
            //    WinAPIuser32.CallFuntion = null;

            //    WinAPIuser32.SendTextMessage(list[0], WM_SETTEXT, 0, "李俊");
            //    PostMessage(list[0], WM_KEYDOWN, 13, 0);
            //    WinAPIuser32.SendTextMessage(list[2], WM_SETTEXT, 0, txtMsg.Text);



            //}

        }

        private void btnSendOneMsg_Click(object sender, EventArgs e)
        {
            MsgCount++;
            logger.InfoFormat("第{0}条开始发送", MsgCount);
            IntPtr wndFx = FindWindow(null, this.txtWindowName.Text);

            if (wndFx == null)
            {
                MessageBox.Show("飞信2013窗口未找到");
                return;
            }

            Rectangle rc = new Rectangle();
            GetWindowRect(wndFx.ToInt32(), ref rc);

            //移动到短信按钮，并点击
            SetCursorPos(rc.X + 69, rc.Y + 438);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

            Thread.Sleep(500);

            IntPtr wndSendMsg = FindWindow(null, "发短信");
            if (wndSendMsg != null)
            {
                Rectangle rcSendMsg = new Rectangle();
                GetWindowRect(wndSendMsg.ToInt32(), ref rcSendMsg);

                //移动到接收人，并设置光标
                SetCursorPos(rcSendMsg.X + 100, rcSendMsg.Y + 42); //设置鼠标位置
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                Thread.Sleep(500);

                //寻找接收人和短信内容句柄
                List<IntPtr> list = new List<IntPtr>();
                WinAPIuser32.CallFuntion = delegate(IntPtr enumIntPtr)
                {
                    StringBuilder s = new StringBuilder(2000);
                    WinAPIuser32.GetClassName(enumIntPtr, s, 255);
                    if (s.ToString() == "FxRichEdit")
                    {
                        list.Add(enumIntPtr);
                    }
                };
                WinAPIuser32.EnumChildWindows(wndSendMsg, 0);
                WinAPIuser32.CallFuntion = null;

                //设置接收人
                Console.WriteLine("lst count:" + list.Count);
                WinAPIuser32.SendTextMessage(list[0], WM_SETTEXT, 0, txtUserCode.Text);

                Thread.Sleep(500);
                //模拟回车
                PostMessage(list[0], WM_KEYDOWN, 13, 0);

                //设置短信内容
                Thread.Sleep(500);
                WinAPIuser32.SendTextMessage(list[2], WM_SETTEXT, 0, txtMsg.Text);


                //移动到发送按钮，并点击
                SetCursorPos(rcSendMsg.X + 340, rcSendMsg.Y + 210);
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                Thread.Sleep(4000);
                //移动到关闭按钮，并点击
                SetCursorPos(rcSendMsg.X + 380, rcSendMsg.Y + 16);
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }

            logger.InfoFormat("第{0}条完成发送", MsgCount);
            btnSendOneMsg.Enabled = true;
        }


        private void btnAddUser_Click(object sender, EventArgs e)
        {
            IntPtr wndFx = FindWindow(null, this.txtWindowName.Text);

            if (wndFx == null)
            {
                MessageBox.Show("飞信2013窗口未找到");
                return;
            }

            Rectangle rc = new Rectangle();
            GetWindowRect(wndFx.ToInt32(), ref rc);
            SetCursorPos(rc.X + 140, rc.Y + 468); //设置鼠标位置  

            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

            Thread.Sleep(500);


            IntPtr wndAddUser = FindWindow(null, "添加好友");
            if (wndAddUser != null)
            {
                Rectangle rcAddUser = new Rectangle();
                GetWindowRect(wndAddUser.ToInt32(), ref rcAddUser);

                //寻找好友句柄
                List<IntPtr> list = new List<IntPtr>();
                WinAPIuser32.CallFuntion = delegate(IntPtr enumIntPtr)
                {
                    StringBuilder s = new StringBuilder(2000);
                    WinAPIuser32.GetClassName(enumIntPtr, s, 255);
                    if (s.ToString() == "FxEdit")
                    {
                        list.Add(enumIntPtr);
                    }
                };
                WinAPIuser32.EnumChildWindows(wndAddUser, 0);
                WinAPIuser32.CallFuntion = null;

                //设置好友
                Console.WriteLine("lst count:" + list.Count);
                WinAPIuser32.SendTextMessage(list[3], WM_SETTEXT, 0, txtUserCode.Text);

                //移动到发送按钮，并点击
                SetCursorPos(rcAddUser.X + 430, rcAddUser.Y + 100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                Thread.Sleep(500);

                //寻找显示名称，申请理由句柄
                list = new List<IntPtr>();
                WinAPIuser32.CallFuntion = delegate(IntPtr enumIntPtr)
                {
                    StringBuilder s = new StringBuilder(2000);
                    WinAPIuser32.GetClassName(enumIntPtr, s, 255);
                    if (s.ToString() == "FxEdit")
                    {
                        list.Add(enumIntPtr);
                    }
                };
                WinAPIuser32.EnumChildWindows(wndAddUser, 0);
                WinAPIuser32.CallFuntion = null;

                WinAPIuser32.SendTextMessage(list[4], WM_SETTEXT, 0, txtUserCode.Text);
                WinAPIuser32.SendTextMessage(list[5], WM_SETTEXT, 0, string.Format("欢迎使用淘快词，请加为好友，否则将不能收到短信通知"));

                //Thread.Sleep(500);
                ////模拟回车
                ////移动到确定按钮，并点击
                //SetCursorPos(rcAddUser.X + 390, rcAddUser.Y + 318);
                //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                //Thread.Sleep(4000);
                ////移动到关闭按钮，并点击
                //SetCursorPos(rcAddUser.X + 490, rcAddUser.Y + 10);
                //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
        }

        /// <summary>
        /// 发送一个字符串
        /// </summary>
        /// <param name="myIntPtr">窗口句柄</param>
        /// <param name="Input">字符串</param>
        public void InputStr(IntPtr myIntPtr, string Input)
        {
            byte[] ch = (ASCIIEncoding.ASCII.GetBytes(Input));
            for (int i = 0; i < ch.Length; i++)
            {
                SendMessage(myIntPtr, WM_CHAR, ch[i], 0);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            btnSendOneMsg.Enabled = true;
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!btnSendOneMsg.Enabled)
            {
                btnSendOneMsg.Enabled = true;
            }
            //btnSearch_Click(null, null);
            btnGetMsgSendPhone_Click(null, null);
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            timer1.Enabled = true;
        }

        /// <summary>
        /// 获取信息，并通过飞信发送
        /// </summary>
        private void btnGetMsgSendPhone_Click(object sender, EventArgs e)
        {
            //DianCheWebService.ItemClickService ws = new DianCheWebService.ItemClickService();
            //DianCheWebService.MySoapHeader header = new DianCheWebService.MySoapHeader();
            //header.UserName = "yurongsheng";
            //header.PassWord = "carlos";
            //ws.MySoapHeaderValue = header;
            //var arrMsg = ws.GetMsg("短信");
            //if (arrMsg.Length == 0)
            //    return;
            //foreach (var msg in arrMsg)
            //{
            //    try
            //    {
            //        txtUserCode.Text = msg.msg_to;
            //        txtMsg.Text = msg.msg_content;
            //        Thread.Sleep(1000);

            //        btnSendOneMsg_Click(null, null);
            //        ws.UpdateMsgStatus(msg.local_msg_id, 2);
            //    }
            //    catch (Exception se)
            //    {
            //        logger.Error(string.Format("用户：{0}，local_msg_id：{1}，信息发送失败", msg.msg_from, msg.local_msg_id), se);
            //        ws.UpdateMsgStatus(msg.local_msg_id, 3);
            //    }

            //}
        }


    }
}