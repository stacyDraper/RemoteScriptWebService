using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for setup
/// </summary>
public class Setup
{
    public Setup()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string BuildSharePointAssets(string hostUrl)
    {
        // Starting with ClientContext, the constructor requires a URL to the 
        // server running SharePoint. 
        ClientContext context = new ClientContext(hostUrl);
        context.Credentials = Utilities.sp365credential();

        // The SharePoint web at the URL.
        Web web = context.Web;
        
        ListCreationInformation creationInfo = new ListCreationInformation();
        creationInfo.Title = "My List";
        creationInfo.TemplateType = 100;
        List list = web.Lists.Add(creationInfo);
        list.Description = "New Description";
        SP.Field oField = list.Fields.AddFieldAsXml("<Field DisplayName='MyField' Type='Number' />", true, AddFieldOptions.DefaultValue);

        SP.FieldNumber fieldNumber = context.CastTo<FieldNumber>(oField);
        fieldNumber.MaximumValue = 100;
        fieldNumber.MinimumValue = 35;

        fieldNumber.Update();

        context.ExecuteQuery();

        return "success!";
    }

}