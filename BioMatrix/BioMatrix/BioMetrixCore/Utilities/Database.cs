using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Utilities
{
    class Database
    {
        //Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;
        private SqlConnection DBConnect()
        {
            string value = System.Configuration.ConfigurationManager.AppSettings["DB"];
            //SqlConnection con = new SqlConnection("Data Source=10.0.91.40;Initial Catalog=FP2;User ID=sa;password=hrh@123");
            SqlConnection con = new SqlConnection(value);
            con.Open();
            return con;
        }

        private void DBclose(SqlConnection con)
        {
            con.Close();
        }

        public int getEmployeeId(int emp_pin)
        {
            int employeeId = 0;
            SqlConnection con = DBConnect();
            SqlCommand command = new SqlCommand("Select id from hr_employee where emp_pin = " + emp_pin, con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) { employeeId = reader.GetInt32(0); }
            DBclose(con);
            if (employeeId == 0)
            {
                insertEmployee(emp_pin);
                getEmployeeId(emp_pin);
            }
            return employeeId;



        }

        public int getTerminalId(String ip)
        {
            int terminalId = 0;
            SqlConnection con = DBConnect();
            SqlCommand command = new SqlCommand("Select id from att_terminal where terminal_tcpip = '" + ip + "'", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) { terminalId = reader.GetInt32(0); }
            DBclose(con);
            return terminalId;

        }

        public List<Terminal> getWorkingTerminalList()
        {
            List<Terminal> terminalList = new List<Terminal>();
            Terminal terminal;
            SqlConnection con = DBConnect();
            SqlCommand command = new SqlCommand("Select id, terminal_name,terminal_type,terminal_connectpwd,terminal_tcpip,terminal_port,terminal_status,terminal_location from att_terminal where terminal_status = 1 order by terminal_location ASC ,terminal_name", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                terminal = new Terminal();
                terminal.id = reader.GetInt32(0);
                terminal.name = reader.GetString(1);
                terminal.type = reader.GetString(2);
                terminal.PWD = reader.GetString(3);
                terminal.ip = reader.GetString(4);
                terminal.port = reader.GetInt32(5);
                terminal.status = reader.GetInt32(6);
                terminal.location = reader.GetString(7);
                terminalList.Add(terminal);
            }
            DBclose(con);
            return terminalList;
        }

        public List<Terminal> getAllTerminalList()
        {
            List<Terminal> terminalList = new List<Terminal>();
            Terminal terminal;
            SqlConnection con = DBConnect();
            SqlCommand command = new SqlCommand("Select id, terminal_name,terminal_type,terminal_connectpwd,terminal_tcpip,terminal_port,terminal_status,terminal_location from att_terminal order by terminal_location ASC , terminal_name ", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                terminal = new Terminal();
                terminal.id = reader.GetInt32(0);
                terminal.name = reader.GetString(1);
                terminal.type = reader.GetString(2);
                terminal.PWD = reader.GetString(3);
                terminal.ip = reader.GetString(4);
                terminal.port = reader.GetInt32(5);
                terminal.status = reader.GetInt32(6);
                terminal.location = reader.GetString(7);
                terminalList.Add(terminal);
            }
            DBclose(con);
            return terminalList;
        }

        public bool isLogExist(int emp_pin, String date_time_record, String ip)
        {
            bool isExist = false;
            DateTime oDate = DateTime.Parse(date_time_record);
            int terminal_id = getTerminalId(ip);
            int employee_id = getEmployeeId(emp_pin);
            SqlConnection con = DBConnect();
            SqlCommand command = new SqlCommand("Select id from att_punches where terminal_id = " + terminal_id + " and employee_id = " + employee_id + " and punch_time = '" + oDate.ToString("yyyy-MM-dd HH:mm:ss") + "'", con);
            //Console.WriteLine("Select id from att_punches where terminal_id = " + terminal_id + " and employee_id = " + employee_id + " and punch_time = '" + oDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            SqlDataReader reader = command.ExecuteReader();
            isExist = reader.Read();
            DBclose(con);
            return isExist;

        }

        public void insertLog(int emp_pin, String date_time_record, String ip)
        {
            SqlConnection con = DBConnect();
            DateTime oDate = DateTime.Parse(date_time_record);
            int terminal_id = getTerminalId(ip);
            int employee_id = getEmployeeId(emp_pin);
            //Console.WriteLine("done");
            String sql = " insert into att_punches (employee_id,punch_time,terminal_id) values (" + employee_id + ",'" + oDate.ToString("yyyy-MM-dd HH:mm:ss") + "'," + terminal_id + ")";
            //Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }

        public void insertEmployee(int emp_pin)
        {
            SqlConnection con = DBConnect();
            DateTime now = DateTime.Now;
            String sql = " insert into hr_employee (emp_lastname,emp_firstname,emp_pin,emp_hiredate,department_id,emp_active) values (" + emp_pin + "," + emp_pin + "," + emp_pin + ",'" + now.ToString("yyyy-MM-dd 00:00:00") + "',3,1)";
            //Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }

        public void insertTerminalFailure(int terminal_id, string failure_reason)
        {
            SqlConnection con = DBConnect();
            DateTime now = DateTime.Now;

            String sql = " insert into terminal_failure (terminal_id,failure_reason,occur_time) values (" + terminal_id + ",'" + failure_reason + "','" + now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            //Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }
        public void insertTerminal(string terminal_name, string terminal_type, int terminal_status, string terminal_tcpip, string terminal_pwd, string terminal_port,string terminal_location)
        {
            SqlConnection con = DBConnect();
            DateTime now = DateTime.Now;

            String sql = " insert into att_terminal (terminal_name,terminal_status,terminal_type,terminal_connectpwd,terminal_tcpip,terminal_port,terminal_location,terminal_no,terminal_category) values ('" + terminal_name + "'," + terminal_status + ",'" + terminal_type + "','" + terminal_pwd + "','" + terminal_tcpip + "','" + terminal_port +"','" + terminal_location + "',1,1)";
            //Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }
        public void updateTerminal(int terminal_id, string terminal_name, string terminal_type, int terminal_status, string terminal_tcpip, string terminal_pwd, string terminal_port,string terminal_location)
        {
            SqlConnection con = DBConnect();
            DateTime now = DateTime.Now;

            String sql = " update att_terminal set terminal_name = '" + terminal_name + "',terminal_type = '" + terminal_type + "',terminal_status = " + terminal_status + ",terminal_tcpip ='" + terminal_tcpip + "' , terminal_connectpwd = '" + terminal_pwd + "',terminal_port = '" + terminal_port + "',terminal_location = '"+terminal_location+"' where id = " + terminal_id;
            Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = command;
            adapter.UpdateCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }
        public void truncateTerminalFailure()
        {
            SqlConnection con = DBConnect();
            string sql = "TRUNCATE TABLE terminal_failure";
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }

        public DataSet getTerminalFailureDataSet()
        {
            SqlConnection con = DBConnect();
            string sql = "select * from v_terminal_failure";
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds, "terminal_failure");
            return ds;
        }

        public DataSet getRecordesByID(string employeeID, string fromDate,string toDate)
        {
            SqlConnection con = DBConnect();
            string sql = "SELECT punch_time,emp_pin,time,date FROM sesco WHERE emp_pin = " + employeeID+" and punch_time between '"+fromDate+"' AND '"+toDate+"' ORDER BY punch_time DESC";
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds, "employeeRecords");
            return ds;
        }
        public DataSet getAllRecordes(string fromDate, string toDate)
        {
            SqlConnection con = DBConnect();
            string sql = "SELECT punch_time,emp_pin,time,date FROM sesco WHERE punch_time between '" + fromDate + "' AND '" + toDate + "' ORDER BY punch_time DESC";
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds, "employeeRecords");
            return ds;
        }

    }
}
