using AutoMapper;
using CiudadMujeres.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Profiles
{
	public class AccountProfile : Profile
	{
		public AccountProfile()
		{
			CreateMap<ApplicationUser, Models.Account.ResponseCredentials>().ReverseMap();
			CreateMap<IdentityRole, Models.Account.RoleDto>().ReverseMap();
		}
	}
}
