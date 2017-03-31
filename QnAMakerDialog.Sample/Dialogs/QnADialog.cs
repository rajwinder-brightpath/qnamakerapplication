using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace QnAMakerDialog.Sample.Dialogs
{
    [Serializable]
    [QnAMakerService("16c5a1e70abf426aa3b3f97604b292da", "5ab0bc00-b599-4bbd-bc9d-ff81611949aa")]
    public class QnADialog : QnAMakerDialog<object>
    {
        /// <summary>
        /// Handler used when the QnAMaker finds no appropriate answer
        /// </summary>
        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            await context.PostAsync($"Sorry, I couldn't find an answer for '{originalQueryText}'.");
            context.Wait(MessageReceived);
        }

        /// <summary>
        /// This is the default handler used if no specific applicable score handlers are found
        /// </summary>
        public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            // ProcessResultAndCreateMessageActivity will remove any attachment markup from the results answer
            // and add any attachments to a new message activity with the message activity text set by default
            // to the answer property from the result
            var messageActivity = ProcessResultAndCreateMessageActivity(context, ref result);
            messageActivity.Text = $"I found an answer that might help...{result.Answer}.";

            await context.PostAsync(messageActivity);

            context.Wait(MessageReceived);
        }

        /// <summary>
        /// Handler to respond when QnAMakerResult score is a maximum of 50
        /// </summary>
        [QnAMakerResponseHandler(50)]
        public async Task LowScoreHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            var messageActivity = ProcessResultAndCreateMessageActivity(context, ref result);
            messageActivity.Text = $"I found an answer that might help...{result.Answer}.";
            await context.PostAsync(messageActivity);

            context.Wait(MessageReceived);
        }
    }
}