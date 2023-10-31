using UnityEngine.UIElements;

namespace CryptoQuestEditor.System.YarnSpinner
{
    public class TabbedMenuController
    {
        private const string TAB_CLASS_NAME = "tab";
        private const string CURRENTLY_SELECTED_TAB_CLASS_NAME = "currentlySelectedTab";
        private const string UNSELECTED_CONTENT_CLASS_NAME = "unselectedContent";
        private const string TAB_NAME_SUFFIX = "Tab";
        private const string CONTENT_NAME_SUFFIX = "Content";

        private readonly VisualElement _root;

        public TabbedMenuController(VisualElement root) => _root = root;


        private static bool TabIsCurrentlySelected(VisualElement tab) =>
            tab.ClassListContains(CURRENTLY_SELECTED_TAB_CLASS_NAME);

        private UQueryBuilder<Label> GetAllTabs() => _root.Query<Label>(className: TAB_CLASS_NAME);

        private static string GenerateContentName(VisualElement tab) =>
            tab.name.Replace(TAB_NAME_SUFFIX, CONTENT_NAME_SUFFIX);

        private VisualElement FindContent(Label tab) => _root.Q(GenerateContentName(tab));

        public void RegisterTabCallbacks()
        {
            UQueryBuilder<Label> tabs = GetAllTabs();
            tabs.ForEach(tab => { tab.RegisterCallback<ClickEvent>(TabOnClick); });
        }

        private void TabOnClick(ClickEvent evt)
        {
            Label clickedTab = evt.currentTarget as Label;
            if (TabIsCurrentlySelected(clickedTab)) return;

            GetAllTabs().Where(
                (tab) => tab != clickedTab && TabIsCurrentlySelected(tab)
            ).ForEach(UnselectTab);

            SelectTab(clickedTab);
        }

        private void SelectTab(Label tab)
        {
            tab.AddToClassList(CURRENTLY_SELECTED_TAB_CLASS_NAME);
            VisualElement content = FindContent(tab);
            content.RemoveFromClassList(UNSELECTED_CONTENT_CLASS_NAME);
        }

        private void UnselectTab(Label tab)
        {
            tab.RemoveFromClassList(CURRENTLY_SELECTED_TAB_CLASS_NAME);
            VisualElement content = FindContent(tab);
            content.AddToClassList(UNSELECTED_CONTENT_CLASS_NAME);
        }
    }
}