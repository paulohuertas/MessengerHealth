using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerHealth.Models
{
    public class ServiceOperation
    {
        public DateTime LastOperationDate { get; set; }
        public string Service { get; set; }
        public string Operation { get; set; }
        public string ServiceCorrect
        {
            get
            {
                if (this.Service.Contains("-"))
                {
                    return this.Service.Split('-')[1];
                }
                return this.Service;
            }
        }
        public int NumberFiles { get; set; }
    }
}