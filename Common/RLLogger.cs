using System.IO;
using System.Text;

namespace RateLimiterWeb.Common
{
    public class RLLogger : IRLLogger
    {
       // StreamWriter writer;
        string filePath;
        public RLLogger()
        {
            string DateTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");
             filePath= "c:/Log" + DateTimeString + ".txt";            
        }

        public  void DEBUG(string msg)
        {
            var message = DateTime.Now.ToString("yyyyMMddHHmmss") + " DEBUG " + "- " + msg;
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(message);
            };
        }
        public  void INFO(string msg)
        {
            var message = DateTime.Now.ToString("yyyyMMddHHmmss") + " INFO " + "- " + msg;
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(message);
            }; ;
        }
        public  void ERROR(string msg)
        {
            var message = DateTime.Now.ToString("yyyyMMddHHmmss") + " ERROR " + "- " + msg;
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(message);
            };
        }
        private void WritetoFile( string message)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(message);
            };

        }
    }
}
