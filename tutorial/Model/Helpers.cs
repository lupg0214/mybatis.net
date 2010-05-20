namespace iBatisTutorial.Model
{
	/// <summary>
	/// Singleton "controller" for Helper classes.
	/// </summary>
	public class Helpers
	{
		private static volatile PersonHelper _PersonHelper = null;

		public static PersonHelper Person ()
		{
			if (_PersonHelper == null)
			{
				lock (typeof (PersonHelper))
				{
					if (_PersonHelper == null) // double-check
						_PersonHelper = new PersonHelper ();
				}
			}
			return _PersonHelper;
		}
	}
}
