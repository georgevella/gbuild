using System;

namespace GBuild.Core.Exceptions
{
	public class CommitAnalysisException : Exception
	{
		public CommitAnalysisException() : base("A general error occured during commit analysis.")
		{
			
		}

		public CommitAnalysisException(string message, Exception innerException) : base($"Commit Analysis Failure: {message ?? "<null>"}", innerException)
		{
			
		}

		public CommitAnalysisException(string message) : base($"Commit Analysis Failure: {message ?? "<null>"}")
		{
			
		}
	}
}