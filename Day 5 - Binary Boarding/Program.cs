using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Binary_Boarding
{
	class Program
	{
		static void Main(string[] args)
		{
			List<string> input = new List<string>();
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 5 - Binary Boarding\input.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					input.Add(line);
				}
			}

			List<int> seatIDs = new List<int>();
			foreach (var item in input)
			{
				int min = 0;
				int max = 127;
				foreach (var character in item.Substring(0, 7))
				{
					CalculateHalf(ref min, ref max, character == 'B');
				}
				int row = min;

				min = 0;
				max = 8;
				foreach (var character in item.Substring(7))
				{
					CalculateHalf(ref min, ref max, character == 'R');
				}
				int column = min;

				seatIDs.Add(row * 8 + column);
			}

			Console.WriteLine($"The highest seat ID on a pass is: {seatIDs.Max()}");

			seatIDs.Sort();
			for (int i = 1; i < seatIDs.Count; i++)
			{
				if (seatIDs[i - 1] != seatIDs[i] - 1)
				{
					Console.WriteLine($"The ID of your seat is: {seatIDs[i] - 1}");
					break;
				}
			}
		}

		static void CalculateHalf(ref int min, ref int max, bool upper)
		{
			int c = max - min;

			if (upper)
			{
				c = (int)Math.Round(c / 2f, MidpointRounding.ToPositiveInfinity); // 2.5 -> 3
				min = min + c;
			}
			else
			{
				c = (int)Math.Round(c / 2f, MidpointRounding.ToNegativeInfinity); // 2.5 -> 2
				max = min + c;
			}
		}
	}
}
