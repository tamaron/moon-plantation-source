using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
namespace U1W.MoonPlant
{
    public class PlantManager : MonoBehaviour
    {
        [SerializeField] private AnimationManager AnimationManager;
        public Dictionary<(int x, int y), Plant> Plants = new Dictionary<(int x, int y), Plant>
        {
            {(0, 0), new Plant()},
            {(1, 0), new Plant()},
            {(2, 0), new Plant()},
            {(3, 0), new Plant()},
            {(0, 1), new Plant()},
            {(1, 1), new Plant()},
            {(2, 1), new Plant()},
            {(3, 1), new Plant()},
            {(0, 2), new Plant()},
            {(1, 2), new Plant()},
            {(2, 2), new Plant()},
            {(3, 2), new Plant()},
            {(0, 3), new Plant()},
            {(1, 3), new Plant()},
            {(2, 3), new Plant()},
            {(3, 3), new Plant()},
        };

        public void InitializePlants()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    SingleInitComp((i, j));
                }
            }
            
        }

        public void DayStep()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Plants[(i, j)].Moon = (Plants[(i, j)].Moon + 1) % Plant.MOON;
                    if(Random.Range(0f, 1f) < 0.2f && Plants[(i, j)].Growth == 0)  Plants[(i, j)].Growth = Math.Min(Plants[(i, j)].Growth + 1, Plant.GROWTH);
                }
            }
        }

        public void SingleInit((int x, int y) point)
        {
            if (Random.Range(0f, 1f) < 0.2f)
            {
                Plants[point].Growth = 1;
            }
            else
            {
                Plants[point].Growth = 0;
            }
            Plants[point].Moon = Random.Range(0, Plant.MOON);
            Plants[point].Color = Random.Range(0, Plant.COLOR);
        }
        
        public void SingleInitComp((int x, int y) point)
        {
            Plants[point].Growth = 0;
            Plants[point].Moon = Random.Range(0, Plant.MOON);
            Plants[point].Color = Random.Range(0, Plant.COLOR);
        }

        public void Step()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Random.Range(0f, 1f) < 0.2f)
                    {
                        Plants[(i, j)].Growth = Math.Max(Plants[(i, j)].Growth + 1, Plant.GROWTH);
                    }
                    Plants[(i, j)].Moon = (Plants[(i, j)].Moon + 1) % Plant.MOON;
                }
            }
        }
        
        public int Cut((int x, int y) point)
        {
            var list = new List<Plant>();
            if (Plants[point].Growth >= 2)
            {
                list.Add(new Plant
                    {
                        Growth = Plants[point].Growth,
                        Color = Plants[point].Color,
                        Moon = Plants[point].Moon,
                    }
                );
            }

            SingleInitComp(point);
            for (int i = 0; i < 4; i++)
            {
                if (point.y == i) continue;
                if (Plants[(point.x, i)].Growth >= 2)
                {
                    list.Add(new Plant
                        {
                            Growth = Plants[(point.x, i)].Growth,
                            Color = Plants[(point.x, i)].Color,
                            Moon = Plants[(point.x, i)].Moon,
                        }
                    );
                }

                SingleInitComp((point.x, i));
            }
            
            for (int i = 0; i < 4; i++)
            {
                if (point.x == i) continue;
                if (Plants[(i, point.y)].Growth >= 2)
                {
                    list.Add(new Plant
                        {
                            Growth = Plants[(i, point.y)].Growth,
                            Color = Plants[(i, point.y)].Color,
                            Moon = Plants[(i, point.y)].Moon,
                        }
                    );
                }
                SingleInitComp((i, point.y));
            }

            return ScoreCul(list);
        }

        public int ScoreCul(List<Plant> plants)
        {
            if (plants.Count > 7) throw new Exception();
            foreach (var plant in plants)
            {
                Debug.Log((plant.Growth, plant.Color, plant.Moon));
            }
            int bas = 0;
            
            
            // color
            var colorScoreTable = new List<int>
            {
                0, 100, 200, 400, 800, 1600, 3200, 6400
            };
            
            var colordict = new Dictionary<int, int>
            {
                {0, 0},
                {1, 0},
                {2, 0},
            };
            foreach (var plant in plants)
            {
                colordict[plant.Color]++;
            }

            bas += colorScoreTable[colordict.Values.Max()];
            
            var times = 0;
            var plantDict = new Dictionary<int, int>
            {
                {0, 1},
                {1, 2},
                {2, 3},
                {3, 5},
            };
            foreach (var plant in plants)
            {
                times += plantDict[plant.Moon];
            }

            AnimationManager.SetHarvestBoard(
                    colorScoreTable[colordict.Values.Max()], 
                times
                );
            
            return times * bas;
        }
        
        
        public void Randomize((int x, int y) point)
        {
            Plants[point].Color = Random.Range(0, Plant.COLOR);
            Plants[point].Moon = Random.Range(0, Plant.MOON);
            for (int i = 0; i < 4; i++)
            {
                if (i == point.x) continue;
                Plants[(point.x, i)].Color = Random.Range(0, Plant.COLOR);
                Plants[(point.x, i)].Moon = Random.Range(0, Plant.MOON);

            }
            
            for (int i = 0; i < 4; i++)
            {
                if (i == point.y) continue;
                Plants[(i, point.y)].Color = Random.Range(0, Plant.COLOR);
                Plants[(i, point.y)].Moon = Random.Range(0, Plant.MOON);
            }
        }
        public void ColorMed((int x, int y) point)
        {
            Plants[point].Color = (Plants[point].Color + 1) % Plant.COLOR;
            for (int i = 0; i < 4; i++)
            {
                if (i == point.x) continue;
                Plants[(i, point.y)].Color = (Plants[(i, point.y)].Color + 1) % Plant.COLOR;
            }
            
            for (int i = 0; i < 4; i++)
            {
                if (i == point.y) continue;
                Plants[(point.x, i)].Color = (Plants[(point.x, i)].Color + 1) % Plant.COLOR;
            }

        }
        public void MoonMed((int x, int y) point)
        {
            Plants[point].Moon = (Plants[point].Moon + 1) % Plant.MOON;
            for (int i = 0; i < 4; i++)
            {
                if (i == point.x) continue;
                Plants[(i, point.y)].Moon = (Plants[(i, point.y)].Moon + 1) % Plant.MOON;
            }
            
            for (int i = 0; i < 4; i++)
            {
                if (i == point.y) continue;
                Plants[(point.x, i)].Moon = (Plants[(point.x, i)].Moon + 1) % Plant.MOON;
            }
        }
        public void TakeBlueSheet((int x, int y) point)
        {
            Plants[point].Moon = (Plants[point].Moon + Plant.MOON - 1) % Plant.MOON;
            for (int i = 0; i < 4; i++)
            {
                if (i == point.x) continue;
                Plants[(i, point.y)].Moon = (Plants[(i, point.y)].Moon + Plant.MOON - 1) % Plant.MOON;
            }
            
            for (int i = 0; i < 4; i++)
            {
                if (i == point.y) continue;
                Plants[(point.x, i)].Moon = (Plants[(point.x, i)].Moon + Plant.MOON - 1) % Plant.MOON;
            }
        }
        public void Zyouro((int x, int y) point)
        {
            Plants[point].Growth = Math.Min(Plants[point].Growth + 1, Plant.GROWTH);
            for (int i = 0; i < 4; i++)
            {
                if (i == point.y) continue;
                Plants[(point.x, i)].Growth = Math.Min(Plants[(point.x, i)].Growth + 1, Plant.GROWTH);
            }
            
            for (int i = 0; i < 4; i++)
            {
                if (i == point.x) continue;
                Plants[(i, point.y)].Growth = Math.Min(Plants[(i, point.y)].Growth + 1, Plant.GROWTH);
            }
        }
        
    }
}
