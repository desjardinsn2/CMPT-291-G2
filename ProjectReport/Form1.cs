using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProjectReport
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=LAPTOP-7R60URD2;Initial Catalog=CMPT291Project;Integrated Security=True";
        SqlDataAdapter sqlDa;
        public Form1() 
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            InitializeComponent();
            ComBox.Items.Add("Total Past Reservations");
            ComBox.Items.Add("Total Past Reservations By Customer");
            ComBox.Items.Add("Total Current Reservations");
            ComBox.Items.Add("Total Current Reservations By Customer");
            ComBox.Items.Add("Total Current Reservations By Employee");
            ComBox.Items.Add("Customer With Most Past Reservations");
            ComBox.Items.Add("Number Of Current Reservations By Vehicle Type");
            ComBox.Items.Add("Total Current Reservations By Branch");
            ComBox.Items.Add("Available Vehicle Types By Branch");
            ComBox.Items.Add("Branches With No Current Reservation");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                if (ComBox.Text.Equals("Total Past Reservations")){
                    sqlDa = new SqlDataAdapter("select sum(no_of_times_reserved) as Total_Past_Reservations from Customer", sqlCon);
                }
                if (ComBox.Text.Equals("Total Past Reservations By Customer"))
                {
                    sqlDa = new SqlDataAdapter("select customer_name, sum(no_of_times_reserved) as Num_Completed_Reservations from customer group by customer_name", sqlCon);
                }
                if (ComBox.Text.Equals("Total Current Reservations"))
                {
                    sqlDa = new SqlDataAdapter("select count(reservation_id) as Total_Current_Reservations from Reservation", sqlCon);
                }
                if (ComBox.Text.Equals("Total Current Reservations By Customer"))
                {
                    sqlDa = new SqlDataAdapter("select customer_name, count(reservation_id) as Num_Current_Reservations from (Reservation full join Customer on Reservation.customer_id = Customer.customer_id) group by customer_name", sqlCon);
                }
                if (ComBox.Text.Equals("Total Current Reservations By Employee"))
                {
                    sqlDa = new SqlDataAdapter("select Employee.employee_name, count(reservation_id) as Num_Current_Reservations from (reservation full join Employee on reservation.employee_id = employee.employee_id) group by employee_name", sqlCon);
                }
                if (ComBox.Text.Equals("Customer With Most Past Reservations"))
                {
                    sqlDa = new SqlDataAdapter("select customer_name as Most_Frequent_Customer, no_of_times_reserved from customer, (select MAX(no_of_times_reserved) as Most_Reservations from customer) as newtable where no_of_times_reserved = Most_Reservations", sqlCon);
                }
                if (ComBox.Text.Equals("Number Of Current Reservations By Vehicle Type"))
                {
                    sqlDa = new SqlDataAdapter("select type_name, count(reservation_id) as Num_Current_Reservations from Reservation left join car on reservation.car_id = car.car_id left join type on type.type_id = car.type_id group by type_name", sqlCon);
                }
                if (ComBox.Text.Equals("Total Current Reservations By Branch"))
                {
                    sqlDa = new SqlDataAdapter("select branch_name, count(reservation_id) as Num_Current_Reservations from Reservation full join branch on Reservation.pick_up_branch_id = branch.branch_id group by branch_name", sqlCon);
                }
                if (ComBox.Text.Equals("Available Vehicle Types By Branch"))
                {
                    sqlDa = new SqlDataAdapter("select branch_name, type.type_name, count(car.type_id) as Available_Cars from ((branch full join car on branch.branch_id = car.branch_id) left join type on car.type_id = type.type_id) group by branch_name, type.type_name having type_name != 'NULL'", sqlCon);
                }
                if (ComBox.Text.Equals("Branches With No Current Reservation"))
                {
                    sqlDa = new SqlDataAdapter("select branch_name from reservation full join branch on reservation.pick_up_branch_id = branch.branch_id except (select branch_name from reservation left join branch on reservation.pick_up_branch_id = branch.branch_id)", sqlCon);
                }
                /*Template to add more of these bitches*/
                /*if (ComBox.Text.Equals(""))
                {
                    sqlDa = new SqlDataAdapter("", sqlCon);
                }*/
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                DataGrid.DataSource = dtbl;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
