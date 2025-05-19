using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using System.Drawing;
using System.Globalization;

namespace TechnologyOneNumberToWordsConverter
{
	public static class NumberToWordsConverter
	{
		private static readonly Dictionary<string, string> NumberWordRelation = new()
		{
			{ "0", "zero" }, { "1", "one" }, { "2", "two" }, { "3", "three" }, { "4", "four" },
			{ "5", "five" }, { "6", "six" }, { "7", "seven" },{ "8", "eight" }, { "9", "nine" },
			{ "10", "ten" }, { "11", "eleven" }, { "12", "twelve" }, { "13", "thirteen" }, { "14", "fourteen" },
			{ "15", "fifteen" }, { "16", "sixteen" }, { "17", "seventeen" }, { "18", "eighteen" }, { "19", "nineteen" },
			{ "20", "twenty" }, { "30", "thirty" },{ "40", "forty" }, { "50", "fifty" }, { "60", "sixty" },
			{ "70", "seventy" }, { "80", "eighty" }, { "90", "ninety" }
		};

		private static readonly Dictionary<int, string> ExponentWordRelation = new()
		{
			{1, "" }, { 2, "thousand" }, { 3, "million" }, { 4, "billion" }, { 5, "trillion" },
			{ 6, "quadrillion" }, { 7, "quintillion" }, { 8, "sextillion" }, { 9, "septillion" }, { 10, "octillion" },
			{ 11, "nonillion" }, { 12, "decillion" }
		};


		public static string Convert(string number)
		{
			// Split the number into whole and decimal parts
			string[] splitNumber = number.Split('.');

			// Check if it is a negative number and then remove the minus sign
			bool negativeNumber = splitNumber[0][0] == '-';
			if (negativeNumber)
				splitNumber[0] = splitNumber[0].Remove(0, 1);

			string result = "";

			// If number is bigger than 999 decillion, then we are going to simplify it significantly
			if (splitNumber[0].Length > 36)
			{
				result = (negativeNumber ? "Negative " : "") + $"{NumberWordRelation[splitNumber[0][0].ToString()]} ";
				if (splitNumber[0][1] != '0')
				{
					result += $"point {NumberWordRelation[splitNumber[0][1].ToString()]} ";
				}
				result += $"times ten to the power of {Convert((splitNumber[0].Length - 1).ToString())}";
				return result;
					
			}

			#region Handle Whole Number
			// Split the number into groups of three digits
			List<string> numberGroups = new();
			string currentGroup = "";
			for (int i = splitNumber[0].Length - 1; i >= 0; i--)
			{
				currentGroup = currentGroup.Insert(0, splitNumber[0][i].ToString());

				if (currentGroup.Length == 3)
				{
					numberGroups.Insert(0, currentGroup);
					currentGroup = "";
				}
			}
			// Insert the last group if it exists
			if (currentGroup != "")
			{
				numberGroups.Insert(0, currentGroup);
			}

			int exponent = numberGroups.Count;
			for (int i = 0; i < numberGroups.Count; i++)
			{
				// Skip if the group is 000
				if (numberGroups[i] == "000")
				{
					exponent--;
					continue; 
				}					

				// Add to result based on length of number group
				switch (numberGroups[i].Length)
				{
					case 1:
						result += $"{NumberWordRelation[numberGroups[i]]} {ExponentWordRelation[exponent]} ";
						break;
					case 2:
						result += $"{HandleTwoDigitNumber(numberGroups[i])} {ExponentWordRelation[exponent]} ";
						break;

					case 3:
						string doubleDigitNumber = HandleTwoDigitNumber(numberGroups[i][1].ToString() + numberGroups[i][2].ToString());
						if (numberGroups[i][0] != '0')
						{
							result += $"{NumberWordRelation[numberGroups[i][0].ToString()]} hundred ";

							if (doubleDigitNumber != "")
								result += $"and {doubleDigitNumber}";

							result += $" {ExponentWordRelation[exponent]} ";
							break;
						}
						else
						{
							if (doubleDigitNumber != "")
							{
								// If this is the last whole number group so we need to add "and"
								if (exponent == 1)
									result += "and ";

								result += $"{doubleDigitNumber} {ExponentWordRelation[exponent]} ";
							}
						}
						break;

					default:
						throw new Exception("Error in number grouping");
				}
				exponent--;
			}
			#endregion

			#region Handle Decimal Number
			// If we have any decimals, then we continue our work here
			if (splitNumber.Length > 1)
			{
				result += "point";
				foreach (char c in splitNumber[1])
				{
					result += $" {NumberWordRelation[c.ToString()]}";
				}
			}
			// If we don't have decimals, then there is an extra space so lets get rid of that
			else
			{
				result = result.TrimEnd();
			}
			#endregion

			// If negative, add minus
			if (negativeNumber)
				result = result.Insert(0, "Negative ");

			// To be safe, remove any double spaces that may have crept in
			result = result.Replace("  ", " ");

			return result;
		}

		static string HandleTwoDigitNumber(string num)
		{
			if (num.Length != 2)
				throw new Exception($"Attempting to treat invalid number ({num}) as two digits");

			// If the number ends in a zero (except if the number is 10) we don't care about the second digit
			bool endsWithZero = num[1] == '0';

			// If number is 05 because the whole number is 205 we want to ignore the 0
			// If the number is 00 then we just ignore it entirely
			if (num[0] == '0')
			{
				return endsWithZero ? "" : NumberWordRelation[num[1].ToString()];
			}
			// if number is 17, we want to send both numbers together so we get seventeen
			if (num[0] == '1')
			{
				return NumberWordRelation[num];
			}
			// If first digit is anything but 1, we treat it as two separate values, but we still need the first digit to be a multiple of ten, so we add 0 to the end
			// e.g. 27 is sent as 20 and 7, returning twenty and seven. This is then combined into twenty-seven
			else
			{
				// Of course, if the number is 20, we don't want to say twenty-zero so we ignore the second digit
				if (!endsWithZero)
					return $"{NumberWordRelation[num[0].ToString() + "0"]}-{NumberWordRelation[num[1].ToString()]}";
				else
					return NumberWordRelation[num[0].ToString() + "0"];
			}
		}
	}
}
