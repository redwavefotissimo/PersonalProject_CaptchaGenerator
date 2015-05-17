using System;
using System.Drawing;
using CaptchaGenerator;

namespace CaptchaGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Canvas Captcha = new Canvas();
            Captcha.SaveImage(Captcha.Generate(), "Merged");
            
            string test = Captcha.selectedfontfamily;
        }
    }
}
