using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{

    /// Базовые:
    ///  Кофе, чай, газировка
    /// Добавки:
    ///  шоколад, молоко
    
    /// Кофе(1.5) + шоколад(0.3)
    /// Цена 1.8
    /// "Кофе, шоколад"

    interface IBeverage
    {
        Queue<string> GetName();
        double GetCost();
    }

    class BeverageBase : IBeverage
    {
        protected string name;
        protected double cost;

        protected BeverageBase(
            string name = "Unknown beverage",
            double cost = 0
            )
        {
            this.name = name;
            this.cost = cost;
        }

        public virtual Queue<string> GetName()
        {
            var que = new Queue<string>();
            que.Enqueue(name);

            return que;
        }
        public virtual double GetCost()
        {
            return cost;
        }
    }

    class Coffe : BeverageBase
    {
        public Coffe(
            double cost = 1.5
            )
            : base("Coffe", cost)
        {
        }
    }
    class Tea : BeverageBase
    {
        public Tea(
            double cost = 1.0
            )
            : base("Tea", cost)
        {
        }
    }
    class Soda : BeverageBase
    {
        public Soda(
            double cost = 1.3
            )
            : base("Soda", cost)
        {
        }
    }

    class CondinmentDecorator
        : BeverageBase
    {
        IBeverage b;

        protected CondinmentDecorator(
            IBeverage b,
            string name = "Unknown condinment",
            double cost = 0
            )
            : base(name, cost)
        {
            this.b = b;
        }

        public override Queue<string> GetName() 
        {
            var q = b.GetName();
            var tmp = base.GetName();

            foreach(var str in tmp)
                q.Enqueue(str);

            return q;
        }
        public override double GetCost()
        {
            return b.GetCost() + base.GetCost();
        }
    }

    class Milk
        : CondinmentDecorator
    {
        public Milk(
            IBeverage b,
            double cost = 0.3
            )
            : base(b, "Milk", cost)
        {
        }
    }
    class Chocolate
        : CondinmentDecorator
    {
        int size;

        public Chocolate(
            IBeverage b,
            double cost = 0.4
            )
            : base(b, "Chocolate", cost)
        {

        }
    }

    abstract class OutputFormatDecorator
    {
        protected IBeverage b;

        protected OutputFormatDecorator(IBeverage b)
        {
            this.b = b;
        }

        public abstract void Print();
    }

    /// Кофе(1.5) + шоколад(0.3)
    /// Цена 1.8
    /// "Кофе, шоколад"
    class DefaultOutputFormatDecorator
        : OutputFormatDecorator
    {
        public DefaultOutputFormatDecorator(IBeverage b)
            : base(b)
        {

        }

        public override void Print()
        {
            var que = b.GetName();

            string s = que.Dequeue();
            foreach(var name in que)
            {
                s += ", "+ name;
            }

            Console.Write(
                "Name: " + s +
                " \nCost: " + b.GetCost() +
                "\n"
                );
        }
    }

    class Program
    {
        static void PrintBeverage(IBeverage b)
        {
            Console.Write(
                "Name: " + b.GetName() + 
                " \nCost: " + b.GetCost() + 
                "\n"
                );
        }
        static void Main(string[] args)
        {
            IBeverage b = new Coffe();
            b = new Milk(b);
            b = new Milk(b);

            b = new Chocolate(b);

            var formater = new DefaultOutputFormatDecorator(b);

            formater.Print();

            //PrintBeverage(b);
        }
    }
}
