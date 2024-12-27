using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace POSRestaurant.Sale
{
   public class OpenDrawerClass
    {
        public static byte[] GetDocument()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {

                    pr(bw);
                
                
                bw.Flush();

                return ms.ToArray();
            }
        }
        public static void pr(BinaryWriter bw)
        {
            bw.NormalFont(System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, 112, 48, 55, 121 }));
        }
       public static void Print(string printerName)
       {

           NativeMethods.DOC_INFO_1 documentInfo;
           IntPtr printerHandle;
           byte[] managedData = null;
           string addrs = "";

           managedData = GetDocument();
           documentInfo = new NativeMethods.DOC_INFO_1();
           documentInfo.pDataType = "RAW";
           documentInfo.pDocName = "Receipt";
           printerHandle = new IntPtr(0);
           if (NativeMethods.OpenPrinter(printerName.Normalize(), out printerHandle, IntPtr.Zero))
           {
               if (NativeMethods.StartDocPrinter(printerHandle, 1, documentInfo))
               {
                   int bytesWritten;

                   IntPtr unmanagedData;

                   //managedData = document;
                   unmanagedData = Marshal.AllocCoTaskMem(managedData.Length);
                   Marshal.Copy(managedData, 0, unmanagedData, managedData.Length);

                   if (NativeMethods.StartPagePrinter(printerHandle))
                   {
                       NativeMethods.WritePrinter(
                           printerHandle,
                           unmanagedData,
                           managedData.Length,
                           out bytesWritten);
                       NativeMethods.EndPagePrinter(printerHandle);
                   }
                   else
                   {
                       //throw new Win32Exception();
                   }

                   Marshal.FreeCoTaskMem(unmanagedData);

                   NativeMethods.EndDocPrinter(printerHandle);
               }
               else
               {
                 //  throw new Win32Exception();
               }

               NativeMethods.ClosePrinter(printerHandle);
           }
           else
           {
               //throw new Win32Exception();
           }

       }
    }
}
