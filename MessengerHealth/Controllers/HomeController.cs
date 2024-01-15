using MessengerHealth.Models;
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

            string path = @"C:\01. Paulo Huertas\c#\Messenger\MessengerHealth\MessengerHealth\directories_local.json";
            ReadAndParseJsonFile parser = new ReadAndParseJsonFile(path);
            List<Jurisdiction> juridisctions = parser.UseUserDefinedObjectJson();

            Dictionary<string, Dictionary<string, List<string>>> proceduresServiceDirectoryIn = new Dictionary<string, Dictionary<string, List<string>>>();
            Dictionary<string, Dictionary<string, List<string>>> proceduresServiceDirectoryOut = new Dictionary<string, Dictionary<string, List<string>>>();
            Dictionary<string, List<string>> serviceDirectoryIn = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> serviceDirectoryOut = new Dictionary<string, List<string>>();
            List<string> inDirectories = new List<string>();
            
            List<string> outDirectories = new List<string>();
            for (int i = 0; i < juridisctions.Count; i++)
            {
                string keyCode = juridisctions[i].JuridisctionCode;

                foreach (var inPath in juridisctions[i].Procedures.Services.In)
                {
                    string pathDir = inPath.DirectoryPath;
                    inDirectories.Add(pathDir);

                    if (serviceDirectoryIn.ContainsKey(keyCode))
                    {
                        serviceDirectoryIn[keyCode] = inDirectories.ToList();
                    }
                    else
                    {
                        serviceDirectoryIn.Add(keyCode, inDirectories.ToList());
                    }
                }

                inDirectories.Clear();

                keyCode = juridisctions[i].JuridisctionCode;

                foreach (var outPath in juridisctions[i].Procedures.Services.Out)
                {
                    string pathDir = outPath.DirectoryPath;
                    outDirectories.Add(pathDir);

                    if (serviceDirectoryOut.ContainsKey(keyCode))
                    {
                        serviceDirectoryOut[keyCode] = outDirectories.ToList();
                    }
                    else
                    {
                        serviceDirectoryOut.Add(keyCode, outDirectories.ToList());
                    }
                }

                outDirectories.Clear();
            }

            Dictionary<string, Dictionary<string, List<string>>> keyValuePairs = new Dictionary<string, Dictionary<string, List<string>>>();
            keyValuePairs.Add("In", serviceDirectoryIn);
            keyValuePairs.Add("Out", serviceDirectoryOut);

            List<string> keys = new List<string>(keyValuePairs.Keys);
            Dictionary<string, int> inTotalNumberFiles = new Dictionary<string, int>();

            int counter = 0;
            foreach(var dict in keyValuePairs)
            {
                string currentKey = keys[counter];

                Dictionary<string, List<string>> values = keyValuePairs[currentKey];

                List<string> procedureKeys = new List<string>(values.Keys);

                for (int i = 0; i < values.Count; i++)
                {
                    string procedureKey = procedureKeys[i];

                    for(int j = 0; j < values[procedureKey].Count; j++)
                    {
                        string currentPath = values[procedureKey][j];
                        if (!Directory.Exists(currentPath))
                        {
                            Directory.CreateDirectory(currentPath);
                        }

                        int numberOfFilesInDirectory = Directory.GetFiles(currentPath).Length;
                        if (numberOfFilesInDirectory > 0)
                        {
                            inTotalNumberFiles.Add(currentPath, numberOfFilesInDirectory);
                        }
                    }
                    
                }
                counter++;
            }

            List<Dictionary<string, int>> list = new List<Dictionary<string, int>>();
            list.Add(inTotalNumberFiles);

            //return View(messengerFiles1);
            return View(list);
        }
        [HttpPost]
        public ActionResult Index(string jurisdiction, string procedureCode, string service)
        {
            string keySearch = $@"directory{service}";
            string dir;

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

            return View(listOfFiles);
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