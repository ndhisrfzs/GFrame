﻿using System;
using System.Collections.Generic;

namespace HHW.Service
{
    public static class ObjectPool
    {
        private static readonly Dictionary<Type, Queue<Object>> dictionary = new Dictionary<Type, Queue<Object>>();

        public static T Fetch<T>()
            where T : Object
        {
            Type type = typeof(T);
            Queue<Object> queue = null;
            if (!dictionary.TryGetValue(type, out queue))
            {
                queue = new Queue<Object>();
                dictionary.Add(type, queue);
            }

            T obj;
            if (queue.Count > 0)
            {
                obj = (T)queue.Dequeue();
                obj.IsFromPool = true;
                if (obj is Component)
                {
                    (obj as Component).AddEventSystem();
                }
                return obj;
            }

            obj = (T)Activator.CreateInstance(type);
            obj.IsFromPool = true;
            return obj;
        }

        public static void Recycle(Object obj)
        {
            Type type = obj.GetType();
            Queue<Object> queue = null;
            if (!dictionary.TryGetValue(type, out queue))
            {
                queue = new Queue<Object>();
                dictionary.Add(type, queue);
            }
            queue.Enqueue(obj);
        }
    }
}
