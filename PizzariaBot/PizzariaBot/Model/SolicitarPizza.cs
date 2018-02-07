using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzariaBot.Model
{
    public enum Cardapio
    {
        Brócolis,
        Cheddar,
        Frango,
        Portuguesa,
        Mussarela,
        Atum,
        Romana
    }

    public enum Pagamento
    {
        Dinheiro,
        Credito,
        Debito,
    }

    [Serializable]
    public class SolicitarPizza
    {
        public Cardapio Cardapio;
        public long? QuantidadePizza;
        public Pagamento? Pagamento;
        public string Endereco;
        public double Total;

        public static IForm<SolicitarPizza> BuildForm()
        {
            return new FormBuilder<SolicitarPizza>()
                .Message("Bem vindo ao assistente da pizzaria Michelangelo")
                .Build();
        }
    }
}