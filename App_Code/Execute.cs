using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;
using System.Text;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;


/// <summary>
/// Summary description for ExecuteJob
/// </summary>
public class Execute
{

    public Execute()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string Job(string id, string jobSiteUrl)
    {

        //string server
        Job job = getJob(id, jobSiteUrl);

        job.Start = DateTime.Now;

        string result = RunScript(job);

        job.updateOutputList();

        job.Finish = DateTime.Now;

        return result;

    }

    private Job getJob(string id, string siteUrl)
    {
        Job job = new Job();
        job.JobListSiteUrl = siteUrl;
        job.ID = id;

        ClientContext clientContext = new ClientContext(siteUrl);
        clientContext.Credentials = Utilities.sp365credential();
        
        SP.List oList = clientContext.Web.Lists.GetByTitle("Jobs");

        CamlQuery camlQuery = new CamlQuery();
        camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + id + "</Value></Eq></Where></Query>" +
            "<ViewFields><FieldRef Name='Hardware' />" +
            "<FieldRef Name='Script' />" +
            "<FieldRef Name='OutputSiteUrl' />" +
            "<FieldRef Name='OutputListName' />" +
            "<FieldRef Name='OutputColumn' />" +
            "</ViewFields><QueryOptions /></View>";
        ListItemCollection collListItem = oList.GetItems(camlQuery);

        clientContext.Load(collListItem);

        clientContext.ExecuteQuery();

        
        job.IP = collListItem[0]["Hardware"].ToString();
        job.OutputSiteUrl = collListItem[0]["OutputSiteUrl"].ToString();
        job.OutputListName = collListItem[0]["OutputListName"].ToString();
        job.OutputColumnName = collListItem[0]["OutputColumn"].ToString();
        job.Script = getScript(collListItem[0]["Script"].ToString(), siteUrl);


        return job;
    }

    private Script getScript(string id, string siteUrl)
    {
        Script script = new Script();
        script.ID = id;
        
        ClientContext clientContext = new ClientContext(siteUrl);
        clientContext.Credentials = Utilities.sp365credential();
        SP.List oList = clientContext.Web.Lists.GetByTitle("Scripts");

        CamlQuery camlQuery = new CamlQuery();
        camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + id + "</Value></Eq></Where></Query>" +
            "<ViewFields><FieldRef Name='ScriptText' />" +
            "<FieldRef Name='Title' />" +
            "<FieldRef Name='ScriptType' />" +
            "</ViewFields><QueryOptions /></View>";
        ListItemCollection collListItem = oList.GetItems(camlQuery);

        clientContext.Load(collListItem);

        clientContext.ExecuteQuery();


        script.Title = collListItem[0]["Title"].ToString();
        script.ScriptText = collListItem[0]["ScriptText"].ToString();
        script.ScriptType = collListItem[0]["ScriptType"].ToString();

        return script;
    }

    private string getListItemColumnValue(string id, string siteUrl, string listTitle, string col)
    {
        string value = "";


        ClientContext clientContext = new ClientContext(siteUrl);
        SP.List oList = clientContext.Web.Lists.GetByTitle(listTitle);

        CamlQuery camlQuery = new CamlQuery();
        camlQuery.ViewXml = "<View><Query><Where><FieldRef Name='ID'/>" +
            "<Value Type='Number'>" + id + "</Value></Where></Query>";
        ListItemCollection collListItem = oList.GetItems(camlQuery);

        clientContext.Load(collListItem);

        clientContext.ExecuteQuery();

        value = collListItem[0][col].ToString();

        return value;
    }




    private string RunScript(Job job)
    {
        switch (job.Script.ScriptTypeName)
        {
            case Script.ScriptTypeNames.PowerShell:
                return runPowerShellScript(job);
                break;
            case Script.ScriptTypeNames.WMI:
                //return runWMIScript(scriptText);
                break;
            case Script.ScriptTypeNames.SNMP:
                //return runSNMPScript(scriptText);
                break;
            case Script.ScriptTypeNames.Registry:
                //return runWMIScript(scriptText);
                break;
            default:
                return "Unknown script type";
                break;
        }

        return "Unknown script type";
    }


    private string runPowerShellScript(Job job)
    {
        Collection<PSObject> results;

        WSManConnectionInfo connectionInfo = null;

        connectionInfo = new WSManConnectionInfo(new Uri("http://" + job.IP  + ":5985"));

        connectionInfo.OperationTimeout = 4 * 60 * 1000;
        connectionInfo.OpenTimeout = 1 * 60 * 1000;
        connectionInfo.AuthenticationMechanism = AuthenticationMechanism.NegotiateWithImplicitCredential;

        using (Runspace remoteRunspace = RunspaceFactory.CreateRunspace(connectionInfo))
        {

            remoteRunspace.Open();

            Pipeline pipeline = remoteRunspace.CreatePipeline();
            pipeline.Commands.AddScript(job.Script.ScriptText);
            pipeline.Commands.Add("Out-String");

            results = pipeline.Invoke();

            remoteRunspace.Close();
        }

        // convert the script result into a single string
        StringBuilder stringBuilder = new StringBuilder();
        foreach (PSObject obj in results)
        {
            stringBuilder.AppendLine(obj.ToString());
        }
        
        job.Output = stringBuilder.ToString();

        return job.Output;

    }

}