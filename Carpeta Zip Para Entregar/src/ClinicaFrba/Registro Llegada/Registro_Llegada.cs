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
using ClinicaFrba.Dominio;

namespace ClinicaFrba.Registro_Llegada
{
    public partial class Registro_Llegada : Form
    {
        DataTable tabla;
        int bono;

        public Registro_Llegada()
        {
            InitializeComponent();
            llenarComboBox();
            button4.Enabled = false;
            button5.Enabled = false;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoResizeColumns();
            dataGridView1.ReadOnly = true;
            dataGridView1.MultiSelect = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        // TRAE LOS TURNOS DEL AFILIADO
        private void button3_Click(object sender, EventArgs e)
        {
            if (validarEntrada())
            {
                SqlConnection cn = (new BDConnection()).getInstance();
                SqlCommand cm = new SqlCommand("DREAM_TEAM.getTurnos", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@af_id", getID());
                cm.Parameters.AddWithValue("@af_rel_id", getRelID());
                cm.Parameters.AddWithValue("@esp_id", getEspID());
                cm.Parameters.AddWithValue("@prof_apellido",textBox1.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cm);
                tabla = new DataTable();
                sda.Fill(tabla);
                dataGridView1.DataSource = tabla;
                dataGridView1.ClearSelection();

            }
        }

        // SELECCIONAR BONO PARA LA ATENCION
        private void button4_Click(object sender, EventArgs e)
        {
            SeleccionarBono form = new SeleccionarBono(getID(), getRelID());
            DialogResult res = form.ShowDialog();
            if (res == DialogResult.OK)
            {
                bono = form.bono;
                button5.Enabled = true;
                textBox2.Enabled = false;
            }
        }

        // GENERAR CONSULTA MEDICA
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                checkearBono();
                int turno = getTurno();
                SqlConnection cn = (new BDConnection()).getInstance();
                SqlCommand cm = new SqlCommand("DREAM_TEAM.generateConsultaMedica", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@turno_id", turno);
                cm.Parameters.AddWithValue("@bono_id", bono);
                cm.Parameters.AddWithValue("@hora_llegada", Program.nuevaFechaSistema());
                cm.Parameters.AddWithValue("@af_id", getID());
                cm.Parameters.AddWithValue("@af_rel_id", getRelID());
                cm.ExecuteNonQuery();
                cm.Dispose();
                Close();
                MessageBox.Show("Consulta generada con éxito", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // LLENAR EL COMBO BOX PARA LA BUSQUEDA DEL TURNO
        private void llenarComboBox()
        {
            SqlConnection cn = (new BDConnection()).getInstance();
            SqlCommand cm = new SqlCommand("DREAM_TEAM.getEspecialidadesMedicas", cn);
            cm.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(new Item(dr.GetString(1), Int32.Parse(dr.GetValue(0).ToString())));
            }
            dr.Close();
            dr.Dispose();
            cm.Dispose();
        }

        private bool validarEntrada()
        {
            bool flag = true;
            int n;
            if (!int.TryParse(textBox2.Text, out n))
            {
                MessageBox.Show("No se ha ingresado un número de afiliado válido.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                flag = false;
                
            }
            if (flag && comboBox1.SelectedIndex == -1 && (textBox1.Text.Length == 0 || !textBox1.Text.All(d => Char.IsLetter(d))))
            {
                MessageBox.Show("Datos invalidos para la búsqueda del turno.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                flag = false;
            }
            return flag;
        }

        public long getID()
        {
            long ret;
            ret = long.Parse(textBox2.Text) / 100;
            return ret;
        }

        public int getRelID()
        {
            int ret;
            ret = int.Parse(textBox2.Text) % 100;
            return ret;
        }

        public int getEspID()
        {
            int ret;
            try
            {
                ret = ((Item)comboBox1.SelectedItem).Value;
            }
            catch (Exception e)
            {
                ret = -1;
            }
            return ret;
        }

        public int getTurno()
        {
            int ret;
            int index;
            index = dataGridView1.CurrentCell.RowIndex;
            ret = int.Parse(tabla.Rows[index][0].ToString());
            return ret;
        }

        //CHECKEA QUE EL BONO SEA VÁLIDO PARA LA ATENCION.
        public void checkearBono()
        {
            SqlConnection cn = (new BDConnection()).getInstance();
            SqlCommand cm = new SqlCommand("DREAM_TEAM.checkBono", cn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddWithValue("@bono_id", bono);
            cm.Parameters.AddWithValue("@turno_id", getTurno());
            cm.ExecuteNonQuery();
            cm.Dispose();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button4.Enabled = true;
        }

     }
}
