using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Casting
{
    // מושלם צריך לבדוק,מחזיר רשימה של חנויות ורשימה של חיפושים 
    public class CategoryCast
    {
        public static ProjectEntities db = new ProjectEntities();
        public static CategoryDTO GetCategoryDTO(Category category)
        {
            List<int> codesShops = db.Category_to_shop.Where(c => c.codeCategory == category.codeCategory).Select(x => x.codeShop).ToList();     
            
            return new CategoryDTO()
            {
                codeCategory = category.codeCategory,
                nameCategory = category.nameCategory
            };
        }
        public static Category GetCategory(CategoryDTO category)
        {
            return new Category() 
            {
                codeCategory = category.codeCategory,
                nameCategory = category.nameCategory
            };
        }
        //המרת רשימה שלמה
        public static List<CategoryDTO> GetCategoriesDTO(List<Category> categories)
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            foreach (var item in categories)
            {
                categoryDTOs.Add(new CategoryDTO()
                    { codeCategory = item.codeCategory, nameCategory = item.nameCategory });
            }
            return categoryDTOs;
        }
    }
}
