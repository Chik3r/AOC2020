using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seating_System
{
	class Program
	{
		static (int, int)[] dirs = new (int, int)[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

		static void Main(string[] args)
		{
			List<List<TileType>> input = new List<List<TileType>>(); // [y][x]
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 11 - Seating System\input.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					List<TileType> lineTypes = new List<TileType>();
					foreach (var item in line)
					{
						lineTypes.Add(item == 'L' ? TileType.FREE_SEAT : TileType.FLOOR);
					}
					input.Add(lineTypes);
				}
			}

			List<List<TileType>> modifiedList = new List<List<TileType>>();
			while (true)
			{
				modifiedList = input.Select(y => y.Select(x => x).ToList()).ToList();
				for (int y = 0; y < input.Count; y++)
				{
					for (int x = 0; x < input[y].Count; x++)
					{
						//int nearTiles = GetNearTiles(x, y, input);	// Uncomment for Part 1
						int nearTiles = GetCardinalTiles(x, y, input);	// Comment for Part 1
						switch (input[y][x])
						{
							case TileType.FREE_SEAT:
								if (nearTiles == 0)
									modifiedList[y][x] = TileType.OCCUPIED_SEAT;
								break;
							case TileType.OCCUPIED_SEAT:
								//if (nearTiles >= 4)	// Uncomment for Part 1
								if (nearTiles >= 5)	// Comment for Part 1
									modifiedList[y][x] = TileType.FREE_SEAT;
								break;
							case TileType.FLOOR:
								break;
						}
					}
				}

				int checkEquality = 0;
				for (int i = 0; i < modifiedList.Count; i++)
				{
					if (modifiedList[i].SequenceEqual(input[i]))
						checkEquality++;
				}
				if (checkEquality == input.Count)
					break;

				input = modifiedList.Select(y => y.Select(x => x).ToList()).ToList();
			}

			int occupiedSeats = modifiedList.Sum(i => i.Count(x => x == TileType.OCCUPIED_SEAT));
			Console.WriteLine($"Number of occupied seats is: {occupiedSeats}");
		}

		static int GetNearTiles(int x, int y, List<List<TileType>> input, TileType targetTile = TileType.OCCUPIED_SEAT)
		{
			int totalNeighbours = 0;

			foreach (var item in dirs)
			{
				int currentX = x + item.Item1;
				int currentY = y + item.Item2;

				if (currentX >= 0 && currentY >= 0 && currentX < input.First().Count && currentY < input.Count())
				{
					if (input[currentY][currentX] == TileType.OCCUPIED_SEAT)
						totalNeighbours++;
				}
			}

			return totalNeighbours;
		}

		static int GetCardinalTiles(int x, int y, List<List<TileType>> input, TileType targetTile = TileType.OCCUPIED_SEAT)
		{
			int totalNeighbours = 0;

			foreach (var item in dirs)
			{
				int currentX = x + item.Item1;
				int currentY = y + item.Item2;

				while (currentX >= 0 && currentY >= 0 && currentX < input.First().Count && currentY < input.Count())
				{
					if (input[currentY][currentX] == TileType.OCCUPIED_SEAT)
						totalNeighbours++;
					if (input[currentY][currentX] != TileType.FLOOR)
						break;

					currentX += item.Item1;
					currentY += item.Item2;
				}
			}

			return totalNeighbours;
		}
	}

	enum TileType
	{
		FREE_SEAT,
		OCCUPIED_SEAT,
		FLOOR
	}
}
