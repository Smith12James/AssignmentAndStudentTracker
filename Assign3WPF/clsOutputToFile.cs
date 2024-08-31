using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Assign3WPF
{
    class clsOutputToFile
    {
        /// <summary>
        /// write to file using stringbuilder as info to write to the file, and file name as the name of the file
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="sFileName"></param>
        public bool writeToFile(StringBuilder sb, string sFileName)
        {
            try
            {
                if (sFileName == "" || sFileName == null)
                {
                    sFileName = "ScoresOutput";

                }

                bool bCheckFile = checkFileName("C:\\Users\\Public\\" + sFileName + ".txt");

                if (bCheckFile)
                {
                    string message = "This file name exists. Would you like to overwrite the file?";
                    string caption = "File Name Exists";

                    MessageBoxButton buttons = MessageBoxButton.YesNo;

                    var result = MessageBox.Show(message, caption, buttons);

                    if (result == MessageBoxResult.Yes)
                    {
                        outputFile(sb, sFileName);

                    }
                    else
                    {
                        return false;

                    }

                }
                else
                {
                    outputFile(sb, sFileName);

                }

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

        }

        /// <summary>
        /// check file name will check if the file name is already created and return a boolean depending on status of check
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        private bool checkFileName(string sFileName)
        {
            try
            {
                bool bIsFile = false;

                bIsFile = File.Exists(sFileName) ? true : false;

                return bIsFile;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

        }

        /// <summary>
        /// this will create or overwrite the file using file name and stringbuilder as info to write.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="sFileName"></param>
        private async void outputFile(StringBuilder sb, string sFileName)
        {
            try
            {
                Thread.Sleep(2000);

                string sFileLocation = "C:\\Users\\Public";

                using (StreamWriter outputFile = new StreamWriter($"{sFileLocation}\\{sFileName}.txt", false)) // false to overwrite file if there is one
                {
                    await outputFile.WriteAsync(sb.ToString());

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

        }

    }
}
