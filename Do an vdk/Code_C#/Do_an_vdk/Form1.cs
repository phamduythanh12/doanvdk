using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Do_an_vdk
{
    public partial class Form1 : Form
    {

       
        String id_card = "";
        Boolean status = true;
        public Form1()
        {   
            InitializeComponent();
            serialPort1.Open();
            input.Visible = false;
            Gb_search.Visible = false;
            GBshow.Visible = false;
        }
        int id = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'project_vdkDataSet1.customer' table. You can move, or remove it, as needed.

            Display();
        }
        private void search_Click(object sender, EventArgs e)
        {
            String cmnn = textBox5.Text;
            Display1Customer(cmnn);
        }
        public void Display()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = Query_Data_MySQL.getAllCustomer().Tables[0];

        }
        public void Display1Customer(String cmnn)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = Query_Data_MySQL.get1Customer(cmnn).Tables[0];

        }
        // event dataDridview
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.Rows[e.RowIndex];
             id = (int) row.Cells[0].Value;
            tb_name.Text = row.Cells[1].Value.ToString();
            tb_bien_so_xe.Text = row.Cells[2].Value.ToString();
            bt_idCard.Text = row.Cells[4].Value.ToString();
            textBox_phone.Text = row.Cells[3].Value.ToString();
            textBox_CMNN.Text = row.Cells[5].Value.ToString();
            dateTimePicker1.Text = row.Cells[6].Value.ToString();
            dateTimePicker2.Text = row.Cells[7].Value.ToString();

        }
        private void bt_ok_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer(tb_name.Text, tb_bien_so_xe.Text, textBox_phone.Text, bt_idCard.Text, textBox_CMNN.Text, dateTimePicker1.Text, dateTimePicker2.Text);
            Query_Data_MySQL.insertCustomer(customer);
            Display();
            bt_ok.Visible = false;

        }

        // khai bao stt của bảng hoạt động
        int stt = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true) {
                try
                {   
                    String ketqua = "";
                  
                    try
                    {
                        id_card = serialPort1.ReadExisting();
                        if (id_card.Length != 0)
                        {

                        nhay_toi_day_khi_di_vao:   // label goto

                            if (id_card.Length == 11)
                            {
                               
                                id_card = id_card.Substring(0, 8);

                                string sql = "Select * from customer1 where Id_card= '" + id_card + "'";
                                int customer_id = getID(sql);
                                String query = "SELECT status.id FROM status where customer_id = '" + customer_id + "' and status.status_customer = 0 ";
                                int statusID = getID(query);
                               if (statusID != 0) { 
                                        String message = Query_Data_MySQL.updateStatus(getID(query));
                                       serialPort1.WriteLine("#2:Good bye!|"); //dieu khien servo mo cua
                                }
                                else
                                {
                                    goto nhay_toi_day_khi_di_vao;
                                }
                            }
                            else
                            {


                                if (Query_Data_MySQL.checkPark())
                                {
                                    serialPort1.WriteLine("#3:Sorry!Da het cho dau!|"); //dieu khien servo mo cua
                                    return;
                                }
                                id_card = id_card.Substring(0, 8);
                                using (SqlConnection conn = new SqlConnection(ConnecttionString.connecttionString))
                                {
                                    conn.Open();
                                    try
                                    {
                                       // String query = "SELECT status.id FROM status INNER JOIN customer1 ON customer1.Id_card = '" + id_card + "' and status.status_customer = 0 ";
                                       // if (getID(query) == 0)
                                        
                                            string sql = "Select * from customer1 where Id_card= '" + id_card + "'";
                                            // Tạo một đối tượng Command.
                                            SqlCommand cmd = new SqlCommand(sql, conn);
                                            using (DbDataReader reader = cmd.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read())
                                                    {
                                                    // Chỉ số (index) của cột ID trong câu lệnh SQL.
                                                      int Index_id = reader.GetOrdinal("id"); // 
                                                      int id = reader.GetInt32(Index_id);
                                                    int startTime = reader.GetOrdinal("Start_Time"); // 
                                                    DateTime startTimeKH = reader.GetDateTime(startTime);
                                                    
                                                    // Chỉ số (index) của cột Emp_ID trong câu lệnh SQL.
                                                    int endTime = reader.GetOrdinal("End_Time");
                                                    DateTime endTimeKH = reader.GetDateTime(endTime);
                                                    if (checkActive(startTimeKH, endTimeKH))
                                                    {
                                                        serialPort1.WriteLine("#3:The da het han!|"); //dieu khien servo mo cua
                                                        break;

                                                    }
                                                    String query = "SELECT status.id FROM status where customer_id = '" + id + "' and status.status_customer = 0 ";
                                                    // Chỉ số (index) của cột Emp_ID trong câu lệnh SQL.
                                                    if (getID(query) != 0)
                                                    {
                                                        serialPort1.WriteLine("#3:Xe Dang Gui!|");
                                                        conn.Close();
                                                        return;
                                                    }
                                                    // them vao bang trang thai
                                                    String message = Query_Data_MySQL.insertStatus(id);
                                                    ListViewItem LVItem = new ListViewItem(stt.ToString());
                                                        // Chỉ số (index) của cột Emp_ID trong câu lệnh SQL.
                                                        int nameIndex = reader.GetOrdinal("Name"); // 0
                                                                                                   // Cột Emp_No có index = 1.
                                                        string name = reader.GetString(nameIndex);
                                                        ListViewItem.ListViewSubItem LVSItem1 = new ListViewItem.ListViewSubItem(LVItem, name);
                                                        int id_theIndex = reader.GetOrdinal("Bien_so_xe");// 2
                                                        string id_the = reader.GetString(id_theIndex);
                                                        ListViewItem.ListViewSubItem LVSItem2 = new ListViewItem.ListViewSubItem(LVItem, id_the);
                                                        int phone = reader.GetOrdinal("Phone");// 2
                                                        string phoneString = reader.GetString(phone);
                                                        ListViewItem.ListViewSubItem LVSItem3 = new ListViewItem.ListViewSubItem(LVItem, phoneString);
                                                        int cmnn = reader.GetOrdinal("CMNN");// 2
                                                        string cmnnString = reader.GetString(cmnn);
                                                        ListViewItem.ListViewSubItem LVSItem4 = new ListViewItem.ListViewSubItem(LVItem, cmnnString);
                                                        LVItem.SubItems.Add(LVSItem1);
                                                        LVItem.SubItems.Add(LVSItem2);
                                                        LVItem.SubItems.Add(LVSItem3);
                                                        LVItem.SubItems.Add(LVSItem4);
                                                        listView1.Items.Add(LVItem);

                                                        ketqua = name + "," + id_the;
                                                    stt++;

                                                }
                                                }
                                            }
                                            if (ketqua.Length >= 5)
                                            {
                                                serialPort1.WriteLine("#1:" + ketqua + "|"); //dieu khien servo mo cua 
                                            }
                                            else
                                            {
                                                serialPort1.WriteLine("#3:Chua Dang Ky!|"); //dieu khien servo mo cua
                                            }
                                    }
                                    catch (Exception en)
                                    {
                                        serialPort1.WriteLine("#3:Chua Dang Ky123!|"); //dieu khien servo mo cua
                                    }
                                    conn.Close();

                                }
                               
                            }
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("hello pham duy thanh");
                }
            }
        }
        // Method lay id cua khach hang da gui xe ma chua lay xe rad
        private  int getID(String query)
        {
            int id = 0;
            using (SqlConnection conn = new SqlConnection(ConnecttionString.connecttionString))
            {
                conn.Open();
                // Tạo một đối tượng Command.
                SqlCommand cmd = new SqlCommand(query, conn);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Chỉ số (index) của cột Emp_ID trong câu lệnh SQL.
                            int IndexId = reader.GetOrdinal("id");
                            id = reader.GetInt32(IndexId);
                        }
                    }
                }

            }
            return id;
        }
        //cat chuoi trong date
        Boolean checkActive(DateTime dateStart, DateTime dateEnd)
        {
            DateTime dateNow = DateTime.Now;
           
            if (DateTime.Compare(dateNow, dateStart) > 0 && DateTime.Compare(dateNow, dateEnd) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // event button start 
        private void bt_start_Click(object sender, EventArgs e)
        {
            if (status) {
                timer1.Start();
                bt_start.Text = "Stop";
                status = !status;
            }
            else
            {
                timer1.Stop();
                bt_start.Text = "Start";
                status = !status;
            }
            
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            input.Visible = false;
            Gb_search.Visible = true;
            GBshow.Visible = false;
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetInput();
            MessageBox.Show("Bạn vui lòng quét thẻ mới để thêm khách hàng!");
            String id_card1 = serialPort1.ReadLine();
            id_card1 = id_card1.Substring(0,8);
            if(id_card1.Length != 0) {                
                bt_idCard.Text = id_card1;
            }          
            input.Visible = true;
            Gb_search.Visible = false;
            input.Text = "Input";
            update.Visible = false;
            bt_ok.Visible = true;
            dataGridView2.Visible = false;

        }

        private  void resetInput()
        {
            tb_name.Text = "";
            tb_bien_so_xe.Text = "";
            bt_idCard.Text = "";
            textBox_phone.Text = "";
            textBox_CMNN.Text = "";
            dateTimePicker1.Text = "";
            dateTimePicker2.Text = "";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult thongbao;
            thongbao = MessageBox.Show("Bạn Có Muốn Xóa Hay Không?", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (thongbao == DialogResult.OK)
            {
                String query = "delete from customer1 where id = '" + id + "'";
                String message = Query_Data_MySQL.deleteCustomer(query);// xoa thông tin cua 1 khach hàng;
                MessageBox.Show(message);
            }
            Display();
        }

        private void update_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer(tb_name.Text, tb_bien_so_xe.Text, textBox_phone.Text, bt_idCard.Text,  textBox_CMNN.Text, dateTimePicker1.Text, dateTimePicker2.Text);
            String message = Query_Data_MySQL.updateCustomer(customer,id);// xoa thông tin cua 1 khach hàng;
            MessageBox.Show(message);
            Display();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            input.Visible = true;
            Gb_search.Visible = false;
            input.Text = "Update";
            update.Visible = true;
            bt_ok.Visible = false;
            GBshow.Visible = false;

        }

        private void viewCustomerInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.DataSource = Query_Data_MySQL.getInfStatus(id).Tables[0];
            dataGridView2.Visible = true;
            input.Visible = false;
            Gb_search.Visible = false;

        }


    }
}
