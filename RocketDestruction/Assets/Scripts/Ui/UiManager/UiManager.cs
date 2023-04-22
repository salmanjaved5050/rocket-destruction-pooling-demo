using System.Collections.Generic;
using System.Linq;
using RocketDestruction.Core;
using RocketDestruction.SignalSystem;
using Supyrb;
using UnityEngine;

namespace RocketDestruction.Ui.UiManager
{
    public class UiManager : MonoBehaviour,IRdManager
    {
        [SerializeField] private Canvas   canvas;
        [SerializeField] private MenuType defaultMenu;

        private List<UiMenu> _uiMenus;
        private UiMenu       _currentMenu;
        
        private void OnEnable()
        {
            Signals.Get<HideMenu>()
                .AddListener(OnHideMenu);

            Signals.Get<ShowMenu>()
                .AddListener(OnShowMenu);
        }
        
        private void OnDisable()
        {
            Signals.Get<HideMenu>()
                .RemoveListener(OnHideMenu);

            Signals.Get<ShowMenu>()
                .RemoveListener(OnShowMenu);
        }

        private void InitializeUi()
        {
            _uiMenus = canvas.GetComponentsInChildren<UiMenu>(true)
                .ToList();

            if (!_uiMenus.Any()) Debug.LogError("No UI Menus Found In Scene.");

            List<MenuType> dups = _uiMenus.GroupBy(menu => menu.GetMenuType())
                .Where(group => group.Count() > 1)
                .Select(menu => menu.Key)
                .ToList();

            if (!dups.Any()) return;

            for (int i = 0; i < dups.Count; i++)
                Debug.LogError("Found Duplicate Menu : " + dups[i]);
        }

        private UiMenu GetMenu(MenuType menuName)
        {
            return _uiMenus.Find(menu => menu.GetMenuType() == menuName);
        }

        private void HideCurrentMenu()
        {
            if (!_currentMenu) return;
            _currentMenu.HideMenu();
        }

        private void ShowMenu(MenuType menuName)
        {
            UiMenu menu = GetMenu(menuName);

            if (menu)
            {
                HideCurrentMenu();
                menu.ShowMenu();
                _currentMenu = menu;
            }
            else
            {
                Debug.LogError("No Menu Found With Name : " + menuName);
            }
        }
        
        private void SwitchMenu(MenuType menuType)
        {
            ShowMenu(menuType);
        }

        private void HideMenu(MenuType menuType)
        {
            UiMenu menu = GetMenu(menuType);
            if (menu)
                menu.HideMenu();
            else
                Debug.LogError("No Menu Found With Name : " + menuType);
        }

        private void OnShowMenu(MenuType menuType)
        {
            ShowMenu(menuType);
        }

        private void OnHideMenu(MenuType menuType)
        {
            HideMenu(menuType);
        }

        public void Init()
        {
            InitializeUi();
            
            for (int i = 0; i < _uiMenus.Count; i++)
            {
                _uiMenus[i]
                    .HideMenu();
            }

            ShowMenu(defaultMenu);
        }

        public void Reset()
        {
            for (int i = 0; i < _uiMenus.Count; i++)
            {
                _uiMenus[i].ResetMenu();
            }
        }
    }
}