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
			List<List<List<List<bool>>>> pocketDim = new List<List<List<List<bool>>>>(); // [z][y][x]
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
				List<List<List<bool>>> zInput = new List<List<List<bool>>>();
				zInput.Add(input);
				pocketDim.Add(zInput);
			}

			List<List<List<List<bool>>>> modifiedDim = new List<List<List<List<bool>>>>(); // [z][y][x]

			for (int i = 0; i < 6; i++) // Expand the space
			{
				modifiedDim = pocketDim.DeepCopyList();

				List<bool> yList = new List<bool>(new bool[pocketDim.First().First().First().Count]);
				List<List<bool>> zList = new List<List<bool>>();
				for (int m = 0; m < pocketDim.First().First().Count; m++)
					zList.Add(yList.DeepCopyList());
				List<List<List<bool>>> wList = new List<List<List<bool>>>();
				for (int m = 0; m < pocketDim.First().Count; m++)
					wList.Add(zList.DeepCopyList());

				modifiedDim.Insert(0, wList.DeepCopyList()); // Expand the plane on the z axis
				modifiedDim.Insert(modifiedDim.Count, wList.DeepCopyList());
				pocketDim.Insert(0, wList.DeepCopyList());
				pocketDim.Insert(pocketDim.Count, wList.DeepCopyList());

				for (int w = 0; w < modifiedDim.Count; w++)
				{
					modifiedDim[w].Insert(0, zList.DeepCopyList()); // Expand the plane on the z axis
					modifiedDim[w].Insert(modifiedDim[w].Count, zList.DeepCopyList());
					pocketDim[w].Insert(0, zList.DeepCopyList());
					pocketDim[w].Insert(pocketDim[w].Count, zList.DeepCopyList());

					for (int z = 0; z < modifiedDim[w].Count; z++)
					{
						modifiedDim[w][z].Insert(0, yList.DeepCopyList());
						modifiedDim[w][z].Insert(modifiedDim[w][z].Count, yList.DeepCopyList());
						pocketDim[w][z].Insert(0, yList.DeepCopyList());
						pocketDim[w][z].Insert(pocketDim[w][z].Count, yList.DeepCopyList());

						for (int y = 0; y < modifiedDim[w][z].Count; y++)
						{
							modifiedDim[w][z][y].Insert(0, false);
							modifiedDim[w][z][y].Insert(modifiedDim[w][z][y].Count, false);
							pocketDim[w][z][y].Insert(0, false);
							pocketDim[w][z][y].Insert(pocketDim[w][z][y].Count, false);
						}
					}
				}

				pocketDim = modifiedDim.DeepCopyList();
			}

			for (int cycle = 0; cycle < 6; cycle++)
			{
				modifiedDim = pocketDim.DeepCopyList();

				for (int w = 0; w < modifiedDim.Count; w++)
				{
					for (int z = 0; z < modifiedDim[w].Count; z++)
					{
						for (int y = 0; y < modifiedDim[w][z].Count; y++)
						{
							for (int x = 0; x < modifiedDim[w][z][y].Count; x++)
							{
								int nearTiles = GetNearTiles(x, y, z, w, pocketDim);

								if (pocketDim[w][z][y][x] && !(nearTiles == 2 || nearTiles == 3))
									modifiedDim[w][z][y][x] = false;
								else if (!pocketDim[w][z][y][x] && nearTiles == 3)
									modifiedDim[w][z][y][x] = true;
							}
						}
					}
				}

				pocketDim = modifiedDim.DeepCopyList();
			}

			int activeCubes = pocketDim.Sum(w => w.Sum(z => z.Sum(y => y.Count(x => x))));
			Console.WriteLine(activeCubes);
		}

		static int GetNearTiles(int x, int y, int z, int w, List<List<List<List<bool>>>> input)
		{
			int totalNeighbours = 0;

			for (int currW = -1; currW < 2; currW++)
			{
				for (int currX = -1; currX < 2; currX++)
				{
					for (int currY = -1; currY < 2; currY++)
					{
						for (int currZ = -1; currZ < 2; currZ++)
						{
							int currentX = x + currX;
							int currentY = y + currY;
							int currentZ = z + currZ;
							int currentW = w + currW;

							if (currentX == x && currentY == y && currentZ == z && currentW == w)
								continue;

							if (currentX >= 0 && currentY >= 0 && currentZ >= 0 && currentW >= 0 && currentW < input.Count && currentZ < input[currentW].Count() && currentY < input[currentW][currentZ].Count() && currentX < input[currentW][currentZ][currentY].Count)
							{
								if (input[currentW][currentZ][currentY][currentX] == true)
									totalNeighbours++;
							}
						}
					}
				}
			}

			return totalNeighbours;
		}
	}

	static class ListUtils
	{
		public static List<List<List<List<bool>>>> DeepCopyList(this List<List<List<List<bool>>>> list)
		{
			return list.Select(w => w.Select(z => z.Select(y => y.Select(x => x).ToList()).ToList()).ToList()).ToList();
		}

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
