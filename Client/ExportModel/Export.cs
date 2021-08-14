using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Client.RemoteModel;

namespace Client.ExportModel {
    
    /// <summary>
    /// This class represents the output format of a Workspace.
    /// In this class, there are definitions of equivalents of all the remote data types. For
    /// example, the <see cref="Client.RemoteModel.User"/> class is converted into a
    /// <see cref="Client.ExportModel.Export.User"/> type, using the corresponding Convert method.
    /// <br/><br/>
    /// To select what part of the remote data will be exported, change the Convert method of each type.
    /// The export equivalent of the <see cref="Workspace"/> class is the <see cref="Export"/> class itself.
    /// <br/><br/>
    /// Some fields are stored as object because they are too complicated, or are not consistent in their form.
    /// For more information, see the twist api documentation here: https://developer.twist.com/v3/#introduction.
    /// </summary>
    public class Export {
        public string WorkspaceName { get; set; }

        public List<User> Users { get; set; } = new();
        public List<Channel> Channels { get; set; } = new();

        public static Export Convert(Workspace workspace) {
            return new() {WorkspaceName = workspace.name};
        }

        public async Task SaveToFolder(string folderPath, HttpClient client) {
            if (! Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string jsonFilePath = Path.Combine(folderPath, "data.json");
            string attachmentsFolder = Path.Combine(folderPath, "attachments");
            if (! Directory.Exists(attachmentsFolder))
                Directory.CreateDirectory(attachmentsFolder);
            
            List<Task> tasksToAwait = new List<Task>();

            tasksToAwait.Add(File.WriteAllTextAsync(jsonFilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions {WriteIndented = true, IgnoreNullValues = false})));

            DownloadQueue fileDownloadQueue = new DownloadQueue();
            foreach (var channel in Channels) {
                foreach (var thread in channel.Threads) {
                    foreach (var attachment in thread.Attachments) {
                        fileDownloadQueue.AddNewDownload(attachmentsFolder, attachment);
                    }

                    foreach (var comment in thread.Comments) {
                        foreach (var attachment in comment.Attachments) {
                            fileDownloadQueue.AddNewDownload(attachmentsFolder, attachment);
                        }
                    }
                }
            }

            tasksToAwait.Add(fileDownloadQueue.DownloadAll(client));

            await Task.WhenAll(tasksToAwait);
        }

        public class User {
            public int Id { get; set; }
            public string ContactInfo { get; set; }
            public bool Bot { get; set; }
            public bool Removed { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }

            public static User Convert(RemoteModel.User other) {
                return new() {
                    Id = other.id,
                    ContactInfo = other.contact_info,
                    Bot = other.bot,
                    Removed = other.removed,
                    Name = other.name,
                    Email = other.email
                };
            }
        }
    
        public class Channel {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int CreatorId { get; set; }
            public List<int> UserIds { get; set; }
            public bool IsPublic { get; set; }
            public bool Archived { get; set; }
            public int CreatedTimestamp { get; set; }
            public List<Thread> Threads { get; set; }
            
            public static Channel Convert(RemoteModel.Channel other) {
                return new() {
                    Id = other.id,
                    Name = other.name,
                    Description = other.description,
                    CreatorId = other.creator,
                    UserIds = other.user_ids,
                    IsPublic = other.@public,
                    Archived = other.archived,
                    CreatedTimestamp = other.created_ts
                };
            }
        }
    
        public class Thread {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public int CreatorId { get; set; }
            public int? LastEditedTimestamp { get; set; }
            public int PostedTimestamp { get; set; }
            public bool Archived { get; set; }
            public List<Attachment> Attachments { get; set; }
            public List<Comment> Comments { get; set; }
            
            public static Thread Convert(RemoteModel.Thread other) {
                return new() {
                    Id = other.id,
                    Title = other.title,
                    Content = other.content,
                    CreatorId = other.creator,
                    LastEditedTimestamp = other.last_edited_ts,
                    PostedTimestamp = other.posted_ts,
                    Archived = other.is_archived,
                    Attachments = other.attachments.Where(attachment => attachment.file_name is not null).Select(Attachment.Convert).ToList()
                };
            }
        }
        
        public class Comment {
            public int Id { get; set; }
            public string Content { get; set; }
            public int CreatorId { get; set; }
            public bool Deleted { get; set; }
            public int PostedTimestamp { get; set; }
            public int? LastEditedTimestamp { get; set; }
            public List<Attachment> Attachments { get; set; }
            
            public static Comment Convert(RemoteModel.Comment other) {
                return new() {
                    Id = other.id,
                    Content = other.content,
                    CreatorId = other.creator,
                    Deleted = other.deleted,
                    LastEditedTimestamp = other.last_edited_ts,
                    PostedTimestamp = other.posted_ts,
                    Attachments = other.attachments.Where(attachment => attachment.file_name is not null).Select(Attachment.Convert).ToList()
                };
            }
        }
        
        public class Attachment {
            public string Id { get; set; }
            public string Title { get; set; }
            public string FileName { get; set; }
            [JsonIgnore]
            public string Url { get; set; }
            
            public static Attachment Convert(RemoteModel.Attachment other) {
                return new() {
                    Id = other.attachment_id,
                    Title = other.title,
                    FileName = other.file_name,
                    Url = other.url
                };
            }
        }
    }
}