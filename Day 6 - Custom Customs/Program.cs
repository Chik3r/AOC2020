using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Custom_Customs
{
	class Program
	{
		static void Main(string[] args)
		{
			List<List<string>> input = new List<List<string>>();
			List<string> tmpList = new List<string>();
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 6 - Custom Customs\input.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					if (string.IsNullOrWhiteSpace(line))
					{
						input.Add(tmpList);
						tmpList = new List<string>();
						continue;
					}

					tmpList.Add(line);
				}
			}

			int totalQuestionsAnswered = 0;
			int everyoneAnswered = 0;
			foreach (var item in input)
			{
				totalQuestionsAnswered += string.Join("", item).Distinct().Count();
				foreach (var character in item[0])
				{
					var b = item.Where(x => x.Contains(character));
					if (b.Count() == item.Count)
						everyoneAnswered++;
				}
			}
			Console.WriteLine($"The total number of questions answered is: {totalQuestionsAnswered}");
			Console.WriteLine($"The total number of questions everyone answered is: {everyoneAnswered}");
		}
	}
}
