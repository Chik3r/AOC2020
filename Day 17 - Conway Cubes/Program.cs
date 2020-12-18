using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Conway_Cubes
{
	class Program
	{
		static void Main(string[] args)
		{
			var path = Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\input.txt");
			List<List<List<bool>>> pocketDim = new List<List<List<bool>>>(); // [z][y][x]
			using (TextReader sr = new StreamReader(path, Encoding.UTF8))
			{
				string line;
				List<List<bool>> input = new List<List<bool>>();

				while ((line = sr.ReadLine()) != null)
				{
					List<bool> lineTypes = new List<bool>();

					foreach (var item in line)
					{
						lineTypes.Add(item == '#');
					}

					input.Add(lineTypes);
				}

				pocketDim.Add(input);
			}

			List<List<List<bool>>> modifiedDim = new List<List<List<bool>>>(); // [z][y][x]
			var a = GetNearTiles(1, 1, 0, pocketDim);

			for (int i = 0; i < 6; i++) // Expand the space
			{
				modifiedDim = pocketDim.DeepCopyList();

				List<bool> yList = new List<bool>(new bool[pocketDim.First().First().Count]);
				List<List<bool>> zList = new List<List<bool>>();
				for (int m = 0; m < pocketDim.First().Count; m++)
					zList.Add(yList.Select(k => k).ToList());

				modifiedDim.Insert(0, zList.DeepCopyList()); // Expand the plane on the z axis
				modifiedDim.Insert(modifiedDim.Count, zList.DeepCopyList());
				pocketDim.Insert(0, zList.DeepCopyList());
				pocketDim.Insert(pocketDim.Count, zList.DeepCopyList());

				for (int z = 0; z < modifiedDim.Count; z++)
				{
					modifiedDim[z].Insert(0, yList.DeepCopyList());
					modifiedDim[z].Insert(modifiedDim[z].Count, yList.DeepCopyList());
					pocketDim[z].Insert(0, yList.DeepCopyList());
					pocketDim[z].Insert(pocketDim[z].Count, yList.DeepCopyList());

					for (int y = 0; y < modifiedDim[z].Count; y++)
					{
						modifiedDim[z][y].Insert(0, false);
						modifiedDim[z][y].Insert(modifiedDim[z][y].Count, false);
						pocketDim[z][y].Insert(0, false);
						pocketDim[z][y].Insert(pocketDim[z][y].Count, false);
					}
				}

				pocketDim = modifiedDim.DeepCopyList();
			}

			for (int cycle = 0; cycle < 6; cycle++)
			{
				modifiedDim = pocketDim.DeepCopyList();

				for (int z = 0; z < modifiedDim.Count; z++)
				{
					for (int y = 0; y < modifiedDim[z].Count; y++)
					{
						for (int x = 0; x < modifiedDim[z][y].Count; x++)
						{
							int nearTiles = GetNearTiles(x, y, z, pocketDim);

							if (pocketDim[z][y][x] && !(nearTiles == 2 || nearTiles == 3))
								modifiedDim[z][y][x] = false;
							else if (!pocketDim[z][y][x] && nearTiles == 3)
								modifiedDim[z][y][x] = true;
						}
					}
				}

				pocketDim = modifiedDim.DeepCopyList();
			}

			int activeCubes = pocketDim.Sum(z => z.Sum(y => y.Count(x => x)));
			Console.WriteLine(activeCubes);
		}

		static int GetNearTiles(int x, int y, int z, List<List<List<bool>>> input)
		{
			int totalNeighbours = 0;

			for (int currX = -1; currX < 2; currX++)
			{
				for (int currY = -1; currY < 2; currY++)
				{
					for (int currZ = -1; currZ < 2; currZ++)
					{
						int currentX = x + currX;
						int currentY = y + currY;
						int currentZ = z + currZ;

						if (currentX == x && currentY == y && currentZ == z)
							continue;

						if (currentX >= 0 && currentY >= 0 && currentZ >= 0 && currentZ < input.Count() && currentY < input[currentZ].Count() && currentX < input[currentZ][currentY].Count)
						{
							if (input[currentZ][currentY][currentX] == true)
								totalNeighbours++;
						}
					}
				}
			}

			return totalNeighbours;
		}
	}

	static class ListUtils
	{
		public static List<List<List<bool>>> DeepCopyList(this List<List<List<bool>>> list)
		{
			return list.Select(z => z.Select(y => y.Select(x => x).ToList()).ToList()).ToList();
		}

		public static List<List<bool>> DeepCopyList(this List<List<bool>> list)
		{
			return list.Select(z => z.Select(y => y).ToList()).ToList();
		}

		public static List<bool> DeepCopyList(this List<bool> list)
		{
			return list.Select(z => z).ToList();
		}
	}
}
