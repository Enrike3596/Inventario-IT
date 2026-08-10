using System.Text.Json;

namespace Services.Emails
{
    public class EmailTemplate : IEmailTemplate
    {
        private readonly string _templatesPath;

        public EmailTemplate()
        {
            _templatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");
        }

        public Task<string> RenderAsync(string templateName, object model)
        {
            var filePath = Path.Combine(_templatesPath, $"{templateName}.html");
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Template {templateName} not found at {filePath}");

            var content = File.ReadAllText(filePath);
            var result = ReplacePlaceholders(content, model);
            return Task.FromResult(result);
        }

        public Task<string> RenderAsync<T>(string templateName, T model) where T : class
        {
            return RenderAsync(templateName, (object)model);
        }

        private static string ReplacePlaceholders(string template, object model)
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
                JsonSerializer.Serialize(model));

            if (dict == null) return template;

            foreach (var kvp in dict)
            {
                var placeholder = $"{{{{{kvp.Key}}}}}";
                var value = kvp.Value.ValueKind switch
                {
                    JsonValueKind.String => kvp.Value.GetString(),
                    JsonValueKind.Number => kvp.Value.GetRawText(),
                    JsonValueKind.True => "true",
                    JsonValueKind.False => "false",
                    _ => kvp.Value.GetRawText()
                };
                template = template.Replace(placeholder, value ?? "");
            }

            return template;
        }
    }
}
