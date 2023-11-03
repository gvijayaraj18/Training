using EmpRegistration.Models;
using ERPDBUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace EmpRegistration.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    [HttpGet("Getid/{id}")]

    public ActionResult<userdetails> GetbyId(int id)
    {
      SqlParameter[] parameters = new SqlParameter[5];
      parameters[0]=new SqlParameter("@id", SqlDbType.Int);
      parameters[0].Value = id;
      parameters[1] = new SqlParameter("@status",SqlDbType.Int );
      parameters[1].Direction = ParameterDirection.Output;

      DataSet ds =  SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "[spo_DayEndgetempdetails]", parameters);
      if (Convert.ToInt32(parameters[1].Value)==1)
      {
        IEnumerable<userdetails> data = (from dr in ds.Tables[0].AsEnumerable()
                                         select new userdetails()
                                         {
                                           UserId = Convert.ToInt32(dr["userid"]),
                                           firstName = Convert.ToString(dr["FirstName"]),
                                           lastName = Convert.ToString(dr["LastName"]),
                                           email = Convert.ToString(dr["Email"]),
                                           mobile = Convert.ToString(dr["Mobileno"]),
                                           gender = Convert.ToString(dr["gender"])
                                         });
        return Ok(data);
      }
      else return BadRequest();
      
    }

    [HttpPost("PostData")]
    public ActionResult<string> postdata(userdetails userdetails)
    {
      SqlParameter[] parameter1 = new SqlParameter[8];
      parameter1[0] = new SqlParameter("@userid", SqlDbType.Int);
      parameter1[0].Value = userdetails.UserId;
      parameter1[1] = new SqlParameter("@firstname", SqlDbType.VarChar);
      parameter1[1].Value = userdetails.firstName;
      parameter1[2] = new SqlParameter("@lastname", SqlDbType.VarChar);
      parameter1[2].Value = userdetails.lastName;
      parameter1[3] = new SqlParameter("@email", SqlDbType.VarChar);
      parameter1[3].Value = userdetails.email;
      parameter1[4] = new SqlParameter("@mobileno", SqlDbType.VarChar);
      parameter1[4].Value = userdetails.mobile;
      parameter1[5] = new SqlParameter("@gender", SqlDbType.VarChar);
      parameter1[5].Value = userdetails.gender;
      parameter1[6] = new SqlParameter("@pwd", SqlDbType.VarChar);
      parameter1[6].Value = userdetails.pwd;
      parameter1[7] = new SqlParameter("@status", SqlDbType.Int);
      parameter1[7].Direction = ParameterDirection.Output;

      DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "[spo_DayEndEMPREGISTRATION]", parameter1);

      if (Convert.ToInt32(parameter1[7].Value) == 1)
      {
        return Ok("Data Added Successfully");
      }
      else
      {
        return BadRequest();
      }
    }
  }
}

