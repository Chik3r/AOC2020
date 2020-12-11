using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Adapter_Array
{
	class Program
	{
		static void Main(string[] args)
		{
			//Dictionary<int, int> input = new Dictionary<int, int>();
			List<int> input = new List<int>();
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 10 - Adapter Array\input.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					//input.Add(int.Parse(line), 0);
					input.Add(int.Parse(line));
				}
			}
			//input = input.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
			input.Sort();

			int previousAdapter = 0;
			int differenceOf3 = 1; // Starts with one because the computer always has a difference of 3
			int differenceOf1 = 0;
			foreach (var item in input)
			{
				switch (item - previousAdapter)
				{
					case 1:
						differenceOf1++;
						break;
					case 3:
						differenceOf3++;
						break;
				}
				previousAdapter = item;
			}

			Console.WriteLine($"The number of 1-jolt differences multiplied by the number of 3-jolt differences is : {differenceOf1 * differenceOf3}");

			input.Add(input.Max() + 3);
			long[] answers = new long[input.Max() + 3];
			answers[0] = 1;
			foreach (var item in input)
			{ 
				// Tribonacci time
				answers[item] = answers.ElementAtOrDefault(item - 1) + answers.ElementAtOrDefault(item - 2) + answers.ElementAtOrDefault(item - 3);
			}

			Console.WriteLine($"Total number of arrangements: {answers.Max()}");
		}
	}
}