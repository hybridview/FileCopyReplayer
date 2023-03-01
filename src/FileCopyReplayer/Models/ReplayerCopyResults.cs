namespace FileCopyReplayer.Models;

public class ReplayerCopyResults
{
    public string DestinationPath { get; set; } = "";

    public string ErrorMessages { get; set; } = "";

    public  int CopyFileCount { get; set; } = 0;
    public  int SkippedFileCount { get; set; } = 0;

        
}