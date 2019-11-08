using DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;
using System.Data.SqlClient;

namespace BL.Helpers
{
    public class PlacesAndLocations
    {
        static ProjectEntities db = new ProjectEntities();
        //הגרלת מיקום משתמש
        public static ShopDetailsForUsers GetRandomLocation()
        {
            int count = db.Shops.Count();
            Random random = new Random();
            int r = random.Next() % count;
            Shop shop = db.Shops.ToList()[r];
            return new ShopDetailsForUsers()
            {
                AddressString = shop.addressString,
                Latitude = shop.latitude,
                Longitude = shop.longitude
            };
        }
        //פונקציית עזר לפונקציה המחזירה מרחק
        private static double rad(double x)
        {
            return x * Math.PI / 180;
        }
        // הפונקציה מחזירה מרחק בין שני מיקומים במטרים
        private static double getDistance(double p1Lat, double p1Long, double p2Lat, double p2Long)
        {
            int R = 6378137;// Earth’s mean radius in meter
            double dLat = rad(p2Lat - p1Lat);
            double dLong = rad(p2Long - p1Long);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(rad(p1Lat)) * Math.Cos(rad(p2Lat)) *
                Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c;
            return d;// returns the distance in meter
        }

        public static WebResult<ShopDetailsForUsers> CheckDistance(UserIdWithLocation userIdWithLocation)
        {
            double lat = userIdWithLocation.Lat;
            double lng = userIdWithLocation.Lng;
            var codeUser = db.Users.First(f => f.passwordUser == userIdWithLocation.Uuid).codeUser;
            foreach (var search in db.Searches)
            {
                //only if the search is from this user and its status is 0, to find
                if (search.codeUser == codeUser && search.status == 0)
                {
                    foreach (var shop in db.Shops)
                    {
                        //if distance is less than 1000 meter
                        if (getDistance(lat, lng, shop.latitude, shop.longitude) < 1000)
                        {
                            //if there is the category that the user search in that shop
                            if (Casting.ShopCast.GetShopDTO(shop).Categories.FirstOrDefault(f => f.codeCategory == search.codeCategory) != null)
                            {
                                return new WebResult<ShopDetailsForUsers>()
                                {
                                    Status = true,
                                    Message = "found shop",
                                    Value = new ShopDetailsForUsers()
                                    {
                                        AddressString = shop.addressString,
                                        NameShop = shop.nameShop,
                                        PhoneShop = shop.phoneShop,
                                        FromHour = shop.fromHour,
                                        ToHour = shop.toHour,
                                        Latitude = shop.latitude,
                                        Longitude = shop.longitude
                                    }
                                };
                            }

                        }
                    }
                }
            }
            return new WebResult<ShopDetailsForUsers>()
            {
                Status = false,
                Message = "not found close shop",
                Value = null
            };
        }

        //פונקציה זו, כל 6 שניות מדפיסה את התאריך
        //public static void Timer()
        //{

        //    var startTimeSpan = TimeSpan.Zero;
        //    var periodTimeSpan = TimeSpan.FromMinutes(0.1);

        //    var timer = new System.Threading.Timer((e) =>
        //    {   
        //        Console.Write(DateTime.Now);
        //    }, null, startTimeSpan, periodTimeSpan);
        //}
    }
}
