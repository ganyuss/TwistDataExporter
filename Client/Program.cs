using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.ExportModel;
using Client.RemoteModel;

namespace Client {

    class Program
    {
        static HttpClient client = new();

        static async Task<HttpResponseMessage> Get(string path) {

            HttpResponseMessage response = await client.GetAsync(path);
            if (! response.IsSuccessStatusCode) {
                Console.WriteLine($"Error {response.StatusCode}: {response.ReasonPhrase}, {response.Headers}");
            }
           
            return response;
        }
        
        static async Task<T> GetAsyncJson<T>(string path)
        {
            T output = default;
            HttpResponseMessage response = await Get(path);
            if (response.IsSuccessStatusCode)
            {
                output = await response.Content.ReadFromJsonAsync<T>();
            }
            return output;
        }

        static void Main(string[] args)
        {
            if (args.Length < 1) {
                Console.WriteLine("Error: no OAuth token supplied as first parameter of this program. Exiting.");
                return;
            }
            RunAsync(args[0]).GetAwaiter().GetResult();
        }

        static async Task RunAsync(string oauthToken)
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://api.twist.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",$"{oauthToken}");
            
            try {
                Console.Write("Downloading workspaces...");
                List<Workspace> workspaces = await GetAsyncJson<List<Workspace>>("api/v3/workspaces/get");
                Console.WriteLine(" Done.");
                
                foreach (var workspace in workspaces) {
                    Console.Write($"Downloading data from workspace \"{workspace.name}\"...");
                    Export workspaceExport = Export.Convert(workspace);
                    
                    List<User> users = await GetAsyncJson<List<User>>($"/api/v3/workspaces/get_users?id={workspace.id}");
                    List<Channel> channels = await GetAsyncJson<List<Channel>>($"api/v3/channels/get?workspace_id={workspace.id}");

                    workspaceExport.Users = users.Select(Export.User.Convert).ToList();
                    
                    foreach (var channel in channels) {
                        Export.Channel exportChannel = Export.Channel.Convert(channel);
                        
                        List<Thread> threads = await GetAsyncJson<List<Thread>>($"/api/v3/threads/get?channel_id={channel.id}");
                        exportChannel.Threads = new List<Export.Thread>();
                        
                        foreach (var thread in threads) {
                            List<Comment> comments = await GetAsyncJson<List<Comment>>($"/api/v3/comments/get?thread_id={thread.id}");

                            Export.Thread exportThread = Export.Thread.Convert(thread);
                            exportThread.Comments = comments.Select(Export.Comment.Convert).ToList();

                            exportChannel.Threads.Add(exportThread);
                        }

                        workspaceExport.Channels.Add(exportChannel);
                    }
                    Console.WriteLine(" Done.");
                    
                    Console.Write("Writing data to disk and downloading attachments...");
                    await workspaceExport.SaveToFolder(Path.Combine("export", workspaceExport.WorkspaceName), client);
                    Console.WriteLine(" Done.");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().Name}: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}