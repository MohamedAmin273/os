using System;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Security.Cryptography;

namespace osApp
{
    public class MiniFAT
    {
        private static int[] FAT = new int[1024];
        //private static FileStream disk;

        // تهيئة جدول FAT بإعدادات فارغة
        public static void InitializeFAT()
        {
            for (int i = 0; i < FAT.Length; i++)
            {
                if (i == 0 || i == 4)
                {
                    FAT[i] = -1;
                }
                else if (i > 0 && i < 4)
                {
                    FAT[i] = i + 1;
                }
                else
                {
                    FAT[i] = 0;
                }
            }
        }

        public static void WriteFAT()
        {
            // تحويل جدول FAT إلى مصفوفة بايتات
            List<byte> FATBytes = Converter.IntToByteArray(FAT);

            // تقسيم FATBytes إلى قطع صغيرة لتخزينها في الـ clusters
            List<List<byte>> clusters = SplitBytes(FATBytes, 1024);

            for (int i = 0; i < clusters.Count; i++)
            {
                VirtualDisk.WriteCluster(clusters[i].ToArray(), i + 1);
            }
        }
        public static void ReadFAT()
        {
            List<byte> bytes = new List<byte>();

            // Read clusters 1 to 3 (inclusive)
            for (int i = 1; i < 4; i++)
            {
                byte[] clusterData = VirtualDisk.ReadCluster(i); // Read the cluster data once
                bytes.AddRange(clusterData); // Add the cluster's data to the list
            }

            // Convert the collected bytes into an integer array for the FAT
             Converter.ByteArrayToIntArray(FAT, bytes);
        
        }

        public static List<byte> CreateSuperBlock()
        {
            int[] arr = new int[256];
            for (int i = 0; i < 256; i++)
                arr[i] = 0;

            List<byte> super = Converter.IntToByteArray(arr);

            return super;
        }
        // تقسيم المصفوفة من البايتات إلى أجزاء صغيرة
        private static List<List<byte>> SplitBytes(List<byte> byteArray, int chunkSize)
        {
            List<List<byte>> chunks = new List<List<byte>>();
            for (int i = 0; i < byteArray.Count; i += chunkSize)
            {
                List<byte> chunk = byteArray.GetRange(i, Math.Min(chunkSize, byteArray.Count - i));
                chunks.Add(chunk);
            }
            return chunks;
        }

        public static int get_available()
        {
            for (int i = 0; i < 1024; i++)
            {
                if (FAT[i] == 0)
                    return i;
            }
            return -1;
        }
        public static void set_cluster_pointer(int start_cluster, int next_cluster)
        {
            if (start_cluster < 0 || start_cluster >= 1024)
                return;
            FAT[start_cluster] = next_cluster;
            WriteFAT();
        }

        public static int get_cluster_pointer(int cluster)
        {
            if (cluster < 0 || cluster >= 1024)
                return -1;
            return FAT[cluster];
        }
        public static int Get_Number_Of_Free_Blocks()
        {
            int count = 0;
            foreach (int f in FAT)
            {
                if (f == 0)
                {
                    count++;
                }
            }
            return count;
        }
        public static int Get_Free_Space()
        {
            return Get_Number_Of_Free_Blocks() * 1024;
        } 
        public static void InitializeOrOpenFileSystem(string name)
        {
            // إنشاء دليل الجذر
            Directory Root = new Directory("root", 'D', 0, null);
            Program.currentDirectory = Root;
            Program.path = new string(Root.dir_name);

            // فتح أو إنشاء القرص
            VirtualDisk.OpenOrCreate(name);

            if (VirtualDisk.IsNewFile())
            {
                // كتابة السوبر بلوك
                byte[] superBlock = MiniFAT.CreateSuperBlock().ToArray();
                VirtualDisk.WriteCluster(superBlock, 0);

                // تهيئة جدول FAT
                MiniFAT.InitializeFAT();
                MiniFAT.WriteFAT();

                // كتابة الدليل الجذر إلى القرص
                Root.WriteDirectory();
            }
            else
            {
                // قراءة جدول FAT
                MiniFAT.ReadFAT();

                // قراءة محتويات الجذر من القرص
                Root.ReadDirectory();
            }
        }

        
        public static void Print_Clusters()
        {
            Console.WriteLine("Fat Table: ");
            for (int i = 0; i < FAT.Length; i++)
            {
                Console.WriteLine($"Block {i}: {FAT[i]}");
            }
        }
    }
}