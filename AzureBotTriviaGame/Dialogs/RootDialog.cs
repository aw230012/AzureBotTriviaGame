using System;
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
        private IDialog<object> _menuDialog = null;

        public async Task StartAsync(IDialogContext context)
        {
            await Task.Run(() => context.Wait(MessageReceivedAsync));
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = (IMessageActivity)await result;

            if (_menuDialog == null)
            {
                _menuDialog = new MenuDialog();
            }

            await context.Forward(_menuDialog, AfterMenuResume, activity, CancellationToken.None);
        }

        private Task AfterMenuResume(IDialogContext context, IAwaitable<object> result)
        {
            return Task.CompletedTask;
        }
    }
}