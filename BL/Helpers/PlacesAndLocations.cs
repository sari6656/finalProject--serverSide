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
        public static double getDistance(double p1Lat, double p1Long, double p2Lat, double p2Long)
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

        //פונקציה זו, כל 6 שניות מדפיסה את התאריך
        public static void Timer()
        {
           
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(0.1);

            var timer = new System.Threading.Timer((e) =>
            {   
                Console.Write(DateTime.Now);
            }, null, startTimeSpan, periodTimeSpan);
        }
    }
}
