using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VMultiDllWrapper
{
    public class Mouse
    {
        #region API
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(
            int nIndex
        );
        const int SM_CXSCREEN = 0, SM_CYSCREEN=1;


        #endregion
        MouseReport report;
        double ImageresolutionX, ImageresolutionY;
        public Mouse()
        {
            ImageresolutionX = GetSystemMetrics(SM_CXSCREEN)-1;
            ImageresolutionY = GetSystemMetrics(SM_CYSCREEN)-1;
            if (Variable_Internal.vmulti==(null))
                Variable_Internal.vmulti = new VMulti();
            report = new MouseReport();
        }
        ~Mouse()
        {
            if (!(Variable_Internal.vmulti == null))
            {
                Variable_Internal.vmulti.disconnect();
                Variable_Internal.vmulti.free();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            if (!(Variable_Internal.vmulti == null) && !Variable_Internal.vmulti.isConnected())
                return Variable_Internal.vmulti.connect();
            else if (!(Variable_Internal.vmulti == null) && Variable_Internal.vmulti.isConnected())
                return true;
            else
                return false;
        }
        #region button
        public bool LeftDown()
        {
            report.Buttons.Add(MouseReport.Button.MOUSE_BUTTON_1);
            report.X = 0;
            report.Y = 0;
            return Variable_Internal.vmulti.updateMouse(report);
        }
        public bool LeftUp()
        {
            report.Buttons.Remove(MouseReport.Button.MOUSE_BUTTON_1);
            report.X = 0;
            report.Y = 0;
            return Variable_Internal.vmulti.updateMouse(report);
        }
        public bool RightDown()
        {
            report.Buttons.Add(MouseReport.Button.MOUSE_BUTTON_2);
            report.X = 0;
            report.Y = 0;
            return Variable_Internal.vmulti.updateMouse(report);
        }
        public bool RightUp()
        {
            report.Buttons.Remove(MouseReport.Button.MOUSE_BUTTON_2);
            report.X = 0;
            report.Y = 0;
            return Variable_Internal.vmulti.updateMouse(report);
        }
        public bool MiddleDown()
        {
            report.Buttons.Add(MouseReport.Button.MOUSE_BUTTON_3);
            report.X = 0;
            report.Y = 0;
            return Variable_Internal.vmulti.updateMouse(report);
        }
        public bool MiddleUp()
        {
            report.Buttons.Remove(MouseReport.Button.MOUSE_BUTTON_3);
            report.X = 0;
            report.Y = 0;
            return Variable_Internal.vmulti.updateMouse(report);
        }
        #endregion button
        /// <summary>
        /// 滾動滑鼠滾輪
        /// </summary>
        /// <param name="c">滾動圈數(-127~+127)</param>
        /// <returns></returns>
        public bool Wheel(int c)
        {          
            report.wheelPosition = (byte)(256-c);
            report.X = 0;
            report.Y = 0;
            return Variable_Internal.vmulti.updateMouse(report);
        }
        
        /*
        public bool MouseMove(ushort x,ushort y)
        {
            report.X = x;
            report.Y = y;
            return Variable_Internal.vmulti.updateMouse(report);
        }
        */
        /*
        public bool MouseMoveRalative(ushort x, ushort y)
        {
            report.X = x;
            report.Y = y;
            return Variable_Internal.vmulti.updateRelativeMouse(report);
        }
        */

        public bool MouseMove(uint x, uint y)
        {
            report.X = (x + y == 0) ? (ushort)1 : ((ushort)(Math.Ceiling((32767.0 / ImageresolutionX) * (double)x <= ImageresolutionX ? (double)x : ImageresolutionX)));
            report.Y = (x + y == 0) ? (ushort)1 : ((ushort)(Math.Ceiling((32767.0 / ImageresolutionY) * (double)y <= ImageresolutionY ? (double)y : ImageresolutionY)));
            return Variable_Internal.vmulti.updateMouse(report);
        }
        public bool MouseMoveRalative(int x, int y)
        {
            int[] xx = { 0, 0 }, yy = { 0, 0 };
            xx[0] = x / 127;
            yy[0] = y / 127;
            xx[1] = x % 127;
            yy[1] = y % 127;
            bool resoult = false;
            while (xx[0] != 0 || xx[1] != 0 || yy[0] != 0 || yy[1] != 0)
            {
                if (xx[0] > 0)
                {
                    report.X = (ushort)(16384 + 127);
                    xx[0]--;
                }
                else if (xx[0] < 0)
                {
                    report.X = (ushort)(16384 - 127);
                    xx[0]++;
                }
                else
                {
                    report.X = (ushort)(16384 + xx[1]);
                    xx[1] = 0;
                }

                if (yy[0] > 0)
                {
                    report.Y = (ushort)(16384 + 127);
                    yy[0]--;
                }
                else if (yy[0] < 0)
                {
                    report.Y = (ushort)(16384 - 127);
                    yy[0]++;
                }
                else
                {
                    report.Y = (ushort)(16384 + yy[1]);
                    yy[1] = 0;
                }
                resoult = Variable_Internal.vmulti.updateRelativeMouse(report);
            }

            return resoult;
        }
    }
}
