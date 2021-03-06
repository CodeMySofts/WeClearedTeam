﻿using System;
using System.Configuration;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Owin;
using Owin;
using WeClearedTeam.AddonsServer;
using WeClearedTeam.Controllers;
using WeClearedTeam.Models;

[assembly: OwinStartupAttribute(typeof(WeClearedTeam.Startup))]
namespace WeClearedTeam
{
    public partial class Startup
    {
        /// <summary>
        /// Contient les classes qui permettent les tâches en arrière-plan
        /// </summary>
        public AddonsParser Addons { get; set; }

        public XmlRefresh XmlRefreshClass = new XmlRefresh();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);


            // Assignation de hangfire
            GlobalConfiguration.Configuration.UseSqlServerStorage("Hangfire");
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            // tâches en arrière-plan
            // Ajout de la classe de parsing en arrière-plan
            Addons = new AddonsParser();
            BackgroundJob.Enqueue(() => Addons.Parse());
            RecurringJob.AddOrUpdate("UpdateAddons", () => Addons.Parse(), Cron.HourInterval(6));
            // Ajout de la tache de Xml building
            RecurringJob.AddOrUpdate("CreateXml", () => XmlRefreshClass.CreateXmlFiles(), Cron.MinuteInterval(1));
        }
    }
}
