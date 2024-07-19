using Elements.Core;
using FrooxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static PortableMirror.Mirror;

namespace PortableMirror {
    internal class PersonalLight {

        public PersonalLight() {
        }

        private static float3 offsetPositionMath(User user, float offsetDistance) {

            float3 vect;
            vect = new float3(user.LocalUserRoot.HeadFacingDirection.x * offsetDistance, user.LocalUserRoot.HeadFacingDirection.y * offsetDistance, user.LocalUserRoot.HeadFacingDirection.z * offsetDistance);
            float3 mull = new float3(offsetDistance, offsetDistance, offsetDistance);
            return vect + user.LocalUserRoot.HeadPosition;
            
            //return (vect * offsetDistance) + user.LocalUserRoot.HeadPosition;
        }

        public Slot SpawnLight(Slot parent) {

            Slot L = parent.AddSlot("LocalLight", false);
            L.GlobalPosition = parent.LocalUserRoot.HeadPosition;
            L.GlobalRotation = parent.LocalUserRoot.HeadRotation;
            _LocalLight = L.AddSlot("LocalLight", false);
            _LocalLight.LocalPosition = new float3(0, 0, 1);
            _LocalLight.SetParent(parent);
            L.Destroy();
            _grab = _LocalLight.AttachComponent<Grabbable>();
            CrossMesh bm = _LocalLight.AttachComponent<CrossMesh>();
            _mr = _LocalLight.AttachComponent<MeshRenderer>();

            MeshCollider mc = _LocalLight.AttachComponent<MeshCollider>();
            PBS_Metallic mat = _LocalLight.AttachComponent<PBS_Metallic>();
            _Light = _LocalLight.AttachComponent<Light>();
            mc.Mesh.Value = bm.ReferenceID;

            bm.Size.Value = 0.3f;
            _mr.Mesh.Value = bm.ReferenceID;
            _mr.Materials.Add(mat);
            _mr.ShadowCastMode.Value = ShadowCastMode.Off;

            StaticTexture2D statex = _LocalLight.AttachComponent<StaticTexture2D>();
            _LocalLight.AttachComponent<DuplicateBlock>();
            _LocalLight.AttachComponent<DestroyBlock>();
            statex.URL.Value = new Uri("resdb:///45952b461117e6e4e9258981d4f934918d2dfebf7ca130901ee6b07705eb98ee.png");
            mat.AlbedoTexture.Value = statex.ReferenceID;
            mat.EmissiveMap.Value = statex.ReferenceID;
            mat.EmissiveColor.Value = new colorX(0.8f, 0.8f, 0.8f);
            _Light.Range.Value = _range;

            return _LocalLight;
        }

        public void OnCommonUpdate(Slot parent) {
            if (_enabled) {

                if (_LocalLight == null) {
                    _LocalLight = SpawnLight(parent);
                }

                _grab.Enabled = _grabable;
                _mr.Enabled = _visual;


            }
            else {
                if (_LocalLight != null) {
                    _LocalLight.Destroy();
                    _LocalLight = null;
                }
            }

        }

        public void EnableToggle(IButton b, ButtonEventData d) {
            if (_enabled == false) {
                _enabled = true;
            }
            else _enabled = false;
        }

        public void GrabableToggle(IButton b, ButtonEventData d) {
            if (_grabable == false) {
                _grabable = true;
            }
            else _grabable = false;
        }

        public void VisualToggle(IButton b, ButtonEventData d) {
            if (_visual == false) {
                _visual = true;
            }
            else _visual = false;
        }

        public void resetSettings(IButton b, ButtonEventData d) {
            PortableMirror.upDateMirrorValues(6);

        }

        public bool getEnabled() { return _enabled; }
        public bool getGrab() { return _grabable; }

        public bool getVisual() { return _visual; }
        public Slot getLightSlot() { return _LocalLight; }

        public void setValues(colorX col, float range) {
            _color = col;
            _range = range;
            //_visual = vis;
        }

        bool _enabled = false;
        bool _grabable = true;
        bool _visual = true;
        float _range = 30;

        colorX _color;

        Slot _LocalLight;
        Grabbable _grab;
        Light _Light;
        MeshRenderer _mr;
    }
}
