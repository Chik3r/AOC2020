using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Password_Philosophy
{
	class Program
	{
		static void Main(string[] args)
		{
			List<string> input = new List<string>();
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 2 - Password Philosophy\input.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					input.Add(line);
				}
			}

			int correctPasswords = 0;
			foreach (var item in input)
			{
				string[] firstSplit = item.Split(": ");
				string[] secondSplit = firstSplit[0].Split(' ');
				string[] thirdSplit = secondSplit[0].Split('-');
				int min = int.Parse(thirdSplit[0]);
				int max = int.Parse(thirdSplit[1]);
				int charOccurrences = Regex.Matches(firstSplit[1], $"({secondSplit[1]})").Count;

				if (min <= charOccurrences && charOccurrences <= max)
					correctPasswords++;
			}

			Console.WriteLine($"The number of correct passwords is: {correctPasswords}");

			correctPasswords = 0;
			foreach (var item in input)
			{
				string[] firstSplit = item.Split(": ");
				string[] secondSplit = firstSplit[0].Split(' ');
				string[] thirdSplit = secondSplit[0].Split('-');
				int min = int.Parse(thirdSplit[0]);
				int max = int.Parse(thirdSplit[1]);
				//int charOccurrences = Regex.Matches(firstSplit[1], $"({secondSplit[1]})").Count;

				if ((firstSplit[1][min-1] == char.Parse(secondSplit[1])) ^ (firstSplit[1][max - 1] == char.Parse(secondSplit[1]))) 
					correctPasswords++;
			}

			Console.WriteLine($"The number of correct passwords for part 2 is: {correctPasswords}");
		}
	}
}
