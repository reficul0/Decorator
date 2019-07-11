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
    public enum SizesOfPortion 
    {
        small = 1,
        medium,
        big
    }
    interface IBeverage
    {
        Queue<string> GetName();
        double GetCost();
    }

    class BeverageBase : IBeverage
    {
        protected string name;
        protected double cost;
        public int size;

        protected BeverageBase(
            string name = "Unknown beverage",
            double cost = 0,
            int size = 1
            )
        {
            this.name = name + " "+ Enum.Parse(typeof(SizesOfPortion),Enum.GetName(typeof(SizesOfPortion),size));
            this.cost = cost;
            this.size = size;
        }

        public virtual Queue<string> GetName()
        {
            var que = new Queue<string>();
            que.Enqueue(name);

            return que;
        }
        public virtual double GetCost()
        {
            return cost*size;
        }
    }

    class Coffe : BeverageBase
    {
        public Coffe(
            double cost = 1.5, int size = 1
            )
            : base("Coffe", cost, size)
        {
        }
    }
    class Tea : BeverageBase
    {
        public Tea(
            double cost = 1.0, int size = 1
            )
            : base("Tea", cost, size)
        {
        }
    }
    class Soda : BeverageBase
    {
        public Soda(
            double cost = 1.3, int size = 1
            )
            : base("Soda", cost, size)
        {
        }
    }

    class CondimentDecorator
        : BeverageBase
    {
        IBeverage beverage;
    

        protected CondimentDecorator(
            IBeverage beverage,
            string name = "Unknown condinment",
            double cost = 0,
            int size = 1
            )
            : base(name, cost, size)
        {
            this.beverage = beverage;
        }

        public override Queue<string> GetName() 
        {
            var que = beverage.GetName();
            var tmp = base.GetName();

            foreach(var str in tmp)
                que.Enqueue(str);

            return que;
        }
        public override double GetCost()
        {
            return beverage.GetCost() + base.GetCost();
        }
    }

    class Milk
        : CondimentDecorator
    {
        public Milk(
            IBeverage beverage,
            double cost = 0.3,
            int size = 1
            )
            : base(beverage, "Milk", cost, size)
        {
        }
    }
    class Chocolate
        : CondimentDecorator
    {

        public Chocolate(
            IBeverage beverage,
            double cost = 0.4,
            int size = 1
            )
            : base(beverage, "Chocolate", cost, size)
        {
        }
    }

    abstract class OutputFormatDecorator
    {
        protected IBeverage beverage;

        protected OutputFormatDecorator(IBeverage beverage)
        {
            this.beverage = beverage;
        }

        public abstract void Print();
    }

    /// Кофе(1.5) + шоколад(0.3)
    /// Цена 1.8
    /// "Кофе, шоколад"
    class DefaultOutputFormatDecorator
        : OutputFormatDecorator
    {
        public DefaultOutputFormatDecorator(IBeverage beverage)
            : base(beverage)
        {

        }

        public override void Print()
        {
            var que = beverage.GetName();

            string str = que.Dequeue();
            foreach(var name in que)
            {
                str += ", "+ name;
            }

            Console.Write(
                "Name: " + str +
                " \nCost: " + beverage.GetCost() +
                "\n"
                );
        }
    }

    class Program
    {
        static void PrintBeverage(IBeverage beverage)
        {
            Console.Write(
                "Name: " + beverage.GetName() + 
                " \nCost: " + beverage.GetCost() + 
                "\n"
                );
        }
        static void Main(string[] args)
        {
            
            int big = (int)SizesOfPortion.big;
            int medium = (int)SizesOfPortion.medium;
            int small = (int)SizesOfPortion.small;

           

            IBeverage beverage = new Coffe(size:medium);
            beverage = new Milk(beverage, size:big);
            beverage = new Chocolate(beverage, size:small);

            var formater = new DefaultOutputFormatDecorator(beverage);

            formater.Print();
            Console.ReadLine();
        }
    }
}
