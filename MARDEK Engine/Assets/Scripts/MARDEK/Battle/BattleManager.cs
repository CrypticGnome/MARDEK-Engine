using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MARDEK.CharacterSystem;
using MARDEK.Stats;

namespace MARDEK.Battle
{
    using Progress;
    using UnityEngine.Events;

    public class BattleManager : MonoBehaviour
    {
        const float actResolution = 1000;
        public static EncounterSet encounter { private get; set; }
        [SerializeField] IntegerStat ACTStat = null;
        [SerializeField] IntegerStat AGLStat = null;
        [SerializeField] Party playerParty;
        [SerializeField] List<Character> DummyEnemies;
        [SerializeField] GameObject characterActionUI = null;
        [SerializeField] UnityEvent OnVictory;
        public List<Character> EnemyCharacters { get; private set; } = new List<Character>();
        public List<Character> PlayableCharacters { get { return playerParty.Characters; } }
        public static Character characterActing { get; private set; }
        public static Stats.IActionSlot selectedAction { get; set; }

        private void Awake()
        {
            if (encounter)
                EnemyCharacters = encounter.InstantiateEncounter();
            else
                EnemyCharacters = DummyEnemies;
        }
        private void Update()
        {
            for(int i = EnemyCharacters.Count - 1; i >= 0; i--)
            {
                var enemy = EnemyCharacters[i];
                var health = enemy.GetStat(StatsGlobals.Instance.CurrentHP);
                if (health <= 0)
                    EnemyCharacters.Remove(enemy);
            }

            var victory = EnemyCharacters.Count == 0;
            if (victory)
            {
                print("victory!!");
                OnVictory.Invoke();
                enabled = false;
                return;
            }

            if (characterActing == null)
            {
                characterActing = StepActCycleTryGetNextCharacter();
                if (characterActing != null)
                {
                    characterActionUI.SetActive(true);
                }
            }
            else
            {
                if (selectedAction != null)
                {
                    Character target;
                    if (EnemyCharacters.Contains(characterActing))
                    {
                        target = PlayableCharacters[Random.Range(0, PlayableCharacters.Count-1)];
                    }
                    else
                    {
                        target = EnemyCharacters[Random.Range(0, EnemyCharacters.Count-1)];
                    }

                    Debug.Log($"{characterActing.Profile.displayName} targets {target.Profile.displayName}");
                    selectedAction.ApplyAction(characterActing, target);
                    selectedAction = null;
                    characterActing = null;
                    characterActionUI.SetActive(false);
                }
            }
        }
        Character StepActCycleTryGetNextCharacter()
        {
            var charactersInBattle = GetCharactersInOrder();
            AddTickRateToACT(ref charactersInBattle, Time.deltaTime);
            var readyToAct = GetNextCharacterReadyToAct(charactersInBattle);
            if (readyToAct != null)
                readyToAct.ModifyStat(ACTStat, -(int)actResolution); // "reset" charact ACT
            return readyToAct;
        }
        List<Character> GetCharactersInOrder()
        {
            // order by Position (p1 e1 p2 e2 p3 e3 p4 e4)
            List<Character> returnList = new List<Character>();
            for (int i = 0; i < 4; i++)
            {
                if (PlayableCharacters.Count > i)
                    returnList.Add(PlayableCharacters[i]);
                if (EnemyCharacters.Count > i)
                    returnList.Add(EnemyCharacters[i]);
            }
            return returnList;
        }
        void AddTickRateToACT(ref List<Character> characters, float deltatime)
        {
            foreach(var c in characters)
            {
                var tickRate = 1 + 0.05f * c.GetStat(AGLStat);
                tickRate *= 1000 * deltatime;
                c.ModifyStat(ACTStat, (int)tickRate);
            }
        }
        Character GetNextCharacterReadyToAct(List<Character> characters)
        {
            float maxAct = 0;
            foreach(var c in characters)
            {
                var act = c.GetStat(ACTStat);
                if (act > maxAct)
                    maxAct = act;
            }
            if (maxAct < actResolution)
                return null;
            foreach (var c in characters)
                if (c.GetStat(ACTStat) == maxAct)
                    return c;
            throw new System.Exception("A character had enough ACT to take a turn but wasn't returned by this method");
        }
        public void SkipCurrentCharacterTurn()
        {
            characterActing = null;
            characterActionUI.SetActive(false);
        }
    }
}