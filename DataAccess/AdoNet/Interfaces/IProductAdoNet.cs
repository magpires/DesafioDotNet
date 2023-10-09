using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.AdoNet.Interfaces
{
    public interface IProductAdoNet
    {
        Task<Product> InsertAsync(Product product);
    }
}
