using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TT.Lib.DataAccess;
using TT.Lib.Entities;
using TT.Lib.Mvc;

namespace TT.Api.Controllers
{
    [Route("[controller]")]
    public class PropertyController :  ReadWriteController<Property, int>
    {
        private readonly IPropertyService mainService;

        public PropertyController(IPropertyService mainService) : base((IReadWriteService<Property>)mainService)
        {
            this.mainService = mainService;
        }
    }
}
