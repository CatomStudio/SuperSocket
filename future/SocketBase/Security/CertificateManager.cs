﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SuperSocket.Common;
using SuperSocket.SocketBase.Config;

namespace SuperSocket.SocketBase.Security
{
    static class CertificateManager
    {
        internal static X509Certificate Initialize(ICertificateConfig cerConfig)
        {
            //To keep compatible with website hosting
            string filePath;

            if (Path.IsPathRooted(cerConfig.FilePath))
                filePath = cerConfig.FilePath;
            else
            {
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cerConfig.FilePath);
            }

            return new X509Certificate2(filePath, cerConfig.Password);
        }

        internal static void CreateCertificate(string commonName, ICertificateConfig cerConfig)
        {
            byte[] certificateData = Certificate.CreateSelfSignCertificatePfx(commonName, //host name
                DateTime.Now, //not valid before
                DateTime.Now.AddYears(5), //not valid after
                cerConfig.Password);

            using (BinaryWriter binWriter = new BinaryWriter(File.Open(cerConfig.FilePath, FileMode.Create)))
            {
                binWriter.Write(certificateData);
                binWriter.Flush();
            }
        }
    }
}
