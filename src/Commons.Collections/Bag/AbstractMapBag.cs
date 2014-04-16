﻿// Copyright CommonsForNET. Author: Gujun Yang. email: gujun.yang@gmail.com
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
using System.Text;
using System.Threading.Tasks;

namespace Commons.Collections.Bag
{
    [CLSCompliant(true)]
    public abstract class AbstractMapBag<T> : IBag<T>
    {
        protected IMap<T, int> Map { get; private set; }

        protected AbstractMapBag(IMap<T, int> map)
        {
            Map = map;
        }
        public virtual int GetCount(T item)
        {
            return Map[item];
        }

        public virtual void Add(T item, int copies = 1)
        {
            if (Map.ContainsKey(item))
            {
                Map[item] += copies;
            }
            else
            {
                Map.Add(item, copies);
            }
        }

        public virtual bool Remove(T item, int copies)
        {
            var removed = false;
            if (Map.ContainsKey(item))
            {
                var count = Map[item];
                if (count > copies)
                {
                    Map[item] -= copies;
                }
                else
                {
                    Remove(item);
                }
                removed = true;
            }
            return removed;
        }

        public virtual bool Remove(T item)
        {
            var removed = false;
            if (Map.ContainsKey(item))
            {
                Map.Remove(item);
                removed = true;
            }
            return removed;
        }

        public virtual ITreeSet<T> UniqueSet()
        {
            return null;
        }

        public bool ContainsAll(ICollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAll(ICollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public bool RetainAll(ICollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
