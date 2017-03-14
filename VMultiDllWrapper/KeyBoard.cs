using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VMultiDllWrapper
{
    public class KeyBoard
    {
        [DllImport("USER32.dll")]
        static extern short GetKeyState(int nVirtKey);
        KeyboardReport report;
       
        public KeyBoard()
        {
            if ((Variable_Internal.vmulti==null))
                Variable_Internal.vmulti = new VMulti();
            report = new KeyboardReport();
        }
        ~KeyBoard()
        {
            if (!(Variable_Internal.vmulti == null))
            {
                Variable_Internal.vmulti.disconnect();
                Variable_Internal.vmulti.free();
            }
        }
        public void Init()
        {
            if (!(Variable_Internal.vmulti==null) && !Variable_Internal.vmulti.isConnected())
                Variable_Internal.vmulti.connect();
        }
        public void KeyDown(KeyboardKey key = new KeyboardKey(), KeyboardModifier modikey = new KeyboardModifier())
        {
            if (key != 0)
                report.keyDown(key);
            if (modikey != 0)
                report.keyDown(modikey);
            Variable_Internal.vmulti.updateKeyboard(report);
        }
        public void KeyUp(KeyboardKey key = new KeyboardKey(), KeyboardModifier modikey = new KeyboardModifier())
        {
            if (key != 0)
                report.keyUp(key);
            if (modikey != 0)
                report.keyUp(modikey);
            Variable_Internal.vmulti.updateKeyboard(report);
        }
        public void KeyPress(KeyboardKey key = new KeyboardKey(), KeyboardModifier modikey = new KeyboardModifier(), int Intervals1 = 20,int Intervals2 = 20)
        {
            KeyDown(key, modikey);
            System.Threading.Thread.Sleep(Intervals1);
            KeyUp(key, modikey);
            System.Threading.Thread.Sleep(Intervals2);
        }

        public void KeyDown(HashSet<KeyboardKey> keys = null, KeyboardModifier[] modikeys = null)
        {
            if (keys != null)
                foreach (KeyboardKey key in keys)
                    report.keyDown(key);
            if (modikeys != null)
                foreach (KeyboardKey key in modikeys)
                    report.keyDown(key);
            Variable_Internal.vmulti.updateKeyboard(report);
        }
        public void KeyUp(HashSet<KeyboardKey> keys = null, KeyboardModifier[] modikeys = null)
        {
            if (keys != null)
                foreach (KeyboardKey key in keys)
                    report.keyUp(key);
            if (modikeys != null)
                foreach (KeyboardKey key in modikeys)
                    report.keyUp(key);
            Variable_Internal.vmulti.updateKeyboard(report);
        }
    }
}
