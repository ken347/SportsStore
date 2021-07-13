using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage ="請輸入姓名")]
        [Display(Name ="姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage ="請填寫第一個地址")]
        [Display(Name = "地址第一行")]
        public string Line1 { get; set; }
        [Display(Name = "地址第二行")]
        public string Line2 { get; set; }
        [Display(Name = "地址第三行")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "請輸入城市名稱")]
        [Display(Name = "城市")]
        public string City { get; set; }

        [Required(ErrorMessage = "請輸入州名")]
        [Display(Name = "州")]
        public string State { get; set; }
        [Display(Name = "郵遞區號")]
        public string Zip { get; set; }

        [Required(ErrorMessage ="請輸入國家名稱")]
        [Display(Name = "國家")]
        public string Country { get; set; }
        [Display(Name = "包裝這些商品")]
        public bool GiftWrap { get; set; }


    }
}
