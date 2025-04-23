using System.Text.RegularExpressions;
using System.Text.Json;
using IntercomCsvConverter.Models;
using IntercomCsvConverter.Utils.Extensions;

namespace IntercomCsvConverter;

// TODO: Improve performance + memory consumption
public static class Program
{
    private static void Main()
    {
        Console.WriteLine("Welcome to Intercom CSV Converter!\n" +
                          "Please enter/drag your folder path containing your json files:");
        var conversationFolderPath = Console.ReadLine()?.Trim();
        Console.WriteLine("Please enter/drag your folder path containing your json files with AI responses.\n" +
                          "Leave blank if none.");
        var conversationAiResponsesFolderPath = Console.ReadLine()?.Trim();;
        var anyAiResponsesFolderPath = !string.IsNullOrWhiteSpace(conversationAiResponsesFolderPath);
        if (!Directory.Exists(conversationFolderPath))
        {
            Console.WriteLine($"❌ Folder not found: {conversationFolderPath}");
            return;
        }
        if (anyAiResponsesFolderPath && !Directory.Exists(conversationAiResponsesFolderPath))
        {
            Console.WriteLine($"❌ Folder not found: {conversationAiResponsesFolderPath}");
            return;
        }
        
        var allAiResponseJsonFiles = anyAiResponsesFolderPath
            ? Directory.GetFiles(conversationAiResponsesFolderPath, "*.json")
            : [];
        var allJsonFiles = Directory
            .GetFiles(conversationFolderPath, "*.json")
            .Concat(allAiResponseJsonFiles)
            .ToArray();

        var conversations = new List<ConversationDetails>();
        foreach (var file in allJsonFiles)
        {
            try
            {
                var jsonContent = File.ReadAllText(file);
                var conversation = JsonSerializer.Deserialize<ConversationDetails>(jsonContent);
                if (conversation != null && conversation.Question.DeliveredAs == "customer_initiated")
                {
                    conversations.Add(conversation);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading {file}: {ex.Message}");
            }
        }

        var outputCsvPath = Path.Combine(conversationFolderPath, "all_conversations.csv");
        using (var writer = new StreamWriter(outputCsvPath))
        {
            writer.WriteLine("id,question");
            foreach (var conversation in conversations)
            {
                var plainText = conversation.Question?.Body?.StripHtml();
                if (!string.IsNullOrWhiteSpace(plainText))
                {
                    writer.WriteLine($"\"{conversation.Id}\",\"{plainText}\"");
                }
            }
        }

        Console.WriteLine($"✅ CSV file created: {outputCsvPath}");
    }
}