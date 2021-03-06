﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using ZeroMQ;

namespace Agebull.ZeroNet.Core
{
    /// <summary>
    /// 站点连接池
    /// </summary>
    class StationSocketPool
    {
        /// <summary>
        /// 对应的配置
        /// </summary>
        public StationConfig Config { get; set; }

        /// <summary>
        /// 所有连接
        /// </summary>
        private readonly List<ZSocket> _sockets = new List<ZSocket>();

        /// <summary>
        /// 连接池
        /// </summary>
        [IgnoreDataMember, JsonIgnore] private readonly ConcurrentQueue<ZSocket> _pools = new ConcurrentQueue<ZSocket>();

        /// <summary>
        /// 取得一个连接对象
        /// </summary>
        /// <returns></returns>
        internal ZSocket GetSocket()
        {
            if (_isDisposed)
                return null;
            ZSocket socket;

            while (_pools.TryDequeue(out socket))
            {
                if (socket != null && !socket.IsDisposed)
                {
                    socket.IsUsing = true;
                    return socket;
                }
            }

            socket = ZSocket.CreateRequestSocket(Config.RequestAddress);
            if (socket == null)
                return null;
            socket.IsUsing = true;
            socket.StationName = Config.StationName;
            _sockets.Add(socket);
            return socket;
        }

        /// <summary>
        /// 释放一个连接对象
        /// </summary>
        /// <returns></returns>
        internal void Free(ZSocket socket)
        {
            if (socket == null)
                return;
            socket.IsUsing = false;
            if (_isDisposed || socket.IsDisposed || socket.LastError != null)
            {
                Close(ref socket);
            }
            //Close(ref socket);
            if (_pools.Count > 99)
            {
                Close(ref socket);
            }
            else
            {
                _pools.Enqueue(socket);
            }
        }
        /// <summary>
        /// 释放一个连接对象
        /// </summary>
        /// <returns></returns>
        internal void Close(ref ZSocket socket)
        {
            if (socket == null)
                return;
            if (!_isDisposed)
                _sockets.Remove(socket);
            socket.TryClose();
            socket = null;
        }

        /// <summary>
        /// 是否已析构
        /// </summary>
        [IgnoreDataMember, JsonIgnore]
        private bool _isDisposed;

        /// <summary>
        /// 释放一个连接对象
        /// </summary>
        /// <returns></returns>
        public void Resume()
        {
            if (!_isDisposed)
                return;
            DoDispose();
            _isDisposed = false;
        }

        /// <summary>
        /// 释放一个连接对象
        /// </summary>
        /// <returns></returns>
        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            DoDispose();
        }

        /// <summary>
        /// 释放一个连接对象
        /// </summary>
        /// <returns></returns>
        private void DoDispose()
        {
            while (_pools.TryDequeue(out var socket))
                socket?.TryClose();
            foreach (var socket in _sockets.ToArray())
            {
                if (!socket.IsUsing)
                    socket?.TryClose();
            }
            _sockets.Clear();
        }
    }
}