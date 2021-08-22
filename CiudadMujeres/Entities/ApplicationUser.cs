using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string RefreshToken { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MaidenName { get; set; }
		[NotMapped]
		public string FullName => $"{FirstName} {LastName} {MaidenName}";
	}
}
