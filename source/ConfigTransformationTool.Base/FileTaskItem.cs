using System;
using System.Collections;
using Microsoft.Build.Framework;

namespace ConfigTransformationTool.Base
{
	/// <summary>
	/// Simple implementation of <see cref="ITaskItem"/>. Just store at <see cref="ItemSpec"/>.
	/// </summary>
	internal class FileTaskItem : ITaskItem
	{
		/// <summary>
		/// Create new object and set <paramref name="filePath"/> to <see cref="ItemSpec"/>.
		/// </summary>
		/// <param name="filePath">File path</param>
		public FileTaskItem(string filePath)
		{
			ItemSpec = filePath;
		}

		/// <summary>
		/// File path
		/// </summary>
		public string ItemSpec { get; set; }

		#region Not implemented

		public string GetMetadata(string metadataName)
		{
			throw new NotImplementedException();
		}

		public void SetMetadata(string metadataName, string metadataValue)
		{
			throw new NotImplementedException();
		}

		public void RemoveMetadata(string metadataName)
		{
			throw new NotImplementedException();
		}

		public void CopyMetadataTo(ITaskItem destinationItem)
		{
			throw new NotImplementedException();
		}

		public IDictionary CloneCustomMetadata()
		{
			throw new NotImplementedException();
		}

		public ICollection MetadataNames
		{
			get { throw new NotImplementedException(); }
		}

		public int MetadataCount
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

	}
}