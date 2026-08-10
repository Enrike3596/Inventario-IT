namespace Services.Emails
{
    public interface IEmailTemplate
    {
        Task<string> RenderAsync(string templateName, object model);
        Task<string> RenderAsync<T>(string templateName, T model) where T : class;
    }
}
