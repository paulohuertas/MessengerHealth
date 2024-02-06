using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerHealth.Models
{
    public class MessengerFiles
    {
        public string DirectoryPath;
        public DateTime LasTimeAccessed;
        public DateTime LastWrittenDate;
        public int TotalFiles;
        public string ServiceCode;

        public MessengerFiles(string file, string serviceCode, DateTime lastWrittenDate, int totalFiles)
        {
            this.DirectoryPath = file;
            this.LastWrittenDate = lastWrittenDate;
            this.TotalFiles = totalFiles;
            this.ServiceCode = serviceCode;
        }

        public MessengerFiles(string file, string serviceCode, DateTime lastWrittenDate)
        {
            this.DirectoryPath = file;
            this.LastWrittenDate = lastWrittenDate;
            this.ServiceCode = serviceCode;
        }

        public MessengerFiles() { }

        public string GetShortFileName()
        {
            try
            {
                if (String.IsNullOrEmpty(this.DirectoryPath)) return "";

                string temp = String.Empty;

                temp = this.DirectoryPath;

                temp = temp.Substring(this.DirectoryPath.LastIndexOf("\\") + 1);

                return temp;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}