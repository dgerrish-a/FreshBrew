
using Riverbed.Test.FreshBrew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riverbed.Test.FreshBrew.Models
{
    public interface IOrderItem
    {
        IEnumerable<OrderItem> GetAll();
        OrderItem Get(int id);
        OrderItem Add(Order newOrderItem);
        OrderItem Update(int id, Order newOrderItem);
        bool Delete(int id);
    }
}
