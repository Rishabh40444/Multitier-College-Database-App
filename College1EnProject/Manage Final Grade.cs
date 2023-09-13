using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace College1EnProject
{
    public partial class Manage_Final_Grade : Form
    {
        public static Manage_Final_Grade current;
        public Manage_Final_Grade()
        {
            current = this;
            InitializeComponent();
            
        }

        private void Manage_Final_Grade_Load(object sender, EventArgs e)
        {
            txtCourseID.ReadOnly = true;
            txtCourseName.ReadOnly = true;
            txtFinalGrade.ReadOnly = false;
            txtStudentID.ReadOnly = true;
            txtStudentName.ReadOnly = true;
        }

        internal static void Start(string operation,DataGridViewSelectedRowCollection rows)
        {
           
            
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("The changes which you made will not be saved!\n " +
                "Do you Wish to Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == (DialogResult.Yes))
            {

                this.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you want to make changes in the final Grade?","Question",MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == (DialogResult.OK))
            {
                MessageBox.Show("Changes are Saved!");
                this.Visible = false;
            }

        }





    }
}
