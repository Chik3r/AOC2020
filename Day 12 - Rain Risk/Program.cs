using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Rain_Risk
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Instruction> input = new List<Instruction>();
			var path = Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\input.txt");
			using (TextReader sr = new StreamReader(path, Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					int value;
					if (line[0] == 'L' || line[0] == 'R')
						value = int.Parse(line.Substring(1)) / 90;
					else
						value = int.Parse(line.Substring(1));
					input.Add(new Instruction(line[0], value));
				}
			}

			int horizontalPos = 0; // Positive: East -- Negative: West
			int verticalPos = 0; // Positive: North -- Negative: South
			int direction = 1; // 0-N / 1-E / 2-S / 3-W
			foreach (var item in input)
			{
				switch (item.action)
				{
					case 'N':
						verticalPos += item.value;
						break;
					case 'E':
						horizontalPos += item.value;
						break;
					case 'S':
						verticalPos -= item.value;
						break;
					case 'W':
						horizontalPos -= item.value;
						break;
					case 'L':
						direction -= item.value;
						if (direction < 0)
							direction += 4;
						direction %= 4;
						break;
					case 'R':
						direction += item.value;
						direction %= 4;
						break;
					case 'F':
						if (direction == 0)
							verticalPos += item.value;
						else if (direction == 1)
							horizontalPos += item.value;
						else if (direction == 2)
							verticalPos -= item.value;
						else if (direction == 3)
							horizontalPos -= item.value;
						break;
				}
			}

			Console.WriteLine($"The Manhattan distance between that location and the ship's starting position is: {Math.Abs(horizontalPos) + Math.Abs(verticalPos)}");

			// ---- PART TWO ----
			horizontalPos = 0; // Positive: East -- Negative: West
			verticalPos = 0; // Positive: North -- Negative: South
			direction = 1; // 0-N / 1-E / 2-S / 3-W
			int waypointHorizontal = 10;
			int waypointVertical = 1;
			foreach (var item in input)
			{
				int origX = waypointHorizontal;
				int origY = waypointVertical;
				double radians = (90 * item.value) * (Math.PI / 180f);
				switch (item.action)
				{
					case 'N':
						waypointVertical += item.value;
						break;
					case 'E':
						waypointHorizontal += item.value;
						break;
					case 'S':
						waypointVertical -= item.value;
						break;
					case 'W':
						waypointHorizontal -= item.value;
						break;
					case 'L':
						waypointHorizontal = (int)Math.Round(origX * Math.Cos(radians) - (origY * Math.Sin(radians)));
						waypointVertical = (int)Math.Round(origX * Math.Sin(radians) + origY * Math.Cos(radians));
						break;
					case 'R':
						waypointHorizontal = (int)Math.Round((origX * Math.Cos(radians)) + (origY * Math.Sin(radians)));
						waypointVertical = (int)Math.Round(-(origX * Math.Sin(radians)) + origY * Math.Cos(radians));
						break;
					case 'F':
						horizontalPos += waypointHorizontal * item.value;
						verticalPos += waypointVertical * item.value;
						break;
				}
			}

			Console.WriteLine($"The Manhattan distance between that location and the ship's starting position is: {Math.Abs(horizontalPos) + Math.Abs(verticalPos)}");
		}
	}

	struct Instruction
	{
		public char action;
		public int value;

		public Instruction(char action, int value)
		{
			this.action = action;
			this.value = value;
		}
	}
}
