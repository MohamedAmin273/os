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
                List<byte> dirBytes = Converter.DirectoryEntryToBytes(DirOrFiles);
                List<List<byte>> bytesChunks = Converter.SplitBytes(dirBytes, 1024);

                int clusterFATIndex = dir_firstCluster != 0 ? dir_firstCluster : MiniFAT.get_available();
                dir_firstCluster = clusterFATIndex;

                int lastCluster = -1;

                foreach (var chunk in bytesChunks)
                {
                    if (clusterFATIndex == -1) break;

                    VirtualDisk.WriteCluster(chunk.ToArray(), clusterFATIndex);
                    MiniFAT.set_cluster_pointer(clusterFATIndex, -1);

                    if (lastCluster != -1)
                        MiniFAT.set_cluster_pointer(lastCluster, clusterFATIndex);

                    lastCluster = clusterFATIndex;
                    clusterFATIndex = MiniFAT.get_available();
                }
            }
        }

        // Read Directory
        public void ReadDirectory()
        {
            DirOrFiles.Clear();

            if (dir_firstCluster == 0) return;

            int cluster = dir_firstCluster;
            int next = MiniFAT.get_cluster_pointer(cluster);
            List<byte> data = new List<byte>();
            while (cluster != -1)
            {
                byte[] clusterData = VirtualDisk.ReadCluster(cluster);
                data.AddRange(clusterData);

                cluster = next;
                if (cluster != -1)
                    next = MiniFAT.get_cluster_pointer(cluster);
            }

            DirOrFiles = Converter.BytesToDirectoryEntries(data);
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


