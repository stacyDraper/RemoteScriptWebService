using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;



public class Service : IService
{

    public string setup(string hostUrl)
    {
        return Setup.BuildSharePointAssets(hostUrl);
    }

    public string ExecuteJobs(string[] ids, string[] jobSiteUrls)
    {
        int i = 0;
        foreach (string id in ids)
        {
            ExecuteJob(id, jobSiteUrls[i]);
            i++;
        }
        return "batch completed";
    }

    public string ExecuteJob(string id, string jobSiteUrl)
    {
        Execute execute = new Execute();
        string retValue = execute.Job(id, jobSiteUrl);
        return retValue;
    }

    public string ExecuteBatch(string id, string hardwareSiteUrl, string jobSiteUrl, string batch)
    {
        Execute execute = new Execute();
        string retValue = execute.Batch(id, hardwareSiteUrl, jobSiteUrl, batch);
        return retValue;
    }

}
