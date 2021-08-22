using AutoMapper;
using CiudadMujeres.Common;
using CiudadMujeres.Data;
using CiudadMujeres.Entities;
using CiudadMujeres.Models.Account;
using CiudadMujeres.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CiudadMujeres.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenCreator _jwtCreator;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        private readonly PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

        public AccountController(UserManager<ApplicationUser> userManager, ILogger<AccountController> logger,
            IConfiguration configuration, SignInManager<ApplicationUser> signInManager,
            JwtTokenCreator jwtCreator, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _logger = logger;
            _configuration = configuration;
            _roleManager = roleManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtCreator = jwtCreator;
        }

        #region Crear y Eliminar Cuenta de Administrador del Sistema
        /// <summary>
        /// Este endpoint se utiliza para registrar a un usuario como administrador dentro del sistema
        /// </summary>
        /// <param name="editarAdmin">la entidad para editar al administrador</param>
        /// <returns>regresa una tarea o la accion de resultado</returns>
        [HttpPost]
        [Route(RouteEntity.AccountInsertAdmin)]
        public async Task<ActionResult> RegistrarAdministrador(EditarAdmin editarAdmin)
        {
            //consultamos al usuario por email para asignarlo como administrador
            var usuario = await _userManager.FindByEmailAsync(editarAdmin.Email);
            //agregamos el claim descrito en la politica de seguridad o policy
            await _userManager.AddClaimAsync(usuario, new Claim("Admin", "1"));
            //no regresamos ningun tipo de respuesta
            return NoContent();
        }

        #region Login de Acceso al usuario dentro del sistema
        [HttpPost(RouteEntity.AccountLogin)]
        /// <summary>
        /// Este metodo se encarga de crear una sesión para un usuario dentro del sistema
        /// </summary>
        /// <param name="userLogin">el usuario con password y nombre de usuario</param>
        /// <returns>una entidad del tipo ResponseTokenAutenticacion</returns>
        public async Task<ActionResult<ResponseCredentials>> Login(UserLogin userLogin)
        {
            var signIn = await _signInManager.PasswordSignInAsync(userLogin.Username, userLogin.Password, false, false);

            if (signIn.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userLogin.Username);
                var roles = await _userManager.GetRolesAsync(user) as List<string>;
                var token = _jwtCreator.Generate(user.Email, user.Id, roles);

                ResponseCredentials response = _mapper.Map<ResponseCredentials>(user);
                response.Roles = roles;

                Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });
                Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });
                Response.Cookies.Append("X-Refresh-Token", Guid.NewGuid().ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });


                return Ok(response);
            }
            else
            {
                return BadRequest(ErrorLogController.LoginDenegate);
            }
        }

        #endregion

        [HttpGet(RouteEntity.AccountRefresh)]
		public async Task<IActionResult> Refresh()
		{
			if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
				return BadRequest();

			var user = _userManager.Users.FirstOrDefault(i => i.UserName == userName && i.RefreshToken == refreshToken);

            var roles = await _userManager.GetRolesAsync(user) as List<string>;

            if (user == null)
				return BadRequest();

			var token = _jwtCreator.Generate(user.Email, user.Id, roles);

			user.RefreshToken = Guid.NewGuid().ToString();

			await _userManager.UpdateAsync(user);

			Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });
			Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });
			Response.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });

			return Ok();
		}

		[HttpGet(RouteEntity.AccountLogout)]
		public IActionResult Logout()
		{
			foreach (var cookie in Request.Cookies)
			{
				Response.Cookies.Delete(cookie.Key);
			}
			return Ok();
		}

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario, Administrador")]
        [HttpGet("ping")]
        public string Ping() 
        {
            //var claimsIdentity = User.Identity as ClaimsIdentity;
            //foreach (var claim in claimsIdentity.Claims)
            //{
            //    Console.WriteLine(claim.Type + ":" + claim.Value);
            //}
            return "pong";
        }

		/// <summary>
		/// Este metodo se encarga de eliminar  un administrador dentro del sistema
		/// </summary>
		/// <param name="editarAdmin">la entidad editarAdmin para proceder a eliminar la entidad</param>
		/// <returns>una tarea o un actionresult</returns>
		[HttpPost]
        [Route(RouteEntity.AccountEliminarAdmin)]
        public async Task<ActionResult> EliminarAdministrador(EditarAdmin editarAdmin)
        {
            //consultamos al usuario por email para asignarlo como administrador
            var usuario = await _userManager.FindByEmailAsync(editarAdmin.Email);
            //eliminamos el claim descrito en la politica de seguridad o policy
            await _userManager.RemoveClaimAsync(usuario, new Claim("Admin", "1"));
            //no regresamos ningun tipo de respuesta
            return NoContent();
        }




        #endregion


        #region Crear un usuario dentro del sistema 
        /// <summary>
        /// Este metodo se encarga de registrar un usuario, solo se registran email y password
        /// </summary>
        /// <param name="userCredencial">el usuario con passwrd y email</param>
        /// <returns>una entidad del tipo ResponseTokenAutenticacion</returns>
        [Route(RouteEntity.AccountInsert)]
        [HttpPost]
        public async Task<ActionResult<ResponseTokenAutenticacion>> Registrar(UserCredencial userCredencial)
        {
            try
            {
                //registro de entidad de usuario de forma sencilla  mediante email, password
                var userIdentity = new ApplicationUser { UserName = userCredencial.Email, Email = userCredencial.Email };
                //se registra el usuario con las credenciales basicas
                var result = await _userManager.CreateAsync(userIdentity, userCredencial.Password);
                if (result.Succeeded)
                {

                    return await CrearToken(userCredencial);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorLogController.ErrorNameController + ex.Message);
                return StatusCode(500, ErrorLogController.ServerError);
            }
        }
        #endregion

        #region Generación de Token JWT
        /// <summary>
        /// Este metodo se encarga de construir el token de seguridad del usuario
        /// </summary>
        /// <param name="userCredencial">la entidad del tipo userCredencial con las credenciales del usuario</param>
        /// <returns>una entidad responseTokenAutenticacion que contiene el token de autenticacion</returns>
        private async Task<ResponseTokenAutenticacion> CrearToken(UserCredencial userCredencial)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,userCredencial.Email)
            };
            //1.-para el proceso de asignar los claims adecuados al token es necesario consultar al usuario por email
            var usuario = await _userManager.FindByEmailAsync(userCredencial.Email);
            //2.-obtenemos todos los claims del usuario
            var Dbclaims = await _userManager.GetClaimsAsync(usuario);
            //3.-Agregamos al rango los claims
            claims.AddRange(Dbclaims);


            //encriptamos el secret key generado para la aplicación
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretJwtKey"]));
            //generamos la credencial con la llave secreta
            var credencial = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Agregamos la expiracion del token
            var expiracion = DateTime.Now.AddYears(1);
            //construimos el security token que incluyen los claims y las credenciales con la llave secreta en hash
            var tokenSecurity = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: credencial);

            //regresamos como respuesta el jwtsecuritytoken que me devuelve las credenciales adecuadas del usuario
            return new ResponseTokenAutenticacion
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenSecurity),
                Expiracion = expiracion
            };
        }

        /// <summary>
        /// Este metodo se encarga de refrescar el token de seguridad de un usuario
        /// </summary>
        /// <returns>El token con la nueva actualizacion del token actualizado</returns>
        [HttpGet("RefreshToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ResponseTokenAutenticacion>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            var email = emailClaim.Value;
            var userCredencial = new UserCredencial()
            {
                Email = email
            };
            return await CrearToken(userCredencial);
        }

        #endregion
    }
}
