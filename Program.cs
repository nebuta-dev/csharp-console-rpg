using System;
using System.Collections.Generic;
using System.Threading;

namespace Day4;

// Player Class **********************************************************************************
public class Player
{
    public string PlayerName;
    public List<string> inventory;
    public int[] stats;
    private int maxHealth = 100;

    //Constructor for Player class
    public Player(string name)
    {
        PlayerName = name;
        inventory = new List<string>();
        stats = new int[3];
        stats[0] = 80;
        stats[1] = 80;
        stats[2] = 80;
    }

    //Methods for Player class
    public void viewInventory()
    {
        Console.WriteLine("Your inventory:");
        foreach (string item in inventory)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"    {item}");
            Console.ResetColor();
        }
    }
    public void viewStats()
    {
        Console.WriteLine();
        Program.StatBar("Health", stats[0], 100, ConsoleColor.Red);
        Program.StatBar("Mana", stats[1], 100, ConsoleColor.Cyan);
        Program.StatBar("Strength", stats[2], 100, ConsoleColor.Yellow);
        Console.WriteLine();
    }
    public void useItem(string potion)
    {
        try
        {
            int check = inventory.IndexOf(potion);
            if (check == -1)
            {
                Console.WriteLine($"You do not hold any {potion}");
                return;
            }
            if (potion == "Health Potion")
            {
                stats[0] += 25;
                if (stats[0] > maxHealth)
                {
                    stats[0] = maxHealth;
                }
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You used your {potion}, your health was increased by 25 points to a maximum of 100.");
                Console.ResetColor();
            }

            else if (potion == "Mana Potion")
            {
                stats[1] += 25;
                if (stats[1] > maxHealth)
                {
                    stats[1] = maxHealth;
                }
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You used your {potion}, your Mana was increased by 25 points to a maximum of 100.");
                Console.ResetColor();
            }

            else
            {
                Console.WriteLine("This is not a valid item.");
                return;
            }

            inventory.RemoveAt(check);
            viewStats();
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("This is not a valid item.");
        }
    }
    // use item as a demo of overloading
    public void useItem()
    {
        Console.WriteLine("You did not specify which potion you wanted...");
    }

    public bool isAlive()
    {
        return stats[0] > 0;
    }
    public void takeDamage(int damage)
    {
        stats[0] -= damage;
        if (stats[0] < 0)
        {
            stats[0] = 0;
        }
    }
    public int getHealth()
    {
        return stats[0];
    }
    public int getMana()
    {
        return stats[1];
    }
    public int getStrength()
    {
        return stats[2];
    }
    
}


// End of Player Class *****************************************************************************

// Creature Class **********************************************************************************

public abstract class Creature
{
    public string CreatureName;
    public int Health;
    protected int AttackPower;

    //Constructor for creature
    public Creature(string creatureName, int health, int attackPower)
    {
        CreatureName = creatureName;
        Health = health;
        AttackPower = attackPower;
    }

    //Methods for Player class
    public bool isAlive()
    {
        return Health > 0;
    }
    public void takeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }
    }
    public abstract void CreatureAttack(Player player);
}

// End of Creature Class ***************************************************************************

// Creature Child Classes **************************************************************************

public class Goblin : Creature
{
    public Goblin(string creatureName, int health, int attackPower) : base(creatureName, health, attackPower)
    {

    }

    public void showArt()
    {
        Console.WriteLine(@"
        ,      ,
        /(.-""-.)\
    |\  \/      \/  /|
    | \ / =.  .= \ / |
    \( \   o\/o   / )/
    \_, '-/  \-' ,_/
        /   \__/   \
        \ \__/\__/ /
    ___\ \|--|/ /___
    /`    \      /    `\
    ");
    }

    public override void CreatureAttack(Player player)
    {
        Console.WriteLine($"{CreatureName} attacks you with his dagger and deals {AttackPower} of damage.");
        player.takeDamage(AttackPower);
    }
}

public class Troll : Creature
{
    public Troll(string creatureName, int health, int attackPower) : base(creatureName, health, attackPower)
    {

    }
    public override void CreatureAttack(Player player)
    {
        Console.WriteLine($"{CreatureName} attacks you with his axe and deals {AttackPower} of damage.");
        player.takeDamage(AttackPower);
    }
}

public class Orc : Creature
{
    public Orc(string creatureName, int health, int attackPower) : base(creatureName, health, attackPower)
    {

    }

    public override void CreatureAttack(Player player)
    {
        Console.WriteLine($"The {CreatureName} attacks you with his battle axe and deals {AttackPower} of damage.");
        player.takeDamage(AttackPower);
    }
}

// End of Creature Child Classes *******************************************************************

class Program
{
    static Player player = null!;

    // Main calling for methods
    static void Main(string[] args)
    {
        player = intro();

        rushOut();

        Goblin foe1 = new Goblin("Forest Goblin", 30, 10);
        fight(foe1);
    }

    static void keyBreak()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadLine();
    }

    static Player intro()
    {
        GameTitle();
        Console.WriteLine("You wake up before the sun as you usually do, but this time feels different, you do not recognise your bedroom.");
        Console.WriteLine("*Knock knock... Knock knock* Someone is banging at the door.");
        Console.WriteLine("...: 'Who's there?'");
        Console.WriteLine("Stable boy: 'It's the stable boy! Your squire has just been killed by a Goblin.'");
        Console.WriteLine("You open the door in a hurry.");
        Console.WriteLine("Stable boy: 'What is your name my lord?'");

        string? name = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Please enter a name (cannot be empty):");
            name = Console.ReadLine();
        }

        Player p = new Player(name);
        Console.WriteLine($"'Let's hurry, I have prepared your horse my Lord {p.PlayerName}'");
        return p;
    }

    static void rushOut()
    {
        Console.WriteLine("You take your sword, a health potion, your shield and almost forgot your coin pouch.");
        player.inventory.Add("Basic Sword");
        player.inventory.Add("White lion shield");
        player.inventory.Add("Coin Pouch");
        player.inventory.Add("Health Potion");
        player.inventory.Add("Mana Potion");
        player.viewInventory();

        Console.WriteLine("Before rushing out, you look at yourself in the mirror");
        player.viewStats();
        Console.WriteLine("'Shame I drank so much at last night's celebration, I am not feeling at my best. I hope I won't have to fight'");
        Console.WriteLine("While you run down the corridor, you can access the Menu. Do you wish to? Y/N");
        accessMenu();
    }

    static void accessMenu()
    {
        string answer = (Console.ReadLine() ?? "").ToLowerInvariant();

        if (answer == "n")
        {
            return;
        }
        else if (answer != "y")
        {
            Console.WriteLine("This is an invalid entry");
            return;
        }

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("***   MAIN MENU   ***");
            Console.WriteLine("Choose between options 1 to 6:");
            Console.WriteLine("1/ View Inventory");
            Console.WriteLine("2/ View Stats");
            Console.WriteLine("3/ Use a Health Potion");
            Console.WriteLine("4/ Use a Mana Potion");
            Console.WriteLine("5/ Exit menu");
            Console.WriteLine("6/ Exit Game");
            Console.ResetColor();

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    player.viewInventory();
                    break;
                case "2":
                    player.viewStats();
                    break;
                case "3":
                    player.useItem("Health Potion");
                    break;
                case "4":
                    player.useItem("Mana Potion");
                    break;
                case "5":
                    Console.WriteLine("Closing menu...");
                    return;
                case "6":
                    Console.WriteLine("Thank you for playing THE LEGEND OF THE WHITE DRAGON.");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid entry, please choose between 1 and 6.");
                    continue;
            }
        }
    }

    static void slowPrint(string text, int delay = 30)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
    }

    static void fight(Creature foe)
    {
        slowPrint($"A {foe.CreatureName} jumps in front of you!");
        if (foe is Goblin goblin)
        {
            goblin.showArt();
        }

        while (foe.isAlive())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("***   FIGHT MENU   ***");
            Console.WriteLine("Choose between options 1 to 4:");
            Console.WriteLine("1/ Attack");
            Console.WriteLine("2/ Use Health Potion");
            Console.WriteLine("3/ Use Mana Potion");
            Console.WriteLine("4/ Run Away like a chicken");
            Console.ResetColor();

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    int heroAttack = 15;
                    foe.takeDamage(heroAttack);

                    if (!foe.isAlive())
                    {
                        Console.WriteLine($"You have killed the {foe.CreatureName}.");
                        Console.WriteLine("You managed to evade the rest of enemies with the stable boy, you are now going to enter the forest of the thousand nightmares.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"The enemy has now {foe.Health} HP. He prepares for an attack!");
                        foe.CreatureAttack(player);

                        if (!player.isAlive())
                        {
                            Console.WriteLine("You have died bravely, but in your next life, try not to drink so much!");
                            Environment.Exit(0);
                        }
                        else
                        {
                            player.viewStats();
                        }
                    }
                    break;

                case "2":
                    player.useItem("Health Potion");
                    break;

                case "3":
                    player.useItem("Mana Potion");
                    break;

                case "4":
                    Console.WriteLine("You run like a chicken! Everyone laughs at you and the stable boy dies...");
                    Console.WriteLine("It was your secret illegitimate son, no one trusts you anymore, you lose the game.");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid entry, please choose between 1 and 3.");
                    continue;
            }
        }
    }
    public static void StatBar(string label, int value, int max, ConsoleColor color)
    {
        int barSize = 20;
        int filled = (value * barSize) / max;

        Console.Write($"{label.PadRight(10)} ");

        Console.ForegroundColor = color;
        Console.Write(new string('█', filled));

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(new string('░', barSize - filled));

        Console.ResetColor();
        Console.WriteLine($" {value}/{max}");
    }
    static void GameTitle()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;

        string[] title =
        {
            "THE LEGEND OF THE WHITE DRAGON"
        };

        int width = title[0].Length + 10;

        Console.WriteLine("╔" + new string('═', width - 2) + "╗");
        Console.WriteLine("║" + new string(' ', width - 2) + "║");

        foreach (string line in title)
        {
            int padding = (width - 2 - line.Length) / 2;
            Console.WriteLine("║" +
                new string(' ', padding) +
                line +
                new string(' ', width - 2 - line.Length - padding) +
                "║");
        }

        Console.WriteLine("║" + new string(' ', width - 2) + "║");
        Console.WriteLine("╚" + new string('═', width - 2) + "╝");

        Console.ResetColor();
        Console.WriteLine();
    }
}
