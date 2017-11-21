using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using WeClearedTeam.Models;

namespace WeClearedTeam.AddonsServer
{
    public class AddonsParser
    {
        #region Constructeur

        public AddonsParser()
        {
            ZipClass = new ZipClass(this);
        }

        #endregion

        #region Variables

        /// <summary>
        /// Liens vers la base de données
        /// </summary>
        private AddonsParserContext AddonsDataBase = new AddonsParserContext();

        /// <summary>
        /// Path to the folder where the addons will be stored.
        /// </summary>
        public string AddonsPath = @"C:\Users\Patrick\Google Drive\SitesWebs\Addons\";

        /// <summary>
        /// Class qui contien les fonctions d'extraction.
        /// </summary>
        public ZipClass ZipClass { get; set; }

        #endregion

        #region Fonctions

        /// <summary>
        /// Point d'entré appelé toutes les heures (Pulse)
        /// </summary>
        public void Parse()
        {
            foreach (var l_Addon in AddonsDataBase.Addons)
            {
                if (!l_Addon.WebPath.Contains("local"))
                {
                    // Télécharger l'addon
                    DownloadAddon(l_Addon);
                    Thread.Sleep(5000);

                    // Extraire l'addon
                    ZipClass.ExtractAddon(l_Addon);
                    Thread.Sleep(5000);
                }

                // Mettre à jour la version de l'addon
                l_Addon.Version = GetAddonVersion(l_Addon);
            }
            // Enregistrer les modifications de la base de données
            AddonsDataBase.SaveChanges();
        }

        private void DownloadAddon(Addon p_Addon)
        {
            using (var l_WebClient = new WebClient())
            {
                var l_AddonZipUri = new Uri(p_Addon.WebPath);
                var l_TempAddonFolder = Regex.Replace(p_Addon.Nom, "[^A-Za-z0-9()]", "");
                var l_TempsFolderZipPath = Path.Combine(AddonsPath, "Temp", l_TempAddonFolder) + ".zip";
                l_WebClient.DownloadFile(l_AddonZipUri, l_TempsFolderZipPath);
            }
        }
        
        /// <summary>
        /// Lire le .toc pour trouver le # de version.
        /// </summary>
        /// <param name="p_Addon"></param>
        /// <returns></returns>
        private string GetAddonVersion(Addon p_Addon)
        {
            try
            {
                // Emplacement du fichier .toc
                var tempAddonFolder = Regex.Replace(p_Addon.Nom, "[^A-Za-z0-9()]", "");
                var tocPath = Path.Combine(AddonsPath, tempAddonFolder, p_Addon.VersionPath, p_Addon.VersionPath) + ".toc";

                // Streamreader pour trouver la ligne qui contien la version
                var l_SettingsReader = new StreamReader(tocPath);
                var l_VersionLine = "";
                while (!l_SettingsReader.EndOfStream && l_VersionLine == "")
                {
                    var l_ThisLine = l_SettingsReader.ReadLine();
                    if (l_ThisLine != null && l_ThisLine.Contains("## Version:"))
                    {
                        l_VersionLine = l_ThisLine;
                    }
                }
                l_SettingsReader.Close();

                // Regex: Conserver seulement les chiffres
                var l_AddonVersion = Regex.Replace(l_VersionLine, "[^0-9.]", "");
                
                return l_AddonVersion;

            }
            catch { }

            return "0.0";
        }

        #endregion
    }

    public class AddonsParserContext : DbContext
    {
        public AddonsParserContext() : base("WeClearedDatabase")
        { }
        public DbSet<Addon> Addons { get; set; }
    }
}