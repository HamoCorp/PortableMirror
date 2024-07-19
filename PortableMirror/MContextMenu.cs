using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using Elements.Core;
using System.Xml.Linq;
using System.Reflection;

namespace PortableMirror {
    internal class MContextMenu {

        public MContextMenu() {
            
        }

        public void addRootItem(Slot UserRootSlot) {


            //conmenu.OnValueChange
            Slot ContextMenuRoot = UserRootSlot.AddSlot("Portable Mirror Mod");
            RootContextMenuItem rootcontext = ContextMenuRoot.AttachComponent<RootContextMenuItem>();

            ContextMenuItemSource cmitemSource = ContextMenuRoot.AttachComponent<ContextMenuItemSource>();

            ContextMenuSubmenu cmSub = ContextMenuRoot.AttachComponent<ContextMenuSubmenu>();

            SpriteProvider sp = ContextMenuRoot.AttachComponent<SpriteProvider>();
            StaticTexture2D statex = ContextMenuRoot.AttachComponent<StaticTexture2D>();

            statex.URL.Value = _sprites.Root;
            sp.Texture.Value = statex.ReferenceID;

            cmitemSource.Label.Value = "Portable Mirror";
            rootcontext.Item.Value = cmitemSource.ReferenceID;
            cmSub.ItemsRoot.Value = ContextMenuRoot.ReferenceID;
            cmitemSource.Sprite.Value = sp.ReferenceID;

            MainMenu(ContextMenuRoot);

        }
        public static Slot addItem(string Name, Slot Root, float R = 1, float G = 1, float B = 1, float A = 1, bool subMenu = false, ButtonEventHandler OnPress = null, Uri imageLink = null) {

            Slot CMItem = Root.AddSlot(Name, true);
            ContextMenuItemSource cmitemSource = CMItem.AttachComponent<ContextMenuItemSource>();
            cmitemSource.Label.Value = Name;
            cmitemSource.Color.Value = new colorX(R, G, B, A);

            SpriteProvider sp = CMItem.AttachComponent<SpriteProvider>();
            StaticTexture2D statex = CMItem.AttachComponent<StaticTexture2D>();
            statex.URL.Value = imageLink;
            sp.Texture.Value = statex.ReferenceID;
            cmitemSource.Sprite.Value = sp.ReferenceID;

            if (subMenu) {
                ContextMenuSubmenu cmSub = CMItem.AttachComponent<ContextMenuSubmenu>();

                cmSub.ItemsRoot.Value = CMItem.ReferenceID;
            }

            if (OnPress != null) {
                cmitemSource.LocalPressed += OnPress;
            }

            return CMItem;
        }

        public static Slot addToggle(string Name, string trueName, string falseName, Slot Root, ButtonEventHandler OnPress = null, Mirror.MirrorValues Value = Mirror.MirrorValues.none, int ColorProfile = 0, Uri imageLink = null, int index = 0) {

            Slot CMItem = Root.AddSlot(Name, true);
            ContextMenuItemSource cmitemSource = CMItem.AttachComponent<ContextMenuItemSource>();
            BooleanValueDriver<colorX> BVDColor = CMItem.AttachComponent<BooleanValueDriver<colorX>>();
            BooleanValueDriver<string> BVDText = CMItem.AttachComponent<BooleanValueDriver<string>>();
            DynamicValueVariable<bool> MirrorEnabled = CMItem.AttachComponent<DynamicValueVariable<bool>>();
            ValueCopy<bool> VCColourBool = CMItem.AttachComponent<ValueCopy<bool>>();
            ValueCopy<bool> VCTextBool = CMItem.AttachComponent<ValueCopy<bool>>();
//            ButtonToggle buttonToggle = CMItem.AttachComponent<ButtonToggle>();

            SpriteProvider sp = CMItem.AttachComponent<SpriteProvider>();
            StaticTexture2D statex = CMItem.AttachComponent<StaticTexture2D>();
            statex.URL.Value = imageLink;
            sp.Texture.Value = statex.ReferenceID;
            cmitemSource.Sprite.Value = sp.ReferenceID;

            MirrorEnabled.VariableName.Value = "User/" + Name + index.ToString();
            MirrorEnabled.Value.Value = false;

            if (ColorProfile == 0) {
                BVDColor.TrueValue.Value = new colorX(0.17f, 1, 0, 1);
                BVDColor.FalseValue.Value = new colorX(1, 0.06f, 0.06f, 1);
            }
            else {
                BVDColor.TrueValue.Value = new colorX(1, 0.95f, 0, 1);
                BVDColor.FalseValue.Value = new colorX(1, 0, 0.92f, 1);
            }
            BVDColor.TargetField.Value = cmitemSource.Color.ReferenceID;
            VCColourBool.Source.Value = MirrorEnabled.Value.ReferenceID;
            VCColourBool.Target.Value = BVDColor.State.ReferenceID;

            BVDText.TrueValue.Value = trueName;
            BVDText.FalseValue.Value = falseName;
            BVDText.TargetField.Value = cmitemSource.Label.ReferenceID;
            VCTextBool.Source.Value = MirrorEnabled.Value.ReferenceID;
            VCTextBool.Target.Value = BVDText.State.ReferenceID;

 //           buttonToggle.TargetValue.Value = MirrorEnabled.Value.ReferenceID;

            //            MirrorEnabled.Value.Changed += (IChangeable obj) => {
            //
            //                Mirror.UserSpaceSlot.AddSlot("Ligma", false);
            //                Mirror.UserSpaceSlot.LocalUser.Root.Destroy();
            //            };
            if (OnPress != null) {
                cmitemSource.LocalPressed += OnPress;
            }

            cmitemSource.LocalPressed += (IButton button, ButtonEventData eventData) => {

                MirrorEnabled.Value.Value = Mirror.MirrorsList[index].getValue(Value);
            };

            return CMItem;
        }

        private static void CmitemSource_LocalPressed(IButton button, ButtonEventData eventData) {
            throw new NotImplementedException();
        }

        public static Slot addBackButton(Slot Root, Slot BackLocation) {

            Slot BackSlot = Root.AddSlot("Back", true);
            ContextMenuItemSource cmitemSource = BackSlot.AttachComponent<ContextMenuItemSource>();
            cmitemSource.Label.Value = "Back";
            cmitemSource.Color.Value = new colorX(1, 0.6f, 0.6f, 1);

            ContextMenuSubmenu cmSub = BackSlot.AttachComponent<ContextMenuSubmenu>();
            cmSub.ItemsRoot.Value = BackLocation.ReferenceID;

            SpriteProvider sp = BackSlot.AttachComponent<SpriteProvider>();
            StaticTexture2D statex = BackSlot.AttachComponent<StaticTexture2D>();
            statex.URL.Value = _sprites.Back;
            sp.Texture.Value = statex.ReferenceID;
            cmitemSource.Sprite.Value = sp.ReferenceID;
            return BackSlot;
        }

        public static string getLocalString(string Name) {
            switch (Name) {
                case "":
                    return "Japanese";
                    break;
            }
            return Name;
        }

        private static void MainMenu(Slot Root) {
            addToggle("Mirror", "Mirror Enabled", "Mirror Disabled", Root, Mirror.MirrorsList[0].EnableToggle, Mirror.MirrorValues.Enabled, 0, _sprites.MainToggle);
            addToggle("Grabbable", "Grabbable Enabled", "Grabbable Disabled", Root, Mirror.MirrorsList[0].GrabableToggle, Mirror.MirrorValues.Grabable, 0, _sprites.grab);
            AddQualitySwitcher(Root);
            AddSettingMenu(Root);
            AddMoreMirrors(Root);
            //addToggle("Lock Position", "Use Local Position", "Use World Position", Root, null, 1, _sprites.Lock);
        }

        private static void AddSettingMenu(Slot MainMenuRoot, int index = 0) {
            Slot SettingRoot = addItem("Settings", MainMenuRoot, 1, 1, 1, 1, true, null, _sprites.Settings);
            addItem("Brightness +", SettingRoot, 0, 1, 0.86f, 1, false, Mirror.MirrorsList[index].BrightnessUp, _sprites.Add);
            addItem("Brightness -", SettingRoot, 0, 1, 0.86f, 1, false, Mirror.MirrorsList[index].BrightnessDown, _sprites.Sub);
            addItem("Opacity +", SettingRoot, 1, 0.2f, 0.2f, 1, false, Mirror.MirrorsList[index].OpacityUp, _sprites.Add);
            addItem("Opacity -", SettingRoot, 1, 0.2f, 0.2f, 1, false, Mirror.MirrorsList[index].OpacityDown, _sprites.Sub);
            addItem("Resolution +", SettingRoot, 0.02f, 1, 0, 1, false, Mirror.MirrorsList[index].ResolutionUp, _sprites.Add);
            addItem("Resolution -", SettingRoot, 0.2f, 1, 0, 1, false, Mirror.MirrorsList[index].ResolutionDown, _sprites.Sub);
            addItem("Scale +", SettingRoot, 0.98f, 1, 0.21f, 1, false, Mirror.MirrorsList[index].ScaleUp, _sprites.Add);
            addItem("Scale -", SettingRoot, 0.98f, 1, 0.21f, 1, false, Mirror.MirrorsList[index].ScaleDown, _sprites.Sub);
            AddMoreSettingMenu(SettingRoot, index);
            addBackButton(SettingRoot, MainMenuRoot);
        }

        private static void AddMoreSettingMenu(Slot SettingsMenuRoot, int index = 0) {
            Slot MoreSettingRoot = addItem("More Settings", SettingsMenuRoot, 1, 1, 1, 1, true, null, _sprites.Settings);
            addToggle("Disable Shadows", "Shadows Enabled", "Shadows Disabled", MoreSettingRoot, Mirror.MirrorsList[index].Shadows, Mirror.MirrorValues.DisableShadows, 0, _sprites.X, index);
            addToggle("Pin to view", "Pined to view", "Pin to view", MoreSettingRoot, Mirror.MirrorsList[index].Pin, Mirror.MirrorValues.Pin, 0, _sprites.Lock, index);
            addItem("Reset Settings", MoreSettingRoot, 0.98f, 1, 0.21f, 1, false, Mirror.MirrorsList[index].resetSettings, _sprites.Warning);
            addBackButton(MoreSettingRoot, SettingsMenuRoot);
        }

        private static void AddMoreMirrors(Slot MainMenuRoot) {
            Slot MoreMirrorsRoot = addItem("More Mirrors", MainMenuRoot, 0, 0, 1, 0, true, null, _sprites.Add);
            MoreMirrorMirror(MoreMirrorsRoot, 1);
            MoreMirrorMirror(MoreMirrorsRoot, 2);
            MoreMirrorMirror(MoreMirrorsRoot, 3);
            MoreMirrorMirror(MoreMirrorsRoot, 4);
            MoreMirrorMirror(MoreMirrorsRoot, 5);
            LocalLight(MoreMirrorsRoot);
            addBackButton(MoreMirrorsRoot, MainMenuRoot);
        }

        private static void AddQualitySwitcher(Slot MainMenuRoot, int index = 0) {
            Slot QualitySwitcherRoot = addItem("3D / Cuttout", MainMenuRoot, 0.32f, 0.98f, 1, 1, true, null, _sprites.MType);
            addItem("High Quality", QualitySwitcherRoot, 0, 1, 1, 1, false, Mirror.MirrorsList[index].MirrorHQ, _sprites.MType);
            //addItem("High Quality Cutout (Experiemental)", QualitySwitcherRoot, 0, 0, 1, 0, false, Mirror.MirrorsList[index].MirrorHQ);
            addItem("High Quality Opaque", QualitySwitcherRoot, 0, 1, 1, 1, false, Mirror.MirrorsList[index].MirrorHQOpaque, _sprites.MType);
            addItem("Camera", QualitySwitcherRoot, 0.412f, 0, 0.322f, 1, false, Mirror.MirrorsList[index].MirrorLQ, _sprites.Camera);
            addItem("Camera Skybox", QualitySwitcherRoot, 0.412f, 0, 0.322f, 1, false, Mirror.MirrorsList[index].MirrorLQSky, _sprites.Camera);
            addItem("Camera Opaque", QualitySwitcherRoot, 0.412f, 0, 0.322f, 1, false, Mirror.MirrorsList[index].MirrorLQOpaque, _sprites.Camera);
            addBackButton(QualitySwitcherRoot, MainMenuRoot);
        }

        private static void MoreMirrorMirror(Slot MMRoot, int index) {
            Slot MItem = addItem(Mirror.MirrorsList[index].getName(), MMRoot, 0, 0, 1, 0, true, null, _sprites.Add);
            addToggle("Mirror", "Mirror Enabled", "Mirror Disabled", MItem, Mirror.MirrorsList[index].EnableToggle, Mirror.MirrorValues.Enabled, 0, _sprites.MainToggle, index);
            addToggle("Grabbable", "Grabbable Enabled", "Grabbable Disabled", MItem, Mirror.MirrorsList[index].GrabableToggle, Mirror.MirrorValues.Grabable, 0, _sprites.grab, index);
            AddQualitySwitcher(MItem, index);
            AddSettingMenu(MItem, index);
            //addToggle("Lock Position", "Use Local Position", "Use World Position", MItem, null, Mirror.MirrorValues.Lock, 1, _sprites.Lock, index);
            addBackButton(MItem, MMRoot);
        }

        private static void LocalLight(Slot Root) {
            Slot LItem = addItem("Local light", Root, 0, 0, 0, 0, true, null, _sprites.Add);
            addToggle("LocalLight", "Light Enabled", "Light Disabled", LItem, Mirror.personalLight.EnableToggle, Mirror.MirrorValues.LightON, 0, _sprites.MainToggle, 7);
            addToggle("Grabbable", "Grabbable Enabled", "Grabbable Disabled", LItem, Mirror.MirrorsList[0].GrabableToggle, Mirror.MirrorValues.LightGrab, 0, _sprites.grab, 7);
            addToggle("UI", "Visual Enabled", "Visual Disabled", LItem, Mirror.MirrorsList[0].GrabableToggle, Mirror.MirrorValues.LightVisual, 0, _sprites.RedX, 7);
            addItem("Reset Settings", LItem, 0.98f, 1, 0.21f, 1, false, Mirror.personalLight.resetSettings, _sprites.Warning);

        }

        private struct _sprites {
            public static Uri Root = new Uri("resdb:///45952b461117e6e4e9258981d4f934918d2dfebf7ca130901ee6b07705eb98ee.png");
            public static Uri MainToggle = new Uri("resdb:///398cefdc4717c058dc7c9f6f1ac51d411cd6a9bfef8fd3a92fd40fafeec7e849.png");
            public static Uri Settings = new Uri("resdb:///cb7a566b05c8d60ee049b7a1cab875b8318901dcacd5f2bdec61fb47725d5535.png");
            public static Uri MType = new Uri("resdb:///c12a3af0764ffdc650d88a5b500dfdf09b664fe6fe9e8a0516c6ab99eb563a4b.png");
            public static Uri Lock = new Uri("resdb:///d3fb20ab9e32cf83865c3fd9f2210a86010cba72d1a13bce88d36993fada6f72.png");
            public static Uri grab = new Uri("resdb:///aa6a4df6b4a3f2e2a21bdbbcbe1643c31948505daaffe492263121a13451c7f5.png");
            public static Uri Add = new Uri("resdb:///ea7ef1770fc435fe1724b0eb169b4a8102bf03771a152f57c44aeaa24cb450ed.png");
            public static Uri Sub = new Uri("resdb:///99917b5f270735bd93364d8a0f5ca20145d244d41ef587fdebe9dcbb2dbb88c9.png");
            public static Uri Back = new Uri("resdb:///bcc9a58399d865680e29f13fc9ed537c94424753f88cfa6aca03fdaff8d43e20.png");
            public static Uri X = new Uri("resdb:///3f6e3edfeebf9193d5860f503a1b0962d4175a50bf1bec6f1fa1b98ccf919372.png");
            public static Uri RedX = new Uri("resdb:///c842210514b3956b03366fad5fab1a3137040e2ac7b7791372565d3c579be09c.png");
            public static Uri Warning = new Uri("resdb:///038264af30acb65e6c3d57e2cbc1540ac92d993076e79ddc0598a6e81663af7f.png");
            public static Uri Camera = new Uri("resdb:///5a315c19e2cdd47d2873f17a7234490cd517b21d61686d32c02caf276c347660.png");

        }


    }
}
