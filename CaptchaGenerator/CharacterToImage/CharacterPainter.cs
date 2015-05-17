using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using MindHelper;
using MindHelper.Reflection;

namespace CaptchaGenerator
{
    internal class CharacterPainter
    {
        private int _CharacterCanvasWidth = 55,
                    _CharacterCanvasHeight = 55;
        private string _CharacterToDraw,
                       _FontFamily,
                       selectedfontfamily;
        private bool _UseDarkBackground = false;
        private List<string> _DarkColorList,
                             _LightColorList;
        private Bitmap _CharacterCanvas;
        private Graphics _Graphics;
        private Font _Font;
        private ClassReflector _BrushClass;

        #region LOAD

        /// <summary>
        /// Class Constructor:
        /// </summary>
        public CharacterPainter()
        {
            _CharacterCanvas = new Bitmap(_CharacterCanvasWidth, _CharacterCanvasHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            _BrushClass = new ClassReflector(typeof(Brushes));
            New();
            SetUpDarkColor();
            SetUpLightColor();
        }

        /// <summary>
        /// Sets/Resets all Settings.
        /// </summary>
        public void New()
        {
            _FontFamily = GetRandomizeFontFamily();
            _Font = new Font(_FontFamily, GetSize());
            DetermineDark_LightUsageForBackground();
        }

        /// <summary>
        /// Setup the Dark Color List.
        /// </summary>
        private void SetUpDarkColor()
        {
            _DarkColorList = new List<string>()
                {
                    Color.Black.Name,
                    Color.Blue.Name,
                    Color.BlueViolet.Name,
                    Color.Brown.Name,
                    Color.Crimson.Name,
                    Color.DarkBlue.Name,
                    Color.DarkCyan.Name,
                    Color.DarkGoldenrod.Name,
                    Color.DarkGreen.Name,
                    Color.DarkMagenta.Name,
                    Color.DarkOliveGreen.Name,
                    Color.DarkOrchid.Name,
                    Color.DarkRed.Name,
                    Color.DarkSlateBlue.Name,
                    Color.DarkSlateGray.Name,
                    Color.DarkViolet.Name,
                    Color.Firebrick.Name,
                    Color.ForestGreen.Name,
                    Color.Fuchsia.Name,
                    Color.Green.Name,
                    Color.Indigo.Name,
                    Color.LightSeaGreen.Name,
                    Color.Magenta.Name,
                    Color.Maroon.Name,
                    Color.MediumBlue.Name,
                    Color.MediumVioletRed.Name,
                    Color.MidnightBlue.Name,
                    Color.Navy.Name,
                    Color.Olive.Name,
                    Color.OliveDrab.Name,
                    Color.OrangeRed.Name,
                    Color.Purple.Name,
                    Color.Red.Name,
                    Color.SaddleBrown.Name,
                    Color.SeaGreen.Name,
                    Color.Sienna.Name,
                    Color.Teal.Name
                };
        }

        /// <summary>
        /// Setup the Light Color List.
        /// </summary>
        private void SetUpLightColor()
        {
            _LightColorList = new List<string>()
                {
                    Color.AliceBlue.Name,
                    Color.AntiqueWhite.Name,
                    Color.Aqua.Name,
                    Color.Aquamarine.Name,
                    Color.Azure.Name,
                    Color.Beige.Name,
                    Color.Bisque.Name,
                    Color.BlanchedAlmond.Name,
                    Color.BurlyWood.Name,
                    Color.CadetBlue.Name,
                    Color.Chartreuse.Name,
                    Color.Chocolate.Name,
                    Color.Coral.Name,
                    Color.CornflowerBlue.Name,
                    Color.Cornsilk.Name,
                    Color.Cyan.Name,
                    Color.DarkGray.Name,
                    Color.DarkKhaki.Name,
                    Color.DarkOrange.Name,
                    Color.DarkSalmon.Name,
                    Color.DarkSeaGreen.Name,
                    Color.DarkTurquoise.Name,
                    Color.DeepPink.Name,
                    Color.DeepSkyBlue.Name,
                    Color.DimGray.Name,
                    Color.DodgerBlue.Name,
                    Color.FloralWhite.Name,                    
                    Color.Gainsboro.Name,
                    Color.GhostWhite.Name,
                    Color.Gold.Name,
                    Color.Goldenrod.Name,
                    Color.Gray.Name,
                    Color.GreenYellow.Name,
                    Color.Honeydew.Name,
                    Color.HotPink.Name,
                    Color.IndianRed.Name,
                    Color.Ivory.Name,
                    Color.Khaki.Name,
                    Color.Lavender.Name,
                    Color.LavenderBlush.Name,
                    Color.LawnGreen.Name,
                    Color.LemonChiffon.Name,
                    Color.LightBlue.Name,
                    Color.LightCoral.Name,
                    Color.LightCyan.Name,
                    Color.LightGoldenrodYellow.Name,
                    Color.LightGray.Name,
                    Color.LightGreen.Name,
                    Color.LightPink.Name,
                    Color.LightSalmon.Name,
                    Color.LightSkyBlue.Name,
                    Color.LightSlateGray.Name,
                    Color.LightSteelBlue.Name,
                    Color.LightYellow.Name,
                    Color.Lime.Name,
                    Color.LimeGreen.Name,
                    Color.Linen.Name,
                    Color.MediumAquamarine.Name,
                    Color.MediumOrchid.Name,
                    Color.MediumPurple.Name,
                    Color.MediumSeaGreen.Name,
                    Color.MediumSlateBlue.Name,
                    Color.MediumSpringGreen.Name,
                    Color.MediumTurquoise.Name,
                    Color.MintCream.Name,
                    Color.MistyRose.Name,
                    Color.Moccasin.Name,
                    Color.NavajoWhite.Name,
                    Color.OldLace.Name,
                    Color.Orange.Name,
                    Color.Orchid.Name,
                    Color.PaleGoldenrod.Name,
                    Color.PaleGreen.Name,
                    Color.PaleTurquoise.Name,
                    Color.PaleVioletRed.Name,
                    Color.PapayaWhip.Name,
                    Color.PeachPuff.Name,
                    Color.Peru.Name,
                    Color.Pink.Name,
                    Color.Plum.Name,
                    Color.PowderBlue.Name,
                    Color.RosyBrown.Name,
                    Color.RoyalBlue.Name,
                    Color.Salmon.Name,
                    Color.SandyBrown.Name,
                    Color.SeaShell.Name,
                    Color.Silver.Name,
                    Color.SkyBlue.Name,
                    Color.SlateBlue.Name,
                    Color.SlateGray.Name,
                    Color.Snow.Name,
                    Color.SpringGreen.Name,
                    Color.SteelBlue.Name,
                    Color.Tan.Name,
                    Color.Thistle.Name,
                    Color.Tomato.Name,
                    Color.Turquoise.Name,
                    Color.Violet.Name,
                    Color.Wheat.Name,
                    Color.White.Name,
                    Color.WhiteSmoke.Name,
                    Color.Yellow.Name,
                    Color.YellowGreen.Name
                };
        }

        #endregion

        #region PROCESS

        /// <summary>
        /// Draw the character to a Bitmap object.
        /// </summary>
        /// <param name="Character">Character to be drawn.</param>
        /// <returns>Bitmap object with the drawn Character.</returns>
        public Bitmap Paint(string Character)
        {
            _CharacterToDraw = Character;
            _Graphics = Graphics.FromImage(_CharacterCanvas);
            _Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            _Graphics.Clear(Color.Transparent);
            _Graphics.DrawString(Character, _Font, GetRandomBrushColor(), new PointF() { X = 0, Y = 0 });
            _Graphics.Flush();
            return _CharacterCanvas;
        }

        /// <summary>
        /// Disposes the Graphics object after the process.
        /// </summary>
        public void Release()
        {
            _Graphics.Dispose(); // dispose _graphics
        }

        #endregion

        #region UTILITY

        /// <summary>
        /// Gets the Font name used.
        /// </summary>
        /// <returns>Font Name.</returns>
        public string GetFontNameUsed()
        {
            return selectedfontfamily;
        }

        /// <summary>
        /// Get Character Width according to the Font used.
        /// </summary>
        /// <returns>Character Width</returns>
        public float GetCharacterWidth()
        {
            return _Graphics.MeasureString(_CharacterToDraw, _Font).Width;
        }

        /// <summary>
        /// Gets a random Color.
        /// </summary>
        /// <returns>Random Color.</returns>
        public Color GetRandomColor()
        {
           return  Color.FromName(GetRandomColorName(ColorListToUse(_UseDarkBackground)));
        }

        /// <summary>
        /// Gets a random Brush Color.
        /// </summary>
        /// <returns>Random Brush Color.</returns>
        private Brush GetRandomBrushColor()
        {
            return (Brush)_BrushClass.GetPropertyData(GetRandomColorName(ColorListToUse(!_UseDarkBackground)));
        }

        /// <summary>
        /// Calculate Font Size using the given character canvas height.
        /// </summary>
        /// <returns>Font Size.</returns>
        private float GetSize()
        {
            return _CharacterCanvasHeight * (72F / 96F);
        }

        /// <summary>
        /// Get a Random Font Family to be used.
        /// </summary>
        /// <returns>Random Font Family/Name.</returns>
        private string GetRandomizeFontFamily()
        {
            string[] FontFamilyList = FontFamily.Families.Where(FF => FF.IsStyleAvailable(FontStyle.Regular) && IsNotInFontList(FF.Name)).Select(FF => FF.Name).ToArray();
            selectedfontfamily = FontFamilyList[Helpers.GetRandomNumber(FontFamilyList.Length)];
            return selectedfontfamily; 
        }

        /// <summary>
        /// Get a Random Color Name.
        /// </summary>
        /// <param name="ColorListToRandom"></param>
        /// <returns></returns>
        private string GetRandomColorName(List<string> ColorListToRandom)
        {
            return ColorListToRandom[Helpers.GetRandomNumber(ColorListToRandom.Count)];
        }

        /// <summary>
        /// Determine whether to use dark background or light background.
        /// </summary>
        private void DetermineDark_LightUsageForBackground()
        {
            _UseDarkBackground = Helpers.GetRandomNumber() % 2 == 0;
        }

        /// <summary>
        /// Gets the color List to be used according to the _UseDarkBackground boolean variable.
        /// </summary>
        /// <param name="UseDarkBackground">whether to use dark background or not.</param>
        /// <returns>Color List to be used.</returns>
        private List<string> ColorListToUse(bool UseDarkBackground)
        {
            return UseDarkBackground ? _DarkColorList : _LightColorList; 
        }

        /// <summary>
        /// Checks if the Font Name specified is in the restriction list.
        /// </summary>
        /// <param name="FontName">Font Name to check.</param>
        /// <returns>True if not in this list, otherwise False.</returns>
        private bool IsNotInFontList(string FontName)
        {
            string NotAllowedFonts = "Lucida Calligraphy,Pericles,Webdings,Bookshelf Symbol 7,Nyala,Malgun Gothic,Moire Light,Trebuchet MS," +
                                     "DilleniaUPC,Mangal,Cordia New,Niagara Engraved,Verdana,Angsana New,Marlett,Rod,Century,Lucida Handwriting," +
                                     "Meiryo UI,Shruti,Lindsey,MS Reference Specialty,Lucida Bright,Cambria Math,Tempus Sans ITC,Euphemia,Broadway," +
                                     "DokChampa,Century Gothic,Angsana New,Arial,Kootenay,Kunstler Script,Onyx,Sakkal Majalla,Kristen ITC,Vrinda," +
                                     "Raavi,MV Boli,Cambria,Informal Roman,Arial Unicode MS,Constantia,Lucida Fax,Garamond,Times New Roman,Gisha," +
                                     "Comic Sans MS,Meiryo,Poor Richard,Vladimir Script,Georgia,Simplified Arabic,Plantagenet Cherokee,Narkisim," +
                                     "Lucida Sans Unicode,Moire ExtraBold,MS Reference Sans Serif,Ebrima,Wasco Sans,Arabic Typesetting,Gulim," +
                                     "Franklin Gothic Medium,Tunga,AngsanaUPC,Quartz MS,Estrangelo Edessa,Latha,OCR A Extended,Jing Jing,Vijaya," +
                                     "Ravie,Chiller,MoolBoran,Calibri,Britannic Bold,Symbol,Book Antiqua,Kartika,MapInfo Cartographic,Andalus," +
                                     "Gabriola,Berlin Sans FB,Miramonte,Bookman Old Style,Arial Black,Pericles Light,MS Outlook,DaunPenh,Corbel," +
                                     "Tahoma,BrowalliaUPC,Courier New,Browallia New,Juice ITC,Impact,Vani,Palatino Linotype,Traditional Arabic," +
                                     "Leelawadee,Mistral,Jokerman,Courier New,Candara,Lao UI,Californian FB,Moire,Wide Latin,Iskoola Pota,Harrington," +
                                     "Bauhaus 93,Centaur,CordiaUPC,Map Symbols,MT Extra,Baskerville Old Face,Sylfaen,Cooper Black,Haettenschweiler," +
                                     "News Gothic,Khmer UI,Freestyle Script,Playbill,Snap ITC,Kalinga,Consolas,Gautami,Niagara Solid,Kokila,Miriam," +
                                     "Viner Hand ITC,Gisha,Dotum,Arial Narrow,Pescadero,High Tower Text,Mongolian Baiti,Modern No. 20,";

            return !(FontName.Contains(" BT") || FontName.Contains(" MT") || FontName.Contains(" SF") || FontName.Contains("Segoe") || FontName.Contains("Microsoft") || FontName.Contains("Wingdings") || NotAllowedFonts.Contains(FontName + ","));
        }

        #endregion
    }
}
