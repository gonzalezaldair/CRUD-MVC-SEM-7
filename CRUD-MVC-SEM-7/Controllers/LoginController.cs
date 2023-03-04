using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRUD_MVC_SEM_7.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioBL _oUsuarioBL;

        public LoginController()
        {
            _oUsuarioBL = new UsuarioBL();
        }
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioCLS modelo)
        {
            int usuario_creado = _oUsuarioBL.guardarUsuario(modelo);

            if (usuario_creado == 1)
            {
                TempData["Mensaje"] = "Usuario Registrado";
                return RedirectToAction("Login", "Login");
            }

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {

            var listaErrores = new List<string>();

            if (string.IsNullOrEmpty(correo))
            {
                listaErrores.Add("Campo Usuario: Debe Ingresar usuario");
            }

            if (string.IsNullOrEmpty(clave))
            {
                listaErrores.Add("Campo Compania:  Debe Ingresar Contraseña");
            }

            if (listaErrores.Count > 0) return Json(listaErrores);
            UsuarioBL oUsuarioBL = new UsuarioBL();
            UsuarioCLS oUsuarioCLS = null;

            oUsuarioCLS = oUsuarioBL.Login(correo, clave);

            if (oUsuarioCLS == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>() {
                            new Claim(ClaimTypes.Name, oUsuarioCLS.nombreusuario),
                            new Claim("idUsuario", oUsuarioCLS.idUsuario.ToString()),
                            new Claim("Correo", oUsuarioCLS.correo),
                            new Claim(ClaimTypes.Role, oUsuarioCLS.rol)

                        };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }
    }
}

