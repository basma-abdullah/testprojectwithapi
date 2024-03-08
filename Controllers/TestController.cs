using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace testprojectwithapi.Controllers
{
    [Route("api/Test/add")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello from ASP.NET Core Web API!");
        }

        [HttpPost]
        public IActionResult registration([Bind("name,,phone,gender,email,password")] customer cust)
        {
            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\OneDrive\\المستندات\\customer.mdf;Integrated Security=True;Connect Timeout=30");
            conn.Open();
            string sql;
            sql = "select * from customer where phone = '" + cust.phone + "'";
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {

                reader.Close();
                conn.Close();
                return Ok("phone already exists");
            }
            else
            {
                reader.Close();
                sql = "insert into customer (name,phone,gender,email,password)  values  ('" + cust.name + "','" + cust.phone + "','" + cust.gender + "','" + cust.email + "' ,'" + cust.password + "')";
                comm = new SqlCommand(sql, conn);
                comm.ExecuteNonQuery();
                reader.Close();
                conn.Close();
                return Ok("Sucessfully added");

            }


        }
        [HttpGet("{phone}")]
        public string Login(int phone) {

            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\OneDrive\\المستندات\\customer.mdf;Integrated Security=True;Connect Timeout=30");
            conn.Open();
            string sql ="select * from customer where phone="+phone;
            SqlCommand Comm = new SqlCommand(sql, conn);
            
            SqlDataReader reader = Comm.ExecuteReader();
            
            if(reader.Read())
            {
                reader.Close();
                conn.Close();
                return ("exist");
            }
            reader.Close();
            conn.Close();
            return ("not excist");
        }

        [HttpPut("phone")]
        public IActionResult resetpassword(int phone , string password) {
            string exist = Login(phone).ToString();
            System.Console.WriteLine(exist);
            if(exist == "exist")
            {
                SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\OneDrive\\المستندات\\customer.mdf;Integrated Security=True;Connect Timeout=30");
                conn.Open();
                string sql = "update customer set password = "+password +"where phone ="+phone;
                SqlCommand Comm = new SqlCommand(sql, conn);
                Comm.ExecuteNonQuery();
                conn.Close();
                return Ok("update");
            }
            return NotFound("not excist");
        }

    }


   

}
