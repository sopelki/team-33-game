using System;
using System.Collections.Generic;

namespace Logic.Monster
{
    public class MonsterSystem
    {
        private readonly List<MonsterModel> monsters = new();

        public event Action<MonsterModel> OnMonsterCreated;
        public event Action<MonsterModel> OnMonsterDied;

        public void AddMonster(MonsterModel monster)
        {
            monsters.Add(monster);
            OnMonsterCreated?.Invoke(monster);
        }


        public void Tick()
        {
            // Debug.Log("MonsterSystem Tick");
            for (var i = monsters.Count - 1; i >= 0; i--)
            {
                var monster = monsters[i];

                monster.Tick();

                if (monster.IsDead)
                {
                    monsters.RemoveAt(i);
                    OnMonsterDied?.Invoke(monster);
                }
            }
        }

        public IReadOnlyList<MonsterModel> GetAllMonsters()
        {
            return monsters;
        }
    }
}