using HotelBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HotelBot.Dialogs
{
    [LuisModel("01f7e5c2-fb1f-49d0-acbe-2daa063d8bf3", "7abfc4f6aaf341c89ed4ea1f949bcbd0")]
    [Serializable]
    public class LUISDialog : LuisDialog<RoomReservation>
    {
        private readonly BuildFormDelegate<RoomReservation> _reserveRoom;

        public LUISDialog(BuildFormDelegate<RoomReservation> reserveRoom)
        {
            this._reserveRoom = reserveRoom;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Desculpe eu não sei o que isso significa.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingDialog(), Callback);
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }

        [LuisIntent("ReserveRoom")]
        public async Task RoomReservation(IDialogContext context, LuisResult result)
        {
            var enrollmentForm = new FormDialog<RoomReservation>(new RoomReservation(), this._reserveRoom, FormOptions.PromptInStart);
            context.Call<RoomReservation>(enrollmentForm, Callback);
        }

        [LuisIntent("QueryAmenities")]
        public async Task QueryAmenities(IDialogContext context, LuisResult result)
        {
            foreach (var entity in result.Entities.Where(Entity => Entity.Type == "Amenidade"))
            {
                var value = entity.Entity.ToLower();
                if (value == "piscina" || value == "academia" || value == "wifi" || value == "toalhas")
                {
                    await context.PostAsync("Sim, nos temos!");
                    context.Wait(MessageReceived);
                    return;
                }
                else
                {
                    await context.PostAsync("Desculpe, nós não temos!");
                    context.Wait(MessageReceived);
                    return;
                }
            }
            await context.PostAsync("Desculpe, nós não temos!");
            context.Wait(MessageReceived);
            return;
        }
    }
}