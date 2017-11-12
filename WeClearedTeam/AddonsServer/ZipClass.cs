using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WeClearedTeam.Models;

namespace WeClearedTeam.AddonsServer
{
    public class ZipClass
    {
        #region Constructeur

        private AddonsParser Base { get; set; }
        /// <summary>
        /// Classe qui gère le dézippage des fichiers
        /// </summary>
        /// <param name="p_Base"></param>
        internal ZipClass(AddonsParser p_Base)
        {
            Base = p_Base;
        }

        #endregion

        #region Fonctions

        /// <summary>
        /// Extraire un addon à partir du dossier Temp
        /// </summary>
        /// <param name="p_Addon"></param>
        internal void ExtractAddon(Addon p_Addon)
        {
            try
            {
                var l_TempDirectory = Path.Combine(Base.AddonsPath, @"Temp");
                var l_AddonFolderName = Regex.Replace(p_Addon.Nom, "[^A-Za-z0-9()]", "");
                var l_AddonFolder = Path.Combine(Base.AddonsPath, l_AddonFolderName);

                if (Directory.Exists(l_AddonFolder))
                    Directory.Delete(l_AddonFolder, true);

                ZipFile.ExtractToDirectory(Path.Combine(l_TempDirectory, l_AddonFolderName) + ".zip", l_AddonFolder);

                // Exception to pull ElvUI out of the master folder
                if (p_Addon.Nom == "ElvUI")
                {
                    var l_ElvUiDirectory = Directory.GetDirectories(l_AddonFolder)[0];
                    //Now Create all of the directories
                    foreach (string l_DirectoryPath in Directory.GetDirectories(l_ElvUiDirectory, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(l_DirectoryPath.Replace(l_ElvUiDirectory, l_AddonFolder));
                    }

                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(l_ElvUiDirectory, "*.*", SearchOption.AllDirectories))
                    {
                        File.Copy(newPath, newPath.Replace(l_ElvUiDirectory, l_AddonFolder), true);
                    }
                }
            }
            catch { }
        }
        
        #endregion
    }
}