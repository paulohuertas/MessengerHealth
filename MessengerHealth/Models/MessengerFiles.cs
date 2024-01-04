using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerHealth.Models
{
    public class MessengerFiles
    {
        public string FileName;
        public DateTime LasTimeAccessed;
        public static DateTime lastWrittenDate;

        public MessengerFiles(string file, DateTime lastTimeAccessed)
        {
            this.FileName = file;
            this.LasTimeAccessed = lastTimeAccessed;
        }

        public string GetShortFileName()
        {
            try
            {
                if (String.IsNullOrEmpty(this.FileName)) return "";

                string temp = String.Empty;

                temp = this.FileName;

                temp = temp.Substring(this.FileName.LastIndexOf("\\") + 1);

                return temp;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}