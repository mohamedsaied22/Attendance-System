using System;
using System.Windows.Forms;

namespace Attendance
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new AddUser("Add User"));
            //Application.Run(new Login());
            //Application.Run(new Main(new Utilities.UserDatabase().getUser("abd")));
            var cmdArgs = Environment.GetCommandLineArgs();
            if (cmdArgs.Length < 2)
            {
                MessageBox.Show("Please Enter The Location");
            }


            Application.Run(new AttendanceLoop(cmdArgs[1]));
            //Application.Run(new Users());
        }
    }
}
