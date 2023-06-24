using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace U1W.MoonPlant
{
    /// <summary>
    /// 月のアニメーションを制御する
    /// </summary>
    public class MoonAnim : MonoBehaviour
    {
        void Start()
        {
            transform.DOLocalRotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart);
        }
    }
}