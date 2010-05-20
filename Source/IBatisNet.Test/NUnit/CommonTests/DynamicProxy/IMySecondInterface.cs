using System;

namespace IBatisNet.Test.NUnit.CommonTests.DynamicProxy
{
	/// <summary>
	///  Summary description for IMySecondInterface.
	/// </summary>
	public interface IMySecondInterface : IMyInterface
	{
		string Address
		{
			get;
			set;
		}
	}
}
