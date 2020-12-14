using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Docking_Data
{
	class Program
	{
		enum BitmaskState
		{
			On, 
			Off,
			Old
		}

		static void Main(string[] args)
		{
			Dictionary<int, long> memory = new Dictionary<int, long>(); // Address, value
			List<BitmaskState> bitmask = new List<BitmaskState>();
			var path = Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\input.txt");
			using (TextReader sr = new StreamReader(path, Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					if (line.Contains('X')) // Check if it's a bitmask and grab it
					{
						bitmask = line.Split(" = ")[1].Select(bit =>
						{
							if (bit == '0')
								return BitmaskState.Off;
							else if (bit == '1')
								return BitmaskState.On;
							else
								return BitmaskState.Old;
						}).ToList();
						bitmask.Reverse();
						continue;
					}

					Regex memRegex = new Regex(@"\[(\w+)\] = (\w+)");
					GroupCollection memGroup = memRegex.Match(line).Groups;

					int address = int.Parse(memGroup[1].Value);
					long value = long.Parse(memGroup[2].Value);

					BitArray valueBits = new BitArray(BitConverter.GetBytes(value));
					for (int i = 0; i < bitmask.Count; i++)
					{
						switch (bitmask[i])
						{
							case BitmaskState.On:
								valueBits[i] = true;
								break;
							case BitmaskState.Off:
								valueBits[i] = false;
								break;
						}
					}
					value = GetIntFromBitArray(valueBits);

					if (memory.ContainsKey(address))
						memory[address] = value;
					else
						memory.Add(address, value);
				}
			}

			Console.WriteLine($"The result is: {memory.Values.Sum()}");
		}

		static long GetIntFromBitArray(BitArray bitArray)
		{
			var array = new byte[8];
			bitArray.CopyTo(array, 0);
			return BitConverter.ToInt64(array, 0);
		}
	}
}
