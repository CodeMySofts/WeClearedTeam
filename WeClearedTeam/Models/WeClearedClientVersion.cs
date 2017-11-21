using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeClearedTeam.Models
{
    public class WeClearedClientVersion
    {
        [Key]
        public string Version { get; set; }

        public string XmlContent { get; set; }

        public byte[] Data { get; set; }
    }
}