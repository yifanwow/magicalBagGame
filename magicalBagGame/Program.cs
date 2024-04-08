using System;
using System.Threading;

class cal
{
    static void Main(string[] args)
    {
        Console.WriteLine("请选择处理方式：\n1. 自动处理\n2. 手动处理");
        int choice = int.Parse(Console.ReadLine());

        int totalInstances = 0;
        int successfulInstances = 0;
        int failedInstances = 0;
        int initialMoney = 0;
        int targetAmount = 0;
        int attempts = 0;
        List<string> instanceResults = new List<string>();

        if (choice == 1)
        {
            Console.WriteLine("请输入要运行的实例测试次数：");
            int instanceCount = int.Parse(Console.ReadLine());

            Console.WriteLine("请输入初始金额：");
            initialMoney = int.Parse(Console.ReadLine());

            Console.WriteLine("请选择结束条件：\n1. 尝试的次数\n2. 具体金额");
            int endCondition = int.Parse(Console.ReadLine());

            if (endCondition == 1)
            {
                Console.WriteLine("请输入尝试的次数：");
                attempts = int.Parse(Console.ReadLine());
            }
            else if (endCondition == 2)
            {
                Console.WriteLine("请输入目标金额：");
                targetAmount = int.Parse(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("无效的选项。");
                return;
            }

            for (int i = 1; i <= instanceCount; i++)
            {
                Console.WriteLine($"\n\n================== 第 {i} 次实例测试 ==================\n");
                string instanceResult = AutomateGame(
                    initialMoney,
                    endCondition,
                    targetAmount,
                    attempts,
                    i
                );

                instanceResults.Add(instanceResult);

                if (instanceResult.Contains("Good"))
                {
                    successfulInstances++;
                }
                else
                {
                    failedInstances++;
                }

                totalInstances++;
            }
        }
        else if (choice == 2)
        {
            ManualGame();
        }
        else
        {
            Console.WriteLine("无效的选项。");
        }
        foreach (var result in instanceResults)
        {
            Console.WriteLine(result);
        }
        Console.WriteLine("\n\n================== 所有实例测试完成 ==================\n");
        Console.WriteLine(
            $"你的实例条件是：初始金额{initialMoney}，目标金额{targetAmount}\n总共完成了 {totalInstances} 次实例测试，成功 {successfulInstances} 次，失败 {failedInstances} 次。\n\n"
        );
        double successRate = (double)successfulInstances / totalInstances * 100;
        Console.WriteLine($"成功率为：{successRate:F2}%。\n\n");

        // 输出每次实例测试的结果
    }

    static string AutomateGame(
        int initialMoney,
        int endCondition,
        int targetAmount,
        int attempts,
        int ins
    )
    {
        Console.WriteLine($"初始金额：{initialMoney}");
        Console.WriteLine($"结束条件：{(endCondition == 1 ? "尝试的次数" : "具体金额")}");
        if (endCondition == 1)
        {
            Console.WriteLine($"尝试的次数：{attempts}");
        }
        else
        {
            Console.WriteLine($"目标金额：{targetAmount}");
        }

        int money = initialMoney;
        int successes = 0;
        int losses = 0;
        int profit = 0;
        int loss = 0;
        int maxMoney = initialMoney;
        int maxMoneyAttempt = 0;
        int maxLossAttempt = 0;
        int maxLossAmount = 0;
        int consecutiveLossCount = 0;
        int maxConsecutiveLossCount = 0;
        int LossinOnerun = 0;
        int maxLossinOnerun = 0;

        Random random = new Random();
        int amount = 1;

        bool endGame = false;

        while (!endGame && (money >= 1))
        {
            bool isSuccess = random.Next(2) == 0;
            attempts++;
            if (amount > money)
            {
                amount = money;
            }

            if (isSuccess)
            {
                money += amount;
                successes++;
                profit += amount;
                //Console.WriteLine($"成功！\n\n你放入：{amount}元，你获得了两倍其放入神奇袋子里面的钱。\n你现在有{money}元。\n");
                amount = 1;
                consecutiveLossCount = 0;
                LossinOnerun = 0;
            }
            else
            {
                money -= amount;
                losses++;
                loss += amount;
                //Console.WriteLine($"失败！\n\n你放入：{amount}元，你失去了这次放入神奇袋子里面的所有钱。\n你现在有{money}元。\n");

                consecutiveLossCount++;
                LossinOnerun = LossinOnerun + amount;
                //Console.WriteLine($"你这一轮失败了{consecutiveLossCount}次，总计在这一轮丢失了{LossinOnerun}元\n");
                amount = amount * 2;
                if (amount > maxLossAmount)
                {
                    maxLossAmount = amount;
                    maxLossAttempt = attempts;
                    maxConsecutiveLossCount = consecutiveLossCount;
                }
                if (LossinOnerun > maxLossinOnerun)
                {
                    maxLossinOnerun = LossinOnerun;
                }
            }

            if (money > maxMoney)
            {
                maxMoney = money;
                maxMoneyAttempt = attempts;
            }

            Console.WriteLine(
                $"第{ins}个实例，已完成{attempts}次尝试, 成功{successes}次，失败{losses}次，亏损{loss}，盈利{profit}，目前有{money}元。\n"
            );
            //Thread.Sleep(1);

            if (endCondition == 1 && attempts >= targetAmount)
            {
                endGame = true;
            }
            else if (endCondition == 2 && money >= targetAmount)
            {
                endGame = true;
            }
        }

        int maxWin = maxMoney - initialMoney;
        if (money < 1)
        {
            Console.WriteLine("你已经破产了。\n");
            return $"Bad! 你总共尝试了{attempts}次，成功{successes}次，失败{losses}次，亏损{loss}，盈利{profit}。\n最高拥有的钱是第{maxMoneyAttempt}次尝试，金额为{maxMoney}，如果你在这个时候停止，你可以赚{maxWin}元。\n最多一次的亏损时第{maxLossAttempt}次尝试，金额为{maxLossAmount}。\n那个时候是你那一轮第{maxConsecutiveLossCount}次连续失败，那轮的连续失败导致你亏损{maxLossinOnerun}\n连续失败{maxConsecutiveLossCount}次的概率为：{100 * (1.0 / Math.Pow(2, maxConsecutiveLossCount)):0.00}%。\n";
        }
        else
        {
            Console.WriteLine("你已经达到了你的目标。\n");
            return $"Good! 你总共尝试了{attempts}次，成功{successes}次，失败{losses}次，亏损{loss}，盈利{profit}。\n最高拥有的钱是第{maxMoneyAttempt}次尝试，金额为{maxMoney}，如果你在这个时候停止，你可以赚{maxWin}元。\n最多一次的亏损时第{maxLossAttempt}次尝试，金额为{maxLossAmount}。\n那个时候是你那一轮第{maxConsecutiveLossCount}次连续失败，那轮的连续失败导致你亏损{maxLossinOnerun}\n连续失败{maxConsecutiveLossCount}次的概率为：{100 * (1.0 / Math.Pow(2, maxConsecutiveLossCount)):0.00}%。\n";
        }
    }

    static void ManualGame()
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

        while (money >= 1)
        {
            Console.WriteLine($"你有{money}元。你要放多少钱到神奇的袋子里面？");
            string input = Console.ReadLine();
            int amount;

            if (!int.TryParse(input, out amount))
            {
                if (input.ToLower() == "q")
                {
                    break;
                }
                Console.WriteLine("输入无效。");
                continue;
            }

            if (amount > money || amount <= 0)
            {
                Console.WriteLine("输入无效。");
                continue;
            }

            bool isSuccess = random.Next(2) == 0;
            attempts++;

            if (isSuccess)
            {
                money += amount;
                successes++;
                profit += amount;
                Console.WriteLine($"成功！你获得了两倍其放入神奇袋子里面的钱。你现在有{money}元。\n");
            }
            else
            {
                money -= amount;
                losses++;
                loss += amount;
                Console.WriteLine($"失败！你失去了这次放入神奇袋子里面的所有钱，你现在有{money}元。\n");
            }

            Console.WriteLine("是否要继续尝试？按下q退出，输入数字则直接放入。");
        }

        Console.WriteLine($"你总共尝试了{attempts}次，成功{successes}次，失败{losses}次，亏损{loss}，盈利{profit}。");
    }
}
