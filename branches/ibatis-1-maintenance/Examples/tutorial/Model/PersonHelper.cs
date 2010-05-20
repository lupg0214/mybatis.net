using System.Collections;

namespace iBatisTutorial.Model
{
	/// <summary>
	/// Helper class for Person entities.
	/// </summary>
	public class PersonHelper : Helper
	{
		public Person Select (int id)
		{
			return (Person) Mapper ().QueryForObject ("Select", id);
		}

		public IList SelectAll ()
		{
			return Mapper ().QueryForList ("Select", null);
		}

		public int Insert (Person person)
		{
			Mapper ().Insert ("Insert", person);
			// Insert is designed so that it can return the new key
			// but we are not utilizing that feature here
			return 1;
		}

		public int Update (Person person)
		{
			return Mapper ().Update ("Update", person);
		}

		public int Delete (int id)
		{
			return Mapper ().Delete ("Delete", id);
		}

	}
}