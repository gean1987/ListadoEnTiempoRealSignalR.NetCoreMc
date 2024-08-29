using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AplicacionTiempoReal.Models;
using AplicacionTiempoReal.Datos;

namespace AplicacionTiempoReal.DAO
{
    public class DAO_Matricula
    {
        private readonly string _connectionString;

        public DAO_Matricula(ConexionDB conexionDB)
        {
            _connectionString = conexionDB.getCadenaSQL();
        }

        public async Task<List<Alumnos>> GetAllAsync()
        {
            var products = new List<Alumnos>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Alumnos", conn))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new Alumnos
                                {
                                    AlumnoId = reader.GetInt32(0),
                                    nombre = reader.GetString(1),
                                    apellido = reader.GetString(2),
                                    correo = reader.GetString(3),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la lista de alumnos: {ex.Message}");
            }

            return products;
        }

    }
}
