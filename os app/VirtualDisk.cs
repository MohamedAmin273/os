namespace osApp
{
    public class VirtualDisk
    {
        private static FileStream disk;
        public bool IsNew { get; private set; } = false;

        // This function is used to open or create a file
        public static void OpenOrCreate(string path)
        {
            if (File.Exists(path))
            {
                disk = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            }
            else
            {
                disk = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                // disk.Close();
                // disk = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            }
        }

        // This function is used to write a cluster to the disk
        public static void WriteCluster(byte[] data, int sizeIndex)
        {
            if (disk == null)
                throw new InvalidOperationException("Disk is not initialized. Call OpenOrCreate first.");

            disk.Seek(1024 * sizeIndex, SeekOrigin.Begin);
            disk.Write(data, 0, Math.Min(1024, data.Length));
            disk.Flush();
        }

        // This function is used to read a cluster from the disk
        public static byte[] ReadCluster(int sIndex)
        {
            if (disk == null)
                throw new InvalidOperationException("Disk is not initialized. Call OpenOrCreate first.");

            disk.Seek(sIndex * 1024, SeekOrigin.Begin);
            byte[] data = new byte[1024];
            disk.Read(data, 0, 1024);
            return data;
        }

        // This function is used to check if the file is new or not
        public static bool IsNewFile()
        {
            if (disk == null)
                throw new InvalidOperationException("Disk is not initialized. Call OpenOrCreate first.");

            return disk.Length == 0;
        }

        // This function is used to get the length of the file in bytes
        public static int GetLength()
        {
            if (disk == null)
                throw new InvalidOperationException("Disk is not initialized. Call OpenOrCreate first.");

            return 1024 * 1024 - (int)disk.Length;
        }


    }
}