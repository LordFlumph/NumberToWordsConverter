using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TechnologyOneNumberToWordsConverter
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertController : ControllerBase
    {
		[HttpPost]
		public IActionResult Convert([FromBody] ConvertRequest request)
		{
			string? number = request.Amount;

			// Confirm that given input is valid and clean it up (remove commas, leading zeroes, etc)
			if (!ValidateAndSanitiseInput(ref number))
			{
				return BadRequest("Invalid Input");
			}

			// Number is valid so we convert it			
			try
			{
				// Get result and make it all upper case
				string result = NumberToWordsConverter.Convert(number).ToUpper();
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest($"Error converting number: {e.Message}");
			}			
		}

		public bool ValidateAndSanitiseInput(ref string? number)
		{
			// Confirm there is text
			if (string.IsNullOrEmpty(number))
			{
				return false;
			}

			// Remove spaces
			number = number.Replace(" ", "");

			// confirm value is valid and clean up value for parsing
			bool hasDecimalPoint = false;
			bool isNegativeNumber = false;
			bool containsDigit = false;
			for (int i = number.Length - 1; i >= 0; i--)
			{
				// Confirm there is only one decimal point at most
				if (number[i] == '.')
				{
					if (hasDecimalPoint)
					{
						return false;
					}

					hasDecimalPoint = true;
					continue;
				}

				// Remove commas (1,234,567.89 is allowed as input but will be ignored)
				if (number[i] == ',')
				{
					number = number.Remove(i, 1);
					continue;
				}

				if (!char.IsDigit(number[i]))
				{
					// If the first character is a minus sign, then we need to remember that, but remove the minus sign for later sanitation purposes
					if (i == 0 && number[i] == '-')
					{
						isNegativeNumber = true;
						number = number.Remove(i, 1);
					}
					else
					{
						return false;
					}
					
				}
				else
				{
					containsDigit = true;
				}
			}

			// If there are no digits in this, then we don't need to do anything at all
			if (!containsDigit)
				return false;


			string[] splitNumber = number.Split('.');
			// Handle zeroes at the start of the whole number
			if (number[0] == '0')
			{			
				bool allZeroes = true;
				foreach (char c in splitNumber[0])
				{
					if (c != '0')
					{
						allZeroes = false;
						break;
					}
				}

				// Remove all leading zeroes
				number = number.TrimStart('0');

				// If the whole number was nothing but zeroes, then we add one back in
				if (allZeroes)
				{
					number = number.Insert(0, "0");
				}				
			}

			// If there are decimal numbers, then we trim the zeroes from the end
			if (splitNumber.Length > 1)
			{
				number = number.TrimEnd('0');
			}

			// If there are no digits before the decimal point, add a zero
			if (number[0] == '.')
			{
				number = number.Insert(0, "0");
			}

			// If all the digits after the decimal point were zeroes, remove the decimal point
			if (number[number.Length-1] == '.')
			{
				number = number.Remove(number.Length - 1, 1);
			}

			// Add the minus sign back in if it was there to begin with
			if (isNegativeNumber)
			{
				number = number.Insert(0, "-");
			}

			return true;
		}
	}

	public class ConvertRequest()
	{
		public string? Amount { get; set; }
	}
}
