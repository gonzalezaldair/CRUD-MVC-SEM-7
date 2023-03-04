namespace CapaEntidad
{
    public class UsuarioCLS
    {
        public int idUsuario { get; set; }
        public string nombreusuario { get; set; }
        public string? clave { get; set; }
        public string? correo { get; set; }
        public string? rol { get; set; }
        public bool? activo { get; set; }

    }
}
