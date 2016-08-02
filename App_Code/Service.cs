using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;

public class Service : IService
{

    public string ExecuteJob(string id, string jobSiteUrl)
    {
        //string server
        ScriptJob job = getJob(id, jobSiteUrl);

        string result = RunScript(job.Script, job.IP);

        return result;

    }

    private ScriptJob getJob(string id, string siteUrl)
    {
        ClientContext clientContext = new ClientContext(siteUrl);
        SP.List oList = clientContext.Web.Lists.GetByTitle("Jobs");

        CamlQuery camlQuery = new CamlQuery();
        camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + id + "</Value></Eq></Where></Query>" +
            "<ViewFields><FieldRef Name='Hardware' />" +
            "<FieldRef Name='Script' />" +
            "</ViewFields><QueryOptions /></View>";
        ListItemCollection collListItem = oList.GetItems(camlQuery);

        clientContext.Load(collListItem);

        clientContext.ExecuteQuery();

        ScriptJob job = new ScriptJob();
        job.IP = collListItem[0]["Hardware"].ToString();
        job.Script = getScript(collListItem[0]["Script"].ToString(), siteUrl);
        

        return job;
    }

    private Script getScript(string id, string siteUrl)
    {
        ClientContext clientContext = new ClientContext(siteUrl);
        SP.List oList = clientContext.Web.Lists.GetByTitle("Scripts");

        CamlQuery camlQuery = new CamlQuery();
        camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>"+id+ "</Value></Eq></Where></Query>" + 
            "<ViewFields><FieldRef Name='ScriptText' />" +
            "<FieldRef Name='ScriptType' />" +
            "<FieldRef Name='Server' /></ViewFields><QueryOptions /></View>";
        ListItemCollection collListItem = oList.GetItems(camlQuery);

        clientContext.Load(collListItem);

        clientContext.ExecuteQuery();

        Script script = new Script();
        script.ScriptText = collListItem[0]["ScriptText"].ToString();
        script.ScriptType = collListItem[0]["ScriptType"].ToString();
        string server = collListItem[0]["Server"].ToString();


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

    private string RunScript(Script script, string server)
    {
        switch (script.ScriptTypeName)
        {
            case Script.ScriptTypeNames.PowerShell:
                return runPowerShellScript(script.ScriptText, server);
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


    private string runPowerShellScript(string scriptText, string server)
    {
        Collection<PSObject> results;

        WSManConnectionInfo connectionInfo = null;

        connectionInfo = new WSManConnectionInfo(new Uri("http://" + server + ":5985"));

        connectionInfo.OperationTimeout = 4 * 60 * 1000;
        connectionInfo.OpenTimeout = 1 * 60 * 1000;
        connectionInfo.AuthenticationMechanism = AuthenticationMechanism.NegotiateWithImplicitCredential;

        using (Runspace remoteRunspace = RunspaceFactory.CreateRunspace(connectionInfo))
        {
           
            remoteRunspace.Open();

            Pipeline pipeline = remoteRunspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);
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

        return stringBuilder.ToString();

        

    }



    
}
