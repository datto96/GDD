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

namespace ClinicaFrba.Compra_Bono
{
    public partial class Confirmacion_Compra : Form
    {
        long idP;
        int idR;
        public Confirmacion_Compra(long id, int id_rel, int planMedico, int cantidadBonos)
        {
            InitializeComponent();
            idP = id;
            idR = id_rel;
            setTextBoxs(id,planMedico,cantidadBonos);
            calcularValor(planMedico, cantidadBonos);
        }

        public void setTextBoxs(long id, int planMedico, int cantidadBonos)
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox1.Text = String.Format(id.ToString());
            textBox2.Text = String.Format(planMedico.ToString());
            textBox3.Text = String.Format(cantidadBonos.ToString());
        }

        public void calcularValor(int planMedico, int cantidadBonos)
        {
            float precio = getValorDelPlan(planMedico);
            textBox4.Text = precio.ToString("0.00");
            textBox6.Text = (cantidadBonos * precio).ToString("0.00");
        }

        public float getValorDelPlan(int planMedico)
        {
            float precio;
            SqlConnection conn = (new BDConnection()).getInstance();
            string query = String.Format("DREAM_TEAM.getPrecioBonoDelPlan");
            SqlCommand com = new SqlCommand(query, conn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@planmed_id", planMedico);
            precio = float.Parse(com.ExecuteScalar().ToString());
            com.Dispose();
            return precio;
        }

        // REALIZA LA COMPRA DE BONOS
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = (new BDConnection()).getInstance();
            string query = String.Format("DREAM_TEAM.comprarBonos");
            SqlCommand com = new SqlCommand(query, conn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@af_id" , idP);
            com.Parameters.AddWithValue("@af_rel_id" , idR);
            com.Parameters.AddWithValue("@cantidad" , Int32.Parse(textBox3.Text));
            com.Parameters.AddWithValue("@monto" , float.Parse(textBox6.Text));
            com.Parameters.AddWithValue("@fecha", DateTime.Parse(Program.nuevaFechaSistema()));
            SqlDataReader dr = com.ExecuteReader();
            mostrarBonos(dr);
            com.Dispose();
            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        void mostrarBonos(SqlDataReader dr)
        {
            string mensaje = "Los códigos de bonos comprados son: ";
            dr.Read();
            mensaje += dr[0].ToString();
            while (dr.Read())
            {
                mensaje += String.Format(",{0} ", dr[0].ToString());
            }
            mensaje += '.';
            MessageBox.Show(mensaje, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
