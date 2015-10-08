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
using System.Collections;
using System.Collections.Generic;

namespace Commons.Collections.Map
{
	/// <summary>
	/// The keys of the multi-value map are sorted. The map provides the operations on maximum and minimum keys.
	/// </summary>
	/// <typeparam name="K">The type of the key.</typeparam>
	/// <typeparam name="V">The type of the value.</typeparam>
    [CLSCompliant(true)]
    public interface ISortedMultiValueMap<K, V> : IMultiValueMap<K, V>, ICollection<KeyValuePair<K, V>>, IEnumerable<KeyValuePair<K, V>>, IEnumerable
    {
        /// <summary>
        /// Retrieves the maximum key in the multi value map.
        /// </summary>
        K Max { get; }

        /// <summary>
        /// Retrieves the minimum key in the multi value map.
        /// </summary>
        K Min { get; }

        /// <summary>
        /// Removes the max value of the multi value map.
        /// </summary>
        void RemoveMax();

        /// <summary>
        /// Removes the min value of the multi value map.
        /// </summary>
        void RemoveMin();

        /// <summary>
        /// Check whether the map is empty.
        /// </summary>
        bool IsEmpty { get; }
    }
}
