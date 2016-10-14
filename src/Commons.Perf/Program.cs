﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Commons.Json;
using Commons.Test.Json;
using Newtonsoft.Json;

namespace Commons.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestSmallObjectToJson();
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

                sw2.Start();
                JsonConvert.SerializeObject(post);
                sw2.Stop();
            }


            Console.WriteLine("Standard object to Json------ ");
            PrintResult("JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
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

                sw2.Start();
                JsonConvert.SerializeObject(p);
                sw2.Stop();
            }


            Console.WriteLine("Small object to Json----- ");
            PrintResult("JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
        }

        public static void TestSmallObjectJsonToObject()
        {
            const int LN = 1000000;
            var warm = SmallPost.Create((int)DateTime.Now.Ticks & 0x0000ffff);
            var jsonWarm = JsonMapper.ToJson(warm);
            JsonMapper.To<SmallPost>(jsonWarm);
            JsonConvert.DeserializeObject<SmallPost>(jsonWarm);

            var sw1 = new Stopwatch();
            var sw2 = new Stopwatch();
            for (var i = 0; i < LN; i++)
            {
                var p = SmallPost.Create(i);
                var json = JsonMapper.ToJson(p);

                sw1.Start();
                JsonMapper.To<SmallPost>(json);
                sw1.Stop();

                sw2.Start();
                JsonConvert.DeserializeObject<SmallPost>(json);
                sw2.Stop();
            }

            Console.WriteLine("Json to small object ----- ");
            PrintResult("JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);
        }

        public static void TestStandardObjectJsonToObject()
        {
            const int LN = 100000;
            var warm = Post.Factory<Post, Vote, PostState, Comment>((int)(0x0000ffff & DateTime.Now.Ticks), x => (PostState)x);
            var jsonWarm = JsonMapper.ToJson(warm);
            JsonMapper.To<SmallPost>(jsonWarm);
            JsonConvert.DeserializeObject<SmallPost>(jsonWarm);

            var sw1 = new Stopwatch();
            var sw2 = new Stopwatch();
            for (var i = 0; i < LN; i++)
            {
                var post = Post.Factory<Post, Vote, PostState, Comment>(i, x => (PostState)x);
                var json = JsonMapper.ToJson(post);

                sw1.Start();
                JsonMapper.To<SmallPost>(json);
                sw1.Stop();

                sw2.Start();
                JsonConvert.DeserializeObject<SmallPost>(json);
                sw2.Stop();
            }

            Console.WriteLine("Json to small object ----- ");
            PrintResult("JsonMapper", sw1.ElapsedMilliseconds, "Json.NET", sw2.ElapsedMilliseconds);

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
    }
}
