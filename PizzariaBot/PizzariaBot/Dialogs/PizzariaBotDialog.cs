using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using PizzariaBot.Model;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PizzariaBot.Dialogs
{
    public class PizzariaBotDialog
    {
        public static readonly IDialog<string> dialog = Chain.PostToChain()
            .Select(msg => msg.Text)
            .Switch(
            new RegexCase<IDialog<string>>(new Regex("^Oi", RegexOptions.IgnoreCase), (context, text) =>
            {
                return Chain.ContinueWith(new SaudacaoDialog(), AfterSaudacao);
            }),
            new RegexCase<IDialog<string>>(new Regex("^Ola", RegexOptions.IgnoreCase), (context, text) =>
            {
                return Chain.ContinueWith(new SaudacaoDialog(), AfterSaudacao);
            }),
            new DefaultCase<string, IDialog<string>>((context, text) =>
            {
                return Chain.ContinueWith(FormDialog.FromForm(SolicitarPizza.BuildForm, FormOptions.PromptInStart), AfterSaudacao);
            }))
            .Unwrap()
            .PostToUser();

        private async static Task<IDialog<string>> AfterSaudacao(IBotContext context, IAwaitable<object> item)
        {
            var token = await item;
            var name = "User";

            context.UserData.TryGetValue<string>("Name", out name);
            return Chain.Return($"Obrigado por sua escolha: {name}");
        }
    }
}