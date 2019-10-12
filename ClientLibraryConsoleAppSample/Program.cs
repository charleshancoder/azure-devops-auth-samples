using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.WebApi;

namespace ClientLibraryConsoleAppSample
{
    class Program
    {
        //============= Config [Edit these with your settings] =====================
        internal const string azureDevOpsOrganizationUrl = "https://dev.azure.com/rymanhealthcare"; //change to the URL of your Azure DevOps account; NOTE: This must use HTTPS
        // internal const string vstsCollectioUrl = "http://myserver:8080/tfs/DefaultCollection" alternate URL for a TFS collection
        //==========================================================================

        //Console application to execute a user defined work item query
        static void Main(string[] args)
        {
            //Prompt user for credential
            VssConnection connection = new VssConnection(new Uri(azureDevOpsOrganizationUrl), new VssClientCredentials());

            

            //create http client and query for resutls
            var buildClient = connection.GetClient<BuildHttpClient>();
            //List<BuildDefinitionReference> buildDefinitions = new List<BuildDefinitionReference>();

            // Iterate (as needed) to get the full set of build definitions
            //string continuationToken = null;
            //do
            //{
            //    IPagedList<BuildDefinitionReference> buildDefinitionsPage = buildClient.GetDefinitionsAsync2(
            //        project: "MyRyman Development",
            //        continuationToken: continuationToken).Result;

            //    buildDefinitions.AddRange(buildDefinitionsPage);

            //    continuationToken = buildDefinitionsPage.ContinuationToken;
            //} while (!string.IsNullOrEmpty(continuationToken));

            //// Show the build definitions
            //foreach (BuildDefinitionReference definition in buildDefinitions)
            //{
            //    Console.WriteLine("{0} {1}", definition.Id.ToString().PadLeft(6), definition.Name);
            //}

            var buildsResult = buildClient.GetBuildsAsync2("MyRyman Development",
                definitions: new List<int> { 95 },
                reasonFilter: BuildReason.PullRequest).Result;

            foreach(var build in buildsResult.Where(b => b.SourceBranch.Contains("495")))
            {
                Console.WriteLine("{0} {1} {2}", build.SourceBranch, build.BuildNumber, build.Result.Value);
            }

            Console.WriteLine("Total Failed {0}", buildsResult.Count(b => b.Result.Value == BuildResult.Failed));
            
            Console.ReadKey();
        }
    }
}
