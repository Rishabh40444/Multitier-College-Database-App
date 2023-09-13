using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace College1EnProject

{
    public partial class Add_or_Modify_Rows : Form
    {
        
        internal enum Modes
        {
            INSERT,UPDATE
        }
        
        
        public static Add_or_Modify_Rows current;

        private Modes mode = Modes.INSERT;

        private string[] enrollInitial;
        public Add_or_Modify_Rows()
        {
            current = this;
            InitializeComponent();
        }

        private void Add_or_Modify_Rows_Load(object sender, EventArgs e)
        {
            // cboCourseID.Items.Add(BindingDataColumn["CId"])

           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("The changes which you made in the table will not be saved.\n " +
                "Do you Wish to Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==(DialogResult.Yes))
            {
                Close();
                //this.Visible = false;
            }
            

        }

        internal void Start(Modes md,DataGridViewSelectedRowCollection rows)
        {
            mode = md;
            Text = "" + mode;

            cboCourseID.DisplayMember = "CId";
            cboCourseID.ValueMember = "CId";
            cboCourseID.DataSource = Data.Courses.GetCourses();
            cboCourseID.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCourseID.SelectedIndex = 0;

            cboStudentID.DisplayMember = "StId";
            cboStudentID.ValueMember = "StId";
            cboStudentID.DataSource = Data.Students.getStudents();
            cboStudentID.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStudentID.SelectedIndex = 0;

            txtCourseName.ReadOnly = true;
            txtStudentName.ReadOnly = true;

            if((mode == Modes.UPDATE) && (rows != null)) {

                cboStudentID.Enabled = false;

                cboCourseID.SelectedValue = rows[0].Cells["CId"].Value;
                cboStudentID.SelectedValue = rows[0].Cells["StId"].Value;
                enrollInitial = new string[] { (string)rows[0].Cells["CId"].Value, (string)rows[0].Cells["StId"].Value};
            }

           

            ShowDialog();

        }
        
        

        private void btnSave_Click(object sender, EventArgs e)
        {
            /* if (MessageBox.Show("Do you want to make changes in the table?", "Question", MessageBoxButtons.OKCancel,
               MessageBoxIcon.Question) == (DialogResult.OK))
             {
                 MessageBox.Show("Changes are Saved!");
                 this.Visible = false;
             }
            */
            





            int result = -1;
            if(mode == Modes.INSERT)
            {
                cboStudentID.Enabled = true;
                result = Data.Enrollments.InsertData(new string[] { (string)cboCourseID.SelectedValue, (string)cboStudentID.SelectedValue, });
            }



            if(mode == Modes.UPDATE)
            {
                List<string[]> lId = new List<string[]>();
                lId.Add(enrollInitial);

                result = Data.Enrollments.InsertData(new string[] { (string)cboCourseID.SelectedValue, (string)cboStudentID.SelectedValue });

                if(result == 0)
                {
                    result = Data.Enrollments.DeleteData(lId);
                }
            }
            if (result == 0)
            {
                Close();
            }

        }

        private void cboStudentID_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cboStudentID.SelectedItem != null)
            {
                var getdata = from a in Data.Students.getStudents().AsEnumerable()
                              where a.Field<string>("StId") == (string)cboStudentID.SelectedValue
                              select new { name = a.Field<string>("StName") };
                txtStudentName.Text = getdata.Single().name;
            }
        }

        private void cboCourseID_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboCourseID.SelectedItem != null)
            {
                var getdata = from a in Data.Courses.GetCourses().AsEnumerable()
                              where a.Field<string>("CId") == (string)cboCourseID.SelectedValue
                              select new { name = a.Field<string>("CName") };
                
                txtCourseName.Text = getdata.Single().name;
            }
            
        }
    }
}

























































































































































