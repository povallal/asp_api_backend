using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.SqlClient;

using asp_net.Models;

namespace asp_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public UserController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public IConfiguration Get_configuration()
        {
            return _configuration;
        }


       


        [HttpPost]
        [Route("Registration")]
        public string Registration(User user)
        {

            string msg = string.Empty;
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlConnection myCon = new SqlConnection(sqlDataSource);
            SqlCommand cmd = null;


            try
            {


                cmd = new SqlCommand("usp_Registration", myCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@PhoneNo", user.PhoneNo);
                cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                myCon.Open();
                int i = cmd.ExecuteNonQuery();
                myCon.Close();
                if (i > 0)
                {
                    msg = "User Registerd Sucessfully";
                }

                else
                {
                    msg = "Registeration Failed";
                }



            }

            catch (Exception Ex)
            {
                msg = Ex.Message;

            }
            return msg;

        }

        [HttpPost]
        [Route("Login")]
        public string Login(User user)
        {
            SqlDataAdapter da = null;
            string msg = string.Empty;
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlConnection myCon = new SqlConnection(sqlDataSource);
            SqlCommand cmd = null;


            try
            {


                da = new SqlDataAdapter("usp_Login", myCon);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Name", user.Name);
                da.SelectCommand.Parameters.AddWithValue("@PhoneNo", user.PhoneNo);
                DataTable dt = new();
                da.Fill(dt);

                if (dt.Rows.Count >0 )
                {
                    msg = "User is Valid";
                }

                else
                {
                    msg = "User is  invalid";
                }



            }

            catch (Exception Ex)
            {
                msg = Ex.Message;

            }
            return msg;

        }





    }

}
