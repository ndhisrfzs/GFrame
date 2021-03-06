﻿using System;

namespace HHW.Service
{
    public abstract class Object : IDisposable
    {
        public long id { get; protected set; }
        protected Object()
        {
            this.id = IdGenerater.GenerateId();
        }
        protected Object(long id)
        {
            this.id = id;
        }

        private bool isFromPool;
        public bool IsFromPool
        {
            get
            {
                return isFromPool;
            }
            set
            {
                isFromPool = value;
                if (id == 0)
                {
                    id = IdGenerater.GenerateId();
                }
            }
        }

        public Object Parent { get; set; }

        public bool IsDisposed
        {
            get
            {
                return this.id == 0;
            }
        }

        public virtual void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.id = 0;
            if (this.isFromPool)
            {
                ObjectPool.Recycle(this);
            }
        }
    }
}
