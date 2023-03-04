using CapaEntidad;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace CapaDatos
{
    public class UsuarioDAL
    {
        public UsuarioCLS Login(string usuario, string contrasena)
        {

            UsuarioCLS oUsuario = null;

            var cn = new ConexionDAL();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("spLogin", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);
                using (var dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        oUsuario = new UsuarioCLS()
                        {
                            nombreusuario = dr["usuario"].ToString(),
                            idUsuario = Convert.ToInt32(dr["IdUsuario"].ToString()),
                            clave = dr["clave"].ToString(),
                            correo = dr["correo"].ToString(),
                            rol = dr["rol"].ToString(),
                            activo = bool.Parse(dr["activo"].ToString())
                        };

                    }
                }
            }

            return oUsuario;
        }
        public List<UsuarioCLS> ListarUsuarios()
        {
            var oLista = new List<UsuarioCLS>();

            var cn = new ConexionDAL();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("spListarUsuarios", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        oLista.Add(new UsuarioCLS()
                        {
                            idUsuario = Convert.ToInt32( dr["IdUsuario"].ToString()),
                            nombreusuario = dr["usuario"].ToString(),
                            clave = dr["clave"].ToString(),
                            correo = dr["correo"].ToString(),
                            rol = dr["rol"].ToString(),
                            activo = bool.Parse(dr["activo"].ToString())
                        });

                    }
                }
            }

            return oLista;
        }

        public UsuarioCLS ObtenerUsuario(int idUsuario)
        {
            UsuarioCLS oUsuario = null;

            var cn = new ConexionDAL();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("spObtenerUsuario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                using (var dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        oUsuario = new UsuarioCLS()
                        {
                            nombreusuario = dr["usuario"].ToString(),
                            clave = dr["clave"].ToString(),
                            correo = dr["correo"].ToString(),
                            rol = dr["rol"].ToString(),
                            activo = Convert.ToBoolean(dr["activo"].ToString())
                        };

                    }
                }
            }

            return oUsuario;
        }

        public int ActualizarUsuario(UsuarioCLS oUsuario)
        {
            int respuesta = 0;
            var cn = new ConexionDAL();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("UPDATE USUARIO  SET  NombreUsuario = @NombreUsuario,   Rol = @Rol,    Activo = @Activo WHERE IdUsuario = @IdUsuario ", conexion);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", oUsuario.idUsuario);
                cmd.Parameters.AddWithValue("@NombreUsuario", oUsuario.nombreusuario);
                cmd.Parameters.AddWithValue("@Rol", oUsuario.rol);
                cmd.Parameters.AddWithValue("@Activo", oUsuario.activo);

                respuesta = cmd.ExecuteNonQuery();
            }

            return respuesta;
        }

        public int CambiarContrasena(UsuarioCLS oUsuario)
        {
            int respuesta = 0;
            var cn = new ConexionDAL();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("UPDATE USUARIO  SET  Clave = @clave WHERE IdUsuario = @IdUsuario ", conexion);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", oUsuario.idUsuario);
                cmd.Parameters.AddWithValue("@clave", oUsuario.clave);

                respuesta = cmd.ExecuteNonQuery();
            }

            return respuesta;
        }

        public int GuardarUsuario(UsuarioCLS oUsuario)
        {
            int respuesta = 0;
            var cn = new ConexionDAL();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO USUARIO(NombreUsuario,Correo,Clave,Rol,Activo) Values(@NombreUsuario, @Correo, @Clave, @Rol, @Activo) ", conexion))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreUsuario", oUsuario.nombreusuario);
                    cmd.Parameters.AddWithValue("@Correo", oUsuario.correo);
                    cmd.Parameters.AddWithValue("@Clave", oUsuario.clave);
                    cmd.Parameters.AddWithValue("@Rol", oUsuario.rol);
                    cmd.Parameters.AddWithValue("@Activo", oUsuario.activo);
                    respuesta = cmd.ExecuteNonQuery();
                }
            }

            return respuesta;
        }

        public int EliminarUsuario(int idUsuario)
        {
            int respuesta = 0;
            var cn = new ConexionDAL();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("spEliminarUsuario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                respuesta = cmd.ExecuteNonQuery();
            }

            return respuesta;
        }
    }
}
