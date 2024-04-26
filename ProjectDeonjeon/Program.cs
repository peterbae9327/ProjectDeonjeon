using System.Xml.Linq;

namespace ProjectDeonjeon
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            //선택
            int choice;
            //상태
            int Lv = 1;
            string Name = "Chad";
            string Occupation = "전사";
            float Attack = 10;
            int Defense = 5;
            int HP = 100;
            int Gold = 1500;
            int PlusAttack = 0;
            int PlusDefense = 0;
            GameManager game = new GameManager();
            Reward reward = new Reward();
            DeonJeonPlay deonjeonplay = new DeonJeonPlay();
            INN inn = new INN();
            int Cleartimes = 0;//던전 클리어 횟수

            //아이템 정보(품목 추가시 출력수도 늘리기)
            //1.수련자 갑옷
            Item item1 = new Item();
            item1.Name = "수련자 갑옷";
            item1.AtkDef = "방어";
            item1.Stat = 5;
            item1.Howmuch = 1000;
            item1.Explanation = "수련에 도움을 주는 갑옷입니다.";
            //2.무쇠갑옷
            Item item2 = new Item();
            item2.Name = "무쇠갑옷";
            item2.AtkDef = "방어";
            item2.Stat = 9;
            item2.Howmuch = 2000;
            item2.Explanation = "무쇠로 만들어져 튼튼한 갑옷입니다.";
            //3.스파르타의 갑옷
            Item item3 = new Item();
            item3.Name = "스파르타의 갑옷";
            item3.AtkDef = "방어";
            item3.Stat = 15;
            item3.Howmuch = 3500;
            item3.Explanation = "스파르타 전사들이 사용했다는 전설의 갑옷입니다.";
            //4.낡은 검
            Item item4 = new Item();
            item4.Name = "낡은 검";
            item4.AtkDef = "공격";
            item4.Stat = 2;
            item4.Howmuch = 600;
            item4.Explanation = "쉽게 볼 수 있는 낡은 검 입니다.";
            //5.청동 도끼
            Item item5 = new Item();
            item5.Name = "청동 도끼";
            item5.AtkDef = "공격";
            item5.Stat = 5;
            item5.Howmuch = 1500;
            item5.Explanation = "어디선가 사용됐던것 같은 도끼입니다.";
            //6.스파르타의 창
            Item item6 = new Item();
            item6.Name = "스파르타의 창";
            item6.AtkDef = "공격";
            item6.Stat = 7;
            item6.Howmuch = 2000;
            item6.Explanation = "스파르타의 전사들이 사용했다는 전설의 창입니다.";
            //7.주먹도끼
            Item item7 = new Item();
            item7.Name = "주먹도끼";
            item7.AtkDef = "공격";
            item7.Stat = 1;
            item7.Howmuch = 400;
            item7.Explanation = "유구한 전통을 가진 석기 시대의 도구입니다.";
            //8.왕의 갑옷
            Item item8 = new Item();
            item8.Name = "왕의 갑옷";
            item8.AtkDef = "방어";
            item8.Stat = 10;
            item8.Howmuch = 4500;
            item8.Explanation = "성능보다는 디자인에 집중한 금도장의 화려한 갑옷입니다.";
            Item[] items = { item1, item2, item3, item4, item5, item6, item7, item8 };//배열지정


            //간단한 소개, 마을에서 할 행동
            while (true)
            {
                choice = Village();
                game.choose = choice;
                while (game.choose < 1 || game.choose > 5)
                    {
                        game.choose = WrongChoice();
                    }
                if (game.choose == 1)//상태보기
                {
                    Status(Lv, Name, Occupation, Attack, Defense, HP, Gold, PlusAttack, PlusDefense);
                    Clear();
                }
                if (game.choose == 2)//인벤토리
                {
                    while (true)
                    {
                        game = ItemList("listing", Gold, items);
                        if (game.choose == 0)//돌아가기
                        {
                            break;
                        }
                        else if (game.choose == 1)//장비착용
                        {
                            game = ItemList("equiping", Gold, items);
                            PlusAttack += game.updateattack;
                            PlusDefense += game.updatedefense;
                            Attack += game.updateattack;
                            Defense += game.updatedefense;
                        }
                    }
                    Clear();
                }
                if(game.choose == 3)//상점
                {
                    while (true)
                    {
                        game = ItemList("stacking", Gold, items);
                        if (game.choose == 0)//돌아가기
                        {
                            break;
                        }
                        else if (game.choose == 1)//아이템구매
                        {
                            game = ItemList("buying", Gold, items);
                            Gold = game.updategold;
                        }
                        else if (game.choose == 2)//아이템 판매
                        {
                            game = ItemList("selling", Gold, items);
                            Gold = game.updategold;
                            PlusAttack += game.updateattack;
                            PlusDefense += game.updatedefense;
                            Attack += game.updateattack;
                            Defense += game.updatedefense;
                        }
                    }
                    Clear();
                }

                if(game.choose == 4)//던전
                {
                    while (true)
                    {
                        deonjeonplay = EnterDeonJeon(Defense);
                        if(deonjeonplay.choice == 0)//돌아가기
                        {
                            Clear();
                            break;
                        }
                        else if(deonjeonplay.choice >= 1 && deonjeonplay.choice <= 3)//성공(1~3)
                        {
                            reward = DeonJeonResult("성공", deonjeonplay.choice, Attack, Defense, HP, Gold);
                            HP -= reward.LooseHP;
                            Gold += reward.GainGold;
                            Cleartimes++;
                            if(Cleartimes == Lv)
                            {
                                Lv++;
                                Attack += 0.5f;
                                Defense += 1;
                                Cleartimes = 0;
                            }
                        }
                        else if( deonjeonplay.choice >= 4 && deonjeonplay.choice <= 5)//실패(4~5)
                        {
                            reward = DeonJeonResult("실패", deonjeonplay.choice, Attack, Defense, HP, Gold);
                            HP -= reward.LooseHP;
                        }

                    }
                }
                if(game.choose == 5)//휴식
                {
                    while (true)
                    {
                        inn = Rest(Gold);
                        if (inn.rested)//휴식완료
                        {
                            HP = 100;
                            Gold -= 500;
                        }
                        if(inn.choice == 0)
                        {
                            Clear();
                            break;
                        }
                    }
                }
            }
        }
        static public int Village()//마을
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");
            return PleaseChoice();
        }
        static public void Clear()//콘솔창 지우기
        {
            Console.Clear();
        }
        static public int WrongChoice()//잘못된 입력
        {
            Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
            Console.Write(">>");
            var choice = Console.ReadLine();
            if (int.TryParse(choice, out int i) == false)
            {
                choice = "-1";
            }
            return int.Parse(choice);
        }
        static public int PleaseChoice()//입력 요구
        {
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            var choice = Console.ReadLine();
            if (int.TryParse(choice, out int i) == false)
            {
                choice = "-1";
            }
            return int.Parse(choice);
        }
        static public void Status(int lv, string name, string occupation, float attack, int defense, int hp, int gold, int plusattack, int plusdefense)
        {//상태보기
            Clear();
            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv. {lv}");//레벨
            Console.WriteLine($"{name} ({occupation})");// 이름 직업
            Console.Write($"공격력 : {attack}");//공격력
            if(plusattack > 0)//추가된 공격력이 있다면 대괄호 안에 표현
            {
                Console.WriteLine($"[+{plusattack}]");
            }
            else
            {
                Console.WriteLine();
            }
            Console.Write($"방어력 : {defense}");//방어력
            if (plusdefense > 0)//추가된 방어력이 있다면 대괄호 안에 표현
            {
                Console.WriteLine($"[+{plusdefense}]");
            }
            else
            {
                Console.WriteLine();
            }
            Console.WriteLine($"체  력 : {hp}");//체력
            Console.WriteLine($"GOLD : {gold}");//골드
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            int choice = PleaseChoice();
            while (choice != 0)
            {
                choice =WrongChoice();
            }
        }
        static public GameManager ItemList(string ACT, int gold, Item[] items)
        {
            GameManager manager = new GameManager();
            manager.choose = 0;
            manager.updategold = 0;
            manager.updateattack = 0;
            manager.updatedefense = 0;
            for (int i = 0; i < items.Length; i++)//아이템별 부여번호 초기화
            {
                items[i].Number = 0;
            }
            int j = 0;//번호매기기
            int choice;//선택
            Clear();
            if (ACT == "listing")//보유아이템
            {
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i =0; i < items.Length; i++)
                {
                    if (items[i].Gained)//가지고 있다면 표시
                    {
                        Console.Write("- ");
                        if (items[i].Equipted)//장비했다면 [E] 표시
                        {
                            Console.Write("[E] ");
                        }
                        Console.WriteLine($"{items[i].Name}\t| {items[i].AtkDef}력 +{items[i].Stat}\t| {items[i].Explanation}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기");
                choice = PleaseChoice();
                while (choice != 0 && choice != 1)
                {
                    choice = WrongChoice();
                }
                manager.choose = choice;
                return manager;
            }
            if (ACT == "equiping")//장비착용
            {
                do
                {
                    Clear();
                    Console.WriteLine("인벤토리 - 장착관리");
                    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                    Console.WriteLine();
                    Console.WriteLine("[아이템 목록]");
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].Gained)//가지고 있다면 표시
                        {
                            Console.Write("- ");
                            j++;
                            items[i].Number = j;//품목별 번호 부여
                            Console.Write(j);
                            if (items[i].Equipted)// 장비했다면 [E} 표시
                            {
                                Console.Write(" [E]");
                            }
                            Console.WriteLine($" {items[i].Name}\t| {items[i].AtkDef}력 +{items[i].Stat}\t| {items[i].Explanation}");
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("0. 나가기");
                    choice = PleaseChoice();
                    if (j != 0 && choice >= 1 && choice <= j)//목록 내의 번호 선택시 실행
                    {
                        for(int i = 0; i< items.Length; i++)//전체 아이템 스캔
                        {
                            if (items[i].Number == choice)//선택번호 부여번호 일치
                            {
                                if (!items[i].Equipted)//장비 착용 안했다면 입히기
                                {
                                    items[i].Equipted = true;
                                    for (int k = 0; k < items.Length; k++)//전체 아이템 스캔
                                    {
                                        
                                        if (k != i && items[k].AtkDef == items[i].AtkDef && items[k].Equipted)
                                        {
                                            items[k].Equipted = false;//같은 종류일 경우 기존의 것 벗기기
                                            if (items[k].AtkDef == "공격")
                                            {
                                                manager.updateattack = -items[k].Stat;
                                            }
                                            else
                                            {
                                                manager.updatedefense = -items[k].Stat;
                                            }
                                        }
                                    }
                                    if (items[i].AtkDef == "공격")
                                    {
                                        manager.updateattack = items[i].Stat;
                                    }
                                    else
                                    {
                                        manager.updatedefense = items[i].Stat;
                                    }

                                }
                                else//아니라면 벗기기
                                {
                                    items[i].Equipted = false;
                                    if (items[i].AtkDef == "공격")
                                    {
                                        manager.updateattack = -items[i].Stat;
                                    }
                                    else
                                    {
                                        manager.updatedefense = -items[i].Stat;
                                    }
                                }

                            }
                            j = 0;
                        }
                    }
                    else if (choice != 0 && choice <= 1 && choice >= j)//목록내 번호 선택 안할경우
                    {
                        do
                        {
                            choice = WrongChoice();
                        }
                        while (choice != 0 && choice <= 1 || choice >= j);
                    }
                } while (choice != 0);
                manager.choose = choice;
                return manager;
                
            }
            if(ACT == "stacking")//상품진열
            {
                Clear();
                Console.WriteLine("상점");
                Console.WriteLine();
                Console.WriteLine("-주인장-");
                Console.WriteLine("\"볼만한 물건이 있는지 한번 살펴보게나~!\"");
                Console.WriteLine();
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유골드]");
                Console.WriteLine($"{gold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < items.Length; i++)
                {
                    Console.Write("- ");
                    if (items[i].Equipted)//장비 착용시 [E]
                    {
                        Console.Write("[E] ");
                    }
                    Console.Write($"{items[i].Name}\t| {items[i].AtkDef}력 +{items[i].Stat}\t| {items[i].Explanation}");
                    if (items[i].Gained)// 보유하고있을경우 구매완료표시
                    {
                        Console.WriteLine("\t| 구매완료");
                    }
                    else//아니면 가격 표시
                    {
                        Console.WriteLine($"\t| {items[i].Howmuch} G");
                    }

                }
                Console.WriteLine();
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                choice = PleaseChoice();
                while (choice != 0 && choice != 1 && choice != 2)
                {
                    choice = WrongChoice();
                }
                manager.choose = choice;
                return manager;
            }
            if (ACT == "buying")//구매
            {
                do
                {
                    Clear();
                    Console.WriteLine("상점 - 아이템 구매");
                    Console.WriteLine();
                    Console.WriteLine("-주인장-");
                    Console.WriteLine("\"어떤 물건을 가지고 싶은가?\"");
                    Console.WriteLine();
                    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                    Console.WriteLine();
                    Console.WriteLine("[보유골드]");
                    Console.WriteLine($"{gold} G");
                    Console.WriteLine();
                    Console.WriteLine("[아이템 목록]");
                    for (int i = 0; i < items.Length; i++)
                    {
                        Console.Write("- ");
                        j++;
                        items[i].Number = j;//품목별 번호 부여
                        Console.Write(j);
                        if (items[i].Equipted)
                        {
                            Console.Write(" [E]");
                        }
                        Console.Write($" {items[i].Name}\t| {items[i].AtkDef}력 +{items[i].Stat}\t| {items[i].Explanation}");
                        if (items[i].Gained)
                        {
                            Console.WriteLine("\t| 구매완료");
                        }
                        else
                        {
                            Console.WriteLine($"\t| {items[i].Howmuch} G");
                        }

                    }
                    Console.WriteLine();
                    Console.WriteLine("0. 나가기");
                    choice = PleaseChoice();
                    if(j != 0 && choice >= 1 && choice <= j)//목록내 번호 선택시
                    {
                        for (int i = 0; i < items.Length; i++)//전체 품목 탐색
                        {
                            if (items[i].Number == choice)// 부여번호 같을경우
                            {
                                if (items[i].Gained)//얻은 상태라면
                                {
                                    Console.WriteLine("이미 구매한 아이템입니다!");
                                }
                                else if (items[i].Howmuch <= gold)//골드가 가격보다 크거나 같다면
                                {
                                    items[i].Gained = true;
                                    gold -= items[i].Howmuch;
                                }
                                else//골드가 가격보다 작다면
                                {
                                    Console.WriteLine("Gold가 부족합니다!");
                                }
                            }
                            j = 0;
                        }
                    }
                    else if (choice != 0 && choice <= 1 && choice >= j)//목록내 번호 선택 안했을 경우
                    {
                        do
                        {
                            choice = WrongChoice();
                        }
                        while (choice != 0 && choice <= 1 || choice >= j);
                    }
                } while (choice != 0);
                manager.updategold = gold;
                manager.choose = choice;
                return manager;
            }
            if (ACT == "selling")//판매
            {
                do
                {
                    Clear();
                    Console.WriteLine("상점 - 아이템 판매");
                    Console.WriteLine();
                    Console.WriteLine("-주인장-");
                    Console.WriteLine("\"중고물품이니까 85%정도는 쳐주겠네.\"");
                    Console.WriteLine();
                    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                    Console.WriteLine();
                    Console.WriteLine("[보유골드]");
                    Console.WriteLine($"{gold} G");
                    Console.WriteLine();
                    Console.WriteLine("[아이템 목록]");
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].Gained)//보유시 표시
                        {
                            Console.Write("- ");
                            j++;
                            items[i].Number = j;//품목별 번호 부여
                            Console.Write(j);
                            if (items[i].Equipted)//착용시 [E] 표시
                            {
                                Console.Write(" [E]");
                            }
                            Console.WriteLine($" {items[i].Name}\t| {items[i].AtkDef}력 +{items[i].Stat}\t| {items[i].Explanation}\t| {items[i].Howmuch * 85/100} G");//금액 85%
                        }
                    }//////////////////////수정 필요
                    Console.WriteLine();
                    Console.WriteLine("0. 나가기");
                    choice = PleaseChoice();
                    if (j != 0 && choice >= 1 && choice <= j)//목록내 번호 선택시
                    {
                        for (int i = 0; i < items.Length; i++)// 품목별 번호 대조
                        {
                            if (items[i].Number == choice)//번호 같다면
                            {
                                if (items[i].Equipted)//착용했다면
                                {
                                    if (items[i].AtkDef == "공격")//같은 종류의 장비라면
                                    {
                                        manager.updateattack = -items[i].Stat;
                                    }
                                    else
                                    {
                                        manager.updatedefense = -items[i].Stat;
                                    }
                                }
                                items[i].Equipted = false;
                                items[i].Gained = false;
                                gold += items[i].Howmuch * 85/100;//85%가격 수령
                            }
                            j = 0;
                        }///////////////////수정필요
                    }
                    else if (choice != 0 && choice <= 1 && choice >= j)//목록외 번호 선택시
                    {
                        do
                        {
                            choice = WrongChoice();
                        }
                        while (choice != 0 && choice <= 1 || choice >= j);
                    }
                } while(choice != 0);
                manager.updategold = gold;
                manager.choose = choice;
                return manager;
            }
            else
            {
                return manager;
            }
            
        }
        static public DeonJeonPlay EnterDeonJeon(int defense)//던전입장
        {
            int required_defense = 0;
            DeonJeonPlay deonjeonmaster = new DeonJeonPlay();
            Clear();
            Console.WriteLine("던전입장");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 쉬운 던전\t| 방어력 5이상 권장");
            Console.WriteLine("2. 일반 던전\t| 방어력 11이상 권장");
            Console.WriteLine("3. 어려운 던전\t| 방어력 17이상 권장");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            deonjeonmaster.choice = PleaseChoice();
            do
            {
                if (deonjeonmaster.choice == 1)//쉬운던전
                {
                    required_defense = 5;
                    break;
                }
                else if (deonjeonmaster.choice == 2)//일반던전
                {
                    required_defense = 11;
                    break;
                }
                else if (deonjeonmaster.choice == 3)//어려운던전
                {
                    required_defense = 17;
                    break;
                }
                else if (deonjeonmaster.choice == 0)//나가기
                {
                    break;
                }
                else//그외 번호
                {
                    deonjeonmaster.choice =WrongChoice();
                }
            } while (true);
            if (deonjeonmaster.choice == 1 ||  deonjeonmaster.choice == 2 || deonjeonmaster.choice == 3)//던전을 선택했다면
            {
                if (defense >= required_defense)//권장방어력이 높다면
                {
                    deonjeonmaster.issuccess = 1;//무조건 클리어
                }
                else
                {
                    Random random = new Random();//랜덤
                    deonjeonmaster.issuccess = random.Next(5) + 1;
                }
            }return deonjeonmaster;
        }
        static public Reward DeonJeonResult(string result, int playmode, float attack, int defense, int hp, int gold)
        {
            Reward reward = new Reward();
            string mode = null;
            int required_defense = 0;
            int minus_HP = 0;
            int plus_GOLD = 0;
            int reward_gold = 0;
            Loading_Bar("던전 깨는");//로딩바
            Clear();
            if(playmode == 1)//던전난이도 따라 수치 부여
            {
                mode = "쉬운";
                required_defense = 5;
                reward_gold = 1000;
            }
            else if(playmode == 2)
            {
                mode = "일반";
                required_defense = 11;
                reward_gold = 1700;
            }
            else if(playmode == 3)
            {
                mode = "어려운";
                required_defense = 17;
                reward_gold = 2500;
            }
            if(result == "실패")
            {
                reward.LooseHP = hp / 2;//50% 깎기
                Console.WriteLine("실패...");
                Console.WriteLine($"{mode} 던전을 클리어하지 못했습니다...");
                Console.WriteLine();
                Console.WriteLine("[탐험결과]");
                Console.WriteLine($"체력 {hp} -> {hp - reward.LooseHP}");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                
            }
            if(result == "성공")
            {
                minus_HP = required_defense - defense;
                Random HP = new Random();
                reward.LooseHP = HP.Next(16) + 20 + minus_HP;//랜덤부여
                Random GOLD = new Random();
                reward.GainGold = reward_gold * (int)(((GOLD.Next(1) + 1) * attack) + 100) / 100;//추가보상
                Console.WriteLine("던전 클리어");
                Console.WriteLine("축하합니다!!");
                Console.WriteLine($"{mode} 던전을 클리어 하였습니다.");
                Console.WriteLine();
                Console.WriteLine("[탐험결과]");
                Console.WriteLine($"체력 {hp} -> {hp-reward.LooseHP}");
                Console.WriteLine($"Gold {gold} G -> {gold+reward.GainGold} G");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

            }
            int choice = PleaseChoice();
            while (choice != 0)
            {
                choice = WrongChoice();
            }
            return reward;
        }
        static public INN Rest(int gold)
        {
            INN inn = new INN();
            Clear();
            Console.WriteLine("여관");
            Console.WriteLine();
            Console.WriteLine("-알바생-");
            Console.WriteLine("\"ㅔㅔㅔ 500골드 임다~\"");
            Console.WriteLine();
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {gold} G)");
            Console.WriteLine();
            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");
            do
            {
                int choice = PleaseChoice();
                while (choice != 0 && choice != 1)
                {
                    choice = WrongChoice();
                }
                if (choice == 0)
                {
                    break;
                }
                else if (choice == 1)
                {
                    if (gold >= 500)//500골드 이상이라면
                    {
                        Loading_Bar("휴식");//로딩바
                        gold -= 500;
                        inn.rested = true;
                        Console.WriteLine("휴식을 완료했습니다.");
                        Thread.Sleep(1000);//1초 대기
                        choice = 0;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                    }
                }
            } while (true);
            return inn;
        }
        static public void Loading_Bar(string action)//로딩바
        {
            Clear();
            Console.WriteLine();
            Console.WriteLine($"{action}중...");
            Console.WriteLine();
            for(int i = 0; i<20; i++)
            {
                Console.Write("■");
                Thread.Sleep(250);
            }
            Clear();
        }
        public class INN
        {
            public bool rested;
            public int choice;

        }
        public class Item
        {
            public string Name;
            public string AtkDef;
            public int Stat;
            public int Howmuch;
            public string Explanation;
            public bool Gained;
            public bool Equipted;
            public int Number = 0;
        }
        public class GameManager
        {
            public int choose;
            public int updategold;
            public int updateattack;
            public int updatedefense;
        }
        public class Reward
        {
            public int GainGold;
            public int LooseHP;
        }
        public class DeonJeonPlay
        {
            public int choice;
            public int issuccess;
        }

    }
}
