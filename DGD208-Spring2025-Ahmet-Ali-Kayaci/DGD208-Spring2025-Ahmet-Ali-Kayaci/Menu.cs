using System;
using System.Collections.Generic;

public class Menu
{
    public int DisplayMainMenu()
    {
        Console.Clear();
        Console.WriteLine("Main Menu:");
        Console.WriteLine("1. Adopt a Pet");
        Console.WriteLine("2. Interact with a Pet");
        Console.WriteLine("3. View All Pets");
        Console.WriteLine("4. Exit");
        Console.WriteLine("5. About Creator");
        return int.Parse(Console.ReadLine());
    }

    public int DisplayPetTypeMenu()
    {
        Console.Clear();
        Console.WriteLine("Choose a pet type to adopt:");
        Console.WriteLine("1. Cat");
        Console.WriteLine("2. Dog");
        Console.WriteLine("3. Rabbit");
        Console.WriteLine("4. Parrot");
        return int.Parse(Console.ReadLine());
    }

    public string GetPetName()
    {
        Console.WriteLine("Enter a name for your pet:");
        return Console.ReadLine();
    }

    public int DisplayPetSelectionMenu(List<Pet> pets)
    {
        Console.Clear();
        for (int i = 0; i < pets.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {pets[i].name}");
        }
        return int.Parse(Console.ReadLine()) - 1;
    }

    public int DisplayInteractionMenu()
    {
        Console.Clear();
        Console.WriteLine("Choose an interaction:");
        Console.WriteLine("1. Feed");
        Console.WriteLine("2. Put to Sleep");
        Console.WriteLine("3. Play");
        return int.Parse(Console.ReadLine());
    }

    public int DisplayFoodMenu()
    {
        Console.Clear();
        Console.WriteLine("Choose food:");
        Console.WriteLine("1. Cat Food");
        Console.WriteLine("2. Bird Seed");
        return int.Parse(Console.ReadLine());
    }

    public string GetFoodName(int choice)
    {
        return choice switch
        {
            1 => "Cat Food",
            2 => "Bird Seed",
            _ => "tam netlesmedi"
        };
    }
}
