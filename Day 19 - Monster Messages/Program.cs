using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Day_19___Monster_Messages
{
	class Program
	{
		static Regex subRuleRegex = new Regex(@"(\d+): ([\d ]+) \| ([\d ]+)");
		static Regex singleRuleRegex = new Regex(@"(\d+): ([\d ]+)", RegexOptions.Compiled); // 0: 1
		static Regex stringRuleRegex = new Regex("(\\d+): \"(\\w)\"", RegexOptions.Compiled); // 0: "a"

		static void Main(string[] args)
		{
			Dictionary<int, Rule> rules = new Dictionary<int, Rule>();
			List<string> input = new List<string>();

			var path = Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\input.txt");
			bool addingRules = true;

			using (TextReader sr = new StreamReader(path, Encoding.UTF8))
			{
				string line;

				while ((line = sr.ReadLine()) != null)
				{
					if (string.IsNullOrWhiteSpace(line))
					{
						addingRules = false;
						continue;
					}

					if (!addingRules)
					{
						input.Add(line);
						continue;
					}

					if (subRuleRegex.IsMatch(line))
					{
						GroupCollection groups = subRuleRegex.Match(line).Groups;
						Rule rule = new Rule();
						for (int i = 2; i < groups.Count; i++)
						{
							List<int> ids = new List<int>();
							foreach (var item in groups[i].Value.Split(" "))
							{
								ids.Add(int.Parse(item));
							}
							rule.subRules.Add(ids);
						}
						rules.Add(int.Parse(groups[1].Value), rule);
					}
					else if (singleRuleRegex.IsMatch(line))
					{
						GroupCollection groups = singleRuleRegex.Match(line).Groups;
						Rule rule = new Rule();
						for (int i = 2; i < groups.Count; i++)
						{
							List<int> ids = new List<int>();
							foreach (var item in groups[i].Value.Split(" "))
							{
								ids.Add(int.Parse(item));
							}
							rule.subRules.Add(ids);
						}
						rules.Add(int.Parse(groups[1].Value), rule);
					}
					else if (stringRuleRegex.IsMatch(line))
					{
						GroupCollection groups = stringRuleRegex.Match(line).Groups;
						Rule rule = new Rule();
						rule.value = groups[2].Value;
						rules.Add(int.Parse(groups[1].Value), rule);
					}
				}
			}

			string regexExpression = rules[0].GetValue(rules, master: true);
			Regex regex = new Regex(regexExpression, RegexOptions.Compiled);
			List<Match> matches = input.Select(l => regex.Match(l)).ToList();

			Console.WriteLine($"Result: {matches.Where(l => l.Success).Count()}");
		}
	}

	class Rule
	{
		public List<List<int>> subRules = new List<List<int>>();
		public string value = "";

		public string GetValue(Dictionary<int, Rule> rules, int position = -1, bool master = false)
		{
			if (!string.IsNullOrWhiteSpace(value))
				return value;
			
			string result;
			if (master)
				result = "^(";
			else
				result = "(?:";

			for (int i = 0; i < subRules.Count; i++)
			{
				foreach (int rule in subRules[i])
				{
					result += rules[rule].GetValue(rules, rule);
				}
				if (i + 1 != subRules.Count)
					result += "|";
			}

			if (master)
				return result + @")$";
			return result + ")";
		}
	}
}
