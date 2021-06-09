using Riverbed.Test.FreshBrew.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riverbed.Test.FreshBrewApi.Services
{
    public class InMemoryDb : IOrderItem
    {
        InMemoryDatabase db = null;
        public InMemoryDb()
        {
            db = new InMemoryDatabase();
        }

        public OrderItem Add(Order newOrderItem)
        {
            Log.Information("InMemoryDb:Add");
            return db.Add(newOrderItem);
        }

        public bool Delete(int id)
        {
            Log.Information("InMemoryDb:Delete");
            return db.Delete(id);
        }

        public OrderItem Get(int id)
        {
            Log.Information("InMemoryDb:GetById");
            return db.Get(id);
        }

        public IEnumerable<OrderItem> GetAll()
        {
            Log.Information("InMemoryDb:GetAll");
            return db.GetAll();
        }

        public OrderItem Update(int orderId, Order newOrderItem)
        {
            Log.Information("InMemoryDb:Update");
            return db.Update(orderId,newOrderItem);
        }
    }
}
