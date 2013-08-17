using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace AeroAlert
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Help();
                return;
            }

            int speed;
            int.TryParse(args[0], out speed);
            speed = Math.Max(speed, 1);

            bool reset = !args.Contains("-k");
            Color resetColor = aero.GetColor();

            foreach (string a in args)
            {
                try
                {
                    Color c = Color.Empty;
                    if (a.StartsWith("#"))
                    {
                        c = System.Drawing.ColorTranslator.FromHtml(a);
                    }
                    else if(a.Length > 2)
                    {
                        c = Color.FromName(a);
                    }
                    if (c != Color.Empty) aero.ChangeColor(c, speed);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
            }
            if (reset) aero.ChangeColor(resetColor, speed);
        }

        static void Help()
        {
            string str = "";
            str += "\n" + ("AeroAlert - Dave Amenta - http://daveamenta.com");
            str += "\n" + ("");
            str += "\n" + ("-- Usage --");
            str += "\n" + ("AeroAlert.exe <Step#> [-k] <Color1, [Color2], [Color3], ..., [ColorN]>");
            str += "\n" + ("");
            str += "\n" + ("Change the DWM Aero Color to the specified color(s), in the specified number of transform increments.");
            str += "\n" + ("");
            str += "\n" + ("-- Options --");
            str += "\n" + ("Step#    Number of steps to complete the color transform.");
            str += "\n" + ("-k       Persistent.  Do not set the previous color before exit.");
            str += "\n" + ("");
            str += "\n" + ("-- Color Format --");
            str += "\n" + ("Known Color:     Blue, Green, Red, etc. (.NET System.Drawing.Color)");
            str += "\n" + ("RGB Hex Color:   #RRGGBB");
            str += "\n" + ("");
            str += "\n" + ("-- Examples --");
            str += "\n" + ("AeroAlert 100 Blue #ff00ee");
            str += "\n" + ("     Change the color to Blue in 100 steps, then #ff00ee in 100 steps, then return to the user-defined color over 100 steps.\n");
            str += "\n" + ("AeroAlert 1 -k Red");
            str += "\n" + ("     Change the aero color to Red immediately.");
            System.Windows.Forms.MessageBox.Show(str, "AeroAlert Help", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}
