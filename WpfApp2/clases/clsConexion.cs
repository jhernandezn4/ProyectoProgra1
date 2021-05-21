using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace WpfApp2.clases
{
    class clsConexion
    {
        private string servidor = "localhost"; //Nombre o ip del servidor de MySQL
        private string bd = "tienda"; //Nombre de la base de datos
        private string usuario = "root"; //Usuario de acceso a MySQL
        private string password = ""; //Contraseña de usuario de acceso a MySQL
        private string datos = null; //Variable para almacenar el resultado
        private bool iniciado = false;
        MySqlConnection conexionBD;
        MySqlDataReader reader;
        public clsConexion()
        {
            this.iniciar();
        }
        private void iniciar()
        {
            if (!iniciado)
            {
                //Crearemos la cadena de conexión concatenando las variables
                string cadenaConexion = "Database=" + bd + "; Data Source=" + servidor + "; User Id=" + usuario + "; Password=" + password + "";
                //Instancia para conexión a MySQL, recibe la cadena de conexión
                conexionBD = new MySqlConnection(cadenaConexion);
                this.iniciado = true;
            }
        }
        private DataTable ConsultaSimple(string consulta, int columna = 0)
        {

            try
            {
                MySqlCommand comando = new MySqlCommand(consulta); //Declaración SQL para ejecutar contra una base de datos MySQL
                comando.Connection = conexionBD; //Establece la MySqlConnection utilizada por esta instancia de MySqlCommand
                conexionBD.Open(); //Abre la conexión

                DataTable dt = new DataTable();
                dt.Load(comando.ExecuteReader());
                return dt;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                DataTable dt = new DataTable();
                return dt;
            }
            finally
            {
                conexionBD.Close(); //Cierra la conexión a MySQL
            }
        }

        public DataTable MostrarTodos()
        {
            this.iniciar();
            string consulta = "select * from productos";
            return this.ConsultaSimple(consulta);

        }
        public DataTable Buscar(string parametro)
        {
            this.iniciar();
            string consulta = $"select * from productos where nombre like '%{parametro}%' or costo='{parametro}' or precio='{parametro}'  or existencias='{parametro}' or ganancia_unitaria='{parametro}' or ganancia_total='{parametro}'";
            return this.ConsultaSimple(consulta);

        }
        public DataTable Insertar(string nombre, string costo, string precio, string existencia)
        {
            decimal ganancia_unitaria = Convert.ToDecimal(precio) - Convert.ToDecimal(costo);
            decimal ganancia_total = ganancia_unitaria * Convert.ToDecimal(existencia);
            string consulta = $"insert into productos (id, nombre, costo, precio, existencias, ganancia_unitaria, ganancia_total) VALUES ('0','{nombre}', '{costo}', '{precio}', '{existencia}', '{ganancia_unitaria}','{ganancia_total}');";
            this.ConsultaSimple(consulta);
            return this.MostrarTodos();
        }

        public DataTable Actualizar (string id,string nombre, string costo, string precio, string existencia)
        {
            decimal ganancia_unitaria = Convert.ToDecimal(precio) - Convert.ToDecimal(costo);
            decimal ganancia_total = ganancia_unitaria * Convert.ToDecimal(existencia);
            string consulta = $"Update productos set nombre='{nombre}',  costo='{costo}', precio='{precio}', existencias='{existencia}', ganancia_unitaria='{ganancia_unitaria}', ganancia_total='{ganancia_total}' where id={id}";
            this.ConsultaSimple(consulta);
            return this.MostrarTodos();
        }

        public DataTable Eliminar(string id)
        {
          
            string consulta = $"Delete from productos  where id={id}";
            this.ConsultaSimple(consulta);
            return this.MostrarTodos();
        }




    }
}
