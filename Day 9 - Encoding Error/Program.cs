using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Encoding_Error
{
	class Program
	{
		static void Main(string[] args)
		{
			List<long> input = new List<long>();
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 9 - Encoding Error\input.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					input.Add(long.Parse(line));
				}
			}

			long target = CheckNumbersBefore(25, input);
			Console.WriteLine($"The first number that is not the sum of the 25 numbers before is: {target}");

			Console.WriteLine($"The encryption weakness is: {CheckNumbersSum(target, input)}");
		}

		static long CheckNumbersBefore(int preamble, List<long> input)
		{
			for (int i = preamble; i < input.Count; i++)
			{
				List<long> numBefore = input.GetRange(i - preamble, preamble);
				bool numberBeforeValid = false;
				foreach (var x in numBefore)
				{
					if (numBefore.Where(y => x + y == input[i]).Count() != 0)
						numberBeforeValid = true;
				}
				if (!numberBeforeValid)
					return input[i];
			}

			return 0;
		}

		static long CheckNumbersSum(long target, List<long> input)
		{
			List<long> sublist;
			for (int x = 0; x < input.Count; x++)
			{
				for (int y = 1; y < input.Count - x; y++)
				{
					sublist = input.GetRange(x, y);
					if (sublist.Sum() == target)
					{
						return sublist.Min() + sublist.Max(); ;
					}
				}
			}

			return 0;
		}
	}
}
