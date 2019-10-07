using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Casting
{//מושלם צריך לבדוק, אין כאן שום רשימה של מחלקה אחרת
    class SearchCast
    {
        public static SearchDTO GetSearchDTO(Search search)
        {
            return new SearchDTO()
            {
                codeSearch = search.codeSearch,
                codeUser = search.codeUser,
                nameProduct = search.nameProduct,
                codeCategory=search.codeCategory,
               status = search.status,
                codeShop=search.codeShop
            };
        }
        public static Search GetSearch(SearchDTO search)
        {
            return new Search()
            {
                codeSearch = search.codeSearch,
                codeUser = search.codeUser,
                nameProduct = search.nameProduct,
                codeCategory=search.codeCategory,
                status = search.status,
                codeShop=search.codeShop
            };
        }
        //המרת רשימה שלמה
        public static List<SearchDTO> GetSearchesDTO(List<Search> searches)
        {
            List<SearchDTO> searchDTOs = new List<SearchDTO>();
            foreach (var item in searches)
            {
                searchDTOs.Add(new SearchDTO()
                {
                    codeSearch = item.codeSearch,
                    codeUser = item.codeUser,
                    nameProduct = item.nameProduct,
                    codeCategory = item.codeCategory,
                    status = item.status,
                    codeShop = item.codeShop
                });
            }
            return searchDTOs;
        }
    }
}
