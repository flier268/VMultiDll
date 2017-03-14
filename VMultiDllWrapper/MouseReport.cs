using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMultiDllWrapper
{
    internal class MouseReport
    {
        /*
         
#define MOUSE_BUTTON_1     0x01
#define MOUSE_BUTTON_2     0x02
#define MOUSE_BUTTON_3     0x04

#define MOUSE_MIN_COORDINATE   0x0000
#define MOUSE_MAX_COORDINATE   0x7FFF

#define MIN_WHEEL_POS   -127
#define MAX_WHEEL_POS    127*/
        HashSet<Button> _buttons;
        private ushort x = 0, y = 0;
        private byte _wheelPositio=0;
        public enum Button : byte
        {
            MOUSE_BUTTON_1 = 1,
            MOUSE_BUTTON_2 = 2,
            MOUSE_BUTTON_3 = 4
        }
        public MouseReport()
        {
            _buttons = new HashSet<Button>();
        }
        
        public ushort X { get => x; set => x = value; }
        public ushort Y { get => y; set => y = value; }
        public byte wheelPosition { get => _wheelPositio; set => _wheelPositio = value; }
        public HashSet<Button> Buttons { get => _buttons; set => _buttons = value; }

        public void KeyDown(Button btn)
        {
            _buttons.Add(btn);
        }
        public void KeyUp(Button btn)
        {
            _buttons.Remove(btn);
        }
        public byte getButton()
        {
            byte temp = 0;
            foreach (Button modifier in _buttons)
            {
                temp += (byte)modifier;
            }
            return temp;            
        }
    }
}
