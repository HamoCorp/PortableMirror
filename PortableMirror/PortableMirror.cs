using ResoniteModLoader;
using HarmonyLib;
using FrooxEngine;
using Elements.Core;
using System.Threading;
using System;

namespace PortableMirror {
    public class PortableMirror : ResoniteMod {

        public override string Name => "PortableMirror";
        public override string Author => "HamoCorp";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/HamoCorp/PortableMirror";

        private static ModConfiguration Config;

        public override void OnEngineInit() {

            Config = GetConfiguration();
            Config.Save(true);
            Harmony harmony = new Harmony("com.HamoCorp.PortableMirror");
            harmony.PatchAll();

        }

        public static void upDateMirrorValues(int index = 0) {

            Mirror.MirrorsList[0].upDatePrivateValues("Main Mirror", Config.GetValue(_position1), Config.GetValue(_rotation1), Config.GetValue(_scale1), Config.GetValue(_MType1), Config.GetValue(_grabable1), Config.GetValue(_Pin1), false, Config.GetValue(_Shadows1), Config.GetValue(_brightness1), Config.GetValue(_Opacity1), Config.GetValue(_Resolution1), Config.GetValue(_RotationSource1));
            Mirror.MirrorsList[1].upDatePrivateValues(Config.GetValue(_mirror2), Config.GetValue(_position2), Config.GetValue(_rotation2), Config.GetValue(_scale2), Config.GetValue(_MType2), Config.GetValue(_grabable2), Config.GetValue(_Pin2), false, Config.GetValue(_Shadows2), Config.GetValue(_brightness2), Config.GetValue(_Opacity2), Config.GetValue(_Resolution2), Config.GetValue(_RotationSource2));
            Mirror.MirrorsList[2].upDatePrivateValues(Config.GetValue(_mirror3), Config.GetValue(_position3), Config.GetValue(_rotation3), Config.GetValue(_scale3), Config.GetValue(_MType3), Config.GetValue(_grabable3), Config.GetValue(_Pin3), false, Config.GetValue(_Shadows3), Config.GetValue(_brightness3), Config.GetValue(_Opacity3), Config.GetValue(_Resolution3), Config.GetValue(_RotationSource3));
            Mirror.MirrorsList[3].upDatePrivateValues("Face Mirror", Config.GetValue(_position4), Config.GetValue(_rotation4), Config.GetValue(_scale4), Config.GetValue(_MType4), Config.GetValue(_grabable4), Config.GetValue(_Pin4), false, Config.GetValue(_Shadows4), Config.GetValue(_brightness4), Config.GetValue(_Opacity4), Config.GetValue(_Resolution4), Config.GetValue(_RotationSource4));
            Mirror.MirrorsList[4].upDatePrivateValues(Config.GetValue(_mirror5), Config.GetValue(_position5), Config.GetValue(_rotation5), Config.GetValue(_scale5), Config.GetValue(_MType5), Config.GetValue(_grabable5), Config.GetValue(_Pin5), false, Config.GetValue(_Shadows5), Config.GetValue(_brightness5), Config.GetValue(_Opacity5), Config.GetValue(_Resolution5), Config.GetValue(_RotationSource5));
            Mirror.MirrorsList[5].upDatePrivateValues("FBT Mirror", Config.GetValue(_position6), Config.GetValue(_rotation6), Config.GetValue(_scale6), Config.GetValue(_MType6), Config.GetValue(_grabable6), Config.GetValue(_Pin6), false, Config.GetValue(_Shadows6), Config.GetValue(_brightness6), Config.GetValue(_Opacity6), Config.GetValue(_Resolution6), Config.GetValue(_RotationSource6));
            Mirror.personalLight.setValues(Config.GetValue(_LightColor), Config.GetValue(_LightRange));

/*
                switch (index) {
                case 0:
                    Mirror.MirrorsList[0].upDatePrivateValues("Main Mirror", Config.GetValue(_position1), Config.GetValue(_rotation1), Config.GetValue(_scale1), Config.GetValue(_MType1), Config.GetValue(_grabable1), Config.GetValue(_Pin1), false, Config.GetValue(_Shadows1), Config.GetValue(_brightness1), Config.GetValue(_Opacity1), Config.GetValue(_RotationSource1));
                    break; 
                case 1:
                    Mirror.MirrorsList[1].upDatePrivateValues(Config.GetValue(_mirror2), Config.GetValue(_position2), Config.GetValue(_rotation2), Config.GetValue(_scale2), Config.GetValue(_MType2), Config.GetValue(_grabable2), Config.GetValue(_Pin2), false, Config.GetValue(_Shadows2), Config.GetValue(_brightness2), Config.GetValue(_Opacity2), Config.GetValue(_RotationSource2));
                    break;
                case 2:
                    Mirror.MirrorsList[2].upDatePrivateValues(Config.GetValue(_mirror3), Config.GetValue(_position3), Config.GetValue(_rotation3), Config.GetValue(_scale3), Config.GetValue(_MType3), Config.GetValue(_grabable3), Config.GetValue(_Pin3), false, Config.GetValue(_Shadows3), Config.GetValue(_brightness3), Config.GetValue(_Opacity3), Config.GetValue(_RotationSource3));
                    break;
                case 3:
                    Mirror.MirrorsList[3].upDatePrivateValues("Face Mirror", Config.GetValue(_position4), Config.GetValue(_rotation4), Config.GetValue(_scale4), Config.GetValue(_MType4), Config.GetValue(_grabable4), Config.GetValue(_Pin4), false, Config.GetValue(_Shadows4), Config.GetValue(_brightness4), Config.GetValue(_Opacity4), Config.GetValue(_RotationSource4));
                    break;
                case 4:
                    Mirror.MirrorsList[4].upDatePrivateValues(Config.GetValue(_mirror5), Config.GetValue(_position5), Config.GetValue(_rotation5), Config.GetValue(_scale5), Config.GetValue(_MType5), Config.GetValue(_grabable5), Config.GetValue(_Pin5), false, Config.GetValue(_Shadows5), Config.GetValue(_brightness5), Config.GetValue(_Opacity5), Config.GetValue(_RotationSource5));
                    break; 
                case 5:
                    Mirror.MirrorsList[5].upDatePrivateValues("FBT Mirror", Config.GetValue(_position6), Config.GetValue(_rotation6), Config.GetValue(_scale6), Config.GetValue(_MType6), Config.GetValue(_grabable6), Config.GetValue(_Pin6), false, Config.GetValue(_Shadows6), Config.GetValue(_brightness6), Config.GetValue(_Opacity6), Config.GetValue(_RotationSource6));
                    break;
                case 6:
                    Mirror.personalLight.setValues(Config.GetValue(_LightColor), Config.GetValue(_LightRange));
                    break;

            }
*/
        }

        public static string nameGenerator(int length) {

            string name = " ";
            for (int i = 0; i < length; i++) {
                name += " ";
            }
            return name;
        }

        [HarmonyPatch]
        class MirrorPatch {

            [HarmonyPostfix]
            [HarmonyPatch(typeof(UserRoot), "OnStart")]
            public static void UserRootPatch(UserRoot __instance) {
                
                if (__instance.World.IsUserspace()) {
                    Mirror.MirrorsList.Add(new Mirror("Main Mirror"));
                    Mirror.MirrorsList.Add(new Mirror("Ceiling Mirror"));
                    Mirror.MirrorsList.Add(new Mirror("Mirror 45"));
                    Mirror.MirrorsList.Add(new Mirror("Face Mirror"));
                    Mirror.MirrorsList.Add(new Mirror("Extra Mirror"));
                    Mirror.MirrorsList.Add(new Mirror("FBT Mirror"));

                    upDateMirrorValues();
                    upDateMirrorValues(1);
                    upDateMirrorValues(2);
                    upDateMirrorValues(3);
                    upDateMirrorValues(4);
                    upDateMirrorValues(5);
                    upDateMirrorValues(6);

                    return;
                }

                if (__instance.Slot.ActiveUser != null && __instance.Slot.ActiveUser.IsLocalUser && Config.GetValue(_enabled))
                    _ContextMenu.addRootItem(__instance.Slot);

            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(Userspace), "OnCommonUpdate")]
            public static void UspaceUpdate(Userspace __instance) {

                if (_userSpaceSlot != null) {
                    
                    foreach (Mirror m in Mirror.MirrorsList) {
                        m.OnCommonUpdate(_userSpaceSlot);
                    }
                    Mirror.personalLight.OnCommonUpdate(_userSpaceSlot);

                }
                else
                    _userSpaceSlot = __instance.Slot.AddSlot("Portable Mirror Userspace", false);

                
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(FullBodyCalibrator), "OnAttach")]
            public static void FBTCalibratorOpen(FullBodyCalibrator __instance) {
                User user = __instance.TargetUser;
                if (user == __instance.LocalUser && Config.GetValue(_enableFBTMirror)) {
                    Mirror.MirrorsList[5].setEnabled(true);
                }
            }


            [HarmonyPostfix]
            [HarmonyPatch(typeof(FullBodyCalibrator), "OnDestroy")]
            public static void FBTCalibratorClose(FullBodyCalibrator __instance) {
                User user = __instance.TargetUser;
                if (user == __instance.LocalUser && Config.GetValue(_enableFBTMirror)) {
                    Mirror.MirrorsList[5].setEnabled(false);
                }
            }

            public static void FixContextMenuOnMirror() {

            }


            //            [HarmonyPostfix]
            //            [HarmonyPatch(typeof(InteractionLaser), "UpdateLaser")]
            //            public static void MirrorLaserColour(InteractionLaser __instance, Sync<colorX> ____hitColor, float? maintainDistance, float? overrideSmoothing, InteractionOrigin? overrideOrigin, bool targettingOnly) {
            //
            //                Slot LastHit = __instance.LastHit.MeshCollider.Slot;
            //
            //                if (LastHit != null && LastHit.World.IsUserspace()) {
            //                    if(Mirror.personalLight.getLightSlot() == LastHit) {
            //
            //                    }
            //                    foreach (Mirror m in Mirror.MirrorsList) {
            //                        if (m.GetMirrorSlot() == LastHit) {
            //                            //float t = ((float)DateTime.Now.Millisecond) / 1000;
            //                            //____hitColor.Value = new colorX((float)Math.Sin(t), (float)Math.Sin(t * 6), 0);
            //                            ____hitColor.Value = new colorX(1, 1, 1);
            //                        }
            //                    }
            //                }
            //                //hitcolor change
            //                //last hit find mirror
            //            }

        }


        private static MContextMenu _ContextMenu = new MContextMenu();
        private static Slot _userSpaceSlot;


        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _enabled = new ModConfigurationKey<bool>("enabled", "Show Mirrors on Context menu", () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _enableFBTMirror = new ModConfigurationKey<bool>("enabledFBT", "Activate FBT Mirror when calibrator is open", () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d30 = new ModConfigurationKey<dummy>(nameGenerator(30), "█▀█ █▀█ █▀█ ▀█▀ ▄▀█ █▄▄ █░░ █▀▀   █▀▄▀█ █ █▀█ █▀█ █▀█ █▀█");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d31 = new ModConfigurationKey<dummy>(nameGenerator(31), "█▀▀ █▄█ █▀▄ ░█░ █▀█ █▄█ █▄▄ ██▄   █░▀░█ █ █▀▄ █▀▄ █▄█ █▀▄");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d1 = new ModConfigurationKey<dummy>(nameGenerator(1), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d2 = new ModConfigurationKey<dummy>(nameGenerator(2), "Main Mirror");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float3> _position1 = new ModConfigurationKey<float3>("Position1", "Spawn Position", () => new float3(0, -0.25f, 2f));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<floatQ> _rotation1 = new ModConfigurationKey<floatQ>("Rotation1", "Spawn Rotation", () => floatQ.Euler(0,0,0));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float> _scale1 = new ModConfigurationKey<float>("Scale1", "Spawn Scale", () => 3);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d3 = new ModConfigurationKey<dummy>(nameGenerator(3), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<Mirror.MirrorType> _MType1 = new ModConfigurationKey<Mirror.MirrorType>("Mtype1", "Mirror Type", () => Mirror.MirrorType.HighQuality);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<UserRoot.UserNode> _RotationSource1 = new ModConfigurationKey<UserRoot.UserNode>("RotationSource1", "Rotation Source", () => UserRoot.UserNode.View);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _grabable1 = new ModConfigurationKey<bool>("Grabable1", "Grabbable", () => false);

//        [AutoRegisterConfigKey]
//        private static readonly ModConfigurationKey<bool> _LockPos1 = new ModConfigurationKey<bool>("LockPosition1", "Lock Position", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Pin1 = new ModConfigurationKey<bool>("Pin1", "Pin to head", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Shadows1 = new ModConfigurationKey<bool>("Shadows1", "Enable Shadows", () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d4 = new ModConfigurationKey<dummy>(nameGenerator(4), "");

        //[AutoRegisterConfigKey]
        //private static readonly ModConfigurationKey<Mirror> _Resoltion1 = new ModConfigurationKey<Mirror>("Grabable1", "Grabable", () => );

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _brightness1 = new ModConfigurationKey<int>("brightness1", "Defult brightness", () => 10);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Opacity1 = new ModConfigurationKey<int>("opacity1", "Defult opacity", () => 0);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Resolution1 = new ModConfigurationKey<int>("Resolution1", "Defult resolution", () => 2560);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d5 = new ModConfigurationKey<dummy>(nameGenerator(5), "________________________________________________________________________________________________");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d15 = new ModConfigurationKey<dummy>(nameGenerator(15), "Celing Mirror");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<string> _mirror2 = new ModConfigurationKey<string>("Mirror2", "Mirror name", () => "Celing Mirror");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float3> _position2 = new ModConfigurationKey<float3>("Position2", "Spawn Position", () => new float3(0, 2, 0));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<floatQ> _rotation2 = new ModConfigurationKey<floatQ>("Rotation2", "Spawn Rotation", () => floatQ.Euler(-90, 180, 180));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float> _scale2 = new ModConfigurationKey<float>("Scale2", "Spawn Scale", () => 3);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d16 = new ModConfigurationKey<dummy>(nameGenerator(16), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<Mirror.MirrorType> _MType2 = new ModConfigurationKey<Mirror.MirrorType>("Mtype2", "Mirror Type", () => Mirror.MirrorType.HighQuality);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<UserRoot.UserNode> _RotationSource2 = new ModConfigurationKey<UserRoot.UserNode>("RotationSource2", "Rotation Source", () => UserRoot.UserNode.GroundProjectedHead);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _grabable2 = new ModConfigurationKey<bool>("Grabable2", "Grabbable", () => false);

 //       [AutoRegisterConfigKey]
 //       private static readonly ModConfigurationKey<bool> _LockPos2 = new ModConfigurationKey<bool>("LockPosition2", "Lock Position", () => false);
 //
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Pin2 = new ModConfigurationKey<bool>("Pin2", "Pin to head", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Shadows2 = new ModConfigurationKey<bool>("Shadows2", "Enable Shadows", () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d17 = new ModConfigurationKey<dummy>(nameGenerator(17), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _brightness2 = new ModConfigurationKey<int>("brightness2", "Defult brightness", () => 10);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Opacity2 = new ModConfigurationKey<int>("opacity2", "Defult opacity", () => 0);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Resolution2 = new ModConfigurationKey<int>("Resolution2", "Defult resolution", () => 2560);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d7 = new ModConfigurationKey<dummy>(nameGenerator(7), "________________________________________________________________________________________________");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d8 = new ModConfigurationKey<dummy>(nameGenerator(8), "Mirror 45");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<string> _mirror3 = new ModConfigurationKey<string>("Mirror3", "Mirror name", () => "Mirror 45");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float3> _position3 = new ModConfigurationKey<float3>("Position3", "Spawn Position", () => new float3(0, 1.5f, 1.5f));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<floatQ> _rotation3 = new ModConfigurationKey<floatQ>("Rotation3", "Spawn Rotation", () => floatQ.Euler(-45, 0, 0));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float> _scale3 = new ModConfigurationKey<float>("Scale3", "Spawn Scale", () => 3);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d18 = new ModConfigurationKey<dummy>(nameGenerator(18), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<Mirror.MirrorType> _MType3 = new ModConfigurationKey<Mirror.MirrorType>("Mtype3", "Mirror Type", () => Mirror.MirrorType.HighQuality);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<UserRoot.UserNode> _RotationSource3 = new ModConfigurationKey<UserRoot.UserNode>("RotationSource3", "Rotation Source", () => UserRoot.UserNode.GroundProjectedHead);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _grabable3 = new ModConfigurationKey<bool>("Grabable3", "Grabbable", () => false);

//        [AutoRegisterConfigKey]
//        private static readonly ModConfigurationKey<bool> _LockPos3 = new ModConfigurationKey<bool>("LockPosition3", "Lock Position", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Pin3 = new ModConfigurationKey<bool>("Pin3", "Pin to head", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Shadows3 = new ModConfigurationKey<bool>("Shadows3", "Enable Shadows", () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d19 = new ModConfigurationKey<dummy>(nameGenerator(19), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _brightness3 = new ModConfigurationKey<int>("brightness3", "Defult brightness", () => 10);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Opacity3 = new ModConfigurationKey<int>("opacity3", "Defult opacity", () => 0);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Resolution3 = new ModConfigurationKey<int>("Resolution3", "Defult resolution", () => 2560);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d9 = new ModConfigurationKey<dummy>(nameGenerator(9), "________________________________________________________________________________________________");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d10 = new ModConfigurationKey<dummy>(nameGenerator(10), "Face Mirror");

//       [AutoRegisterConfigKey]
//       private static readonly ModConfigurationKey<string> _mirror4 = new ModConfigurationKey<string>("Mirror4", "Mirror name", () => "Face Mirror");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float3> _position4 = new ModConfigurationKey<float3>("Position4", "Spawn Position", () => new float3(-0.12f, -0.05f, 0.3f));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<floatQ> _rotation4 = new ModConfigurationKey<floatQ>("Rotation4", "Spawn Rotation", () => floatQ.Euler(0, 0, 0));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float> _scale4 = new ModConfigurationKey<float>("Scale4", "Spawn Scale", () => 0.15f);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d20 = new ModConfigurationKey<dummy>(nameGenerator(20), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<Mirror.MirrorType> _MType4 = new ModConfigurationKey<Mirror.MirrorType>("Mtype4", "Mirror Type", () => Mirror.MirrorType.Camera);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<UserRoot.UserNode> _RotationSource4 = new ModConfigurationKey<UserRoot.UserNode>("RotationSource4", "Rotation Source", () => UserRoot.UserNode.View);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _grabable4 = new ModConfigurationKey<bool>("Grabable4", "Grabbable", () => false);

//        [AutoRegisterConfigKey]
//        private static readonly ModConfigurationKey<bool> _LockPos4 = new ModConfigurationKey<bool>("LockPosition4", "Lock Position", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Pin4 = new ModConfigurationKey<bool>("Pin4", "Pin to head", () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Shadows4 = new ModConfigurationKey<bool>("Shadows4", "Enable Shadows", () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d21 = new ModConfigurationKey<dummy>(nameGenerator(21), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _brightness4 = new ModConfigurationKey<int>("brightness4", "Defult brightness", () => 10);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Opacity4 = new ModConfigurationKey<int>("opacity4", "Defult opacity", () => 0);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Resolution4 = new ModConfigurationKey<int>("Resolution4", "Defult resolution", () => 2560);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d11 = new ModConfigurationKey<dummy>(nameGenerator(11), "________________________________________________________________________________________________");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d12 = new ModConfigurationKey<dummy>(nameGenerator(12), "Extra Mirror");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<string> _mirror5 = new ModConfigurationKey<string>("Mirror5", "Mirror name", () => "Extra Mirror");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float3> _position5 = new ModConfigurationKey<float3>("Position5", "Spawn Position", () => new float3(0, 0, 2));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<floatQ> _rotation5 = new ModConfigurationKey<floatQ>("Rotation5", "Spawn Rotation", () => floatQ.Euler(0, 0, 0));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float> _scale5 = new ModConfigurationKey<float>("Scale5", "Spawn Scale", () => 3);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d24 = new ModConfigurationKey<dummy>(nameGenerator(24), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<Mirror.MirrorType> _MType5 = new ModConfigurationKey<Mirror.MirrorType>("Mtype5", "Mirror Type", () => Mirror.MirrorType.Camera);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<UserRoot.UserNode> _RotationSource5 = new ModConfigurationKey<UserRoot.UserNode>("RotationSource5", "Rotation Source", () => UserRoot.UserNode.View);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _grabable5 = new ModConfigurationKey<bool>("Grabable5", "Grabbable", () => false);

//        [AutoRegisterConfigKey]
//        private static readonly ModConfigurationKey<bool> _LockPos5 = new ModConfigurationKey<bool>("LockPosition5", "Lock Position", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Pin5 = new ModConfigurationKey<bool>("Pin5", "Pin to head", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Shadows5 = new ModConfigurationKey<bool>("Shadows5", "Enable Shadows", () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d25 = new ModConfigurationKey<dummy>(nameGenerator(25), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _brightness5 = new ModConfigurationKey<int>("brightness5", "Defult brightness", () => 10);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Opacity5 = new ModConfigurationKey<int>("opacity5", "Defult opacity", () => 0);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Resolution5 = new ModConfigurationKey<int>("Resolution5", "Defult resolution", () => 2560);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d13 = new ModConfigurationKey<dummy>(nameGenerator(13), "________________________________________________________________________________________________");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d14 = new ModConfigurationKey<dummy>(nameGenerator(14), "FBT Mirror");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float3> _position6 = new ModConfigurationKey<float3>("Position6", "Spawn Position", () => new float3(0, -0.25f, 2.5f));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<floatQ> _rotation6 = new ModConfigurationKey<floatQ>("Rotation6", "Spawn Rotation", () => floatQ.Euler(0, 0, 0));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<float> _scale6 = new ModConfigurationKey<float>("Scale6", "Spawn Scale", () => 3f);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d22 = new ModConfigurationKey<dummy>(nameGenerator(22), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<Mirror.MirrorType> _MType6 = new ModConfigurationKey<Mirror.MirrorType>("Mtype6", "Mirror Type", () => Mirror.MirrorType.HighQuality);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<UserRoot.UserNode> _RotationSource6 = new ModConfigurationKey<UserRoot.UserNode>("RotationSource6", "Rotation Source", () => UserRoot.UserNode.GroundProjectedHead);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _grabable6 = new ModConfigurationKey<bool>("Grabable6", "Grabbable", () => false);

//        [AutoRegisterConfigKey]
//        private static readonly ModConfigurationKey<bool> _LockPos6 = new ModConfigurationKey<bool>("LockPosition6", "Lock Position", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Pin6 = new ModConfigurationKey<bool>("Pin6", "Pin to head", () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _Shadows6 = new ModConfigurationKey<bool>("Shadows6", "Enable Shadows", () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d23 = new ModConfigurationKey<dummy>(nameGenerator(23), "");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _brightness6 = new ModConfigurationKey<int>("brightness6", "Defult brightness", () => 10);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Opacity6 = new ModConfigurationKey<int>("opacity6", "Defult opacity", () => 0);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _Resolution6= new ModConfigurationKey<int>("Resolution6", "Defult resolution", () => 2560);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d26 = new ModConfigurationKey<dummy>(nameGenerator(26), "________________________________________________________________________________________________");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> _d27 = new ModConfigurationKey<dummy>(nameGenerator(27), "Local Light");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<colorX> _LightColor = new ModConfigurationKey<colorX>("lightColor", "Light Colour", () => new colorX(1, 1, 1, 1));

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> _LightRange = new ModConfigurationKey<int>("LightRange", "Light Range", () => 30);

        //disable grab while dash is open
    }
}
