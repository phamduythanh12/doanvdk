
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_an_vdk
{
    class Query_Data_MySQL
    {     
        public static DataSet getAllCustomer()
        {
            DataSet data = new DataSet();
            String query = "select * from customer1";
            using (SqlConnection con = new SqlConnection(ConnecttionString.connecttionString))
            {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.Fill(data);
                con.Close();

            }
            return data;

        }
        public static DataSet get1Customer(String cmnn)
        {
            DataSet data = new DataSet();
            String query = "select * from customer1 where CMNN = '" + cmnn + "'";
            using (SqlConnection con = new SqlConnection(ConnecttionString.connecttionString))
            {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.Fill(data);
                con.Close();

            }
            return data;

        }
        public static String deleteCustomer(String query)
        {
            using (SqlConnection con = new SqlConnection(ConnecttionString.connecttionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.ExecuteNonQuery();
                con.Close();

            }
            return "Đã Xóa Thành Công!";
        }
        public static String updateCustomer(Customer customer, int id)
        {
            String query = "update customer1 set Name='" + customer.getName() + "' , Id_Card='" + customer.getIdCard() + "' , Bien_so_xe='" + customer.getBienSoXe() + "', Phone='" + customer.getPhone() + "', CMNN ='" + customer.getCMND() + "',Start_Time='" + customer.getStartTime() + "', End_Time='" + customer.getEndTime() + "' where id = '" + id + "'";
            using (SqlConnection con = new SqlConnection(ConnecttionString.connecttionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.ExecuteNonQuery();
                con.Close();

            }
            return "Đã Chỉnh Sửa Thành Công!";
        }
        public static String insertStatus(int id)
        {
            String query = "INSERT INTO status(customer_id,thoi_gian_gui,status_customer)  VALUES (@customerId , @thoiGianGui,0)";
            using (SqlConnection con = new SqlConnection(ConnecttionString.connecttionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("customerId", id);
                DateTime now =  DateTime.Now;               
                command.Parameters.AddWithValue("thoiGianGui", now);            
                command.ExecuteNonQuery();
                con.Close();

            }
            return "true";
        }
        public static String updateStatus(int id)
        {
            DateTime now = DateTime.Now;
            String query = "update status set thoi_gian_lay='" + now + "',status_customer =1 where id="+id;
            using (SqlConnection con = new SqlConnection(ConnecttionString.connecttionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.ExecuteNonQuery();
                con.Close();

            }
            return "true";
        }
        public static void insertCustomer(Customer customer)
        {
            String query = "INSERT INTO customer1(Name,Bien_so_xe,Phone,Id_card,CMNN,Start_Time,End_Time)  VALUES (@Name, @Bien_so_xe, @Phone,@Id_card,@CMNN,@Start_Time, @End_Time)";
            using (SqlConnection con = new SqlConnection(ConnecttionString.connecttionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("Name", customer.getName());
                command.Parameters.AddWithValue("Bien_so_xe", customer.getBienSoXe());
                command.Parameters.AddWithValue("Id_Card", customer.getIdCard());
                command.Parameters.AddWithValue("Phone", customer.getPhone());
                command.Parameters.AddWithValue("CMNN", customer.getCMND());
                command.Parameters.AddWithValue("Start_Time", customer.getStartTime());
                command.Parameters.AddWithValue("End_Time", customer.getEndTime());
                command.ExecuteNonQuery();
                con.Close();
            }

        }
        public static bool checkPark()
        {
            String query = "select id from status where status_customer = 0";
            using (SqlConnection conn = new SqlConnection(ConnecttionString.connecttionString))
            {
                conn.Open();
               
                    // String query = "SELECT status.id FROM status INNER JOIN customer1 ON customer1.Id_card = '" + id_card + "' and status.status_customer = 0 ";
                    // if (getID(query) == 0)
                    // Tạo một đối tượng Command.
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int kt = 0;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                kt++;
                            }
                        }
                    }
                conn.Close();
                    if( kt == 4)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
             }

         }
        public static DataSet getInfStatus(int customerID)
        {
            DataSet data = new DataSet();
            String query = "select * from status where customer_id = '" + customerID + "'";
            using (SqlConnection con = new SqlConnection(ConnecttionString.connecttionString))
            {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.Fill(data);
                con.Close();

            }
            return data;

        }

    }
}
