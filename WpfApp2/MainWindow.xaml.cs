using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.clases;

namespace WpfApp2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        clsConexion conexion;
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void IniciarConexion(object sender, RoutedEventArgs e)
        {
            this.conexion = new clsConexion();
            DataTable datos = this.conexion.MostrarTodos();
            this.gridResultados.ItemsSource = datos.DefaultView;
        }
        private void nombre_TextChanged()
        {

        }

        private void nombre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.btnGuardar.Content = "Actualizar";
            DataRowView row = this.gridResultados.SelectedItem as DataRowView;
            if (row !=null)
            {
                this.txtCodigo.Text = row.Row.ItemArray[0].ToString();
                this.txtNombre.Text = row.Row.ItemArray[1].ToString();
                this.txtCosto.Text = row.Row.ItemArray[2].ToString();
                this.txtPrecio.Text = row.Row.ItemArray[3].ToString();
                this.txtExistencias.Text = row.Row.ItemArray[4].ToString();
                this.btnEliminar.IsEnabled = true;
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string parametro = this.txtBuscar.Text;
            DataTable datos;
            if (parametro == "")
            {
                datos = this.conexion.MostrarTodos();
            }
            else
            {
                datos = this.conexion.Buscar(parametro);
            }
            
            this.gridResultados.ItemsSource = datos.DefaultView;
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            this.btnGuardar.Content = "Guardar";
            this.txtCodigo.Text = "Nuevo";
            this.txtNombre.Text = "";
            this.txtCosto.Text = "";
            this.txtPrecio.Text = "";
            this.txtExistencias.Text = "";
            this.btnEliminar.IsEnabled = false;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtCodigo.Text == "Nuevo")
            {
                this.conexion.Insertar(this.txtNombre.Text, this.txtCosto.Text, this.txtPrecio.Text, this.txtExistencias.Text);
            }
            else
            {
                this.conexion.Actualizar(this.txtCodigo.Text,this.txtNombre.Text, this.txtCosto.Text, this.txtPrecio.Text, this.txtExistencias.Text);
            }
            DataTable datos = this.conexion.MostrarTodos();
            this.gridResultados.ItemsSource = datos.DefaultView;
            this.btnNuevo.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));///se hace click en el boton nuevo para limpar todos los campos
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.conexion.Eliminar(this.txtCodigo.Text);
            DataTable datos = this.conexion.MostrarTodos();
            this.gridResultados.ItemsSource = datos.DefaultView;
            this.btnNuevo.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));///se hace click en el boton nuevo para limpar todos los campos
        }
    }
}
