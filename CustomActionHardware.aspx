<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomActionHardware.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
   <head>
       <META HTTP-EQUIV="CACHE-CONTROL" CONTENT="NO-CACHE">
       <META HTTP-EQUIV="Pragma" CONTENT="no-cache">
       <META HTTP-EQUIV="Expires" CONTENT="-1">

       <script src="jquery-3.1.0.js"></script>
      <title>Running scripts</title>
   </head>

    <body>
        <form id="form1" runat="server">
        <div id="Message">
            <div id="buttons">
                
                <asp:RadioButtonList ID="myChoices" runat="server"></asp:RadioButtonList>
                </div>
            <button id="startButton" onclick="start()">Start</button>
        </div>
        </form>
    </body>
    <script 
 type="text/javascript" 
 src="http://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js">
</script>
<script type="text/javascript" src="/sp.runtime.js"></script>
<script type="text/javascript" src="/sp.js"></script>
    <script type="text/javascript">
        var I = 0;
        var Total = 0;
        var Batch = "";

        function enableStartButton() {
            $("startButton").prop('disabled', false);
        }

        function start() {
            
            var jobSiteUrl = decode(getUrlVars()["jobSiteUrl"]);
            var hardwareSiteUrl = decode(getUrlVars()["jobSiteUrl"]);
            Batch = $("input[name='myChoices']:checked").val();

            

            var id = decode(getUrlVars()["id"]);

            if (id == undefined) {
                $("#Message").text("No id.  There is nothing to run.");
                return;
            }


            if (id.indexOf("|") >= 0) {
                var ids = id.split('|');
                for (var i in ids) {
                    if (ids[i]) {
                        executeBatch(ids[i], hardwareSiteUrl, jobSiteUrl);
                    }
                }
            }
            else {
                executeBatch(id, hardwareSiteUrl, jobSiteUrl);
            }
            $("#Message").html("Working on it...");
        }

        function executeBatch(id, HardwareSiteUrl, JobSiteUrl) {
            Total++;
            $.getJSON("Service.svc/ExecuteBatch", { id: id, hardwareSiteUrl: HardwareSiteUrl, jobSiteUrl: JobSiteUrl, batch: Batch })
                .always(function (data) {
                    I++;
                })
                .done(function (json) {
                    changeMessage();
                    //console.log("JSON Data: " + json.users[3].name);
                })
                .fail(function (jqxhr, textStatus, error) {
                    changeMessage();
                    //var err = textStatus + ", " + error;
                    //console.log("Request Failed: " + err);
                });
        }


        function executeJob(id, siteUrl) {
            Total++;
            $.getJSON("Service.svc/ExecuteJob", { id: id, jobSiteUrl: siteUrl })
                .always(function (data) {
                    I++;
                })
                .done(function (json) {
                    changeMessage();
                    //console.log("JSON Data: " + json.users[3].name);
                })
                .fail(function (jqxhr, textStatus, error) {
                    changeMessage();
                    //var err = textStatus + ", " + error;
                    //console.log("Request Failed: " + err);
                });
        }

        function changeMessage() {
            if (I == Total) {
                $("#Message").html("Completed... <BR /><BR /><BR /><smaller>If you see this message the next time a script is run it is be cause the browser settings are set to cache this page to change go to Tools >  Internet Options.  Click the Browsing History \"Settings\" button.  Check for newer versions of stored pages: and select \"Everytime I visit the page\".</smaller>");
            }
        }
        
        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }

        function decode(stringToDecode) {
            if (stringToDecode == undefined)
                return;
            stringToDecode = decodeURIComponent((stringToDecode).replace(/\+/g, " "));

            return stringToDecode
        }

        function getBatchOptions() {
            alert()
            //SP.SOD.executeFunc('sp.js', 'SP.ClientContext', sharePointReady);

            // Setup context and load current web
            var context = new SP.ClientContext("https://coupledtech.sharepoint.com/sites/Dev");
            
            var web = context.get_web();
            context.load();

            // Get task list
            var taskList = web.get_lists().getByTitle("Jobs");

            // Get Priority field (choice field)
            var priorityField = context.castTo(taskList.get_fields().getByInternalNameOrTitle("Batch"),
                                               SP.FieldChoice);

            // Load the field
            context.load(priorityField);

            // Call server
            context.executeQueryAsync(Function.createDelegate(this, this.onSuccessMethod),
                                      Function.createDelegate(this, this.onFailureMethod));

            function onSuccessMethod(sender, args) {
                // Get string arry of possible choices (but NOT fill-in choices)
                var choices = priorityField.get_choices();
                var buttons = '';
                $(choices).each(function (i) {
                    buttons += '<input type="radio" name="group1" value="' + allButtons[i] + '">' + allButtons[i] + '<br>'
                });

                alert("Choices: (" + choices.length + ") - " + choices.join(", "));
            }

            function onFailureMethod(sender, args) {
                alert("oh oh!");
            }

        }

        //$(document).ready(getBatchOptions());

 

    </script>
</html>


