using System;

namespace ILMerging
{
	public class ILMerger
	{
		/// <summary>
		/// Given a set of IL assemblies, merge them into a single IL assembly.
		/// All inter-assembly references in the set are resolved to be
		/// intra-assembly references in the merged assembly.
		/// </summary>
		[STAThread]
		public static int Main(string[] args)
		{
			ILMerge merger = new ILMerge();

			#region Check Usage

			if (merger.CheckUsage(args))
			{
				Console.WriteLine(merger.UsageString);
				return 0;
			}

			#endregion

			#region Process Command Line Arguments

			if (!merger.ProcessCommandLineOptions(args))
			{
				Console.WriteLine(merger.UsageString);
				return 1;
			}

			#endregion

			#region Validate Options

			if (!merger.ValidateOptions())
			{
				Console.WriteLine(merger.UsageString);
				return 1;
			}

			#endregion

			#region Echo command line into log

			{
				merger.WriteToLog("=============================================");
				merger.WriteToLog("Timestamp (UTC): " + DateTime.UtcNow.ToString());
				System.Reflection.Assembly a = typeof(ILMerge).Assembly;
				merger.WriteToLog("ILMerge version " + a.GetName().Version.ToString());
				merger.WriteToLog("Copyright (C) Microsoft Corporation 2004-2006. All rights reserved.");
			}
			string commandLine = "ILMerge ";
			foreach (string s in args)
			{
				commandLine += s + " ";
			}

			merger.WriteToLog(commandLine);

			#endregion Echo command line into log

			#region Perform real merging

			try
			{
				merger.Merge();
			}
			catch (Exception e)
			{
				if (merger.log)
				{
					merger.WriteToLog("An exception occurred during merging:");
					merger.WriteToLog(e.Message);
					merger.WriteToLog(e.StackTrace);
				}
				else
				{
					Console.WriteLine("An exception occurred during merging:");
					Console.WriteLine(e.Message);
					Console.WriteLine(e.StackTrace);
				}

				return 1;
			}

			#endregion

			return 0;
		}
	}
}