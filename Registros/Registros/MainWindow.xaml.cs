using System;
using System.Collections.Generic;
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
using System.Data.OleDb;//Agregamos libreria OleDB
using System.Data; //Agregamos System.Data

namespace Registros
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexión
        DataTable dt;   //Agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            //Conectamos la base de datos a nuestro archivo Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Registros.mdb";
            MostrarDatos();
        }

        //Mostramos los registros de la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Playeras";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Limpiamos todos los campos
        private void LimpiaTodo()
        {
            txtNped.Text = "";
            txtNombre.Text = "";
            cbGenero.SelectedIndex = 0;
            cbPlayera.SelectedIndex = 0;
            cbTallaPl.SelectedIndex = 0;
            txtTelefono.Text = "";
            btnNuevo.Content = "Nuevo";
            txtNped.IsEnabled = true;
        }
        //Editamos alumnos existentes
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtNped.Text = row["Numero_Pedido"].ToString();
                txtNombre.Text = row["Nombre"].ToString();
                cbGenero.Text = row["Genero"].ToString();
                cbPlayera.Text = row["Playera"].ToString();
                cbTallaPl.Text = row["TallaPl"].ToString();
                txtTelefono.Text = row["Telefono"].ToString();
                txtNped.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar a un alumno de la lista...");
            }
        }

        //Eliminamos Alumnos
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from Playeras where Numero_Pedido=" + row["Numero_Pedido"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Pedido Eliminado correctamenta.");
                LimpiaTodo();
            }
            else
            {
                MessageBox.Show("Favor de seleccionar a un alumno de la lista...");
            }
        }
        //Salimos de la aplicación
        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Agregamos nuevos alumnos
        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtNped.Text != "")
            {
                if (txtNped.IsEnabled == true)
                {
                    if (cbGenero.Text != "Selecciona Genero")
                    {
                        if (cbGenero.IsEnabled == true)
                        {
                            if (cbPlayera.Text != "Selecciona tipo de playera")
                            {
                                if (cbPlayera.IsEnabled == true)
                                {
                                    cmd.CommandText = "insert into Playeras(Numero_Pedido,Nombre,Genero,Playera,TallaPl,Telefono) " +
                                    "Values(" + txtNped.Text + ",'" + txtNombre.Text + "','" + cbGenero.Text + "', '" + cbPlayera.Text + "', '" + cbTallaPl.Text + "', '" + cbTallaPl.Text + "', " + txtTelefono.Text + ")";
                                    cmd.ExecuteNonQuery();
                                    MostrarDatos();
                                    MessageBox.Show("Pedido agregado correctamente.");
                                    LimpiaTodo();
                                }

                            }
                            else
                            {
                                MessageBox.Show("Llena todos los campos.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Llena todos los campos.");
                    }
                }
                else
                {
                    cmd.CommandText = "update Playeras set Nombre='" + txtNombre.Text + "',Genero='" + cbGenero.Text + "',Playera='" + cbPlayera.Text + "',TallaPl='"
                        + cbTallaPl.Text + "',Telefono=" + txtTelefono.Text + " where Numero_Pedido=" + txtNped.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos del pedido Actualizados.");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Favor de poner el numero de Pedido.");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }
    }
}
