using FileCopyReplayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileCopyReplayer
{
    public class ReplayerService
    {


        public void CopyFiles(MainModel m, out ReplayerCopyResults results)
        {
            //try
            //{
            results = new ReplayerCopyResults();
            results.DestinationPath = m.DestinationPath;
                if (!Directory.Exists(m.DestinationPath))
                {
                    throw new Exception("Destination directory '" + m.DestinationPath + "' not found! Please check your input.");
                }

                results.CopyFileCount = 0;
                results.SkippedFileCount = 0;

                //string messages = "";

                foreach (string fl in m.SourceFileList)
                {
                    if (!File.Exists(fl))
                    {
                        //throw new Exception("Source file '" + fl + "' not found! It may have been deleted at some point. File has been skipped.");
                        string msg = "Source file '" + fl + "' not found! It may have been deleted at some point. File has been skipped.";
                        results.ErrorMessages += msg + "\r\n";
                        results.SkippedFileCount++;
                    }
                    else
                    {
                        var srcFile = new FileInfo(fl);
                        //fi.LastWriteTime
                        string destFilePath = System.IO.Path.Combine(m.DestinationPath, srcFile.Name);
                        if (File.Exists(destFilePath))
                        {
                            var destFile = new FileInfo(destFilePath);

                            if (srcFile.LastAccessTime > destFile.LastWriteTime)
                            {
                                // Copy file is newer than existing.
                                _CopyFile(srcFile, m.DestinationPath);
                                results.CopyFileCount++;
                            }
                            else
                            {
                                results.SkippedFileCount++;
                            }
                        }
                        else
                        {
                            _CopyFile(srcFile, m.DestinationPath);
                            results.CopyFileCount++;
                        }
                    }



                }



                //if (!string.IsNullOrEmpty(messages))
                //{
                //    MessageBox.Show("Info: " + messages);
                //}
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error! " + ex.Message);
            //}

        }


        private void _CopyFile(FileInfo srcFileInfo, string destFolderPath)
        {
            File.Copy(srcFileInfo.FullName, System.IO.Path.Combine(destFolderPath, srcFileInfo.Name), true);
        }
    }
}
