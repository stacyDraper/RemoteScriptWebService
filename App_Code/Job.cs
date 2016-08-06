using System;
using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;

public class Job
{

    public string IP { get; set; }
    public string ID { get; set; }
    public Script Script { get; set; }
    public string JobListSiteUrl { get; set; }
    public string OutputSiteUrl { get; set; }
    public string OutputListName { get; set; }
    public string OutputColumnName { get; set; }
    public string Output { get; set; }


    private DateTime start;

    public DateTime Start
    {
        get { return start; }
        set
        {
            start = value;
            ClientContext clientContext = new ClientContext(this.JobListSiteUrl);
            clientContext.Credentials = Utilities.sp365credential();
            SP.List list = clientContext.Web.Lists.GetByTitle("Jobs");
            ListItem oListItem = list.GetItemById(this.ID);

            oListItem["LastStarted"] = this.Start;
            oListItem["LastFinished"] = null;
            oListItem["LastDuration"] = "";


            oListItem.Update();

            clientContext.ExecuteQuery();
        }
    }

    private DateTime finish;

    public DateTime Finish
    {
        get { return finish; }
        set
        {
            finish = value;
            this.Duration = (System.TimeSpan)(this.Finish.Subtract(this.Start));
        }
    }

    private TimeSpan duration;

    public TimeSpan Duration
    {
        get { return duration; }
        set
        {
            duration = value;
            ClientContext clientContext = new ClientContext(this.JobListSiteUrl);
            clientContext.Credentials = Utilities.sp365credential();
            SP.List list = clientContext.Web.Lists.GetByTitle("Jobs");
            ListItem listItem = list.GetItemById(this.ID);

            listItem["LastFinished"] = this.Finish;
            listItem["LastDuration"] = this.duration.TotalSeconds;
            listItem["LastOutput"] = this.Output;

            listItem.Update();

            clientContext.ExecuteQuery();

        }
    }

    public void updateOutputList()
    {
        ClientContext clientContext = new ClientContext(this.OutputSiteUrl);
        clientContext.Credentials = Utilities.sp365credential();
        SP.List list = clientContext.Web.Lists.GetByTitle(this.OutputListName);
        CamlQuery camlQuery = new CamlQuery();
        camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + this.IP + "</Value></Eq></Where></Query>" +
            "<ViewFields>" +
            "<FieldRef Name='ID' />" +
            "</ViewFields><QueryOptions /></View>";
        ListItemCollection listItems = list.GetItems(camlQuery);

        clientContext.Load(listItems);
        
        clientContext.ExecuteQuery();

        if(listItems.Count >= 1)
        {
            foreach (ListItem listItem in listItems)
            {
                listItem[this.OutputColumnName] = this.Output.ToString();
                listItem.Update();
            }
        }
        else
        {
            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            ListItem listItem = list.AddItem(itemCreateInfo);
            listItem["Title"] = this.IP;
            listItem[this.OutputColumnName] = this.Output.ToString();
            
            listItem.Update();

        }
        //listItem[this.OutputColumnName] = this.Output;
        //listItem.Update();

        clientContext.ExecuteQuery();
        
    }

}