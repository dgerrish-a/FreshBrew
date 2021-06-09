
using Microsoft.EntityFrameworkCore;
using Riverbed.Test.FreshBrew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riverbed.Test.FreshBrewApi.Models
{
    public class OrderContext: DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options):base(options)
        {

        }
        public DbSet<OrderItem> OrderItemDbSet { get; set; }
    }
}
