namespace TechnologyOneNumberToWordsConverter.Tests
{
	public static class UnitTest
	{
		static readonly SortedDictionary<string, string> tests = new()
		{
			{ "A", "Invalid Input" }, { "0", "Zero" }, { "1", "One" }, { "10", "Ten" },
			{ "15", "Fifteen" }, { "21", "Twenty-one" }, { "42", "Forty-two" }, { "100", "One hundred" },
			{ "123", "One hundred and twenty-three" }, { "1,000", "One thousand" },
			{ "1234", "One thousand two hundred and thirty-four" }, { "10,000,000", "Ten million" },
			{ "10000001", "Ten million and one" }, { "2,000,000,200", "Two billion two hundred" },
			{ "005", "Five" }, { "-5", "Negative five" }, { "-005", "Negative five" },
			{ "0.2", "Zero point two" }, { "-0.2", "Negative zero point two" },
			{ ".1", "Zero point one" }, { "-.1", "Negative zero point one" },
			{ "-02.01", "Negative two point zero one" }, { "1.020", "one point zero two" },
			{ "00,02", "Two" }, { "15,000,100,005", "Fifteen billion one hundred thousand and five" },
			{ "861743861493643", "Eight hundred and sixty-one trillion seven hundred and forty-three billion eight hundred and sixty-one million four hundred and ninety-three thousand six hundred and forty-three" },
			{ "999,999,999,999,999,999,999,999,999,999,999,999,999", "Nine point nine times ten to the power of thirty-eight" },
			{ "999,999,999,999,999,999,999,999,999,999,999,999,999.01", "Nine point nine times ten to the power of thirty-eight" },
			{ "1,000,000,000,000,000,000,000,000,000", "One octillion" },
			{ "5,,5,29", "Five thousand five hundred and twenty-nine" },
			{ ".00200", "Zero point zero zero two" }, { "12 5", "One hundred and twenty-five" },
			{ ",,,", "Invalid Input" }, { "-0", "Negative zero" }, { "0-0", "Invalid Input" },
			{ "0.0.1", "Invalid Input" }, {"00.00", "Zero"}, {"-0.0.0", "Invalid Input" },
			{ "-000.000", "Negative zero"}, {"- 019,104 ,1 .9", "Negative one hundred and ninety-one thousand and forty-one point nine"}
		};

		// For access to ValideAndSanitiseInput method
		static ConvertController controller = new();

		public static void RunTests()
		{
			List<(string key, string output)> failedKeys = new();
			foreach (var test in tests)
			{
				if (string.IsNullOrEmpty(test.Key))
				{
					Console.WriteLine("TEST KEY IS MISSING");
					continue;
				}

				string result = "";

				string validatedKey = test.Key;
				if (controller.ValidateAndSanitiseInput(ref validatedKey))
				{
					try
					{
						result = NumberToWordsConverter.Convert(validatedKey);
					}
					catch (Exception e)
					{
						result = "Invalid Input";
						Console.WriteLine("Error: " + e.Message);
					}
				}
				else
				{
					result = "Invalid Input";
				}

				if (result.ToLower() != test.Value.ToLower())
				{
					failedKeys.Add((test.Key, result));
				}
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Passed Tests: {tests.Count - failedKeys.Count}");

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Failed Tests: {failedKeys.Count}");
			if (failedKeys.Count > 0)
			{
				Console.WriteLine("Failures:");
				foreach (var failedKey in failedKeys)
				{
					Console.WriteLine($"Input: {failedKey.key}, Expected Output: {tests[failedKey.key]}, Actual Output: {failedKey.output}");
				}
			}
			Console.ResetColor();
		}
	}
}


