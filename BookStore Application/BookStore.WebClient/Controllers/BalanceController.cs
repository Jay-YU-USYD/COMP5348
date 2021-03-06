﻿using BookStore.Business.Components;
using BookStore.Services.MessageTypes;
using BookStore.WebClient.ClientModels;
using DeliveryCo.MessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BookStore.WebClient.Controllers
{
    public class BalanceController : Controller
    {
        // GET: Balance
        public ActionResult Index(String UserId)
        {
            User user = ServiceFactory.Instance.UserService.ReadUserById(int.Parse(UserId));

            // The transfer service may not be available, show an error message if it is not
            double balance = -1;
            try 
            {
                ServiceFactory.Instance.UserService.Log("(" + DateTime.Now + ") Request made to show the balance of user " + user.Id);
                balance = ExternalServiceFactory.Instance.TransferService.ShowBalance(user.Id); 
            }
            catch 
            {
                ServiceFactory.Instance.UserService.Log("(" + DateTime.Now + ") Request to show the balance of user " + user.Id + " failed. Bank Service may be unavailable.");
            }
            if (balance == -1) ViewBag.Message = "not currently available as the Bank Service is down.";
            else ViewBag.Message = "$" + balance;
       
            return View(ViewBag);
        }
    }
}