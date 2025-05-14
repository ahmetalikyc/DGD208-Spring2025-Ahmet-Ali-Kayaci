using System;
using System.Collections.Generic;

public class PetManager
{
    private List<Pet> pets;

    public PetManager()
    {
        pets = new List<Pet>();
    }

    public void AddPet(Pet pet)
    {
        pets.Add(pet);
    }

    public bool HasPets()
    {
        return pets.Count > 0;
    }

    public List<Pet> GetPets()
    {
        return pets;
    }

    public Pet GetPet(int index)
    {
        return pets[index];
    }
}
