using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Security
{
    public class KeyReader
    {
        public KeyInformation GetKey(string provider)
        {
            var filePath = GetKeyFileName(provider);
            var fileContent = File.ReadAllText(filePath);
            var fileData = fileContent.Split(",".ToArray());
            if (fileData.Length < 2) return new KeyInformation();
            return new KeyInformation()
                {
                    Public = fileData[0], Private = fileData[1]
                };
        }

        private string GetKeyFileName(string provider)
        {
            return HttpContext.Current.Server.MapPath(string.Format(@"\{0}.auth.keys", provider));
        }
    }

    public class KeyInformation
    {
        public KeyInformation()
        {
            Public = string.Empty;
            Private = string.Empty;
        }

        public string Public { get; set; }
        public string Private { get; set; }
    }
}
