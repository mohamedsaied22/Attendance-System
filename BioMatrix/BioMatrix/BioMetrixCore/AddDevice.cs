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
    public partial class AddDevice : Form
    {
        Terminal terminal;
        Database db = new Database();
        UserDatabase userDB = new UserDatabase();
        User currentUser;

        public AddDevice(string title, User currentUser)
        {
            InitializeComponent();
            this.title.Text = title;
            this.edit.Visible = false;
            this.save.Visible = true;
            this.currentUser = currentUser;
        }

        public AddDevice(Terminal terminal, string title, User currentUser)
        {
            InitializeComponent();
            this.terminal = terminal;
            this.name.Text = terminal.name;
            this.status.SelectedIndex = terminal.status;
            this.type.Text = terminal.type;
            this.ip.Text = terminal.ip;
            this.port.Text = terminal.port.ToString();
            this.password.Text = terminal.PWD;
            this.location.Text = terminal.location;
            this.title.Text = title;
            this.edit.Visible = true;
            this.save.Visible = false;
            this.currentUser = currentUser;
        }

        private void clear_Click(object sender, EventArgs e)
        {
            name.Text = "";
            status.SelectedIndex = -1;
            type.Text = "";
            ip.Text = "";
            port.Text = "";
            password.Text = "";
            location.Text = "";
        }

        private void save_Click(object sender, EventArgs e)
        {
            db.insertTerminal(name.Text, type.Text, status.SelectedIndex, ip.Text, password.Text, port.Text,
                location.Text);
            string message = name.Text + " has been added successfully";
            string title = "Add Device";
            MessageBox.Show(message, title);
            userDB.insertLog(message, currentUser.id);
        }

        private void edit_Click(object sender, EventArgs e)
        {
            db.updateTerminal(terminal.id, name.Text, type.Text, status.SelectedIndex, ip.Text, password.Text,
                port.Text, location.Text);
            string message = name.Text + " has been edited successfully";
            string title = "Edit Device";
            MessageBox.Show(message, title);
            userDB.insertLog(message, currentUser.id);
        }

        private void location_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}