using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using WeClearedTeam.AddonsServer;
using WeClearedTeam.Models;

namespace WeClearedTeam.Controllers
{
    public class WeClearedClientController : ApiController
    {
        private WeClearedDBContext db = new WeClearedDBContext();

#if DEBUG
        private string UpdatesFolder = @"D:\Google Drive\SitesWebs\WeClearedUpdates\update\";
#else
        private string UpdatesFolder = @"C:\Users\Patrick\Google Drive\SitesWebs\WeCleared\update\";
#endif
        
        /// <summary>
        /// Recevoir le fichier Xml qui représente la version demandée
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Version(int id = 1)
        {
            var version = (id == 1 ? "release" : id == 2 ? "local" : "debug");
            var entry = db.ClientVersions.FirstOrDefault(x => x.Version == version);
            if (entry != null)
            {
                // dossier de l'exécutable
                var xml = entry.XmlContent;
                // version de l'exécutable

                // retourner le xml
                return new HttpResponseMessage()
                {
                    Content = new StringContent(xml, Encoding.UTF8, "application/xml")
                };
            }
            else
            {
                // retourner le xml
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Database Entry is missing", Encoding.UTF8, "application/xml")
                };
            }
        }
        
        /// <summary>
        /// Télécharger le fichier via la base de données
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage DownloadInstaller(int id = 1)
        {
            var version = (id == 1 ? "release" : id == 2 ? "local" : "debug");

            // database set
            var entry = db.ClientVersions.FirstOrDefault(x => x.Version == version + "installer")?.Data;
            if (entry != null)
            {
                var entryStream = new MemoryStream(entry);

                // Construire le result
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(entryStream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "WeClearedInstaller.msi"
                };

                // retourner le fichier construit
                return result;
            }
            else
            {
                var result = new HttpResponseMessage(HttpStatusCode.NotFound);
                return result;
            }
        }

        /// <summary>
        /// Télécharger le fichier via la base de données
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Download(int id = 1)
        {
            var version = (id == 1 ? "release" : id == 2 ? "local" : "debug");

            // database set
            var entry = db.ClientVersions.FirstOrDefault(x => x.Version == version)?.Data;
            if (entry != null)
            {
                var entryStream = new MemoryStream(entry);

                // Construire le result
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(entryStream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "WeCleared.exe"
                };

                // retourner le fichier construit
                return result;
            }
            else
            {
                var result = new HttpResponseMessage(HttpStatusCode.NotFound);
                return result;
            }
        }
    }


    public class XmlRefresh
    {
        private WeClearedDBContext db = new WeClearedDBContext();

#if DEBUG
        private string UpdatesFolder = @"D:\Google Drive\SitesWebs\WeCleared\update\";
#else
        private string UpdatesFolder = @"C:\Users\Patrick\Google Drive\SitesWebs\WeCleared\update\";
#endif

        public void CreateXmlFiles()
        {
            try
            {
                for (var i = 1; i < 4; i++)
                {
                    var version = (i == 1 ? "release" : i == 2 ? "local" : "debug");
                    // dossier de l'exécutable
                    var exeFilePath = UpdatesFolder + version + @"\WeCleared.exe";
                    // version de l'exécutable
                    var exeVersionInfo = FileVersionInfo.GetVersionInfo(exeFilePath);
                    var exeVersion = exeVersionInfo.ProductVersion;

                    // hash de l'exécutable
                    var hashedFileMd5 = Controllers.Hasher.Md5HashFile(exeFilePath);

                    // l'api à contacter pour effectuer le téléchargement
                    var downloadPath = "http://" + (i == 1 ? Dns.GetHostAddresses("codemylife.ca")[0].ToString() : "192.168.0.59") + ":44443/api/weclearedclient/download/" + i;

                    // aller chercher le contenu du changelog
                    var changelog = File.ReadAllText(UpdatesFolder + "Changelog.txt");

                    // contenu de l'xml
                    var xml = "<?xml version='1.0'?>" +
                              "<sharpUpdate>" +
                                  "<update appId='We Cleared Client'>" +
                                      "<version>" + exeVersion + "</version>" +
                                      "<url>" + downloadPath + "</url>" +
                                      "<fileName>WeCleared.exe</fileName>" +
                                      "<md5>" + hashedFileMd5 + "</md5>" +
                                      "<description>" + changelog + "</description>" +
                                      "<lauchArgs></lauchArgs>" +
                                  "</update>" +
                              "</sharpUpdate>";

                    byte[] bytes;
                    using (FileStream exeStream = new FileStream(exeFilePath, FileMode.Open))
                    {
                        using (BinaryReader br = new BinaryReader(exeStream))
                        {
                            bytes = br.ReadBytes((Int32)exeStream.Length);
                        }
                    }

                    var entry = db.ClientVersions.FirstOrDefault(x => x.Version == version);
                    if (entry != null)
                    {
                        entry.XmlContent = xml;
                        entry.Data = bytes;
                    }
                    else
                    {
                        var newEntry = new WeClearedClientVersion
                        {
                            Version = version,
                            XmlContent = xml,
                            Data = bytes
                        };
                        db.ClientVersions.Add(newEntry);
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.ToString());
#endif
            }


            try
            {
                for (var i = 1; i < 4; i++)
                {
                    var version = (i == 1 ? "release" : i == 2 ? "local" : "debug");
                    // dossier de l'exécutable
                    var msiFilePath = UpdatesFolder + version + @"\WeClearedInstaller.msi";

                    byte[] bytes;
                    using (FileStream msiStream = new FileStream(msiFilePath, FileMode.Open))
                    {
                        using (BinaryReader br = new BinaryReader(msiStream))
                        {
                            bytes = br.ReadBytes((Int32)msiStream.Length);
                        }
                    }

                    var entry = db.ClientVersions.FirstOrDefault(x => x.Version == version + "installer");
                    if (entry != null)
                    {
                        entry.XmlContent = "";
                        entry.Data = bytes;
                    }
                    else
                    {
                        var newEntry = new WeClearedClientVersion
                        {
                            Version = version + "installer",
                            XmlContent = "",
                            Data = bytes
                        };
                        db.ClientVersions.Add(newEntry);
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.ToString());
#endif
            }
        }
    }

    /// <summary>
    /// Classe utilisée pour créer le hash sums des fichiers
    /// </summary>
    internal class Hasher
    {
        /// <summary>
        /// Génère le hash sums d'un fichier
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="algo"></param>
        /// <returns>Le hash du fichier</returns>
        internal static string Md5HashFile(string filePath)
        {
            // ouvrir le fichier
            var exeStream = new FileStream(filePath, FileMode.Open);

            //construire le hash
            var exeHash = MakeHashString(MD5.Create().ComputeHash(exeStream));

            // disposer du fichier
            exeStream.Dispose();

            //retourner le hash
            return exeHash;
        }

        /// <summary>
        /// Convertis les byte[] en string
        /// </summary>
        /// <param name="hash"></param>
        /// <returns>Le hash en string</returns>
        private static string MakeHashString(byte[] hash)
        {
            var stringBuilder = new StringBuilder(hash.Length * 2);

            foreach (var l_Byte in hash)
            {
                stringBuilder.Append(l_Byte.ToString("X2").ToLower());
            }

            return stringBuilder.ToString();
        }
    }
}
