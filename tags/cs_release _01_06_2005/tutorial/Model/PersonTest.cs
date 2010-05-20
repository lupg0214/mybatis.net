using System.Collections;
using NUnit.Framework;

namespace iBatisTutorial.Model
{
	/// <summary>
	/// Tests to exercise Person and PersonHelper methods.
	/// </summary>
	[TestFixture]
	public class PersonTest
	{
		[Test]
		public void PersonList ()
		{
			// try it 
			IList people = Helpers.Person ().SelectAll ();

			// test it 
			Assert.IsNotNull (people, "Person list not returned");
			Assert.IsTrue (people.Count > 0, "Person list is empty");
			Person person = (Person) people[0];
			Assert.IsNotNull (person, "Person not returned");
		}

		[Test]
		public void PersonUpdate ()
		{
			const string EXPECT = "Clinton";
			const string EDITED = "Notnilc";

			// get it
			Person person = Helpers.Person ().Select (1);

			// test it
			Assert.IsNotNull (person, "Missing person");
			Assert.IsTrue (EXPECT.Equals (person.FirstName), "Mistaken identity");

			//change it
			person.FirstName = EDITED;
			Helpers.Person ().Update (person);

			// get it again
			person = Helpers.Person ().Select (1);

			// test it 
			Assert.IsTrue (EDITED.Equals (person.FirstName), "Same old, same old?");

			// change it back
			person.FirstName = EXPECT;
			Helpers.Person ().Update (person);
		}

		[Test]
		public void PersonInsertDelete ()
		{
			// insert it
			Person person = new Person ();
			person.Id = -1;
			Helpers.Person ().Insert (person);
			// delete it
			int count = Helpers.Person ().Delete (person.Id);
			Assert.IsTrue (count > 0, "Nothing to delete");
		}
	}
}