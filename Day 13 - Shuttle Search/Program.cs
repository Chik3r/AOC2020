using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shuttle_Search
{
	class Program
	{
		static void Main(string[] args)
		{
			Dictionary<Bus, int> input = new Dictionary<Bus, int>();
			int targetTime = 0;

			string path = Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\input.txt");
			int lineNum = 0;
			using (TextReader sr = new StreamReader(path, Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					if (lineNum == 0)
					{
						targetTime = int.Parse(line);
						lineNum++;
						continue;
					}

					foreach (var item in line.Split(','))
					{
						if (item == "x")
						{
							lineNum++;
							continue;
						}

						input.Add(new Bus(int.Parse(item), lineNum - 1), 0);
						lineNum++;
					}
				}
			}

			List<Bus> keys = new List<Bus>(input.Keys);
			foreach (var key in keys)
			{
				input[key] = ((int)Math.Round((float)targetTime / (float)key.ID, MidpointRounding.ToPositiveInfinity)) * key.ID;
			}

			KeyValuePair<Bus, int> minValue = input.Aggregate((l, r) => l.Value < r.Value ? l : r);
			Console.WriteLine($"Result: {(minValue.Value - targetTime) * minValue.Key.ID}");

			// -------- Part Two --------
			int dictIndex = 0;
			Dictionary<int, Bus> newInput = input.Select(x => x.Key).ToDictionary(x => dictIndex++);
			List<long> resultOperations = new List<long>(new long[newInput.Count]);
			for (int i = 0; i < newInput.Count; i++)
			{
				for (int x = 0; x < newInput.Count; x++)
				{
					if (x == i)
						continue;

					if (resultOperations[x] != 0)
						resultOperations[x] *= (long)newInput[i].ID;
					else
						resultOperations[x] = (long)newInput[i].ID;
				}
			}

			for (int i = 0; i < newInput.Count; i++)
			{
				long moduloResult = (resultOperations[i] + newInput[i].offset) % newInput[i].ID;

				if (moduloResult == 0)
					continue;

				for (long m = 2; m < Math.Pow(10, 12); m++)
				{
					long mod = ((resultOperations[i] * m) + (long)newInput[i].offset) % (long)newInput[i].ID;
					if (mod == 0)
					{
						resultOperations[i] *= m;
						break;
					}
				}
			}

			Console.WriteLine(resultOperations.Sum());
			Console.WriteLine(resultOperations.Sum() % newInput.Aggregate((long)1, (acc, val) => acc * (long)val.Value.ID));
		}

		class Bus
		{
			public int ID;
			public int offset;

			public Bus(int id, int offset)
			{
				ID = id;
				this.offset = offset;
			}
		}
	}
}
