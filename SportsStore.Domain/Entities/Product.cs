using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportsStore.Domain.Entities
{
    public class Product
    {
        [Key]
        [HiddenInput(DisplayValue=false)]
        public int ProductID { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name="名稱")]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "描述")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "單價")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "分類")]
        public string Catagory { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}
