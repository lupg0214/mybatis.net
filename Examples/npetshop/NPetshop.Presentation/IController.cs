using System;

namespace NPetshop.Presentation
{
	/// <summary>
	/// Summary description for IController.
	/// </summary>
	public interface IController
	{
		string CurrentView
		{
			get;
			set;
		}

		string NextView
		{
			get;
			set;
		}
	}
}
