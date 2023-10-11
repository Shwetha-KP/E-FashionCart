using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tailoringapp.Context;
using tailoringapp.Entities;


namespace tailoringapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentController : ControllerBase
        {
        private readonly ILogger<CustomerController> _logger;
        private readonly DataContext _dbcontext;
        public AgentController(ILogger<CustomerController> logger, DataContext dbcontext)
            {
            _logger = logger;
            _dbcontext = dbcontext;

            }
        [HttpGet("agentlogin/{username}/{password}")]
        public IActionResult AgentLogin(string username,string password) {
            var agent = _dbcontext.Agent.FirstOrDefault(u => u.MobileNumber == username);
            if(agent == null)
                {
                return Ok(new {Status=-1 });
                }
            else if(agent.Password != password)
                {
                return Ok(new { Status = 0 });
                }
            else
                {
                var list = _dbcontext.Order.Join(_dbcontext.Customers,
                    order => order.CustomerId,
                    customer => customer.Id,
                    (order, customer) => new { order, customer }
                    )
                    .Where(u => (u.order.OrderStatus == 2 || u.order.OrderStatus == 5) && u.order.AgentId == agent.Id)
                    .Select(u => new { OrderId = u.order.Id, CustomerId = u.customer.Id,
                        CustomerName = u.customer.FirstName + " " + u.customer.LastName,
                        Status = u.order.OrderStatus
                        }).ToList();
                return Ok(new { Status = 1, data = agent,items= list }); ;
                }
            }
        [HttpGet("getorder/{orderid}/{agentid}")]
        public IActionResult GetOrder(int orderid, int agentid)
        {
            var agent = _dbcontext.Agent.Find(agentid);
            if (agent == null)
            {
                return Ok(new { Status = -1 });
            }
            else
            {
                try
                {
                    var order = _dbcontext.Order.Find(orderid);
                    if (order != null)
                    {
                        var address = _dbcontext.Addresses.Find(order.AddressId);
                        if (address != null)
                        {
                            var list = _dbcontext.Measurement.Where(u => u.OrderId == orderid).ToList();
                            var details = _dbcontext.DetailsOfOrder.Where(s => s.OrderId == orderid).ToList();
                            var mydetail = new List<OrderDetailModel>();
                            for (int j = 0; j < details.Count; j++)
                            {
                                var detail1 = new OrderDetailModel();
                                detail1.DetailId = details[j].Id;
                                if (details[j].PatternType == "Default")
                                {
                                    var pattrn = _dbcontext.Pattern.Find(details[j].PatternId);
                                    detail1.Photo = pattrn.Picture;
                                    detail1.Name = pattrn.PatternName;
                                    detail1.Price = pattrn.Price;
                                }
                                else
                                {
                                    var custpattrn = _dbcontext.CustomPattern.Find(details[j].PatternId);
                                    detail1.Photo = custpattrn.Photo;
                                    detail1.Name = "Custom";
                                    detail1.Price = custpattrn.Price;
                                }
                                detail1.DetailId = details[j].Id;
                                detail1.StitchingAmount = details[j].StitchingAmount;
                                detail1.Quantity = details[j].Quantity;

                                mydetail.Add(detail1);
                            }

                            return Ok(new { Status = 1, order = order, detail = mydetail, address = address, measure = list });
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                return Ok(new { Status = -1 }); ;
            }
        }
        public class OrderDetailModel
        {
            public int DetailId { get; set; }
            public string Photo { get; set; }
            public string Name { get; set; }
            public decimal? Price { get; set; }
            public decimal? StitchingAmount { get; set; }
            public int? Quantity { get; set; }
        }
        //[HttpGet("getorder/{orderid}/{agentid}")]
        //public IActionResult GetOrder(int orderid, int agentid)
        //    {
        //    var agent = _dbcontext.Agent.Find(agentid);
        //    if (agent == null)
        //        {
        //        return Ok(new { Status = -1 });
        //        }            
        //    else
        //        {
        //        try{
        //            var order = _dbcontext.Order.Find(orderid);
        //            if (order != null)
        //                {
        //                var address = _dbcontext.Addresses.Find(order.AddressId);
        //                if (address != null)
        //                    {
        //                    var list = _dbcontext.Measurement.Where(u => u.OrderId == orderid).ToList();
        //                    var details = _dbcontext.DetailsOfOrder.Join(_dbcontext.Pattern,
        //                        det => det.PatternId,
        //                        pat => pat.Id,
        //                        (det, pat) => new { det, pat }
        //                        )
        //                        .Where(u => u.det.OrderId == orderid).ToList()
        //                       .Select(u => new { 
        //                           DetailId=u.det.Id,
        //                           Photo=u.det.PatternType== "Default" ?u.pat.Picture:
        //                           _dbcontext.CustomPattern.Find(u.det.PatternId).Photo,
        //                           Name=u.det.PatternType=="Default"?u.pat.PatternName:"Custom",
        //                           Price= u.det.PatternType == "Default" ?u.pat.Price:
        //                           _dbcontext.CustomPattern.Find(u.det.PatternId).Price,
        //                           StitchingAmount=u.det.StitchingAmount,
        //                           Quantity=u.det.Quantity
        //                           })
        //                        .ToList();

        //                    return Ok(new { Status = 1, order = order,detail= details, address = address, measure = list });
        //                    }
        //                }
        //            }
        //        catch(Exception ex) 
        //            {

        //            }

        //        return Ok(new { Status = -1 }); ;
        //        }
        //    }
        [HttpPost("addmeasure")]
        public IActionResult AddMeasure([FromBody]Measurement item)
            {
            if(item.Id==0)_dbcontext.Measurement.Add(item);
            else _dbcontext.Entry(item).State=EntityState.Modified;
            _dbcontext.SaveChanges();
            return Ok(item);
            }
        [HttpPost("removemeasure")]
        public IActionResult RemoveMeasure([FromBody] Measurement item)
            {
            _dbcontext.Measurement.Remove(item);
            _dbcontext.SaveChanges();
            return Ok(item);
            }
        [HttpPost("PickedUp")]
        public IActionResult PickedUp([FromBody] Order order)
            {
            order.OrderStatus++;
            _dbcontext.Entry(order).State = EntityState.Modified;
            _dbcontext.SaveChanges();
            return Ok();
            }
        }
}

