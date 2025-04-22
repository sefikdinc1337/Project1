using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Entities.Concrete;
using Core.DataAccess;
using Entities.DTOs;

namespace DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<Product>
    {
        List<ProductDetailDto> GetProductDetails();
        
    }
}
