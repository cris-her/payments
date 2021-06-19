using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace payments.Models
{
    public class MakePayment
    {
        public static async Task<dynamic> PayAsync(string cardnumber, int month, int year, string cvc, int value) 
        {
            try
            {
                StripeConfiguration.ApiKey = "";

                var optionstoken = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    { 
                        Number = cardnumber,
                        ExpMonth = month,
                        ExpYear = year,
                        Cvc = cvc
                    }
                };

                var servicetoken = new TokenService();
                Token stripetoken = await servicetoken.CreateAsync(optionstoken);

                var options = new ChargeCreateOptions
                {
                    Amount = value,
                    Currency = "usd",
                    Description = "test",
                    Source = stripetoken.Id
                };

                var service = new ChargeService();
                Charge charge = await service.CreateAsync(options);
            
                if (charge.Paid)
                {
                    return "Success";
                }
                else
                {
                    return "failed";
                }
            
            }
            catch (Exception e) 
            {
                return e.Message;
            }
        }
    }
}
