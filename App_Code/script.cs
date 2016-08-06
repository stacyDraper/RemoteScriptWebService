using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;



/// <summary>
/// Summary description for script
/// </summary>
public class Script
{

    public Script()
    {
        // popluate values
    }
    public string ID { get; set; }

    private string title;

    public string Title
    {
        get { return title; }
        set { title = value; }
    }


    private string scriptText;

    public string ScriptText
    {
        get { return scriptText; }
        set { scriptText = value; }
    }

    private ScriptTypeNames scriptTypeName;

    public ScriptTypeNames ScriptTypeName
    {
        get { return scriptTypeName; }
        set { scriptTypeName = value; }
    }

    private string scriptType;

    public string ScriptType
    {
        get { return scriptType; }
        set
        {
            scriptType = value;
            switch (scriptType)
            {
                case "PowerShell":
                    this.ScriptTypeName = ScriptTypeNames.PowerShell;
                    break;
                case "WMI":
                    this.ScriptTypeName = ScriptTypeNames.WMI;
                    break;
                case "SNMP":
                    this.ScriptTypeName = ScriptTypeNames.SNMP;
                    break;
                case "Registry":
                    this.ScriptTypeName = ScriptTypeNames.Registry;
                    break;

                default:
                    break;
            }
        }
    }
    

    public enum ScriptTypeNames
    {
        PowerShell,
        WMI,
        SNMP,
        Registry
    }

}


