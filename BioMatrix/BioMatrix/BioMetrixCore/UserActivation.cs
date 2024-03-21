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
    public partial class UserActivation : Form
    {
        User user;
        UserDatabase userDB = new UserDatabase();
        public UserActivation(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void btn_active_Click(object sender, EventArgs e)
        {
            if(password.Text == passwordConfirm.Text)
            {
                userDB.activateUser(user, Encryption.Encrypt(password.Text));
                userDB.insertLog("Activation", user.id);
                this.Hide();
                Main main = new Main(user);
                main.ShowDialog();
                this.Close();
            }
        }
    }
}
