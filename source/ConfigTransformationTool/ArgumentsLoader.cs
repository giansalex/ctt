namespace ConfigTransformationTool
{
	internal class ArgumentsLoader
	{
		public void Load(string[] args)
		{
			DestinationFilePath = string.Empty;
			SourceFilePath = string.Empty;
			TransformFilePath = string.Empty;

			foreach (string arg in args)
			{
				if (arg.StartsWith("s:") || arg.StartsWith("source:"))
					SourceFilePath = GetFileNameFromArguments(arg);

				if (arg.StartsWith("t:") || arg.StartsWith("transform:"))
					TransformFilePath = GetFileNameFromArguments(arg);

				if (arg.StartsWith("d:") || arg.StartsWith("destination:"))
					DestinationFilePath = GetFileNameFromArguments(arg);
			}
		}

		public bool IsAllParametersSet
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

		private static string GetFileNameFromArguments(string arg)
		{
			int startIndex = arg.IndexOf(":") + 1;
			return arg.Substring(startIndex, arg.Length - startIndex).Trim('"');
		}
	}
}