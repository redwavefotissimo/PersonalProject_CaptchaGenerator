using System;
using System.Collections.Generic;
using System.Linq;
using MindHelper;

namespace CaptchaGenerator
{
    /// <summary>
    /// Character Generator: Generates random Character base on Character Weight.
    /// </summary>
    internal class CharacterGenerator
    {
        List<string> _NumberList,
                     _LetterList,
                     _SymbolList;
        List<Character> _CharacterList;
        int _MaxWeigtLeft,
            _StartingWeight = 20;

        #region LOAD

        /// <summary>
        /// Class Constructor:
        /// </summary>
        public CharacterGenerator()
        {
            _CharacterList = new List<Character>();
            SetUpNumberList();
            SetUpLetterList();
            SetUpSymbolList();
            SetUpMaxWeightLeft();
        }

        /// <summary>
        /// Sets/Resets all settings.
        /// </summary>
        public void New()
        {
            _MaxWeigtLeft = 0;
        }

        /// <summary>
        /// Setup Number List.
        /// </summary>
        private void SetUpNumberList()
        {
            _NumberList = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            SetUpNumberWeight();
        }

        /// <summary>
        /// Setup Letter List.
        /// </summary>
        private void SetUpLetterList()
        {
            _LetterList = new List<string>() 
            { 
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
            };
            SetUpLetterWeight();
        }

        /// <summary>
        /// Setup Symbol List.
        /// </summary>
        private void SetUpSymbolList()
        {
            _SymbolList = new List<string>() { "@", "#", "$", "%", "*", "/", "\\", "(", ")", "\"", "\'", "~", "!", "?", ":", ";", "<", ">", "&", "{", "}", "+" };
            SetUpSymbolWeight();
        }

        /// <summary>
        /// Setup Number Weight.
        /// </summary>
        private void SetUpNumberWeight()
        {
            for (int i = 0; i < _NumberList.Count; i++)
            {
                Character Character = new Character();
                Character.Value = _NumberList[i];
                Character.MinWeight = 0;
                Character.MaxWeight = (i + 1) * _StartingWeight;
                _CharacterList.Add(Character);
            }

            // setup the new starting weight
            _StartingWeight = SetUpNewStartingWeight();
        }

        /// <summary>
        /// Setup Letter Weight.
        /// </summary>
        private void SetUpLetterWeight()
        {
            for (int i = 0; i < _LetterList.Count; i++)
            {
                Character Character = new Character();
                Character.Value = _LetterList[i];
                Character.MinWeight = GetMinWeight(i);
                Character.MaxWeight = GetMaxWeight(i);
                _CharacterList.Add(Character);
            }
            // setup the new starting weight
            _StartingWeight = SetUpNewStartingWeight();
        }

        /// <summary>
        /// Setup Symbol Weight.
        /// </summary>
        private void SetUpSymbolWeight()
        {
            for (int i = 0; i < _SymbolList.Count; i++)
            {
                Character Character = new Character();
                Character.Value = _SymbolList[i];
                Character.MinWeight = GetMinWeight(i);
                Character.MaxWeight = GetMaxWeight(i);
                _CharacterList.Add(Character);
            }
        }

        /// <summary>
        /// Calculate the MinWeight.
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>Calculated MinWeight.</returns>
        private int GetMinWeight(int i)
        {
            return (i + 1) * (_StartingWeight / 3);
        }

        /// <summary>
        /// Calculate the MaxWeight.
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>Calculated MaxWeight.</returns>
        private int GetMaxWeight(int i)
        {
            return (i + 1) * (_StartingWeight / 2);
        }

        /// <summary>
        /// Setup New Starting Weight for next calculation.
        /// </summary>
        /// <returns></returns>
        private int SetUpNewStartingWeight()
        {
            return _CharacterList.Last().MaxWeight;
        }

        #endregion

        #region PROCESS

        /// <summary>
        /// Gets a randomized Character based on its weight.
        /// </summary>
        /// <returns>Randomized Character.</returns>
        public string Next()
        {
            int RandomizedWeight = Helpers.GetRandomNumber(0, _MaxWeigtLeft);
            List<string> CharactersWithInRange = GetAllCharactersWithinRange(RandomizedWeight);
            SetUpMaxWeightLeft(RandomizedWeight);
            return CharactersWithInRange[Helpers.GetRandomNumber(CharactersWithInRange.Count)];
        }
        
        /// <summary>
        /// Gets all the character within the min and max weight ranged from the given weight.
        /// </summary>
        /// <param name="RandomizedWeigth">Weight number to be compared to.</param>
        /// <returns>List of Characters within the Ranged.</returns>
        private List<string> GetAllCharactersWithinRange(int RandomizedWeigth)
        {
            return _CharacterList.Where(CL => CL.IsWithinRange(RandomizedWeigth)).Select(CL => CL.Value).ToList();
        }

        /// <summary>
        /// Calculates Weight left until characters can reparticipate in the random selection.
        /// </summary>
        /// <param name="RandomizedWeight">Weight number.</param>
        private void SetUpMaxWeightLeft(int RandomizedWeight = 0)
        {
            if (_MaxWeigtLeft <= 0)
            {
                _MaxWeigtLeft = DetermineToIncludeSymbolCharacters();
            }
            else
            {
                _MaxWeigtLeft -= RandomizedWeight;
            }
        }

        /// <summary>
        /// Determine whether to include Symbol Characters or not.
        /// </summary>
        /// <returns>returns the maximum number of items from the character list to be included in the random selection logic.</returns>
        private int DetermineToIncludeSymbolCharacters()
        {
            if (Helpers.GetRandomNumber(_CharacterList.First().MaxWeight) % 2 == 0) // if a number is even do not include symbols
            {
                return _CharacterList[(_NumberList.Count + _LetterList.Count) - 2].MaxWeight;
            }
            else
            {
                return _CharacterList.Last().MaxWeight;
            }
        }

        #endregion        
    }
}
