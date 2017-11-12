using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeClearedTeam.Models
{
    public class Raider
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        
        public int ClassId { get; set; }
        public int SpecializationId { get; set; }

        public DateTime? Joined { get; set; }
        public DateTime? Left { get; set; }

        /// <summary>
        /// Trouver le nom associé à la classe.
        /// </summary>
        [NotMapped]
        public Classes Classe { get; set; }

        /// <summary>
        /// Trouver le nom de la spécialization.
        /// </summary>
        [NotMapped]
        public Specializations Specialisation { get; set; }

        /// <summary>
        /// Trouver le Role en fonction de la spécialisation.
        /// </summary>
        [NotMapped]
        public Roles Role { get; set; }

        /// <summary>
        /// Trouver la couleur assignée pour la classe.
        /// </summary>
        /// <returns></returns>
        public Color ClassColor()
        {
            switch ((Classes)ClassId)
            {
                case Classes.Death_Knight:
                    return Color.FromArgb(255, 196, 30, 59);
                case Classes.Demon_Hunter:
                    return Color.FromArgb(255, 163, 48, 201);
                case Classes.Druid:
                    return Color.FromArgb(255, 255, 125, 10);
                case Classes.Hunter:
                    return Color.FromArgb(255, 171, 212, 115);
                case Classes.Mage:
                    return Color.FromArgb(255, 105, 204, 240);
                case Classes.Monk:
                    return Color.FromArgb(255, 0, 255, 150);
                case Classes.Paladin:
                    return Color.FromArgb(255, 245, 140, 186);
                case Classes.Priest:
                    return Color.FromArgb(255, 255, 255, 255);
                case Classes.Rogue:
                    return Color.FromArgb(255, 255, 245, 105);
                case Classes.Shaman:
                    return Color.FromArgb(255, 0, 112, 222);
                case Classes.Warlock:
                    return Color.FromArgb(255, 148, 130, 201);
                case Classes.Warrior:
                    return Color.FromArgb(255, 199, 156, 110);
            }
            return Color.Black;
        }

        /// <summary>
        /// Définir les rôles en fonction des classes.
        /// </summary>
        public static Dictionary<Specializations, Roles> SpecializationsRoles = new Dictionary<Specializations, Roles>
        {
            [Specializations.Affliction] = Roles.Damager,
            [Specializations.Arcane] = Roles.Damager,
            [Specializations.Arms] = Roles.Damager,
            [Specializations.Assassination] = Roles.Damager,
            [Specializations.Balance] = Roles.Damager,
            [Specializations.Beast_Mastery] = Roles.Damager,
            [Specializations.Brewmaster] = Roles.Tank,
            [Specializations.Blood] = Roles.Tank,
            [Specializations.Demonology] = Roles.Damager,
            [Specializations.Destruction] = Roles.Damager,
            [Specializations.Discipline] = Roles.Healer,
            [Specializations.Elemental] = Roles.Damager,
            [Specializations.Enhancement] = Roles.Damager,
            [Specializations.Feral] = Roles.Damager,
            [Specializations.Fire] = Roles.Damager,
            [Specializations.Frost] = Roles.Damager,
            [Specializations.Fury] = Roles.Damager,
            [Specializations.Guardian] = Roles.Tank,
            [Specializations.Havoc] = Roles.Damager,
            [Specializations.Holy] = Roles.Healer,
            [Specializations.Marksmanship] = Roles.Damager,
            [Specializations.Mistweaver] = Roles.Healer,
            [Specializations.Outlaw] = Roles.Damager,
            [Specializations.Protection] = Roles.Tank,
            [Specializations.Restoration] = Roles.Healer,
            [Specializations.Retribution] = Roles.Damager,
            [Specializations.Unholy] = Roles.Damager,
            [Specializations.Shadow] = Roles.Damager,
            [Specializations.Subtlety] = Roles.Damager,
            [Specializations.Survival] = Roles.Damager,
            [Specializations.Vengeance] = Roles.Tank,
            [Specializations.Windwalker] = Roles.Damager
        };
    }

    /// <summary>
    /// Énumération des classes.
    /// </summary>
    public enum Classes
    {
        Death_Knight = 1,
        Demon_Hunter = 2,
        Druid = 3,
        Hunter = 4,
        Mage = 5,
        Monk = 6,
        Paladin = 7,
        Priest = 8,
        Rogue = 9,
        Shaman = 10,
        Warlock = 11,
        Warrior = 12
    }

    /// <summary>
    /// Énumération des spécializations.
    /// </summary>
    public enum Specializations
    {
        Affliction = 1,
        Arcane = 2,
        Arms = 3,
        Assassination = 4,
        Balance = 5,
        Beast_Mastery = 6,
        Brewmaster = 7,
        Blood = 8,
        Demonology = 9,
        Destruction = 10,
        Discipline = 11,
        Elemental = 12,
        Enhancement = 13,
        Feral = 14,
        Fire = 15,
        Frost = 16,
        Fury = 17,
        Guardian = 18,
        Havoc = 19,
        Holy = 20,
        Marksmanship = 21,
        Mistweaver = 22,
        Outlaw = 23,
        Protection = 24,
        Restoration = 25,
        Retribution = 26,
        Unholy = 27,
        Shadow = 28,
        Subtlety = 29,
        Survival = 30,
        Vengeance = 31,
        Windwalker = 32
    }

    /// <summary>
    /// Énumération des rôles.
    /// </summary>
    public enum Roles
    {
        Damager = 1,
        Tank = 2,
        Healer = 3
    }

    public class WeClearedDBContext : DbContext
    {
        public WeClearedDBContext() : base("WeClearedDatabase")
        { }
        public DbSet<Raider> Raiders { get; set; }

        public DbSet<Addon> Addons { get; set; }
    }
}