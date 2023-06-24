using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ingame;
using UnityEngine;
using UnityEngine.UI;

namespace U1W.MoonPlant
{
    public class PlantImage : MonoBehaviour
    {
        [SerializeField] private PlantManager _plantManager;

        [SerializeField] private Sprite Plant000;
        [SerializeField] private Sprite Plant001;
        [SerializeField] private Sprite Plant002;
        [SerializeField] private Sprite Plant003;
        [SerializeField] private Sprite Plant010;
        [SerializeField] private Sprite Plant011;
        [SerializeField] private Sprite Plant012;
        [SerializeField] private Sprite Plant013;
        [SerializeField] private Sprite Plant020;
        [SerializeField] private Sprite Plant021;
        [SerializeField] private Sprite Plant022;
        [SerializeField] private Sprite Plant023;

        [SerializeField] private Sprite Plant100;
        [SerializeField] private Sprite Plant101;
        [SerializeField] private Sprite Plant102;
        [SerializeField] private Sprite Plant103;
        [SerializeField] private Sprite Plant110;
        [SerializeField] private Sprite Plant111;
        [SerializeField] private Sprite Plant112;
        [SerializeField] private Sprite Plant113;
        [SerializeField] private Sprite Plant120;
        [SerializeField] private Sprite Plant121;
        [SerializeField] private Sprite Plant122;
        [SerializeField] private Sprite Plant123;

        [SerializeField] private Sprite Plant200;
        [SerializeField] private Sprite Plant201;
        [SerializeField] private Sprite Plant202;
        [SerializeField] private Sprite Plant203;
        [SerializeField] private Sprite Plant210;
        [SerializeField] private Sprite Plant211;
        [SerializeField] private Sprite Plant212;
        [SerializeField] private Sprite Plant213;
        [SerializeField] private Sprite Plant220;
        [SerializeField] private Sprite Plant221;
        [SerializeField] private Sprite Plant222;
        [SerializeField] private Sprite Plant223;

        private Dictionary<(int glowth, int color, int moon), Sprite> PlantDict;

        void Start()
        {
            PlantDict = new Dictionary<(int glowth, int color, int moon), Sprite>
            {
                { (0, 0, 0), Plant000 },
                { (0, 0, 1), Plant001 },
                { (0, 0, 2), Plant002 },
                { (0, 0, 3), Plant003 },
                { (0, 1, 0), Plant010 },
                { (0, 1, 1), Plant011 },
                { (0, 1, 2), Plant012 },
                { (0, 1, 3), Plant013 },
                { (0, 2, 0), Plant020 },
                { (0, 2, 1), Plant021 },
                { (0, 2, 2), Plant022 },
                { (0, 2, 3), Plant023 },

                { (1, 0, 0), Plant100 },
                { (1, 0, 1), Plant101 },
                { (1, 0, 2), Plant102 },
                { (1, 0, 3), Plant103 },
                { (1, 1, 0), Plant110 },
                { (1, 1, 1), Plant111 },
                { (1, 1, 2), Plant112 },
                { (1, 1, 3), Plant113 },
                { (1, 2, 0), Plant120 },
                { (1, 2, 1), Plant121 },
                { (1, 2, 2), Plant122 },
                { (1, 2, 3), Plant123 },

                { (2, 0, 0), Plant200 },
                { (2, 0, 1), Plant201 },
                { (2, 0, 2), Plant202 },
                { (2, 0, 3), Plant203 },
                { (2, 1, 0), Plant210 },
                { (2, 1, 1), Plant211 },
                { (2, 1, 2), Plant212 },
                { (2, 1, 3), Plant213 },
                { (2, 2, 0), Plant220 },
                { (2, 2, 1), Plant221 },
                { (2, 2, 2), Plant222 },
                { (2, 2, 3), Plant223 },
            };
        }

        public async UniTask SetImage()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var spriteRenderer = GameObject.Find($"plant{i}{j}").GetComponent<SpriteRenderer>();
                    var plant = _plantManager.Plants[(i, j)];
                    DOTween.To(
                        () => 1.0f,
                        x =>
                        {
                            var color = spriteRenderer.color;
                            color.a = x;
                            spriteRenderer.color = color;
                        },
                        0f,
                        0.3f
                    );
                }
            }

            await UniTask.Delay(500);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var spriteRenderer = GameObject.Find($"plant{i}{j}").GetComponent<SpriteRenderer>();
                    var plant = _plantManager.Plants[(i, j)];
                    spriteRenderer.sprite = PlantDict[(plant.Growth, plant.Color, plant.Moon)];
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var spriteRenderer = GameObject.Find($"plant{i}{j}").GetComponent<SpriteRenderer>();
                    var plant = _plantManager.Plants[(i, j)];
                    DOTween.To(
                        () => 0f,
                        x =>
                        {
                            var color = spriteRenderer.color;
                            color.a = x;
                            spriteRenderer.color = color;
                        },
                        1f,
                        0.3f
                    );
                }
            }
        }
    }
}