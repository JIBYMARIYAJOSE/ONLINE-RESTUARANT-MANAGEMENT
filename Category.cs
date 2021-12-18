using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace firstProjectWith_ASP.Models
{ //IMPLEMENT DB WITH CODES
    public class Category
    {  //BY DEFAULT THE ID OR THE CATEGORY ID IS THE PRIMARY KEY
      [Required]  [Key] public int Id { get; set; }
        [Required]// validation for the DB to make the property not nullabale
        public string Name { get; set; }
        [Required] [Range(1,int.MaxValue,ErrorMessage ="Display Order for Category must be greater than 0 ")]public int displayOrder { get; set; }
    }
}
