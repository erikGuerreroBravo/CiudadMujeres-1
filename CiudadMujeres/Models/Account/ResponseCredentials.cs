using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Models.Account
{
	public class ResponseCredentials
	{
		public string FullName { get; set; }
		public string Email { get; set; }

		public List<string> Roles { get; set; }
	}
}
