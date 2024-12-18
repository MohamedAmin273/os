

namespace osApp
{
    public class File_Entry : Directory_Entry
    {
        public string Content { get; private set; }
        public Directory Parent { get; private set; }
        public File_Entry(string name, char dirAttr, int firstCluster, Directory parent)
       : base(name, dirAttr, firstCluster)
        {
            Content = string.Empty;
            Parent = parent;
        }
        public File_Entry(Directory_Entry entry, Directory parent)
        : base(entry.dir_name.ToString(), entry.dir_att, entry.dir_FirstCluster)
        {
            for (int i = 0; i < 12; i++)
            {
                dir_empty[i] = entry.dir_empty[i];
            }
            dir_fileSize = entry.dir_fileSize;
            Content = string.Empty;
            Parent = parent;
        }
        public int GetMySizeOnDisk()
        {
            int size = 0;

            if (dir_FirstCluster != 0)
            {
                int cluster = dir_FirstCluster;
                int next = MiniFAT.get_cluster_pointer(cluster);

                do
                {
                    size++;
                    cluster = next;

                    if (cluster != -1)
                    {
                        next = MiniFAT.get_cluster_pointer(cluster);
                    }
                } while (cluster != -1);
            }

            return size;
        }

        public void EmptyMyClusters()
        {
            if (dir_FirstCluster != 0)
            {
                int cluster = dir_FirstCluster;
                int next = MiniFAT.get_cluster_pointer(cluster);

                do
                {
                    MiniFAT.set_cluster_pointer(cluster, 0);
                    cluster = next;

                    if (cluster != -1)
                    {
                        next = MiniFAT.get_cluster_pointer(cluster);
                    }
                } while (cluster != -1);
            }
        }

        public Directory_Entry GetDirectoryEntry()
        {
            var me = new Directory_Entry(dir_name.ToString(), dir_att, dir_FirstCluster);

            for (int i = 0; i < 12; i++)
            {
                me.dir_empty[i] = dir_empty[i];
            }

            me.dir_fileSize = dir_fileSize;
            return me;
        }
        public void WriteFileContent()
        {
            Directory_Entry me = GetDirectoryEntry();

            if (!string.IsNullOrEmpty(Content))
            {
                List<byte> contentBytes = Converter.StringToBytesList(Content);
                List<List<byte>> bytesList = Converter.SplitBytes(contentBytes);

                int clusterFATIndex;

                if (dir_FirstCluster != 0)
                {
                    EmptyMyClusters();
                    clusterFATIndex = MiniFAT.get_available();
                    dir_FirstCluster = clusterFATIndex;
                }
                else
                {
                    clusterFATIndex = MiniFAT.get_available();
                    if (clusterFATIndex != -1)
                    {
                        dir_FirstCluster = clusterFATIndex;
                    }
                }

                int lastCluster = -1;
                foreach (var chunk in bytesList)
                {
                    if (clusterFATIndex != -1)
                    {
                        VirtualDisk.WriteCluster(chunk.ToArray(), clusterFATIndex);
                        MiniFAT.set_cluster_pointer(clusterFATIndex, -1); // Mark as end of the chain

                        if (lastCluster != -1)
                        {
                            MiniFAT.set_cluster_pointer(lastCluster, clusterFATIndex); // Link previous cluster to current cluster
                        }

                        lastCluster = clusterFATIndex;
                        clusterFATIndex = MiniFAT.get_available(); // Get next available cluster for next chunk
                    }
                }
            }
            else
            {
                if (dir_FirstCluster != 0)
                    EmptyMyClusters();

                dir_FirstCluster = 0;
            }

            Directory_Entry updatedEntry = GetDirectoryEntry();
            if (Parent != null)
            {
                Parent.UpdateContent(me, updatedEntry);
                Parent.WriteDirectory();
            }

            MiniFAT.WriteFAT();
        }

        public void ReadFileContent()
        {
            if (dir_FirstCluster != 0)
            {
                Content = string.Empty;

                int cluster = dir_FirstCluster;
                int next = MiniFAT.get_cluster_pointer(cluster);

                List<byte> data = new List<byte>();

                do
                {
                    List<byte> clusterData = VirtualDisk.ReadCluster(cluster).ToList();
                    data.AddRange(clusterData);

                    cluster = next;

                    if (cluster != -1)
                    {
                        next = MiniFAT.get_cluster_pointer(cluster);
                    }
                } while (cluster != -1);

                Content = Converter.BytesToString(data);
            }
        }

        public void DeleteFile()
        {
            EmptyMyClusters();

            if (Parent != null)
            {
                Parent.RemoveEntry(GetDirectoryEntry());
            }
        }
        public void PrintContent()
        {
            Console.WriteLine($"\n{dir_name}\n\n{Content}\n");
        }
    }
}