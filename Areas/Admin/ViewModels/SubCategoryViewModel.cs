using System.Collections.Generic;
using Spice.Models;

namespace Spice.Areas.Admin.ViewModels
{
	public class SubCategoryViewModel
	{
		public IEnumerable<Category> Categories { get; set; }
		public SubCategory SubCategory { get; set; }
	}
}
