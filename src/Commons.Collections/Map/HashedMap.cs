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
using Commons.Utils;

namespace Commons.Collections.Map
{
    [CLSCompliant(true)]
    public class HashedMap<K, V> : AbstractHashedMap<K, V>, IDictionary<K, V>, IReadOnlyDictionary<K, V>
    {
        private const int DefaultCapacity = 16;

        public HashedMap() : this(DefaultCapacity)
        {
        }

        public HashedMap(IEqualityComparer<K> comparer)
            : this(comparer.Equals)
        {
        }

		public HashedMap(Equator<K> equator) : this(DefaultCapacity, equator)
		{
		}

        public HashedMap(int capacity) : this(capacity, EqualityComparer<K>.Default)
        {
        }

        public HashedMap(int capacity, IEqualityComparer<K> comparer)
            : this(capacity, comparer.Equals)
        {
        }

        public HashedMap(int capacity, Equator<K> equator) : base(capacity, equator)
        {
        }

		public HashedMap(IDictionary<K, V> items, IEqualityComparer<K> comparer) : this(items, comparer.Equals)
		{
		}

		public HashedMap(IDictionary<K, V> items) : this(items, EqualityComparer<K>.Default.Equals)
		{
		}

        public HashedMap(IDictionary<K, V> items, Equator<K> equator)
            : base(items == null ? DefaultCapacity : items.Count, equator)
        {
            if (null != items)
            {
                foreach (var item in items)
                {
                    Add(item);
                }
            }
        }

        protected override long HashIndex(K key)
        {
            var hash = key.GetHashCode();
            return hash & (Capacity - 1);
        }
    }
}