using System;
using System.Collections.Generic;
using System.Linq;

namespace PetSimulator
{
    public class Pet
    {
        #region Fields
        private Dictionary<PetStat, int> stats;
        private Dictionary<ItemType, int> dailyUsage;
        private int birthDay;
        private string deathCause;
        private bool hasSleptToday;
        private int dailyActionCount;
        #endregion

        #region Properties
        public string Name { get; private set; }
        public PetType Type { get; private set; }
        public bool IsAlive { get; private set; }
        #endregion

        #region Constructor
        public Pet(string name, PetType type)
        {
            Name = name;
            Type = type;
            IsAlive = true;
            birthDay = 1; 
            deathCause = "";
            hasSleptToday = false;
            dailyActionCount = 0;
            
            stats = new Dictionary<PetStat, int>
            {
                { PetStat.Happiness, 85 }, { PetStat.Hunger, 85 },
                { PetStat.Sleep, 90 }, { PetStat.Energy, 90 }
            };

            dailyUsage = new Dictionary<ItemType, int>();
            foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
                dailyUsage[itemType] = 0;
        }
        #endregion

        #region Stat Management
        public int GetStatValue(PetStat stat) => stats[stat];

        public void UpdateStat(PetStat stat, int change)
        {
            stats[stat] = Math.Max(0, Math.Min(100, stats[stat] + change));
            
            if (stat == PetStat.Sleep && change != 0)
                stats[PetStat.Energy] = Math.Max(0, Math.Min(100, stats[PetStat.Energy] + (change / 2)));

            if (stat != PetStat.Happiness)
            {
                int minOtherStat = Math.Min(Math.Min(stats[PetStat.Hunger], stats[PetStat.Sleep]), stats[PetStat.Energy]);
                if (minOtherStat < 65)
                    stats[PetStat.Happiness] = Math.Min(stats[PetStat.Happiness], 65);
            }
            
            if (stats[stat] <= 5 && IsAlive)
            {
                IsAlive = false;
                deathCause = stat switch
                {
                    PetStat.Hunger => "Severe Starvation",
                    PetStat.Sleep => "Complete Exhaustion",
                    PetStat.Happiness => "Deep Depression",
                    PetStat.Energy => "Complete Energy Depletion",
                    _ => "Unknown"
                };
                EventSystem.TriggerPetDied(this);
            }
        }
        #endregion

        #region Activity Systems
        public string ApplyFood(Item food)
        {
            ApplyBaseActivityEffects(-5, -5, 0);
            
            if (!PetActivity.IsFoodAppropriate(Type, food.Type))
            {
                UpdateStat(PetStat.Happiness, -10);
                UpdateStat(PetStat.Hunger, -15);
                return PetActivity.GetFoodRejectionResponse(this, food);
            }
            
            if (++dailyUsage[food.Type] > 2)
            {
                UpdateStat(PetStat.Hunger, -3);
                return $"{Name} is getting full and doesn't want more {food.Name}!";
            }
            
            UpdateStat(PetStat.Hunger, 25);
            UpdateStat(PetStat.Energy, 20);
            UpdateStat(PetStat.Happiness, 17);
            return $"{Name} enjoys the {food.Name} and feels satisfied!";
        }

        public string ApplyPlay(string playType)
        {
            ApplyBaseActivityEffects(-5, -5, -5);
            
            var result = PetActivity.GetPlayResult(this, playType);
            if (result.CanPlay)
            {
                UpdateStat(PetStat.Happiness, result.HappinessBonus);
                UpdateStat(PetStat.Energy, result.EnergyChange);
                UpdateStat(PetStat.Sleep, result.SleepChange);
            }
            else
            {
                UpdateStat(PetStat.Happiness, -10);
            }
            
            return result.Message;
        }

        public string ApplyRest(string restType)
        {
            ApplyBaseActivityEffects(-5, -5, -5);
            
            var result = PetActivity.GetRestResult(this, restType);
            if (result.CanSleep)
            {
                hasSleptToday = true;
                UpdateStat(PetStat.Sleep, 45);
                UpdateStat(PetStat.Energy, 25);
                UpdateStat(PetStat.Happiness, result.HappinessBonus);
            }
            else
            {
                UpdateStat(PetStat.Happiness, -10);
            }
            
            return result.Message;
        }

        private void ApplyBaseActivityEffects(int sleep, int hunger, int happiness)
        {
            if (sleep != 0) UpdateStat(PetStat.Sleep, sleep);
            if (hunger != 0) UpdateStat(PetStat.Hunger, hunger);
            if (happiness != 0) UpdateStat(PetStat.Happiness, happiness);
        }
        #endregion

        #region Day Cycle & Status
        public void ApplyEndOfDayEffects()
        {
            if (!hasSleptToday)
            {
                UpdateStat(PetStat.Sleep, -30);
                UpdateStat(PetStat.Energy, -15);
                UpdateStat(PetStat.Happiness, -8);
                EventSystem.TriggerGameMessage($"{Name} looks tired because he/she didn't rest today!");
            }
            
            UpdateStat(PetStat.Hunger, -15);
            UpdateStat(PetStat.Energy, -10);
            hasSleptToday = false;
        }
        
        public bool CheckAndProcessDeath()
        {
            return !IsAlive;
        }

        public void ResetDailyUsage()
        {
            foreach (var key in dailyUsage.Keys.ToList())
                dailyUsage[key] = 0;
            hasSleptToday = false;
            dailyActionCount = 0;
        }

        public int GetDailyActionCount() => dailyActionCount;
        
        public bool CanDoMoreActions() => dailyActionCount < 3;
        
        public void IncrementActionCount() => dailyActionCount++;

        public int GetAge(int currentGameDay) => currentGameDay - birthDay;
        public string GetDeathCause() => deathCause;
        
        public string GetMood() => StatManager.GetMood(stats[PetStat.Happiness]);

        public string GetCurrentStatus() => $"{Name} ({Type}) - H:{stats[PetStat.Happiness]} Hu:{stats[PetStat.Hunger]} S:{stats[PetStat.Sleep]} E:{stats[PetStat.Energy]} - Mood: {GetMood()}";
        
        public override string ToString() => $"{Name} ({Type}) - Age: {birthDay} days, Mood: {GetMood()}";
        #endregion
    }
}