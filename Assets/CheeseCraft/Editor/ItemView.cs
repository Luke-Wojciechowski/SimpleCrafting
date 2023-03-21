using CheeseCraft.Script;
 
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace CheeseCraft.Editor
{
    public sealed class ItemView : UnityEditor.Experimental.GraphView.Node
    {
 
        private ItemConfig _item;
        
        public ItemConfig Item => _item;

        public enum ItemType
        {
            Component,
            Recipe
        }
        
        public ItemView(ItemConfig item)
        {
            this._item = item;
            this.title = item.name;
            
        }
    }
}