using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WeClearedTeam.Models
{
    [DataContract]
    [Table("Addons")]
    public class Addon
    {
        [Key]
        public int Id { get; set; }
        [DataMember]
        public string Nom { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string VersionPath { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string WebPath { get; set; }

        [NotMapped]
        public string VersionString { get; set; }

        [NotMapped]
        public bool IsOutdated { get; set; }
    }
}