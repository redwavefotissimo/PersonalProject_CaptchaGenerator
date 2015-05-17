using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using MindHelper;
using MindHelper.Reflection;

namespace CaptchaGenerator
{
    public static class Filters
    {
        private static Dictionary<int, List<Filter._FilterFunction>> FilterFunctionList;
        
        #region LOAD

        /// <summary>
        /// Class Constructor:
        /// </summary>
        static Filters()
        {
            FilterFunctionList = new Dictionary<int, List<Filter._FilterFunction>>();
            InitFilterLevel();
        }

        /// <summary>
        /// Initializes objects.
        /// </summary>
        private static void InitFilterLevel()
        {
            ClassReflector  EnumReflector = new ClassReflector();
            foreach (KeyValuePair<string,int> item in EnumReflector.GetEnumMembers(typeof(Filter.Level)))
            {
                FilterFunctionList.Add(item.Value, new List<Filter._FilterFunction>());
                InitFiltersByLevel(item.Value);
                int TotalNoFilterFunctionToBeAdded = FilterFunctionList[item.Value].Count * 3;
                if (TotalNoFilterFunctionToBeAdded < 1)
                    TotalNoFilterFunctionToBeAdded = 1;
                for (int i = 0; i < TotalNoFilterFunctionToBeAdded; i++)
                    FilterFunctionList[item.Value].Add(NoFilter);
            }
        }

        /// <summary>
        /// Setup the Filters by Level.
        /// </summary>
        /// <param name="FilterLevel">Filter Level.</param>
        private static void InitFiltersByLevel(int FilterLevel)
        {
            switch (FilterLevel)
            {
                case 2:
                    InitLevel2Filter();
                    break;
                case 3:
                    InitLevel3Filter();
                    break;
                default:
                    InitLevel1Filter();
                    break;
            }
        }

        /// <summary>
        /// Add filter functions to Low level.
        /// </summary>
        private static void InitLevel1Filter()
        {
            FilterFunctionList[(int)Filter.Level.Low].Add(RotateLeft);
            FilterFunctionList[(int)Filter.Level.Low].Add(RotateRight);
        }

        /// <summary>
        /// Add filter functions to Mid level.
        /// </summary>
        private static void InitLevel2Filter()
        {
            FilterFunctionList[(int)Filter.Level.Mid].Add(TranslateDown);
        }

        /// <summary>
        /// Add filter functions to High level.
        /// </summary>
        private static void InitLevel3Filter()
        {
            FilterFunctionList[(int)Filter.Level.High].Add(Checkerize);
            FilterFunctionList[(int)Filter.Level.High].Add(VerticalStripeLikeFilter);
            FilterFunctionList[(int)Filter.Level.High].Add(DiagonalStripeLikeFilter);
            FilterFunctionList[(int)Filter.Level.High].Add(BlendLikeFilter);
            FilterFunctionList[(int)Filter.Level.High].Add(ScribbleLikeFilter);
        }

        /// <summary>
        /// Adds Custom Filter
        /// </summary>
        /// <param name="FilterLevel">Filter Level ranging from low, mid and high</param>
        /// <param name="FilterFunction">Filter Function to be added, where it accepts Bitmap ParamaterType and returns a Bitmap Datatype.</param>
        public static void AddCustomFilter(Filter.Level FilterLevel, Filter._FilterFunction FilterFunction)
        {
            FilterFunctionList[(int)FilterLevel].Add(FilterFunction);
        }

        #endregion

        #region PROCESS

        /// <summary>
        /// Applies 1 Random Filter from each Level.
        /// </summary>
        /// <param name="CharacterImage">Bitmap to be filtered.</param>
        /// <returns>Filtered Bitmap.</returns>
        public static Bitmap ApplyFilter(Bitmap CharacterImage)
        {
            foreach (KeyValuePair<int, List<Filter._FilterFunction>> item in FilterFunctionList)
            {
                if (item.Value.Count != 0)
                {
                    CharacterImage = item.Value[Helpers.GetRandomNumber(item.Value.Count)](CharacterImage);
                }
            }
            return CharacterImage;
        }

        /// <summary>
        /// Apply 1 specific filter. For Testing Purposes.
        /// </summary>
        /// <param name="CharacterImage">Bitmap to be filtered.</param>
        /// <param name="test">Indicates to use this testing function.</param>
        /// <returns>Filtered Bitmap.</returns>
        public static Bitmap ApplyFilter(Bitmap CharacterImage, bool test)
        {
            return Reverse(CharacterImage);
        }

        #endregion

        #region PRESET FILTERS

        /// <summary>
        /// No Filter.
        /// </summary>
        /// <param name="Bitmap">Bitmap to be Filtered.</param>
        /// <returns>Filtered Bitmap.</returns>
        private static Bitmap NoFilter(Bitmap Bitmap)
        {
            return Bitmap;
        }

        #region LEVEL 1 FILTERS

        /// <summary>
        /// Rotate the Bitmap image to Left.
        /// </summary>
        /// <param name="Bitmap">Bitmap to be Filtered.</param>
        /// <returns>Filtered Bitmap.</returns>
        private static Bitmap RotateLeft(Bitmap Bitmap)
        {
            Bitmap NewBitmap = new Bitmap(Bitmap.Width, Bitmap.Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(NewBitmap);
            graphics.TranslateTransform(Bitmap.Width / 2, Bitmap.Height / 2);
            graphics.RotateTransform((90 * 4) - Helpers.GetRandomNumber(20, 30));
            graphics.TranslateTransform(-Bitmap.Width / 2, -Bitmap.Height / 2);
            graphics.DrawImage(Bitmap, new Point() { X = 0, Y = 0 });
            graphics.Flush();
            graphics.Dispose();
            return NewBitmap;
        }

        /// <summary>
        /// Rotate the Bitmap image to Right.
        /// </summary>
        /// <param name="Bitmap">Bitmap to be Filtered.</param>
        /// <returns>Filtered Bitmap.</returns>
        private static Bitmap RotateRight(Bitmap Bitmap)
        {
            Bitmap NewBitmap = new Bitmap(Bitmap.Width, Bitmap.Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(NewBitmap);
            graphics.TranslateTransform(Bitmap.Width / 2, Bitmap.Height / 2);
            graphics.RotateTransform(Helpers.GetRandomNumber(20, 30));
            graphics.TranslateTransform(-Bitmap.Width / 2, -Bitmap.Height / 2);
            graphics.DrawImage(Bitmap, new Point() { X = 0, Y = 0 });
            graphics.Flush();
            graphics.Dispose();
            return NewBitmap;
        }

        #endregion

        #region LEVEL 2 FILTERS

        private static int MaxDownwardMovePoint = 15;

        /// <summary>
        /// Translate the Image Downward.
        /// </summary>
        /// <param name="Bitmap">Bitmap to be Filtered.</param>
        /// <returns>Filtered Bitmap.</returns>
        private static Bitmap TranslateDown(Bitmap Bitmap)
        {
            Bitmap NewBitmap = new Bitmap(Bitmap.Width, Bitmap.Height + MaxDownwardMovePoint, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(NewBitmap);
            graphics.TranslateTransform(0, Helpers.GetRandomNumber(0, MaxDownwardMovePoint));
            graphics.DrawImage(Bitmap, new Point() { X = 0, Y = 0 });
            graphics.Flush();
            graphics.Dispose();
            return NewBitmap;
        }

        #endregion

        #region LEVEL 3 FILTERS

        private delegate byte ShiftType(byte Byte);
        private static List<ShiftType> Shifts;
        private const int TotalBitForColor = 255;

        /// <summary>
        /// Modifies the Byte by shifting each set of byte determined by TotalSkip 
        /// </summary>
        /// <param name="Bitmap">Bitmap to be modified.</param>
        /// <param name="TotalSkip">Set of Bytes to have similar shift values.</param>
        /// <returns></returns>
        private static Bitmap ByteModifier(Bitmap Bitmap, int TotalSkip)
        {
            SetUpShifts();
            BitmapData Data = Bitmap.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr Ptr = Data.Scan0;
            byte[] ByteBuffer = new byte[Data.Stride * Data.Height];
            Marshal.Copy(Ptr, ByteBuffer, 0, ByteBuffer.Length);

            for (int k = 0; k < ByteBuffer.Length; k+=TotalSkip)
            {
                byte ShiftResult = Shifts[Helpers.GetRandomNumber(Shifts.Count)](ByteBuffer[k]);
                for (int L = k; L < k + TotalSkip; L++)
                {
                    if (L >= ByteBuffer.Length)
                    {
                        break;
                    }
                    ByteBuffer[L] = ShiftResult;
                }
            }
 
            Marshal.Copy(ByteBuffer, 0, Ptr, ByteBuffer.Length);
            Bitmap.UnlockBits(Data);
            Data = null;
            ByteBuffer = null;
            return Bitmap;
        }

        /// <summary>
        /// Checherize the bitmap image.
        /// </summary>
        /// <param name="Bitmap">Bitmap to be Filtered.</param>
        /// <returns>Filtered Bitmap.</returns>
        private static Bitmap Checkerize(Bitmap Bitmap)
        {
            return ByteModifier(Bitmap, 3);
        }

        /// <summary>
        /// Vertical Stripe the Bitmap image.
        /// </summary>
        /// <param name="Bitmap">Bitmap to be Filtered.</param>
        /// <returns>Filtered Bitmap.</returns>
        private static Bitmap VerticalStripeLikeFilter(Bitmap Bitmap)
        {
            return ByteModifier(Bitmap, 5);
        }

        /// <summary>
        /// Scribble the Bitmap image.
        /// </summary>
        /// <param name="Bitmap">Bitmap to be Filtered.</param>
        /// <returns>Filtered Bitmap.</returns>
        private static Bitmap ScribbleLikeFilter(Bitmap Bitmap)
        {
            return ByteModifier(Bitmap, 6);
        }

        /// <summary>
        /// Diagonal Stripe the Bitmap image.
        /// </summary>
        /// <param name="Bitmap">Bitmap to be Filtered.</param>
        /// <returns>Filtered Bitmap.</returns>
        private static Bitmap DiagonalStripeLikeFilter(Bitmap Bitmap)
        {
            return ByteModifier(Bitmap, 9);
        }

        /// <summary>
        /// Blend the Bitmap image.
        /// </summary>
        /// <param name="Bitmap">Bitmap to be Filtered.</param>
        /// <returns>Filtered Bitmap.</returns>
        private static Bitmap BlendLikeFilter(Bitmap Bitmap)
        {
            return ByteModifier(Bitmap, 19);
        }

        /// <summary>
        /// Sets up Shifts List.
        /// </summary>
        private static void SetUpShifts()
        {
            if (Shifts == null)
            {
                Shifts = new List<ShiftType>();
                Shifts.Add(LeftShift);
                Shifts.Add(RightShift);
            }
        }

        /// <summary>
        /// Left Shift the Byte.
        /// </summary>
        /// <param name="Byte">Byte to be calculated.</param>
        /// <returns>Calculated Byte.</returns>
        private static byte LeftShift(byte Byte)
        {
            int ShiftResult = Byte << Helpers.GetRandomNumber(Byte.ToString().Length);
            if (ShiftResult > TotalBitForColor)
                ShiftResult = TotalBitForColor;
            return Convert.ToByte(ShiftResult);
        }

        /// <summary>
        /// Right Shift the Byte.
        /// </summary>
        /// <param name="Byte">Byte to be calculated.</param>
        /// <returns>Calculated Byte.</returns>
        private static byte RightShift(byte Byte)
        {
            int ShiftResult = Byte << Helpers.GetRandomNumber(Byte.ToString().Length);
            if (ShiftResult > TotalBitForColor)
                ShiftResult = TotalBitForColor;
            return Convert.ToByte(ShiftResult);
        }

        
        private static Bitmap Reverse(Bitmap Bitmap)
        {
            return Bitmap;
        }

        #endregion

        #endregion
    }
}
