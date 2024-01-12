using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MessengerHealth.Models
{
    public class Juridisction
    {
        public int JurisdictionId { get; set; }
        public string JuridisctionCode { get; set; }
        public List<Service> Services { get; set; }
    }
    public class In
    {
        public List<Procedure> Procedures { get; set; }
    }

    public class Out
    {
        public List<Procedure> Procedures { get; set; }
    }

    public class Procedure
    {
        public string procedureCode { get; set; }
        public string directoryPath { get; set; }
    }

    public class Service
    {
        public List<Out> Out { get; set; }
        public List<In> In { get; set; }
    }

    public class ReadAndParseJsonFile
    {
        private readonly string _sampleJsonFile;
        public ReadAndParseJsonFile(string jsonFile)
        {
            _sampleJsonFile = jsonFile;
        }

        public List<Juridisction> UseUserDefinedObjectJson()
        {
            using (StreamReader streamReader = new StreamReader(_sampleJsonFile))
            {
                var json = streamReader.ReadToEnd();
                List<Juridisction> juridisctions = JsonConvert.DeserializeObject<List<Juridisction>>(json);
                return juridisctions;
            }
        }
    }
}