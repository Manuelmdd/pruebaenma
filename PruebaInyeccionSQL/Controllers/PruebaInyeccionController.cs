using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PruebaInyeccionSQL.Controllers
{
    [ApiController]
    [Route("inyeccionsql")]
    public class PruebaInyeccionController : ControllerBase
    {
        IConfiguration configuration;
        public PruebaInyeccionController(IConfiguration configuracion)
        {
                configuration = configuracion;
        }

        [HttpGet("Get-Performance")]
        public async Task<ActionResult> mostrarciudad(int performance_id)
        {
            string? cadenaBD = configuration.GetConnectionString("ConxTest");
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaBD))
                {
                    await conexion.OpenAsync();
                   
                    using (SqlCommand comando = new SqlCommand("SELECT name FROM TestPerformance WHERE PerformanceId = @PerformanceId", conexion))
                    {
                        comando.Parameters.AddWithValue("@PerformanceId", performance_id);

                        using (SqlDataReader reader = await comando.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                // Aquí puedes leer los valores de las columnas y mapearlos a tu modelo TestPerformance
                                string nombre = "";
                                
                               nombre= reader.GetString(0);
                                    // Otros campos de tu entidad
                                

                                return Ok(nombre);
                            }
                            else
                            {
                                string nombre = "error en la sql";

                                
                                return NotFound(nombre);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                string error = "error al conectar en la bd";
                return NotFound(error);
            }

        
        }
        [HttpGet("versionHLeon")]
        public async Task<ActionResult> prueba2(string id_perfor)
        {
            string? cadenaBD = configuration.GetConnectionString("ConxTest");
            string query = "select name FROM TestPerformance WHERE PerformanceId=('"+id_perfor+"')";
            using(SqlConnection conexion = new SqlConnection( cadenaBD)) 
            {
                SqlCommand comando = new SqlCommand(query,conexion);
                await conexion.OpenAsync();

                await comando.ExecuteNonQueryAsync();

                return Ok("exito");
            }
        }
       
    }
}
