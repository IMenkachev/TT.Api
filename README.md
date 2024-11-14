# Documentation for Changes in TT.API

## Overview
This document describes the changes made to two APIs, `DataController` and `ExportController`, including new DTOs, a migration, and database services to improve data access, error handling, and product property export functionality.

---

## 1. DataController: TT.Api/Controllers

The `DataController` class manages data retrieval operations for fetching product and property data.

### Key Changes
- **Dependency Injection**: `IDatabaseService` and `ILogger<DataController>` are injected to handle data retrieval and logging.
- **Error Handling**: Each endpoint uses a `try-catch` block to handle `SqlException` errors. If a database error occurs, an error message is logged, and a `500 Internal Server Error` is returned.

### Methods
- **GetAllProducts** (`GET /data/products`)
  - Retrieves all products using `IDatabaseService`.
  - **Error Handling**: Logs errors and returns a `500 Internal Server Error` if a database issue occurs.
  
- **GetAllProperties** (`GET /data/properties`)
  - Retrieves all properties using `IDatabaseService`.
  - **Error Handling**: Similar to `GetAllProducts`, with error logging for troubleshooting.

---

## 2. ExportController: TT.Api/Controllers

The `ExportController` class exports product data, including brand information and a property hierarchy.

### Key Changes
- **Dependency Injection**: `TTDbContext`, `ProductPropertiesService`, and `ILogger<ExportController>` are injected.
- **Error Handling**: Error handling includes specific exception types, such as `DbUpdateException`, `InvalidOperationException`, and a general `Exception`, each with detailed logging and appropriate HTTP response codes.

### Methods
- **GetProductExport** (`GET /export/product`)
  - Fetches products with associated brands and builds a property hierarchy for each product using `ProductPropertiesService`.
  - **Error Handling**: Logs database update issues, invalid operations, and unexpected errors separately. Returns `500 Internal Server Error` for all exception cases, with specific error messages for clarity.

---

## 3. New DTOs: TT.Lib/Dtos

The following DTOs support data transfer between the service layer and the API endpoints.

- **BrandDto**: Represents brand data, with fields `Id` and `BrandName`.
- **ProductExportDto**: Represents exported product data with fields `Id`, `Name`, `Brand`, and a dictionary of `Properties`.
- **ProductDto**: Represents product structure used by the `DataController`, including `ProductId`, `ProductName`, `BrandName`, and various property-related fields.
- **ProductPropertyDto** and **PropertyDto**: Represent product properties and individual property details.

Each DTO is designed to decouple data presentation from core database entities.

---

## 4. Service Layer and Helper: TT.Lib/Services

### DatabaseService
The `DatabaseService` class implements `IDatabaseService` and provides methods for fetching products and properties from the database.
- **ExecuteReaderAsync**: A helper method, `ExecuteReaderAsync`, in `ServiceHelper` enables efficient data retrieval using `SqlDataReader`.

### ProductPropertiesService
The `ProductPropertiesService` class builds a hierarchical structure of product properties based on parent-child relationships.
- **GetProductPropertiesAsync**: Fetches product properties and builds a nested dictionary hierarchy starting from a root property.
- **BuildPropertyHierarchy**: A recursive helper method that constructs nested properties, with optimized handling for empty or null values.

---

## 5. Database Migration: AddTypeColumnToProperties - TT.Lib/Migrations

A migration script, `AddTypeColumnToProperties`, adds a new column `Type` to the `Properties` table to specify property types.

- **Up Migration**: Adds the `Type` column to `Properties`.
- **Down Migration**: Removes the `Type` column, providing a rollback mechanism.

---

## 6. Stored Procedure: GetAllProducts - TT.Lib/DataScripts

A new stored procedure, `GetAllProducts`, retrieves products and associated property data:

- **Joins**: Combines data from `Products`, `Brand`, `ProductProperties`, and `Properties` tables.
- **Columns**: Selects fields such as `ProductId`, `ProductName`, `BrandName`, `ProductPropertyId`, `PropertyName`, and `PropertyValue`.
- **Sorting**: Orders results by `ProductId` for consistent output.

- **Query Example**:
  ```sql
  USE tt
  GO
  
  CREATE PROCEDURE GetAllProducts
  AS
  BEGIN
    SELECT 
        p.Id AS ProductId,
        p.Name AS ProductName,
        b.Name AS BrandName,
        pp.Id AS ProductPropertyId,
        prop.Name AS PropertyName,
        pp.Value AS PropertyValue
    FROM 
        [tt].[dbo].[Products] AS p
    JOIN 
        [tt].[dbo].[Brand] AS b ON p.BrandId = b.Id
    LEFT JOIN 
        [tt].[dbo].[ProductProperties] AS pp ON p.Id = pp.ProductId
    LEFT JOIN 
        [tt].[dbo].[Properties] AS prop ON pp.PropertyId = prop.Id
    ORDER BY 
        p.Id
  
  END;


## 7. Database View: `vw_AllProperties` - TT.Lib/DataScripts

The `vw_AllProperties` database view provides a consolidated and structured view of all property-related information, which simplifies querying the `Properties` table and reduces the need for complex joins in application code.

- **Fields**:
  - `PropertyId`: The unique identifier for each property.
  - `PropertyName`: The name of the property.
  - `ParentId`: The ID of the parent property, enabling a hierarchical structure.
  - `PropertyType`: The type of the property (e.g., text, numeric), added to provide additional context about each property’s data type.
  
- **Usage**:
  - Utilized in `DatabaseService` to retrieve property data efficiently through the `GetAllPropertiesAsync` method, allowing for faster, more streamlined queries when fetching property details.

- **Query Example**:
  ```sql
  SELECT 
      Id AS PropertyId, 
      Name AS PropertyName, 
      ParentId, 
      Type AS PropertyType
  FROM   
      dbo.Properties;
