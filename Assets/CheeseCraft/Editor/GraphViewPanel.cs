using System.Collections.Generic;
using System.Linq;
using CheeseCraft.Script;
 
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CheeseCraft.Editor
{
    public sealed class GraphViewPanel : GraphView
    {
        private const int ITEMS_RESPAWN_MARGIN = 50;
        public new class UxmlFactory : UxmlFactory<GraphViewPanel, GraphView.UxmlTraits> { }
        
        private List<ItemConfig> _items = new ();
        private ItemConfig _currentRecipeItem;
        
       internal ItemConfig CurrentRecipeItem
       {
           set
           {
               if(value == null)
                   return;
               
               _items.Clear();
               DeleteElements(graphElements.ToList());
               
               _currentRecipeItem = value;
               CreateNodeList();
           }
       }
        public GraphViewPanel()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements.ToList());
            graphViewChanged += OnGraphViewChanged;
        }
   
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
 
            graphViewChange.elementsToRemove?.ForEach(e =>
           {
               if (e is ItemView view)
               {
                   _items.Remove(view.Item);
               }
           });
            return graphViewChange;
        }
        private void CreateNodeList()
        {
            _currentRecipeItem.Recipe ??= new RecipeConfig();
            var recipeItems = _currentRecipeItem.Recipe.neededItems;
            
            var cashedSize = recipeItems.Count;
            for (int i = 0; i < cashedSize; i++)
            {
                _items.Add(recipeItems[i]);
                var x = CreateNodeView(recipeItems[i]);
                x.transform.position = new Vector3(0, i* ITEMS_RESPAWN_MARGIN);
            }
           
        }
        
        private ItemView CreateNodeView(ItemConfig node)
        {
            var view = new ItemView(node);

            this.AddElement(view);
            return view;
        }

        public void AddComponent(ItemConfig node)
        {
            if(node == null || node == _currentRecipeItem)
                return;

            _items.Add(node);
            CreateNodeView(node);
        }

        public void SaveChanges()
        {
            if(_currentRecipeItem == null)
                return;
                
            _currentRecipeItem.Recipe.neededItems = new List<ItemConfig>(_items);
            
            EditorUtility.SetDirty(_currentRecipeItem);
            AssetDatabase.SaveAssets();
        }
        
    }
}