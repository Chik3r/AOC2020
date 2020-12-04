using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Passport_Processing
{
	class Program
	{
		static void Main(string[] args)
		{
			//string[] neededFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

			List<string> input = new List<string>();
			string tmpString = "";
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 4 - Passport Processing\input.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					if (string.IsNullOrWhiteSpace(line))
					{
						input.Add(tmpString);
						tmpString = "";
						continue;
					}

					tmpString += line + " ";
				}
			}

			int correctPassports = 0;
			foreach (var passport in input)
			{
				if (CheckPassportValid(passport))
					correctPassports++;

				//if (neededFields.All(passport.Contains))
				//	correctPassports++;
			}

			Console.WriteLine($"The number of valid passwords is: {correctPassports}");
		}

		static bool CheckPassportValid(string passport)
		{
			int keysFound = 0;
			string[] split = passport.Split(' ');
			foreach (var item in split)
			{
				string[] pair = item.Split(':');
				switch (pair[0])
				{
					case "byr":
						if (int.Parse(pair[1]) >= 1920 && int.Parse(pair[1]) <= 2002)
							keysFound++;
						continue;
					case "iyr":
						if (int.Parse(pair[1]) >= 2010 && int.Parse(pair[1]) <= 2020)
							keysFound++;
						continue;
					case "eyr":
						if (int.Parse(pair[1]) >= 2020 && int.Parse(pair[1]) <= 2030)
							keysFound++;
						continue;
					case "hgt":
						string unit = pair[1].Substring(pair[1].Length - 2);
						string value = pair[1].Substring(0, pair[1].Length - 2);
						if (unit == "cm" && int.Parse(value) >= 150 && int.Parse(value) <= 193)
							keysFound++;
						if (unit == "in" && int.Parse(value) >= 59 && int.Parse(value) <= 76)
							keysFound++;
						continue;
					case "hcl":
						if (pair[1][0] == '#' && pair[1].Length == 7)
							keysFound++;
						continue;
					case "ecl":
						string[] eyeColors = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
						if (eyeColors.Any(pair[1].Contains))
							keysFound++;
						continue;
					case "pid":
						if (pair[1].Length == 9)
						{
							foreach (var character in pair[1])
							{
								if (!int.TryParse(character.ToString(), out _))
									return false;
							}
							keysFound++;
						}
						continue;
				}
			}

			if (keysFound == 7)
				return true;
			return false;
		}
	}
}
