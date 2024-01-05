using MessengerHealth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessengerHealth.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string dir = ConfigurationManager.AppSettings["directoryIn"];

            string[] files = Directory.GetFiles(dir);
            MessengerFiles.lastWrittenDate = Directory.GetLastWriteTime(dir);
            List<MessengerFiles> messengerFiles1 = new List<MessengerFiles>();

            for (int i = 0; i < files.Length; i++)
            {
                DateTime lastTime = Directory.GetLastAccessTimeUtc(files[i]);
                string fileName = files[i];
                MessengerFiles messengerFiles = new MessengerFiles(fileName, lastTime);
                messengerFiles1.Add(messengerFiles);
            }

            int numberOfFiles = Directory.GetFiles(dir).Length;

            return View(messengerFiles1);
        }
        [HttpPost]
        public ActionResult Index(string jurisdiction, string procedureCode, string service)
        {
            string keySearch = $@"directory{service}";
            string dir = String.Empty;

            if (!String.IsNullOrEmpty(keySearch))
            {
                dir = ConfigurationManager.AppSettings[keySearch];
            }
            else
            {
                dir = ConfigurationManager.AppSettings["directoryIn"];
            }
            
            if (!String.IsNullOrEmpty(dir)) dir += $@"{jurisdiction}\{procedureCode}";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string[] files = Directory.GetFiles(dir);

            MessengerFiles.lastWrittenDate = Directory.GetLastWriteTime(dir);

            List<MessengerFiles> listOfFiles = new List<MessengerFiles>();

            for (int i = 0; i < files.Length; i++)
            {
                DateTime lastTime = Directory.GetLastAccessTimeUtc(files[i]);
                string fileName = files[i];
                MessengerFiles messengerFiles = new MessengerFiles(fileName, lastTime);
                listOfFiles.Add(messengerFiles);
            }

            int numberOfFiles = files.Length;

            return View(listOfFiles);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}