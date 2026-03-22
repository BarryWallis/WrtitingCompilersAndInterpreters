using System.Globalization;

using MessageLib;

namespace Tokenizer;

public partial class Pascal
{
    /// <summary>
    /// Listens for backend messages and writes execution or compilation summaries to the console.
    /// </summary>
    private sealed class BackendMessageListener : IMessageListener
    {
        /// <summary>
        /// Handles backend messages.
        /// </summary>
        /// <param name="message">The received message.</param>
        public void MessageReceived(Message message)
        {
            switch (message)
            {
                case InterpreterSummaryMessage interpreterSummaryMessage:
                    Console.Write(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            _interpreterSummaryFormat,
                            interpreterSummaryMessage.ExecutionCount,
                            interpreterSummaryMessage.RuntimeErrors,
                            interpreterSummaryMessage.ElapsedTime));
                    break;

                case CompilerSummaryMessage compilerSummaryMessage:
                    Console.Write(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            _compilerSummaryFormat,
                            compilerSummaryMessage.InstructionCount,
                            compilerSummaryMessage.ElapsedCodeGenerationTime));
                    break;
            }
        }
    }
}
