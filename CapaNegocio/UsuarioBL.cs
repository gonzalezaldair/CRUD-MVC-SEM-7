using CapaDatos;
using CapaEntidad;
using System.Security.Cryptography;
using System.Text;

namespace CapaNegocio
{
    public class UsuarioBL
    {

        private readonly UsuarioDAL _oUsuarioDAL;

        public UsuarioBL()
        {
            _oUsuarioDAL = new UsuarioDAL();
        }
        public List<UsuarioCLS> ListarUsuarios()
        {
            return _oUsuarioDAL.ListarUsuarios();
        }

        private static string EncriptarClave(string clave)
        {

            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;

                byte[] result = hash.ComputeHash(enc.GetBytes(clave));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();

        }

        public UsuarioCLS Login(string usuario, string contrasena)
        {
            string contrasenaCifrada = EncriptarClave(contrasena);
            return _oUsuarioDAL.Login(usuario, contrasenaCifrada);
        }

        public int guardarUsuario(UsuarioCLS oUsuario)
        {
            oUsuario.clave = EncriptarClave(oUsuario.clave);
            oUsuario.activo = true;
            oUsuario.rol = string.IsNullOrEmpty(oUsuario.rol) ? "Suscriptor" : oUsuario.rol;
            return _oUsuarioDAL.GuardarUsuario(oUsuario);
        }

        public int actualizarUsuario(UsuarioCLS oUsuario)
        {
            return _oUsuarioDAL.ActualizarUsuario(oUsuario);
        }

        public int CambiarContrasena(UsuarioCLS oUsuario)
        {
            oUsuario.clave = EncriptarClave(oUsuario.clave);
            return _oUsuarioDAL.CambiarContrasena(oUsuario);
        }

        public UsuarioCLS recuperarUsuario(int idUsuario)
        {
            return _oUsuarioDAL.ObtenerUsuario(idUsuario);
        }

        public int eliminarUsuario(int idUsuario)
        {
            return _oUsuarioDAL.EliminarUsuario(idUsuario);
        }

    }
}
