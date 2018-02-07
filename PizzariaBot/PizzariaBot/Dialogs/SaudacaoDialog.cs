using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace PizzariaBot.Dialogs
{
    [Serializable]
    public class SaudacaoDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Olá eu sou o Maike, seu assistente");

            await Respond(context);

            context.Wait(MessageReceiveAsync);
        }

        private async Task Respond(IDialogContext context)
        {
            var userName = string.Empty;

            context.UserData.TryGetValue<string>("Name", out userName);

            if(string.IsNullOrWhiteSpace(userName))
            {
                await context.PostAsync("Qual é o seu nome?");

                context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                await context.PostAsync($"Oi {userName}. Como eu posso te ajudar?");
            }
        }

        private async Task MessageReceiveAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            var userName = string.Empty;
            var getName = false;

            context.UserData.TryGetValue<string>("Name", out userName);

            context.UserData.TryGetValue<bool>("GetName", out getName);

            if(getName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", getName);
            }

            await Respond(context);

            context.Done(message);
        }
    }
}