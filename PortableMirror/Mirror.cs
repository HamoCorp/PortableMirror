using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static FrooxEngine.CubemapCreator;
using PortableMirror;

namespace PortableMirror {
    internal class Mirror {

        public Mirror(string name) {
            _name = name;
            
            //buttenEvents.
        }

        //public void setPrivateValue(int brightness, int opacity) {
        //    _brightness = brightness;
        //    _Opacity = opacity;
        //}
        public void upDatePrivateValues(string name, float3 position, floatQ rotation, float scale, MirrorType mType, bool grabable, bool pin, bool Lock, bool shadows, int brightness, int opacity, int resolution, UserRoot.UserNode rotSource) {
            _name = name;
            _Position = position;
            _Roatation = rotation;
            _Scale = new float3(scale, scale, scale);
            _MType = mType;
            _Grabable = grabable;
            _pinToHead = pin;
            _LockPosition = Lock;
            _shadows = shadows;
            _brightness = brightness;
            _Opacity = opacity;
            _rotationSource = rotSource;
            _Resolution = resolution;
        }

        public enum MirrorType {
            HighQuality,
            //HighQualityCutout,
            HighQualityOpaque,
            
            Camera,
            CameraSkyBox,
            CameraOpaque,
        }
        public void Enabled_OnValueChange(SyncField<bool> syncField) {
            if (syncField.Value == _enabled)
                return;
            _enabled = syncField.Value;

            if(_enabled == false && _mirrorSlot != null) {
                _mirrorSlot.Destroy();
                return;
            }

        }

        public void OnCommonUpdate(Slot UserspaceParrent) {
            if (_enabled) {
                if (_updateMirrorTpye == true) {
                    _updateMirrorTpye = false;
                    if(_mirrorSlot != null) {
                        _mirrorSlot.Destroy();
                        _mirrorSlot = null;
                    }
                }
                if (_mirrorSlot == null) {
                    _mirrorSlot = SetUpMirror(UserspaceParrent);
                }
                _collider.Enabled = _Grabable;

                if (_mirrorSlot.LocalPosition != new float3(0, 0, 0)) {
                    _PosAtUs.Enabled = _pinToHead;
                    if(_pinToHead == true) {
                        _PosAtUs.RotationDrive.Value = _mirrorSlot.Rotation_Field.ReferenceID;
                    }
                    else {
                        _PosAtUs.RotationDrive.Value = RefID.Null;
                        _mirrorSlot.LocalRotation = floatQ.Euler(_mirrorSlot.LocalRotation.EulerAngles.x, _mirrorSlot.LocalRotation.EulerAngles.y, 0);
                    }
                }

                if (_TextChange == true) {
                    
                    if(_smoval.TargetValue.Value == 0) {
                        _smoval.TargetValue.Value = 1;
                    }
                    else {
                        _smoval.TargetValue.Value = 0;
                    }

                    _textRenderer.Text.Value = _Text;
                    _TextChange = false;
                }

                float b = ((float)_brightness) / 10;
                float O = ((float)_Opacity) / 10;
                _rtp.Size.Value = new int2(_Resolution, _Resolution);
                if (_updateScale == true) {
                    _mirror.LocalScale = _Scale;
                    _updateScale = false;
                }
                colorX col = new colorX(b, b, b, O);

                switch (_MType) {
                    case MirrorType.Camera:
                    case MirrorType.CameraSkyBox:
                        _camMat.TintColor.Value = col;
                        _cam.RenderShadows.Value = _shadows;
                        break;
                    case MirrorType.CameraOpaque:
                        _camMat.TintColor.Value = new colorX(1, 1, 1, 1);
                        _cam.RenderShadows.Value = _shadows;
                        break;
                    case MirrorType.HighQualityOpaque:
                        _reflectMat.TintColor.Value = new colorX(1, 1, 1, 1);
                        _camPort.DisableShadows.Value = !_shadows;
                        break;
                    case MirrorType.HighQuality:
                        _reflectMat.TintColor.Value = col;
                        _camPort.DisableShadows.Value = !_shadows;
                        break;
                }

            } else {
               if(_mirrorSlot != null) {
                   _mirrorSlot.Destroy();
                    _mirrorSlot = null;
               }
           }


        }

        public enum PositonType {
            Front,
            FrontLocked,
            Top,
            Head
        }

//        private static float3 offsetPositionMath(User user, float offsetDistance, MirrorType position) {
//
//            float3 vect;
//            if (position == PositonType.Front) {
//                vect = new float3(user.LocalUserRoot.HeadFacingDirection.x * offsetDistance, user.LocalUserRoot.HeadFacingDirection.y * offsetDistance, user.LocalUserRoot.HeadFacingDirection.z * offsetDistance);
//                float3 mull = new float3(offsetDistance, offsetDistance, offsetDistance);
//                return vect + user.LocalUserRoot.HeadPosition;
//
//            } else if(position == PositonType.Top) {
//                vect = new float3(user.LocalUserRoot.HeadFacingDirection.x, user.LocalUserRoot.HeadFacingDirection.y * offsetDistance, user.LocalUserRoot.HeadFacingDirection.z);
//                return (user.LocalUserRoot.HeadFacingDirection + user.LocalUserRoot.HeadPosition) * new float3(1, offsetDistance, 1);
//            }
//            else {
//
//                vect = new float3(user.LocalUserRoot.HeadFacingDirection.x, user.LocalUserRoot.HeadFacingDirection.y, user.LocalUserRoot.HeadFacingDirection.z);
//                return (user.LocalUserRoot.HeadFacingDirection * offsetDistance) + user.LocalUserRoot.HeadPosition;
//            }
//            
//            //return (vect * offsetDistance) + user.LocalUserRoot.HeadPosition;
//        }

        public Slot SetUpMirror(Slot parent) {

            Slot PositionSlot = parent.AddSlot(_name, false);
            _PosAtUs = PositionSlot.AttachComponent<PositionAtUser>();
            _PosAtUs.TargetUser.Value = parent.LocalUser.ReferenceID;
            _PosAtUs.PositionSource.Value = UserRoot.UserNode.Head;
            _PosAtUs.RotationSource.Value = _rotationSource;

            switch (_MType) {
                case MirrorType.Camera:
                case MirrorType.CameraOpaque:
                case MirrorType.CameraSkyBox:
                    _mirror = addMirrorLQ(PositionSlot);
                    break;
                case MirrorType.HighQualityOpaque:
                case MirrorType.HighQuality:
                    _mirror = addMirrorHQ(PositionSlot);
                    break;
            }

            _TextSlot = MirrorText(_mirror);
            return PositionSlot;
        }

        public Slot addMirrorHQ(Slot parent) {

            Slot mSlot = parent.AddSlot("MirrorHQ", false);
            Grabbable grab = mSlot.AttachComponent<Grabbable>();
            mSlot.GlobalPosition = _Position;
            floatQ rotrot = mSlot.Rotation_Field.Value * _Roatation;
            mSlot.GlobalRotation = rotrot;

            mSlot.GlobalScale = _Scale;
            QuadMesh qmesh = mSlot.AttachComponent<QuadMesh>();
            MeshRenderer mR = mSlot.AttachComponent<MeshRenderer>();
            grab.Scalable.Value = true;
            _camPort = mSlot.AttachComponent<CameraPortal>();
            _rtp = mSlot.AttachComponent<RenderTextureProvider>();
            _rtp.Size.Value = new int2(_Resolution, _Resolution);
            _reflectMat = mSlot.AttachComponent<ReflectionMaterial>();
            float b = ((float)_brightness) / 10;
            float O = ((float)_Opacity) / 10;
            _reflectMat.TintColor.Value = new colorX(b, b, b, O, ColorProfile.Linear);
            _reflectMat.ReflectionTexture.Value = _rtp.ReferenceID;
            MeshCollider collider = mSlot.AttachComponent<MeshCollider>();
            mSlot.AttachComponent<DuplicateBlock>();
            mSlot.AttachComponent<DestroyBlock>();

            mR.Mesh.Value = qmesh.ReferenceID;
            collider.Mesh.Value = qmesh.ReferenceID;
            collider.Sidedness.Value = MeshColliderSidedness.DualSided;
            mR.Material.Value = _reflectMat.ReferenceID;
            _reflectMat.Sidedness.Value = Sidedness.Front;

            _camPort.Renderer.Value = mR.ReferenceID;
            _camPort.ReflectionTexture.Value = _rtp.ReferenceID;

            _collider = collider;

            switch (_MType) {
                case MirrorType.HighQualityOpaque:
                    _reflectMat.BlendMode.Value = BlendMode.Opaque;
                    break;
                case MirrorType.HighQuality:
                    _reflectMat.BlendMode.Value = BlendMode.Transparent;
                    break;
            }

            return mSlot;
        }

        public Slot addMirrorLQ(Slot parent) {


            Slot mSlot = parent.AddSlot("MirrorLQ", false);

            Grabbable grab = mSlot.AttachComponent<Grabbable>();
            mSlot.GlobalPosition = _Position;
            floatQ rotrot = mSlot.Rotation_Field.Value * _Roatation;
            mSlot.GlobalRotation = rotrot;

            mSlot.GlobalScale = _Scale;
            QuadMesh qmesh = mSlot.AttachComponent<QuadMesh>();
            MeshRenderer mR = mSlot.AttachComponent<MeshRenderer>();
            grab.Scalable.Value = true;
            Slot camSlot;
            if (_name != "Face Mirror") {
                camSlot = mSlot.AddSlot("Camera", false);
                
            }
            else {
                Slot posSlot = mSlot.AddSlot("Camera Rot", false);
                PositionAtUser pos = posSlot.AttachComponent<PositionAtUser>();
                pos.TargetUser.Value = parent.LocalUser.ReferenceID;
                pos.PositionSource.Value = UserRoot.UserNode.View;
                pos.RotationSource.Value = UserRoot.UserNode.View;

                camSlot = posSlot.AddSlot("Camera", false);
                camSlot.LocalPosition = new float3(0, 0, 1.3f);
            }
            camSlot.LocalRotation = floatQ.Euler(0, 180, 0);
            _cam = camSlot.AttachComponent<Camera>();
            _rtp = mSlot.AttachComponent<RenderTextureProvider>();
            _rtp.Size.Value = new int2(_Resolution, _Resolution);
            _camMat = mSlot.AttachComponent<UnlitMaterial>();
            float b = ((float)_brightness) / 10;
            float O = ((float)_Opacity) / 10;
            _camMat.TintColor.Value = new colorX(b, b, b, O, ColorProfile.Linear);
            _camMat.Texture.Value = _rtp.ReferenceID;
            qmesh.UVOffset.Value = new float2(1, 0);
            qmesh.UVScale.Value = new float2(-1, 1);

            MeshCollider collider = mSlot.AttachComponent<MeshCollider>();
            mSlot.AttachComponent<DuplicateBlock>();
            mSlot.AttachComponent<DestroyBlock>();

            mR.Mesh.Value = qmesh.ReferenceID;
            collider.Mesh.Value = qmesh.ReferenceID;
            collider.Sidedness.Value = MeshColliderSidedness.DualSided;
            mR.Material.Value = _camMat.ReferenceID;
            _camMat.Sidedness.Value = Sidedness.Front;

            _cam.RenderTexture.Value = _rtp.ReferenceID;
            _cam.Projection.Value = CameraProjection.Perspective;
            _cam.OrthographicSize.Value = 0.1f;
            _cam.FieldOfView.Value = 90;
            _cam.MotionBlur.Value = false;
            _cam.Clear.Value = CameraClearMode.Color;
            _cam.ForwardOnly.Value = true;

            switch (_MType) {
                case MirrorType.Camera:
                    _cam.Clear.Value = CameraClearMode.Color;
                    _cam.FarClipping.Value = 4;
                    _camMat.BlendMode.Value = BlendMode.Transparent;
                    if (_name == "Face Mirror") {
                        _cam.FarClipping.Value = 0.5f;
                    }
                        break;
                case MirrorType.CameraSkyBox:
                    _cam.Clear.Value = CameraClearMode.Skybox;
                    _cam.FarClipping.Value = 4096;
                    _camMat.BlendMode.Value = BlendMode.Transparent;
                    break;
                case MirrorType.CameraOpaque:
                    _cam.Clear.Value = CameraClearMode.Skybox;
                    _cam.FarClipping.Value = 4096;
                    _camMat.BlendMode.Value = BlendMode.Opaque;
                    break;
            }

            //_cam.SelectiveRender.Add(parent.LocalUserRoot.Slot);

            _collider = collider;
            
            return mSlot;
        }

        public Slot MirrorText(Slot Mirror) {
            Slot texS = Mirror.AddSlot("Text", false);
            texS.LocalPosition = new float3(0, 0, -0.01f);
            _textRenderer = texS.AttachComponent<TextRenderer>();
            _textRenderer.Size.Value = 0.7f;
            _smoval = texS.AttachComponent<SmoothValue<float>>();
            _smoval.Speed.Value = 2;
            ValueGradientDriver<bool> vgd = texS.AttachComponent<ValueGradientDriver<bool>>();
            vgd.AddPoint(0, false);
            vgd.AddPoint(0.02f, true);
            vgd.AddPoint(0.98f, true);
            vgd.AddPoint(1, false);

            vgd.Target.Value = _textRenderer.EnabledField.ReferenceID;
            _smoval.Value.Value = vgd.Progress.ReferenceID;

            return texS;
        }

        public void EnableToggle(IButton b, ButtonEventData d) {
            if (_enabled == false) {
                _enabled = true;
            }
            else _enabled = false;
        }

        public void GrabableToggle(IButton b, ButtonEventData d) {
            if (_Grabable == false) {
                _Grabable = true;
                UpdateText("Grabable Enabled");
            }
            else { 
                _Grabable = false;
                UpdateText("Grabable Disabled");
            }
        }

        public void Quality(IButton b, ButtonEventData d) {
            
        }

        public void resetSettings(IButton b, ButtonEventData d) {
            int i = 0;
            PortableMirror.upDateMirrorValues(i);
            //            foreach (var m in MirrorsList) {
            //                if (m == this) {
            //                    PortableMirror.upDateMirrorValues(i);
            //                    _updateMirrorTpye = true;
            //                    _updateScale = true;
            //                    UpdateText("Mirror Settings Reset");
            //                    i++;
            //                }
            //            }
            foreach (var m in MirrorsList) {
                m.setEnabled(false);
            }
        }

        public void Lock(IButton b, ButtonEventData d) {
            
        }

        public void Shadows(IButton b, ButtonEventData d) {
            if (_shadows == false) {
                _shadows = true;
                UpdateText("Shadows Enabled");
            }
            else {
                _shadows = false;
                UpdateText("Shadows Disabled");
            }
        }

        public void Pin(IButton b, ButtonEventData d) {
            if (_pinToHead == false) {
                _pinToHead = true;
                UpdateText("Mirror pined to view");
            }
            else {
                _pinToHead = false;
                UpdateText("Mirror unpined");
            }
        }

        public void BrightnessUp(IButton b, ButtonEventData d) {
            if(_brightness < 10) {
                _brightness += 1;
            }

            UpdateText("Brightness: " + _brightness.ToString());
        }

        public void BrightnessDown(IButton b, ButtonEventData d) {
            if (_brightness > 0) {
                _brightness -= 1;
            }

            UpdateText("Brightness: " + _brightness.ToString());
        }

        public void OpacityUp(IButton b, ButtonEventData d) {
            if (_Opacity < 10) {
                _Opacity += 1;
            }

            UpdateText("Opacity: " + _Opacity.ToString());
        }

        public void OpacityDown(IButton b, ButtonEventData d) {
            if (_Opacity > 0) {
                _Opacity -= 1;
            }
            UpdateText("Opacity: " + _Opacity.ToString());
        }

        public void ResolutionUp(IButton b, ButtonEventData d) {
            if (_Resolution < 9216) {
                _Resolution += 512;
                UpdateText("Resolution: " + _Resolution.ToString());
            }
            else {
                UpdateText("NO NO TO MUCH");
            }
        }

        public void ResolutionDown(IButton b, ButtonEventData d) {
            if (_Resolution > 512) {
                _Resolution -= 512;
                UpdateText("Resolution: " + _Resolution.ToString());
            }
            else {
                UpdateText("Resolution to small");
            }
        }

        public void ScaleUp(IButton b, ButtonEventData d) {
            if (_mirror != null && !_mirror.IsDestroyed) {
                _Scale = _mirror.LocalScale;

                if (_Scale.X < 10000) {
                    _Scale = new float3(_Scale.X + 0.3f, _Scale.Y + 0.3f, _Scale.Z + 0.3f);
                    UpdateText("Scale: " + _Scale.x.ToString());
                    _updateScale = true;
                }
                else {
                    UpdateText("TO BIG");
                }
            }
        }

        public void ScaleDown(IButton b, ButtonEventData d) {
            if (_mirror != null && !_mirror.IsDestroyed) {
                _Scale = _mirror.LocalScale;

                if (_Scale.X > 0.6) {
                    _Scale = new float3(_Scale.X - 0.3f, _Scale.Y - 0.3f, _Scale.Z - 0.3f);
                    UpdateText("Scale: " + _Scale.x.ToString());
                    _updateScale = true;
                }
                else {
                    UpdateText("TO SMALL 🐜");
                }
                
            }
        }

        public void MirrorHQOpaque(IButton b, ButtonEventData d) {
            _MType = MirrorType.HighQualityOpaque;
            _updateMirrorTpye = true;
        }

        public void MirrorHQ(IButton b, ButtonEventData d) {
            _MType = MirrorType.HighQuality;
            _updateMirrorTpye = true;
        }

        public void MirrorLQ(IButton b, ButtonEventData d) {
            _MType = MirrorType.Camera;
            _updateMirrorTpye = true;
        }

        public void MirrorLQSky(IButton b, ButtonEventData d) {
            _MType = MirrorType.CameraSkyBox;
            _updateMirrorTpye = true;
        }

        public void MirrorLQOpaque(IButton b, ButtonEventData d) {
            _MType = MirrorType.CameraOpaque;
            _updateMirrorTpye = true;
        }

        private void UpdateText(string text) {
            _Text = text;

            if (_TextChange == false) {
                _TextChange = true;
            }
            else _TextChange = false;

        }

        public static List<Mirror> MirrorsList = new List<Mirror>();
        public static PersonalLight personalLight = new PersonalLight();

        public void setEnabled(bool enabled) {
            _enabled = enabled;
        }

        public Slot GetMirrorSlot() {
            return _mirrorSlot;
        }

        public string getName() { return _name; }

        public enum MirrorValues {
            none,
            Enabled,
            Grabable,
            DisableShadows,
            Pin,
            Lock,
            LightON,
            LightGrab,
            LightVisual
        }
        public bool getValue(MirrorValues value = MirrorValues.none) {

            switch (value) {
                case MirrorValues.Enabled:
                    return _enabled;

                case MirrorValues.Grabable:
                    return _Grabable;

                case MirrorValues.DisableShadows:
                    return _shadows;

                case MirrorValues.Pin:
                    return _pinToHead;

                case MirrorValues.Lock:
                    return _LockPosition;

                case MirrorValues.LightON:
                    return personalLight.getEnabled();

                case MirrorValues.LightGrab:
                    return personalLight.getGrab();

                case MirrorValues.LightVisual:
                    return personalLight.getVisual();


                default:
                case MirrorValues.none:
                    return false;
            }
        }


        string _name;
        bool _enabled = false;
        bool _updateMirrorTpye = false;
        bool _updateScale = false;
        bool _Grabable = false;
        int _brightness = 10;
        int _Opacity = 0;
        int _Resolution = 2560;
        Camera _cam;
        CameraPortal _camPort;

        MeshCollider _collider;
        PositionAtUser _PosAtUs;// addd this and clean this up
        bool _LockPosition = false;
        bool _pinToHead = false;
        bool _shadows = true;

        SmoothValue<float> _smoval;
        string _Text;
        bool _TextChange = false;
        float3 _Position = new float3(0, 0, 3);
        float3 _Scale = new float3(3f, 3f, 3f);
        floatQ _Roatation = new floatQ(1, 0, 0, 0);
        MirrorType _MType = MirrorType.HighQuality;
        UserRoot.UserNode _rotationSource;
        Slot _mirrorSlot;
        Slot _mirror;
        Slot _TextSlot;

        ReflectionMaterial _reflectMat;
        UnlitMaterial _camMat;
        RenderTextureProvider _rtp;
        TextRenderer _textRenderer;

    }
}