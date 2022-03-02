using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Spice.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string Name { get; set; }

		[Required]
		[StringLength(200, MinimumLength = 3)]
		public string StreetAddress { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 3)]
		public string City { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 3)]
		public string State { get; set; }

		public int PostalCode { get; set; }
	}
}