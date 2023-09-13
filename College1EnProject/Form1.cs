using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace College1EnProject
{
    public partial class Form1 : Form
    {
        internal enum Grids
        {
            Programs,Courses,Students,Enrollments
        }
        private Grids grid;

        public static Form1 current;
        public Form1()
        {
            current = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Add_or_Modify_Rows();
            Add_or_Modify_Rows.current.Visible = false;

            dataGridView1.Dock = DockStyle.Fill;
        }

        private void programsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Programs;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource1.DataSource = Data.Programs.GetPrograms();
            bindingSource1.Sort = "ProgId";
            dataGridView1.DataSource = bindingSource1;

            dataGridView1.Columns["ProgId"].DisplayIndex = 0;
            dataGridView1.Columns["ProgName"].HeaderText = "Program Name";
            dataGridView1.Columns["ProgName"].DisplayIndex = 1;
        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Courses;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource2.DataSource = Data.Courses.GetCourses();
            bindingSource2.Sort = "CId";
            dataGridView1.DataSource = bindingSource2;

            dataGridView1.Columns["CId"].DisplayIndex = 0;
            dataGridView1.Columns["CName"].HeaderText = "Course Name";
            dataGridView1.Columns["CName"].DisplayIndex = 1;
            dataGridView1.Columns["ProgId"].DisplayIndex = 2;
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Students;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource3.DataSource = Data.Students.getStudents();
            bindingSource3.Sort = "StId";
            dataGridView1.DataSource = bindingSource3;

            dataGridView1.Columns["StId"].DisplayIndex = 0;
            dataGridView1.Columns["StName"].HeaderText = "Student Name";
            dataGridView1.Columns["StName"].DisplayIndex = 1;
            dataGridView1.Columns["ProgId"].DisplayIndex = 2;

        }
        private void enrollmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(grid != Grids.Enrollments)
            {
                grid = Grids.Enrollments;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource4.DataSource = Data.Enrollments.getDisplayEnrollments();
                bindingSource4.Sort = "StId";
                bindingSource4.Sort = "CId";
                dataGridView1.DataSource = bindingSource4;


            }
            

        }
        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            BuisnessLayer.Programs.UpdatePrograms();
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
            BuisnessLayer.Courses.UpdateCourses();
        }

        private void bindingSource3_CurrentChanged(object sender, EventArgs e)
        {
            BuisnessLayer.Students.UpdateStudents();
        }
        private void bindingSource4_CurrentChanged(object sender, EventArgs e)
        {
            //BuisnessLayer.Enrollments.UpdateEnrollments();
        }
        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            BuisnessLayer.Programs.UpdatePrograms();
            BuisnessLayer.Courses.UpdateCourses();
            BuisnessLayer.Students.UpdateStudents();
           // BuisnessLayer.Enrollments.UpdateEnrollments();
        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Impossible to insert, update or delete rows!");
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form1.current.Visible = true;
            //Add_or_Modify_Rows.current.ShowDialog();
            Add_or_Modify_Rows.current.Text = "Add a Record";
            Add_or_Modify_Rows.current.Start(Add_or_Modify_Rows.Modes.INSERT, null);
        }

        private void ModifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Form1.current.Visible =true;
            //Add_or_Modify_Rows.current.ShowDialog();

            Add_or_Modify_Rows.current.Text = "Update or Modify a Record";
            DataGridViewSelectedRowCollection rows = dataGridView1.SelectedRows;
            
            if(rows.Count == 0)
            {
                MessageBox.Show("Please select a row in the table for modification");
            }
            else if (rows.Count > 1)
            {
                MessageBox.Show("Only one row must be selected for Modification");
            }
            else
            {
                Add_or_Modify_Rows.current.Start(Add_or_Modify_Rows.Modes.UPDATE, rows);
            }

        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = dataGridView1.SelectedRows;
            if(rows.Count == 0)
            {
                MessageBox.Show("Please select atleast one row for deletion");
            }
            else
            {
                List<string[]> lId = new List<string[]>();
                for (int i = 0; i < rows.Count; i++)
                {
                    lId.Add(new string[] { ("" + rows[i].Cells["StId"].Value),
                                        ("" + rows[i].Cells["CId"].Value) });
                }
                Data.Enrollments.DeleteData(lId);
            }

        }

        private void ManageFinalGradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form1.current.Visible = true;
            //Manage_Final_Grade.current.ShowDialog();


            Manage_Final_Grade.current.Text = "Add a final Grade for the course";
            DataGridViewSelectedRowCollection rows = dataGridView1.SelectedRows;

            if (rows.Count == 0)
            {
                MessageBox.Show("You need to select a row in order to add a final grade ");
            }
            else if (rows.Count > 1)
            {
                MessageBox.Show("Only one row must be selected for Modification");
            }
            else
            {
                Form1.current.Visible = true;
                Manage_Final_Grade.current.ShowDialog();
                
                
            }

        }

        internal static void BLLMessage(string s)
        {
            MessageBox.Show("Buisness Layer: " + s);
        }
        internal static void DALMessage(string s)
        {
            MessageBox.Show("Data Link Layer : " + s);
        }

       
    }
}

