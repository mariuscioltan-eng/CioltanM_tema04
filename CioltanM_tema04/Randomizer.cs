using System;
using System.Drawing;

namespace CioltanM_tema04
{
    class Randomizer
    {
        private Random rnd;

        public Randomizer()
        {
            rnd = new Random();
        }

        public Color RandomColor()
        {
            return Color.FromArgb(
                rnd.Next(1, 256),
                rnd.Next(1, 256),
                rnd.Next(1, 256)
            );
        }

        public int RandomInt(int minVal, int maxVal)
        {
            return rnd.Next(minVal, maxVal);
        }

        public float RandomFloat(float minVal, float maxVal)
        {
            return (float)(minVal + rnd.NextDouble() * (maxVal - minVal));
        }

        public bool RandomBool()
        {
            return rnd.Next(0, 2) == 0;
        }
    }
}
