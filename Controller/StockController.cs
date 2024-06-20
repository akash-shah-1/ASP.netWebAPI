using api.Data; // This includes the namespace api.Data so that classes and methods within that namespace can be used in this file.
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc; // This includes ASP.NET Core MVC components, enabling the use of controllers, actions, and routing.

namespace api.Controller
{
    [Route("api/stock")]
    [ApiController] // This attribute indicates that this class is an API controller and provides automatic model state validation, among other things.
    public class StockController : ControllerBase //StockController that inherits from ControllerBase, which provides basic functionality for handling HTTP requests.
    {
        private readonly ApplicationDBContext dbContext; // Declares a read-only field to hold the database context.

        // This is a constructor that takes an ApplicationDBContext parameter. The provided context is assigned to the dbContext field.
        // This is an example of dependency injection, where ApplicationDBContext is injected into the controller by the dependency injection framework.
        public StockController(ApplicationDBContext context)
        {
            dbContext = context;
        }

        //Get all stocks
        [HttpGet]
        public IActionResult GetAll()
        {
            var stocks = dbContext.Stock.ToList().Select(s=>s.ToStockDto()); //This retrieves all stock records from the database as a list.
            return Ok(stocks);
        }

        //Get stock by Id
        [HttpGet("{id}")] //This attribute specifies that this action responds to HTTP GET requests with a URL parameter (id).
        public IActionResult GetById([FromRoute] int id)
        { //This method takes an id parameter from the route and returns an IActionResult. [FromRoute]int id: This is model binding, where the value of id in the URL is automatically bound to the id parameter of the method.
            var stock = dbContext.Stock.Find(id);
            if(stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        //IActionResult, and takes a CreateStockRequestDto object as a parameter.The [FromBody]
        //attribute indicates that the stockDto should be deserialized from the JSON body of the HTTP request. 
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockfromCreateDto(); //This line converts the CreateStockRequestDto (stockDto) into a Stock entity (stockModel) using the ToStockfromCreateDto mapping method.
            dbContext.Stock.Add(stockModel);
            dbContext.SaveChanges();
            //
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto()); 
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateStockDto)
        {
            var stockModel =  dbContext.Stock.FirstOrDefault(s=> s.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            stockModel.Symbol = UpdateStockDto.Symbol;
            stockModel.CompanyName = UpdateStockDto.CompanyName;
            stockModel.Purchase = UpdateStockDto.Purchase;
            stockModel.LastDiv = UpdateStockDto.LastDiv;
            stockModel.Industry = UpdateStockDto.Industry;
            stockModel.MarketCap = UpdateStockDto.MarketCap;

            dbContext.SaveChanges();

            return Ok(new 
            { 
                message = $"Stock model with Id = {stockModel.Id} is Updated successfully ", 
                stock = stockModel.ToStockDto()
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id) {
         
            var stockModel = dbContext.Stock.FirstOrDefault(d=>d.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            dbContext.Stock.Remove(stockModel);
            dbContext.SaveChanges();
            return Ok(new{
            message = "Stock Deleted Successfully "
            });
        }
    }
}
