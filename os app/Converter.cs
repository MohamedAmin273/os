// namespace osApp
// {
//     public class Converter
//     {
//         public static List<char> IntToByte(int n)
//         {
//             List<char> bytes = new List<char>
//     {
//         (char)(n & 0xFF),
//         (char)(n >> 8 & 0xFF),
//         (char)(n >> 16 & 0xFF),
//         (char)(n >> 24 & 0xFF)
//     };
//             return bytes;
//         }
//         public static int ByteToInt(List<byte> bytes)
//         {
//             int n = 0;
//             for (int i = 0; i < bytes.Count; i++)
//             {
//                 n |= (bytes[i] & 0xFF) << i * 8;
//             }
//             return n;
//         }
//         // دي موجوده علي النت
//         //private static List<byte> IntArrayToByteArray(int[] arr)
//         //{
//         //    List<byte> byteArray = new List<byte>();
//         //    foreach (int num in arr)
//         //    {
//         //        byteArray.AddRange(BitConverter.GetBytes(num));
//         //    }
//         //    return byteArray;
//         //}
//
//         public static List<byte> IntToByteArray(int[] ints)
//         {
//             List<byte> bytes = new List<byte>();
//             for (int i = 0; i < 4; i++)
//             {
//                 int num = ints[i];
//                 bytes.Add((byte)(num & 0xFF));
//                 bytes.Add((byte)(num >> 8 & 0xFF));
//                 bytes.Add((byte)(num >> 16 & 0xFF));
//                 bytes.Add((byte)(num >> 24 & 0xFF));
//             }
//             return bytes;
//         }
//
//         public static void ByteArrayToIntArray(int[] ints, List<byte> bytes)
//         {
//             for (int i = 0; i < ints.Length; i++)
//             {
//                 List<byte> byteSegment = bytes.Skip(i * 4).Take(4).ToList();
//                 ints[i] = ByteToInt(byteSegment);
//             }
//         }
//         private static List<List<byte>> SplitBytes(List<byte> byteArray, int chunkSize)
//         {
//             List<List<byte>> chunks = new List<List<byte>>();
//             for (int i = 0; i < byteArray.Count; i += chunkSize)
//             {
//                 List<byte> chunk = byteArray.GetRange(i, Math.Min(chunkSize, byteArray.Count - i));
//                 chunks.Add(chunk);
//
//             }
//             return chunks;
//         }
//
//
//
//     };
// }

using System.Numerics;
using System.Text;

namespace osApp
{
    public class Converter
    {
        public static List<byte> DirectoryEntryToBytes(List<Directory_Entry> entries)
        {
            List<byte> bytes = new List<byte>();

            foreach (var entry in entries)
            {
                // تحويل كل مدخل (entry) إلى بايتات
                // إضافة dir_name (يجب تحويل الحروف إلى بايتات)  
                foreach (var c in entry.dir_name)
                {
                    bytes.Add((byte)c); // إضافة كل حرف كـ byte
                }

                bytes.Add((byte)entry.dir_att); // dir_att (حرف واحد)

                // إضافة dir_empty
                foreach (var c in entry.dir_empty)
                {
                    bytes.Add((byte)c); // إضافة كل حرف كـ byte
                }

                // إضافة dir_FirstCluster (int -> 4 بايتات)
                bytes.AddRange(BitConverter.GetBytes(entry.dir_FirstCluster));

                // إضافة dir_fileSize (int -> 4 بايتات)
                bytes.AddRange(BitConverter.GetBytes(entry.dir_fileSize));
            }

            return bytes;
        }

        // تحويل عدد صحيح إلى قائمة من الأحرف (Byte)
        public static List<char> IntToByte(int n)
        {
            List<char> bytes = new List<char>
            {
                (char)(n & 0xFF),
                (char)(n >> 8 & 0xFF),
                (char)(n >> 16 & 0xFF),
                (char)(n >> 24 & 0xFF)
            };
            return bytes;
        }

        // تحويل قائمة من البايتات إلى عدد صحيح
        public static int ByteToInt(List<byte> bytes)
        {
            int n = 0;
            for (int i = 0; i < bytes.Count; i++)
            {
                n |= (bytes[i] & 0xFF) << i * 8;
            }
            return n;
        }

        // تحويل عدد صحيح إلى "مدخل دليل" (Directory Entry)
        public static string IntToDirectoryEntry(int value)
        {
            // تحويل العدد الصحيح إلى قائمة بايتات ثم تحويلها إلى نص
            List<byte> bytes = new List<byte>
            {
                (byte)(value & 0xFF),
                (byte)(value >> 8 & 0xFF),
                (byte)(value >> 16 & 0xFF),
                (byte)(value >> 24 & 0xFF)
            };
            return System.Text.Encoding.ASCII.GetString(bytes.ToArray());
        }
       
        // تحويل "مدخل دليل" (Directory Entry) إلى عدد صحيح
        public static int DirectoryEntryToInt(string directoryEntry)
        {
            // تحويل النص إلى بايتات ثم دمجها كعدد صحيح
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(directoryEntry);
            int value = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                value |= (bytes[i] & 0xFF) << (i * 8);
            }
            return value;
        }

        // تحويل مصفوفة أعداد صحيحة إلى بايتات
        public static List<byte> IntToByteArray(int[] ints)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < 4; i++)
            {
                int num = ints[i];
                bytes.Add((byte)(num & 0xFF));
                bytes.Add((byte)(num >> 8 & 0xFF));
                bytes.Add((byte)(num >> 16 & 0xFF));
                bytes.Add((byte)(num >> 24 & 0xFF));
            }
            return bytes;
        }
        public static List<Directory_Entry> BytesToDirectoryEntries(List<byte> bytes)
        {
            List<Directory_Entry> entries = new List<Directory_Entry>();
            int entrySize = 32; // حجم الـ Directory_Entry هو 32 بايت

            // التأكد من أن حجم البايتات قابل للقسمة على 32
            if (bytes.Count % entrySize != 0)
            {
                throw new ArgumentException("حجم البايتات غير متوافق مع حجم Directory_Entry.");
            }

            // تقسيم البايتات إلى أجزاء بحجم 32 بايت لكل Entry
            for (int i = 0; i < bytes.Count; i += entrySize)
            {
                // أخذ جزء من البايتات بحجم 32 بايت
                List<byte> entryBytes = bytes.Skip(i).Take(entrySize).ToList();

                // تحويل الـ 32 بايت إلى Directory_Entry
                string name = Encoding.ASCII.GetString(entryBytes.Take(11).ToArray()).Trim(); // 11 بايت للاسم
                char dirAttr = (char)entryBytes[11]; // 1 بايت للـ attribute
                List<char> dirEmpty = new List<char>(); // 12 بايت للمساحة الفارغة
                for (int j = 12; j < 24; j++)
                {
                    dirEmpty.Add((char)entryBytes[j]);
                }
                int firstCluster = BitConverter.ToInt32(entryBytes.Skip(24).Take(4).ToArray(), 0); // 4 بايت للـ firstCluster
                int fileSize = BitConverter.ToInt32(entryBytes.Skip(28).Take(4).ToArray(), 0); // 4 بايت لحجم الملف

                // إنشاء Directory_Entry
                Directory_Entry entry = new Directory_Entry(name, dirAttr, firstCluster);

                // إضافة الـ entry إلى القائمة
                entries.Add(entry);
            }

            return entries;
        }

        // تحويل قائمة البايتات إلى مصفوفة أعداد صحيحة
        public static void ByteArrayToIntArray(int[] ints, List<byte> bytes)
        {
            for (int i = 0; i < ints.Length; i++)
            {
                List<byte> byteSegment = bytes.Skip(i * 4).Take(4).ToList();
                ints[i] = ByteToInt(byteSegment);
            }
        }
        

 public static List<List<byte>> SplitBytes(List<byte> bytes, int chunkSize = 1024)
    {
        List<List<byte>> result = new List<List<byte>>();

        if (bytes.Count > 0)
        {
            int numberOfArrays = bytes.Count / chunkSize;
            int rem = bytes.Count % chunkSize;

            for (int i = 0; i < numberOfArrays; i++)
            {
                List<byte> b = new List<byte>();
                for (int j = i * chunkSize, k = 0; k < chunkSize; j++, k++)
                {
                    b.Add(bytes[j]);
                }
                result.Add(b);
            }

            if (rem > 0)
            {
                List<byte> b1 = new List<byte>(chunkSize);
                for (int i = numberOfArrays * chunkSize, k = 0; k < rem; i++, k++)
                {
                    b1.Add(bytes[i]);
                }
                for (int i = rem; i < chunkSize; i++)
                {
                    b1.Add(0); // Pad with zeros
                }
                result.Add(b1);
            }
        }
        else
        {
            List<byte> b1 = new List<byte>(chunkSize);
            for (int i = 0; i < chunkSize; i++)
            {
                b1.Add(0);
            }
            result.Add(b1);
        }

        return result;
    }
        public static List<byte> StringToBytesList(string s)
        {
            List<byte> bytes = new List<byte>();
            foreach (char c in s)
            {
                bytes.Add((byte)c);
            }
            bytes.Add(0); 
            return bytes;
        }
        public static string BytesToString(List<byte> bytes)
        {
            byte[] byteArray = bytes.ToArray();
            return System.Text.Encoding.ASCII.GetString(byteArray, 0, byteArray.Length);
        }
    }
}
