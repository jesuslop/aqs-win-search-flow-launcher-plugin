// requires nu-get package tlbimp-microsoft.search.interop
// references COM object ADODB (microsoft activex data object b691e011-1797-432e-907a-4d8c69339129)
// 
// TODO
// configuralize SCOPE path

using System.Collections.Generic;
using Flow.Launcher.Plugin;
using ADODB;
using Flow.Launcher.Plugin.SharedCommands;
using System;

namespace HelloWorldCSharp
{
    public class Main : IPlugin, IPluginI18n
    {
        internal PluginInitContext Context;
        
        internal Microsoft.Search.Interop.CSearchCatalogManager searchCatalogManager;

        public List<Result> Query(Query query)
        {
            var query_helper = searchCatalogManager.GetQueryHelper();

            query_helper.QueryMaxResults = 10;
            query_helper.QuerySelectColumns = "System.ItemUrl";
            query_helper.QueryWhereRestrictions = "AND SCOPE = 'C:\\Users\\jesus\\backed-up\\plain\\Investigacion'";

            if (query.Search.Trim().Length == 0) {
                return new List<Result>();
            }
            var sql_query = query_helper.GenerateSQLFromUserQuery(query.Search.Trim());

            var connection = new ADODB.Connection();
            connection.Open("Provider=Search.CollatorDSO;Extended Properties='Application=Windows';");

            var recordset = new ADODB.Recordset();
            recordset.Open(sql_query, connection);

            var results = new List<Result>();

            while (!recordset.EOF) {
                // just one column
                var file_uri_string = recordset.Fields[0].Value;
                var file_uri = new Uri(file_uri_string);
                var file_path = file_uri.LocalPath;

                var result = new Result
                {
                    Title = System.IO.Path.GetFileName(file_path),
                    SubTitle = System.IO.Path.GetDirectoryName(file_path),
                    Action = c =>
                    {
                        FilesFolders.OpenPath(file_path);
                        return true;
                    },
                    IcoPath = "Images/app.png"
                };
                results.Add(result);                

                recordset.MoveNext();
            }

            return results;
        }

        public void Init(PluginInitContext context)
        {
            Context = context;
            var searchManager = new Microsoft.Search.Interop.CSearchManagerClass(); 
            searchCatalogManager = searchManager.GetCatalog("SystemIndex");
        }

        public string GetTranslatedPluginTitle()
        {
            return Context.API.GetTranslation("plugin_helloworldcsharp_plugin_name");
        }

        public string GetTranslatedPluginDescription()
        {
            return Context.API.GetTranslation("plugin_helloworldcsharp_plugin_description");
        }
    }
}
