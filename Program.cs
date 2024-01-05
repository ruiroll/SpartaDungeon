using System;
using System.ComponentModel;
using System.Numerics;
using System.Text.Json;
using System.Xml.Linq;
using Newtonsoft.Json;
public class Character
{
    public int Level { get; set; }
    public string Chad { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public int Atk2 { get; set; }
    public int Def2 { get; set; }
    public int Health { get; set; }
    public int Gold { get; set; }
    public Inventory inventory { get; set; }

    public Character()
    {
        Level = 1;
        Chad = "전사";
        Atk = 10;
        Def = 5;
        Atk2 = 0;
        Def2 = 0;
        Health = 100;
        Gold = 1500;
        inventory = new Inventory();
    }

    public Character(int level, string chad, int atk, int def, int health, int gold, Inventory inven)
    {
        Level = level;
        Chad = chad;
        Atk = atk;
        Def = def;
        Health = health;
        Gold = gold;
        inventory = inven;
    }

    public override string ToString()
    {
        string str = "";
        str += "Lv. " + Convert.ToString(Level).PadLeft(2, '0');
        str += "\nChad ( " + Chad + " )\n";
        str += "공격력 : " + (Atk + Atk2);
        if(Atk2 > 0)
        {
            str += " (+" + Atk2 + ")";
        }
        str += "\n방어력 : " + (Def + Def2);
        if (Def2 > 0)
        {
            str += " (+" + Def2 + ")";
        }
        str += "\n체 력 : " + Health;
        str += "\nGold : " + Gold + " G\n";

        return str;
    }
}

public class Item
{
    public string Name { get; set; }
    public char Type { get; set; }
    public int Stat { get; set; }
    public string Explanation { get; set; }
    public int Price { get; set; }
    public bool IsEquipped { get; set; }
    public bool Buy { get; set; }

    public Item(string name, char type, int stat, string explanation, int price)
    {
        Name = name;
        Type = type;
        Stat = stat;
        Explanation = explanation;
        Price = price;
        IsEquipped = false;
        Buy = false;
    }

    public override string ToString()
    {
        string itemInfo = String.Format($"{Name,-10}| ");
        if( Type == 'w' ) 
        {
            itemInfo += String.Format($"공격력 +{Stat,-3}| ");
        }
        else
        {
            itemInfo += String.Format($"방어력 +{Stat,-3}| ");
        }
        itemInfo += String.Format($"{Explanation,-30}");

        return itemInfo;
    }

}

public class Store
{
    public bool manage;
    private List<Item> Items;

    public Store() 
    {
        manage = false;
        Items = new List<Item>();
        Items.Add(new Item("수련자 갑옷", 'a', 5, "수련에 도움을 주는 갑옷입니다.", 1000));
        Items.Add(new Item("무쇠 갑옷", 'a', 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000));
        Items.Add(new Item("스파르타의 갑옷", 'a', 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500));
        Items.Add(new Item("낡은 검", 'w', 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600));
        Items.Add(new Item("청동 도끼", 'w', 5, "어디선가 사용됐던 것 같은 도끼입니다.", 1500));
        Items.Add(new Item("스파르타의 창", 'w', 7, "스파르타의 전사들이 사용했다는 전설의 검입니다.", 3000));
    }

    public Item getItem(int i)
    {
        return Items[i];
    }
    public int ItemCount()
    {
        return Items.Count();
    }

    public override string ToString()
    {
        string str = "";
        for(int i = 0; i < Items.Count; i++)
        {
            str += "- ";
            if (manage == true)
            {
                str += (i + 1) + " ";
            }
            str += Items[i].ToString();
            if (Items[i].Buy == true)
            {
                str += "|  구매 완료\n";
            }
            else
            {
                str += "| " + Items[i].Price + " G\n";
            }
        }
        return str;
    }
}

public class Inventory
{
    public bool manage;
    public List<Item> Items;

    public Inventory()
    {
        Items = new List<Item>();
        manage = false;
    }


    public void addItem(Item item)
    {
        Items.Add(item);
    }

    public void removeItem(Item item)
    {
        Items.Remove(item);
    }

    public Item getItem(int i)
    {
        return Items[i];
    }

    public int ItemCount()
    {
        return Items.Count();
    }

    public override string ToString()
    {
        string str = "";
        for (int i = 0; i < Items.Count; i++)
        {
            str += "- ";
            if (manage == true)
            {
                str += (i + 1) + " ";
            }
            if (Items[i].IsEquipped == true)
            {
                str += "[E]";
            }
            str += Items[i].ToString() + "\n";
        }
        return str;
    }
}

public class GameStart
{
    public int[] playerPos = { 60, 28 };
    Character character;
    Store store;
    public GameStart(Character c, Store s)
    {
        character = c;
        store = s;
    }
    public void save(Character character)
    {
        string filePath = "character.json";
        string json = JsonConvert.SerializeObject(character);
        File.WriteAllText(filePath, json);
    }

    public void StartScene()
    {
        Console.Clear();

        DrawFrame();
        DrawPlayer(playerPos, 2);

        while (true)
        {
            // 키 입력이 있는 경우에만 방향을 변경합니다.
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        playerPos[1] -= 1;
                        DrawPlayer(playerPos, 0);
                        break;
                    case ConsoleKey.DownArrow:
                        playerPos[1] += 1;
                        DrawPlayer(playerPos, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        playerPos[0] -= 1;
                        DrawPlayer(playerPos, 2);
                        break;
                    case ConsoleKey.RightArrow:
                        playerPos[0] += 1;
                        DrawPlayer(playerPos, 3);
                        break;
                }
            }

            if (playerPos[1] < 1) playerPos[1] = 1;
            if (playerPos[1] > 28) playerPos[1] = 28;
            if (playerPos[0] < 2) playerPos[0] = 2;
            if (playerPos[0] > 118) playerPos[0] = 118;

            if (playerPos[0] >= 25 && playerPos[0] <= 35)
            {
                if (playerPos[1] >= 4 && playerPos[1] <= 9)
                {
                    playerPos[0] = 30;
                    playerPos[1] = 10;
                    RestScene();
                    break;
                }
                else if (playerPos[1] >= 20 && playerPos[1] <= 25)
                {
                    playerPos[0] = 30;
                    playerPos[1] = 26;
                    ShowInventory();
                    break;
                }
            }
            else if (playerPos[0] >= 85 && playerPos[0] <= 95)
            {
                if (playerPos[1] >= 4 && playerPos[1] <= 9)
                {
                    playerPos[0] = 90;
                    playerPos[1] = 10;
                    ShowStore();
                    break;
                }
                else if (playerPos[1] >= 20 && playerPos[1] <= 25)
                {
                    playerPos[0] = 90;
                    playerPos[1] = 26;
                    ShowDungeon();
                    break;
                }
            }
            else if (playerPos[0] >= 55 && playerPos[0] <= 65)
            {
                if (playerPos[1] >= 12 && playerPos[1] <= 17)
                {
                    playerPos[0] = 60;
                    playerPos[1] = 18;
                    ShowInfo();
                    break;
                }
            }
        }
    }

    public void ShowInfo()
    {
        int c;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("상태 보기");
        Console.ResetColor();
        Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

        Console.WriteLine(character);

        Console.WriteLine("0. 나가기\n");

        while (true)
        {
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            try
            {
                c = int.Parse(Console.ReadLine());
            }
            catch (Exception e) { c = -1; }
            if (c == 0)
            {
                StartScene();
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.\n");
            }
        }
    }

    public void ShowInventory()
    {
        int c;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("인벤토리");
        Console.ResetColor();
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

        Console.WriteLine("[아이템 목록]");
        Console.WriteLine(character.inventory);

        Console.WriteLine("1. 장착 관리");
        Console.WriteLine("0. 나가기\n");

        while (true)
        {
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            try
            {
                c = int.Parse(Console.ReadLine());
            }
            catch (Exception e) { c = -1; }
            if (c == 0)
            {
                StartScene();
                break;
            }
            else if (c == 1)
            {
                character.inventory.manage = true;
                ItemEquip();
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.\n");
            }
        }
    }

    public void ItemEquip()
    {
        int c;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("인벤토리 - 장착 관리");
        Console.ResetColor();
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

        Console.WriteLine("[아이템 목록]");
        Console.WriteLine(character.inventory);

        Console.WriteLine("0. 나가기\n");

        while (true)
        {
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            try
            {
                c = int.Parse(Console.ReadLine());
            }
            catch (Exception e) { c = -1; }
            if (c == 0)
            {
                character.inventory.manage = false;
                ShowInventory();
                break;
            }
            else if (c <= character.inventory.ItemCount() && c > 0)
            {
                Item t = character.inventory.getItem(c - 1);
                if (t.IsEquipped == false)
                {
                    if (t.Type == 'w')
                    {
                        for (int i = 0; i < character.inventory.ItemCount(); i++)
                        {
                            Item temp = character.inventory.getItem(i);
                            if (temp.IsEquipped == true && temp.Type == 'w')
                            {
                                temp.IsEquipped = false;
                                character.Atk2 -= temp.Stat;
                            }
                        }
                        character.Atk2 += t.Stat;
                    }
                    else
                    {
                        for (int i = 0; i < character.inventory.ItemCount(); i++)
                        {
                            Item temp = character.inventory.getItem(i);
                            if (temp.IsEquipped == true && temp.Type == 'a')
                            {
                                temp.IsEquipped = false;
                                character.Def2 -= temp.Stat;
                            }
                        }
                        character.Def2 += t.Stat;
                    }
                    t.IsEquipped = true;
                }
                else
                {
                    t.IsEquipped = false;
                    if (t.Type == 'w')
                    {
                        character.Atk2 -= t.Stat;
                    }
                    else
                    {
                        character.Def2 -= t.Stat;
                    }
                }
                save(character);
                ItemEquip();
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.\n");
            }
        }
    }

    public void ShowStore()
    {
        int c;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("상점");
        Console.ResetColor();
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

        Console.WriteLine("[보유 골드]");
        Console.WriteLine(character.Gold + " G\n");

        Console.WriteLine("[아이템 목록]");
        Console.WriteLine(store);

        Console.WriteLine("1. 아이템 구매");
        Console.WriteLine("2. 아이템 판매");
        Console.WriteLine("0. 나가기\n");

        while (true)
        {
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            try
            {
                c = int.Parse(Console.ReadLine());
            }
            catch (Exception e) { c = -1; }
            if (c == 0)
            {
                StartScene();
                break;
            }
            else if (c == 1)
            {
                store.manage = true;
                ItemBuy();
                break;
            }
            else if (c == 2)
            {
                store.manage = true;
                ItemSell();
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    public void ItemBuy()
    {
        int c;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("상점 - 아이템 구매");
        Console.ResetColor();
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

        Console.WriteLine("[보유 골드]");
        Console.WriteLine(character.Gold + " G\n");

        Console.WriteLine("[아이템 목록]");
        Console.WriteLine(store);

        Console.WriteLine("0. 나가기\n");

        while(true)
        {
            Console.WriteLine("구매하실 아이템을 입력해주세요.");
            try
            {
                c = int.Parse(Console.ReadLine());
            }
            catch (Exception e) { c = -1; }
            if (c == 0)
            {
                store.manage = false;
                ShowStore();
                break;
            }
            else if (c <= store.ItemCount() && c > 0)
            {
                Item t = store.getItem(c - 1);
                if (t.Buy == true)
                {
                    Console.WriteLine("이미 구매한 아이템입니다.\n");
                }
                else if (character.Gold >= t.Price)
                {
                    Console.WriteLine("구매를 완료했습니다.\n");
                    t.Buy = true;
                    character.Gold -= t.Price;
                    character.inventory.addItem(t);
                    save(character);
                    Thread.Sleep(500);
                    ItemBuy();
                    break;
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다.\n");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.\n");
            }
        }
        
    }

    public void ItemSell()
    {
        int c;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("상점 - 아이템 판매");
        Console.ResetColor();
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

        Console.WriteLine("[보유 골드]");
        Console.WriteLine(character.Gold + " G\n");

        Console.WriteLine("[아이템 목록]");
        Console.WriteLine(store);

        Console.WriteLine("0. 나가기\n");

        while(true)
        {
            Console.WriteLine("판매하실 아이템을 입력해주세요.");
            try
            {
                c = int.Parse(Console.ReadLine());
            }
            catch (Exception e) { c = -1; }
            if (c == 0)
            {
                store.manage = false;
                ShowStore();
                break;
            }
            else if (c <= store.ItemCount() && c > 0)
            {
                Item t = store.getItem(c - 1);
                if (t.Buy == false)
                {
                    Console.WriteLine("구매하지 않은 아이템입니다.\n");
                }
                else
                {
                    Console.WriteLine("판매를 완료했습니다.\n");
                    t.Buy = false;
                    character.Gold += (int)(t.Price * 0.85);
                    if (t.IsEquipped == true)
                    {
                        if (t.Type == 'w')
                        {
                            character.Atk2 -= t.Stat;
                        }
                        else
                        {
                            character.Def2 -= t.Stat;
                        }
                        t.IsEquipped = false;
                    }
                    character.inventory.removeItem(t);
                    save(character);
                    Thread.Sleep(500);
                    ItemSell();
                    break;
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.\n");
            }
        }
        
    }

    public void ShowDungeon()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("던전 입장");
        Console.ResetColor();
        Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

        Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
        Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
        Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
        Console.WriteLine("0. 나가기\n");

        while (true)
        {
            int c;
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            try
            {
                c = int.Parse(Console.ReadLine());
            }
            catch (Exception e) { c = -1; }
            if (c == 0)
            {
                StartScene();
                break;
            }
            else if (c < 4 && c > 0)
            {
                EnterDungeon(c);
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    public void EnterDungeon(int c)
    {
        int originHp = character.Health;
        int originGold = character.Gold;
        bool clear = false;
        int rDef;
        Random rand = new Random();

        if (c == 1)
        {
            rDef = 5;
        }
        else if (c == 2)
        {
            rDef = 11;
        }
        else
        {
            rDef = 17;
        }

        if (character.Def + character.Def2 >= rDef)
        {
            clear = true;
        }
        else
        {
            int p = rand.Next(0, 10);
            if (p > 5)
            {
                clear = true;
            }
        }

        if (clear)
        {
            int d = character.Def + character.Def2 - rDef;
            int HpUse = rand.Next(20, 35) - d;
            character.Health -= HpUse;
            int b = rand.Next(character.Atk + character.Atk2, (character.Atk + character.Atk2) * 2 + 1);
            float bonus = (float)b / 100;
            if (c == 1)
            {
                character.Gold += 1000 + (int)(1000 * bonus);
            }
            else if (c == 2)
            {
                character.Gold += 1700 + (int)(1700 * bonus);
            }
            else
            {
                character.Gold += 2500 + (int)(2500 * bonus);
            }
            save(character);
            DungeonClear(c, originHp, originGold);
        }
        else
        {
            int d = character.Def + character.Def2 - rDef;
            int HpUse = rand.Next(20, 35) - d;
            character.Health -= (int)(HpUse * 0.5);
            save(character);
            DungeonFail(c, originHp);
        }
    }

    public void DungeonClear(int c, int originHp, int originGold)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("던전 클리어");
        Console.ResetColor();
        Console.WriteLine("축하합니다!!");

        if (c == 1)
        {
            Console.WriteLine("쉬운 던전을 클리어 하였습니다.\n");
        }
        else if (c == 2)
        {
            Console.WriteLine("일반 던전을 클리어 하였습니다.\n");
        }
        else
        {
            Console.WriteLine("어려운 던전을 클리어 하였습니다.\n");
        }

        Console.WriteLine("[탐험 결과]");
        Console.WriteLine("체력 {0} -> {1}", originHp, character.Health);
        Console.WriteLine("Gold {0} G -> {1} G\n", originGold, character.Gold);
        Console.WriteLine("0. 나가기\n");

        while (true)
        {
            int n;
            Console.WriteLine("원하시는 행동을 입력해주세요");
            try
            {
                n = int.Parse(Console.ReadLine());
            }
            catch (Exception e) { n = -1; }
            if (n == 0)
            {
                if (character.Health <= 0)
                {
                    GameOver();
                    break;
                }
                StartScene();
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

    }

    public void DungeonFail(int c, int originHp)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("던전 실패\n");
        Console.ResetColor();

        Console.WriteLine("[탐험 결과]");
        Console.WriteLine("체력 {0} -> {1}\n", originHp, character.Health);
        Console.WriteLine("0. 나가기\n");

        while (true)
        {
            int n;
            Console.WriteLine("원하시는 행동을 입력해주세요");
            try
            {
                n = int.Parse(Console.ReadLine());
            }
            catch (Exception e) { n = -1; }
            if (n == 0)
            {
                if (character.Health <= 0)
                {
                    GameOver();
                    break;
                }
                StartScene();
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    public void RestScene()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("휴식하기");
        Console.ResetColor();
        Console.WriteLine("500G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {0} G)\n", character.Gold);
        Console.WriteLine("1. 휴식하기");
        Console.WriteLine("0. 나가기\n");

        while (true)
        {
            int c;
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            try
            {
                c = int.Parse(Console.ReadLine());
            }
            catch (Exception e) { c = -1; }
            if (c == 0)
            {
                StartScene();
                break;
            }
            else if (c == 1)
            {
                if(character.Gold >= 500)
                {
                    character.Gold -= 500;
                    character.Health = 100;
                    Console.Clear();
                    Console.ForegroundColor= ConsoleColor.Yellow;
                    Console.WriteLine("휴식 완료!\n");
                    Console.ResetColor();
                    Console.WriteLine("현재 체력 : 100\n");
                    save(character);
                    Thread.Sleep(1000);
                    StartScene();
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    public void GameOver()
    {
        character = new Character();
        store = new Store();
        save(character);

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("게임 오버!\n");
        Console.ResetColor();

        while(true)
        {
            Console.Write("다시 사작하시겠습니까? (Y/N) : ");
            string retry = Console.ReadLine();
            if (retry.ToLower() == "y")
            {
                StartScene();
                break;
            }
            else if (retry.ToLower() == "n")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.\n");
            }
        }
    }

    public void DrawFrame()
    {
        for (int i = 0; i < 120; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("#");
            Console.SetCursorPosition(i, 29);
            Console.Write("#");
        }

        for (int i = 0; i < 30; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("#");
            Console.SetCursorPosition(119, i);
            Console.Write("#");
        }

        DrawSquare(25, 4);
        DrawSquare(85, 4);
        DrawSquare(25, 20);
        DrawSquare(85, 20);
        DrawSquare(55, 12);

        Console.SetCursorPosition(54, 0);
        Console.WriteLine(" 스파르타 마을 ");

        Console.SetCursorPosition(26, 7);
        Console.WriteLine("휴식하기");
        Console.SetCursorPosition(26, 23);
        Console.WriteLine("인벤토리");
        Console.SetCursorPosition(88, 7);
        Console.WriteLine("상점");
        Console.SetCursorPosition(86, 23);
        Console.WriteLine("던전 입장");
        Console.SetCursorPosition(57, 14);
        Console.WriteLine("캐릭터");
        Console.SetCursorPosition(58, 15);
        Console.WriteLine("정보");
    }

    public void DrawSquare(int x, int y)
    {
        for (int i = 0; i < 10; i++)
        {
            Console.SetCursorPosition(i + x, y);
            Console.Write("#");
            Console.SetCursorPosition(i + x, 5 + y);
            Console.Write("#");
        }

        for (int i = 0; i < 5; i++)
        {
            Console.SetCursorPosition(x, i + y);
            Console.Write("#");
            Console.SetCursorPosition(10 + x, i + y);
            Console.Write("#");
        }
        Console.SetCursorPosition(10 + x, 5 + y);
        Console.Write("#");
    }

    public void DrawPlayer(int[] playerPos, int d)
    {
        if (playerPos[1] < 1) playerPos[1] = 1;
        if (playerPos[1] > 28) playerPos[1] = 28;
        if (playerPos[0] < 2) playerPos[0] = 2;
        if (playerPos[0] > 118) playerPos[0] = 118;

        if (d == 0)
        {
            Console.SetCursorPosition(playerPos[0], playerPos[1] + 1);
            Console.Write(" ");
            Console.SetCursorPosition(playerPos[0], playerPos[1]);
            Console.Write("*");
        }
        else if (d == 1)
        {
            Console.SetCursorPosition(playerPos[0], playerPos[1] - 1);
            Console.Write(" ");
            Console.SetCursorPosition(playerPos[0], playerPos[1]);
            Console.Write("*");
        }
        else if (d == 2)
        {
            Console.SetCursorPosition(playerPos[0] + 1, playerPos[1]);
            Console.Write(" ");
            Console.SetCursorPosition(playerPos[0], playerPos[1]);
            Console.Write("*");
        }
        else if (d == 3)
        {
            Console.SetCursorPosition(playerPos[0] - 1, playerPos[1]);
            Console.Write(" ");
            Console.SetCursorPosition(playerPos[0], playerPos[1]);
            Console.Write("*");
        }
    }
}

internal class Program
{
    static Character Load()
    {
        // 파일에서 JSON 읽기
        string filePath = "character.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Character>(json);
        }
        else
        {
            return new Character(); //클래스 생성
        }
    }
    static void Main(string[] args)
    {
        Character savedCharacter = Load();
        Store store = new Store();
        for (int i = 0; i < savedCharacter.inventory.ItemCount(); i++)
        {
            for (int j = 0; j < store.ItemCount(); j++)
            {
                if (savedCharacter.inventory.getItem(0).Name == store.getItem(j).Name)
                {
                    store.getItem(j).Buy = true;
                    store.getItem(j).IsEquipped = savedCharacter.inventory.getItem(0).IsEquipped;
                    savedCharacter.inventory.removeItem(savedCharacter.inventory.getItem(0));
                    savedCharacter.inventory.addItem(store.getItem(j));
                }
            }
        }
        GameStart game = new GameStart(savedCharacter, store);
        game.StartScene();
    }

}