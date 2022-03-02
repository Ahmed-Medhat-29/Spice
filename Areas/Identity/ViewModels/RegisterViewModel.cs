using System.ComponentModel.DataAnnotations;

namespace Spice.Areas.Identity.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		[StringLength(200, MinimumLength = 3)]
		public string Email { get; set; }

		[Required]
		[Phone]
		[Display(Name = "Phone Number")]
		[StringLength(25, MinimumLength = 3)]
		public string PhoneNumber { get; set; }

		[Required]
		[Display(Name = "Street Address")]
		[StringLength(200, MinimumLength = 3)]
		public string StreetAddress { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 3)]
		public string City { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 3)]
		public string State { get; set; }

		[Display(Name = "Postal Code")]
		public int PostalCode { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 3)]
		public string Password { get; set; }

		[Required]
		[Display(Name = "Confirm Password")]
		[Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
		public string ConfirmPassword { get; set; }
	}
}
