using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AzureBotTriviaGame.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await Task.Run(() => context.Wait(MessageReceivedAsync));
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = (IMessageActivity)await result;

            if (activity.Text == "trivia")
            {
                await context.PostAsync("You chose trivia!");
            }
            else
            {
                await BeginMenu(context);
            }

            context.Wait(MessageReceivedAsync);
        }

        private async Task BeginMenu(IDialogContext context)
        {
            const string imageUrl = "https://dev.botframework.com/Client/Images/ChatBot-BotFramework.png";
            const string learnMoreUrl = "https://dev.botframework.com/";
            const string joke = "Why do seagulls fly be the sea?  Because if they flew by a bay they'd be bagels.";

            var reply = ((Activity)context.Activity).CreateReply();
            var card = new HeroCard("Make a choice!")
            {
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "Learn More", value: learnMoreUrl),
                    new CardAction(ActionTypes.ImBack, "Tell a Joke", value: joke),
                    new CardAction(ActionTypes.PostBack, "Play Trivia", value: "trivia")
                },
                Images = new List<CardImage>
                {
                    new CardImage(imageUrl)
                }
            };

            reply.Attachments.Add(card.ToAttachment());
            await context.PostAsync(reply);
        }
    }
}