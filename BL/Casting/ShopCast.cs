using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Casting
{//מושלם צריך לבדוק, ובעיון
    class ShopCast
    {
        public static ProjectEntities db = new ProjectEntities();
        public static ShopDTO GetShopDTO(Shop shop)
        {
            var cat = CategoryCast.GetCategoriesDTO(shop.Category_to_shop.Select(s => s.Category).ToList());
            return new ShopDTO()
            {
                codeShop = shop.codeShop,
                nameShop = shop.nameShop,
                passwordShop = shop.passwordShop,
                phoneShop = shop.phoneShop,
                longitude=shop.longitude,
                latitude=shop.latitude,
                fromHour=shop.fromHour,
                toHour=shop.toHour,
                mailShop=shop.mailShop,
                addressString=shop.addressString,
                Categories=cat
            };
        }
        public static Shop GetShop(ShopDTO shop)
        {
            return new Shop()
            {
                codeShop = shop.codeShop,
                nameShop = shop.nameShop,
                passwordShop = shop.passwordShop,
                phoneShop = shop.phoneShop,
                mailShop=shop.mailShop,
                latitude=shop.latitude,
                longitude=shop.longitude,
                fromHour=shop.fromHour,
                toHour=shop.toHour,
                addressString=shop.addressString
            };
        }
        //המרת רשימה שלמה
        public static List<ShopDTO> GetShopsDTO(List<Shop> shops)
        {
            List<ShopDTO> shopDTOs = new List<ShopDTO>();
            foreach (var item in shops)
            {
                shopDTOs.Add(new ShopDTO()
                {
                    codeShop = item.codeShop,
                    nameShop = item.nameShop,
                    passwordShop = item.passwordShop,
                    phoneShop = item.phoneShop,
                  longitude=item.longitude,
                  latitude=item.latitude,
                    mailShop=item.mailShop,
                    fromHour=item.fromHour,
                    toHour=item.toHour,
                    addressString=item.addressString
                });
            }
            return shopDTOs;
        }
    }
}
