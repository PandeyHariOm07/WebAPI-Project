using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.Project.DataAccessLayer
{
    public class CustomerDetails : ICustomer
    {
        private SqlConnection _con = null;
        private SqlConnection _connection = null;
        private SqlCommand _command = null;
        private SqlDataReader _reader = null;

        public List<string> GetDistinctProduct()
        {
            List<string> customer = new List<string>();
            using (_con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("select Distinct(Country) from Customers", _con))
                {
                    if (_con.State != ConnectionState.Open) _con.Open();
                    using (_reader = _command.ExecuteReader())
                    {
                        if (_reader.HasRows)
                        {
                            while (_reader.Read())
                            {
                                customer.Add(_reader.GetValue(0).ToString());
                            }
                        }
                    }
                }
            }
            return customer;
        }
    }
}
