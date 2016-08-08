<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
            Preparing...
        </div>
        </form>
    </body>

    <script type="text/javascript">
        var I = 0;
        var Total = 0;

        function start() {
            var jobSiteUrl = getUrlVars()["jobSiteUrl"];
            var id = getUrlVars()["id"];

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
            $("#Message").html("Working on it...<br /> You can close this window and view the status on the jobs list.");
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
                $("#Message").text("Completed...");
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
 
        $(document).ready(start());
    </script>
</html>


