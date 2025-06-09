using System;

namespace PetSimulator
{
    public static class EventSystem
    {
        #region Pet Events
        public static event Action<Pet> OnPetAdopted;
        public static event Action<Pet> OnPetDied;
        public static event Action<Pet, string> OnPetStatusChanged;
        #endregion

        #region Game Events
        public static event Action<string> OnGameMessage;
        public static event Action<int> OnDayChanged;
        #endregion

        #region Item Events
        public static event Action<Pet, Item> OnItemUsed;
        #endregion

        #region Pet Event Triggers
        public static void TriggerPetAdopted(Pet pet)
        {
            OnPetAdopted?.Invoke(pet);
        }

        public static void TriggerPetDied(Pet pet)
        {
            OnPetDied?.Invoke(pet);
        }

        public static void TriggerPetStatusChanged(Pet pet, string message)
        {
            OnPetStatusChanged?.Invoke(pet, message);
        }
        #endregion

        #region Game Event Triggers
        public static void TriggerGameMessage(string message)
        {
            OnGameMessage?.Invoke(message);
        }

        public static void TriggerDayChanged(int newDay)
        {
            OnDayChanged?.Invoke(newDay);
        }
        #endregion

        #region Item Event Triggers
        public static void TriggerItemUsed(Pet pet, Item item)
        {
            OnItemUsed?.Invoke(pet, item);
        }
        #endregion
    }
}