using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountReconciliationDetailsController : Controller
    {

        private readonly IAccountReconciliationDetailService _accountReconciliationDetailService;

        public AccountReconciliationDetailsController(IAccountReconciliationDetailService accountReconciliationDetailService)
        {
            _accountReconciliationDetailService = accountReconciliationDetailService;
        }

        [HttpPost("addFromExcel")]
        public IActionResult AddFromExcel(IFormFile file, int accountReconciliationId)
        {
            if (file.Length > 0)
            {
                //var fileName = Guid.NewGuid().ToString() + ".xlsx";
                //var filePath = $"{Directory.GetCurrentDirectory()}/Content/{fileName}";
                //using (FileStream stream = System.IO.File.Create(filePath))
                //{
                //    file.CopyTo(stream);
                //    stream.Flush();
                //}
                var fileName = Guid.NewGuid().ToString() + ".xlsx";
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Content");
                var filePath = Path.Combine(directoryPath, fileName);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (FileStream stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                    stream.Flush();
                }

                var result = _accountReconciliationDetailService.AddToExcel(filePath, accountReconciliationId);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result.Message);
            }
            return BadRequest("Dosya Seçimi Yapmadınız");
        }



        [HttpPost("add")]
        public IActionResult Add(AccountReconciliationDetail accountReconciliationDetai)
        {
            var result = _accountReconciliationDetailService.Add(accountReconciliationDetai);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("delete")]
        public IActionResult Delete(AccountReconciliationDetail accountReconciliationDetai)
        {
            var result = _accountReconciliationDetailService.Delete(accountReconciliationDetai);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("update")]
        public IActionResult Update(AccountReconciliationDetail accountReconciliationDetai)
        {
            var result = _accountReconciliationDetailService.Update(accountReconciliationDetai);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var result = _accountReconciliationDetailService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getList")]
        public IActionResult GetList(int accountReconciliationId)
        {
            var result = _accountReconciliationDetailService.GetList(accountReconciliationId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
