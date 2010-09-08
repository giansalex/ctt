using System;
using System.Diagnostics;

namespace ConfigTransformationTool.Tests
{
	/// <summary>
	/// http://lukencode.com/2010/03/28/c-micro-performance-testing-class/
	/// </summary>
	public class PerformanceTester
	{
		public TimeSpan TotalTime { get; private set; }
		public TimeSpan AverageTime { get; private set; }
		public TimeSpan MinTime { get; private set; }
		public TimeSpan MaxTime { get; private set; }
		public Action Action { get; set; }

		public PerformanceTester(Action action)
		{
			Action = action;
			MaxTime = TimeSpan.MinValue;
			MinTime = TimeSpan.MaxValue;
		}

		/// <summary>
		/// 	Micro performance testing
		/// </summary>
		public void MeasureExecTime()
		{
			var sw = Stopwatch.StartNew();
			Action();
			sw.Stop();
			AverageTime = sw.Elapsed;
			TotalTime = sw.Elapsed;
		}

		/// <summary>
		/// 	Micro performance testing
		/// </summary>
		/// <param name = "iterations">the number of times to perform action</param>
		/// <returns></returns>
		public void MeasureExecTime(int iterations)
		{
			Action(); // warm up
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				Action();
			}
			sw.Stop();
			AverageTime = new TimeSpan(sw.Elapsed.Ticks/iterations);
			TotalTime = sw.Elapsed;
		}

		/// <summary>
		/// 	Micro performance testing, also measures
		/// 	max and min execution times
		/// </summary>
		/// <param name = "iterations">the number of times to perform action</param>
		public void MeasureExecTimeWithMetrics(int iterations)
		{
			TimeSpan total = new TimeSpan(0);

			Action(); // warm up
			for (int i = 0; i < iterations; i++)
			{
				var sw = Stopwatch.StartNew();

				Action();

				sw.Stop();
				TimeSpan thisIteration = sw.Elapsed;
				total += thisIteration;

				if (thisIteration > MaxTime) MaxTime = thisIteration;
				if (thisIteration < MinTime) MinTime = thisIteration;
			}

			TotalTime = total;
			AverageTime = new TimeSpan(total.Ticks/iterations);
		}
	}
}