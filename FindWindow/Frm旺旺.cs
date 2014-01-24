using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace FindWindow
{
    public partial class Frm旺旺 : FrmBase
    {
        private string strSendUser = "zhanaiqiu";

        public Frm旺旺()
        {
            InitializeComponent();
        }

        private void Frm旺旺_Load(object sender, EventArgs e)
        {
            txtSendUser.Text = strSendUser;
            txtWindowName.Text = string.Format("{0}-阿里旺旺", strSendUser);
            txtUserName.Text = "巴西球星";
            txtMsg.Text = "我是一只小小鸟2";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            IntPtr wndFx = FindWindow(null, this.txtWindowName.Text);

            if (wndFx == null)
            {
                MessageBox.Show(string.Format("{0}窗口未找到", this.txtWindowName.Text));
                return;
            }

            Rectangle rc = new Rectangle();
            GetWindowRect(wndFx.ToInt32(), ref rc);

            //移动到好友搜索栏，并点击
            SetCursorPos(rc.X + 50, rc.Y + 150);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

            //寻找查找好友，群和宝贝句柄
            List<IntPtr> list = new List<IntPtr>();
            WinAPIuser32.CallFuntion = delegate(IntPtr enumIntPtr)
            {
                StringBuilder s = new StringBuilder(2000);
                WinAPIuser32.GetClassName(enumIntPtr, s, 255);
                if (s.ToString() == "EditComponent")
                {
                    list.Add(enumIntPtr);
                }
            };
            WinAPIuser32.EnumChildWindows(wndFx, 0);
            WinAPIuser32.CallFuntion = null;

            //设置好友名称
            Console.WriteLine("lst count:" + list.Count());
            WinAPIuser32.SendTextMessage(list[1], WM_SETTEXT, 0, txtUserName.Text);

            Thread.Sleep(500);

            //模拟回车
            PostMessage(list[1], WM_KEYDOWN, 13, 0);


            Thread.Sleep(500);

            //寻找聊天窗口
            IntPtr wndSendMsg = FindWindow(null, string.Format("{0} - {1}", txtUserName.Text, txtSendUser.Text));


            //寻找信息编辑框
            List<IntPtr> listRichEditComponent = new List<IntPtr>();
            WinAPIuser32.CallFuntion = delegate(IntPtr enumIntPtr)
            {
                StringBuilder s = new StringBuilder(2000);
                WinAPIuser32.GetClassName(enumIntPtr, s, 255);
                if (s.ToString() == "RichEditComponent")
                {
                    listRichEditComponent.Add(enumIntPtr);
                }
            };
            WinAPIuser32.EnumChildWindows(wndSendMsg, 0);
            WinAPIuser32.CallFuntion = null;

            //输入信息内容
            Console.WriteLine("listRichEditComponent count:" + listRichEditComponent.Count());
            WinAPIuser32.SendTextMessage(listRichEditComponent[0], WM_SETTEXT, 0, txtMsg.Text);

            Thread.Sleep(500);

            //寻找发送按钮
            Rectangle rcSendMsg = new Rectangle();
            GetWindowRect(wndSendMsg.ToInt32(), ref rcSendMsg);

            //移动到发送按钮，并点击
            SetCursorPos(rcSendMsg.X + 300, rcSendMsg.Y + 488);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);


            Thread.Sleep(500);
            //移动到关闭按钮，并点击
            SetCursorPos(rcSendMsg.X + 544, rcSendMsg.Y + 14);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
    }
}
