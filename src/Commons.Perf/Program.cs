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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Commons.Json;
using Commons.Test.Json;
using Commons.Test.Reflect;
using Commons.Reflect;
using System.Reflection;

namespace Commons.Perf
{
	public class Program
    {
        public static void Main(string[] args)
        {
			//TestCommonsJsonSingleThread();
			TestCommonsReflect();
        }

		public static void TestCommonsReflect()
		{
			const int LN = 10000000;
			var vehicle = new Vehicle
			{
				VehicleType = "Car",
				Color = "White"
			};
			vehicle.GetPropertyValue("VehicleType");
			var prop = typeof(Vehicle).GetProperty("VehicleType");
			var getter = prop.GetGetMethod();
			getter.Invoke(vehicle, null);

			var sw1 = new Stopwatch();
			sw1.Start();
			for (var i = 0; i < LN; i++)
			{
				vehicle.GetPropertyValue("VehicleType");
			}
			sw1.Stop();

			var sw2 = new Stopwatch();
			sw2.Start();
			for (var i = 0; i < LN; i++)
			{
				getter.Invoke(vehicle, null);
			}
			sw2.Stop();

			PrintResult("Reflection new instance default constructor", 
				"Commons.Reflect", sw1.ElapsedMilliseconds, "System.Reflection", sw2.ElapsedMilliseconds);
		}

	    public static void TestCommonsJsonSingleThread()
        {
			TestTrivialObjectToJson();
			TestSmallObjectToJson();
			TestNormalObjectToJson();
			TestStandardObjectToJson();
			TestLargeObjectToJson();

			TestTrivialObjectJsonToObject();
			TestSmallObjectJsonToObject();
			TestStandardObjectJsonToObject();
			TestLargeObjectJsonToObject();
        }

		public static void TestMultiThread()
		{
			TestStandardObjectMultiThreadToJson();
			TestStandardObjectMultiThreadJsonToObject();
		}

		public static void TestStandardObjectMultiThreadToJson()
		{
			const int LN = 20000;
            var warm = Post.Factory<Post, Vote, PostState, Comment>((int)(0x0000ffff & DateTime.Now.Ticks), x => (PostState)x);
            JsonMapper.ToJson(warm);
            JsonConvert.SerializeObject(warm);

			var sw1 = new Stopwatch();
			var sw2 = new Stopwatch();

			var jmTasks = new Task[4];
			var post = Post.Factory<Post, Vote, PostState, Comment>(24123, x => (PostState)x);
			for (var n = 0; n < 4; n++)
			{
				jmTasks[n] = new Task(() =>
				{
					for (var i = 0; i < LN; i++)
					{
						JsonMapper.ToJson(post);
					}
				});
			}
			sw1.Start();
			foreach (var t in jmTasks)
			{
				t.Start();
			}
			Task.WaitAll(jmTasks);
			sw1.Stop();

			var jcTasks = new Task[4];
			for (var n = 0; n < 4; n++)
			{
				jcTasks[n] = new Task(() =>
				{
					for (var i = 0; i < LN; i++)
					{
						JsonConvert.SerializeObject(post);
					}
				});
			}
			sw2.Start();
			foreach (var t in jcTasks)
			{
				t.Start();
			}

			Task.WaitAll(jcTasks);
			sw2.Stop();

			PrintResult("Multi-thread standard object to Json", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
		}

		public static void TestStandardObjectMultiThreadJsonToObject()
		{
			const int LN = 20000;
			var json = ReadFile("Post.txt");
			JsonMapper.To<Post>(json);
			JsonConvert.DeserializeObject<Post>(json);

			var sw1 = new Stopwatch();
			var sw2 = new Stopwatch();

			var jmTasks = new Task[4];
			for (var n = 0; n < 4; n++)
			{
				jmTasks[n] = new Task(() =>
				{
					for (var i = 0; i < LN; i++)
					{
						JsonMapper.To<Post>(json);
					}
				});
			}
			sw1.Start();
			foreach (var t in jmTasks)
			{
				t.Start();
			}
			Task.WaitAll(jmTasks);
			sw1.Stop();

			var jcTasks = new Task[4];
			for (var n = 0; n < 4; n++)
			{
				jcTasks[n] = new Task(() =>
				{
					for (var i = 0; i < LN; i++)
					{
						JsonConvert.DeserializeObject<Post>(json);
					}
				});
			}
			sw2.Start();
			foreach (var t in jcTasks)
			{
				t.Start();
			}
			Task.WaitAll(jcTasks);
			sw2.Stop();

			PrintResult("Multi-thread Json to standard object", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
		}

		public static void TestTrivialObjectToJson()
		{
			const int LN = 1000000;
			var warm = Message.Create(0);
			JsonMapper.ToJson(warm);
			JsonConvert.SerializeObject(warm);
			var sw1 = new Stopwatch();
			var sw2 = new Stopwatch();
			for (var i = 0; i < LN; i++)
			{
				var message = Message.Create(i + 1);
				sw1.Start();
				JsonMapper.ToJson(message);
				sw1.Stop();
			}

			for (var i = 0; i < LN; i++)
			{
				var message = Message.Create(i + 1);
				sw2.Start();
				JsonConvert.SerializeObject(message);
				sw2.Stop();
			}

            PrintResult("Trivial object to Json", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
		}

		public static void TestTrivialObjectJsonToObject()
		{
			const int LN = 1000000;
			var rand = new Random((int)(0x0000ffff & DateTime.Now.Ticks));
			var template = ReadFile("Message.txt");
			var warm = GenerateMessageJson(rand, template);
			JsonMapper.To<Message>(warm);
			JsonConvert.DeserializeObject<Message>(warm);

			var sw1 = new Stopwatch();
			var sw2 = new Stopwatch();
			for (var i = 0; i < LN; i++)
			{
				var json = GenerateMessageJson(rand, template);
				sw1.Start();
				JsonMapper.To<Message>(json);
				sw1.Stop();
			}

			for (var i = 0; i < LN; i++)
			{
				var json = GenerateMessageJson(rand, template);
				sw2.Start();
				JsonConvert.DeserializeObject<Message>(json);
				sw2.Stop();
			}
            PrintResult("Json to trivial object", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
		}

		public static void TestLargeObjectToJson()
		{
			const int LN = 10000;
            var warm = Book.Factory<Book, Genre, Page, Headnote, Footnote>(7, x => (Genre)x);
			JsonMapper.ToJson(warm);
			JsonConvert.SerializeObject(warm);

			var sw1 = new Stopwatch();
			var sw2 = new Stopwatch();

			for (var i = 0; i < LN; i++)
			{
				var book = Book.Factory<Book, Genre, Page, Headnote, Footnote>(7, x => (Genre)x);
				sw1.Start();
				JsonMapper.ToJson(book);
				sw1.Stop();
			}

			GC.Collect();
			Thread.Sleep(1000);

			for (var i = 0; i < LN; i++)
			{
				var book = Book.Factory<Book, Genre, Page, Headnote, Footnote>(7, x => (Genre)x);
				sw2.Start();
				JsonConvert.SerializeObject(book);
				sw2.Stop();
			}

            PrintResult("Large object to Json", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
		}

        public static void TestStandardObjectToJson()
        {
            const int LN = 100000;

            var warm = Post.Factory<Post, Vote, PostState, Comment>((int)(0x0000ffff & DateTime.Now.Ticks), x => (PostState)x);
            JsonMapper.ToJson(warm);
            JsonConvert.SerializeObject(warm);

            var sw1 = new Stopwatch();
            var sw2 = new Stopwatch();
			for (var i = 0; i < LN; i++)
			{
				var post = Post.Factory<Post, Vote, PostState, Comment>(i, x => (PostState)x);
				sw1.Start();
				JsonMapper.ToJson(post);
				sw1.Stop();
			}

            for (var i = 0; i < LN; i++)
            {
                var post = Post.Factory<Post, Vote, PostState, Comment>(i, x => (PostState)x);
                sw2.Start();
                JsonConvert.SerializeObject(post);
                sw2.Stop();
            }

            PrintResult("Standard object to Json", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
        }

        public static void TestSmallObjectToJson()
        {
            const int LN = 1000000;
            var warm = SmallPost.Create((int)DateTime.Now.Ticks & 0x0000ffff);
            JsonMapper.ToJson(warm);
            JsonConvert.SerializeObject(warm);
            var sw1 = new Stopwatch();
            var sw2 = new Stopwatch();
			for (var i = 0; i < LN; i++)
			{
				var p = SmallPost.Create(i);
				sw1.Start();
				JsonMapper.ToJson(p);
				sw1.Stop();
			}

            for (var i = 0; i < LN; i++)
            {
				var p = SmallPost.Create(i);
                sw2.Start();
                JsonConvert.SerializeObject(p);
                sw2.Stop();
            }


            PrintResult("Small object to Json", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
        }

		public static void TestNormalObjectToJson()
		{
			const int LN = 1000000;
			var rand = new Random((int)(0x0000ffff & DateTime.Now.Ticks));
			var warm = CompletePrimitiveObject.Factory(rand);
			JsonConvert.SerializeObject(warm);
			JsonMapper.ToJson(warm);

			var sw1 = new Stopwatch();
			var sw2 = new Stopwatch();
			for (var i = 0; i < LN; i++)
			{
				var obj = CompletePrimitiveObject.Factory(rand);
				sw1.Start();
				JsonMapper.ToJson(obj);
				sw1.Stop();
			}

			for (var i = 0; i < LN; i++)
			{
				var obj = CompletePrimitiveObject.Factory(rand);
				sw2.Start();
				JsonConvert.SerializeObject(obj);
				sw2.Stop();
			}
            PrintResult("Complete primitive object to Json", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
		}

        public static void TestSmallObjectJsonToObject()
        {
            const int LN = 1000000;

			var rand = new Random((int)(0x0000ffff & DateTime.Now.Ticks));

			var jsonTemplate = ReadFile("SmallPost.txt");
			var warm = GenerateSmallPostJson(rand, jsonTemplate);
            JsonMapper.To<SmallPost>(warm);
            JsonConvert.DeserializeObject<SmallPost>(warm);

            var sw1 = new Stopwatch();
            var sw2 = new Stopwatch();
			for (var i = 0; i < LN; i++)
			{
				var json = GenerateSmallPostJson(rand, jsonTemplate);
				sw1.Start();
				JsonMapper.To<SmallPost>(json);
				sw1.Stop();
			}

            for (var i = 0; i < LN; i++)
            {
				var json = GenerateSmallPostJson(rand, jsonTemplate);
                sw2.Start();
                JsonConvert.DeserializeObject<SmallPost>(json);
                sw2.Stop();
            }

            PrintResult("Json to small object", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
        }

        public static void TestStandardObjectJsonToObject()
        {
			var json = ReadFile("Post.txt");
			JsonMapper.To<Post>(json);
			JsonConvert.DeserializeObject<Post>(json);

            const int LN = 10000;

            var sw1 = new Stopwatch();
            var sw2 = new Stopwatch();
			for (var i = 0; i < LN; i++)
			{
				sw1.Start();
				JsonMapper.To<Post>(json);
				sw1.Stop();
			}

            for (var i = 0; i < LN; i++)
            {
                sw2.Start();
                JsonConvert.DeserializeObject<Post>(json);
                sw2.Stop();
            }

            PrintResult("Json to standard object", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
        }

		public static void TestLargeObjectJsonToObject()
		{
			var json = ReadFile("NewBook.txt");
			JsonMapper.To<NewBook>(json);
			JsonConvert.DeserializeObject<NewBook>(json);

            const int LN = 10000;

            var sw1 = new Stopwatch();
            var sw2 = new Stopwatch();
			for (var i = 0; i < LN; i++)
			{
				sw1.Start();
				JsonMapper.To<NewBook>(json);
				sw1.Stop();
			}

			GC.Collect();
			Thread.Sleep(1000);

            for (var i = 0; i < LN; i++)
            {
                sw2.Start();
                JsonConvert.DeserializeObject<NewBook>(json);
                sw2.Stop();
            }

            PrintResult("Json to large object", "JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
		}

		private static void PrintResult(string title, string test1, long test1Ms, string test2, long test2Ms)
		{
			Console.WriteLine("------------------");
            Console.WriteLine(title);
			PrintResult(test1, test1Ms, test2, test2Ms);
			Console.WriteLine("------------------");
		}

        private static void PrintResult(string test1, long test1Ms, string test2, long test2Ms)
        {
            string faster, slower;
            double rate;
            if (test1Ms > test2Ms)
            {
                faster = test2;
                slower = test1;
                rate = ((double)test1Ms - (double)test2Ms) / (double)test2Ms;
            }
            else
            {
                faster = test1;
                slower = test2;
                rate = ((double)test2Ms - (double)test1Ms) / (double)test1Ms;
            }
            rate = rate * 100;
            rate = Math.Round(rate, 2);

            var result = string.Format("{0}: {1} \n{2}: {3}", test1, test1Ms, test2, test2Ms);
            var comp = string.Format("{0} is slower than {1} by {2}%", slower, faster, rate);
            Console.WriteLine(result);
            Console.WriteLine(comp);
            
        }

		private static string ReadFile(string name)
		{
			string result;
			using (var fs = new FileStream(name, FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					result = sr.ReadToEnd();
				}
			}
			return result;
		}

		private static string GenerateSmallPostJson(Random rand, string template)
		{
			var result = template.Replace("(GUID)", Guid.NewGuid().ToString());
			result = result.Replace("(TITLE)", "Some title" + rand.Next());
			result = result.Replace("(ACTIVE)", (rand.Next() % 2 == 0).ToString().ToLower());
			result = result.Replace("(CREATED)", DateTime.Now.ToString(CultureInfo.InvariantCulture));
			result = result.Replace("(COUNT)", (rand.Next() % 1000).ToString());

			return result;
		}

		private static string GenerateMessageJson(Random rand, string template)
		{
			var result = template.Replace("(MESSAGE)", "Some message " + rand.Next());
			result = result.Replace("(VERSION)", rand.Next().ToString());
			return result;
		}
    }
}
