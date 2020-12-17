using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Ticket_Translation
{
	class Program
	{
		static void Main(string[] args)
		{
			Dictionary<string, MultipleRange> fields = new Dictionary<string, MultipleRange>();
			List<int> ownTicket = new List<int>();
			List<List<int>> nearbyTickets = new List<List<int>>();
			int currentLine = 0;
			int fieldIndex = 0;

			var path = Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\input.txt");
			using (TextReader sr = new StreamReader(path, Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					if (string.IsNullOrWhiteSpace(line))
					{
						currentLine++;
						continue;
					}

					if (currentLine == 0)
					{
						Regex regex = new Regex(@"([\w ]+): ([0-9]+)-([0-9]+) or ([0-9]+)-([0-9]+)"); // Get fields values and field name
						GroupCollection groups = regex.Match(line).Groups;

						MultipleRange range = new MultipleRange();
						range.ranges.Add((int.Parse(groups[2].Value), int.Parse(groups[3].Value))); 
						range.ranges.Add((int.Parse(groups[4].Value), int.Parse(groups[5].Value)));
						range.listIndex = fieldIndex;
						fieldIndex++;

						fields[groups[1].Value] = range;
					}
					else if (currentLine == 1 || currentLine == 3)
						currentLine++;
					else if (currentLine == 2)
					{
						ownTicket = line.Split(',').Select(num => int.Parse(num)).ToList();
					}
					else
					{
						nearbyTickets.Add(line.Split(',').Select(num => int.Parse(num)).ToList());
					}
				}
			}

			// Part one
			List<int> wrongFields = new List<int>();
			for (int i = 0; i < nearbyTickets.Count; i++)
			{
				foreach (var value in nearbyTickets[i]) // Loop through the fields in a ticket
				{
					if (!fields.Any(x => x.Value.InRanges(value))) // And check that it is a valid field
					{
						wrongFields.Add(value);
						nearbyTickets.RemoveAt(i);
						i--; // This is done because RemoveAt changes the index of the following values
					}
				}
			}

			Console.WriteLine($"Error rate: {wrongFields.Sum()}");

			Console.WriteLine("-------- Part Two --------");

			Dictionary<string, MultipleRange> ticketFields = new Dictionary<string, MultipleRange>(fields); // Deep copy the original fields
			do
			{
				foreach (var item in fields)
				{
					if (!ticketFields.ContainsKey(item.Key))
						continue;

					List<int> invalidIndexes = fields.Select(x => x.Value.index).ToList(); // Indexes of already found fields
					for (int i = 0; i < fields.Count; i++)
					{
						if (invalidIndexes.Contains(i)) // Ignore already found indexes
							continue;

						List<int> ticketsAtIndex = nearbyTickets.Select(x => x[i]).ToList(); // Get the values at i column in every ticket
						var validFields = ticketFields.Where(field => ticketsAtIndex.All(value => field.Value.InRanges(value))).ToList(); // Get all fields that are valid
						if (validFields.Count == 1)
						{
							KeyValuePair<string, MultipleRange> field = validFields.First();
							Console.WriteLine($"Field: {field.Key} - index: {i}");
							fields[field.Key].index = i;
							ticketFields.Remove(field.Key);
							break;
						}
					}
				}
			} while (ticketFields.Count != 0);

			List<KeyValuePair<string, MultipleRange>> departureFields = fields.Where(value => value.Key.StartsWith("departure")).ToList(); // Get fields starting with departure
			long departureProduct = 1; // End product, starts with 1 so it can always be multiplied
			foreach (var item in departureFields)
			{
				departureProduct *= ownTicket[item.Value.index];
			}
			Console.WriteLine($"\nDeparture product: {departureProduct}");
		}
	}

	class MultipleRange
	{
		public List<(int, int)> ranges = new List<(int, int)>();
		public int index = -1;
		public int listIndex = -1;

		public bool InRanges(int number)
		{
			foreach (var item in ranges)
			{
				if (number >= item.Item1 && number <= item.Item2)
					return true;
			}

			return false;
		}
	}
}
