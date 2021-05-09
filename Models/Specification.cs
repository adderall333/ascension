using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Attributes;

namespace Models
{
    public class Specification : IModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        
        [SimpleProperty]
        public string Name { get; set; }
        
        [NotAdministered]
        public int CategoryId { get; set; }
        
        [ManyToOne]
        public Category Category { get; set; }
        
        [OneToMany]
        public IEnumerable<SpecificationOption> SpecificationOptions { get; set; }
        
        public override string ToString()
        {
            return $"{Id}.{Name}";
        }
        
        public static IModel GetModel(int id)
        {
            var context = new ApplicationContext();
            return context
                .Specification
                .Include(specification => specification.Category)
                .Include(specification => specification.SpecificationOptions)
                .First(specification => specification.Id == id);
        }

        public Specification(string name, int category, List<int> specificationOptions, ApplicationContext context = null)
        {
            Name = name;
            
            if (category > 0)
                Category = context?.Category.First(c => c.Id == category);
                
            if (specificationOptions.Any())
                SpecificationOptions = context?.SpecificationOption.Where(sOp => specificationOptions.Contains(sOp.Id));
        }
        
        public void Update(string name, int category, List<int> specificationOptions, ApplicationContext context = null)
        {
            Name = name;
            
            if (category > 0)
                Category = context?.Category.First(c => c.Id == category);
                
            if (specificationOptions.Any())
                SpecificationOptions = context?.SpecificationOption.Where(sOp => specificationOptions.Contains(sOp.Id));
        }

        public Specification()
        {
        }
    }
}