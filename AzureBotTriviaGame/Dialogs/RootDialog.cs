﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AzureBotTriviaGame.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private IDialog<object> _triviaDialog = null;

        public async Task StartAsync(IDialogContext context)
        {
            await Task.Run(() => context.Wait(MessageReceivedAsync));
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = (IMessageActivity)await result;

            if (activity.Text.Contains("trivia"))
            {
                if (_triviaDialog == null)
                {
                    _triviaDialog = new PlayTriviaDialog();
                }

                await context.Forward(_triviaDialog, AfterTriviaResume, activity, CancellationToken.None);
            }
            else
            {
                if (activity.Text == "joke")
                {
                    const string joke = "Why do seagulls fly be the sea?  Because if they flew by the bay, they'd be bagels.";
                    await context.PostAsync(joke);
                }

                await BeginMenu(context);
            }

            context.Wait(MessageReceivedAsync);
        }

        private Task AfterTriviaResume(IDialogContext context, IAwaitable<object> result)
        {
            // do nothing here because we're handling all the output in the PlayTriviaDialog
            return Task.CompletedTask;   
        }

        private async Task BeginMenu(IDialogContext context)
        {
            const string imageUrl = "https://dev.botframework.com/Client/Images/ChatBot-BotFramework.png";
            const string learnMoreUrl = "https://dev.botframework.com/";

            var reply = ((Activity)context.Activity).CreateReply();
            var card = new HeroCard("Make a choice!")
            {
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "Learn More", value: learnMoreUrl),
                    new CardAction(ActionTypes.PostBack, "Tell a Joke", value: "joke"),
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