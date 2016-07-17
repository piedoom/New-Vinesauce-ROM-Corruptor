using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinesauce_ROM_Corrupter
{
    class File
    {
        /// <summary>
        /// The filename without full path
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The full system path of the file
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Files used for listing ROMs
        /// </summary>
        /// <param name="fullPath">The full system path to the file used in operations</param>
        public File(string fullPath)
        {
            FullPath = fullPath;
            FileName = Path.GetFileName(fullPath);
        }

        internal static byte[] ReadAllBytes(string filePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs an array of Files based on a string array usually gotten from file methods
        /// </summary>
        /// <param name="files">The array of strings to pass into the object</param>
        /// <returns>a list of file objects</returns>
        public static List<File> FileListBuilder(String[] files)
        {
            // init an empty list
            List<File> finalFileList = new List<File>();

            foreach (string filePath in files)
            {
                // build our object and append it to our list
                finalFileList.Add(new File(filePath));
            }

            // return our list of files when done
            return finalFileList;
        }

        /// <summary>
        /// Return a new file name with a special string appended so the original ROM is not overwritten
        /// </summary>
        /// <param name="filename">the filename (not path) to parse</param>
        /// <returns></returns>
        public static string ConvertFileNameToCorruptedName(string filename)
        {
            return Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename) + "-corrupted" + Path.GetExtension(filename);
        }
    }
}
