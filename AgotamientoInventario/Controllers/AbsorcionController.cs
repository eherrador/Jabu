using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AgotamientoInventario.Models;

namespace AgotamientoInventario.Controllers
{
    public class AbsorcionController : ApiController
    {
        string conexion = ConfigurationManager.ConnectionStrings["BusinessConnection"].ToString();
         
        List<Proyeccion> proyecciones = new List<Proyeccion>();

        public IEnumerable<Proyeccion> GetProyeccion(int idEstado) //, int idClasificacion)
        {
            using (SqlConnection cnn = new SqlConnection(conexion))
            {
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "usp_Graficas_AgotamientoInventario";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Id_Estado", idEstado));
                cmd.Parameters.Add(new SqlParameter("@Id_Clasificacion", 5));
                //cmd.Parameters.Add(new SqlParameter("@Id_Estado", idEstado));
                //cmd.Parameters.Add(new SqlParameter("@Id_Clasificacion", idClasificacion));

                cnn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Proyeccion proyeccion = new Proyeccion();
                    proyeccion.Absorcion = dr.GetFloat(dr.GetOrdinal("Absorcion"));
                    proyeccion.Meses = dr.GetInt16(dr.GetOrdinal("Meses"));

                    proyecciones.Add(proyeccion);
                }

                return proyecciones;
            }
        }
    }
}
