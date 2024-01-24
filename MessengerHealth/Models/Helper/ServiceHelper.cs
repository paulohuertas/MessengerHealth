using MessengerHealth.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MessengerHealth.Models.Helper
{
    public static class ServiceHelper
    {
        public static List<Dictionary<string, List<string>>> FetchDirectoryServices(List<Jurisdiction> jurisdictions, List<ServiceOperation> servicos)
        {
            Dictionary<string, List<string>> serviceDirectoryIn = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> serviceDirectoryOut = new Dictionary<string, List<string>>();

            List<string> inDirectories = new List<string>();
            List<string> outDirectories = new List<string>();

            for (int i = 0; i < jurisdictions.Count; i++)
            {
                string keyCode = jurisdictions[i].JuridisctionCode;
                
                //if (keyCode == "GB") keyCode = "UK";

                foreach (var inPath in jurisdictions[i].Procedures.Services.In)
                {
                    string pathDir = inPath.DirectoryPath;
                    if(servicos.Any(x=> pathDir.EndsWith(x.ServiceCorrect)) && servicos.Any(x => keyCode.Contains(x.Service.Split('-')[0])))
                    {
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
                }

                inDirectories.Clear();

                foreach (var outPath in jurisdictions[i].Procedures.Services.Out)
                {
                    string pathDir = outPath.DirectoryPath;
                    
                    if (servicos.Any(x => pathDir.EndsWith(x.ServiceCorrect)))
                    {
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
                }
                outDirectories.Clear();
            }

            List<Dictionary<string, List<string>>> serviceList = new List<Dictionary<string, List<string>>>();
            serviceList.Add(serviceDirectoryIn);
            serviceList.Add(serviceDirectoryOut);

            return serviceList;
        }

        public static Dictionary<string, MessengerFiles> FetchMessengerFilesDictionary(Dictionary<string, Dictionary<string, List<string>>> keyValuePairs, List<ServiceOperation> services)
        {
            List<string> keys = new List<string>(keyValuePairs.Keys);
            Dictionary<string, MessengerFiles> messengerFilesDictionaryObject = new Dictionary<string, MessengerFiles>();

            int counter = 0;
            foreach (var dict in keyValuePairs)
            {
                string serviceKey = keys[counter];

                Dictionary<string, List<string>> values = keyValuePairs[serviceKey];
                List<string> procedureKeys = new List<string>(values.Keys);

                for (int i = 0; i < values.Count; i++)
                {
                   
                    string procedureKey = procedureKeys[i];

                    for (int j = 0; j < values[procedureKey].Count; j++)
                    {
                        string currentPath = values[procedureKey][j];
                        string searchPath = currentPath.Substring(0, currentPath.LastIndexOf("\\"));
                        string currentPathCode = currentPath.Substring(currentPath.LastIndexOf("\\") + 1);
                        string searchPattern = currentPath.Substring(currentPath.LastIndexOf("\\") + 1) + "*";
                        string messengerService = $"{procedureKey}-{currentPathCode}";

                        if (serviceKey == "In") serviceKey = "Resp";
                        if (serviceKey == "Out") serviceKey = "Send";

                        string[] dir = Directory.GetDirectories(searchPath, searchPattern, SearchOption.AllDirectories);

                        var response = services.Find(x => x.Service == messengerService && x.Operation == serviceKey);

                        if(response != null)
                        {
                            int totalNumber = 0;
                            for (int h = 0; h < dir.Length; h++)
                            {
                                totalNumber += Directory.GetFiles(dir[h]).Length;
                            }

                            if (!Directory.Exists(currentPath))
                            {
                                Directory.CreateDirectory(currentPath);
                            }

                            if (response != null)
                            {
                                MessengerFiles messengerFiles = new MessengerFiles(currentPath, serviceKey, response.LastOperationDate, totalNumber);
                                messengerFilesDictionaryObject.Add(currentPath, messengerFiles);
                            }
                        }   
                    }

                }

                counter++;
            }
            return messengerFilesDictionaryObject;
        }
    }
}