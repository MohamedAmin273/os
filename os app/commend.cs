namespace osApp;

public class commend
{
    private static Directory currentDirectory;
    private static string currentPath;
    public static string rootDirectoryName = "S:";

    public commend(string rootDirectoryName, Directory rootDirectory)
    {
        rootDirectoryName = rootDirectoryName;
        currentDirectory = rootDirectory;
        currentPath = rootDirectoryName;
    }


    public void Clear()
    {
        Console.Clear();
    }

    public static void quit()
    {
        Environment.Exit(0);
    }

//     public static Directory MoveToDir(string fullPath)
//     {
//         // Split the path into parts
//         string[] Parts = fullPath.Split('\\');
//
// // Validate the root directory name
//         // if (Parts.Length == 0 || Parts[0] != rootDirectoryName)
//         // {
//         //     Console.WriteLine("Error: Invalid root directory. Expected root: " + rootDirectoryName);
//         //     return null;
//         // }
//
// // Initialize the root directory
//         Directory tempDirectory = new Directory(rootDirectoryName, 'D', 0, null);
//         tempDirectory.ReadDirectory();
//         string tempPath = rootDirectoryName;
//
// // Traverse through the path
//         for (int i = 1; i < Parts.Length; i++)
//         {
//             string dirName = Parts[i];
//             // Search for the directory in the current directory
//             int index = tempDirectory.SearchDirectory(tempDirectory.dir_name);
//             if (index == -1)
//             {
//                 Console.WriteLine(
//                     $"Error: Directory '{dirName}' not found in '{tempDirectory.GetDirectoryEntry().dir_name}'.");
//                 return null;
//             }
//
//             Directory_Entry nextEntry = tempDirectory.DirOrFiles[index];
//
//             // Instantiate the next directory
//             tempDirectory = new Directory(nextEntry.dir_name.ToString(), nextEntry.dir_att, nextEntry.dir_FirstCluster,
//                 tempDirectory);
//             tempDirectory.ReadDirectory();
//             // Edit Path
//             tempPath += "\\" + dirName;
//         }
//
//         currentDirectory = tempDirectory;
//         currentPath = tempPath;
//
//         return currentDirectory;
//     }
    public static Directory MoveToDir(string fullPath)
    {
        // Split the path into parts using '/' as the delimiter
        string[] Parts = fullPath.Split("S:");

        // Validate the root directory, for Linux the root is '/'
        if (Parts.Length == 0 || Parts[0] != rootDirectoryName)  // تأكد من أنك تتحقق من الجذر بشكل صحيح
        {
            Console.WriteLine("Error: Invalid root directory.");
            return null;
        }

        // Initialize the root directory (assuming rootDirectoryName is "/")
        Directory tempDirectory = new Directory(rootDirectoryName, 'D', 0, null);
        tempDirectory.ReadDirectory();
        string tempPath = rootDirectoryName; // Start from root

        // Traverse through the path
        for (int i = 1; i < Parts.Length; i++)
        {
            string dirName = Parts[i];

            // Search for the directory in the current directory
            int index = tempDirectory.SearchDirectory(dirName);
            if (index == -1)
            {
                Console.WriteLine($"Error: Directory '{dirName}' not found in '{tempDirectory.GetDirectoryEntry().dir_name}'.");
                return null;
            }

            // Get the directory entry
            Directory_Entry nextEntry = tempDirectory.DirOrFiles[index];

            // Instantiate the next directory
            tempDirectory = new Directory(new string(nextEntry.dir_name), nextEntry.dir_att, nextEntry.dir_FirstCluster, tempDirectory);
            tempDirectory.ReadDirectory();

            // Update the path
            tempPath += "/" + dirName;
        }

        // Set the current directory and path
        currentDirectory = tempDirectory;
        currentPath = tempPath;

        return currentDirectory;
    }

}