using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
namespace NPC.Tasks
{
    public enum Skills
    {
        Strength,
        Tech
    }
    [System.Serializable]
    public class Skill
    {
        [field: SerializeField] public Skills Type { get; protected set; }
        [field: SerializeField] public int Level { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Skill other = obj as Skill;
            if (other == null) return false;
            return Type == other.Type || Level == other.Level;
        }
        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Level.GetHashCode();
        }
        public override string ToString()
        {
            return $"{Type.ToString()} level {Level}.";
        }
    }
    [System.Serializable]
    public struct Requirement
    {
        public Skills Type;
        public int Level;
        /// <summary>
        /// How important is this requirement? Should be set to 1 by default,
        /// but structs are cringe (though efficient).
        /// </summary>
        public float Weight;
    }
    [System.Serializable]
    public class Task
    {
        /// <summary>
        /// Fires when the task is successfully completed.
        /// </summary>
        public UnityEvent OnCompletion;
        [field: SerializeField] public List<Requirement> Requirements { get; protected set; } = new();
        /// <summary>
        /// How likely the completion of the task is.
        /// </summary>
        [field: SerializeField] public float SuccessProbability { get; protected set; }
        /// <summary>
        /// Calculate probability of success given a set of input skills. 
        /// Does not play nice with duplicated skill types. 
        /// SuccessProbabilty is set to probability of success.
        /// </summary>
        /// <param name="inputSkills">The given skills to calculate probability for.</param>
        /// <returns>The probability of success.</returns>
        public float GetSuccessProbability(List<Skill> inputSkills)
        {
            SuccessProbability = 1;
            for (int i = 0; i < Requirements.Count; i++)
            {
                var requirement = Requirements[i];
                var skill = inputSkills.FirstOrDefault((s) =>
                {
                    return s.Type == requirement.Type;
                });
                if (skill == null) return 0;
                SuccessProbability -= (requirement.Level - skill.Level) * GlobalSettings.SkillDiffCoefficient * requirement.Weight;
            }
            return SuccessProbability;
        }
        /// <summary>
        /// Try to resolve the task, with the task's SuccessProbability.
        /// Fires OnCompletion if the task was completed.
        /// </summary>
        /// <returns>True if the task was completed successfully.</returns>
        public bool Resolve()
        {
            if (SuccessProbability >= 1 || (SuccessProbability > 0 && Random.value < SuccessProbability))
            {
                OnCompletion?.Invoke();
                return true;
            }
            return false;
        }
    }
}