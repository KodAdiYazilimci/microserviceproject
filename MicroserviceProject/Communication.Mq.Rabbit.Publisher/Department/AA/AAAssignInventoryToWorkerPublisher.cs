﻿using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.AA;
using Communication.Mq.Rabbit.Publisher.Department.AA.Models;

using System;

namespace Communication.Mq.Rabbit.Publisher.Department.AA
{
    /// <summary>
    /// Çalışana envanter ekleyecek rabbit kuyruğuna yeni bir kayıt ekler
    /// </summary>
    public class AAAssignInventoryToWorkerPublisher : BasePublisher<WorkerModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Çalışana envanter ekleyecek rabbit kuyruğuna yeni bir kayıt ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public AAAssignInventoryToWorkerPublisher(
            AAAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration)
            : base(rabbitConfiguration)
        {

        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;
                }
            }
        }
    }
}