using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web.Security;
using System.Security.Claims;


namespace UPPHercegovina.WebApplication.Models
{
    //singleton pattern
    public class GlobalData
    {
        private static GlobalData _instance;
        private static object _syncRoot = new object();

        public Timer timer;

        public List<PersonProduct> forDelivery;
        public List<PersonProduct> ReservedProducts;

        //Administrator
        public int CountOfNewProducts;
        public int CountOfNewAppointments;
        public int CountOfNewBuyerRequests;

        //Buyer
        public int CountOfAcceptedReservations;
        public string DateOfNextPickUpStr;
        public DateTime DateOfNextPickUp;

        //User
        public int CountOfAcceptedProducts;
        public int CountOfAcceptedDeliveries;
        public string DateOfNextDeliveryStr;
        public DateTime DateOfNextDelivery;    

        public string CurrentUserId;

        public bool IsBuyer = false;
        public bool IsAdmin = false;
        public bool IsProducer = false;

        private GlobalData()
        {
            forDelivery = new List<PersonProduct>();
            ReservedProducts = new List<PersonProduct>();

            timer = new Timer(10);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 5000;
            timer.Enabled = true;
            timer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(CurrentUserId))
            {
                if(IsAdmin)
                {
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        CountOfNewProducts = context.PersonProducts.Where(p => p.Status == true)
                            .Where(p => p.Accepted == false).Count();
            
                        CountOfNewAppointments = context.Appointments.Where(t => t.Status == true).Count();

                        CountOfNewBuyerRequests = context.BuyerRequests.Where(r => r.Status == true).Where(r => r.Accepted == false).Count();
                    }

                }
                else if(IsBuyer)
                {
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                            var requests = context.BuyerRequests
                                 .Where(b => b.BuyerId == CurrentUserId)
                                 .Where(b => b.Status == true && b.Accepted == true).ToList();

                        CountOfAcceptedReservations = requests.Count();

                        DateOfNextPickUp = requests.Select(r => r.DateOfPickUp).Min();
                        DateOfNextPickUpStr = DateOfNextPickUp.ToShortDateString();

                    }
                }
                else if(IsProducer)
                {
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        var appointments = context.Appointments
                        .Where(a => a.UserId == CurrentUserId && a.Status == true).ToList();

                        CountOfAcceptedDeliveries = appointments.Count();

                        DateOfNextDelivery = appointments.Select(a => a.DeliveryDate).Min();
                        DateOfNextDeliveryStr = DateOfNextDelivery.ToShortDateString();

                        CountOfAcceptedProducts = context.PersonProducts
                           .Where(p => p.Status == true && p.Accepted == true)
                           .Where(p => p.UserId == CurrentUserId).Count();
                    }

                }
            }

        }

        protected bool IsInRole(string userId, string roleName)
        {
            ApplicationDbContext _dbContextUsers = new ApplicationDbContext();

            var roles = _dbContextUsers.Roles.FirstOrDefault(p => p.Name.Equals(roleName));
            if (roles != null)
                return roles.Users.Any(p => p.UserId.Equals(userId));

            return false;
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
    }
}