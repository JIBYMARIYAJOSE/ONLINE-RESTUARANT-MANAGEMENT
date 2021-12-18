using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace firstProjectWith_ASP.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ShortDesc { get; set; }
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        public string Image { get; set; }
        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Application Type")]
        public int ApplicationTypeId { get; set; }
        //IF A error occurs in the migration no need to change the migration but make changes in the model and create a new migration and then update

        [ForeignKey("ApplicationTypeId")]//step1: add as foreign key
        //step 2: Add migration to push the changes to the database
        public virtual ApplicationType ApplicationType { get; set; }

    }
}