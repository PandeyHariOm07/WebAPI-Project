using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.Project.DataAccessLayer
{
    public class ProductDetailsConnect : IProduct
    {
        private SqlConnection _con = null;
        private SqlConnection _connection = null;
        private SqlCommand _command = null;
        private SqlDataReader _reader = null;

        public bool AddProduct(string productName, decimal? unitPrice, short? unitsInStock)
        {
            using (_con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (SqlCommand _command = new SqlCommand("usp_AddProduct",_con))
                {
                    _command.CommandType = CommandType.StoredProcedure;



                    if (_con.State != ConnectionState.Open)
                    {
                        _con.Open();
                    }



                    _command.Parameters.Add(new SqlParameter("@productName", SqlDbType.NVarChar, 40));
                    _command.Parameters.Add(new SqlParameter("@unitPrice", SqlDbType.Money, 8));
                    _command.Parameters.Add(new SqlParameter("@unitsInStock", SqlDbType.SmallInt, 2));



                    _command.Parameters["@productName"].Value = productName;
                    _command.Parameters["@unitPrice"].Value = unitPrice.Value;
                    _command.Parameters["@unitsInStock"].Value = unitsInStock.Value;



                    var res = _command.ExecuteNonQuery();
                    return res > 0;
                }



            }



        }

        public bool DeleteProduct(int id)
        {
            using (SqlConnection con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                con.Open();

                string deleteQuery = "delete from Products where ProductId = @Id";

                using (SqlCommand command = new SqlCommand(deleteQuery, con))
                {
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = id;

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }

        public List<Product> GetProduct()
        {
            List<Product> products = new List<Product>();
            using(_con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using(_command = new SqlCommand("Select * from Products", _con))
                {
                    if (_con.State != ConnectionState.Open) _con.Open();
                    using(_reader = _command.ExecuteReader())
                    {
                        if (_reader.HasRows)
                        {
                            while (_reader.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductId = Convert.ToInt32(_reader.GetValue(0)),
                                    ProductName = _reader.GetValue(1).ToString(),
                                    unitPrice = Convert.ToDecimal(_reader.GetValue(5)),
                                    UnitsInStock = Convert.ToInt16(_reader.GetValue(6))
                                });
                            }
                        }
                    }
                }
            }
            return products;
        }

        public Product GetProductById(int id)
        {
            Product pro = null;
            using (_con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("Select * from Products where ProductId = @ProductId", _con))
                {
                    _command.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int, 4));
                    _command.Parameters["@ProductId"].Value = id;
                    if (_con.State != ConnectionState.Open) _con.Open();
                    using (_reader = _command.ExecuteReader())
                    {
                        if (_reader.HasRows)
                        {
                            _reader.Read();
                            pro = new Product();
                            pro.ProductId = Convert.ToInt32(_reader.GetValue(0));
                            pro.ProductName = _reader.GetValue(1).ToString();
                            pro.unitPrice = Convert.ToDecimal(_reader.GetValue(5));
                            pro.UnitsInStock = Convert.ToInt16(_reader.GetValue(6));
                        }
                    }
                }
            }
            return pro;
        }

        public bool UpdateProduct(int id, Product product)
        {
            using (_connection = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("usp_UpdateProduct2", _connection))
                {
                    _command.CommandType = CommandType.StoredProcedure;
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    _command.Parameters.Add(new SqlParameter("@prodID", SqlDbType.Int, 32));
                    _command.Parameters.Add(new SqlParameter("@productName", SqlDbType.NVarChar, 40));
                    _command.Parameters.Add(new SqlParameter("@unitPrice", SqlDbType.Money, 8));
                    _command.Parameters.Add(new SqlParameter("@unitsInStock", SqlDbType.SmallInt, 2));



                    _command.Parameters["@productName"].Value = product.ProductName;
                    _command.Parameters["@unitPrice"].Value = product.unitPrice.Value;
                    _command.Parameters["@unitsInStock"].Value = product.UnitsInStock.Value;
                    _command.Parameters["@prodID"].Value = id;



                    var res = _command.ExecuteNonQuery();
                    return res > 0;
                }
            }
        }
    }
}
