using Dapper;
using DomainEntities;
using DomainInterface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DomainRepository
{
    public class ProductRepo : IProductRepo
    {
        IOptions<ReadConfig> connectionString;
        public ProductRepo(IOptions<ReadConfig> _connectionString)
        {
            connectionString = _connectionString;
        }

        public List<Products> getAllProducts()
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    int operation = 2;
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@operation", operation);// 1 create, 2 read, 3 update, 4 delete        
                    param.Add("@result", null);
                    var returnType = SqlMapper.Query<Products>(
                                     connection, "[dbo].[usp_CrudProducts]", param, commandType: CommandType.StoredProcedure).ToList();
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int saveProducts(Products products)
        {
            try
            {
                int? operation = null;
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    if (products.ProductID == null)
                    {
                        operation = 1;
                    }
                    else
                    {
                        operation = 3;
                    }
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@productID", products.ProductID);
                    param.Add("@productName", products.ProductName);
                    param.Add("@description", products.ProductDescription);
                    param.Add("@entryBy", products.EntryBy);
                    param.Add("@isActive", products.IsProductActive);
                    param.Add("@operation", operation);// 1 create, 2 read, 3 update, 4 delete   
                    param.Add("@result", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 554321);
                    var result = SqlMapper.Query<int>(
                                    connection, "[dbo].[usp_CrudProducts]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                                       
                    return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReturnType saveAttribute(Attributes attributes)
        {
            try
            {
                int? operation = null;
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    if (attributes.OptionID == null)
                    {
                        operation = 1;
                    }
                    else
                    {
                        operation = 3;
                    }
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@productID", attributes.ProductID);
                    param.Add("@optionID", attributes.OptionID);
                    param.Add("@optionName", attributes.OptionName);
                    param.Add("@description", attributes.OptionDescription);                    
                    param.Add("@isActive", attributes.IsOptionActive);
                    param.Add("@operation", operation);// 1 create, 2 read, 3 update, 4 delete                    
                    var returnType = SqlMapper.Query<ReturnType>(
                                     connection, "[dbo].[usp_CrudOptions]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<Attributes> getAllAttributes(int? productID)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    int operation = 2;
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@productID", productID);
                    param.Add("@operation", operation);// 1 create, 2 read, 3 update, 4 delete                    
                    var returnType = SqlMapper.Query<Attributes>(
                                     connection, "[dbo].[usp_CrudOptions]", param, commandType: CommandType.StoredProcedure).ToList();
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReturnType saveAttributeValue(AttributeValue attributeValue)
        {
            try
            {
                int? operation = null;
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    if (attributeValue.ValueID == null)
                    {
                        operation = 1;
                    }
                    else
                    {
                        operation = 3;
                    }
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@productID", attributeValue.ProductID);
                    param.Add("@optionID", attributeValue.OptionID);
                    param.Add("@valueID", attributeValue.ValueID);
                    param.Add("@valueName", attributeValue.ValueName);
                    param.Add("@isActive", attributeValue.IsValueActive);                   
                    param.Add("@operation", operation);// 1 create, 2 read, 3 update, 4 delete                    
                    var returnType = SqlMapper.Query<ReturnType>(
                                     connection, "[dbo].[usp_CrudOptionsValue]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AttributeValue> getAllAttributeValues(int? productID, int? optionID)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    int operation = 2;
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@productID", productID);
                    param.Add("@optionID", optionID);
                    param.Add("@operation", operation);// 1 create, 2 read, 3 update, 4 delete                    
                    var returnType = SqlMapper.Query<AttributeValue>(
                                     connection, "[dbo].[usp_CrudOptionsValue]", param, commandType: CommandType.StoredProcedure).ToList();
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int saveSKUs(List<SKUs> skus)
        {
            try
            {
                int results = 0;           
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    string skuName = "";
                    foreach (SKUs sku in skus)
                    {
                        skuName+=sku.SKU+"-";
                    }                   
                   skuName=skuName.TrimEnd('-');

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@productID", skus[0].ProductID);
                    param.Add("@optionID", null);
                    param.Add("@valueID", null);
                    param.Add("@skuID", skus[0].SKUID);
                    param.Add("@sku", skuName);
                    param.Add("@IsSkuActive", null);
                    param.Add("@operation", 1);// 1 create, 2 read, 3 update, 4 delete  
                    param.Add("@result", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 554321);
                    var result = SqlMapper.Query<int>(
                                     connection, "[dbo].[usp_CrudSKUs]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                    int SKUID=Convert.ToInt32(result);

                    foreach (SKUs sku in skus)
                    {                         
                        param.Add("@productID", sku.ProductID);
                        param.Add("@optionID", sku.OptionID);
                        param.Add("@valueID", sku.ValueID);
                        param.Add("@skuID", SKUID);
                        param.Add("@sku", null);
                        param.Add("@IsSkuActive", null);
                        param.Add("@operation", 2);// 1 create, 2 read, 3 update, 4 delete     
                        param.Add("@result", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 554321);
                        var output = SqlMapper.Query<int>(
                                         connection, "[dbo].[usp_CrudSKUs]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                        results = Convert.ToInt32(output);
                    }
                   
                }
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SKUs> getALLSkus(int? productID)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))                {
                    
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@productID", productID);                    
                    param.Add("@operation", 3);// 1 create, 2 read, 3 update, 4 delete                    
                    var returnType = SqlMapper.Query<SKUs>(
                                     connection, "[dbo].[usp_CrudOptionsValue]", param, commandType: CommandType.StoredProcedure).ToList();
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
