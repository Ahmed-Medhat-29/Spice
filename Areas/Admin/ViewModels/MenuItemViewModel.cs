using System.Collections.Generic;
using Spice.Models;

namespace Spice.Areas.Admin.ViewModels
{
	public class MenuItemViewModel
	{
		public IEnumerable<Category> Categories { get; set; }
		public MenuItem MenuItem { get; set; }
	}
}
