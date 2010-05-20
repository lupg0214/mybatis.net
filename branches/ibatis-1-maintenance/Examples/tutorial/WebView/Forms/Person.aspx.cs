using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using iBatisTutorial.Model;

namespace iBatisTutorial.Web.Forms
{
	public class PersonPage : Page
	{
		#region panel: List 

		protected Panel pnlList;
		protected DataGrid dgList;
		protected Button btnAdd;

		private void List_Init ()
		{
			btnAdd.Text = "Add New Person";
			this.btnAdd.Click += new EventHandler (List_Add);
		}

		private void List_Load ()
		{
			dgList.DataSource = Helpers.Person ().SelectAll ();
			dgList.DataBind ();
		}

		protected void List_Delete (object source, DataGridCommandEventArgs e)
		{
			int id = GetKey (dgList, e);
			Helpers.Person ().Delete (id);
			List_Load ();
		}

		protected void List_Edit (object source, DataGridCommandEventArgs e)
		{
			dgList.EditItemIndex = e.Item.ItemIndex;
			List_Load ();
		}

		protected void List_Update (object source, DataGridCommandEventArgs e)
		{
			Person person = new Person ();
			person.Id = GetKey (dgList, e);
			person.FirstName = GetText (e, 0);
			person.LastName = GetText (e, 1);
			person.HeightInMeters = GetDouble (e, 2);
			person.WeightInKilograms = GetDouble (e, 3);
			Helpers.Person ().Update (person);
			List_Cancel(source,e); // Almost a hack :)
		}

		protected void List_Cancel (object source, DataGridCommandEventArgs e)
		{
			dgList.EditItemIndex = -1;
			List_Load ();
		}

		private int GetKey (DataGrid dg, DataGridCommandEventArgs e)
		{
			return (Int32) dg.DataKeys[e.Item.DataSetIndex];
		}

		private string GetText (DataGridCommandEventArgs e, int v)
		{
			return ((TextBox) e.Item.Cells[v].Controls[0]).Text;
		}

		private double GetDouble (DataGridCommandEventArgs e, int v)
		{
			return Convert.ToDouble (GetText (e, v));
		}

		protected void List_Add (object source, EventArgs e)
		{
			Person person = new Person ();
			person.FirstName = "--New Person--";
			Helpers.Person ().Insert (person);
			List_Load ();
		}

		#endregion

		private void Page_Load (object sender, EventArgs e)
		{
			List_Init ();
			if (!IsPostBack)
				List_Load ();
		}

		#region Web Form Designer generated code

		protected override void OnInit (EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent ();
			base.OnInit (e);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.Load += new EventHandler (this.Page_Load);
		}

		#endregion
	}
}