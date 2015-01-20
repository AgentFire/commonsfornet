﻿// Copyright CommonsForNET.
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using Commons.Collections.Map;
using Commons.Collections.Set;

namespace Test.Commons.Collections
{
    public class SortedMapTest
    {
        [Fact]
        public void TestTreeSetContructor()
        {
            Random randomValue = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            List<Order> orderList = new List<Order>();
            for (var i = 0; i < 100; i++)
            {
                Order order = new Order();
                order.Id = randomValue.Next();
                order.Name = order.Id + "(*^&^%";
                if (!orderList.Contains(order, new OrderEqualityComparer()))
                {
                    orderList.Add(order);
                }
            }
            TreeSet<Order> orderSet = new TreeSet<Order>(orderList, new OrderComparer());
            foreach (var item in orderList)
            {
                Assert.True(orderSet.Contains(item));
            }

            TreeSet<int> simpleSet = new TreeSet<int>(Enumerable.Range(0, 100));
            for (var i = 0; i < 100; i++)
            {
                Assert.True(simpleSet.Contains(i));
            }
            Assert.False(simpleSet.Contains(101));
        }

        [Fact]
        public void TestTreeSetAdd()
        {
            Random randomValue = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            List<Order> orderList = new List<Order>();
            TreeSet<Order> orderSet = new TreeSet<Order>(new OrderComparer());

            for (var i = 0; i < 1000; )
            {
                var next = randomValue.Next();
                var orderToSet = new Order();
                orderToSet.Id = next;
                orderToSet.Name = next + "*%%%(*&()*_)(;;";
                if (!orderSet.Contains(orderToSet))
                {
                    var orderToList = new Order();
                    orderToList.Id = next;
                    orderToList.Name = next + "2*%^*((%";
                    orderList.Add(orderToList);
                    orderSet.Add(orderToSet);
                    i++;
                }
            }

            Assert.Equal(1000, orderSet.Count);

            foreach (var item in orderList)
            {
                Assert.True(orderSet.Contains(item));
            }
            orderSet.Clear();
            Assert.Equal(0, orderSet.Count);
        }

        [Fact]
        public void TestTreeSetRemove()
        {
            Random r = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            List<Order> orderlist = new List<Order>();
            TreeSet<Order> orderSet = new TreeSet<Order>(new OrderComparer());

            for (var i = 0; i < 1000; )
            {
                var next = r.Next();
                var orderToSet = new Order();
                orderToSet.Id = next;
                orderToSet.Name = "(JHOI(*^Y" + next;
                if (!orderSet.Contains(orderToSet))
                {
                    var orderToList = new Order();
                    orderToList.Id = next;
                    orderToList.Name = "(*^TGUHJIO" + next;
                    orderlist.Add(orderToList);
                    orderSet.Add(orderToSet);
                    i++;
                }
            }
            for (var i = 0; i < 500; i++)
            {
                Assert.True(orderSet.Remove(orderlist[i]));
                Assert.False(orderSet.Contains(orderlist[i]));
            }
            Assert.Equal(500, orderSet.Count);
            var notExist = new Order();
            notExist.Id = 1;
            notExist.Name = "not exist";
            Assert.False(orderSet.Remove(notExist));
        }

        [Fact]
        public void TestTreeSetRemoveMax()
        {
            Random r = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            List<Order> orderlist = new List<Order>();
            TreeSet<Order> orderSet = new TreeSet<Order>(new OrderComparer());

            for (var i = 0; i < 200; )
            {
                var next = r.Next();
                var orderToSet = new Order();
                orderToSet.Id = next;
                orderToSet.Name = "(JHOI(*^Y" + next;
                if (!orderSet.Contains(orderToSet))
                {
                    var orderToList = new Order();
                    orderToList.Id = next;
                    orderToList.Name = "(*^TGUHJIO" + next;
                    orderlist.Add(orderToList);
                    orderSet.Add(orderToSet);
                    i++;
                }
            }

            var orderedList = orderlist.OrderByDescending(o => o.Id).ToList();
            for (var i = 0; i < 50; i++)
            {
                var maxOrder = orderedList[i];
                Assert.True(orderSet.Contains(maxOrder));
                var comparer = new OrderComparer();
                Assert.True(comparer.Compare(maxOrder, orderSet.Max) == 0);
                orderSet.RemoveMax();
                Assert.False(orderSet.Contains(maxOrder));
                Assert.True(comparer.Compare(maxOrder, orderSet.Max) > 0);
            }

            Assert.Equal(150, orderSet.Count);
        }

        [Fact]
        public void TestTreeSetRemoveMin()
        {
            Random r = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            List<Order> orderlist = new List<Order>();
            TreeSet<Order> orderSet = new TreeSet<Order>(new OrderComparer());

            for (var i = 0; i < 200; )
            {
                var next = r.Next();
                var orderToSet = new Order();
                orderToSet.Id = next;
                orderToSet.Name = "(JHOI(*^Y" + next;
                if (!orderSet.Contains(orderToSet))
                {
                    var orderToList = new Order();
                    orderToList.Id = next;
                    orderToList.Name = "(*^TGUHJIO" + next;
                    orderlist.Add(orderToList);
                    orderSet.Add(orderToSet);
                    i++;
                }
            }

            var orderedList = orderlist.OrderBy(o => o.Id).ToList();
            for (var i = 0; i < 50; i++)
            {
                var minOrder = orderedList[i];
                Assert.True(orderSet.Contains(minOrder));
                var comparer = new OrderComparer();
                Assert.True(comparer.Compare(minOrder, orderSet.Min) == 0);
                orderSet.RemoveMin();
                Assert.False(orderSet.Contains(minOrder));
                Assert.True(comparer.Compare(minOrder, orderSet.Min) < 0);
            }

            Assert.Equal(150, orderSet.Count);
        }

        [Fact]
        public void TestTreeSetCopyTo()
        {
            Random r = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            TreeSet<Order> orderSet = new TreeSet<Order>(new OrderComparer());
            for (var i = 0; i < 1000; )
            {
                var o = new Order();
                o.Id = r.Next();
                o.Name = i + " age4356A;";
                if (!orderSet.Contains(o))
                {
                    orderSet.Add(o);
                    i++;
                }
            }
            var orders = new Order[1003];
            orderSet.CopyTo(orders, 3);
            for (var i = 3; i < 1003; i++)
            {
                Assert.True(orderSet.Contains(orders[i]));
            }
            for (var i = 0; i < 3; i++)
            {
                Assert.Null(orders[i]);
            }
        }

        [Fact]
        public void TestTreeSetEnumerator()
        {
            Random r = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            List<Order> orderlist = new List<Order>();
            TreeSet<Order> orderSet = new TreeSet<Order>(new OrderComparer());

            for (var i = 0; i < 200; )
            {
                var next = r.Next();
                var orderToSet = new Order();
                orderToSet.Id = next;
                orderToSet.Name = "(JHOI(*^Y" + next;
                if (!orderSet.Contains(orderToSet))
                {
                    var orderToList = new Order();
                    orderToList.Id = next;
                    orderToList.Name = "(*^TGUHJIO" + next;
                    orderlist.Add(orderToList);
                    orderSet.Add(orderToSet);
                    i++;
                }
            }
            var total = 0;
            foreach (var item in orderSet)
            {
                total++;
                Assert.True(orderlist.Contains(item, new OrderEqualityComparer()));
            }
            Assert.Equal(total, orderSet.Count);
        }

        [Fact]
        public void TestTreeSetNoItem()
        {
            TreeSet<int> set = new TreeSet<int>();
            var index = 0;
            foreach (var item in set)
            {
                index++;
            }
            Assert.Equal(0, index);
            Assert.Empty(set);
            Assert.False(set.Contains(5));
            Assert.False(set.Remove(6));
            var array = new int[5] { 0 , 0, 0, 0, 0 };
            set.CopyTo(array, 0);
            Assert.Equal(0, array[0]);
        }

        [Fact]
        public void TestTreeSetExceptions()
        {
            var set = new TreeSet<Order>();
            Assert.Throws(typeof(ArgumentNullException), () => set.Add(null));
            Assert.Throws(typeof(ArgumentNullException), () => set.Remove(null));
            Assert.Throws(typeof(ArgumentNullException), () => set.Contains(null));
            Assert.Throws(typeof(ArgumentNullException), () => set.CopyTo(null, 0));
        }

        [Fact]
        public void TestTreeMapContructor()
        {
            var randomValue = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            var orderDict = new Dictionary<Order, Guid>();
            for (var i = 0; i < 100; i++)
            {
                var order = new Order();
                order.Id = randomValue.Next();
                order.Name = order.Id + "(*^&^%";
                if (!orderDict.ContainsKey(order))
                {
                    orderDict.Add(order, Guid.NewGuid());
                }
            }
            var orderMap = new TreeMap<Order, Guid>(orderDict, new OrderComparer());
            foreach (var item in orderDict.Keys)
            {
                Assert.True(orderMap.ContainsKey(item));
            }

            var simpleDict = new Dictionary<int, string>();
            for (int i = 0; i < 100; i++)
            {
                simpleDict.Add(i, Guid.NewGuid().ToString());
            }
            var simpleMap = new TreeMap<int, string>(simpleDict);
            Assert.Equal(simpleDict.Count, simpleMap.Count);
            foreach (var k in simpleDict.Keys)
            {
                Assert.True(simpleMap.ContainsKey(k));
            }
        }

        [Fact]
        public void TestSkipListMapConstructor()
        {
            var map = new SkipListMap<int, int>();
            for (var i = 0; i < 10000; i++)
            {
                map.Add(i, i);
            }
            for (var i = 0; i < 10000; i++)
            {
                Assert.True(map.ContainsKey(i));
            }

            var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
            for (var i = 0; i < 10000; i++)
            {
                orderMap.Add(new Order { Id = i }, new Bill());
            }

            for (var i = 0; i < 10000; i++)
            {
                Assert.True(orderMap.ContainsKey(new Order { Id = i }));
            }

            var orderMap2 = new SkipListMap<Order, Bill>((x1, x2) => x1.Id - x2.Id);

            for (var i = 0; i < 10000; i++)
            {
                orderMap2.Add(new Order { Id = i }, new Bill());
            }
            for (var i = 0; i < 10000; i++)
            {
                Assert.True(orderMap2.ContainsKey(new Order { Id = i }));
            }
        }

        [Fact]
        public void TestTreeMapAdd()
        {
            var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			SortedMapAdd(orderMap);
        }

        public void TestSkipListMapAdd()
        {
            var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
            SortedMapAdd(orderMap);
        }

		private void SortedMapAdd(ISortedMap<Order, Bill> orderMap)
		{
            var randomValue = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            var orderList = new List<Order>();
            for (var i = 0; i < 1000; )
            {
                var next = randomValue.Next();
                var order = new Order();
                order.Id = next;
                order.Name = next + "*%%%(*&()*_)(;;";
                if (!orderList.Contains(order))
                {
                    var orderToList = new Order();
                    orderToList.Id = next;
                    orderToList.Name = next + "2*%^*((%";
                    orderList.Add(orderToList);
                    Bill bill = new Bill();
                    bill.Id = next;
                    bill.Count = next % 1000;
                    orderMap.Add(order, bill);
                    i++;
                }
            }

            Assert.Equal(1000, orderMap.Count);

            foreach (var item in orderList)
            {
                Assert.True(orderMap.ContainsKey(item));
            }
            orderMap.Clear();
            Assert.Equal(0, orderMap.Count);
		}


        [Fact]
        public void TestTreeMapRemove()
        {
            var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			SortedMapRemove(orderMap);
        }

        [Fact]
        public void TestSkipListMapRemove()
        {
            var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
            SortedMapRemove(orderMap);
        }

		private void SortedMapRemove(ISortedMap<Order, Bill> orderMap)
		{
            var randomValue = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            List<Order> orderList = new List<Order>();
            for (var i = 0; i < 1000; ) 
            {
                var next = randomValue.Next();
                var order = new Order();
                order.Id = next;
                order.Name = next + "*%%%(*&()*_)(;;";
                if (!orderList.Contains(order))
                {
                    var orderToList = new Order();
                    orderToList.Id = next;
                    orderToList.Name = next + "2*%^*((%";
                    orderList.Add(orderToList);
                    Bill bill = new Bill();
                    bill.Id = next;
                    bill.Count = next % 1000;
                    orderMap.Add(order, bill);
                    i++;
                }
            }

            for (var i = 0; i < 500; i++)
            {
                Assert.True(orderMap.Remove(orderList[i]));
                Assert.False(orderMap.ContainsKey(orderList[i]));
            }
            Assert.Equal(500, orderMap.Count);
            var notExist = new Order();
            notExist.Id = 1;
            notExist.Name = "not exist";
            Assert.False(orderMap.Remove(notExist));
		}

        [Fact]
        public void TestTreeMapRemoveMax()
        {
            var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			SortedMapRemoveMax(orderMap);
        }

        [Fact]
        public void TestSkipListMapRemoveMax()
        {
            var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
			SortedMapRemoveMax(orderMap);
        }

		private void SortedMapRemoveMax(ISortedMap<Order, Bill> orderMap)
		{
            var r = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            var orderlist = new List<Order>();
            for (var i = 0; i < 200; )
            {
                var next = r.Next();
                var order = new Order();
                order.Id = next;
                order.Name = "(JHOI(*^Y" + next;
                if (!orderMap.ContainsKey(order))
                {
                    var orderToList = new Order();
                    orderToList.Id = next;
                    orderToList.Name = "(*^TGUHJIO" + next;
                    orderlist.Add(orderToList);
                    Bill bill = new Bill();
                    bill.Id = next;
                    bill.Count = next % 1000;
                    orderMap.Add(order, bill);
                    i++;
                }
            }

            var orderedList = orderlist.OrderByDescending(o => o.Id).ToList();
            for (var i = 0; i < 50; i++)
            {
                var maxOrder = orderedList[i];
                Assert.True(orderMap.ContainsKey(maxOrder));
                var comparer = new OrderComparer();
                Assert.True(comparer.Compare(maxOrder, orderMap.Max.Key) == 0);
                orderMap.RemoveMax();
                Assert.False(orderMap.ContainsKey(maxOrder));
                Assert.True(comparer.Compare(maxOrder, orderMap.Max.Key) > 0);
            }

            Assert.Equal(150, orderMap.Count);

		}

        [Fact]
        public void TestTreeMapRemoveMin()
        {
            var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			SortedMapRemoveMin(orderMap);
        }

        [Fact]
        public void TestSkipListMapRemoveMin()
        {
            var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
			SortedMapRemoveMin(orderMap);
        }

		private void SortedMapRemoveMin(ISortedMap<Order, Bill> orderMap)
		{
            var r = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            var orderlist = new List<Order>();
            for (var i = 0; i < 200; )
            {
                var next = r.Next();
                var order = new Order();
                order.Id = next;
                order.Name = "(JHOI(*^Y" + next;
                if (!orderMap.ContainsKey(order))
                {
                    var orderToList = new Order();
                    orderToList.Id = next;
                    orderToList.Name = "(*^TGUHJIO" + next;
                    orderlist.Add(orderToList);
                    Bill bill = new Bill();
                    bill.Id = next;
                    bill.Count = next % 1000;
                    orderMap.Add(order, bill);
                    i++;
                }
            }

            var orderedList = orderlist.OrderBy(o => o.Id).ToList();
            for (var i = 0; i < 50; i++)
            {
                var minOrder = orderedList[i];
                Assert.True(orderMap.ContainsKey(minOrder));
                var comparer = new OrderComparer();
                Assert.True(comparer.Compare(minOrder, orderMap.Min.Key) == 0);
                orderMap.RemoveMin();
                Assert.False(orderMap.ContainsKey(minOrder));
                Assert.True(comparer.Compare(minOrder, orderMap.Min.Key) < 0);
            }

            Assert.Equal(150, orderMap.Count);

		}

        [Fact]
        public void TestTreeMapCopyTo()
        {
            var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			SortedMapCopyTo(orderMap);
        }

        [Fact]
        public void TestSkipListMapCopyTo()
        {
            var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
			SortedMapCopyTo(orderMap);
        }
		
		private void SortedMapCopyTo(ISortedMap<Order, Bill> orderMap)
		{
            var r = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            for (var i = 0; i < 1000; )
            {
                var o = new Order();
                o.Id = r.Next();
                o.Name = i + " age4356A;";
                var bill = new Bill();
                bill.Id = o.Id;
                bill.Count = o.Id % 1000;
                if (!orderMap.ContainsKey(o))
                {
                    orderMap.Add(o, bill);
                    i++;
                }
            }
            var kvps = new KeyValuePair<Order, Bill>[1003];
            orderMap.CopyTo(kvps, 3);
            for (var i = 3; i < 1003; i++)
            {
                Assert.True(orderMap.Contains(kvps[i]));
            }
            for (var i = 0; i < 3; i++)
            {
                Assert.Null(kvps[i].Key);
                Assert.Null(kvps[i].Value);
            }
		}

        [Fact]
        public void TestTreeMapEnumerator()
        {
            var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			SortedMapEnumerator(orderMap);
        }

        [Fact]
        public void TestSkipListMapEnumerator()
        {
            var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
			SortedMapEnumerator(orderMap);
        }

		private void SortedMapEnumerator(ISortedMap<Order, Bill> orderMap)
		{
            var r = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            var orderList = new List<KeyValuePair<Order, Bill>>();
            for (var i = 0; i < 1000; )
            {
                var o = new Order();
                o.Id = r.Next();
                o.Name = i + " age4356A;";
                var bill = new Bill();
                bill.Id = o.Id;
                bill.Count = o.Id % 1000;
                if (!orderMap.ContainsKey(o))
                {
                    var os = new Order();
                    os.Id = o.Id;
                    os.Name = o.Name;
                    var bs = new Bill();
                    bs.Id = bill.Id;
                    bs.Count = bill.Count;
                    orderList.Add(new KeyValuePair<Order, Bill>(os, bs));
                    orderMap.Add(o, bill);
                    i++;
                }
            }

            var total = 0;
            foreach (var kvp in orderMap)
            {
                total++;
                Assert.True(orderList.Contains(kvp, new OrderBillEqualityComparer()));
            }
            Assert.Equal(1000, total);
		}

        [Fact]
        public void TestTreeMapIndexer()
        {
			var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			var orderDict = new Dictionary<Order, Bill>(new OrderEqualityComparer());
            InitializeMap(orderMap, orderDict);
			SortedMapIndexer(orderMap, orderDict);
        }

        [Fact]
        public void TestSkipListMapIndexer()
        {
			var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
			var orderDict = new Dictionary<Order, Bill>(new OrderEqualityComparer());
            InitializeMap(orderMap, orderDict);
			SortedMapIndexer(orderMap, orderDict);
        }

		private void SortedMapIndexer(ISortedMap<Order, Bill> orderMap, Dictionary<Order, Bill> orderDict)
		{
            foreach (var key in orderDict.Keys)
            {
                Assert.True(orderMap.ContainsKey(key));
                Assert.Equal(orderDict[key].Id, orderMap[key].Id);
                Assert.Equal(orderDict[key].Count, orderMap[key].Count);
            }
		}

        [Fact]
        public void TestTreeMapKeys()
        {
			var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			var orderDict = new Dictionary<Order, Bill>(new OrderEqualityComparer());
            InitializeMap(orderMap, orderDict);
			SortedMapKeys(orderMap, orderDict);
        }

        [Fact]
        public void TestSkipListMapKeys()
        {
			var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
			var orderDict = new Dictionary<Order, Bill>(new OrderEqualityComparer());
            InitializeMap(orderMap, orderDict);
			SortedMapKeys(orderMap, orderDict);
        }

		private void SortedMapKeys(ISortedMap<Order, Bill> orderMap, Dictionary<Order, Bill> orderDict)
		{
            foreach (var key in orderMap.Keys)
            {
                Assert.True(orderDict.ContainsKey(key));
                Assert.Equal(orderDict[key].Id, orderMap[key].Id);
                Assert.Equal(orderDict[key].Count, orderMap[key].Count);
            }
		}

        [Fact]
        public void TestTreeMapKeySet()
        {
			var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			var orderDict = new Dictionary<Order, Bill>(new OrderEqualityComparer());
            InitializeMap(orderMap, orderDict);
			SortedMapKeySet(orderMap, orderDict);
        }

        [Fact]
        public void TestSkipListMapKeySet()
        {
			var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
			var orderDict = new Dictionary<Order, Bill>(new OrderEqualityComparer());
            InitializeMap(orderMap, orderDict);
			SortedMapKeySet(orderMap, orderDict);
        }

		private void SortedMapKeySet(ISortedMap<Order, Bill> orderMap, Dictionary<Order, Bill> orderDict)
		{
            var set = orderMap.SortedKeySet;
            foreach (var item in set)
            {
                Assert.True(orderDict.ContainsKey(item));
            }
		}

        [Fact]
        public void TestTreeMapValues()
        {
			var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			var orderDict = new Dictionary<Order, Bill>(new OrderEqualityComparer());
            InitializeMap(orderMap, orderDict);
			SortedMapValues(orderMap, orderDict);
        }

        [Fact]
        public void TestSkipListMapValues()
        {
			var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
			var orderDict = new Dictionary<Order, Bill>(new OrderEqualityComparer());
            InitializeMap(orderMap, orderDict);
			SortedMapValues(orderMap, orderDict);
        }

		private void SortedMapValues(ISortedMap<Order, Bill> orderMap, Dictionary<Order, Bill> orderDict)
		{
            var values = orderDict.Values;
            foreach (var v in orderMap.Values)
            {
                values.Contains(v, new BillEqualityComparer());
            }
		}

        [Fact]
        public void TestTreeMapTryGetValue()
        {
			var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
			var orderDict = new Dictionary<Order, Bill>(new OrderEqualityComparer());
            InitializeMap(orderMap, orderDict);
			SortedMapTryGetValue(orderMap, orderDict);
        }

        [Fact]
        public void TestSkipListMapTryGetValue()
        {
			var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
			var orderDict = new Dictionary<Order, Bill>(new OrderEqualityComparer());
            InitializeMap(orderMap, orderDict);
			SortedMapTryGetValue(orderMap, orderDict);
        }

		private void SortedMapTryGetValue(ISortedMap<Order, Bill> orderMap, Dictionary<Order, Bill> orderDict)
		{
            foreach (var key in orderDict.Keys)
            {
                Bill bill = null;
                Assert.True(orderMap.TryGetValue(key, out bill));
                Assert.Equal(orderDict[key].Id, bill.Id);
                Assert.Equal(orderDict[key].Count, bill.Count);
            }

            var notExist = new Order();

            Bill b = null;
            Assert.False(orderMap.TryGetValue(notExist, out b));
            Assert.Null(b);
		}

        [Fact]
        public void TestTreeMapAddRemoveKvp()
        {
			var orderMap = new TreeMap<Order, Bill>(new OrderComparer());
            InitializeMap(orderMap, new Dictionary<Order, Bill>(new OrderEqualityComparer()));
			SortedMapAddRemoveKey(orderMap);
        }

        [Fact]
        public void TestSkipListMapAddRemoveKvp()
        {
			var orderMap = new SkipListMap<Order, Bill>(new OrderComparer());
            InitializeMap(orderMap, new Dictionary<Order, Bill>(new OrderEqualityComparer()));
			SortedMapAddRemoveKey(orderMap);
        }

		private void SortedMapAddRemoveKey(ISortedMap<Order, Bill> orderMap)
		{
            var newMap = new TreeMap<Order, Bill>(new OrderComparer());
            foreach (var kvp in orderMap)
            {
                newMap.Add(kvp);
            }
            Assert.Equal(orderMap.Count, newMap.Count);

            var count = 0;
            foreach (var kvp in orderMap)
            {
                if (count++ < 500)
                {
                    Assert.True(newMap.Remove(kvp));
                }
            }

            Assert.Equal(500, newMap.Count);
            var kvps = new KeyValuePair<Order, Bill>[500];
            newMap.CopyTo(kvps, 0);
            var item = new KeyValuePair<Order, Bill>(kvps[250].Key, new Bill());
            Assert.True(newMap.ContainsKey(item.Key));
            Assert.False(newMap.Remove(item));
		}

        [Fact]
        public void TestTreeMapNoItem()
        {
            var map = new TreeMap<int, string>();
			SortedMapNoItem(map);
        }

        [Fact]
        public void TestSkipListMapNoItem()
        {
            var map = new SkipListMap<int, string>();
			SortedMapNoItem(map);
        }

		private void SortedMapNoItem(ISortedMap<int, string> map)
		{
            Assert.Equal(0, map.Count);
            Assert.Empty(map);
            var count = 0;
            foreach (var kvp in map)
            {
                count++;
            }
            Assert.Equal(0, count);
            Assert.False(map.ContainsKey(0));
            Assert.False(map.Remove(0));
		}

        [Fact]
        public void TestTreeMapExceptions()
        {
            var map = new TreeMap<Order, Bill>();
            SortedMapExceptions(map);
        }

        [Fact]
        public void TestSkipListMapExceptions()
        {
            var map = new SkipListMap<Order, Bill>();
            SortedMapExceptions(map);
        }

		private void SortedMapExceptions(INavigableMap<Order, Bill> map)
		{
            Bill b;
            Assert.Throws(typeof(ArgumentNullException), () => map.Add(null, null));
            Assert.Throws(typeof(ArgumentNullException), () => map[null] = null);
            Assert.Throws(typeof(ArgumentNullException), () => { var v = map[null]; });
            Assert.Throws(typeof(ArgumentNullException), () => map.Remove(null));
            Assert.Throws(typeof(ArgumentNullException), () => map.TryGetValue(null, out b));
            Assert.Throws(typeof(ArgumentNullException), () => map.ContainsKey(null));
            Assert.Throws(typeof(ArgumentNullException), () => map.CopyTo(null, 0));
			Assert.Throws(typeof(ArgumentNullException), () => map.Higher(null));
			Assert.Throws(typeof(ArgumentNullException), () => map.Lower(null));
			Assert.Throws(typeof(ArgumentNullException), () => map.Ceiling(null));
			Assert.Throws(typeof(ArgumentNullException), () => map.Floor(null));
		}

		[Fact]
		public void TestNavigableMapHigher()
		{
			var map = new TreeMap<int, int>();
			NavigableMapHigher(map);
		}

		[Fact]
		public void TestSkipListMapHigher()
		{
			var map = new SkipListMap<int, int>();
			NavigableMapHigher(map);
		}

		private void NavigableMapHigher(INavigableMap<int, int> map)
		{
			for (var i = 0; i < 10000; i++)
			{
				map.Add(i * 5, i * 5);
			}
			Assert.Equal(0, map.Higher(-100).Key);
			for (var i = 0; i < 9999; i++)
			{
				Assert.Equal((i + 1) * 5, map.Higher(i * 5 + 1).Key);
				Assert.Equal(i * 5, map.Higher(i * 5 - 1).Key);
				Assert.Equal((i + 1) * 5, map.Higher(i * 5).Key);
			}

			Assert.Throws(typeof(ArgumentException), () => map.Higher(9999 * 5));
			Assert.Throws(typeof(ArgumentException), () => map.Higher(10000 * 5));
		}

		[Fact]
		public void TestNavigableMapCeiling()
		{
			var map = new TreeMap<int, int>();
			NavigableMapCeiling(map);
		}

		[Fact]
		public void TestSkipListMapCeiling()
		{
			var map = new SkipListMap<int, int>();
			NavigableMapCeiling(map);
		}

		private void NavigableMapCeiling(INavigableMap<int, int> map)
		{
			for (var i = 0; i < 10000; i++)
			{
				map.Add(i * 5, i * 5);
			}
			Assert.Equal(0, map.Ceiling(-100).Key);
			for (var i = 0; i < 9999; i++)
			{
				Assert.Equal((i + 1) * 5, map.Ceiling(i * 5 + 1).Key);
				Assert.Equal(i * 5, map.Ceiling(i * 5 - 1).Key);
				Assert.Equal(i * 5, map.Ceiling(i * 5).Key);
			}

			Assert.Throws(typeof(ArgumentException), () => map.Ceiling(10000 * 5));
		}

		[Fact]
		public void TestNavigableMapLower()
		{
			var map = new TreeMap<int, int>();
			NavigableMapLower(map);
		}

		[Fact]
		public void TestSkipListMapLower()
		{
			var map = new SkipListMap<int, int>();
			NavigableMapLower(map);
		}

		private void NavigableMapLower(INavigableMap<int, int> map)
		{
			for (var i = 0; i < 10000; i++)
			{
				map.Add(i * 5, i * 5);
			}
			Assert.Equal(9999 * 5, map.Lower(50000).Key);
			Assert.Equal(9999 * 5, map.Lower(100000).Key);
			for (var i = 1; i < 10000; i++)
			{
				Assert.Equal(i * 5, map.Lower(i * 5 + 1).Key);
				Assert.Equal(i * 5, map.Lower((i + 1) * 5 - 1).Key);
				Assert.Equal(i * 5, map.Lower((i + 1) * 5).Key);
			}

			Assert.Throws(typeof(ArgumentException), () => map.Lower(0));
			Assert.Throws(typeof(ArgumentException), () => map.Lower(-100));
		}

		[Fact]
		public void TestNavigableMapFloor()
		{
			var map = new TreeMap<int, int>();
			NavigableMapFloor(map);
		}

		[Fact]
		public void TestSkipListMapFloor()
		{
			var map = new SkipListMap<int, int>();
			NavigableMapFloor(map);
		}
		
		private void NavigableMapFloor(INavigableMap<int, int> map)
		{
			for (var i = 0; i < 10000; i++)
			{
				map.Add(i * 5, i * 5);
			}
			Assert.Equal(9999 * 5, map.Floor(50000).Key);
			Assert.Equal(9999 * 5, map.Floor(100000).Key);
			for (var i = 0; i < 10000; i++)
			{
				Assert.Equal(i * 5, map.Floor(i * 5 + 1).Key);
				Assert.Equal(i * 5, map.Floor((i + 1) * 5 - 1).Key);
				Assert.Equal(i * 5, map.Floor(i * 5).Key);
			}

			Assert.Throws(typeof(ArgumentException), () => map.Floor(-100));
		}

		[Fact]
		public void TestNavigableSetHigher()
		{
			var set = new TreeSet<int>();
			for (var i = 0; i < 10000; i++)
			{
				set.Add(i * 5);
			}
			Assert.Equal(0, set.Higher(-100));
			for (var i = 0; i < 9999; i++)
			{
				Assert.Equal((i + 1) * 5, set.Higher(i * 5 + 1));
				Assert.Equal(i * 5, set.Higher(i * 5 - 1));
				Assert.Equal((i + 1) * 5, set.Higher(i * 5));
			}

			Assert.Throws(typeof(ArgumentException), () => set.Higher(9999 * 5));
			Assert.Throws(typeof(ArgumentException), () => set.Higher(10000 * 5));
		}

		[Fact]
		public void TestNavigableSetCeiling()
		{
			var set = new TreeSet<int>();
			for (var i = 0; i < 10000; i++)
			{
				set.Add(i * 5);
			}
			Assert.Equal(0, set.Ceiling(-100));
			for (var i = 0; i < 9999; i++)
			{
				Assert.Equal((i + 1) * 5, set.Ceiling(i * 5 + 1));
				Assert.Equal(i * 5, set.Ceiling(i * 5 - 1));
				Assert.Equal(i * 5, set.Ceiling(i * 5));
			}

			Assert.Throws(typeof(ArgumentException), () => set.Ceiling(10000 * 5));
		}

		[Fact]
		public void TestNavigableSetLower()
		{
			var set = new TreeSet<int>();
			for (var i = 0; i < 10000; i++)
			{
				set.Add(i * 5);
			}
			Assert.Equal(9999 * 5, set.Lower(50000));
			Assert.Equal(9999 * 5, set.Lower(100000));
			for (var i = 1; i < 10000; i++)
			{
				Assert.Equal(i * 5, set.Lower(i * 5 + 1));
				Assert.Equal(i * 5, set.Lower((i + 1) * 5 - 1));
				Assert.Equal(i * 5, set.Lower((i + 1) * 5));
			}

			Assert.Throws(typeof(ArgumentException), () => set.Lower(0));
			Assert.Throws(typeof(ArgumentException), () => set.Lower(-100));
		}

		[Fact]
		public void TestNavigableSetFloor()
		{
			var set = new TreeSet<int>();
			for (var i = 0; i < 10000; i++)
			{
				set.Add(i * 5);
			}
			Assert.Equal(9999 * 5, set.Floor(50000));
			Assert.Equal(9999 * 5, set.Floor(100000));
			for (var i = 0; i < 10000; i++)
			{
				Assert.Equal(i * 5, set.Floor(i * 5 + 1));
				Assert.Equal(i * 5, set.Floor((i + 1) * 5 - 1));
				Assert.Equal(i * 5, set.Floor(i * 5));
			}

			Assert.Throws(typeof(ArgumentException), () => set.Floor(-100));
		}

		[Fact]
		public void TestEmptyNavigableMap()
		{
			var map = new TreeMap<int, int>();
            EmptyNavigableMap(map);
		}

        [Fact]
        public void TestEmptySkipListMap()
        {
            var map = new SkipListMap<int, int>();
            EmptyNavigableMap(map);
        }

        private void EmptyNavigableMap(INavigableMap<int, int> map)
        {
			Assert.Throws(typeof(InvalidOperationException), () => map.Higher(0));
			Assert.Throws(typeof(InvalidOperationException), () => map.Ceiling(0));
			Assert.Throws(typeof(InvalidOperationException), () => map.Lower(0));
			Assert.Throws(typeof(InvalidOperationException), () => map.Floor(0));
        }

		[Fact]
		public void TestEmptyNavigableSet()
		{
			var set = new TreeSet<int>();
			Assert.Throws(typeof(InvalidOperationException), () => set.Higher(0));
			Assert.Throws(typeof(InvalidOperationException), () => set.Ceiling(0));
			Assert.Throws(typeof(InvalidOperationException), () => set.Lower(0));
			Assert.Throws(typeof(InvalidOperationException), () => set.Floor(0));
		}

		private void InitializeMap(ISortedMap<Order, Bill> map, Dictionary<Order, Bill> dict)
		{
            var r = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));
            for (var i = 0; i < 1000; )
            {
                var o = new Order();
                o.Id = r.Next();
                o.Name = i + " age4356A;";
                var bill = new Bill();
                bill.Id = o.Id;
                bill.Count = o.Id % 1000;
                if (!map.ContainsKey(o))
                {
                    var os = new Order();
                    os.Id = o.Id;
                    os.Name = o.Name;
                    var bs = new Bill();
                    bs.Id = bill.Id;
                    bs.Count = bill.Count;
                    dict.Add(os, bs);
                    map.Add(o, bill);
                    i++;
                }
            }
        }
        
        private class OrderBillEqualityComparer : IEqualityComparer<KeyValuePair<Order, Bill>>
        {

            public bool Equals(KeyValuePair<Order, Bill> x, KeyValuePair<Order, Bill> y)
            {
                return (x.Key.Id == y.Key.Id) && (x.Key.Name == y.Key.Name) 
                    && (x.Value.Id == y.Value.Id) && (x.Value.Count == y.Value.Count);
            }

            public int GetHashCode(KeyValuePair<Order, Bill> obj)
            {
                return 0;
            }
        }
    }
} 