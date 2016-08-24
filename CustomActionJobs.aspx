<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomActionJobs.aspx.cs" Inherits="_Default" %>

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
        <div id="Message">
            Preparing...
        </div>
        </form>
    </body>
    
    <script type="text/javascript">
        var I = 0;
        var Total = 0;

        function start() {
            var jobSiteUrl = decode(getUrlVars()["jobSiteUrl"]);
            var id = decode(getUrlVars()["id"]);

            if (id == undefined) {
                $("#Message").text("No id.  There is nothing to run.");
                return;
            }


            if (id.indexOf("|") >= 0) {
                var ids = id.split('|');
                for (var i in ids) {
                    if (ids[i]) {
                        executeJob(ids[i], jobSiteUrl);
                    }
                }
            }
            else {
                executeJob(id, jobSiteUrl);
            }

            var processing = '<div style="padding: 10px;"><div class="ms-dlgLoadingTextDiv ms-alignCenter"><span style="padding-top: 6px; padding-right: 10px;"><img src="/images/gears_anv4.gif?rev=44"></span><span class="ms-core-pageTitle ms-accentText">Processing...</span></div><div class="ms-textXLarge ms-alignCenter">Please wait while request is in progress...</div></div>';
            $("#Message").html(processing);
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

 
        $(document).ready(start());

    </script>
</html>


