using CapaNegocio;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using System.Security.Claims;

namespace CRUD_MVC_SEM_7.Controllers
{
    
    public class UsuarioController : Controller
    {
        private readonly UsuarioBL _oUsuarioBL;

        public UsuarioController()
        {
            _oUsuarioBL = new UsuarioBL();
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var oLista = _oUsuarioBL.ListarUsuarios();

            return View(oLista);
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult CrearUsuario()
        {
            return View();
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult CrearUsuario(UsuarioCLS oUsuario)
        {
            if (!ModelState.IsValid)
                return View();


            var respuesta = _oUsuarioBL.guardarUsuario(oUsuario);

            if (respuesta == 1)
            {
                TempData["Mensaje"] = "Usuario Creado";
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Mensaje"] = "Error al Crear usuario";
                return View();
            }
        }
        [Authorize(Roles = "Administrador,Suscriptor")]
        public IActionResult CambiarContrasena()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string idusuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                idusuario = claimuser.Claims.Where(c => c.Type == "idUsuario")
                    .Select(c => c.Value).SingleOrDefault();
            }

            return View(new UsuarioCLS { idUsuario =  Convert.ToInt32(idusuario) });
        }
        [Authorize(Roles = "Administrador,Suscriptor")]
        [HttpPost]
        public IActionResult CambiarContrasena(UsuarioCLS oUsuario)
        {
            var respuesta = _oUsuarioBL.CambiarContrasena(oUsuario);

            if (respuesta == 1) {

                TempData["Mensaje"] = "Contraseña Actualizada";

                return RedirectToAction("Index", "Home"); 
            }
            else {

                ViewData["Mensaje"] = "Error al actualizar la contraseña";

                return View(); 
            }
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult EditarUsuario(int idUsuario)
        {
            var oUsuario = _oUsuarioBL.recuperarUsuario(idUsuario);
            return View(oUsuario);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult EditarUsuario(UsuarioCLS oUsuario)
        {
            if (!ModelState.IsValid)
                return View();


            var respuesta = _oUsuarioBL.actualizarUsuario(oUsuario);

            if (respuesta == 1)
            {
                TempData["Mensaje"] = "Usuario Actualizado";

                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Mensaje"] = "Error al actualizar usuario";
                return View();
            }
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarUsuario(int idUsuario)
        {
            var oUsuario = _oUsuarioBL.recuperarUsuario(idUsuario);
            return View(oUsuario);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Eliminar(UsuarioCLS oUsuario)
        {

            var respuesta = _oUsuarioBL.eliminarUsuario(oUsuario.idUsuario);

            if (respuesta == 1)
            {
                TempData["Mensaje"] = "Usuario Eliminado";
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Mensaje"] = "Error al Eliminar usuario";
                return View();
            }
        }
        [Authorize(Roles = "Administrador,Suscriptor")]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Mensaje"] = "Sesion Cerrada Correctamente";
            return RedirectToAction("Login", "Login");
        }
    }
}
