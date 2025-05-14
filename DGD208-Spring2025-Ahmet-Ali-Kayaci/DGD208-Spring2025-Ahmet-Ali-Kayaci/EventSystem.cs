using System;

public class EventSystem
{
    public event Action<Pet> OnPetDied; 

    public void PetDied(Pet pet)
    {
        OnPetDied?.Invoke(pet);
    }

    public event Action<Pet> OnPetStatChanged; 

    public void PetStatChanged(Pet pet)
    {
        OnPetStatChanged?.Invoke(pet);
    }
}
