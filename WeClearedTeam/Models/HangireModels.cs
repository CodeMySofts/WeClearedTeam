using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web.Mvc;

namespace WeClearedTeam.Models
{
    public class HangfireContext : DbContext
    {
        public DbSet<CodeSnippet> CodeSnippets { get; set; }

        public HangfireContext() : base("Hangfire")
        {
        }
    }

    public class CodeSnippet
    {
        public int Id { get; set; }

        [Required, AllowHtml, Display(Name = "C# source")]
        public string SourceCode { get; set; }
        public string HighlightedCode { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? HighlightedAt { get; set; }
    }
}