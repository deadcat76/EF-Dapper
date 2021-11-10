using BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winforms
{
    public partial class Form1 : Form
    {
        Logic logic = new Logic();
      
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logic.AddEmployee(textBox1.Text, Int32.Parse(textBox2.Text), Int32.Parse(textBox3.Text), Int32.Parse(textBox8.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var age = !String.IsNullOrEmpty(textBox2.Text) ? Int32.Parse(textBox2.Text) : 0;
            var salary = !String.IsNullOrEmpty(textBox3.Text) ? Int32.Parse(textBox3.Text) : 0;
            logic.UpdateEmployee((Int32.Parse(textBox4.Text), textBox1.Text, age, salary));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            logic.DeleteEmployee(Int32.Parse(textBox4.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            var employees = logic.GetEmployees();
            foreach ((int, string, int, int, int?) employee in employees)
            {
                dataGridView1.Rows.Add(employee.Item1.ToString(), employee.Item2, employee.Item3.ToString(), employee.Item4.ToString(), employee.Item5.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var ChoosingEmployeesString = textBox7.Text.Split(' ');
            var ChoosingEmployees = new int[ChoosingEmployeesString.Length];
            for (int i = 0; i < ChoosingEmployeesString.Length; i++)
            {
                ChoosingEmployees[i] = Int32.Parse(ChoosingEmployeesString[i]);
            }
            logic.AddDepartment(textBox6.Text, ChoosingEmployees);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            logic.DeleteDepartment(Int32.Parse(textBox5.Text));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var departments = logic.GetDepartaments();
            string Result = "";
            foreach ((int, string, List<(int, string, int, int)>) department in departments)
            {
                Result += department.Item1 + " " + department.Item2 + ": \n ";
                foreach ((int, string, int, int) employee in department.Item3)
                {
                    Result += "\t" + employee.Item1 + " " + employee.Item2 + " " + employee.Item3 + " " + employee.Item4 + "\n ";
                }

            }
            MessageBox.Show(Result);
        }
    }
}
