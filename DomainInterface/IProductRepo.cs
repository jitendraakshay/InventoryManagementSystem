using DomainEntities;
using System.Collections.Generic;

namespace DomainInterface
{
    public interface IProductRepo
    {
        int saveProducts(Products products);
        List<Products> getAllProducts();
        ReturnType saveAttribute(Attributes attribute);
        List<Attributes> getAllAttributes(int? productID);
        ReturnType saveAttributeValue(AttributeValue attributeValue);
        List<AttributeValue> getAllAttributeValues(int? productID, int? optionID);
        int saveSKUs(List<SKUs> sKUs);
        List<SKUs> getALLSkus(int? productID);
    }
}
