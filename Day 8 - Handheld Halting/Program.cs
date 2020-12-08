using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Handheld_Halting
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Instruction> input = new List<Instruction>();
			using (TextReader sr = new StreamReader(@"C:\Users\ikerv\Documents\Programming\Advent of Code\2020\Day 8 - Handheld Halting\input.txt", Encoding.UTF8))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					string[] split = line.Split(' ');
					input.Add(new Instruction(split[0], int.Parse(split[1])));
				}
			}

			// Part 1
			int accumulator = 0;
			int cursor = 0;
			while (true)
			{
				if (input[cursor].completed)
					break;

				input[cursor].completed = true;
				switch (input[cursor].type)
				{
					case InstructionType.acc:
						accumulator += input[cursor].argument;
						cursor++;
						break;
					case InstructionType.jmp:
						cursor += input[cursor].argument;
						break;
					case InstructionType.nop:
						cursor++;
						break;
				}
			}

			Console.WriteLine($"The value of the accumulator is: {accumulator}");

			// Part 2
			input = input.ConvertAll(x => new Instruction(x.type, x.argument));
			List<Instruction> modifiedInput = input;
			for (int i = modifiedInput.Count-1; i >= 0; i--) // Brute-force for the win! (TODO: be smarter and stop brute-forcing this)
			{
				if (modifiedInput[i].type == InstructionType.nop)
					modifiedInput[i].type = InstructionType.jmp;
				else if (modifiedInput[i].type == InstructionType.jmp)
					modifiedInput[i].type = InstructionType.nop;

				accumulator = 0;
				cursor = 0;
				while (true)
				{
					if (modifiedInput.All(x => x.completed) || cursor >= modifiedInput.Count)
						goto endLoop;

					if (modifiedInput[cursor].completed)
						break;

					modifiedInput[cursor].completed = true;
					switch (modifiedInput[cursor].type)
					{
						case InstructionType.acc:
							accumulator += modifiedInput[cursor].argument;
							cursor++;
							break;
						case InstructionType.jmp:
							cursor += modifiedInput[cursor].argument;
							break;
						case InstructionType.nop:
							cursor++;
							break;
					}
				}
				if (modifiedInput.All(x => x.completed))
					goto endLoop;
				modifiedInput = input.ConvertAll(x => new Instruction(x.type, x.argument));
			}
			
			endLoop:
			Console.WriteLine($"accumulator is: {accumulator}");
		}
	}

	public enum InstructionType
	{
		acc,
		jmp,
		nop
	}

	public class Instruction
	{
		public InstructionType type;
		public int argument;
		public bool completed;

		public Instruction(InstructionType type, int argument, bool completed = false)
		{
			this.type = type;
			this.argument = argument;
			this.completed = completed;
		}

		public Instruction(string type, int argument, bool completed = false)
		{
			InstructionType insType = InstructionType.nop;
			if (type == "acc")
				insType = InstructionType.acc;
			else if (type == "jmp")
				insType = InstructionType.jmp;

			this.type = insType;
			this.argument = argument;
			this.completed = completed;
		}
	}
}
