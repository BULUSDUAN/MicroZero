﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ZeroMQ
{
    /// <summary>
    /// A single or multi-part message, sent or received via a <see cref="ZSocket"/>.
    /// </summary>
    public class ZMessage : MemoryCheck, IList<ZFrame>, ICloneable
    {
#if UNMANAGE_MONEY_CHECK
        protected override string TypeName => nameof(ZMessage);
#endif
        private List<ZFrame> _frames;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZMessage"/> class.
        /// Creates an empty message.
        /// </summary>
        public ZMessage()
        {
            _frames = new List<ZFrame>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ZMessage"/> class.
        /// Creates an empty message.
        /// </summary>
        public ZMessage(byte[] desc, params string[] args)
        {
            _frames = new List<ZFrame> { new ZFrame(desc) };
            foreach (var frame in args)
                _frames.Add(new ZFrame(frame));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZMessage"/> class.
        /// Creates an empty message.
        /// </summary>
        public static ZMessage Create(params byte[][] frames)
        {
            return new ZMessage(frames);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ZMessage"/> class.
        /// Creates a message that contains the given <see cref="ZFrame"/> objects.
        /// </summary>
        /// <param name="frames">A collection of <see cref="ZFrame"/> objects to be stored by this <see cref="ZMessage"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="frames"/> is null.</exception>
        public ZMessage(IEnumerable<byte[]> frames)
        {
            _frames = new List<ZFrame>();
            foreach (var frame in frames)
                _frames.Add(new ZFrame(frame));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ZMessage"/> class.
        /// Creates a message that contains the given <see cref="ZFrame"/> objects.
        /// </summary>
        /// <param name="frames">A collection of <see cref="ZFrame"/> objects to be stored by this <see cref="ZMessage"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="frames"/> is null.</exception>
        public ZMessage(IEnumerable<ZFrame> frames)
        {
            if (frames == null)
            {
                throw new ArgumentNullException(nameof(frames));
            }

            _frames = new List<ZFrame>(frames);
        }

        /// <summary>
        /// 析构
        /// </summary>
        protected override void DoDispose()
        {
            if (_frames != null)
            {
                foreach (var frame in _frames)
                {
                    frame.Close();
                }
            }
            _frames = null;
        }

        public void ReplaceAt(int index, ZFrame replacement)
        {
            ReplaceAt(index, replacement, true);
        }

        public ZFrame ReplaceAt(int index, ZFrame replacement, bool dispose)
        {
            var old = _frames[index];
            _frames[index] = replacement;
            if (dispose)
            {
                old.Dispose();
                return null;
            }
            return old;
        }

        #region IList implementation

        public int IndexOf(ZFrame item)
        {
            return _frames.IndexOf(item);
        }

        public void Prepend(ZFrame item)
        {
            Insert(0, item);
        }

        public void Insert(int index, ZFrame item)
        {
            _frames.Insert(index, item);
        }

        /// <summary>
        /// Removes ZFrames. Note: Disposes the ZFrame.
        /// </summary>
        /// <returns>The <see cref="ZeroMQ.ZFrame"/>.</returns>
        public void RemoveAt(int index)
        {
            RemoveAt(index, true);
        }

        /// <summary>
        /// Removes ZFrames.
        /// </summary>
        /// <returns>The <see cref="ZeroMQ.ZFrame"/>.</returns>
        /// <param name="index"></param>
        /// <param name="dispose">If set to <c>false</c>, do not dispose the ZFrame.</param>
        public ZFrame RemoveAt(int index, bool dispose)
        {
            var frame = _frames[index];
            _frames.RemoveAt(index);

            if (!dispose)
                return frame;
            frame.Dispose();
            return null;
        }

        public ZFrame Pop()
        {
            var result = RemoveAt(0, false);
            result.Position = 0; // TODO maybe remove this here again, see https://github.com/zeromq/clrzmq4/issues/110
            return result;
        }

        public int PopBytes(byte[] buffer, int offset, int count)
        {
            using (var frame = Pop())
            {
                return frame.Read(buffer, offset, count);
            }
        }

        public int PopByte()
        {
            using (var frame = Pop())
            {
                return frame.ReadByte();
            }
        }

        public byte PopAsByte()
        {
            using (var frame = Pop())
            {
                return frame.ReadAsByte();
            }
        }

        public Int16 PopInt16()
        {
            using (var frame = Pop())
            {
                return frame.ReadInt16();
            }
        }

        public UInt16 PopUInt16()
        {
            using (var frame = Pop())
            {
                return frame.ReadUInt16();
            }
        }

        public Char PopChar()
        {
            using (var frame = Pop())
            {
                return frame.ReadChar();
            }
        }

        public Int32 PopInt32()
        {
            using (var frame = Pop())
            {
                return frame.ReadInt32();
            }
        }

        public UInt32 PopUInt32()
        {
            using (var frame = Pop())
            {
                return frame.ReadUInt32();
            }
        }

        public Int64 PopInt64()
        {
            using (var frame = Pop())
            {
                return frame.ReadInt64();
            }
        }

        public UInt64 PopUInt64()
        {
            using (var frame = Pop())
            {
                return frame.ReadUInt64();
            }
        }

        public String PopString()
        {
            return PopString(ZContext.Encoding);
        }

        public String PopString(Encoding encoding)
        {
            using (var frame = Pop())
            {
                return frame.ReadString((int)frame.Length, encoding);
            }
        }

        public String PopString(int bytesCount, Encoding encoding)
        {
            using (var frame = Pop())
            {
                return frame.ReadString(bytesCount, encoding);
            }
        }

        public void Wrap(ZFrame frame)
        {
            Insert(0, new ZFrame());
            Insert(0, frame);
        }

        public ZFrame Unwrap()
        {
            var frame = RemoveAt(0, false);

            if (Count > 0 && this[0].Length == 0)
            {
                RemoveAt(0);
            }

            return frame;
        }

        public ZFrame this[int index]
        {
            get => _frames[index];
            set => _frames[index] = value;
        }

        #endregion

        #region ICollection implementation

        public void Append(ZFrame item)
        {
            Add(item);
        }

        public void AppendRange(IEnumerable<ZFrame> items)
        {
            AddRange(items);
        }

        public void Add(ZFrame item)
        {
            _frames.Add(item);
        }

        public void AddRange(IEnumerable<ZFrame> items)
        {
            _frames.AddRange(items);
        }

        public void Clear()
        {
            foreach (var frame in _frames)
            {
                frame.Dispose();
            }
            _frames.Clear();
        }

        public bool Contains(ZFrame item)
        {
            return _frames.Contains(item);
        }

        void ICollection<ZFrame>.CopyTo(ZFrame[] array, int arrayIndex)
        {
            int i = 0, count = Count;
            foreach (var frame in this)
            {
                array[arrayIndex + i] = ZFrame.CopyFrom(frame);

                i++; if (i >= count) break;
            }
        }

        public bool Remove(ZFrame item)
        {
            if (null != Remove(item, true))
            {
                return false;
            }
            return true;
        }

        public ZFrame Remove(ZFrame item, bool dispose)
        {
            if (_frames.Remove(item))
            {
                if (dispose)
                {
                    item.Dispose();
                    return null;
                }
            }
            return item;
        }
        /// <summary>
        /// 总数
        /// </summary>
        public int Count
        {
            get
            {
                if (_frames == null)
                    return 0;

                return _frames.Count;
            }
        }

        bool ICollection<ZFrame>.IsReadOnly => false;

        #endregion

        #region IEnumerable implementation

        public IEnumerator<ZFrame> GetEnumerator()
        {
            return _frames.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ICloneable implementation

        public object Clone()
        {
            return Duplicate();
        }

        /// <summary>
        /// 重用
        /// </summary>
        /// <returns></returns>
        public ZMessage Duplicate()
        {
            var message = new ZMessage();
            foreach (var frame in this)
            {
                message.Add(frame.Duplicate());
            }
            return message;
        }

        /// <summary>
        /// 重用
        /// </summary>
        /// <returns></returns>
        public ZMessage Duplicate(int start)
        {
            var message = new ZMessage();
            for (var index = start; index < this.Count; index++)
            {
                var frame = this[index];
                message.Add(frame.Duplicate());
            }

            return message;
        }
        public override string ToString()
        {
            var co = new StringBuilder();
            co.AppendLine($"Frames:{Count}");
            foreach (var f in _frames)
            {
                co.AppendLine(f.ReadString());
            }
            return co.ToString();
        }

        #endregion
    }
}