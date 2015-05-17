using System;
using System.Drawing;

namespace CaptchaGenerator
{
    public class Filter
    {
        public enum Level
        {
            Low,
            Mid,
            High
        }

        public delegate Bitmap _FilterFunction(Bitmap ImageToFilter);
    }
}
