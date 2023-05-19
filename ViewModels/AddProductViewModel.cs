using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using ModernWpf.Controls;
using Stylet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WriteDry.Services;
using WriteDry.ViewModels.Framework;
using WriteDry.Views.Dialogs;

namespace WriteDry.ViewModels
{
    public class AddProductResult
    {
        public Product product;
        public string name;
    }

    public class AddProductViewModel : DialogScreen<AddProductResult>
    {
        public List<Pmanufacturer> Manufacturers { get; set; }
        public List<Pcategory> Categories { get; set; }
        public List<Provider> Providers { get; set; }
        public int SelectedCategory { get; set; }
        public int SelectedManufacturer { get; set; } = -1;
        public int SelectedProvider { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int QunatityInStock { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Article { get; set; } = "";
        public string PhotoPath { get; set; } = "";
        public string NewManufacturer { get; set; } = "";

        private ApplicationContext dbContext;
        public AddProductViewModel(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void PickPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.Title = "Выберите изображение для товара";
            openFileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";
            if (openFileDialog.ShowDialog() ?? false)
                PhotoPath = openFileDialog.FileName;
        }
        public override async void OnLaunch(ContentDialog contentDialog)
        {
            Manufacturers = await dbContext.Pmanufacturers.ToListAsync();
            Categories = await dbContext.Pcategories.ToListAsync();
            Providers = await dbContext.Providers.ToListAsync();
            this.root = contentDialog;
        }
        
        public async void AddManufacturer()
        {
            if (string.IsNullOrWhiteSpace(NewManufacturer)) return;
            var manufacturer = new Pmanufacturer { ProductManufacturer = NewManufacturer };
            var resolved_manufacturer = dbContext.Pmanufacturers.Add(manufacturer);
            await dbContext.SaveChangesAsync();
            Manufacturers = await dbContext.Pmanufacturers.ToListAsync();
            SelectedManufacturer = resolved_manufacturer.Entity.PmanufacturerId;
            NewManufacturer = string.Empty;
        }

        public async void DeleteManufacturer()
        {
            if (SelectedManufacturer == -1) return;
            var manufacturer = dbContext.Pmanufacturers.Find(SelectedManufacturer);
            try
            {
                dbContext.Pmanufacturers.Remove(manufacturer);
                await dbContext.SaveChangesAsync();
                Manufacturers = await dbContext.Pmanufacturers.ToListAsync();
                SelectedManufacturer = 0;
            }
            catch
            {
                dbContext.DetachAllEntities();
            }
        }

        public void Apply()
        {
            if (!string.IsNullOrWhiteSpace(PhotoPath))
            {
                var filename = Path.GetFileName(PhotoPath);
                var resourceName = Directory.GetCurrentDirectory() + "\\Assets\\DB\\" + filename;
                if (!File.Exists(resourceName))
                    File.Copy(PhotoPath, resourceName);
                PhotoPath = filename;
            }
            
            this.Close(new()
            {
                product = new Product
                {
                    ProductCategory = SelectedCategory,
                    ProductCost = Price,
                    ProductDescription = Description,
                    ProductPhoto = PhotoPath,
                    ProductManufacturer = SelectedManufacturer,
                    ProductQuantityInStock = QunatityInStock,
                    ProductDiscountAmount = (sbyte)Discount,
                    ProductProvider = SelectedProvider
                },
                name = Name
            });
        }
    }
}
