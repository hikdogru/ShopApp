using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreOrderRepository:EfCoreGenericRepository<Order,ShopContext>, IOrderRepository
    {

    }
}
