using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Client.ExportModel;

namespace Client {
    public class DownloadQueue {
        private struct DownloadInstruction {
            public Uri Uri;
            public string TargetFile;
        }

        private Queue<DownloadInstruction> Instructions = new();

        public void AddNewDownload(string folderPath, Export.Attachment attachment) {
            string outputFile = Path.Combine(folderPath, attachment.Id + Path.GetExtension(attachment.FileName));

            Instructions.Enqueue(new DownloadInstruction { TargetFile = outputFile, Uri = new Uri(attachment.Url) });
        }
        

        public async Task DownloadAll(HttpClient client) {
            List<Task<Task>> taskList = new List<Task<Task>>();
            
            while (Instructions.TryDequeue(out var downloadInstruction)) {

                taskList.Add(
                    // First we donwload
                    client.GetAsync(downloadInstruction.Uri)
                        .ContinueWith(downloadResult => {
                            if (downloadResult.Result.IsSuccessStatusCode) {
                                HttpContent content = downloadResult.Result.Content;
                                // Then we write the result
                                FileStream fs = File.OpenWrite(downloadInstruction.TargetFile);
                                return content.CopyToAsync(fs);
                            }
                            
                            Console.Error.WriteLine($"File not found \"{downloadInstruction.TargetFile}\"");
                            return null;
                        }));
            }
            
            await Task.WhenAll(taskList);
            await Task.WhenAll(taskList.Select(task => task.Result));
        }
    }
}