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
    public partial class AddUser : Form
    {
        UserDatabase userDB = new UserDatabase();
        string name_value = "";
        string username_value = "";
        int role = 0;
        User currentUser;

        User user;
        public AddUser(string title, User currentUser)
        {
            InitializeComponent();
            this.title.Text = title;
            this.save.Visible = true;
            this.edit.Visible = false;
            this.enable.Visible = false ;
            this.disable.Visible = false ;
            this.username.Enabled = true;
            this.currentUser = currentUser;
        }
        public AddUser(string title, User user,User currentUser)
        {
            InitializeComponent();
            this.title.Text = title;
            this.save.Visible = false;
            this.edit.Visible = true;
            this.enable.Visible = !user.enabled;
            this.disable.Visible = user.enabled;
            this.username.Enabled = false;
            name_value = user.name;
            username_value = user.username;
            this.role = user.role_id;
            this.user = user;
            this.currentUser = currentUser;
            if(!user.enabled)
                this.panel2.BackColor  = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
        }
        private void AddUser_Load(object sender, EventArgs e)
        {
            DataTable dt = userDB.getRoleDataResource();
            roleBox.ValueMember = "id";
            roleBox.DisplayMember = "role";
            roleBox.DataSource = dt;
            roleBox.SelectedIndex = role;
            name.Text = name_value;
            username.Text = username_value;
        }
        private void clear_Click(object sender, EventArgs e)
        {
            name.Text = "";
            roleBox.SelectedIndex = 0;
            username.Text = "";
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (!userDB.isUsernameExist(username.Text))
            {
                try
                {
                    userDB.insertUser(name.Text.Trim(), username.Text, roleBox.SelectedIndex);
                    MessageBox.Show(name.Text.Trim() + " has been added successfully", "Add User");
                    userDB.insertLog(name.Text.Trim() + " has been added", currentUser.id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Add User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("This username isn't available. Please enter another username", "Add User", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void edit_Click(object sender, EventArgs e)
        {
            try
            {
                userDB.updateUser(user, name.Text.Trim(), roleBox.SelectedIndex);
                MessageBox.Show(name.Text.Trim() + " has been edited successfully", "Edit User");
                userDB.insertLog(name.Text.Trim()+" has been edited", currentUser.id);
                role = roleBox.SelectedIndex;
                AddUser_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Edit User", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reset_password_Click(object sender, EventArgs e)
        {
            try
            {
                var resultDia = DialogResult.None;
                resultDia = MessageBox.Show("Do you wish to reset the password ??", "Reset Password", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultDia == DialogResult.Yes)
                {
                    userDB.resetUserPassword(user);
                    MessageBox.Show(user.name + "'s passowrd has been reseted successfully", "Reset Password");
                    userDB.insertLog(user.name + " passowrd has been reseted", currentUser.id);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Reset Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void disable_Click(object sender, EventArgs e)
        {
            userDB.enableDisableUser(user);
            panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            MessageBox.Show(user.name + "'s account disabled", "User Account");
            userDB.insertLog(user.name + " account disabled", currentUser.id);
        }

        private void enable_Click(object sender, EventArgs e)
        {
            userDB.enableDisableUser(user);
            panel2.BackColor = System.Drawing.SystemColors.Control;
            MessageBox.Show(user.name + "'s account enabled", "User Account");
            userDB.insertLog(user.name + " account enabled", currentUser.id);
        }
    }
}

