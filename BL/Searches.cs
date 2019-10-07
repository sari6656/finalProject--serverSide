using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DAL;
using BL.Casting;
using BL.Helpers;
using System.Web;

namespace BL
{
    public class Searches
    {
        public static ProjectEntities db = new ProjectEntities();
        //פונקציה שמראה למשתמש את הקטגוריות כדי שיוכל לבחור
        public static WebResult<List<CategoryDTO>> GetCategories()
        {
            return new WebResult<List<CategoryDTO>>
            {
                Message = "רשימת הקטגוריות נשלחה בהצלחה",
                Status = true,
                Value = CategoryCast.GetCategoriesDTO(db.Categories.ToList())
            };
        }
        //יצירה
        public static WebResult<SearchDTO> Create(SearchDTO searchDTO)
        {
            //אילו בדיקות בודקים??????????????????????
            //Has found תמיד צריך להגיע false
            if (HttpContext.Current.Session["User"] == null)
                return new WebResult<SearchDTO>
                {
                    Message = "לא נמצא משתמש פעיל",
                    Status = false,
                    Value = null
                };
            searchDTO.codeUser = (HttpContext.Current.Session["User"] as User).codeUser;
            db.Searches.Add(SearchCast.GetSearch(searchDTO));
            db.SaveChanges();
            return new WebResult<SearchDTO>
            {
                Message = "יצירת חיפוש בוצעה בהצלחה",
                Status = true,
                Value = searchDTO
            };
        }
        //מחיקת חיפוש- המשתמש התחרט
        //החיפוש לא באמת מתבטל אלא הסטטוס משתנה ל2
        public static WebResult<SearchDTO> Delete(int code)
        {
            Search search = db.Searches.Find(code);
            if (search == null)
                return new WebResult<SearchDTO>
                {
                    Message = "לא נמצא חיפוש זה",
                    Status = false,
                    Value = null
                };
            if (search.status == 1)
                return new WebResult<SearchDTO>
                {
                    Message = "אין אפשרות למחו חיפושים שכבר נמצאו",
                    Status = false,
                    Value = null
                };
            search.status = 2;
            db.SaveChanges();
            return new WebResult<SearchDTO>
            {
                Message = "המחיקה בוצעה בהצלחה",
                Status = true,
                Value = SearchCast.GetSearchDTO(search)
            };
        }
        //חיפוש נמצא- אם המשתמש קנה את המוצר
        public static WebResult<SearchDTO> Found(int codeSearch, int codeShop)
        {
            Search search = db.Searches.Find(codeSearch);
            if (search == null)
                return new WebResult<SearchDTO>
                {
                    Message = "לא נמצא חיפוש זה במאגר",
                    Status = false,
                    Value = null
                };
            search.status = 1;
            search.codeShop = codeShop;
            db.SaveChanges();
            return new WebResult<SearchDTO>
            {
                Message = "החיפוש נמצא בהצלחה",
                Status = true,
                Value = SearchCast.GetSearchDTO(search)
            };
        }
        //פונקציה שמחזירה למשתמש את כל החיפושים שלו כולל אלו שמצא
        //מה עדיף? לקבל אובייקט של משתמש או רק המזהה, כי אם נקבל אובייקט נוכל מיד לגשת לרשימה של החיפושים???????????
        public static WebResult<List<SearchDTO>> GetSearchesByUserId()
        {
            if ((HttpContext.Current.Session["User"] as User)==null)
                return new WebResult<List<SearchDTO>>
                {
                    Message = "שגיאת אבטחה, משתמש לא תואם",
                    Value = null,
                    Status = false
                };
            User CurrentUser = HttpContext.Current.Session["User"] as User;
            List<Search> searches = db.Searches.Where(w => w.codeUser == CurrentUser.codeUser && w.status!=2).ToList();
            return new WebResult<List<SearchDTO>>
            {
                Message = "חיפושי המשתמש נשלחו בהצלחה",
                Value = SearchCast.GetSearchesDTO(searches),
                Status = true
            };
        }
        //פונקציה שמחזירה למשתמש את כל החיפושים שעדיין לא נמצאו
        public static WebResult<List<SearchDTO>> GetNotfoundSearchesByUserId(int userId)
        {
            if ((HttpContext.Current.Session["User"] as User).codeUser != userId)
                return new WebResult<List<SearchDTO>>
                {
                    Message = "שגיאת אבטחה, משתמש לא תואם",
                    Value = null,
                    Status = false
                };
            return new WebResult<List<SearchDTO>>
            {
                Message = "חיפושי המשתמש שלא נמצאו נשלחו בהצלחה",
                Value = SearchCast.GetSearchesDTO(db.Searches.Where(w => w.codeUser == userId && w.status == 0).ToList()),
                Status = true
            };
        }
        //פונקציה שמחזירה למשתמש את כל החיפושים שנמצאו
        public static WebResult<List<SearchDTO>> GetfoundSearchesByUserId(int userId)
        {
            if ((HttpContext.Current.Session["User"] as User).codeUser != userId)
                return new WebResult<List<SearchDTO>>
                {
                    Message = "שגיאת אבטחה, משתמש לא תואם",
                    Value = null,
                    Status = false
                };
            return new WebResult<List<SearchDTO>>
            {
                Message = "חיפושי המשתמש שנמצאו נשלחו בהצלחה",
                Value = SearchCast.GetSearchesDTO(db.Searches.Where(w => w.codeUser == userId && w.status == 1).ToList()),
                Status = true
            };
        }
        //מחזירה את כל החנויות שמוכרות קטגוריה מסוימת
        public static WebResult<List<ShopDetailsForUsers>> GetShopsForCategory(int codeCategory)
        {
            List<int> codeShops = new List<int>();
            Shop shop;
            List<ShopDetailsForUsers> shopsToCategory = new List<ShopDetailsForUsers>();
            codeShops = db.Category_to_shop.Where(w => w.codeCategory == codeCategory).Select(s => s.codeShop).ToList();
            if (codeShops.Count == 0)
                return new WebResult<List<ShopDetailsForUsers>>()
                {
                    Status = false,
                    Message = "לא נמצאה חנות שמוכרת קטגוריה זו",
                    Value = null
                };
            foreach (var code in codeShops)
            {
                shop = db.Shops.Find(code);
                shopsToCategory.Add(new ShopDetailsForUsers()
                {
                    NameShop = shop.nameShop,
                    AddressString = shop.addressString,
                    FromHour = shop.fromHour,
                    ToHour = shop.toHour,
                    Latitude = shop.latitude,
                    Longitude = shop.longitude,
                    PhoneShop = shop.phoneShop
                });
            }
            return new WebResult<List<ShopDetailsForUsers>>()
            {
                Status = true,
                Message = "להלן החנויות בהתאם לקטגוריה",
                Value = shopsToCategory
            };
        }


    }
}
