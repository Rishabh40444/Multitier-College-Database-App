using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Data
{
    class Connect
    {
        // =========================================================================
        // We could use the Design Pattern Singleton for this class. 
        // Howeever, it is also possible (and a little simpler) to 
        // just use static attributes and static methods.
        // =========================================================================

        private static String College1enConnectionString = GetConnectString();

        internal static String ConnectionString { get => College1enConnectionString; }

        private static String GetConnectString()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "College1en";
            cs.UserID = "sa";
            cs.Password = "sysadm";
            return cs.ConnectionString;
        }
    }

    internal class DataTables
    {
        // =========================================================================
        // We could use the Design Pattern Singleton for this class. 
        // Howeever, it is also possible (and a little simpler) to 
        // just use static attributes and static methods.
        // =========================================================================

        private static SqlDataAdapter adapterPrograms = InitadapterPrograms();
        private static SqlDataAdapter adapterCourses = InitadapterCourses();
        private static SqlDataAdapter adapterStudents = InitadapterStudents();
        private static SqlDataAdapter adapterEnrollments = InitadapterEnrollments();
        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitadapterPrograms()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "Select * from Programs order by ProgId",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitadapterCourses()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "select * from Courses order by CId",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }
        private static SqlDataAdapter InitadapterStudents()
        {
            SqlDataAdapter r = new SqlDataAdapter("Select * from Students order by StId", Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();
            return r;
        }
        private static SqlDataAdapter InitadapterEnrollments()
        {
            SqlDataAdapter r = new SqlDataAdapter("Select * from Enrollments order by StId, CId", Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();
            return r;
        }
        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            //MessageBox.Show("point 1");
            loadPrograms(ds);
           // MessageBox.Show("point 2");
            loadCourses(ds);
            //MessageBox.Show("point 3");
            loadStudents(ds);
            //MessageBox.Show("point 4");
            loadEnrollments(ds);
            //MessageBox.Show("point 5");
            return ds;
        }

        private static void loadPrograms(DataSet ds)
        {
            // =========================================================================
            //adapterPrograms.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            // =========================================================================

            adapterPrograms.Fill(ds, "Programs");

            // =========================================================================
            ds.Tables["Programs"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["Programs"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["Programs"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Programs"].Columns["ProgId"]};
            // =========================================================================
        }

        private static void loadCourses(DataSet ds)
        {
            adapterCourses.Fill(ds, "Courses");

            ds.Tables["Courses"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["CName"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["ProgId"].AllowDBNull = false;
             
            ds.Tables["Courses"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Courses"].Columns["CId"]};

            // =========================================================================  
            /* Foreign Key between DataTables */

            ForeignKeyConstraint myFKcp = new ForeignKeyConstraint("MyFKcp",
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Courses"].Columns["ProgId"],
                }
            );
            myFKcp.DeleteRule = Rule.None;
            myFKcp.UpdateRule = Rule.Cascade;
            ds.Tables["Courses"].Constraints.Add(myFKcp);

            // =========================================================================  
        }
        private static void loadStudents(DataSet ds)
        {
            adapterStudents.Fill(ds, "Students");
            ds.Tables["Students"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Students"].Columns["StName"].AllowDBNull = false;
            ds.Tables["Students"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Students"].PrimaryKey = new DataColumn[1]
            {
                ds.Tables["Students"].Columns["StId"]
            };

            // Foreign Key Constraints between Students and Programs datatables

            ForeignKeyConstraint myFKsp = new ForeignKeyConstraint("MyFKsp",
              new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
              },
              new DataColumn[] {
                    ds.Tables["Students"].Columns["ProgId"],
              }
          );
            myFKsp.DeleteRule = Rule.None;
            myFKsp.UpdateRule = Rule.Cascade;
            ds.Tables["Students"].Constraints.Add(myFKsp);

        }

        private static void loadEnrollments(DataSet ds)
        {

            adapterEnrollments.Fill(ds, "Enrollments");
            ds.Tables["Enrollments"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["CId"].AllowDBNull = false;

            ds.Tables["Enrollments"].PrimaryKey = new DataColumn[2]
            {
                ds.Tables["Enrollments"].Columns["StId"],
                ds.Tables["Enrollments"].Columns["CId"]
            };

            // Foreign Key Constraints between datatables

            ForeignKeyConstraint myFKes = new ForeignKeyConstraint("MyFKes",
              new DataColumn[]{
                    ds.Tables["Students"].Columns["StId"]
              },
              new DataColumn[] {
                    ds.Tables["Enrollments"].Columns["StId"],
              }              
              );

            myFKes.DeleteRule = Rule.None;
            myFKes.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(myFKes);

            ForeignKeyConstraint myFKec = new ForeignKeyConstraint("myFKec",
                new DataColumn[]
                {
                    ds.Tables["Courses"].Columns["CId"]
                },
                new DataColumn[]
                {
                    ds.Tables["Enrollments"].Columns["CId"]
                }
                );
            

            myFKec.DeleteRule = Rule.None;
            myFKec.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(myFKec);



        }
        internal static SqlDataAdapter getadapterPrograms()
        {
            return adapterPrograms;
        }
        internal static SqlDataAdapter getadapterCourses()
        {
            return adapterCourses;
        }
        internal static SqlDataAdapter getadapterStudents()
        {
            return adapterStudents;
        }
        internal static SqlDataAdapter getadapterEnrollments()
        {
            return adapterEnrollments;
        }
        internal static DataSet getDataSet()
        {
            return ds;
        }
    }

    internal class Programs
    {
        private static SqlDataAdapter adapter = DataTables.getadapterPrograms();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetPrograms()
        {
            return ds.Tables["Programs"];
        }

        internal static int UpdatePrograms()
        {
            if (!ds.Tables["Programs"].HasErrors)
            {
                return adapter.Update(ds.Tables["Programs"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Courses
    {
        private static SqlDataAdapter adapter = DataTables.getadapterCourses();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetCourses()
        {
            return ds.Tables["Courses"];
        }

        internal static int UpdateCourses()
        {
            if (!ds.Tables["Courses"].HasErrors)
            {
                return adapter.Update(ds.Tables["Courses"]);
            }
            else
            {
                return -1;
            }
        }
    }   
    internal class Students
    {
            private static SqlDataAdapter adapter = DataTables.getadapterStudents();
            private static DataSet ds = DataTables.getDataSet();
            internal static DataTable getStudents()
            {
                return ds.Tables["Students"];
            }
            internal static int updateStudents()
            {
                if (!ds.Tables["Students"].HasErrors)
                {
                    return adapter.Update(ds.Tables["Students"]);

                }
                else
                {
                    return -1;
                }
                    
            }
    }
    internal class Enrollments
    {
            private static SqlDataAdapter adapter = DataTables.getadapterEnrollments();
            private static DataSet ds = DataTables.getDataSet();

        /*internal static DataTable getEnrollments()
        {
            return ds.Tables["Enrollments"];
        }
        internal static int updateEnrollments()
        {
            if (!ds.Tables["Enrollments"].HasErrors)
            {
                return adapter.Update(ds.Tables["Enrollments"]);

            }
            else
            {
                return -1;
            }

        }
        */

        private static DataTable displayEnroll = null;

        internal static DataTable getDisplayEnrollments()
        {
            ds.Tables["Enrollments"].AcceptChanges();

           
            var query = (from enroll in ds.Tables["Enrollments"].AsEnumerable()
                             
                         from crse in ds.Tables["Courses"].AsEnumerable()
                         from stud in ds.Tables["Students"].AsEnumerable()
                        from prog in ds.Tables["Programs"].AsEnumerable()
                         where enroll.Field<string>("StId") == stud.Field<string>("StId")
                         where enroll.Field<string>("CId") == crse.Field<string>("CId")
                         where crse.Field<string>("ProgId") == prog.Field<string>("ProgId")
                         select new
                         {

                             studentID = stud.Field<string>("StId"),
                             studentName = stud.Field<string>("StName"),
                             courseID = crse.Field<string>("CId"),
                             courseName = crse.Field<string>("CName"),
                             programID = prog.Field<string>("ProgId"),
                             programName = prog.Field<string>("ProgName"),
                             final_grade = enroll.Field<Nullable<int>>("finalGrade")

                         }) ;
            DataTable result = new DataTable();
            result.Columns.Add("StId", typeof(string));
            result.Columns.Add("StName", typeof(string));
            result.Columns.Add("CId", typeof(string));
            result.Columns.Add("CName", typeof(string));
            result.Columns.Add("ProgId", typeof(string));
            result.Columns.Add("ProgName", typeof(string));
            result.Columns.Add("finalGrade",typeof(int));

            foreach (var a in query)
            {
                object[] allFields = { a.studentID, a.studentName, a.courseID, a.courseName, a.programID, a.programName, a.final_grade };
                result.Rows.Add(allFields);
            }
            displayEnroll = result;
            return displayEnroll;
        }


        internal static int InsertData(string[] a)
        {
            var test = (
                   from enroll in ds.Tables["Enrollments"].AsEnumerable()
                   where enroll.Field<string>("StId") == a[0].ToString()
                   where enroll.Field<string>("CId") == a[1].ToString()
                   select enroll);
            if (test.Count() > 0)
            {
                College1EnProject.Form1.DALMessage("This enrollment already exists");
                return -1;
            }
            try
            {
                DataRow line = ds.Tables["Enrollments"].NewRow();                
                line.SetField("CId", a[0]);
                line.SetField("StId", a[1]);
                ds.Tables["Enrollments"].Rows.Add(line);

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnroll != null)
                {
                    var query = (
                           from stud in ds.Tables["Students"].AsEnumerable()
                           from crse in ds.Tables["Courses"].AsEnumerable()
                           from prog in ds.Tables["Programs"].AsEnumerable()
                           where stud.Field<string>("StId") == a[1].ToString()
                           where crse.Field<string>("CId") == a[0].ToString()
                           where crse.Field<string>("ProgId") == prog.Field<string>("ProgId")
                           select new
                           {
                               studentId = stud.Field<string>("StId"),
                               studentName = stud.Field<string>("StName"),
                               courseId = crse.Field<string>("CId"),
                               courseName = crse.Field<string>("CName"),
                               programID = prog.Field<string>("ProgId"),
                               programName = prog.Field<string>("ProgName"),
                               final_grade = line.Field<Nullable<int>>("finalGrade")
                           }); 
                    var r = query.Single();
                    displayEnroll.Rows.Add(new object[] { r.studentId, r.studentName, r.courseId, r.courseName, r.programID, r.programName, r.final_grade });
                }
                return 0;
            }
            catch (Exception)
            {
                College1EnProject.Form1.DALMessage("Insertion / Update rejected");
                return -1;
            }
        }

        internal static int UpdateData(int[] a)
        {
            return 0;  //not used
        }

        internal static int DeleteData(List<string[]> lId)
        {
            try
            {
                var lines = ds.Tables["Enrollments"].AsEnumerable()
                                .Where(s =>
                                   lId.Any(x => (x[0] == s.Field<string>("StId") && x[1] == s.Field<string>("CId"))));

                foreach (var line in lines)
                {
                    line.Delete();
                }

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnroll != null)
                {
                    foreach (var p in lId)
                    {
                        var r = displayEnroll.AsEnumerable()
                                .Where(s => (s.Field<string>("StId") == p[0] && s.Field<string>("CId") == p[1]))
                                .Single();
                        displayEnroll.Rows.Remove(r);
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                College1EnProject.Form1.DALMessage("Update / Deletion rejected");
                return -1;
            }
        }

    }
}
