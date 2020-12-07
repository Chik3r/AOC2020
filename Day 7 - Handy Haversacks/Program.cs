using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Handy_Haversacks
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Bag> input = new List<Bag>();
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 7 - Handy Haversacks\input.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					string[] split = line.Split(" ");

					Bag bag = new Bag()
					{
						color = $"{split[0]} {split[1]}",
						amount = 1
					};

					if (split[4] == "no")
					{
						input.Add(bag);
						continue;
					}

					List<Bag> containedBags = new List<Bag>();
					for (int offset = 4; offset < split.Length; offset += 4)
					{
						containedBags.Add(new Bag()
						{
							color = $"{split[1 + offset]} {split[2 + offset]}",
							amount = int.Parse(split[offset])
						});
						if (split[offset + 3].EndsWith('.'))
							break;
					}
					bag.contains = containedBags;
					input.Add(bag);
				}
			}

			int numberBags = 0;
			foreach (var item in input)
			{
				if (item.ContainsBag(input, "shiny gold"))
					numberBags++;
			}

			Bag goldBag = input.Where(x => x.color == "shiny gold").First();

			Console.WriteLine($"{numberBags} bags contain at least one shiny gold bag");
			Console.WriteLine($"{goldBag.BagsInside(input, out bool _)} bags are inside a shiny gold bag");
		}

		class Bag
		{
			public string color;
			public int amount;
			public List<Bag> contains;

			public bool ContainsBag (List<Bag> bags, string color, string currentColor = "")
			{
				var tmp = contains;
				if (string.IsNullOrEmpty(currentColor))
					currentColor = color;

				if (tmp == null)
					tmp = bags.Where(x => x.color == currentColor).First().contains;

				if (tmp == null)
					return false;

				foreach (var item in tmp)
				{
					if (item.color == color)
						return true;

					if (item.ContainsBag(bags, color, item.color) == false)
					{
						continue;
					}

					return true;
				}

				return false;
			}

			public int BagsInside(List<Bag> bags, out bool endList)
			{
				var tmp = contains;
				endList = false;

				if (tmp == null)
					tmp = bags.Where(x => x.color == color).First().contains;

				if (tmp == null)
				{
					endList = true;
					return amount;
				}

				int totalInside = 0;
				foreach (var item in tmp)
				{
					int x = item.BagsInside(bags, out bool ended);
					if (!ended)
						totalInside += item.amount + item.amount * x;
					else
						totalInside += item.amount;
				}
				return totalInside;
			}
		}
	}
}
