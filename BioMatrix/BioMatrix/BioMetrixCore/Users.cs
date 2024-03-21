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
    public partial class Users : Form
    {
        UserDatabase userDB = new UserDatabase();
        User selectedUser = null;
        User currentUser;
        public Users(User currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
        }

        private void Users_Load(object sender, EventArgs e)
        {
            userListView.Items.Clear();
            List<User> userList = userDB.getUserList();
            foreach (User user in userList)
            {
                var lvi = new ListViewItem(new string[] { user.name });
                lvi.Tag = user;
                userListView.Items.Add(lvi);
            }
        }

        private void userListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userListView.SelectedItems.Count > 0)
            {
                selectedUser = (User)userListView.SelectedItems[0].Tag;
                if (selectedUser != null)
                {
                    users_container.Controls.Clear();
                    Form frm = new AddUser("Edit User",selectedUser,currentUser)
                    { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                    frm.FormBorderStyle = FormBorderStyle.None;
                    users_container.Controls.Add(frm);
                    frm.Show();

                }
            }
        }

        private void btn_add_user_Click(object sender, EventArgs e)
        {
            users_container.Controls.Clear();
            Form frm = new AddUser("Add User",currentUser) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frm.FormBorderStyle = FormBorderStyle.None;
            users_container.Controls.Add(frm);
            frm.Show();
        }
    }
}
