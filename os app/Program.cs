using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using osApp;
using Directory = osApp.Directory;


// internal class Program
// {


internal class Program
{
<<<<<<< HEAD
    private List<string> commandList = new List<string>
        { "export", "import", "copy", "type", "rename", "rd", "cd", "md", "help", "quit", "cls", "dir", "del" };

    public void parser(List<string> list)
    {
        if (!commandList.Contains(list[0]))
        {
            Console.WriteLine("Error : " + list[0] + " => This Commnand is not supported");
        }
=======
    private List<string> commandList = new List<string> { "export","import","copy","type","rename","rd","cd","md","help","quit","cls","dir","del"};
    
    public void parser(List<string> list)
    {
        if (!commandList.Contains(list[0]))
        {
            Console.WriteLine("Error : " + list[0] + " => This Commnand is not supported");
        }
    }

    public static List<string> Teconizer(string s)
    {
        List<string> list = new List<string>();
        s = s.Trim(); // Trim the input to remove leading/trailing spaces

        StringBuilder current = new StringBuilder(); // Use StringBuilder for mutable strings
        bool insideQuotes = false; // Flag to track if we're inside quotes

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            if (c == '\"') // Toggle the insideQuotes flag
            {
                insideQuotes = !insideQuotes;

                // Skip the quote character itself
                continue;
            }

            if (c == ' ' && !insideQuotes) // Space outside quotes indicates the end of a token
            {
                if (current.Length > 0)
                {
                    list.Add(current.ToString().Trim());
                    current.Clear(); // Clear the StringBuilder for the next token
                }
            }
            else
            {
                current.Append(c); // Append the character to the current token
            }
        }

        // Add the last token if it exists
        if (current.Length > 0)
        {
            list.Add(current.ToString().Trim());
        }

        return list;
    }

    static void Main(string[] args)
    {
        // string s = "  cd \"c:\\direct ory\\fsd.txt\"   f.txt ";
        // List<string> list = Teconizer(s);
        //
        // foreach (var l in list)
        // {
        //     Console.WriteLine(l);
        // }
        while (true)
        {
             commend.MoveToDir(Console.ReadLine());
                    
        }
       

        // Console.ReadKey();
>>>>>>> origin/main
    }

    public static List<string> Teconizer(string s)
    {
        List<string> list = new List<string>();
        s = s.Trim(); // Trim the input to remove leading/trailing spaces

        StringBuilder current = new StringBuilder(); // Use StringBuilder for mutable strings
        bool insideQuotes = false; // Flag to track if we're inside quotes

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            if (c == '\"') // Toggle the insideQuotes flag
            {
                insideQuotes = !insideQuotes;

                // Skip the quote character itself
                continue;
            }

            if (c == ' ' && !insideQuotes) // Space outside quotes indicates the end of a token
            {
                if (current.Length > 0)
                {
                    list.Add(current.ToString().Trim());
                    current.Clear(); // Clear the StringBuilder for the next token
                }
            }
            else
            {
                current.Append(c); // Append the character to the current token
            }
        }

        // Add the last token if it exists
        if (current.Length > 0)
        {
            list.Add(current.ToString().Trim());
        }

        return list;
    }

    // static void Main(string[] args)
    // {
    //     string diskName = "VirtualDisk";
    //
    //     // استدعاء دالة التهيئة أو الفتح
    //     MiniFAT.InitializeOrOpenFileSystem(diskName);
    //
    //     // مثال على استدعاء MoveToDir
    //     string targetPath = "/root/subdir1";
    //
    //     Directory targetDirectory = commend.MoveToDir(targetPath);
    //
    //     if (targetDirectory != null)
    //     {
    //         Console.WriteLine($"Successfully moved to directory: {currentPath}");
    //     }
    //     else
    //     {
    //         Console.WriteLine("Failed to move to the target directory.");
    //     }
    // }
    public static Directory currentDirectory;
    public static string path;
    public static string rootDirectoryName = "H:";  // اسم الدليل الجذر
    public static string DiskFileName = "VirtualDisk.txt";

    static void Main(string[] args)
    {
        // تهيئة أو فتح القرص الافتراضي
        MiniFAT.InitializeOrOpenFileSystem(DiskFileName);

        // عرض محتويات الدليل الجذر
        // DisplayRootDirectory();

        // اختبار MoveToDir
        string dirPath = "VirtualDisk.txt";  // مثال لمسار الدليل
        Directory directory = commend.MoveToDir(dirPath);
        if (directory != null)
        {
            Console.WriteLine($"تم الانتقال إلى الدليل: {directory.GetDirectoryEntry().dir_name}");
        }

        // اختبار MoveToFile
        // string filePath = "H:\\Documents\\Projects\\file.txt";  // مثال لمسار الملف
        // File_Entry file = MoveToFile(filePath);
        // if (file != null)
        // {
        //     Console.WriteLine($"تم العثور على الملف: {file.GetFileEntry().fileName}");
        // }
    

        while (true)
        {
        
            Console.Write(path + "\\> ");
            string input = Console.ReadLine().ToLower().Trim();
            string[] commandParts = input.Split(' ');
            string command = commandParts.Length > 0 ? commandParts[0] : "";
        
        
            switch (command)
            {
                case "help":
                    if (commandParts.Length > 1)
                    {
                        // Help
                        commend.DisplayHelp(commandParts[1]);
                    }
                    else
                    {
                        // All commands
                        commend.Help();
                    }
        
                    break;
        
        
                case "cls":
                    // cls
                    commend.Clear();
                    ;
                    break;
        
        
                case "exit":
                case "quit":
                    //Exit or quit
                    osApp.commend.quit();
                    ;
                    break;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }
}
    