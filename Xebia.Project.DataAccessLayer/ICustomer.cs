using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.Project.DataAccessLayer
{
    public interface ICustomer
    {
        List<string> GetDistinctProduct();
    }
}
