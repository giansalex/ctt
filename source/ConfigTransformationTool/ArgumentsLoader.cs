namespace ConfigTransformationTool
{
	/// <summary>
	/// Command line helper. Load parameters from comman line arguments.
	/// </summary>
	internal class ArgumentsLoader
	{
		/// <summary>
		/// Load arguments from command line
		/// </summary>
		/// <param name="args"></param>
		public void Load(string[] args)
		{
			DestinationFilePath = string.Empty;
			SourceFilePath = string.Empty;
			TransformFilePath = string.Empty;
			ParametersString = string.Empty;
			ForceParametersTask = false;

			foreach (string arg in args)
			{
				if (arg.StartsWith("s:") || arg.StartsWith("source:"))
					SourceFilePath = GetFileNameFromArguments(arg);

				if (arg.StartsWith("t:") || arg.StartsWith("transform:"))
					TransformFilePath = GetFileNameFromArguments(arg);

				if (arg.StartsWith("d:") || arg.StartsWith("destination:"))
					DestinationFilePath = GetFileNameFromArguments(arg);

				if (arg.StartsWith("p:") || arg.StartsWith("parameters:"))
					ParametersString = GetValueFromArguments(arg);

				if (arg.StartsWith("fpt"))
					ForceParametersTask = true;
			}
		}

		public bool IsAllRequiredParametersSet
		{
			get
			{
				return (!string.IsNullOrWhiteSpace(SourceFilePath)
				        && !string.IsNullOrWhiteSpace(TransformFilePath)
				        && !string.IsNullOrWhiteSpace(DestinationFilePath));
			}
		}

		public string SourceFilePath { get; private set; }

		public string TransformFilePath { get; private set; }
		
		public string DestinationFilePath { get; private set; }

		public string ParametersString { get; private set; }

		public bool ForceParametersTask { get; private set; }

		private static string GetFileNameFromArguments(string arg)
		{
			return GetValueFromArguments(arg).Trim('"');
		}

		private static string GetValueFromArguments(string arg)
		{
			int startIndex = arg.IndexOf(":") + 1;
			return arg.Substring(startIndex, arg.Length - startIndex);
		}
	}
}