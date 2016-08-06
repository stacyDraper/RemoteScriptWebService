using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Execute execute = new Execute();
        string jobSiteUrl = Request.QueryString["jobSiteUrl"];
        string retValue = "";

        string id = Request.QueryString["id"];
        if (id.Contains("|"))
        {
            string[] ids = id.Split('|');
            foreach (string identity in ids)
            {
                if (!string.IsNullOrEmpty(identity))
                {
                    execute.Job(identity, jobSiteUrl);
                }
            }
        }
        else
        {
            retValue = execute.Job(id, jobSiteUrl);
        }
        
        Response.Write("The dishes are done, dude!");
    }
}