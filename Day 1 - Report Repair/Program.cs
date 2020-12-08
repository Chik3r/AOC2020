using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReportRepair	
{
	class Program
	{
		static List<int> values = new List<int>();

		static void Main(string[] args)
		{
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 1 - Report Repair\values.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					values.Add(int.Parse(line));
				}
			}

			Console.WriteLine($"The result for 2 values is: {SomeFuncTwo(values, 2020)}");
			Console.WriteLine($"The result for 3 values is: {SomeFuncThree(values, 2020)}");
		}

		static int SomeFuncTwo(List<int> array, int valueCompare)
		{
			foreach (var i in array)
			{
				foreach (var j in array)
				{
					if (i + j == valueCompare)
						return i * j;
				}
			}
			return 0;
		}

		static int SomeFuncThree(List<int> array, int valueCompare)
		{
			foreach (var i in array)
			{
				foreach (var j in array)
				{
					foreach (var k in array)
					{
						if (i + j + k == valueCompare)
							return i * j * k;
					}
				}
			}
			return 0;
		}
	}
}
