using AlmacenWebService.Entities.Abstactions;
using AlmacenWebService.Utilities;
using System.ComponentModel.DataAnnotations;

namespace AlmacenWebService.Entities
{
    public class Category: ICategory
    {
        public int Id { get; set; }

        [Required]
        [FirstLetterShouldBeUpperCaseAtribute]
        [StringLength(25)]
        public string Name { get; set; }


        public Category() { }
        public Category(string name)
        {
            Name = name;
        }
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
