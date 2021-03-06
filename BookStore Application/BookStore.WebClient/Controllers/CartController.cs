﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using BookStore.Services.MessageTypes;
using BookStore.WebClient.ClientModels;
using BookStore.WebClient.ViewModels;


namespace BookStore.WebClient.Controllers
{
    public class CartController : Controller
    {
        public ViewResult Index(Cart pCart, string pReturnUrl)
        {
            ViewData["returnUrl"] = pReturnUrl;
            ViewData["CurrentCategory"] = "Cart";
            return View(pCart);
        }

        public RedirectToRouteResult AddToCart(Cart pCart, int pBookId, string pReturnUrl)
        {
            pCart.AddItem(FetchBookById(pBookId), 1);
            return RedirectToAction("Index", new { pReturnUrl });
        }


        public RedirectToRouteResult RemoveFromCart(Cart pCart, int pBookId, string pReturnUrl)
        {
            pCart.RemoveLine(FetchBookById(pBookId));
            return RedirectToAction("Index", new { pReturnUrl });
        }

        public ActionResult CheckOut(Cart pCart, UserCache pUser)

        {
            Object lockObj = new object();
            lock (lockObj)
            {
                try
                {
                    if (pCart.OrderItems.Count > 0)
                    {
                        pCart.SubmitOrderAndClearCart(pUser);
                    }
                    else {
                        return RedirectToAction("DoubleCheckOut");

                    }
                }
                catch
                {
                    pCart.Clear();
                    pUser.UpdateUserCache();
                    return RedirectToAction("ErrorPage");
                }
              
                return View(new CheckOutViewModel(pUser.Model));
            }
        }

        public ActionResult ErrorPage()
        {
            return View();
        }
        public ActionResult DoubleCheckOut()
        {
            return View();
        }

        public ViewResult Summary(Cart pCart)
        {
            return View(pCart);
        }

        public ActionResult InsufficientStock(String pItem)
        {
            return View(new InsufficientStockViewModel(pItem));
        }

        private Book FetchBookById(int pId)
        {
            return ServiceFactory.Instance.CatalogueService.GetBookById(pId);
        }
    }
}