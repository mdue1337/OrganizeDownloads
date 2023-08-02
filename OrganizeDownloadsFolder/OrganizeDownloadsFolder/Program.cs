using System;
using System.Data.SqlTypes;
using System.IO;

namespace OrganizeDownloadsFolder
{
    internal class Program
    {

        private static readonly string path = @"C:\Users\marti\Downloads\";

        static void Main(string[] args)
        {
            

            using (FileSystemWatcher watcher = new FileSystemWatcher(path))
            {
                watcher.Path = path;

                watcher.Created += MoveToNewFolder;

                watcher.EnableRaisingEvents = true;

                Console.WriteLine($"Monitoring changes in folder: {path}");
                Console.WriteLine("Press 'q' to quit.");

                // Wait for the user to press 'q' to exit the program
                while (Console.ReadKey().Key != ConsoleKey.Q) { }
            }
        }


        private static void MoveToNewFolder(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created && !Directory.Exists(e.FullPath))
            {
                string fileExtension = Path.GetExtension(e.FullPath); // Get the file extension from the full path

                string destinationFolderPath = Path.Combine(path, fileExtension.TrimStart('.')); // Combine the "Downloads" folder path with the file extension

                string destinationFilePath = Path.Combine(destinationFolderPath, Path.GetFileName(e.FullPath));

                if (!Directory.Exists(destinationFolderPath))
                {
                    // Create a new folder with the file extension
                    Directory.CreateDirectory(destinationFolderPath);
                }

                try
                {
                    // Move the file to the destination folder if it doesn't exist there
                    File.Move(e.FullPath, destinationFilePath);
                    Console.WriteLine($"File moved to: {destinationFilePath}");
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that might occur during the file move
                    Console.WriteLine($"Error moving the file: {ex.Message}");
                }
            }
        }

    }
}
