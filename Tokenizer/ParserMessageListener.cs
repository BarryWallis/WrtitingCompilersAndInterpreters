using System.Reflection;
using System.Text;

using Frontend.PascalLib;

using MessageLib;

namespace Tokenizer;

public partial class Pascal
{
    private const int _prefixWidth = 5;

    /// <summary>
    /// Listens for parser messages and writes parser summaries to the console.
    /// </summary>
    private sealed class ParserMessageListener : IMessageListener
    {
        /// <summary>
        /// Handles parser messages.
        /// </summary>
        /// <param name="message">The received message.</param>
        public void MessageReceived(Message message)
        {
            switch (message)
            {
                case TokenMessage tokenMessage:
                    WriteTokenMessage(tokenMessage);
                    break;

                default:
                    _ = TryWriteSyntaxErrorMessage(message);
                    break;
            }
        }

        /// <summary>
        /// Writes token details to the console.
        /// </summary>
        /// <param name="tokenMessage">The token message.</param>
        private static void WriteTokenMessage(TokenMessage tokenMessage)
        {
            Console.WriteLine($">>> {tokenMessage.TokenType.Name,-15} line={tokenMessage.LineNumber:000}, pos={tokenMessage.Position,2}, text=\"{tokenMessage.Text}\"");

            if (tokenMessage.Value is not null)
            {
                object tokenValue = tokenMessage.Value;
                if (tokenValue is string)
                {
                    tokenValue = $"\"{tokenValue}\"";
                }

                Console.WriteLine($">>>                 value={tokenValue}");
            }
        }

        /// <summary>
        /// Writes syntax error details when a syntax-error-shaped message is received.
        /// </summary>
        /// <param name="message">The received message.</param>
        /// <returns><see langword="true"/> when a syntax error message was written; otherwise <see langword="false"/>.</returns>
        private static bool TryWriteSyntaxErrorMessage(Message message)
        {
            Type messageType = message.GetType();
            if (!string.Equals(messageType.Name, "SyntaxErrorMessage", StringComparison.Ordinal))
            {
                return false;
            }

            PropertyInfo? lineNumberProperty = messageType.GetProperty("LineNumber");
            PropertyInfo? positionProperty = messageType.GetProperty("Position");
            PropertyInfo? tokenTextProperty = messageType.GetProperty("TokenText");
            PropertyInfo? errorMessageProperty = messageType.GetProperty("ErrorMessage");

            if (lineNumberProperty?.GetValue(message) is not int ||
                positionProperty?.GetValue(message) is not int position ||
                errorMessageProperty?.GetValue(message) is not string errorMessage)
            {
                return false;
            }

            string? tokenText = tokenTextProperty?.GetValue(message) as string;

            int spaceCount = _prefixWidth + position;
            StringBuilder flagBuffer = new();

            for (int i = 1; i < spaceCount; i++)
            {
                _ = flagBuffer.Append(' ');
            }

            _ = flagBuffer.Append("^\n*** ").Append(errorMessage);

            if (tokenText is not null)
            {
                _ = flagBuffer.Append(" [at \"").Append(tokenText).Append("\"]");
            }

            Console.WriteLine(flagBuffer.ToString());
            return true;
        }
    }
}
