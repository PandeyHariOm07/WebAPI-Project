using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Xebia.Project.DataAccessLayer
{
    public class ProductDetails : IProduct
    {
        private SqlConnection con = null;
        private SqlDataAdapter adapter = null;
        public bool AddProduct(string productName, decimal unitPrice, int unitsInStock)
        {
            using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select ProductName, UnitPrice, UnitsInStock from Products", con))
                {
                    using (DataSet ds = new DataSet())
                    {
                        adapter.Fill(ds, "Products");
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.InsertCommand = builder.GetInsertCommand();
                        DataRow dr = ds.Tables["Products"].NewRow();
                        dr["ProductName"] = productName;
                        dr["UnitPrice"] = unitPrice;
                        dr["UnitsInStock"] = unitsInStock;
                        ds.Tables["Products"].Rows.Add(dr);
                        var res = adapter.Update(ds, "Products") ==1?true:false;
                        return res;
                    }
                }
            }
        }

        public bool DeleteProduct(int id)
        {
            using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select * from Products where @Id = ProductId", con))
                {
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                    using (DataTable dt = new DataTable())
                    {
                        adapter.Fill(dt);
                        dt.PrimaryKey = new DataColumn[1] { dt.Columns["ProductId"] };
                        DataRow dr = dt.Rows[0];
                        if (dr != null)
                        {
                            dr.Delete();
                            adapter.Update(dt);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public List<Product> GetProduct()
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Provide Details of All The Products
        /// </summary>
        /// <returns>Returns DataTable</returns>
        //public DataSet GetProduct()
        //{
        //    using(con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
        //    {
        //        using (adapter = new SqlDataAdapter("Select * from Products", con))
        //        {
        //            using (DataSet ds = new DataSet())
        //            {
        //                adapter.Fill(ds,"Products");
        //                return ds;
        //            }
        //        }
        //    }
        //}

        //public DataSet GetProductById(int id)
        //{
        //    using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
        //    {
        //        using (adapter = new SqlDataAdapter("Select * from Products where ProductId = @Id", con))
        //        {
        //            adapter.SelectCommand.Parameters.Add(new SqlParameter("Id", SqlDbType.Int, 4));
        //            adapter.SelectCommand.Parameters["Id"].Value = id;
        //            using (DataSet ds = new DataSet())
        //            {
        //                adapter.Fill(ds, "Products");
        //                return ds;
        //            }
        //        }
        //    }
        //}

        //public Product GetProductById(int id)
        //{
        //    var product = GetProduct().Tables["Products"].AsEnumerable().Select(x => new Product
        //    {
        //        ProductId = x.Field<int>("ProductId"),
        //        ProductName = x.Field<string>("ProductName")
        //    }).FirstOrDefault(x => x.ProductId == id);
        //    return product;
        //}

        public bool UpdateProduct(int id, Product prd)
        {
            using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select ProductId, ProductName, UnitPrice, UnitsInStock from Products", con))
                {
                    using (DataSet ds = new DataSet())
                    {
                        adapter.Fill(ds, "Products");
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.UpdateCommand = builder.GetUpdateCommand();
                        var pro = ds.Tables["Products"].AsEnumerable().FirstOrDefault(x => x.Field<int>("ProductId") == id);
                        if(pro != null)
                        {
                            pro.BeginEdit();
                            pro["ProductName"] = prd.ProductName == null ? pro["ProductName"] : prd.ProductName;
                            pro["UnitPrice"] = prd.unitPrice == null ? pro["UnitPrice"] : prd.unitPrice.Value;
                            pro["UnitsInStock"] = prd.UnitsInStock == null ? pro["UnitsInStock"] : prd.UnitsInStock.Value;
                            pro.EndEdit();

                            //Update
                            var res = adapter.Update(ds, "Products");
                            return res > 0 ? true : false;
                        }
                        return false;
                    }
                }
            }
        }
    }
}
