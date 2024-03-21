using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Utilities
{
    class UserDatabase
    {
        private SqlConnection DBConnect()
        {
            string value = System.Configuration.ConfigurationManager.AppSettings["userDB"];
            //SqlConnection con = new SqlConnection("Data Source=10.0.91.40;Initial Catalog=FP_Users;User ID=sa;password=hrh@123");
            SqlConnection con = new SqlConnection(value);
            con.Open();
            return con;
        }

        private void DBclose(SqlConnection con)
        {
            con.Close();
        }

        public void insertUser(string name, string username, int role)
        {
            SqlConnection con = DBConnect();
            //DateTime now = DateTime.Now;

            String sql = " insert into users (name,username,role_id,password) values ('" + name + "','" + username + "'," + role + ",'" + Encryption.Encrypt("0000") + "')";
            //Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }


        public void updateUser(User user, string name, int role)
        {
            SqlConnection con = DBConnect();
            String sql = " update users set name = '" + name + "' , role_id = " + role + " where id = '" + user.id + "'";
            Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = command;
            adapter.UpdateCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }
        public void enableDisableUser(User user)
        {
            SqlConnection con = DBConnect();
            String sql = " update users set enabled = '" + !user.enabled + "' where id = '" + user.id + "'";
            Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = command;
            adapter.UpdateCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }
        public void resetUserPassword(User user)
        {
            SqlConnection con = DBConnect();
            String sql = " update users set activated = 0 , password = '" + Encryption.Encrypt("0000") +"' where id = '" + user.id + "'";
            Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = command;
            adapter.UpdateCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);

        }
        public bool isUsernameExist(string username)
        {
            bool isExist = false;

            SqlConnection con = DBConnect();
            string sql = "Select id from users where username = '" + username + "'";
            Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataReader reader = command.ExecuteReader();
            isExist = reader.Read();
            DBclose(con);
            return isExist;
        }



        public List<Role> getRoleList()
        {
            List<Role> roleList = new List<Role>();
            Role role;
            SqlConnection con = DBConnect();
            SqlCommand command = new SqlCommand("Select * from role order by role", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                role = new Role();
                role.id = reader.GetInt32(0);
                role.role = reader.GetString(1);
                roleList.Add(role);
            }
            DBclose(con);
            return roleList;
        }


        public User getUser(string username)
        {
            User user = new User();
            SqlConnection con = DBConnect();
            string sql = "select * from v_users where username = @username";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@username", username);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.id = reader.GetInt32(0);
                user.username = reader.GetString(1);
                user.password = reader.GetString(2);
                user.name = reader.GetString(3);
                user.activated = reader.GetBoolean(4);
                user.enabled = reader.GetBoolean(5);
                user.role_id = reader.GetInt32(6);
                user.role = reader.GetString(7);
            }

            DBclose(con);
            return user;
        }
        public bool isUserExist(string username, string password)
        {
            bool isUserExist = false;
            SqlConnection con = DBConnect();
            string sql = "select username , password from users where username = @username  and password = @password";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            SqlDataReader reader = command.ExecuteReader();
            isUserExist = reader.Read();
            DBclose(con);
            return isUserExist;
        }

        public bool isUserActivated(string username)
        {
            bool isUserActivated = false;
            SqlConnection con = DBConnect();
            string sql = "select username from users where username = @username  and activated = 1";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@username", username);
            SqlDataReader reader = command.ExecuteReader();
            isUserActivated = reader.Read();
            DBclose(con);
            return isUserActivated;
        }

        public void activateUser(User user, string password)
        {
            SqlConnection con = DBConnect();
            String sql = " update users set password = '" + password + "' , activated  = " + 1 + " where id = '" + user.id + "'";
            Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = command;
            adapter.UpdateCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }

        public DataTable getRoleDataResource()
        {
            SqlConnection con = DBConnect();
            SqlDataAdapter da = new SqlDataAdapter("Select * From role order by role", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["role"] = "Select Role";
            dt.Rows.InsertAt(dr, 0);

            return dt;
        }

        public List<User> getUserList()
        {
            List<User> userList = new List<User>();
            User user;
            SqlConnection con = DBConnect();
            SqlCommand command = new SqlCommand("Select * from v_users order by name", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                user = new User();
                user.id = reader.GetInt32(0);
                user.username = reader.GetString(1);
                user.password = reader.GetString(2);
                user.name = reader.GetString(3);

                user.activated = reader.GetBoolean(4);
                user.enabled = reader.GetBoolean(5);
                user.role_id = reader.GetInt32(6);
                user.role = reader.GetString(7);

                userList.Add(user);
            }
            DBclose(con);
            return userList;
        }

        public void insertLog(string action, int user_id)
        {
            SqlConnection con = DBConnect();
            DateTime now = DateTime.Now;

            String sql = " insert into logs (action,user_id,date) values ('" + action + "'," + user_id + ",'"+ now.ToString() + "')";
            Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            DBclose(con);
        }

        public DataSet getLogsDataSet()
        {
            SqlConnection con = DBConnect();
            string sql = "SELECT * FROM v_logs ORDER BY date DESC ";
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds, "logs");
            return ds;
        }
        public DataSet getLogsDataSetLast3Months()
        {
            DateTime threeMonthsAgo = DateTime.Now.AddMonths(-3);
            DateTime now = DateTime.Now;
            SqlConnection con = DBConnect();
            string sql = "SELECT * FROM v_logs where date between '"+threeMonthsAgo+"' and '"+now+"' ORDER BY date DESC ";
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds, "logs");
            return ds;
        }
    }
}
