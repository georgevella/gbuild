using System;
using GBuild.Configuration.Models;

namespace GBuild.CommitHistory
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class SupportedBranchTypeAttribute : Attribute
	{
		public BranchType BranchType { get; }

		public SupportedBranchTypeAttribute(BranchType branchType)
		{
			BranchType = branchType;
		}
	}
}