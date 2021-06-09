using System;
using System.Runtime.Serialization;

namespace Riverbed.Test.FreshBrew.Models
{
   
    public class OrderItem
    {
      
        public int Id { get; set; }
     
        public string Name { get; set; }
       
        public bool IsReady { get; set; }
       
        public int DelayByInSeconds { get; set; }
        
        public override string ToString()
        {
            return $"Id={Id} \t Name={Name} \t IsReady={IsReady} \t DelayByInSeconds={DelayByInSeconds}";
        }
    }
}
