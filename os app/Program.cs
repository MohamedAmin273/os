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
    }
}