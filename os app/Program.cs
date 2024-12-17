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
using osApp;




internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        VirtualDisk.OpenOrCreate("/home/oem/Desktop/ana/hash.txt");
        //VirtualDisk.WriteCluster("1", 10);
        byte[] data = new byte[1024];
        for (int i = 0; i < data.Length; i++)
            data[i] = (byte)(i % 256);
        

        // كتابة البيانات في الموقع الأول
        VirtualDisk.WriteCluster(data, 5);
        MiniFAT.ReadFAT();
    }
}