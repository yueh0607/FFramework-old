//using System;
//using System.Collections.Generic;

//namespace FFramework
//{

//    /// <summary>
//    /// 邮政系统
//    /// </summary>
//    public class MailSystem
//    {
//        private readonly Dictionary<long, MailBox> post = new Dictionary<long, MailBox>();
//        private readonly object post_locker = new object();


//        public MailSystem() { }

//        /// <summary>
//        /// 建立邮箱
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="name"></param>
//        public void Create<T>(FThread thread) where T : MailBox, new()
//        {
//            long address = thread.UID;
//            lock (post_locker)
//            {
//                if (post.ContainsKey(address))
//                    throw new ArgumentException("There is a same address existed;");
//                T mailbox = new T();
//                post.Add(address, mailbox);
//            }
//        }

//        /// <summary>
//        /// 获取邮箱地址
//        /// </summary>
//        /// <param name="address">邮箱地址</param>
//        /// <returns></returns>
//        public MailBox? RefOf(FThread thread)
//        {
//            long address = thread.UID;
//            lock (post_locker)
//            {
//                post.TryGetValue(address, out MailBox? mailbox);
//                return mailbox;
//            }
//        }
//        /// <summary>
//        /// 获取邮箱地址
//        /// </summary>
//        /// <param name="address">邮箱地址</param>
//        /// <returns></returns>
//        public T? RefOf<T>(FThread thread) where T : MailBox
//        {
//            return (T?)RefOf(thread);
//        }

//        /// <summary>
//        /// 删除邮箱
//        /// </summary>
//        /// <param name="address"></param>
//        /// <exception cref="ArgumentException"></exception>
//        public void Remove(FThread thread)
//        {
//            long address = thread.UID;
//            lock (post_locker)
//            {
//                if (!post.ContainsKey(address))
//                    throw new ArgumentException("There is no such address existed;");
//                post.Remove(address);
//            }
//        }

//        /// <summary>
//        /// 关闭邮政网络
//        /// </summary>
//        public void Terminate()
//        {

//        }
//    }
//}
