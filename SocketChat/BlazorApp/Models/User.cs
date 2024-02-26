using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlazorApp.Model
{
	public class User
	{
        public long Id { get; set; }
        public string Username { get; set; }
		public string Email { get; set; }
        public string Token { get; set; }
		public Boolean IsActive { get; set; }
    }

	[RequiresUnreferencedCode("hehe")]
    public class UserLogin
	{
		[Required(ErrorMessage = "Kullanici adi bos olamaz.")]
		[MinLength(3, ErrorMessage = "Kullanici adi en az 3 karakter olmalidir.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Parola bos olamaz.")]
		public string Password { get; set; }
	}
}
