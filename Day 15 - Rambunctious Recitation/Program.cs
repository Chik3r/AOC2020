using System;
using System.Collections.Generic;

namespace Rambunctious_Recitation
{
	class Program
	{
		static void Main(string[] args)
		{
			List<int> startingNumbers = new List<int> { 20, 9, 11, 0, 1, 2 };
			Dictionary<int, List<int>> spokenNumbers = new Dictionary<int, List<int>>();
			int lastNumberSpoken = -1;
			int turnNumber = 1;

			foreach (var item in startingNumbers)
			{
				Console.WriteLine($"Starting: {item} - Turn: {turnNumber}");
				spokenNumbers.Add(item, new List<int> { turnNumber });
				lastNumberSpoken = item;
				turnNumber++;
			}

			do
			{
				if (spokenNumbers[lastNumberSpoken].Count == 1)
				{
					if (spokenNumbers.ContainsKey(0))
						spokenNumbers[0].Add(turnNumber);
					else
						spokenNumbers[0] = new List<int> { turnNumber };

					lastNumberSpoken = 0;
				}
				else
				{
					List<int> turns = spokenNumbers[lastNumberSpoken];
					int newNum = turns[turns.Count - 1] - turns[turns.Count - 2];

					if (spokenNumbers.ContainsKey(newNum))
						spokenNumbers[newNum].Add(turnNumber);
					else
						spokenNumbers[newNum] = new List<int> { turnNumber };

					lastNumberSpoken = newNum;
				}

				// Console.WriteLine($"Turn: {turnNumber} - last: {lastNumberSpoken}");

				turnNumber++;
			// } while (turnNumber <= 2020); // Part one
			} while (turnNumber <= 30000000); // Part two

			Console.WriteLine($"The last number spoken is: {lastNumberSpoken}");
		}
	}
}
