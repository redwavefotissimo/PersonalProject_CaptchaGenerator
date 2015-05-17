using System;
using System.Drawing;
using System.Collections.Generic;
using MindHelper;

namespace CaptchaGenerator
{
    public class Canvas
    {
        public string selectedfontfamily;
        private const int _MinCharacters = 3,
                          _MaxCharacters = 15;
        private int _TotalCharacters = _MinCharacters,
                    _CaptchaHeight = 90;
        private string _Data = string.Empty;
        private float _TotalCharacterWidth = 0f,
                      _FixedCharacterWidth = 0f;
        private bool _Clear;
        private CharacterGenerator _CharacterGenerator;
        private CharacterPainter _CharacterPainter;
        private Bitmap _CaptchaImage;

        #region LOAD

        /// <summary>
        /// Class Constructor:
        /// </summary>
        public Canvas()
        {
            Initialize();
        }

        /// <summary>
        /// Class Constructor:
        /// </summary>
        /// <param name="Width">Resize Width for the Generated Captcha Image.</param>
        /// <param name="Height">Resize Height for the Generated Captcha Image.</param>
        public Canvas(int Width, int Height)
        {
            //TODO: image resize property
            Initialize();
        }

        /// <summary>
        /// Instantiates Objects.
        /// </summary>
        public void Initialize()
        {
            _CharacterGenerator = new CharacterGenerator();
            _CharacterPainter = new CharacterPainter();
        }
        
        /// <summary>
        /// Sets/Resets all settings.
        /// </summary>
        private void Load()
        {
            _Data = string.Empty;
            GetTotalCharacterToProduce();
            _Clear = true;
            _CharacterGenerator.New();
            _CharacterPainter.New();
            _CaptchaImage = new Bitmap(_CaptchaHeight * _TotalCharacters, _CaptchaHeight);
        }

        #endregion

        #region DRAW AREA

        /// <summary>
        /// Generates the Captcha Image in Bitmap datatype.
        /// </summary>
        /// <returns>Captcha Image.</returns>
        public Bitmap Generate()
        {
            Load();
            for (int i = 0; i < _TotalCharacters; i++)
            {
                Draw(_CharacterGenerator.Next(), i);
            }
            return _CaptchaImage;
        }

        /// <summary>
        /// Draws the Character on the Canvas with the specified position.
        /// </summary>
        /// <param name="Character">string Character.</param>
        /// <param name="Position">Drawing Position.</param>
        private void Draw(string Character, int Position)
        {
            _Data += Character;
            Bitmap PaintedCharacter = _CharacterPainter.Paint(Character); // convert string character to an imaged character
            
            PaintedCharacter = Filters.ApplyFilter(PaintedCharacter, true); // apply filter
            
            _TotalCharacterWidth += _CharacterPainter.GetCharacterWidth(); // For Croppping purposes to get true width.
            
            // Actual drawing process
            Graphics Graphics = Graphics.FromImage(_CaptchaImage);
            ClearOnce(Graphics);
            SetFixedCharacterWidth(_CharacterPainter.GetCharacterWidth());           
            Graphics.DrawImage((Image)PaintedCharacter, new Point() { X = (Position * (int)_FixedCharacterWidth), Y = 0 });
            Graphics.Flush();
            
            selectedfontfamily = _CharacterPainter.GetFontNameUsed(); // gets the font name used

            SaveImage(PaintedCharacter); // saves the image to local, for testing purposes
            
            _CharacterPainter.Release();
            Graphics.Dispose();
        }

        /// <summary>
        /// Clear and apply a new background color.
        /// </summary>
        /// <param name="Graphics">Graphics to be cleared.</param>
        /// <returns>Graphics with the new background color.</returns>
        private Graphics ClearOnce(Graphics Graphics)
        {
            if (_Clear)
            {
                Graphics.Clear(_CharacterPainter.GetRandomColor());
                _Clear = false;
            }
            return Graphics;
        }

        #endregion

        #region UTILITY

        /// <summary>
        /// Checks if the input data is equal to the data generated.
        /// </summary>
        /// <param name="obj">string data to be compated.</param>
        /// <returns>True if input data is equal to generated data, otherwise False.</returns>
        public override bool Equals(object obj)
        {
            return (string)obj == _Data;
        }

        /// <summary>
        /// Sets the Fixed Character Width to all other character for positioning
        /// </summary>
        /// <param name="CharacterWidth">Character Width.</param>
        private void SetFixedCharacterWidth(float CharacterWidth)
        {
            if (_FixedCharacterWidth < 1f)
            {
                _FixedCharacterWidth = CharacterWidth;
            }
        }

        /// <summary>
        /// Saves the image.
        /// </summary>
        /// <param name="Image">Image to be saved.</param>
        /// <param name="FileName">Image File name.</param>
        internal void SaveImage(Bitmap Image, string FileName = "test")
        {
            Image.Save(string.Format(@"L:\sharedfolder\{0}.png", FileName), System.Drawing.Imaging.ImageFormat.Png);
        }

        /// <summary>
        /// Gets the number of character to be drawn in the canvas.
        /// </summary>
        internal void GetTotalCharacterToProduce()
        {
            _TotalCharacters = Helpers.GetRandomNumber(_MinCharacters, _MaxCharacters);
        }

        #endregion
    }
}
