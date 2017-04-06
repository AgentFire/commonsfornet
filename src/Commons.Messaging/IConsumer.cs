﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commons.Messaging
{
    public interface IConsumer<T> : IConsumer, IDisposable
    {
		void Subscribe(Action<T> handler);
    }

	public interface IConsumer
	{
	}
}