using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VMultiDllWrapper;

namespace VMultiTest
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);
        static void Main(string[] args)
        {
            Mouse s = new Mouse();

            Console.WriteLine(s.Init());

            Console.WriteLine("Testing Mouse.");
            Thread.Sleep(3000);
            s.MouseMove(30, 500);
            Thread.Sleep(1000);
            s.MouseMoveRalative(500, 500);
            Thread.Sleep(20);
            //s.LeftDown();
            //Thread.Sleep(20);
            //s.LeftUp();
            //s.Wheel(3);

            KeyBoard keyboard = new KeyBoard();
            keyboard.Init();

            Console.WriteLine("Testing KeyBoard.");
            System.Diagnostics.Process.Start("notepad.exe");
            Thread.Sleep(3000);
            Console.WriteLine("Press A");
            keyboard.KeyDown(KeyboardKey.A);
            Thread.Sleep(20);
            keyboard.KeyUp(KeyboardKey.A);
            Thread.Sleep(20);


            Console.WriteLine("as same as this");
            keyboard.KeyPress(KeyboardKey.A);

            keyboard.KeyDown(KeyboardKey.Tab, KeyboardModifier.LAlt);
            Thread.Sleep(20);
            keyboard.KeyUp(KeyboardKey.Tab, KeyboardModifier.LAlt);
            Console.ReadKey();
        }

        [DllImport("USER32.dll")]
        static extern short GetKeyState(int nVirtKey);
      
        private void joystickTest(VMulti vmulti)
        {
            double i = 0;
            bool running = true;
            while (running)
            {
                JoystickButtonState joyButtonState = new JoystickButtonState();
                joyButtonState.A = false;
                joyButtonState.X = false;
                joyButtonState.Left = false;

                double x = Math.Sin(i);
                double y = Math.Cos(i);

                Console.WriteLine("x: " + x + " y: " + y);

                JoystickReport joystickReport = new JoystickReport(joyButtonState, x, y);

                Console.WriteLine("Update Joystick: " + vmulti.updateJoystick(joystickReport));

                i += 0.1;

                System.Threading.Thread.Sleep(100);
            }
        }

        private void multitouchTest(VMulti vmulti)
        {
            double x = 500;
            double y = 500;

            while (true)
            {
                List<MultitouchPointerInfo> touches = new List<MultitouchPointerInfo>();
                bool spacePressed = Convert.ToBoolean(GetKeyState(0x20) & 0x8000);
                MultitouchPointerInfo pointer = new MultitouchPointerInfo();

                bool rightPressed = Convert.ToBoolean(GetKeyState(0x27) & 0x8000);
                if (rightPressed)
                    x += 10;
                bool downPressed = Convert.ToBoolean(GetKeyState(0x28) & 0x8000);
                if (downPressed)
                    y += 10;
                bool leftPressed = Convert.ToBoolean(GetKeyState(0x25) & 0x8000);
                if (leftPressed)
                    x -= 10;
                bool upPressed = Convert.ToBoolean(GetKeyState(0x26) & 0x8000);
                if (upPressed)
                    y -= 10;
                
                if (spacePressed)
                {
                    Console.WriteLine("pressed");
                    pointer.Down = true;
                }
                else
                {
                    pointer.Down = false;
                }

                Point mousePos = Control.MousePosition;
                Console.WriteLine(mousePos);
                pointer.X = x / Screen.PrimaryScreen.Bounds.Width;
                pointer.Y = y / Screen.PrimaryScreen.Bounds.Height;

                Console.WriteLine("X: " + pointer.X);
                Console.WriteLine("Y: " + pointer.Y);

                touches.Add(pointer);

                MultitouchReport report = new MultitouchReport(touches);

                if (!vmulti.updateMultitouch(report))
                {
                    Console.WriteLine("fail");
                }

                System.Threading.Thread.Sleep(10);
            }
        }



    }
}
