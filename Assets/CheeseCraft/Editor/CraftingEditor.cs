using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CheeseCraft.Script;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
 

namespace CheeseCraft.Editor
{
    public sealed class CraftingEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset rootVisualTree;
        [SerializeField] private StyleSheet rootsStyleSheet;
        [SerializeField] private RecipesListConfig recipesListConfig;
        
        
        private GraphViewPanel _graphView;
        private VisualElement _root;
        private List<ItemConfig> _craft = new();
        private Button _button;

        private List<ItemConfig> _cashedItems;
        [MenuItem("Crafting/CraftingEditor")]
        public static void Init()
        {
            CraftingEditor wnd = GetWindow<CraftingEditor>();
            wnd.titleContent = new GUIContent("CraftingEditor");
            wnd.minSize = new Vector2(500, 400); 
        }
      
        public void CreateGUI()
        {
            var root = rootVisualElement;
            
            var visualElement = rootVisualTree;
            visualElement.CloneTree(root);
                
            var styleSheet = rootsStyleSheet;
            root.styleSheets.Add(styleSheet);
            
            _graphView = root.Q<GraphViewPanel>();
            _root = root;
            _cashedItems = new List<ItemConfig>(recipesListConfig.AllItems);
            _button = root.Q<Button>();
            
            _button.UnregisterCallback<MouseUpEvent>(BindButtonEvent);
            _button.RegisterCallback<MouseUpEvent>(BindButtonEvent);
            
            CreateItemRecipeView();
            CreateItemComponentView();
        }

        private void BindButtonEvent(MouseUpEvent ev)
        {
            var textField = _root.Q<TextField>();
            string nameOfNewItem = textField.value;
            var item = ScriptableObject.CreateInstance<ItemConfig>();
            item.ItemName = nameOfNewItem;

            string basicPath = Directory.GetFiles(Application.dataPath, "RecipesList.asset", SearchOption.AllDirectories).FirstOrDefault();

            StringBuilder stringBuilder = new StringBuilder();
            var split = basicPath.Split("/")[^1].Split("\\");
            for (int i = 0; i < split.Length-1; i++)
            {
                stringBuilder.Append(split[i] + "\\");
            }
            
            string path = $"{stringBuilder}Items/{nameOfNewItem}.asset".Replace("/","\\");
       
            AssetDatabase.CreateAsset(item, path);
            recipesListConfig.AllItems.Add(item);
            
            EditorUtility.SetDirty(recipesListConfig);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            _cashedItems = new List<ItemConfig>(recipesListConfig.AllItems);
            Close();
        }
        
        private void CreateItemRecipeView()
        {
            var listView = _root.Q<ListView>("recipe");
            CreateItemView(listView);
            
            listView.onItemsChosen -= OnRecipeItemSelected;
            listView.onItemsChosen += OnRecipeItemSelected;
        }

        private void CreateItemComponentView()
        {
            var listView = _root.Q<ListView>("component");
            CreateItemView(listView);
            
            listView.onItemsChosen -= OnComponentItemSelected;
            listView.onItemsChosen += OnComponentItemSelected;
        }
        
        private void CreateItemView(ListView view)
        {
            Func<VisualElement> makeItem = () => new Label();
            Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = _cashedItems[i].ItemName;

            view.makeItem = makeItem;
            view.bindItem = bindItem;
            view.itemsSource = _cashedItems;
            view.selectionType = SelectionType.Single;
        }
        
        private void OnRecipeItemSelected(IEnumerable<object> collection)
        {
            var item = collection.ToList().FirstOrDefault() as ItemConfig;

            var textTitle = _root.Q<Label>("CurrentView");
            textTitle.text = item.ItemName;
            _graphView.SaveChanges();
            
            _graphView.CurrentRecipeItem = item;
        }

        private void OnComponentItemSelected(IEnumerable<object> collection)
        {
            var item = collection.ToList().FirstOrDefault() as ItemConfig;
            _graphView.AddComponent(item); 
        }
    }
}