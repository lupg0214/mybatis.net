using IBatisNet.DataMapper;

namespace iBatisTutorial.Model
{
	/// <summary>
	/// Base class for Helper objects (*Helper). 
	/// Provides shared utility methods.
	/// </summary>
	public abstract class Helper
	{		
		public SqlMapper Mapper ()
		{
			return IBatisNet.DataMapper.Mapper.Instance ();
		}
	}
}