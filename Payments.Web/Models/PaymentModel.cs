using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace payments.Models
{
    public class PaymentModel
    {
        public string cardnumber { get; set; }

        public int month { get; set; }

        public int year { get; set; }

        public string cvc { get; set; }

        public int value { get; set; }

    }
}
