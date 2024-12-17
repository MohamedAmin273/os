namespace osApp
{

    public class Directory_Entry
    {
        public  char[] dir_name = new char[11];
        public  char dir_att;
        public  char[] dir_empty = new char[12];
        public  int dir_FirstCluster;
        public  int dir_fileSize;

        public  Directory_Entry(string name, char dirAtt, int dir_FCluster, int sizeF)
        {
            string clean = cleanName(name);
            for (int i = 0; i < 11; i++)
            {
                if (i < clean.Length)
                {
                    dir_name[i] = clean[i];
                }
                else
                {
                    dir_name[i] = ' ';
                }
            }
            dir_att = dirAtt;
            dir_FirstCluster = dir_FCluster;
            dir_fileSize = sizeF;
        }
        public static string cleanName(string N)
        {
            string clean_Name = "";
            for (int i = 0; i < N.Length; i++)
            {
                if (N[i] != '/' && N[i] != '?' && N[i] != '@' && N[i] != '$' && N[i] != '%' && N[i] != '&')
                {
                    clean_Name += N[i];
                }
            }
            return clean_Name;
        }
        public static void assignFileName(string name, string extension,Directory_Entry d)
        {
            
            string fullName = cleanName(name) + "." + "txt";
            for (int i = 0; i < 11; i++)
            {
                if (i < fullName.Length)
                {
                    d.dir_name[i] = fullName[i];
                }
                else
                {
                    d.dir_name[i] = ' ';
                }
            }
        }

        public static void assignDIRName(string name,Directory_Entry d)
        {
            string clean = cleanName(name);
            if (clean.Length > 11)
            {
                clean = clean.Substring(0, 11);
            }

            for (int i = 0; i < 11; i++)
            {
                if (i < clean.Length)
                {
                    d.dir_name[i] = clean[i];
                }
                else
                {
                   d.dir_name[i] = ' ';
                }
            }
        }
    }
}