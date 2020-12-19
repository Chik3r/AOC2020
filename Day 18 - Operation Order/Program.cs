using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Operation_Order
{
	class Program
	{
		static Regex regexParentheses = new Regex(@"\(([\w\s\+\*]+)\)");
		static Regex operations = new Regex(@"(\w+) ([\+\*]) (\w+)");
		static Regex operationsSum = new Regex(@"(\w+) (\+) (\w+)");
		static Regex operationsMult = new Regex(@"(\w+) (\*) (\w+)");

		static void Main(string[] args)
		{
			var path = Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\input.txt");
			List<long> results = new List<long>();

			using (TextReader sr = new StreamReader(path, Encoding.UTF8)) /// Part 1
			{
				string line;

				while ((line = sr.ReadLine()) != null)
				{
					while (regexParentheses.IsMatch(line))
					{
						line = regexParentheses.Replace(line, m => SolveMath(m.Groups[1].Value) + "");
					}
					results.Add(SolveMath(line));
				}
			}

			Console.WriteLine($"The sum for part 1 is: {results.Sum()}");
			results = new List<long>();

			using (TextReader sr = new StreamReader(path, Encoding.UTF8))
			{
				string line;

				while ((line = sr.ReadLine()) != null)
				{
					while (regexParentheses.IsMatch(line))
					{
						line = regexParentheses.Replace(line, m => SolveMath2(m.Groups[1].Value) + "");
					}
					results.Add(SolveMath2(line));
				}
			}

			Console.WriteLine($"The sum for part 2 is: {results.Sum()}");
		}

		static long SolveMath(string input)
		{
			while (operations.IsMatch(input))
			{
				input = operations.Replace(input, m => m.Groups[2].Value == "+"
								? (long.Parse(m.Groups[1].Value) + long.Parse(m.Groups[3].Value)) + ""
								: (long.Parse(m.Groups[1].Value) * long.Parse(m.Groups[3].Value)) + "", 
								1);
			}
			return long.Parse(input);
		}

		static long SolveMath2(string input)
		{
			while (operationsSum.IsMatch(input))
			{
				input = operationsSum.Replace(input, m => (long.Parse(m.Groups[1].Value) + long.Parse(m.Groups[3].Value)) + "", 1);
			}
			while (operationsMult.IsMatch(input))
			{
				input = operationsMult.Replace(input, m => (long.Parse(m.Groups[1].Value) * long.Parse(m.Groups[3].Value)) + "", 1);
			}

			return long.Parse(input);
		}
	}
}
