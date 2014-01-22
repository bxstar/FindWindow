using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace FindWindow
{
    public class UtilFindWindow
    {
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        //IMPORTANT : LPARAM  must be a pointer (InterPtr) in VS2005, otherwise an exception will be thrown
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);
        //the callback function for the EnumChildWindows
        private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        //if found  return the handle , otherwise return IntPtr.Zero
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        private string m_classname; // class name to look for
        private string m_caption; // caption name to look for

        private DateTime start;
        private int m_timeout;//If exceed the time. Indicate no windows found.

        private IntPtr m_hWnd; // HWND if found
        public IntPtr FoundHandle
        {
            get { return m_hWnd; }
        }

        private bool m_IsTimeOut;
        public bool IsTimeOut
        {
            get{return m_IsTimeOut;}
            set { m_IsTimeOut = value; }
        }

        // ctor does the work--just instantiate and go
        public UtilFindWindow(IntPtr hwndParent, string classname, string caption, int timeout)
        {
            m_hWnd = IntPtr.Zero;
            m_classname = classname;
            m_caption = caption;
            m_timeout = timeout;
            start = DateTime.Now;
            FindChildClassHwnd(hwndParent, IntPtr.Zero);
        }

        /**/
        /// <summary>
        /// Find the child window, if found m_classname will be assigned 
        /// </summary>
        /// <param name="hwndParent">parent's handle</param>
        /// <param name="lParam">the application value, nonuse</param>
        /// <returns>found or not found</returns>
        //The C++ code is that  lParam is the instance of FindWindow class , if found assign the instance's m_hWnd
        private bool FindChildClassHwnd(IntPtr hwndParent, IntPtr lParam)
        {
            EnumWindowProc childProc = new EnumWindowProc(FindChildClassHwnd);
            IntPtr hwnd = FindWindowEx(hwndParent, IntPtr.Zero, m_classname, m_caption);
            if (hwnd != IntPtr.Zero)
            {
                this.m_hWnd = hwnd; // found: save it
                m_IsTimeOut = false;
                return false; // stop enumerating
            }

            DateTime end = DateTime.Now;

            if (start.AddSeconds(m_timeout) < end)
            {
                m_IsTimeOut = true;
                return false;
            }

            EnumChildWindows(hwndParent, childProc, IntPtr.Zero); // recurse  redo FindChildClassHwnd
            return true;// keep looking
        }
    }
}
