﻿using System;

namespace HHW.Service
{
    public abstract class AMHandler<Request> : IMHandler where Request : class, IRequest
    {
        protected abstract void Run(Session session, Request message);
        public void Handle(Session session, object message)
        {
            try
            {
                Request request = message as Request;
                if (request == null)
                {
                    return;
                }

                uint rpcId = request.RpcId;
                this.Run(session, request);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        public Type GetMessageType()
        {
            return typeof(Request);
        }
    }
}
