using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Task4;

class Program
{
    static void Main(string[] args)
    {
        //Выведите платёжные ссылки для трёх разных систем платежа: 
        //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
        //order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
        //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}

        var order = new Order(15, 12000);

        IEnumerable<IPaymentSystem> paymentSystems = new IPaymentSystem[]
            { new HashMD5(), new MD5AndSum(), new HashSHA1() };

        Convert.ResultOutput(paymentSystems, order);
    }

    interface IPaymentSystem
    {
        string GetPaymentLink(Order order);
    }

    class Order
    {
        public readonly int Id;
        public readonly int Amount;

        public Order(int id, int amount) => (Id, Amount) = (id, amount);
    }

    class HashMD5 : IPaymentSystem
    {
        private string _prefix = "pay.system1.ru/order?";

        public string GetPaymentLink(Order order)
        {
            return $"{_prefix} amount= {order.Amount}RUB & hash = {Convert.CreateMD5(order.Id)}";
        }
    }

    class MD5AndSum : IPaymentSystem
    {
        private string _prefix = "order.system2.ru/pay?";

        public string GetPaymentLink(Order order)
        {
            return $"{_prefix}hash= {Convert.CreateMD5(order.Id + order.Amount)}({order.Id} + {order.Amount})";
        }
    }

    class HashSHA1 : IPaymentSystem
    {
        private string _prefix = "system3.com/pay?";
        private int _secretKey;

        public string GetPaymentLink(Order order)
        {
            _secretKey = order.Amount / order.Id;

            return
                $"{_prefix}amount= {order.Amount} & curency= RUB & hash= {Convert.CreateSHA1(order.Amount + order.Id + _secretKey)}({order.Amount} + {order.Id} + {_secretKey})";
        }
    }

    class Convert
    {
        public static string CreateMD5(int input)
        {
            using var md5 = MD5.Create();

            var hash = string.Concat(md5.ComputeHash(BitConverter
                    .GetBytes(input))
                .Select(x => x.ToString("x2")));

            return hash;
        }

        public static string CreateSHA1(int input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input.ToString()));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public static void ResultOutput(IEnumerable<IPaymentSystem> paymentSystems, Order order)
        {
            foreach (var paymentSystem in paymentSystems)
                Console.WriteLine(paymentSystem.GetPaymentLink(order));
        }
    }
}