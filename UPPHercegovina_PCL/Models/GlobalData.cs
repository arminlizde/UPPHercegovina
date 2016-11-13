using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UPPHercegovina_PCL.Util;

namespace UPPHercegovina_PCL.Models
{
    public class GlobalData
    {
        private static GlobalData _instance;
        private static object _syncRoot = new object();

        public AspNetUserViewModel CurrentUser { get; set; }

        public delegate void Event();

        public bool ApplicationStarted { get; set; }

        #region Buyer

        public List<PersonProductViewModel> ProductsInCart { get; set; }

        public bool IsCartClicked { get; set; }

        public bool ShowCartProductDetails { get; set; }

        public bool NumberOfNotificationChanged { get; set; }
        public int NumberOfNotification { get; set; }

        public Timer timer;
        AutoResetEvent autoEvent;

        public Event BuyersNotifications { get; set; }

        #endregion

        #region Producer

        public Event ProducersNotifications { get; set; }

        public List<PersonProductViewModel> AcceptedProductsForDelivery { get; set; }

        public bool IsDelvieryClicked { get; set; }

        public bool NumberOfAcceptedProductsChanged { get; set; }
        public int NumberOfAcceptedProducts { get; set; }

        #endregion

        public string BaseUrl;

        private GlobalData()
        {
            BaseUrl = "http://localhost:57397";
            CurrentUser = new AspNetUserViewModel();

            ProductsInCart = new List<PersonProductViewModel>();
            IsCartClicked = false;
            IsDelvieryClicked = false;

            NumberOfNotificationChanged = false;
            NumberOfNotification = 0;

            NumberOfAcceptedProducts = 0;
            NumberOfAcceptedProductsChanged = false;

            ApplicationStarted = true;

            AcceptedProductsForDelivery = new List<PersonProductViewModel>();

            ShowCartProductDetails = false;
            autoEvent = new AutoResetEvent(false);

            timer = new Timer(OnTick, autoEvent, 1000, 20000);
        }

        private int GetNumberOfNotifications()
        {
            WebAPIHelper _userService = new WebAPIHelper("http://localhost:57397", "api/BuyerRequests");

            if (CurrentUser == null || CurrentUser.Id == "")
                return 0;

            var response = _userService
                .GetActionResponse("NumberOfAcceptedReservations", CurrentUser.Id)
                .Content.ReadAsStringAsync().Result;

            return Convert.ToInt32(response);
        }

        private void OnTick(object state)
        {
            if (!ApplicationStarted)
            {
                if (CurrentUser.Id != "")
                {
                    if (CurrentUser.Role == "Nakupac")
                    {
                        var lastNumber = NumberOfNotification;
                        NumberOfNotification = GetNumberOfNotifications();

                        if (NumberOfNotification > lastNumber)
                        {
                            NumberOfNotificationChanged = true;
                            if (BuyersNotifications != null)
                                BuyersNotifications();
                        }
                    }
                    else
                    {
                        var lastNumber = NumberOfAcceptedProducts;
                        NumberOfNotification = GetNumberOfAcceptedProducts();

                        if (NumberOfNotification > lastNumber)
                        {
                            NumberOfAcceptedProductsChanged = true;
                            if (ProducersNotifications != null)
                                ProducersNotifications();
                        }
                    }
                }
            }
        }

        private int GetNumberOfAcceptedProducts()
        {
            WebAPIHelper _userService = new WebAPIHelper("http://localhost:57397", "api/PersonProducts");

            if (CurrentUser == null || CurrentUser.Id == "")
                return 0;

            var response = _userService
                .GetActionResponse("GetAcceptedProducts", CurrentUser.Id)
                .Content.ReadAsStringAsync().Result;

            return Convert.ToInt32(response);
        }

        private void BuyerCheckForNotifications()
        {

        }

        public static GlobalData Instance
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new GlobalData();
                    }
                    return _instance;
                }
            }
        }



        public void Clear()
        {
            //odraditi clear funkciju
        }

    }
}
