using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Global
{
    public class clsUtil
    {
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        public static string GenerateGuid()
        {
            Guid newGuid = Guid.NewGuid();
            return newGuid.ToString();
        }
        //public static string GetFileExt(string FilePath)
        //{
        //    return Path.GetExtension(FilePath).ToString();
        //}
        public static bool CreateFoulderIfDoesNotExist(string FoulderPath)
        {
            if (!Directory.Exists(FoulderPath))
            {
                try
                {
                    Directory.CreateDirectory(FoulderPath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return true;
        }
        //public static bool SaveImageToImagesFoulder(ref string ImagePath)
        //{
        //    string FoulderPath = @"F:\Images\";
        //    string GuidName=GenerateGuid();
        //    string Ext= GetFileExt(ImagePath);

        //    string NewPath = FoulderPath + GuidName + Ext;
        //    Image image = Image.FromFile(ImagePath);//Copy to image
        //    image.Save(NewPath);
        //    ImagePath =NewPath;
        //    return true;
        //}
        public static string ReplaceFileNameWithGuid(string FileName)
        {
            FileInfo fi = new FileInfo(FileName);
            string ext=fi.Extension;
            FileName = GenerateGuid() + ext;
            return FileName;

        }
        public static bool CopyImageToImagesFileAndGetItsSourceFile(ref string SourceFile)
        {
            string DestinationFile = @"F:\Images\";
            if (!CreateFoulderIfDoesNotExist(DestinationFile))
            {
                return false;
            }
            string ImagePath = DestinationFile + ReplaceFileNameWithGuid(SourceFile);

            try
            {
                File.Copy(SourceFile, ImagePath);
            }
            catch
            {
                return false;
            }
            SourceFile = ImagePath;
            return true;

        }







    }
}
