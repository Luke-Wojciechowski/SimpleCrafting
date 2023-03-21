using System.Collections.Generic;
using UnityEngine;

namespace CheeseCraft.Script
{
    public sealed class RecipesListConfig : ScriptableObject
    {
        [SerializeReference] private List<ItemConfig> allItems;
        
        public List<ItemConfig> AllItems => allItems;
    }
}