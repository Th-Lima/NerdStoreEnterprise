﻿using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Razor;

namespace NSE.WebApp.MVC.Extensions
{
    public static class RazorHelpers
    {
        public static string HashEmailForGravatar(this RazorPage page, string email)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string CurrencyFormat(this RazorPage page, decimal price)
        {
            return price > 0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", price) : "Gratuito";
        }

        private static string CurrencyFormat(decimal valor)
        {
            return string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", valor);
        }

        public static string StockMessage(this RazorPage page, int amount)
        {
            return amount > 0 ? $"Apenas {amount} em estoque!" : "Produto esgotado!";
        }

        public static string UnitByProduct(this RazorPage page, int unit)
        {
            return unit > 1 ? $"{unit} unidades" : $"{unit} unidade";
        }
        public static string UnitByProductsTotalValue(this RazorPage page, int unit, decimal price)
        {
            return $"{unit}x {CurrencyFormat(price)} = Total: {CurrencyFormat(price * unit)}";
        }

        public static string DisplayStatus(this RazorPage page, int status)
        {
            var statusMessage = "";
            var statusClass = "";

            switch (status)
            {
                case 1:
                    statusClass = "info";
                    statusMessage = "Em aprovação";
                    break;
                case 2:
                    statusClass = "primary";
                    statusMessage = "Aprovado";
                    break;
                case 3:
                    statusClass = "danger";
                    statusMessage = "Recusado";
                    break;
                case 4:
                    statusClass = "success";
                    statusMessage = "Entregue";
                    break;
                case 5:
                    statusClass = "warning";
                    statusMessage = "Cancelado";
                    break;

            }

            return $"<span class='badge badge-{statusClass}'>{statusMessage}</span>";
        }


        public static string SelectOptionsByAmount(this RazorPage page, int amount, int valueSelected = 0)
        {
            var sb = new StringBuilder();
            for (var i = 1; i <= amount; i++)
            {
                var selected = "";
                if (i == valueSelected) selected = "selected";
                sb.Append($"<option {selected} value='{i}'>{i}</option>");
            }

            return sb.ToString();
        }
    }
}