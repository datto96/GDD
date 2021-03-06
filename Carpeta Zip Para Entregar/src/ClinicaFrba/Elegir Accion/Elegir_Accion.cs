﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ClinicaFrba.Elegir_Accion
{
    public partial class Elegir_Accion : Form
    {
        int rol, us_idG;
        public Elegir_Accion(int rol_id, int us_id)
        {
            InitializeComponent();
            rol = rol_id;
            us_idG = us_id;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            llenarComboBox(rol_id);
        }

        // TRAE LAS FUNCIONALDIADES DEL ROL
        public void llenarComboBox(int rol_id)
        {
            SqlConnection conn = (new BDConnection()).getInstance();
            string query = "DREAM_TEAM.getFuncionalidadDelRol";
            SqlCommand com = new SqlCommand(query, conn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@id_rol",rol_id));
            try
            {
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr.GetString(1));
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }
        // SELECCION DE ACCION
        private void button1_Click(object sender, EventArgs e)
        {
                String texto = comboBox1.Text;
                switch (texto)
                {
                    case "ABM de Rol":
                        // ABM ROL
                        AbmRol.ABMRol f1 = new AbmRol.ABMRol();
                        f1.Show();
                        break;

                    case "ABM de Afiliado":
                        // ABM AFILIADO
                        BuscarAfiliado.BuscarAfi f2 = new BuscarAfiliado.BuscarAfi(rol);
                        f2.Show();
                        break;

                    case "Registrar Agenda Profesional":
                        // REGISTRAR AGENDA PROFESIONAL
						Registrar_Agenta_Medico.Registrar_agenda f3 = new Registrar_Agenta_Medico.Registrar_agenda(us_idG);
                        f3.Show();
                        break;

                    case "Compra de Bonos":
                        // COMPRA DE BONOS
                        switch (rol)
                        {                           
                            case 2:
                                Compra_Bono.Compra_Bono f5 = new Compra_Bono.Compra_Bono(us_idG);
                                f5.Show();
                                break;
                            default:
                                Compra_Bono.Compra_Administrador f4 = new Compra_Bono.Compra_Administrador();
                                f4.Show();
                                break;
                        }
                        break;
                        
                    case "Pedido de Turno":
                        // PEDIDO DE TURNO
                        Pedir_Turno.ListadoProfesionales f6 = new Pedir_Turno.ListadoProfesionales(us_idG);
                        f6.Show();
                        break;

                    case "Registro de Llegada":
                        // REGISTRO DE LLEGADA
                        Registro_Llegada.Registro_Llegada f7 = new Registro_Llegada.Registro_Llegada();
                        f7.Show();
                        break;

                    case "Registro de Resultado":
                        // REGISTRO DE RESULTADO
                        Registro_Resultado.Reg_Res f8 = new Registro_Resultado.Reg_Res();
                        f8.Show();
                        break;

                    case "Cancelar Atencion Medica":
                        // CANCELAR ATENCION MEDICA
                        switch (rol)
                        {
                            case 2:
                                Cancelar_Atencion.TurnCancelAfiliado f9 = new Cancelar_Atencion.TurnCancelAfiliado(us_idG);
                                f9.Show();
                                break;
                            case 3:
                                Cancelar_Atencion.TurnCancelProfesional f10 = new Cancelar_Atencion.TurnCancelProfesional(us_idG);
                                f10.Show();
                                break;
                        }
                        break;

                    case "Listado Estadistico":
                        // LISTADO ESTADISTICO
                        Listados.Presentacion_Listados f11 = new Listados.Presentacion_Listados();
                        f11.Show();
                        break;

                    default:
                        MessageBox.Show("No se ha seleccionado ninguna accion", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
            }
            
        }
    }
}
