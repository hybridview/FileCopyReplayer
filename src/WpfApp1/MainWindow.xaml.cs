using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FileCopyReplayer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SourceFileList = new List<string>();

            OrigWidth = this.Width;
            OrigHeight = this.Height;
        }

        public List<string> SourceFileList { get; set; }

        public double OrigWidth { get; set; }
        public double OrigHeight { get; set; }

        private void selectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            try {

            if (!Directory.Exists(sourceFolderTextBox.Text))
            {
                throw new Exception("Source directory '" + sourceFolderTextBox.Text + "' not found! Please check your input.");
            }

            // Open file choose dialog.
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog
            {
                Multiselect = true, InitialDirectory = sourceFolderTextBox.Text
            };
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                SourceFileList = openFileDialog.FileNames.ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(SourceFileList.Count + " file(s).");
                foreach (string f in openFileDialog.FileNames)
                {
                    sb.AppendLine(f);
                }

                filesTextBox.Text = sb.ToString();
                // openFileDialog.
            }
                // txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message);
            }
        }

        private void chooseSourceFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog {IsFolderPicker = true};
            //CommonFileDialogResult result = dialog.ShowDialog();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                sourceFolderTextBox.Text = dialog.FileName;
                this.Title = "File Replayer - " + dialog.FileName;

                // Load all files by default.
                SourceFileList = Directory.EnumerateFiles(dialog.FileName).ToList();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(SourceFileList.Count + " file(s).");
                foreach (var f in SourceFileList)
                {
                    sb.AppendLine(f);
                }
                filesTextBox.Text = sb.ToString();
                // openFileDialog.
            }

        }

        private void chooseDestFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog {IsFolderPicker = true};
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                destFolderTextBox.Text = dialog.FileName;
            }
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string destDir = destFolderTextBox.Text;

                if (!Directory.Exists(destDir))
                {
                    throw new Exception("Destination directory '" + destDir + "' not found! Please check your input.");
                }

                int copyFileCount = 0;
                int skippedFileCount = 0;

                string messages = "";

                foreach (string fl in SourceFileList)
                {
                    if (!File.Exists(fl))
                    {
                        //throw new Exception("Source file '" + fl + "' not found! It may have been deleted at some point. File has been skipped.");
                        string msg = "Source file '" + fl + "' not found! It may have been deleted at some point. File has been skipped.";
                        messages += msg + "\r\n";
                        skippedFileCount++;
                    }
                    else
                    {
                        var srcFile = new FileInfo(fl);
                        //fi.LastWriteTime
                        string destFilePath = System.IO.Path.Combine(destDir, srcFile.Name);
                        if (File.Exists(destFilePath))
                        {
                            var destFile = new FileInfo(destFilePath);

                            if (srcFile.LastAccessTime > destFile.LastWriteTime)
                            {
                                // Copy file is newer than existing.
                                _CopyFile(srcFile, destDir);
                                copyFileCount++;
                            }
                            else
                            {
                                skippedFileCount++;
                            }
                        }
                        else
                        {
                            _CopyFile(srcFile, destDir);
                            copyFileCount++;
                        }
                    }



                }

                resultsLabel.Content = string.Format("{0:hh:mm:ss} - Copied {1} file(s) to {2}. {3} file(s) skipped.",
                    DateTime.Now, copyFileCount, destDir, skippedFileCount);


                if (!string.IsNullOrEmpty(messages))
                {
                    MessageBox.Show("Info: " + messages);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message);
            }

        }


        private void _CopyFile(FileInfo srcFileInfo, string destFolderPath)
        {


            File.Copy(srcFileInfo.FullName, System.IO.Path.Combine(destFolderPath, srcFileInfo.Name), true);

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
             this.Width= OrigWidth;
             this.Height= OrigHeight;
        }
    }
}
