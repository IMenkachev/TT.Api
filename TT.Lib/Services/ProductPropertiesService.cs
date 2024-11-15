using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT.Api.Models;

namespace TT.Lib.Services
{
    public class ProductPropertiesService
    {
        private readonly TTDbContext _context;

        public ProductPropertiesService(TTDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, object>> GetProductPropertiesAsync(int productId)
        {
            // Load all properties and hierarchy in a single query, projected to PropertyDto
            var allProperties = await _context.Properties
                .Select(prop => new PropertyDto
                {
                    PropertyId = prop.Id,
                    PropertyName = prop.Name,
                    ParentId = prop.ParentId
                })
                .ToListAsync();

            // Load all product-specific property values in a single query, projected to PropertyDto
            var productProperties = await _context.ProductProperties
                .Where(pp => pp.ProductId == productId)
                .Select(pp => new
                {
                    pp.PropertyId,
                    pp.Value
                })
                .ToDictionaryAsync(pp => pp.PropertyId, pp => pp.Value);

            // Build property hierarchy
            var propertyHierarchy = BuildPropertyHierarchy(allProperties, productProperties, 0);
            return propertyHierarchy;
        }

        private Dictionary<string, object> BuildPropertyHierarchy(
            List<PropertyDto> allProperties,
            Dictionary<int, string> productPropertyValues,
            int parentId)
        {
            var propertyDict = new Dictionary<string, object>();

            // Get direct child properties of the parent
            var childProperties = allProperties.Where(p => p.ParentId == parentId).ToList();

            foreach (var property in childProperties)
            {
                // Retrieve value if it exists for this product and property combination
                productPropertyValues.TryGetValue(property.PropertyId, out var propertyValue);

                // Check if the property has sub-properties
                bool hasSubProperties = allProperties.Any(p => p.ParentId == property.PropertyId);

                if (hasSubProperties)
                {
                    // Recursively retrieve and assign sub-properties
                    var subProperties = BuildPropertyHierarchy(allProperties, productPropertyValues, property.PropertyId);

                    // Include this property only if it has non-null sub-properties or a non-null direct value
                    if (subProperties.Count > 0 || propertyValue != null)
                    {
                        if (subProperties.Count > 0)
                        {
                            propertyDict[property.PropertyName.ToLower()] = subProperties;
                        }
                        else
                        {
                            propertyDict[property.PropertyName.ToLower()] = propertyValue;
                        }
                    }
                }
                else
                {
                    // Directly assign properties with values, excluding nulls
                    if (propertyValue != null)
                    {
                        propertyDict[property.PropertyName.ToLower()] = propertyValue;
                    }
                }
            }

            return propertyDict;
        }
    }
}
