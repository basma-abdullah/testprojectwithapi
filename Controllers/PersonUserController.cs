using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Numerics;
using testprojectwithapi.Models;

namespace testprojectwithapi.Controllers
{
    [Route("Falak/[controller]")]
    [ApiController]
    public class PersonUserController : ControllerBase
    {
        
        public static SqlConnection dbConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\OneDrive\\المستندات\\FalakDB.mdf;Integrated Security=True;Connect Timeout=30");


        [HttpGet("login/{Username},{Password}")]
        public IActionResult login(string Username , string Password)
        {
            var conn = PersonUserController.dbConn;
            conn.Open();
            string sql = "SELECT * FROM PersonUsers WHERE Username = '" + Username + "' AND Password = '" + Password + "'"; 
            SqlCommand Comm = new SqlCommand(sql, conn);
            //query string
            SqlDataReader reader = Comm.ExecuteReader();

            if (reader.Read())
            {
                reader.Close();
                conn.Close();
                return Ok("user found");
            }
            reader.Close();
            conn.Close();
            return NotFound("user not found");
        }



        [HttpPost("signup")]
        public IActionResult signup([Bind("UserType , FullName ,Password ,PhoneNumber ,Gender , Email ")] PersonUsers user)
        {
            var conn = PersonUserController.dbConn;
            conn.Open();
            string sql;
            if (user.UserType == "Parent")
            {
                sql = "SELECT * FROM PersonUsers WHERE Username = " + user.PhoneNumber.ToString();
                SqlCommand comm = new SqlCommand(sql, conn);
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                    conn.Close();
                    return NotFound("user already exists");
                }
                else
                {
                    reader.Close();
                    string sqladd = "INSERT INTO PersonUsers (Username, UserType, FullName, Password, PhoneNumber, Gender, Email, UsernameType) VALUES ('" + user.PhoneNumber + "','" + user.UserType + "','" + user.FullName + "','" + user.Password + "','" + user.PhoneNumber + "','" + user.Gender + "','" + user.Email + "','Phone')";
                    comm = new SqlCommand(sqladd, conn);
                    comm.ExecuteNonQuery();
                    reader.Close();
                    conn.Close();
                    return Ok("Sucessfully added");

                }
            }
            else
            {
                if (user.UserType == "Child")
                {
                    sql = "SELECT * FROM PersonUsers WHERE Username = '" + user.Email + "'";
                    SqlCommand comm = new SqlCommand(sql, conn);
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        reader.Close();
                        conn.Close();
                        return NotFound("user already exists");
                    }
                    else
                    {
                        reader.Close();
                        string sqladd = "INSERT INTO PersonUsers (Username, UserType, FullName, Password, PhoneNumber, Gender, Email, UsernameType) VALUES ('" + user.Email + "','" + user.UserType + "','" + user.FullName + "','" + user.Password + "','" + user.PhoneNumber + "','" + user.Gender + "','" + user.Email + "','Email')";
                        comm = new SqlCommand(sqladd, conn);
                        comm.ExecuteNonQuery();
                        reader.Close();
                        conn.Close();
                        return Ok("Sucessfully added");

                    }
                }
            }
            return NotFound("error");

        }


        [HttpPut("reset_password/{Username}")]
        public IActionResult reset_password(string Username , string Password)
        {
            var conn = PersonUserController.dbConn;
            conn.Open();
            string sql = "UPDATE PersonUsers SET Password = '" + Password + "' WHERE Username = '" + Username + "'";
            SqlCommand Command = new SqlCommand(sql, conn);
            int rowsAffected = Command.ExecuteNonQuery();
            conn.Close();
            if (rowsAffected > 0)
            {
                return Ok("updated");
            }

            return NotFound("user not found");
        }


        [HttpDelete("delete_account/{Username}")]
        public IActionResult delete_account(string Username) {
            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\OneDrive\\المستندات\\FalakDB.mdf;Integrated Security=True;Connect Timeout=30");
            conn.Open();
            string sql = "DELETE FROM PersonUsers WHERE Username = '" + Username + "'";
            SqlCommand Command = new SqlCommand(sql, conn);
            int rowsAffected = Command.ExecuteNonQuery();
            conn.Close();
            if (rowsAffected > 0)
            {
                return Ok("user deleted");
            }

            return NotFound("user not found");
        }
            
    }
}
