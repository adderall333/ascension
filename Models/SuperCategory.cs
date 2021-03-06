﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Attributes;

namespace Models
{
    public class SuperCategory : IModel, ICategory
    {
        [PrimaryKey]
        public int Id { get; set; }
        
        [SimpleProperty]
        public string Name { get; set; }
                
        [ImageProperty]
        public string ImagePath { get; set; }
        
        [OneToMany]
        public List<Category> Categories { get; set; }
        
        public override string ToString()
        {
            return $"{Id}.{Name}";
        }
        
        public static IModel GetModel(int id)
        {
            var context = new ApplicationContext();
            return context
                .SuperCategory
                .Include(superCategory => superCategory.Categories)
                .First(superCategory => superCategory.Id == id);
        }

        public SuperCategory Update(string name, string imagePath, ApplicationContext context)
        {
            Name = name;
            ImagePath = string.IsNullOrEmpty(imagePath) ? ImagePath : imagePath;
            
            return this;
        }

        public SuperCategory()
        {
        }
    }
}