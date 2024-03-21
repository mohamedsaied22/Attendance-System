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
    public partial class Devices : Form
    {
        Database db = new Database();
        User currentUser;
        Terminal selectedItem = null;
        public Devices(User currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
        }

        private void btn_add_device_Click(object sender, EventArgs e)
        {
            devices_container.Controls.Clear();
            Form frm = new AddDevice("Add Device",currentUser) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frm.FormBorderStyle = FormBorderStyle.None;
            devices_container.Controls.Add(frm);
            frm.Show();
        }

        private void Devices_Load(object sender, EventArgs e)
        {
            deviceListView.Items.Clear();
            List<Terminal> terminalList = db.getAllTerminalList();

            foreach (Terminal terminal in terminalList)
            {
                //var row = new string[] { terminal.name};
                var lvi = new ListViewItem(new string[] { terminal.name });
                lvi.Tag = terminal;
                deviceListView.Items.Add(lvi);
            }
        }

        private void deviceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deviceListView.SelectedItems.Count > 0)
            {
                selectedItem = (Terminal)deviceListView.SelectedItems[0].Tag;
                if (selectedItem != null)
                {
                    devices_container.Controls.Clear();
                    Form frm = new AddDevice(selectedItem, "Edit Device",currentUser) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                    frm.FormBorderStyle = FormBorderStyle.None;
                    devices_container.Controls.Add(frm);
                    frm.Show();
                }
            }
        }
    }
}
