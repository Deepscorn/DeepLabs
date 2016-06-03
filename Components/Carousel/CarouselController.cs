// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Util.UI.Carousel;
using UnityEngine;

namespace Assets.Sources.Scripts.Basic.Carousel
{
    public class CarouselController: MonoBehaviour
    {
        public AbstractTriggerDataProvider InnerBound;
        public AbstractTriggerDataProvider OuterBound;
        public AbstractLayout Layout;
        public Adapter Adapter;
        public int MaxItemsInBounds = 50;
        public bool FillIncreasingPositionsOnEnable = true;
        public bool FillDecreasingPositionsOnEnable = true;
        public bool RecreateOnNoItems = false;

        private readonly Dictionary<int, Stack<ItemData>> pool = new Dictionary<int, Stack<ItemData>>();
        private readonly SortedDictionary<int, ItemData> positionToItemDatas = new SortedDictionary<int, ItemData>();
        private readonly Dictionary<GameObject, ItemData> objToItemDatas = new Dictionary<GameObject, ItemData>();
        
        public void OnEnable()
        {
            Adapter.Reset();

            // we need all parent game objects be positioned already
            // so can't create right now, need to wait
            StartCoroutine(CreateAfterUpdate());
        }

        private IEnumerator CreateAfterUpdate()
        {
            yield return null;
            if (positionToItemDatas.Count == 0)
            {
                CreateItems();
            }
        }

        private void CreateItems()
        {
            InnerBound.OnEnterEvent -= OnInnerBoundEnter;
            OuterBound.OnExitEvent -= OnOuterBoundExit;
            if (FillIncreasingPositionsOnEnable)
            {
                CreateItems(0, MaxItemsInBounds, 1);
            }
            if (FillDecreasingPositionsOnEnable)
            {
                CreateItems(FillIncreasingPositionsOnEnable ? -1 : 0, -MaxItemsInBounds, -1);
            }

            // we do not want these events because of initialization stuff, so
            // we start listening AFTER events fired
            StartCoroutine(LateListen());
        }

        private void CreateItems(int startPosition, int maxPosition, int step)
        {
            ItemData itemToPutInPool = null;
            for (var position = startPosition; itemToPutInPool == null && Mathf.Abs(position - maxPosition) > 0; position += step)
            {
                var itemData = RetrieveFromPoolOrCreate(position);
                StartTracking(itemData);
                Layout.Position(itemData.Item, position, gameObject);
                if (!Layout.Touches(OuterBound.gameObject, itemData.Item))
                {
                    itemToPutInPool = itemData;
                }
            }
            if (itemToPutInPool != null)
            {
                PutInPool(itemToPutInPool);
            }
        }

        private IEnumerator LateListen()
        {
            yield return new WaitForFixedUpdate();

            InnerBound.OnEnterEvent += OnInnerBoundEnter;
            OuterBound.OnExitEvent += OnOuterBoundExit;
        } 

        public void OnDisable()
        {
            InnerBound.OnEnterEvent -= OnInnerBoundEnter;
            OuterBound.OnExitEvent -= OnOuterBoundExit;

            foreach (var item in positionToItemDatas.Values)
            {
                DestroyObject(item.Item);
            }
            positionToItemDatas.Clear();
            objToItemDatas.Clear();
            foreach (var itemsStack in pool.Values)
            {
                foreach (var item in itemsStack)
                {
                    DestroyObject(item.Item);
                }
                
            }
            pool.Clear();
        }

        public void Update()
        {
            if (RecreateOnNoItems)
            {
                StartCoroutine(CreateAfterUpdate());
            }
        }

        private void StartTracking(ItemData itemData)
        {
            positionToItemDatas.Add(itemData.Position, itemData);
            objToItemDatas.Add(itemData.Item, itemData);
        }

        private void StopTracking(ItemData itemData)
        {
            positionToItemDatas.Remove(itemData.Position);
            objToItemDatas.Remove(itemData.Item);
        }

        private void OnInnerBoundEnter(GameObject item)
        {
            if (positionToItemDatas.Count > 0)
            {
                var first = positionToItemDatas.Values.First();
                if (first.Item == item)
                {
                    AddItemAndPutInPoolIfInsideInnerBounds(first.Position - 1);
                }
                var last = positionToItemDatas.Values.Last();
                if (last.Item == item)
                {
                    AddItemAndPutInPoolIfInsideInnerBounds(last.Position + 1);
                }
            }
        }

        private void OnOuterBoundExit(GameObject item)
        {
            ItemData itemData;
            objToItemDatas.TryGetValue(item, out itemData);
            if (itemData != null)
            {
                PutInPool(itemData);
            }
        }

        private void AddItemAndPutInPoolIfInsideInnerBounds(int position)
        {
            var item = RetrieveFromPoolOrCreate(position);
            Layout.Position(item.Item, position, gameObject);
            StartTracking(item);
            if (Layout.Touches(InnerBound.gameObject, item.Item))
            {
                PutInPool(item);
            }
        }

        private ItemData RetrieveFromPoolOrCreate(int position)
        {
            var result = RetrieveFromPool(Adapter.GetItemViewType(position)) ?? new ItemData();
            // set active before positioning to get valid collider bound extents
            // also to be able to start Coroutines on that object by adapter for example
            if (result.Item != null)
            {
                result.Item.SetActive(true);
            }
            result.Item = Adapter.GetView(position, result.Item, gameObject);
            result.Position = position;

            return result;
        }

        private ItemData RetrieveFromPool(int itemViewType)
        {
            Stack<ItemData> itemsOfRequestedType;
            pool.TryGetValue(itemViewType, out itemsOfRequestedType);
            return (itemsOfRequestedType == null || itemsOfRequestedType.Count == 0) ? 
                null : itemsOfRequestedType.Pop();
        }

        private void PutInPool(ItemData item)
        {
            StopTracking(item);
            var itemViewType = Adapter.GetItemViewType(item.Position);
            Stack<ItemData> poolOfSameType;
            pool.TryGetValue(itemViewType, out poolOfSameType);
            if (poolOfSameType == null)
            {
                poolOfSameType = new Stack<ItemData>();
                pool.Add(itemViewType, poolOfSameType);
            }
            poolOfSameType.Push(item);
            item.Item.SetActive(false);
        }

        private class ItemData
        {
            public int Position { get; set; }
            public GameObject Item { get; set; }

            public override string ToString()
            {
                return "position " + Position + " " + Item;
            }
        }
    }
}
