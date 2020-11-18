using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* These are datatypes that are used by the program for various paramters needed to interact with the Hue lights */

namespace hueController
{
    /* Used for storing the Red/Green/Blue values */
    public struct RGB
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public RGB(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }

    /* Used for storing the XY values */
    public struct XY
    {
        public float X { get; set; }
        public float Y { get; set; }

        public XY(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    /* Used for storing just the light name and id */
    public struct SimpleLight
    {
        public string Name { get; set; }
        public string ID { get; set; }

        public SimpleLight(string n, string i)
        {
            Name = n;
            ID = i;
        }
    }

    /* Used for storing just the group anme and id */
    public struct SimpleGroup
    {
        public string Name { get; set; }
        public string ID { get; set; }

        public SimpleGroup(string n, string i)
        {
            Name = n;
            ID = i;
        }
    }

    /* Used for setting light colors */
    class RGBColors
    {
        /* Current built in choices for colors, will be used for the voice recognition */
        public string[] ColorChoices = new string[] { "Red", "Blue", "Yellow", "Purple", "Green",
                                                      "Orange", "LimeGreen", "Teal", "Pink", "White",
                                                      "DullWhite" };
        /* Primary Colors */
        public RGB Red = new RGB(255, 0, 0);
        public RGB Blue = new RGB(0, 0, 255);
        public RGB Yellow = new RGB(255, 255, 0);

        /* Other Colors */
        public RGB Purple = new RGB(128, 0, 255);
        public RGB Green = new RGB(0, 255, 0);
        public RGB Orange = new RGB(255, 128, 0);
        public RGB LimeGreen = new RGB(128, 255, 0);
        public RGB Teal = new RGB(0, 128, 255);
        public RGB Pink = new RGB(255, 0, 255);

        /* White */
        public RGB White = new RGB(255, 255, 255);
        public RGB DullWhite = new RGB(128, 128, 128);

        /* Return the color based on the string name given */
        public RGB returnColor(string colorname)
        {
            switch (colorname)
            {
                case "Red":
                    return Red;
                case "Blue":
                    return Blue;
                case "Yellow":
                    return Yellow;
                case "Purple":
                    return Purple;
                case "Green":
                    return Green;
                case "Orange":
                    return Orange;
                case "LimeGreen":
                    return LimeGreen;
                case "Teal":
                    return Teal;
                case "Pink":
                    return Pink;
                case "White":
                    return White;
                case "DullWhite":
                    return DullWhite;
                default:
                    return White;
            }
        }

    }
}
