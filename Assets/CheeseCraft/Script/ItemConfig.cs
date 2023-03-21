using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseCraft.Script
{
    [CreateAssetMenu(fileName = "Item", menuName = "Crafting/Item")]
    public sealed class ItemConfig : ScriptableObject
    {
        [SerializeField] 
        private string itemName;

        [SerializeReference]
        private  RecipeConfig recipe = new RecipeConfig();
        
        public string ItemName 
        {
            get => itemName;
            set => itemName = value;
        }


        public RecipeConfig Recipe
        {
            get => recipe;
            set
            {
                if(value == null)
                    return;
                
                recipe = value;
            }
        }
    }
}