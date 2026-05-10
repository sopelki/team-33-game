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
            for (var i = monsters.Count - 1; i >= 0; i--)
            {
                var monster = monsters[i];

                monster.Tick();

                if (!monster.IsDead)
                    continue;
                OnMonsterDied?.Invoke(monster);
                monsters.RemoveAt(i);
            }
        }

        public IReadOnlyList<MonsterModel> GetAllMonsters()
        {
            return monsters;
        }

        // public void Clear() => monsters.Clear();
    }
}