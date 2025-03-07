namespace FixingBug
{
    public class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"C:\YourDirectory";

            List<string> txtFiles = new List<string>();
            GetTxtFiles(directoryPath, txtFiles);

            foreach (var file in txtFiles)
            {
                AppendTextToFile(file, "ASPEKT");
            }
        }

        static void GetTxtFiles(string directoryPath, List<string> txtFiles)
        {
            string[] files = Directory.GetFiles(directoryPath, "*.txt");
            txtFiles.AddRange(files);

            string[] subdirectories = Directory.GetDirectories(directoryPath);
            foreach (string subdirectory in subdirectories)
            {
                //GetTxtFiles(directoryPath, txtFiles);
                // when direcotryPath is used it creates an infinite loop
                GetTxtFiles(subdirectory, txtFiles);

            }
        }

        static void AppendTextToFile(string filePath, string textToAppend)
        {
            //StreamWriter writer = null;
            //writer = File.AppendText(filePath);

            // can be assigned diretly
            StreamWriter writer = File.AppendText(filePath);
            writer.WriteLine(textToAppend);
            // missing close - To close the StreamWriter
            writer.Close();
        }
    }

}
