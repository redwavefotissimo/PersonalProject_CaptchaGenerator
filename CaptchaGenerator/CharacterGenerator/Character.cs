using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptchaGenerator
{
    internal class Character
    {
        public string Value { get; set; }

        public int MinWeight { get; set; }

        public int MaxWeight { get; set; }

        /// <summary>
        /// Checks if the given weight is within Min and Max Weight set.
        /// </summary>
        /// <param name="Weight">weight number.</param>
        /// <returns>True if the weight number is within range, otherwise False.</returns>
        public bool IsWithinRange(int Weight)
        {
            return Weight >= MinWeight && Weight <= MaxWeight;
        }
    }
}
