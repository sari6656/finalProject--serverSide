using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DAL;
using BL.Casting;
using System.Collections;
using System.Data;
using System.Web;
using BL.Helpers;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Mail;
namespace BL
{
    public class Shops
    {

        //Register
        public async static Task<WebResult<LoginData<ShopDTO>>> Register(ShopDTO shopDto, Uri request)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                if (db.Shops.FirstOrDefault(w => w.passwordShop == shopDto.passwordShop) != null ||
                db.Shops.FirstOrDefault(w => w.mailShop == shopDto.mailShop) != null)//אם יש כבר  כזה מייל או כזו סיסמה  

                    return new WebResult<LoginData<ShopDTO>>
                    {
                        Message = "אחד מהפרטים שהוקשו כבר קיים במערכת",
                        Status = false,
                        Value = null
                    };
                List<CategoryDTO> sourceCats = shopDto.Categories;
                List<Category_to_shop> category_To_Shops = new List<Category_to_shop>();
                foreach (var item in sourceCats)
                {
                    db.Category_to_shop.Add(new Category_to_shop() { codeCategory = item.codeCategory, codeShop = shopDto.codeShop });
                }

                db.Shops.Add(ShopCast.GetShop(shopDto));
                if (db.SaveChanges() > 0)//בדיקה שהמידע נשמר
                {
                    var accessToken = await GetTokenDataAsync(shopDto.mailShop, shopDto.passwordShop, request);

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        return new WebResult<LoginData<ShopDTO>>
                        {
                            Status = true,
                            Message = "התחברת בהצלחה",
                            Value = new LoginData<ShopDTO>
                            {
                                TokenJson = accessToken,
                                objectDTO = shopDto
                            }
                        };

                    }
                }
                return new WebResult<LoginData<ShopDTO>>
                {
                    Status = false,
                    Message = "ההרשמה נכשלה",
                    Value = null
                };
            }
        }
        //Login
        public static async Task<WebResult<LoginData<ShopDTO>>> Login(string mail, string password, Uri requestUri)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var shop = db.Shops.Where(w => w.mailShop == mail && w.passwordShop == password).FirstOrDefault();
                if (shop != null)//אם המשתמש קיים במאגר המשך לקבלת טוקן, אחרת החזר שגיאה שהמתשמש לא קיים
                {
                    ShopDTO shopDto = ShopCast.GetShopDTO(shop);

                    List<int> codesCategories = db.Category_to_shop.Where(c => c.codeShop == shop.codeShop).Select(x => x.codeCategory).ToList();
                    //Category category;
                    //shopDto.Categories = new List<CategoryDTO>();
                    //foreach (var item in codesCategories)
                    //{
                    //    category = db.Categories.Find(item);

                    //    if (category != null)
                    //        shopDto.Categories.Add(CategoryCast.GetCategoryDTO(category));
                    //}
                    var accessToken = await GetTokenDataAsync(shopDto.mailShop, shopDto.passwordShop, requestUri);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        return new WebResult<LoginData<ShopDTO>>
                        {
                            Status = true,
                            Message = "התחברת בהצלחה",
                            Value = new LoginData<ShopDTO>
                            {
                                TokenJson = accessToken,
                                objectDTO = shopDto
                            }
                        };
                    }
                }
                return new WebResult<LoginData<ShopDTO>>
                {
                    Status = false,
                    Message = " אין משתמש רשום בשם וסיסמא זו  ",
                    Value = null
                };
            }
        }
        //Update shop with categories
        public static WebResult<ShopDTO> Update(ShopDTO shopDTO)
        {
            using (ProjectEntities db = new ProjectEntities())
            {

                Shop shop = db.Shops.FirstOrDefault(f => f.mailShop == shopDTO.mailShop);
                //למייל אסור להשתנות
                if (shop == null)
                    return new WebResult<ShopDTO>
                    {
                        Message = "החנות לא נמצאה",
                        Status = false,
                        Value = null
                    };
                shop.nameShop = shopDTO.nameShop;
                shop.phoneShop = shopDTO.phoneShop;
                shop.latitude = shopDTO.latitude;
                shop.longitude = shopDTO.longitude;
                shop.fromHour = shopDTO.fromHour;
                shop.toHour = shopDTO.toHour;
                shop.addressString = shopDTO.addressString;
                List<CategoryDTO> sourceCats = shopDTO.Categories;
                //למחוק קודם את כל הקטגוריות של החנות
                List<Category_to_shop> category_To_Shops = db.Category_to_shop.ToList();
                category_To_Shops.ForEach(item =>
                {
                    if (item.codeShop == shop.codeShop)
                        db.Category_to_shop.Remove(item);
                });

                db.SaveChanges();

                // להוסיף את כל הקטגוריות החדשות לחנות
                foreach (var item in sourceCats)
                {
                    db.Category_to_shop.Add(new Category_to_shop() { codeCategory = item.codeCategory, codeShop = shop.codeShop });
                }

                db.SaveChanges();
                return new WebResult<ShopDTO>
                {
                    Message = "הנתונים התעדכנו בהצלחה",
                    Value = shopDTO,
                    Status = true
                };
            }
        }
        //Send email for request category
        public static WebResult<string> SendEmailForNewCategory(string categoryName, [UserLogged] ShopDTO shopDTO)
        {

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("rememberproject2019@gmail.com");
                mail.To.Add("shlomomolev@gmail.com");
                mail.Subject = "הוספת קטגוריה חדשה";
                mail.Body = shopDTO?.nameShop + "    מבקש להוסיף את הקטגוריה " + categoryName + " לרשימת הקטגוריות הכללית ";
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("rememberproject2019@gmail.com", "19283746!");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                return new WebResult<string>
                {
                    Message = $"הבקשה להוספת הקטגוריה {categoryName} לא נשלחה ",
                    Status = false,
                    Value = null
                };
            }
            return new WebResult<string>
            {
                Message = $"הבקשה להוספת הקטגוריה {categoryName}  נשלחה בהצלחה",
                Status = true,
                Value = categoryName
            };
        }
        //Returns the searches that found in that shop
        public static WebResult<SearchesForShop> getSearchesForShop([UserLogged] ShopDTO shopDTO)
        {
            using (ProjectEntities db = new ProjectEntities())
            {

                SearchesForShop searchesForShop = new SearchesForShop();
                foreach (var category in shopDTO.Categories)
                {

                    //הוספת שם הקטגוריה
                    searchesForShop.namesCategories.Add(category.nameCategory);
                    //מתחילים לספור כמה יש מהקטגוריה הזו
                    int counter = 0;
                    foreach (var search in db.Searches)
                    {
                        if (search.codeCategory == category.codeCategory && search.codeShop == shopDTO.codeShop)
                            counter++;
                    }
                    searchesForShop.numbersCategories.Add(counter);
                }

                return new WebResult<SearchesForShop>
                {
                    Message = "רשימת החיפושים לחנות נשלחה בהצלחה",
                    Status = true,
                    Value = searchesForShop
                };
            }
        }

        //Returns the categories for choosing
        public static WebResult<List<CategoryDTO>> GetAllCategories()
        {
            using (ProjectEntities db = new ProjectEntities())
            {

                return new WebResult<List<CategoryDTO>>
                {
                    Message = "רשימת קטגוריות כללית נשלחה בהצלחה",
                    Value = CategoryCast.GetCategoriesDTO(db.Categories.ToList()),
                    Status = true
                };
            }
        }

        //Logout
        public static WebResult<string> Logout([UserLogged] ShopDTO shopDTO)
        {
            //get user data
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var claim = identity.Claims.Where(c => c.Type == ClaimTypes.Name).SingleOrDefault();
            if (claim != null)
            { //remove user data
                identity.RemoveClaim(claim);
                return new WebResult<string>
                {
                    Status = true,
                    Message = $"{shopDTO.nameShop} התנתק בהצלחה",
                    Value = null
                };
            }
            return new WebResult<string>
            {
                Status = false,
                Message = $"ההתנתקות נכשלה",
                Value = null
            };
        }
        //Saves who is the shop now
        private static async Task<string> GetTokenDataAsync(string username, string password, Uri req)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(req.Scheme + "://" + req.Authority + "/token");
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("UserName", username));
            postData.Add(new KeyValuePair<string, string>("Password", password));
            postData.Add(new KeyValuePair<string, string>("grant_type", "password"));//don't dare to change that!!!
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response = await httpClient.PostAsync("token", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
