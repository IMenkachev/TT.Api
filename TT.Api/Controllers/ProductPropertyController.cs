using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TT.Lib.DataAccess;
using TT.Lib.Entities;
using TT.Lib.Mvc;

namespace TT.Api.Controllers
{
    [Route("[controller]")]
    public class ProductPropertyController :  ReadWriteController<ProductProperty, int>
    {
        private readonly IProductPropertyService mainService;

        public ProductPropertyController(IProductPropertyService mainService) : base((IReadWriteService<ProductProperty>)mainService)
        {
            this.mainService = mainService;
        }
    }
}
