using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;



namespace osApp
{


    public class Directory
    {
        public string dir_name;
        public char dir_attr;
        public int dir_firstCluster;
        public Directory parent;
        public List<Directory_Entry> DirOrFiles = new List<Directory_Entry>();
        // Constructor
        public Directory(string name, char dirAttr, int dirFirstCluster, Directory parentDir = null)
        {
            this.dir_name = name;
            this.dir_attr = dirAttr;
            this.dir_firstCluster = dirFirstCluster;
            this.parent = parentDir;
        }

        // Get Directory Entry
        public Directory_Entry GetDirectoryEntryByName(string name)
        {
            return DirOrFiles.FirstOrDefault(entry => new string(entry.dir_name).Trim() == name);
        }

        // دالة لإرجاع Directory_Entry بناءً على خصائص الدليل الحالي
        public Directory_Entry GetDirectoryEntry()
        {
            return new Directory_Entry(dir_name, dir_attr, dir_firstCluster);
        }

        // Get Size on Disk
        public int GetMySizeOnDisk()
        {
            int size = 0;
            if (dir_firstCluster != 0)
            {
                int cluster = dir_firstCluster;
                int next = MiniFAT.get_cluster_pointer(cluster);

                while (cluster != -1)
                {
                    size++;
                    cluster = next;
                    if (cluster != -1)
                        next = MiniFAT.get_cluster_pointer(cluster);
                }
            }
            return size * 1024; // حجم كل كلستر
        }
        

        // Add Entry
        public void AddEntry(Directory_Entry d)
        {
            DirOrFiles.Add(d);
            WriteDirectory();
        }

        // Remove Entry
        public void RemoveEntry(Directory_Entry entry)
        {
            DirOrFiles.RemoveAll(e => e.dir_name.SequenceEqual(entry.dir_name));
            WriteDirectory();
        }

        // Search Entry
        public int SearchDirectory(string name)
        {
            ReadDirectory();
            string searchName = osApp.Directory_Entry.cleanName(name);
            return DirOrFiles.FindIndex(e => new string(e.dir_name).Trim() == searchName);
        }

        // Write Directory to Disk
        public void WriteDirectory()
        {
            if (DirOrFiles.Count > 0)
            {
                // تحويل المدخلات إلى بايتات وتقسيمها إلى قطع
                List<byte> dirBytes = Converter.DirectoryEntryToBytes(DirOrFiles);
                List<List<byte>> bytesChunks = Converter.SplitBytes(dirBytes, 1024);

                // تعيين الكتلة الأولى إذا لم تكن موجودة مسبقًا
                int clusterFATIndex = dir_firstCluster != 0 ? dir_firstCluster : MiniFAT.get_available();
        
                // تحديث dir_firstCluster إذا لم تكن مُعينة مسبقًا
                dir_firstCluster = clusterFATIndex;

                int lastCluster = -1;

                // كتابة كل Chunk إلى الكتل المتوفرة
                foreach (var chunk in bytesChunks)
                {
                    // كتابة البيانات إلى الكتلة الحالية
                    VirtualDisk.WriteCluster(chunk.ToArray(), clusterFATIndex);

                    // تحديث الـ FAT
                    MiniFAT.set_cluster_pointer(clusterFATIndex, -1);
                    if (lastCluster != -1)
                    {
                        MiniFAT.set_cluster_pointer(lastCluster, clusterFATIndex);
                    }

                    // تحديث المؤشر إلى الكتلة التالية
                    lastCluster = clusterFATIndex;
                    clusterFATIndex = MiniFAT.get_available(); // الحصول على الكتلة التالية
                }
            }
        }


        // Read Directory
        public void ReadDirectory()
        {
            // تنظيف القائمة الحالية من الملفات والمجلدات
            DirOrFiles = new List<Directory_Entry>();

            // إذا لم يكن هناك كتلة أولى (دليل فارغ)، لا يوجد شيء للقراءة
            if (dir_firstCluster == 0)
            {
                Console.WriteLine("Directory is empty or uninitialized.");
                return;
            }

            // بدء قراءة البيانات من الكتلة الأولى
            int cluster = dir_firstCluster;
            List<byte> data = new List<byte>();

            // قراءة الكتل المرتبطة بالدليل من VirtualDisk باستخدام MiniFAT
            while (cluster != -1)
            {
                // قراءة بيانات الكتلة الحالية
                byte[] clusterData = osApp.VirtualDisk.ReadCluster(cluster);
                //
                // // إضافة البيانات إلى القائمة
                data.AddRange(clusterData);

                // الانتقال إلى الكتلة التالية باستخدام MiniFAT
                cluster = MiniFAT.get_cluster_pointer(cluster);
            }

            // تحويل البيانات الخام إلى إدخالات دليل
            DirOrFiles = Converter.BytesToDirectoryEntries(data);

            Console.WriteLine($"Read {DirOrFiles.Count} entries from directory '{dir_name}'.");
        }
        public void UpdateContent(Directory_Entry oldEntry, Directory_Entry newEntry)
        {
            // Find the index of the old entry
            int index = SearchDirectory(oldEntry.dir_name.ToString());

            // If the old entry was found, update it with the new entry
            if (index != -1)
            {
                DirOrFiles[index] = newEntry;
            }
        }
        public bool canadd(Directory_Entry d)
        {
            int needs = (DirOrFiles.Count + 1) / 32;
            int needc = needs / 1024;
            int res = needs % 1024;
            if (res > 0)
                needc++;
            needc += d.dir_fileSize / 1024;
            int re1 = d.dir_fileSize % 1024;
            if(re1>0) needc++;
            return GetMySizeOnDisk() + MiniFAT.get_available() >= needc;

        }
       

       
        
    }
}


