using System;
using System.Collections.Generic;

namespace WriteDry;

public partial class Product
{
    public string ProductArticleNumber { get; set; }

    public int ProductName { get; set; }

    public string ProductDescription { get; set; }

    public int ProductCategory { get; set; }

    public string ProductPhoto { get; set; }

    public int ProductManufacturer { get; set; }

    public float ProductCost { get; set; }

    public int? ProductMaxDiscount { get; set; }

    public int ProductProvider { get; set; }

    public sbyte? ProductDiscountAmount { get; set; }

    public int ProductQuantityInStock { get; set; }

    public int? Unit { get; set; }

    public string ProductStatus { get; set; }

    public virtual Pcategory ProductCategoryNavigation { get; set; }

    public virtual Pmanufacturer ProductManufacturerNavigation { get; set; }

    public virtual Pname ProductNameNavigation { get; set; }

    public virtual Provider ProductProviderNavigation { get; set; }

    public virtual Unit UnitNavigation { get; set; }
}
