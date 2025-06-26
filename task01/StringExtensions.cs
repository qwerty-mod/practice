using System;
using System.Linq;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var filtr_reg_masssimv = input
                .Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c))
                .Select(char.ToLower)
                .ToArray();

            for (int i = 0; i < filtr_reg_masssimv.Length / 2; i++)
            {
                if (filtr_reg_masssimv[i] != filtr_reg_masssimv[filtr_reg_masssimv.Length - 1 - i])
                    return false;
            }
            return true;
        }
    }
}