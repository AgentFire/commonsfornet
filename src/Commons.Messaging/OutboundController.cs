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
using Commons.Collections.Queue;
using Commons.Messaging.Cache;
using Microsoft.AspNetCore.Http;

namespace Commons.Messaging
{
    [CLSCompliant(false)]
    public class OutboundController : IMessageHandler<OutboundInfo>
    {
        private ICache<long, HttpContext> contextCache;
        public OutboundController(ICache<long, HttpContext> contextCache)
        {
            this.contextCache = contextCache;
        }

        public void Handle(OutboundInfo message)
        {
            var context = contextCache.From(message.SequenceNo);
            try
            {
                if (context != null)
                {
                    var json = (string)message.Content;
                    context.Response.ContentLength = json.Length;
                    context.Response.ContentType = "text/plain";
                    context.Response.WriteAsync(json).Wait();
                }
            }
            finally
            {
                contextCache.Remove(message.SequenceNo);
            }
        }
    }
}
