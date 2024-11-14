using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TT.Lib.DataAccess;
using TT.Lib.Entities;
using TT.Lib.Mvc;

namespace TT.Api.Controllers
{
    [Route("[controller]")]
    public class BrandController :  ReadWriteController<Brand, int>
    {
        private readonly IBrandService mainService;

        public BrandController(IBrandService mainService) : base((IReadWriteService<Brand>)mainService)
        {
            this.mainService = mainService;
        }

        [HttpDelete]
        public new async Task<IActionResult> Delete(Brand brand) => Ok(await base.Delete(brand));
    }
}
