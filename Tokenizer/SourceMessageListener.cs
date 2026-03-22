using System.Globalization;

using MessageLib;

namespace Tokenizer;

public partial class Pascal
{
    /// <summary>
    /// Listens for source line messages and writes them to the console.
    /// </summary>
    private sealed class SourceMessageListener : IMessageListener
    {
        /// <summary>
        /// Handles source line messages.
        /// </summary>
        /// <param name="message">The received message.</param>
        public void MessageReceived(Message message)
        {
            if (message is SourceLineMessage sourceLineMessage)
            {
                Console.WriteLine(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        _sourceLineFormat,
                        sourceLineMessage.LineNumber,
                        sourceLineMessage.Line));
            }
        }
    }
}
