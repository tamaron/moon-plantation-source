using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace U1W.MoonPlant
{
    /// <summary>
    /// プラントのエフェクト管理を行う
    /// </summary>
    public class EffectDistributor : MonoBehaviour
    {
        public EffectState Current = EffectState.None;
        [SerializeField] private GameObject EffectObject;
        private List<GameObject> Effects = new List<GameObject>();
        [SerializeField] private TouchDetector _touchDetector;
        public Material validefect; 
        public Material invalidefect;
        public EventDetector EventDetector;
        public PlantManager PlantManager;

        public void SetEffect((int x, int y) val)
        {
            var pref = Instantiate(EffectObject,
                PlantPostionProv.PositionDictionary[val], Quaternion.identity);

            Effects.Add(pref);
        }
        
        private void Start()
        {
            CompositeDisposable disposables = new CompositeDisposable();
            this.ObserveEveryValueChanged(_ => Current).Subscribe(state =>
            {
                Debug.Log(state);
                disposables.Dispose();
                disposables = new CompositeDisposable();
                switch (state)
                {
                    case EffectState.Single:
                        disposables.Add(
                            _touchDetector.TouceSubjectSingle.Subscribe(val =>
                            {
                                Effects.Clear();

                                foreach (var o in Effects) Destroy(o);
                                Effects.Add(Instantiate(EffectObject, PlantPostionProv.PositionDictionary[val], Quaternion.identity));
                            })
                            );

                        break;
                    
                    case EffectState.Line:
                        disposables.Add(
                            _touchDetector.TouceSubjectSingle.Subscribe(val =>
                            {
                                foreach (var o in Effects) Destroy(o);
                                Effects.Clear();


                                SetEffect(val);
                                for (int i = 0; i < 4; i++)
                                {
                                    if(val.x == i) continue;
                                    SetEffect((i, val.y));
                                }
                                for (int i = 0; i < 4; i++)
                                {
                                    if(val.y == i) continue;
                                    SetEffect((val.x, i));
                                }
                            })
                        );
                        break;
                    case EffectState.All:
                        disposables.Add(
                            _touchDetector.TouceSubjectSingle.Subscribe(val =>
                            {
                                foreach (var o in Effects) Destroy(o);
                                Effects.Clear();
                                for (int i = 0; i < 4; i++)
                                {
                                    for(int j = 0; j < 4; j++)
                                        Effects.Add(Instantiate(EffectObject, PlantPostionProv.PositionDictionary[(i, j)], Quaternion.identity));
                                }
                            })
                        );
                        break;
                    
                    case EffectState.None:
                        foreach (var o in Effects) Destroy(o);
                        Effects.Clear();

                        break;
                }
            }).AddTo(this);
        }

        public void DoEffect((int x, int y) point)
        {
            // test
        }
    }

    public enum EffectState
    {
        None,
        Single,
        Line,
        All
    }
}

