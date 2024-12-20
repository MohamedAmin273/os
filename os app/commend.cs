namespace osApp;

public class commend
{
    public static string rootDirectoryName = "/";
    public static Directory currentDirectory;
    public static string currentPath;

    public commend(string rootDirectoryName, Directory rootDirectory)
    {
        rootDirectoryName = rootDirectoryName;
        currentDirectory = rootDirectory;
        currentPath = rootDirectoryName;
    }


    public static void Clear()
    {
        Console.Clear();
    }

    public static void quit()
    {
        Environment.Exit(0);
    }

     public static void Help()  //help
        {
            Console.WriteLine("Available commands:\n");
            Console.WriteLine("cd             Displays the name of or changes the current directory.\n");
            Console.WriteLine("cls            Clear the console screen.\n");
            Console.WriteLine("dir            Displays a list of files and subdirectories in a directory.\n");
            Console.WriteLine("exit           Quits the CMD.EXE program (command interpreter).\n");
            Console.WriteLine("copy           Copies one or more files to another location.\n");
            Console.WriteLine("del            Deletes one or more files.\n");
            Console.WriteLine("help           Display help for all commands or a specific command\n");
            Console.WriteLine("md             Creates a directory.\n");
            Console.WriteLine("rd             Removes a directory.\n");
            Console.WriteLine("rename         Renames a file or files.\n");
            Console.WriteLine("type           Displays the contents of a text file.\n");
            Console.WriteLine("import         Import text file(s) from your computer.\n");
            Console.WriteLine("export         Export text file(s) to your computer.\n");
        }



        public static void DisplayHelp(string specificCommand)
        {
            Dictionary<string, string> descriptions = new Dictionary<string, string> {
            { "cd", "Displays the name of or changes the current directory" },
            { "cls", "Clear the console screen" },
            { "dir", "Displays a list of files and subdirectories in a directory." },
            { "exit", "Quits the CMD.EXE program (command interpreter)." },
            { "copy", "Copies one or more files to another location." },
            { "del", "Deletes one or more files." },
            { "help", "Display help for all commands or a specific command" },
            { "md", "Creates a directory." },
            { "rd", "Removes a directory." },
            { "rename", "Renames a file or files." },
            { "type", "Displays the contents of a text file." },
            { "import", "Import text file(s) from your computer." },
            { "export", "Export text file(s) to your computer." },  };

            if (descriptions.ContainsKey(specificCommand))
            {
                Console.WriteLine($"{specificCommand}             {descriptions[specificCommand]}");
            }
            else
            {
                Console.WriteLine($"Command '{specificCommand}' is not supported by the help utility.");
            }
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
    // public static Directory MoveToDir(string fullPath)
    // {
    //     // Split the path into parts using '/' as the delimiter
    //     string[] parts = fullPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
    //
    //     // Validate the root directory, for Linux the root is '/'
    //     if (parts.Length == 0 || !fullPath.StartsWith(rootDirectoryName))
    //     {
    //         Console.WriteLine("Error: Invalid root directory.");
    //         return null;
    //     }
    //
    //
    //     // Initialize the root directory (assuming rootDirectoryName is "/")
    //     Directory tempDirectory = new osApp.Directory(rootDirectoryName, 'D', 0, null);
    //     tempDirectory.ReadDirectory();
    //     string tempPath = rootDirectoryName; // Start from root
    //
    //     // Traverse through the path
    //     for (int i = 1; i < parts.Length; i++)
    //     {
    //         string dirName = parts[i];
    //
    //         // Search for the directory in the current directory
    //         int index = tempDirectory.SearchDirectory(dirName);
    //         if (index == -1)
    //         {
    //             Console.WriteLine($"Error: Directory '{dirName}' not found in '{tempDirectory.GetDirectoryEntry().dir_name}'.");
    //             return null;
    //         }
    //
    //         // Get the directory entry
    //         Directory_Entry nextEntry = tempDirectory.DirOrFiles[index];
    //
    //         // Instantiate the next directory
    //         tempDirectory = new Directory(new string(nextEntry.dir_name), nextEntry.dir_att, nextEntry.dir_FirstCluster, tempDirectory);
    //         tempDirectory.ReadDirectory();
    //
    //         // Update the path
    //         tempPath += "/" + dirName;
    //     }
    //
    //     // Set the current directory and path
    //     currentDirectory = tempDirectory;
    //     currentPath = tempPath;
    //
    //     
    //     return currentDirectory;
    // }
    public  static Directory MoveToDir(string fullPath)
    {
        string[] parts = fullPath.Split('\\', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0 || !parts[0].Equals(Program.rootDirectoryName, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Error: Invalid root directory.");
            return null;
        }

        Directory tempDirectory = new Directory(parts[0], 'D', 0, null);
        tempDirectory.ReadDirectory();
        string tempPath = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string dirName = parts[i];

            int index = tempDirectory.SearchDirectory(dirName);
            if (index == -1)
            {
                Console.WriteLine($"Error: Directory '{dirName}' not found in '{tempDirectory.GetDirectoryEntry().dir_name}'.");
                return null;
            }

            Directory_Entry nextEntry = tempDirectory.DirOrFiles[index];
            tempDirectory = new Directory(new string(nextEntry.dir_name), nextEntry.dir_att, nextEntry.dir_FirstCluster, tempDirectory);
            tempDirectory.ReadDirectory();

            tempPath += "\\" + dirName;
        }

        Program.currentDirectory = tempDirectory;
        Program.path = tempPath;

        return Program.currentDirectory;
    }
    // public static File_Entry MoveToFile(string fullPath)
    // {
    //     string fileName = Path.GetFileName(fullPath);
    //     string directoryPath = fullPath.Substring(0, fullPath.LastIndexOf('\\'));
    //
    //     Directory parentDirectory = MoveToDir(directoryPath);
    //     if (parentDirectory == null)
    //     {
    //         Console.WriteLine("Error: Parent directory not found.");
    //         return null;
    //     }
    //
    //     parentDirectory.ReadDirectory();
    //
    //     int index = parentDirectory.SearchDirectory(fileName);
    //     if (index == -1)
    //     {
    //         Console.WriteLine($"Error: File '{fileName}' not found in '{parentDirectory.GetDirectoryEntry().dir_name}'.");
    //         return null;
    //     }
    //
    //     Directory_Entry fileEntry = parentDirectory.DirOrFiles[index];
    //     File_Entry file = new File_Entry(fileEntry.dir_name, fileEntry.dir_att, fileEntry.dir_FirstCluster);
    //     return file;
    // }


}