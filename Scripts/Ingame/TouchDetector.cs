using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace U1W.MoonPlant
{
    /// <summary>
    /// マウスクリックなどのイベントを管理する
    /// </summary>
    public class TouchDetector : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        public IObservable<(int x, int y)> OnTouchedSingle => TouceSubjectSingle;
        public Subject<(int x, int y)> TouceSubjectSingle = new Subject<(int x, int y)>();
        public IObservable<int> OnTouchedX => TouceSubjectX;
        public Subject<int> TouceSubjectX = new Subject<int>();
        public IObservable<int> OnTouchedY => TouceSubjectY;
        public Subject<int> TouceSubjectY = new Subject<int>();
        
        public Subject<(int x, int y)> PushSubjectSingle = new Subject<(int x, int y)>();
        public Subject<int> PushSubjectX = new Subject<int>();
        public Subject<int> PushSubjectY = new Subject<int>();
        private bool clicked;
        
        Camera mainCamera;
        RaycastHit hit;
        GameObject targetObject;
        void Start()
        {
            mainCamera = _camera;
        }
        void Update()
        {
            clicked = false;
            if (Input.GetMouseButtonDown(0)) clicked = true;
            CastRay();
            LookUpTargetObject();   
        }

        void CastRay()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                targetObject = hit.collider.gameObject;
            }
            else
            {
                targetObject = null;
            }
        }
        void LookUpTargetObject()
        {
            if(targetObject == null)
            {
                return;
            }

            var comp = targetObject.GetComponent<PlantCollider>();
            (int x, int y) temp = (comp.x, comp.y);
            if (clicked)
            {
                PushSubjectSingle.OnNext(temp);
                PushSubjectX.OnNext(temp.x);
                PushSubjectY.OnNext(temp.y);
            }
            TouceSubjectSingle.OnNext(temp);
            TouceSubjectX.OnNext(temp.x);
            TouceSubjectY.OnNext(temp.y);
        }
    }
}