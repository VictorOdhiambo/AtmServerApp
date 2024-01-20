namespace ServiceLayer
{
    public class LoggerService
    {
        public static void LogException(string path, string exception)
        {
            // Creates the folder if it does not exists
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            // create a new log file with the current system date
            var logFilePath = Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + ".log");

            // build the exception message
            var errMsg = DateTime.Now.ToString("dd-MMM-yyy HH:mm:ss") + " : Error >>> " + exception;

         
            using (FileStream fs = new FileStream(logFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs)) { }
            }

            //Error Logging Starts

            using (FileStream fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                    sw.WriteLine(errMsg);
            }
        }
    }
}
