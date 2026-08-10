namespace Services.Emails
{
    public class EmailMessage
    {
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsHtml { get; set; } = true;
        public List<string>? AttachmentPaths { get; set; }
    }
}
