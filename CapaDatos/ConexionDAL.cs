using Microsoft.Extensions.Configuration;

namespace CapaDatos
{
    public class ConexionDAL
    {
        private string cadenaSQL = string.Empty;

        public ConexionDAL()
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            cadenaSQL = builder.GetSection("ConnectionStrings:CadenaSQL").Value;
        }

        public string getCadenaSQL()
        {
            return cadenaSQL;
        }
    }
}
