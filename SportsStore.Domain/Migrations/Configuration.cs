namespace SportsStore.Domain.Migrations
{
    using SportsStore.Domain.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SportsStore.Domain.Concrete.EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SportsStore.Domain.Concrete.EFDbContext context)
        {          
            // 加入初始資料
            context.Products.AddOrUpdate(prod=>prod.ProductID,new Product[]
            {
                new Product()
                {
                     Name="Kayak",
                     Description="A boat for one person",
                     Catagory="Watersports",
                     Price=275M
                },
                new Product()
                { 
                    Name="Lifejacket",
                    Description="Protective and fashionable",
                    Catagory="Watersports",
                    Price=48.95M
                },
                new Product()
                { 
                    Name="Soccer Ball",
                    Description="FIFA-approved size and weight",
                    Catagory="Soccer",
                    Price=19.50M
                },
                new Product()
                {
                    Name="Corner Flags",
                    Description="Give your playing field a professional touch",
                    Catagory="Soccer",
                    Price=34.95M
                },
                new Product()
                {
                    Name="Stadium",
                    Description="Flat-packed35,000-seat stadium",
                    Catagory="Soccer",
                    Price=79500.00M
                },
                new Product()
                {
                    Name="Thinking Cap",
                    Description="Improve your brain efficiency by 75%",
                    Catagory="Chess",
                    Price=16.00M
                },
                new Product()
                {
                    Name="Unsteady Chair",
                    Description="Secretly give your opponent a disadvantage",
                    Catagory="Chess",
                    Price=29.95M
                },
                new Product()
                {
                    Name="Human Chess Board",
                    Description="A fun game for the family",
                    Catagory="Chess",
                    Price=75.00M
                },
                new Product()
                {
                    Name="Bling-Bling King",
                    Description="Gold-plated, diamond-studded King",
                    Catagory="Chess",
                    Price=1200.00M
                },
            });
        }
    }
}
