using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace HotelBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            // O método PostAsync será chamado quando o usuário fizer a primeira interação.
            await context.PostAsync("Olá, eu sou o Bot de Teste");

            await Respond(context);

            // Este método é crucial para o funcionamento do chat bot.
            /* Aqui garantimos que o bot irá fazer algo ao finalizar a conversa, ou seja, irá aguardar até 
             a próxima interação do usuário. Todo bot precisa saber o que fazer ao finalizar a interação! Nesse caso irá aguardar.*/
            context.Wait(MessageReceiveAsync);
        }

        private static async Task Respond(IDialogContext context)
        {
            var userName = string.Empty;

            /* Estamos tentando pegar o nome do usuáriom localizado na variável "Name"
            então iremos adicioná-lo na variável userName, para ficar salvo em nosso cache. */
            context.UserData.TryGetValue<string>("Name", out userName);

            // Se não conseguirmos pegar o valor "Name" do usuário, iremos solicitar ao mesmo.
            if (string.IsNullOrWhiteSpace(userName))
            {
                await context.PostAsync("Qual é o seu nome?");

                // Estamos definindo o valor do usuário na váriavel "Name"
                context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                // Iremos saudar o usuário informando seu nome.
                await context.PostAsync($"Oi {userName}. Como eu posso te ajudar?");
            }
        }

        private async Task MessageReceiveAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            var userName = string.Empty;
            var getName = false;

            /* Estamos tentando pegar o nome do usuáriom localizado na variável "Name"
            então iremos adicioná-lo na variável userName, para ficar salvo em nosso cache. */
            context.UserData.TryGetValue<string>("Name", out userName);

            // Flag para verificar se já temos ou não o nome do usuário
            context.UserData.TryGetValue<bool>("GetName", out getName);

            // Se já tivermos iremos setar o valor na váriavel userName
            if (getName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);
            }

            await Respond(context);

            // Todo bot precisa saber o que fazer ao finalizar a interação! Nesse caso irá executar uma chamada recursiva (loop).
            context.Done(message);
        }
    }
}