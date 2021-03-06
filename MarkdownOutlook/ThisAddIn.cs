﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Interop.Outlook;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Markdig;
using Markdig.SyntaxHighlighting;

namespace MarkdownOutlook
{
    public partial class ThisAddIn
    {
        private static MarkdownPipeline pipeline;

        public static bool CachedMarkdownEnabled { get; set; }

        public static string RenderMarkdown(string text)
        {
           return Markdown.ToHtml(text, pipeline);
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseSyntaxHighlighting()
                .Build();
            this.Application.ItemSend += Application_ItemSend;
        }

        void Application_ItemSend(object Item, ref bool Cancel)
        {
            var mailItem = Item as MailItem;
            var useMarkdown = Utility.GetUserProperty<bool>(mailItem, Constants.EnableMarkdownModeFlag);
            if (useMarkdown)
            {
                mailItem.HTMLBody = RenderMarkdown(mailItem.Body);
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
