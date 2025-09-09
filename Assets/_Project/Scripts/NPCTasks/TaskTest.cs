using NPC;
using System.Collections.Generic;
using UnityEngine;

public class TaskTest : MonoBehaviour
{
    public Task task;
    public List<Character> skills;
    public bool calculate = true;
    private void Update()
    {
        if (calculate)
        {
            calculate = false;
            task.AssignCharacters(skills);
        }
    }
}
