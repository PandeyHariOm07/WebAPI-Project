using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.Project.DataAccessLayer
{
    public interface IProduct
    {
        List<Product> GetProduct();
        Product GetProductById(int id);
        bool AddProduct(string productName, decimal unitPrice, int unitsInStock);
        bool DeleteProduct(int id);
        bool UpdateProduct(int id,Product prd);
    }
}
