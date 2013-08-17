using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AeroAlert
{
    public class aero
    {
        [StructLayout(LayoutKind.Sequential)]
        struct DwmColorParams
        {
            public uint color;
            public uint param2;
            public uint param3;
            public uint param4;
            public uint param5;
            public uint param6;
            public bool useTransparency;
        };
        [DllImport("dwmapi.dll", EntryPoint = "#131")]
        static extern int DwmpSetColorizationParameters(ref DwmColorParams dcpParams, bool alwaysTrue);
        [DllImport("dwmapi.dll", EntryPoint = "#127")]
        static extern int DwmpGetColorizationParameters(out DwmColorParams dcpParams);

        public static Color GetColor()
        {
            DwmColorParams p = new DwmColorParams();
            DwmpGetColorizationParameters(out p);
            return Color.FromArgb((int)p.color);
        }

        public static void ChangeColor(Color goalColor, int duration)
        {
            DwmColorParams p = new DwmColorParams();
            DwmpGetColorizationParameters(out p);
            Color startColor = Color.FromArgb((int)p.color);
            if (goalColor == startColor) return;
            CChanger ct = new CChanger(startColor, goalColor, duration);
            while (ct.Transform())
            {
                Color intColor = ct.GetColor();
                p.color = (uint)intColor.ToArgb();
                p.param2 = p.color;

                DwmpSetColorizationParameters(ref p, true);
                Application.DoEvents();
                Thread.Sleep(33);
            }
        }
    }

    class CChanger
    {
        public int currentStep;
        private Color goal;
        private Color start;
        public int steps;

        public CChanger(Color start, Color goal, int steps)
        {
            this.start = start;
            this.goal = goal;
            this.steps = steps;
        }

        public Color GetColor()
        {
            int red = this.start.R + ((this.currentStep * (this.goal.R - this.start.R)) / this.steps);
            int green = this.start.G + ((this.currentStep * (this.goal.G - this.start.G)) / this.steps);
            int blue = this.start.B + ((this.currentStep * (this.goal.B - this.start.B)) / this.steps);
            int alpha = this.start.A + ((this.currentStep * (this.goal.A - this.start.A)) / this.steps);
            return Color.FromArgb(alpha, red, green, blue);
        }

        public bool Transform()
        {
            if (this.currentStep < this.steps)
            {
                this.currentStep++;
                return true;
            }
            return false;
        }
    }
}
