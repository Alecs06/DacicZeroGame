using System.Collections.Generic;
using UnityEngine;
namespace NPC
{
    [System.Serializable]
    public class Character
    {
        [SerializeField] protected string description = "Cannon fodder";
        public List<Skill> Skills = new();
        public override string ToString()
        {
            string s = description;
            if (Skills.Count > 0)
            {
                s += "\nSkills:";
                for (int i = 0; i < Skills.Count; i++)
                {
                    s += $"\n{Skills[i].ToString()}";
                }
            }
            return s;
        }
    }
}
