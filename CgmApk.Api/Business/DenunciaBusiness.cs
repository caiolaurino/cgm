using CgmApk.Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgmApk.Api.Business
{
    public class DenunciaBusiness
    {
        public void SalvarUpload(string email, string autor, string caminho, string conteudo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ContextoCgm"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("prcCriaDenuncia", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Conteudo", conteudo);
                command.Parameters.Add("@Caminho", caminho);
                command.Parameters.Add("@Autor", autor);
                command.Parameters.Add("@Email", email);

                try
                {
                    List<DenunciaDto> listaRetorno = new List<DenunciaDto>();

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<DenunciaDto> Listar()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ContextoCgm"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("prcListaDenuncia", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                try
                {
                    List<DenunciaDto> listaRetorno = new List<DenunciaDto>();

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DenunciaDto dto = new DenunciaDto();
                        dto.Id = (int)reader[0];
                        dto.Conteudo = reader.IsDBNull(1) ? "empty" : reader[1].ToString();
                        dto.Data = reader.IsDBNull(2) ? "01/01/2016" : reader[2].ToString();
                        dto.Autor = reader.IsDBNull(3) ? "Anônimo" : reader[3].ToString();
                        dto.CaminhoAnexo = reader.IsDBNull(4) ? "" : reader[4].ToString();
                        dto.EmailAutor = reader.IsDBNull(5) ? "Anônimo" : reader[5].ToString();

                        listaRetorno.Add(dto);
                    }
                    reader.Close();

                    return listaRetorno;
                }
                catch (Exception ex)
                {
                    throw ex;
                }



            }
        }

        public DenunciaDto Obter(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ContextoCgm"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("prcObterDenuncia", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@IdNoticia", id);

                try
                {
                    DenunciaDto dto = new DenunciaDto();
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dto.Id = (int)reader[0];
                        dto.Conteudo = reader.IsDBNull(1) ? "empty" : reader[1].ToString();
                        dto.Data = reader.IsDBNull(2) ? "01/01/2016" : reader[2].ToString();
                        dto.Autor = reader.IsDBNull(3) ? "Anônimo" : reader[3].ToString();
                        dto.CaminhoAnexo = reader.IsDBNull(4) ? "" : reader[4].ToString();
                        dto.EmailAutor = reader.IsDBNull(5) ? "Anônimo" : reader[5].ToString();

                    }
                    reader.Close();

                    return dto;
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
        }


        public void Teste()
        {
            //string connectionString ="Data Source=(local);Initial Catalog=Northwind; Integrated Security=true";

            string connectionString = ConfigurationManager.ConnectionStrings["ContextoCgm"].ConnectionString;

            // Provide the query string with a parameter placeholder.
            //string queryString =
            //        "SELECT ProductID, UnitPrice, ProductName from dbo.products "
            //        + "WHERE UnitPrice > @pricePoint "
            //        + "ORDER BY UnitPrice DESC;";

            string queryString = "SELECT * from lobt_cgmSQL..Tb_Denuncia";

            // Specify the parameter value.
            int paramValue = 5;

            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);
                //command.Parameters.AddWithValue("@pricePoint", paramValue);

                // Open the connection in a try/catch block. 
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2}",
                            reader[0], reader[1], reader[2]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ;
            }
        }

        public void TesteProcedure()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ContextoCgm"].ConnectionString;



            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("prcListaDenuncia", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                //command.Parameters.AddWithValue("@pricePoint", paramValue);

                // Open the connection in a try/catch block. 
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2}",
                            reader[0], reader[1], reader[2]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ;
            }
        }
    }
}
