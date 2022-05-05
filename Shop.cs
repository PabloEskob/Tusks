using System;
using System.Collections.Generic;

namespace ConsoleApplication2
{
     class Shop
    {
        private static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();
            Shop shop = new Shop(warehouse);

            GoodDisplay goodDisplay = new GoodDisplay();

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            goodDisplay.DisplayAll(warehouse.Goods); //Вывод всех товаров на складе с их остатком

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11,3);//при такой ситуации возникает ошибка так, как нет нужного количества товара на складе
                

            goodDisplay.DisplayAll(cart.Goods); //Вывод всех товаров в корзине

            Console.WriteLine(cart.CreateOrder().Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    public class Good
    {
        private string _name;

        public string Name => _name;

        public Good(string name)
        {
            _name = name;
        }
    }

    public class GoodDisplay
    {
        public void DisplayAll(IReadOnlyDictionary<Good, int> goods)
        {
            if (goods == null)
                throw new ArgumentNullException(nameof(goods));

            Console.WriteLine("Остаток товаров");

            foreach (var product in goods)
                Console.WriteLine(product.Key.Name + ": " + product.Value);

            Console.ReadKey();
        }
    }

    public class Warehouse
    {
        private Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public IReadOnlyDictionary<Good, int> Goods => _goods;

        public void Delive(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException(nameof(good));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (_goods.ContainsKey(good))
                _goods[good] += count;
            else
                _goods.Add(good, count);
        }

        public void DeliverFrom(Good good, int count)
        {
            var availability = CheckAvailability(good, count);
            var countInWarehouse = _goods[good];
            var expectedCount = count;

            switch (availability)
            {
                case 1:
                    _goods[good] = countInWarehouse - expectedCount;
                    if (_goods[good] == 0)
                        _goods.Remove(good);
                    break;
                case 0:
                    throw new ArgumentOutOfRangeException(nameof(count));
                case -1:
                    throw new ArgumentOutOfRangeException(nameof(good));
                default:
                    throw new ArgumentOutOfRangeException(nameof(availability));
            }
        }

        private int CheckAvailability(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException(nameof(good));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (_goods.ContainsKey(good) && _goods[good] >= count)
                return 1;
            if (_goods.ContainsKey(good) && _goods[good] < count)
                return 0;
            return -1;
        }
    }

    public class Order
    {
        private string _paylink = "просто какая-нибудь случайная строка";

        public string Paylink => _paylink;
    }

    public class Cart
    {
        private readonly Warehouse _warehouse;
        private Dictionary<Good, int> _goods = new Dictionary<Good, int>();
        public IReadOnlyDictionary<Good, int> Goods => _goods;

        public Cart(Warehouse warehouse)
        {
            _warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));
        }
        
        public void Add(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException(nameof(good));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            _warehouse.DeliverFrom(good, count);

            if (_goods.ContainsKey(good))
                _goods[good] += count;
            else
                _goods.Add(good, count);
        }

        public Order CreateOrder()
        {
            return new Order();
        }
    }

    public class Shop
    {
        private readonly Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));
        }

        public Cart Cart()
        {
            return new Cart(_warehouse);
        }
    }
}