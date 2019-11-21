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

        //הגרלת מיקום משתמש
        public static ShopDetailsForUsers GetRandomLocation()
        {
            using (ProjectEntities db = new ProjectEntities())
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

        //הפונקציה בודקת האם למיקום הזה יש למשתמש חנות קרובה עבור אחת מהקטגוריות
        public static WebResult<List<SearchInShop>> CheckDistance(UserIdWithLocation userIdWithLocation)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                List<SearchInShop> searchesFound = new List<SearchInShop>();
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
                            if (getDistance(lat, lng, shop.latitude, shop.longitude) < search.distance)
                            {
                                //if there is the category that the user search in that shop

                                if (Casting.ShopCast.GetShopDTO(shop).Categories.FirstOrDefault(f => f.codeCategory == search.codeCategory) != null)
                                {
                                    TimeSpan fromHour = TimeSpan.MinValue, toHour = TimeSpan.MaxValue;
                                    TimeSpan now = DateTime.Now.TimeOfDay;
                                    //if shop is opened
                                    if (shop.fromHour != null && shop.toHour != null)
                                    {
                                        fromHour = TimeSpan.Parse(shop.fromHour);
                                        toHour = TimeSpan.Parse(shop.toHour);                               
                                    }
                                    if (fromHour < now && toHour > now)
                                    {
                                        searchesFound.Add(new SearchInShop()
                                        {
                                            CodeSearch = search.codeSearch,
                                            NameProduct = search.nameProduct,
                                            AddressString = shop.addressString,
                                            NameShop = shop.nameShop,
                                            PhoneShop = shop.phoneShop,
                                            MailShop = shop.mailShop,
                                            FromHour = shop.fromHour,
                                            ToHour = shop.toHour
                                        });
                                    }


                                }

                            }
                        }
                    }
                }
                return new WebResult<List<SearchInShop>>()
                {
                    Status = true,
                    Message = "return searches found",
                    Value = searchesFound
                };
            }

        }
    }
}
