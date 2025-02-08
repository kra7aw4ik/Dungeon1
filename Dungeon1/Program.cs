using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon1
{
    internal class Program
    {
        const int MAP_SIZE = 10;
        const int INVENTORY_SIZE = 5;
        const int INITIAL_HEALTH = 100;
        const int INITIAL_POTIONS = 3;
        const int INITIAL_ARROWS = 5;

        // Перечисление для типов комнат
        enum RoomType { Empty, Monster, Trap, Chest, Trader, Boss }

        // Переменные игрока
        static int health = INITIAL_HEALTH;
        static int potions = INITIAL_POTIONS;
        static int gold = 0;
        static int arrows = INITIAL_ARROWS;
        static bool hasSword = true;
        static bool hasBow = true;
        static string[] inventory = new string[INVENTORY_SIZE];
        static int inventoryCount = 0;

        // Переменные подземелья
        static RoomType[] dungeonMap = new RoomType[MAP_SIZE];
        static Random random = new Random();

        static void Main(string[] args)
        {
            InitializeDungeon();
            GameLoop();
        }

        // Инициализация подземелья
        static void InitializeDungeon()
        {
            for (int i = 0; i < MAP_SIZE - 1; i++)
            {
                dungeonMap[i] = (RoomType)random.Next(0, 5); // Случайный выбор комнаты (кроме босса)
            }
            dungeonMap[MAP_SIZE - 1] = RoomType.Boss; // Последняя комната - босс
        }

        // Основной цикл игры
        static void GameLoop()
        {
            Console.WriteLine("Добро пожаловать в Подземелье!");

            for (int roomNumber = 0; roomNumber < MAP_SIZE; roomNumber++)
            {
                Console.WriteLine($"\nВы входите в комнату {roomNumber + 1}...");

                switch (dungeonMap[roomNumber])
                {
                    case RoomType.Monster:
                        EncounterMonster();
                        break;
                    case RoomType.Trap:
                        TriggerTrap();
                        break;
                    case RoomType.Chest:
                        OpenChest();
                        break;
                    case RoomType.Trader:
                        VisitTrader();
                        break;
                    case RoomType.Boss:
                        FightBoss();
                        break;
                    default:
                        Console.WriteLine("Комната пуста. Здесь ничего не происходит.");
                        break;
                }

                if (health <= 0)
                {
                    Console.WriteLine("Вы погибли. Игра окончена.");
                    return;
                }

                Console.WriteLine($"Ваше здоровье: {health}, Зелья: {potions}, Золото: {gold}, Стрелы: {arrows}");
                DisplayInventory();
            }

            Console.WriteLine("Поздравляем! Вы прошли подземелье и победили финального босса!");
        }

        // Встреча с монстром
        static void EncounterMonster()
        {
            Console.WriteLine("Вы столкнулись с монстром!");
            int monsterHealth = random.Next(20, 51);

            while (health > 0 && monsterHealth > 0)
            {
                Console.WriteLine("Ваш ход! Выберите действие:");
                Console.WriteLine("1. Атаковать мечом");
                Console.WriteLine("2. Атаковать луком");

                if (potions > 0)
                    Console.WriteLine("3. Выпить зелье");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (hasSword)
                        {
                            int damage = random.Next(10, 21);
                            Console.WriteLine($"Вы нанесли монстру {damage} урона мечом.");
                            monsterHealth -= damage;
                        }
                        else
                        {
                            Console.WriteLine("У вас нет меча!");
                        }
                        break;
                    case "2":
                        if (hasBow)
                        {
                            if (arrows > 0)
                            {
                                int damage = random.Next(5, 16);
                                Console.WriteLine($"Вы нанесли монстру {damage} урона из лука.");
                                monsterHealth -= damage;
                                arrows--;
                            }
                            else
                            {
                                Console.WriteLine("У вас нет стрел!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("У вас нет лука!");
                        }
                        break;
                    case "3":
                        if (potions > 0)
                        {
                            health += 30;
                            potions--;
                            Console.WriteLine("Вы выпили зелье и восстановили 30 здоровья.");
                        }
                        else
                        {
                            Console.WriteLine("У вас нет зелий!");
                        }
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                if (monsterHealth > 0)
                {
                    int monsterDamage = random.Next(5, 16);
                    Console.WriteLine($"Монстр нанес вам {monsterDamage} урона.");
                    health -= monsterDamage;
                }

                Console.WriteLine($"Ваше здоровье: {health}, Здоровье монстра: {monsterHealth}");
            }

            if (health <= 0)
            {
                Console.WriteLine("Вы проиграли в бою с монстром.");
            }
            else
            {
                Console.WriteLine("Вы победили монстра!");
            }
        }

        // Попадание в ловушку
        static void TriggerTrap()
        {
            Console.WriteLine("Вы попали в ловушку!");
            int damage = random.Next(10, 21);
            Console.WriteLine($"Вы потеряли {damage} здоровья.");
            health -= damage;

            if (health <= 0)
            {
                Console.WriteLine("Вы погибли от ловушки.");
            }
        }

        // Открытие сундука
        static void OpenChest()
        {
            Console.WriteLine("Вы нашли сундук! Чтобы его открыть, решите загадку:");
            int num1 = random.Next(1, 11);
            int num2 = random.Next(1, 11);
            int answer = num1 + num2;

            Console.WriteLine($"Сколько будет {num1} + {num2}?");
            string playerAnswer = Console.ReadLine();

            if (playerAnswer == answer.ToString())
            {
                Console.WriteLine("Верно! Вы открыли сундук.");
                GiveReward();
            }
            else
            {
                Console.WriteLine("Неверно! Сундук остался закрытым.");
            }
        }

        // Выдача награды из сундука
        static void GiveReward()
        {
            int rewardType = random.Next(0, 3); // 0: зелье, 1: золото, 2: стрелы
            switch (rewardType)
            {
                case 0:
                    if (inventoryCount < INVENTORY_SIZE)
                    {
                        inventory[inventoryCount] = "Зелье";
                        inventoryCount++;
                        Console.WriteLine("Вы нашли зелье и добавили его в инвентарь.");
                    }
                    else
                    {
                        Console.WriteLine("Вы нашли зелье, но ваш инвентарь полон.");
                    }
                    break;
                case 1:
                    int goldAmount = random.Next(20, 51);
                    gold += goldAmount;
                    Console.WriteLine($"Вы нашли {goldAmount} золота.");
                    break;
                case 2:
                    int arrowAmount = random.Next(3, 8);
                    arrows += arrowAmount;
                    Console.WriteLine($"Вы нашли {arrowAmount} стрел.");
                    break;
            }
        }

        // Посещение торговца
        static void VisitTrader()
        {
            Console.WriteLine("Вы встретили торговца.");
            Console.WriteLine($"У вас {gold} золота. Хотите купить зелье за 30 золота? (да/нет)");
            string choice = Console.ReadLine();

            if (choice.ToLower() == "да")
            {
                if (gold >= 30)
                {
                    gold -= 30;
                    potions++;
                    Console.WriteLine("Вы купили зелье.");
                }
                else
                {
                    Console.WriteLine("У вас недостаточно золота.");
                }
            }
            else
            {
                Console.WriteLine("Вы отказались от покупки.");
            }
        }

        // Битва с боссом
        static void FightBoss()
        {
            Console.WriteLine("Вы столкнулись с финальным боссом!");
            int bossHealth = random.Next(70, 101);

            while (health > 0 && bossHealth > 0)
            {
                Console.WriteLine("Ваш ход! Выберите действие:");
                Console.WriteLine("1. Атаковать мечом");
                Console.WriteLine("2. Атаковать луком");

                if (potions > 0)
                    Console.WriteLine("3. Выпить зелье");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (hasSword)
                        {
                            int damage = random.Next(10, 21);
                            Console.WriteLine($"Вы нанесли боссу {damage} урона мечом.");
                            bossHealth -= damage;
                        }
                        else
                        {
                            Console.WriteLine("У вас нет меча!");
                        }
                        break;
                    case "2":
                        if (hasBow)
                        {
                            if (arrows > 0)
                            {
                                int damage = random.Next(5, 16);
                                Console.WriteLine($"Вы нанесли боссу {damage} урона из лука.");
                                bossHealth -= damage;
                                arrows--;
                            }
                            else
                            {
                                Console.WriteLine("У вас нет стрел!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("У вас нет лука!");
                        }
                        break;
                    case "3":
                        if (potions > 0)
                        {
                            health += 30;
                            potions--;
                            Console.WriteLine("Вы выпили зелье и восстановили 30 здоровья.");
                        }
                        else
                        {
                            Console.WriteLine("У вас нет зелий!");
                        }
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                if (bossHealth > 0)
                {
                    int bossDamage = random.Next(10, 21);
                    Console.WriteLine($"Босс нанес вам {bossDamage} урона.");
                    health -= bossDamage;
                }

                Console.WriteLine($"Ваше здоровье: {health}, Здоровье босса: {bossHealth}");
            }

            if (health <= 0)
            {
                Console.WriteLine("Вы проиграли в бою с боссом.");
            }
            else
            {
                Console.WriteLine("Вы победили босса!");
            }
        }

        // Вывод инвентаря
        static void DisplayInventory()
        {
            Console.Write("Инвентарь: ");
            if (inventoryCount == 0)
            {
                Console.WriteLine("пусто");
            }
            else
            {
                for (int i = 0; i < inventoryCount; i++)
                {
                    Console.Write(inventory[i] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
