using System;

class cal
{
    static void Main(string[] args)
    {
        Console.WriteLine("你初始想拥有多少钱？");
        int initialMoney = int.Parse(Console.ReadLine());
        int money = initialMoney;
        int attempts = 0;
        int successes = 0;
        int losses = 0;
        int profit = 0;
        int loss = 0;

        Random random = new Random();

        while (true)
        {
            Console.WriteLine($"你有{money}元。你要放多少钱到神奇的袋子里面？");
            int amount = int.Parse(Console.ReadLine());
            if (amount > money || amount <= 0)
            {
                Console.WriteLine("输入无效。");
                continue;
            }

            bool isSuccess = random.Next(2) == 0; // 有1/2的概率成功，1/2的概率失败
            attempts++;

            if (isSuccess)
            {
                Console.WriteLine("成功！你获得了两倍其放入神奇袋子里面的钱。");
                money += amount; // 获得两倍金额，但因为放入的金额已经从总数中扣除，所以只加回一倍
                successes++;
                profit += amount;
            }
            else
            {
                Console.WriteLine("失败！你失去了这次放入神奇袋子里面的所有钱。");
                money -= amount;
                losses++;
                loss += amount;
            }

            Console.WriteLine("是否要继续尝试？按下q退出，其他键继续。");
            string input = Console.ReadLine();
            if (input.ToLower() == "q")
            {
                break;
            }
        }

        Console.WriteLine($"你总共尝试了{attempts}次，成功{successes}次，失败{losses}次，亏损{loss}，盈利{profit}。");
    }
}
