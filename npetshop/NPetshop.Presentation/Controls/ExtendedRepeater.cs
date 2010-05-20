
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NPetshop.Presentation.Controls
{
	/// <summary>
	/// Summary description for ExtendedRepeater.
	/// </summary>
	public class ExtendedRepeater : Repeater
	{

		private ITemplate _noDataTemplate = null;

		public ITemplate NoDataTemplate 
		{
			get 
			{
				return _noDataTemplate;
			}
			set 
			{
				_noDataTemplate = value;
			}
		}


		protected override void OnPreRender(System.EventArgs e)
		{
			base.OnDataBinding (e);

			if(this.Items.Count == 0) 
			{
				NoDataTemplate.InstantiateIn(this);
			}

		}
	}
}
