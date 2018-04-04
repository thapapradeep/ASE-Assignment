using System;
namespace Fractal
{
    internal class HSB
    {

        public float rChan, gChan, bChan;
        public int num1, num2, num3;
        public HSB()
        {
            rChan = gChan = bChan = 0;
        }
        public void fromHSB(float h, float s, float b, int c, int num1, int num2, int num3)
        {
            this.num1 = num1;
            this.num2 = num2;
            this.num3 = num3;
            float red = b;
            float green = b;
            float blue = b;
            if (s != 0)
            {
                
                float max = b;
                float dif = b * s / 255f;
                float min = b - dif;

                float h2 = h * 360f / 255f;

                if (h2 < 60f)
                {
                    red = max;
                    green = h2 * dif / 60f + min;
                    blue = min;
                }
                else if (h2 < 120f)
                {
                    red = -(h2 - 120f) * dif / 60f + min;
                    green = max;
                    blue = min;
                }
                else if (h2 < 180f)
                {
                    red = min;
                    green = max;
                    blue = (h2 - 120f) * dif / 60f + min;
                }
                else if (h2 < 240f)
                {
                    red = min;
                    green = -(h2 - 240f) * dif / 60f + min;
                    blue = max;
                }
                else if (h2 < 300f)
                {
                    red = (h2 - 240f) * dif / 60f + min;
                    green = min;
                    blue = max;
                }
                else if (h2 <= 360f)
                {
                    red = max;
                    green = min;
                    blue = -(h2 - 360f) * dif / 60 + min;
                }
                else
                {
                    red = 0;
                    green = 0;
                    blue = 0;
                }

            }
            if (c == 0)
            {
            rChan = (float)Math.Round(Math.Min(Math.Max(red, 0f), 255));
            gChan = (float)Math.Round(Math.Min(Math.Max(green, 0), 255));
            bChan = (float)Math.Round(Math.Min(Math.Max(blue, 0), 255));
             }
            else
            {
                
                rChan = (float)Math.Round(Math.Min(Math.Max(red, 0f), num1));
                gChan = (float)Math.Round(Math.Min(Math.Max(green, 0), num2));
                bChan = (float)Math.Round(Math.Min(Math.Max(blue, 0), num3));
                
           }

            }
     
    }
    }

