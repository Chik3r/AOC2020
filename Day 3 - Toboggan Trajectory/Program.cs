using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Toboggan_Trajectory
{
	class Program
	{
		static void Main(string[] args)
		{
			List<List<bool>> input = new List<List<bool>>();
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 3 - Toboggan Trajectory\input.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					List<bool> lineBool = new List<bool>();
					for (int i = 0; i < line.Length; i++)
					{
						lineBool.Add(line[i] == '#');
					}
					input.Add(lineBool);
				}
			}

			Console.WriteLine($"The number of trees encountered is: {CheckSlope(input, 3, 1)}");

			Console.WriteLine($"The number of trees for the following slopes are: \n" +
								$"- Right 1, down 1: {CheckSlope(input, 1, 1)}\n" +
								$"- Right 3, down 1: {CheckSlope(input, 3, 1)}\n" +
								$"- Right 5, down 1: {CheckSlope(input, 5, 1)}\n" +
								$"- Right 7, down 1: {CheckSlope(input, 7, 1)}\n" +
								$"- Right 1, down 2: {CheckSlope(input, 1, 2)}\n" +
								$"If you multiply them together, you get: {CheckSlope(input, 1, 1) * CheckSlope(input, 3, 1) * CheckSlope(input, 5, 1) * CheckSlope(input, 7, 1) * CheckSlope(input, 1, 2)}");
		}

		static long CheckSlope(List<List<bool>> input, int xIncrement, int yIncrement)
		{
			int x = 0;
			int y = 0;
			long numberTrees = 0;

			while (y != input.Count)
			{
				y += yIncrement;
				if (y >= input.Count)
					break;

				x += xIncrement;
				if (x >= input[y].Count)
					x -= input[y].Count;

				if (input[y][x])
					numberTrees++;
			}

			return numberTrees;
		}
	}
}
