using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness
{
    class BD
    {
        private string connectionString { get; set; }

        public BD()
        {
            connectionString = @"Data Source=.;Initial Catalog=Fitness;Integrated Security=True";
        }

        public bool Add(string Column, string Data, string Connect = "Users")
        {
            try
            {
                string sqlExpression = String.Format("INSERT INTO {0} ({1}) VALUES ('{2}')", Connect, Column, Data);
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool AddSomthPerson(string mail, string column, string data, string Connect = "Users")
        {
            string sqlExpression = String.Format("UPDATE {0} SET {1}='{2}' where Mail='{3}'", Connect, column, data, mail);
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<object> Get(string whatGet, string Connect = "Users")
        {
            string sqlExpression = "select* from " + Connect;
            List<object> result = new List<object>();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            switch (whatGet)
            {
                case "mail":
                    {
                        while (reader.Read())
                        {
                            if (!(reader.GetValue(0) is System.DBNull))
                                result.Add(reader.GetValue(0));
                        }
                        break;
                    }
                case "password":
                    {
                        while (reader.Read())
                        {
                            if (!(reader.GetValue(1) is System.DBNull))
                                result.Add(reader.GetValue(1));
                        }
                        break;
                    }
                case "name":
                    {
                        while (reader.Read())
                        {
                            if (!(reader.GetValue(2) is System.DBNull))
                                result.Add(reader.GetValue(2));
                        }
                        break;
                    }
                case "photo":
                    {
                        while (reader.Read())
                        {
                            if (!(reader.GetValue(3) is System.DBNull))
                                result.Add(reader.GetValue(3));
                        }
                        break;
                    }
                case "Таблица":
                    {
                        while (reader.Read())
                        {
                            if (!(reader.GetValue(1) is System.DBNull))
                                result.Add(reader.GetValue(1));
                        }
                        break;
                    }
                case "Группы мышц":
                    {
                        while (reader.Read())
                        {
                            if (!(reader.GetValue(0) is System.DBNull))
                                result.Add(reader.GetValue(0));
                        }
                        break;
                    }
                case "Упражнение":
                    {
                        while (reader.Read())
                        {
                            if (!(reader.GetValue(0) is System.DBNull))
                                result.Add(reader.GetValue(0));
                        }
                        break;
                    }
                case "Видео":
                    {
                        while (reader.Read())
                        {
                            if (!(reader.GetValue(1) is System.DBNull))
                                result.Add(reader.GetValue(1));
                        }
                        break;
                    }
            }
            connection.Close();
            return result;
        }
        public List<object> GetSomthPerson(string Mailperson)
        {
            List<object> result = new List<object>();
            string sqlExpression = String.Format("select* from Users where Mail = '{0}'", Mailperson);
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                for (int a = 0; a < 4; a++)
                {
                    if (!(reader.GetValue(a) is System.DBNull))
                        result.Add(reader.GetValue(a));
                }
            }
            connection.Close();
            return result;
        }
        public List<object> GetSomthPersonStat(string Mailperson)
        {
            List<object> result = new List<object>();
            string sqlExpression = String.Format("select* from Stat where Mail = '{0}'", Mailperson);
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                for (int a = 0; a < 8; a++)
                {
                    if (!(reader.GetValue(a) is System.DBNull))
                        result.Add(reader.GetValue(a));
                }
            }
            connection.Close();
            return result;
        }
        public bool UpdateStat(string mail, string column, string data, string Connect = "Stat")
        {
            string sqlExpression = String.Format("UPDATE {0} SET {1}='{2}' where Mail='{3}'", Connect, column, data, mail);
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<object> GetSomthLess(string NameLess, string bd)
        {
            List<object> result = new List<object>();
            string sqlExpression = String.Format("select* from {0} where [Упражнение] = '{1}'", bd, NameLess);
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                for (int a = 0; a < 3; a++)
                {
                    if (!(reader.GetValue(a) is System.DBNull))
                        result.Add(reader.GetValue(a));
                }
            }
            connection.Close();
            return result;
        }
        public List<object> GetSomthDish(string NameDish, string bd)
        {
            List<object> result = new List<object>();
            string sqlExpression = String.Format("select* from {0} where [Блюдо] = '{1}'", bd, NameDish);
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                for (int a = 0; a < 5; a++)
                {
                    if (!(reader.GetValue(a) is System.DBNull))
                        result.Add(reader.GetValue(a));
                }
            }
            connection.Close();
            return result;
        }
    }
}
