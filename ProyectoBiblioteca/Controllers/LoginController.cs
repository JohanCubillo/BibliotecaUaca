using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace ProyectoBiblioteca.Controllers
{
    public class LoginController : Controller
    {

        private Log log = new Log();
        // GET: Login
        public ActionResult Index()
        {
            return View();
             
    }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
           

        Persona ousuario = PersonaLogica.Instancia.Listar().Where(u => u.Correo == correo && u.Clave == clave && u.oTipoPersona.IdTipoPersona != 5 && u.Estado == true).FirstOrDefault();

            if (ousuario == null)
            {
                log.AddEntry(new LogEntry { Timestamp = DateTime.Now, Username = correo, Level = "WARN", Message = "Inicio de sesión fallido" });
                log.InsertarDatosEnBD(DateTime.Now, correo,"WARN", "hola");


                ViewBag.Error = "Usuario o contraseña no correcta";
                return View();
               
            }
            else
            {
                Session["Usuario"] = ousuario;
                log.AddEntry(new LogEntry { Timestamp = DateTime.Now, Username = correo, Level = "SUSS", Message = "Inicio de sesión exitoso" });
                log.InsertarDatosEnBD(DateTime.Now, correo, "SUSS", "Inicio de sesión exitoso");
                return RedirectToAction("Index", "Admin");
            }
            

        }

       



        public ActionResult MostrarDatos()
        {
            string connectionString = "Data Source=DESKTOP-P0DH32H;Initial Catalog=DB_BIBLIOTECA;Integrated Security=True";
            List<LogEntry> logi = new List<LogEntry>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM logss";

                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DateTime Time = (DateTime)reader["time"];
                    string Usuario = (string)reader["usuario"];
                    string Nivel = (string)reader["nivel"];
                    string Messajes = (string)reader["messaje"];

                    logi.Add(new LogEntry {  Timestamp = Time, Username = Usuario, Level = Nivel, Message = Messajes });
                }

                reader.Close();
            }

            return View(logi);
        }
    }


    






}