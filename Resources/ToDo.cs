using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoSortIva02.Resources
{
    class ToDo
    {

        ////------------------Tasks-------------------------------
        ///
        //Y  Sort mp4 files as well
        //Y it will copy Video files to separate folder (class CopyVideoFiles)
        //Y if you just want the date the file was created (What you see in Windows Explorer) you can just use:
        //Y string file_path = @"C:\20121119_125550.avi"; //Add the correct path
        //NO DateTime result = File.GetCreationTime(file_path);


        // ? if not enough space on the disc - maybe will not do
        //Y how many space it takes all those files


        //Y - add checkbox to delete files from scanning folder
        //Y  -to add exception during the copy if file exists, copy it with text ( copy1, copy2 etc.)
        //Y  -block Exit during process

        // ProgressBar is not checked fully
             // Delete after copying for pic files does not work because Exception "The process cannot access the file because it is being used by another process"
             // I added in CopyMethods > Move method > functionality of repeat tryials in case of exception
             // also im trying to find which process holding the file and make this process iDisposable through: using(){}

        //Y -Make Text box Status -not changeble IsReadOnly="True" 
        //Y -Make possible to move windows after deleting top panel - now it is possible to drag by the TextBlock Text="Foto Sorting App"
        //Y- Added Stop button for stop Process 


        //------------------General Questions----------------------





    }
}
