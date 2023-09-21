using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.Project.DataAccessLayer
{
    public class UserDetails
    {
        public Task<User> ValidateUser(string username, string password)
        {
            return Task.Run(()=>
            {
                using (SqlConnection connection = new SqlConnection(SqlConnectionStrings.GetConnectionString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("select * from NewUser", connection))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            adapter.Fill(ds, "Users");
                            return ds.Tables[0].AsEnumerable().Select(u => new User
                            {
                                Username = u.Field<string>("EmailId"),
                                password = u.Field<string>("Password")
                            })
                                .FirstOrDefault(x => x.Username == username && x.password == password);
                        }
                    }
                }
            });
        }
    }
}
