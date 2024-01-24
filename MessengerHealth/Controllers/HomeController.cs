using MessengerHealth.Data;
using MessengerHealth.Models;
using MessengerHealth.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace MessengerHealth.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            List<ServiceOperation> servs = new List<ServiceOperation>(DataAccess.GetServiceOperationsList());

            string path = ConfigurationManager.AppSettings["localDirectory"];
            ReadAndParseJsonFile parser = new ReadAndParseJsonFile(path);

            List<Jurisdiction> juridisctions = parser.UseUserDefinedObjectJson();
            var serviceList = ServiceHelper.FetchDirectoryServices(juridisctions, servs);

            Dictionary<string, Dictionary<string, List<string>>> keyValuePairs = new Dictionary<string, Dictionary<string, List<string>>>();
            keyValuePairs.Add("In", serviceList[0]);
            keyValuePairs.Add("Out", serviceList[1]);

            Dictionary<string, MessengerFiles> messengerFilesDictionaryObject = ServiceHelper.FetchMessengerFilesDictionary(keyValuePairs, servs);

            List<Dictionary<string, MessengerFiles>> list = new List<Dictionary<string, MessengerFiles>>();
            list.Add(messengerFilesDictionaryObject);

            return View(list);
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}