using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TBPayments.Models;
using Transbank.Webpay;

namespace TBPayments.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var transaction = new Webpay(Configuration.ForTestingWebpayPlusNormal()).NormalTransaction;

            var amount = 2000;
            var buyOrder = new Random().Next(100000, 999999999).ToString();
            var sessionId = "sessionid";

            string returnUrl = "http://localhost:8080/home/Return";
            string finalUrl = "http://localhost:8080/home/Final";

            var initResult = transaction.initTransaction(amount, buyOrder, sessionId, returnUrl, finalUrl);

            var tokenWs = initResult.token;
            var formAction = initResult.url;

            ViewBag.Amount = amount;
            ViewBag.BuyOrder = buyOrder;
            ViewBag.Tokens = tokenWs;
            ViewBag.FormAction = formAction;

            return View();
        }

        public ActionResult Return()
        {
            var transaction = new Webpay(Configuration.ForTestingWebpayPlusNormal()).NormalTransaction;
            string tokenWs = Request.Form("token_ws");

            var result = transaction.getTransactionResult(tokenWs);
            var output = result.detailOutput(0);
            if (output.responseCode == 0)
            {
                ViewBag.UrlRedirection = result.urlRedirection;
                ViewBag.TokenWs = tokenWs;
                ViewBag.ResponseCode = output.responseCode;
                ViewBag.Amount = output.amount;
                ViewBag.AuthorizationCode = output.authorizationCode;
            }

            return View();
        }

        public IActionResult Final()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
