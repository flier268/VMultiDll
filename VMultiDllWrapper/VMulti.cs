using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VMultiDllWrapper
{
    public class VMulti
    {
        [DllImport("VMultiDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HelloWorld();

        [DllImport("VMultiDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr vmulti_alloc();

        [DllImport("VMultiDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void vmulti_free(IntPtr vmulti);
        
        [DllImport("VMultiDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool vmulti_connect(IntPtr vmulti, int i);

        [DllImport("VMultiDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void vmulti_disconnect(IntPtr vmulti);

        [DllImport("VMultiDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool vmulti_update_joystick(IntPtr vmulti, ushort buttons, byte hat, byte x, byte y, byte z, byte rz, byte throttle);

        [DllImport("VMultiDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool vmulti_update_multitouch(IntPtr vmulti, MultitouchPointerInfoRaw[] pTouch, byte actualCount, byte request_type, byte report_control_id);

        [DllImport("VMultiDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool vmulti_update_keyboard(IntPtr vmulti, byte shiftKeyFlags, byte[] keyCodes);
        [DllImport("VMultiDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool vmulti_update_mouse(IntPtr vmulti, byte button, ushort x, ushort y, byte wheelPosition);
        [DllImport("VMultiDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool vmulti_update_relative_mouse(IntPtr vmulti, byte button, ushort x, ushort y, byte wheelPosition);


        private IntPtr vmulti;
        private bool connected;

        public VMulti()
        {
            vmulti = vmulti_alloc();
        }
        ~VMulti()
        {
            disconnect();
            vmulti_free(vmulti);
        }
        public virtual bool isConnected()
        {
            return this.connected;
        }

        public virtual bool connect()
        {
            return this.connected = vmulti_connect(vmulti, 1);
        }

        public virtual void disconnect()
        {
            if (connected)
            {
                vmulti_disconnect(vmulti);
            }
        }
        public virtual void free()
        {
            vmulti_free(vmulti);
        }
        public virtual bool updateJoystick(JoystickReport report)
        {
            if (connected)
            {
                return vmulti_update_joystick(vmulti, report.getButtonsRaw(), report.getPOVRaw(), report.getJoystickXRaw(), report.getJoystickYRaw(), 0, 128, 0);
            }
            else
            {
                return false;
            }
        }

        public virtual bool updateMultitouch(MultitouchReport report)
        {
            if (connected)
            {
                MultitouchPointerInfoRaw[] touches = report.getTouchesRaw();
                return vmulti_update_multitouch(vmulti, touches, report.getTouchesCountRaw(), report.getRequestType(), report.getReportControlId());
            }
            else
            {
                return false;
            }
        }

       internal virtual bool updateKeyboard(KeyboardReport report)
        {
            if (connected)
            {
                return vmulti_update_keyboard(vmulti, report.getRawShiftKeyFlags(), report.getRawKeyCodes());
            }
            else
            {
                return false;
            }
        }
        internal virtual bool updateMouse(MouseReport report)
        {
            if (connected)
            {
                return vmulti_update_mouse(vmulti, report.getButton(),report.X,report.Y, report.wheelPosition);
            }
            else
            {
                return false;
            }
        }
        internal virtual bool updateRelativeMouse(MouseReport report)
        {
            if (connected)
            {
                return vmulti_update_relative_mouse(vmulti, report.getButton(), report.X, report.Y, report.wheelPosition);
            }
            else
            {
                return false;
            }
        }
    }
}
