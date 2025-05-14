public class Game
{
    private Menu _menu;
    private Pet _pet;
    private ItemDatabase _itemDatabase;

    public Game()
    {
        _menu = new Menu();
        _itemDatabase = new ItemDatabase();
    }

    public void StartGame()
    {
        bool gameRunning = true;
        while (gameRunning)
        {
            _menu.ShowMainMenu();
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    _menu.ShowAdoptMenu();
                    int petChoice = int.Parse(Console.ReadLine());
                    AdoptPet(petChoice);
                    break;
                case 2:
                    ShowPetStats();
                    break;
                case 3:
                    gameRunning = false;
                    break;
            }
        }
    }

    private void AdoptPet(int petChoice)
    {
        switch (petChoice)
        {
            case 1:
                _pet = new Pet("Cat", PetType.Cat);
                break;
            case 2:
                _pet = new Pet("Dog", PetType.Dog);
                break;
            case 3:
                _pet = new Pet("Rabbit", PetType.Rabbit);
                break;
            case 4:
                _pet = new Pet("Parrot", PetType.Parrot);
                break;
        }
        Console.WriteLine($"{_pet.Name} has been adopted!");
    }

    private void ShowPetStats()
    {
        Console.WriteLine($"Pet Name: {_pet.Name}");
        Console.WriteLine($"Health: {_pet.Stats[PetStat.Health]}");
        Console.WriteLine($"Fun: {_pet.Stats[PetStat.Fun]}");
        Console.WriteLine($"Fullness: {_pet.Stats[PetStat.Fullness]}");
        Console.WriteLine($"Sleep: {_pet.Stats[PetStat.Sleep]}");
    }
}
