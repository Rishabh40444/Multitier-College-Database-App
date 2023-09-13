using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer

{

    internal class Programs
    {
        internal static int UpdatePrograms()
        {
            DataSet ds = Data.DataTables.getDataSet();

            return Data.Programs.UpdatePrograms();
        }
    }

    internal class Courses
    {
        internal static int UpdateCourses()
        {

            return Data.Courses.UpdateCourses();
        }
    }
    internal class Students
    {
        internal static int UpdateStudents()
        {
            return Data.Students.updateStudents();
        }
    }
    /*
    internal class Enrollments
    {
        internal static int UpdateEnrollments()
        {
            return Data.Enrollments.updateEnrollments();
        }
    }
    */
}
