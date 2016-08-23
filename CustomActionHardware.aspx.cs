using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SP = Microsoft.SharePoint.Client;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.ExpiresAbsolute = DateTime.Now;

        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));

        Response.Cache.SetNoStore();

        Response.AppendHeader("Pragma", "no-cache");

        //Execute execute = new Execute();
        //string jobSiteUrl = Request.QueryString["jobSiteUrl"];
        //string retValue = "";

        //string id = Request.QueryString["id"];
        //if (id.Contains("|"))
        //{
        //    string[] ids = id.Split('|');
        //    foreach (string identity in ids)
        //    {
        //        if (!string.IsNullOrEmpty(identity))
        //        {
        //            execute.Job(identity, jobSiteUrl);
        //        }
        //    }
        //}
        //else
        //{
        //    retValue = execute.Job(id, jobSiteUrl);
        //}

        //Response.Write("The dishes are done, dude!");

        string[] choices = getChoices(Request.QueryString["jobSiteUrl"]);

        foreach (string choice in choices)
        {
            myChoices.Items.Add(choice);
        }


    }

    private String[] getChoices(string siteUrl)
    {
        ClientContext clientContext = new ClientContext(siteUrl);
        clientContext.Credentials = Utilities.sp365credential();
        SP.List oList = clientContext.Web.Lists.GetByTitle("Jobs");
        
        FieldChoice batchOptions = clientContext.CastTo<FieldChoice>(oList.Fields.GetByInternalNameOrTitle("Batch")); ;

        clientContext.Load(batchOptions);
        clientContext.ExecuteQuery();

        string[] choices = batchOptions.Choices;
        
        return choices;
    }
}