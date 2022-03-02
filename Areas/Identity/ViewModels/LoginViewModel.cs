using System.ComponentModel.DataAnnotations;

namespace Spice.Areas.Identity.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		[EmailAddress]
		[StringLength(200, MinimumLength = 3)]
		public string Email { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 3)]
		public string Password { get; set; }
	}
}
