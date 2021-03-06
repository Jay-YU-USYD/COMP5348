﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeliveryCo.Services.Interfaces;
using DeliveryCo.Business.Components.Interfaces;
using System.ServiceModel;
using Microsoft.Practices.ServiceLocation;
using DeliveryCo.MessageTypes;
using System.Collections;

namespace DeliveryCo.Services
{
    public class DeliveryService : IDeliveryService
    {
        private IDeliveryProvider DeliveryProvider
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IDeliveryProvider>();
            }
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Guid SubmitDelivery(DeliveryInfo pDeliveryInfo)
        {
            Console.Out.WriteLine("(" + DateTime.Now + ") Submitting delivery " + pDeliveryInfo.DeliveryIdentifier);
            return DeliveryProvider.SubmitDelivery(
                MessageTypeConverter.Instance.Convert<DeliveryCo.MessageTypes.DeliveryInfo, 
                DeliveryCo.Business.Entities.DeliveryInfo>(pDeliveryInfo)                
            );
        }
        public List<DeliveryInfo> getAllDelivery()
        {
            Console.Out.WriteLine("(" + DateTime.Now + ") Obtaining all deliveries.");
            var list = DeliveryProvider.getAllDelivery();
            List<DeliveryInfo> result = new List<DeliveryInfo>();
            foreach (var a in list) {
                result.Add(MessageTypeConverter.Instance.Convert<DeliveryCo.Business.Entities.DeliveryInfo,
                    DeliveryCo.MessageTypes.DeliveryInfo>(a));
            }
            return result;
        }

        public Guid RefundDelivery(String pDeliveryInfo) {
            Console.Out.WriteLine("(" + DateTime.Now + ") Refunding delivery " + pDeliveryInfo);
            return DeliveryProvider.RefundDelivery(pDeliveryInfo);
        }

        public void Ping()
        {
            // Does nothing
        }

    }
}
