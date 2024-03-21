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
    public partial class Main : Form
    {
        UserDatabase userDB = new UserDatabase();
        User currentUser;
        public Main(User user)
        {
            InitializeComponent();
            this.currentUser = user;
        }


        private void attendanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            container.Controls.Clear();
            Form frm = new Attendance(container,currentUser) {Dock = DockStyle.Fill , TopLevel = false , TopMost = true };
            frm.FormBorderStyle = FormBorderStyle.None;
            container.Controls.Add(frm);
            frm.Show();
        }

        private void terminalFailureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            container.Controls.Clear();
            Form frm = new TerminalFailure() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frm.FormBorderStyle = FormBorderStyle.None;
            container.Controls.Add(frm);
            frm.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            attendanceToolStripMenuItem_Click(null,null);
            usernameToolStripMenuItem.Text = currentUser.name;
            if(currentUser.role_id == 2)
            {
                devicesToolStripMenuItem.Visible = false;
                usersToolStripMenuItem.Visible = false;
                logsToolStripMenuItem.Visible = false;
            }
        }

        private void addDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            container.Controls.Clear();
            Form frm = new AddDevice("Add Device",currentUser) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frm.FormBorderStyle = FormBorderStyle.None;
            container.Controls.Add(frm);
            frm.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var resultDia = DialogResult.None;
            resultDia = MessageBox.Show("Are you sure to logout ?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(resultDia == DialogResult.Yes)
            {
                userDB.insertLog("Logout", currentUser.id);
                this.Hide();
                Login login = new Login();
                login.ShowDialog();
                this.Close();
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            container.Controls.Clear();
            Form frm = new Users(currentUser) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frm.FormBorderStyle = FormBorderStyle.None;
            container.Controls.Add(frm);
            frm.Show();
        }

        private void devicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            container.Controls.Clear();
            Form frm = new Devices(currentUser) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frm.FormBorderStyle = FormBorderStyle.None;
            container.Controls.Add(frm);
            frm.Show();
        }

        private void logsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            container.Controls.Clear();
            Form frm = new Logs() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frm.FormBorderStyle = FormBorderStyle.None;
            container.Controls.Add(frm);
            frm.Show();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            container.Controls.Clear();
            Form frm = new Search(currentUser) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frm.FormBorderStyle = FormBorderStyle.None;
            container.Controls.Add(frm);
            frm.Show();
        }
    }
}
