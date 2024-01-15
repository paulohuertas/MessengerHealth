using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MessengerHealth.Models
{
    public class In
    {
        [JsonProperty("procedureCode")]
        public string ProcedureCode { get; set; }

        [JsonProperty("directoryPath")]
        public string DirectoryPath { get; set; }
    }

    public class Out
    {
        [JsonProperty("procedureCode")]
        public string ProcedureCode { get; set; }

        [JsonProperty("directoryPath")]
        public string DirectoryPath { get; set; }
    }

    public class Procedures
    {
        [JsonProperty("services")]
        public Services Services { get; set; }
    }

    public class Jurisdiction
    {
        [JsonProperty("jurisdictionId")]
        public int JurisdictionId { get; set; }

        [JsonProperty("juridisctionCode")]
        public string JuridisctionCode { get; set; }

        [JsonProperty("procedures")]
        public Procedures Procedures { get; set; }
    }

    public class Services
    {
        [JsonProperty("Out")]
        public List<Out> Out { get; set; }

        [JsonProperty("In")]
        public List<In> In { get; set; }
    }

    public class ReadAndParseJsonFile
    {
        private readonly string _sampleJsonFile;
        public ReadAndParseJsonFile(string jsonFile)
        {
            _sampleJsonFile = jsonFile;
        }

        public List<Jurisdiction> UseUserDefinedObjectJson()
        {
            using (StreamReader streamReader = new StreamReader(_sampleJsonFile))
            {
                var json = streamReader.ReadToEnd();
                List<Jurisdiction> juridisctions = JsonConvert.DeserializeObject<List<Jurisdiction>>(json);
                return juridisctions;
            }
        }
    }
}