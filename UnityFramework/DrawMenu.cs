using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityFramework.Menu;
using UnityEngine;
using UnityFramework.Helpers;
namespace UnityFramework
{
    class DrawMenu : MonoBehaviour
    {
        SubMenu MainMenu = new SubMenu("Main", "Menu");
      
        

        List<SubMenu> MenuHistory = new List<SubMenu>();
        SubMenu CurrentMenu;
        private Entity Selected;
        bool ShowMenu = true;
        string ConfigString = "Config";
        void Start()
        {
           
            SubMenu main = new SubMenu("Main", "");
            SubMenu Esp = new SubMenu("ESP", "Draw Visuals");
            { 
            }
            main.Items.Add(Esp);
            SubMenu Aimbot = new SubMenu("Aimbot", "Lock Onto Enemies");
            { 
            }
            main.Items.Add(Aimbot);
            SubMenu LocalPlayer = new SubMenu("Local Player", "Modify Your Player");
            { 
            
            }
            main.Items.Add(LocalPlayer);
            SubMenu config = new SubMenu("Configs", "Allows You To Save And Load Settings");
            {
                SubMenu SaveConfigMenu = new SubMenu("Save Configs", "");
                {
                    foreach (string str in ConfigHelper.GetConfigs())
                    {
                        Button btn = new Button(string.Concat("Save ", str), "", () => ConfigHelper.SaveConfig(str));
                        SaveConfigMenu.Items.Add(btn);
                    }
                    config.Items.Add(SaveConfigMenu);

                }
                SubMenu LoadConfigMenu = new SubMenu("Load Configs", "");
                {

                    foreach (string str in ConfigHelper.GetConfigs())
                    {
                        Button btn = new Button(string.Concat("Load ", str), "", () => { ConfigHelper.LoadConfig(str); Start(); });
                        LoadConfigMenu.Items.Add(btn);
                    }
                    config.Items.Add(LoadConfigMenu);
                }

                SubMenu CreateConfigMenu = new SubMenu("Create Config", "");
                {
                    TextField ConfigTextbox = new TextField("Config Name", "Enter A Config Name To Create", ref ConfigString);
                    Button CopyConfigNameFromClipboard = new Button($"Copy Config Name From Clipboard {ConfigString}", "Copies The Config Name From Your Clipboard", () => { ConfigString = GUIUtility.systemCopyBuffer; ConfigTextbox.Value = ConfigString; });
                    Button CreateConfig = new Button($"Create Config", "Creates Your Config", () => { ConfigString = ConfigTextbox.Value; ConfigHelper.SaveConfig(ConfigString); Start(); }); // set the value from the text field. GIVE ME ACCESS TO POINTERS!!!!
                    CreateConfigMenu.Items.Add(ConfigTextbox);
                    CreateConfigMenu.Items.Add(CopyConfigNameFromClipboard);
                    CreateConfigMenu.Items.Add(CreateConfig);
                    config.Items.Add(CreateConfigMenu);
                }
            }
            main.Items.Add(config);
            SubMenu menu = new SubMenu("Menu", "Change Menu Functionality");
            {
                SubMenu MenuPositioning = new SubMenu("Menu Positioning", "Certain Options For The Menu");
                {
                    FloatSlider menuposx = new FloatSlider("Menu Pos X", "Change X Axis Of Menu", ref Globals.Config.Menu.MenuPosx, 0, Screen.width, 10);
                    FloatSlider menuposy = new FloatSlider("Menu Pos Y", "Change Y Axis Of Menu", ref Globals.Config.Menu.MenuPosy, 0, Screen.height, 10);
                    MenuPositioning.Items.Add(menuposx);
                    MenuPositioning.Items.Add(menuposy);
                }
            }
            main.Items.Add(menu);
            SubMenu colours = new SubMenu("Colour", "Configure Colours");
            {
                Dictionary<string, SubMenu> CreatedMenuList = new Dictionary<string, SubMenu>();
                foreach (KeyValuePair<string, Color32> value in Globals.Config.Colours.ColoursDict)
                {

                    string HostColourMenuString = value.Key.Substring(0, value.Key.IndexOf(" ")); // find the first space and end there
                    SubMenu HostMenu = new SubMenu(HostColourMenuString, "Colour Categories");
                    if (CreatedMenuList.ContainsKey(HostColourMenuString))
                        HostMenu = CreatedMenuList[HostColourMenuString];
                    else
                    {
                        CreatedMenuList[HostColourMenuString] = HostMenu;
                    }
                    SubMenu colourmenu = new SubMenu(value.Key.Substring(value.Key.IndexOf(" ") + 1, value.Key.Length - value.Key.IndexOf(" ") - 1), "Customize Colours");
                    int alpha = Helpers.ColourHelper.GetColour(value.Key).a;
                    IntSlider slidera = new IntSlider("Alpha", "Change The Colour Opacity", ref alpha, 0, 255, 10);
                    int red = Helpers.ColourHelper.GetColour(value.Key).r;
                    IntSlider sliderr = new IntSlider("Red", "Change Amount Of Red In Colour", ref red, 0, 255, 10);
                    int green = Helpers.ColourHelper.GetColour(value.Key).g;
                    IntSlider sliderg = new IntSlider("Green", "Change Amount Of Green In Colour", ref green, 0, 255, 10);
                    int blue = Helpers.ColourHelper.GetColour(value.Key).b;
                    IntSlider sliderb = new IntSlider("Blue", "Change Amount Of Blue In Colour", ref blue, 0, 255, 10);
                    colourmenu.Items.Add(slidera);
                    colourmenu.Items.Add(sliderr);
                    colourmenu.Items.Add(sliderg);
                    colourmenu.Items.Add(sliderb);
                    colourmenu.Items.Add(new Button("Save Colour", "Right Arrow To Save The Colour", () => Helpers.ColourHelper.SetColour(value.Key, new Color32((byte)red, (byte)green, (byte)blue, (byte)alpha))));
                    HostMenu.Items.Add(colourmenu);


                    // got to add the menus after we have initialized all the values

                }
                foreach (SubMenu menus in CreatedMenuList.Values)
                    colours.Items.Add(menus);
                main.Items.Add(colours);

            }
        }
        void RenderElements()
        {
           

            foreach (Entity entity in CurrentMenu.Items)
            {

                if (Selected == entity)
                {
                    if (entity.Description != null)
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx - 5, Globals.Config.Menu.MenuPosy + (20f * (float)CurrentMenu.Items.Count)), entity.Description, Helpers.ColourHelper.GetColour("Menu Primary Colour"), false, 12, FontStyle.Normal, 0);
                    if (entity is Toggle)
                    {
                        Toggle toggle = entity as Toggle;
                        string ToggleStr = toggle.Value ? "Enabled" : "Disabled";
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"- {entity.Name}: {ToggleStr}", Helpers.ColourHelper.GetColour("Menu Primary Colour"), false, 14, FontStyle.Normal, 0);
                    }
                    if (entity is IntSlider)
                    {
                        IntSlider slider = entity as IntSlider;
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"- {entity.Name}: {slider.Value}", Helpers.ColourHelper.GetColour("Menu Primary Colour"), false, 14, FontStyle.Normal, 0);
                    }
                    if (entity is FloatSlider)
                    {
                        FloatSlider slider = entity as FloatSlider;
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"- {entity.Name}: {slider.Value}", Helpers.ColourHelper.GetColour("Menu Primary Colour"), false, 14, FontStyle.Normal, 0);
                    }
                    if (entity is Keybind)
                    {
                        Keybind bind = entity as Keybind;
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"- {entity.Name}: {bind.Value.ToString()}", Helpers.ColourHelper.GetColour("Menu Primary Colour"), false, 14, FontStyle.Normal, 0);
                    }
                    if (entity is SubMenu)
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"> {entity.Name}", Helpers.ColourHelper.GetColour("Menu Primary Colour"), false, 14, FontStyle.Normal, 0);
                    if (entity is Button)
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"- {entity.Name}", Helpers.ColourHelper.GetColour("Menu Primary Colour"), false, 14, FontStyle.Normal, 0);
                }
                else
                {
                    if (entity is Toggle)
                    {
                        Toggle toggle = entity as Toggle;
                        string ToggleStr = toggle.Value ? "Enabled" : "Disabled";
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"- {entity.Name}: {ToggleStr}", Helpers.ColourHelper.GetColour("Menu Secondary Colour"), false, 12, FontStyle.Normal, 0);
                    }
                    if (entity is IntSlider)
                    {
                        IntSlider slider = entity as IntSlider;
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"- {entity.Name}: {slider.Value}", Helpers.ColourHelper.GetColour("Menu Secondary Colour"), false, 12, FontStyle.Normal, 0);
                    }
                    if (entity is FloatSlider)
                    {
                        FloatSlider slider = entity as FloatSlider;
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"- {entity.Name}: {slider.Value}", Helpers.ColourHelper.GetColour("Menu Secondary Colour"), false, 12, FontStyle.Normal, 0);
                    }
                    if (entity is Keybind)
                    {
                        Keybind bind = entity as Keybind;
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"- {entity.Name}: {bind.Value.ToString()}", Helpers.ColourHelper.GetColour("Menu Secondary Colour"), false, 12, FontStyle.Normal, 0);
                    }
                    if (entity is SubMenu)
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"> {entity.Name}", Helpers.ColourHelper.GetColour("Menu Secondary Colour"), false, 12, FontStyle.Normal, 0);
                    if (entity is Button)
                        Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx, Globals.Config.Menu.MenuPosy + (20 * CurrentMenu.Items.IndexOf(entity))), $"- {entity.Name}", Helpers.ColourHelper.GetColour("Menu Secondary Colour"), false, 12, FontStyle.Normal, 0);
                }
            }
        }
        void Controls()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
                ShowMenu = !ShowMenu;
            if (!ShowMenu)
                return;
            if (Input.GetKeyDown(KeyCode.DownArrow) && CurrentMenu.index < CurrentMenu.Items.Count)
                CurrentMenu.index++;
            if (Input.GetKeyDown(KeyCode.UpArrow) && CurrentMenu.index > 0)
                CurrentMenu.index--;
            if (Input.GetKeyDown(KeyCode.Backspace) && MenuHistory.Count > 1)
            {
                CurrentMenu = MenuHistory[MenuHistory.Count - 2];
                MenuHistory.Remove(MenuHistory.Last<SubMenu>());
                return;
            }
            if (((Input.GetKeyDown(KeyCode.LeftArrow) && Selected is SubMenu)) && CurrentMenu.index < CurrentMenu.Items.Count)
            {

                CurrentMenu = MenuHistory[MenuHistory.Count - 2];
                MenuHistory.Remove(MenuHistory.Last<SubMenu>());
                return;
            }
            foreach (Entity entity in CurrentMenu.Items)
            {

                if (CurrentMenu.index == CurrentMenu.Items.IndexOf(entity))
                    Selected = entity;
                if (entity != Selected)
                    continue;

            }
            if (Selected is Keybind)
            {
                Keybind bind = Selected as Keybind;
                if (bind.Value == KeyCode.None)
                    bind.Value = SetKey();

            }
            if (Selected is SubMenu && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return)))
            {
                CurrentMenu = Selected as SubMenu;
                MenuHistory.Add(Selected as SubMenu);
                return; // opens a new menu so we need to exit the loop to then render our new currentmenu
            }
            if (Selected is Toggle && Input.GetKeyDown(KeyCode.RightArrow))
            {
                Toggle toggle = Selected as Toggle;
                toggle.Value = true;
            }
            if (Selected is Toggle && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Toggle toggle = Selected as Toggle;
                toggle.Value = false;
            }
            if (Selected is Button && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return)))
            {
                Button button = Selected as Button;
                button.Method();
            }
            if (Selected is IntSlider && Input.GetKeyDown(KeyCode.RightArrow))
            {
                IntSlider slider = Selected as IntSlider;
                int result = slider.Value + slider.IncrementValue;

                if (result > slider.MaxValue)
                    slider.Value = slider.MaxValue;
                else
                    slider.Value = result;
            }
            if (Selected is IntSlider && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                IntSlider slider = Selected as IntSlider;
                int result = slider.Value - slider.IncrementValue;

                if (result < slider.MinValue)
                    slider.Value = slider.MinValue;
                else
                    slider.Value = result;
            }
            if (Selected is FloatSlider && Input.GetKeyDown(KeyCode.RightArrow))
            {
                FloatSlider slider = Selected as FloatSlider;
                float result = slider.Value + slider.IncrementValue;

                if (result > slider.MaxValue)
                    slider.Value = slider.MaxValue;
                else
                    slider.Value = result;
            }
            if (Selected is FloatSlider && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                FloatSlider slider = Selected as FloatSlider;
                float result = slider.Value - slider.IncrementValue;

                if (result < slider.MinValue)
                    slider.Value = slider.MinValue;
                else
                    slider.Value = result;
            }
            if (Selected is Keybind && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return)))
            {
                Keybind bind = Selected as Keybind;
                bind.Value = KeyCode.None;
            }
        }
        KeyCode SetKey()
        {
            KeyCode Key = new KeyCode();
            Event e = Event.current;
            if (e.keyCode != KeyCode.RightArrow)
            {
                Key = e.keyCode;


            }
            else
            {
                Key = KeyCode.None;

            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Key = KeyCode.Mouse0;

            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Key = KeyCode.Mouse1;

            }
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                Key = KeyCode.Mouse2;

            }
            if (Input.GetKeyDown(KeyCode.Mouse3))
            {
                Key = KeyCode.Mouse3;

            }
            if (Input.GetKeyDown(KeyCode.Mouse4))
            {
                Key = KeyCode.Mouse4;

            }
            if (Input.GetKeyDown(KeyCode.Mouse5))
            {
                Key = KeyCode.Mouse5;

            }
            if (Input.GetKeyDown(KeyCode.Mouse6))
            {
                Key = KeyCode.Mouse6;

            }
            return Key;
        }
        void OnGUI()
        {
            Drawing.DrawString(new Vector2(5, 0), "Menu Title", Helpers.ColourHelper.GetColour("Menu Primary Colour"), false, 14, FontStyle.Normal, 0);
            Globals.MainCamera = Camera.main;
            if (!ShowMenu)
                return;
            string text = string.Empty;
            if (MenuHistory.Count <= 0)
                return;
            foreach (SubMenu subMenu in MenuHistory)
            {
                if (subMenu == null)
                    return;
                if (subMenu == MenuHistory.Last<SubMenu>())
                {
                    text += subMenu.Name + " v ";
                }
                else
                {
                    text = text + subMenu.Name + " > ";
                }

            }
            Drawing.DrawString(new Vector2(Globals.Config.Menu.MenuPosx - 5, Globals.Config.Menu.MenuPosy - 20), text, Helpers.ColourHelper.GetColour("Menu Primary Colour"), false, 12, FontStyle.Normal, 0); // draw menu history
            RenderElements();
        }
        void Update()
        {
            Controls();
        }
    }
}
