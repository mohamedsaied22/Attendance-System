using Attendance.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance
{
    public partial class Login : Form
    {
        UserDatabase userDB = new UserDatabase();
        public Login()
        {
            InitializeComponent();
        }

        private void login(string username, string password)
        {
            if (userDB.isUserExist(username, Encryption.Encrypt(password)))
            {
                User user = userDB.getUser(username);
                if (user.enabled)
                {
                    if (user.activated)
                    {
                        userDB.insertLog("Login", user.id);
                        //MessageBox.Show("Login successfully", "Login");
                        this.Hide();
                        Main main = new Main(user);
                        main.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        //MessageBox.Show("User Need to Activate", "Login");
                        this.Hide();
                        UserActivation userActivation = new UserActivation(user);
                        userActivation.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("This User is disabled", "Login");
                }

            }
            else
            {

                MessageBox.Show("This user not found please try again", "Login");
            }
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            login(username.Text, password.Text);
        }
    }
}
