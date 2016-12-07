
namespace server
{
    public enum ClientState {
        ReadingProlog,
        ReadingHeaders,
        ReadingContent,
        WritingHeaders,
        WritingContent,
        Closed }
}
