<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomActionHardware.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
   <head>
       <META HTTP-EQUIV="CACHE-CONTROL" CONTENT="NO-CACHE">
       <META HTTP-EQUIV="Pragma" CONTENT="no-cache">
       <META HTTP-EQUIV="Expires" CONTENT="-1">
       <link rel="stylesheet" type="text/css" href="/corev15.css"/>
       <script src="jquery-3.1.0.js"></script>
      <title>Running scripts</title>
   </head>

    <body>
        <form id="form1" runat="server">
        <div id="Message" style="text-align:center">
            <div id="buttons" style="text-align: left;">
                
                <asp:RadioButtonList ID="myChoices" runat="server" ></asp:RadioButtonList>
                </div>
            <button id="startButton" onclick="start()" disabled="disabled">Start</button>
        </div>
        </form>
    </body>

    <script type="text/javascript">
        var I = 0;
        var Total = 0;
        var Batch = "";

        function enableStartButton() {
            $("#startButton").prop('disabled', false);
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
            
            var processing = '<div style="padding: 10px;"><div class="ms-dlgLoadingTextDiv ms-alignCenter"><span style="padding-top: 6px; padding-right: 10px;"><img src="/images/gears_anv4.gif?rev=44"></span><span class="ms-core-pageTitle ms-accentText">Processing...</span></div><div class="ms-textXLarge ms-alignCenter">Please wait while request is in progress...</div></div>';

            $("#Message").html(processing);
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
                var completed = '<div style="padding: 10px;"><div class="ms-dlgLoadingTextDiv ms-alignCenter"><span class="ms-core-pageTitle ms-accentText">Completed...</span></div><div class="ms-textXLarge ms-alignCenter">The lists are updated.</div></div>';
                $("#Message").html(completed);
                var ChachingMessage = "Completed... <BR /><BR /><BR /><smaller>If you see this message the next time a script is run it is be cause the browser settings are set to cache this page to change go to Tools >  Internet Options.  Click the Browsing History \"Settings\" button.  Check for newer versions of stored pages: and select \"Everytime I visit the page\".</smaller>";
                var onlySecureContentIsDisplayedMessage = "Tools>>>Internet options>>>Security tab, Custom level>>>Scroll to: Display mixed content>>>Enable>>>OK"
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

        $("#myChoices").on("click", function () {
            enableStartButton();
        });


    </script>



</html>


