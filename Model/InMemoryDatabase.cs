using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverbed.Test.FreshBrew.Models
{
    public class InMemoryDatabase : IOrderItem
    {
        List<OrderItem> orderList;
        public InMemoryDatabase()
        {
            orderList = new List<OrderItem>
            {
                new OrderItem{Id=0,Name="Iced Espresso Latte",IsReady=false, DelayByInSeconds=0},
                new OrderItem{Id=1,Name="Night Owl Brew",IsReady=false,DelayByInSeconds=0},
                new OrderItem{Id=2,Name="Caramel Iced Coffee",IsReady=false,DelayByInSeconds=0}
            };
        }

        /*
         * Create a new order in db, i.e., adds an order to list
         */
        public OrderItem Add(Order newOrder)
        {
            OrderItem newOrderItem = new OrderItem();
            newOrderItem.Id = orderList.Max(r => r.Id) + 1;
            newOrderItem.Name = newOrder.Name;
            newOrderItem.DelayByInSeconds = newOrder.DelayByInSeconds;

            orderList.Add(newOrderItem);
            if (newOrderItem.DelayByInSeconds > 0)
            {
                var milliSec = 1000 * newOrderItem.DelayByInSeconds;
                System.Threading.Thread.Sleep(milliSec);
            }
            return newOrderItem;
        }

        /*
         * Delete order by id
         */ 
        public bool Delete(int id)
        {               
            var itemInDb = orderList.FirstOrDefault(t => t.Id == id);
            if (itemInDb == null)
                return false;
            orderList.Remove(itemInDb);
            return true;
        }

        /*
         * Get order by id
         */ 

        public OrderItem Get(int id)
        {             
            return orderList.FirstOrDefault(t => t.Id == id);
        }

        /*
        Get all orders
         */
        public IEnumerable<OrderItem> GetAll()
        {               
            return orderList.OrderBy(t => t.Name);
        }

        /*
         * Update an existing order
         */ 
        public OrderItem Update(int orderId, Order newOrderItem)
        {                             
            var itemInDb = orderList.FirstOrDefault(t => t.Id == orderId);
            if (itemInDb == null)
                return null;

            itemInDb.Name = newOrderItem.Name!=null?newOrderItem.Name:itemInDb.Name;
            itemInDb.DelayByInSeconds = newOrderItem.DelayByInSeconds;
            if (newOrderItem.DelayByInSeconds > 0)
            {
                var milliSec = 1000 * newOrderItem.DelayByInSeconds;
                System.Threading.Thread.Sleep(milliSec);
            }
            return itemInDb;
        }
    }
}
