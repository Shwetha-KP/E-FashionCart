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
    public class CustomerController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<CustomerController> _logger;
        private readonly DataContext _dbcontext;

        public CustomerController(ILogger<CustomerController> logger, DataContext dbcontext)
        {
            _logger = logger;
            _dbcontext = dbcontext;

        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("getcustomer")]
        public IActionResult getcustomer()
        {
            var customers = _dbcontext.Customers.ToList();
            var test = _dbcontext.Customers
                 .Select(row => new { row.FirstName, row.Email })

                 .ToList();



            return Ok(test);
        }

        [HttpGet("addcustomer")]
        public IActionResult addcustomer()
        {
            var newcustomer = new Customer
            {
                FirstName = "sumanth",
                Email = "test@gmail.com",
                Password = "12345"
            };
            _dbcontext.Customers.Add(newcustomer);
            _dbcontext.SaveChanges();

            return Ok(newcustomer);
        }
        [HttpGet("getuser/{email}")]
        public IActionResult getuser(string email)
        {

            var user = _dbcontext.Customers.Where(row => row.Email == email).FirstOrDefault();

            if (user != null) user.Password = "";

            return Ok(user);
        }
        [HttpPost("RegisterCustomer")]
        public IActionResult RegisterCustomer([FromBody] Customer body) {
            try
                {
                if (_dbcontext.Customers.Count(u => u.Email == body.Email) > 0)
                    {
                    return Ok(new { Status = -1 });
                    }
                else
                    {
                    _dbcontext.Customers.Add(body);
                    _dbcontext.SaveChanges();
                    return Ok(new { Status = 1,Data=body });
                    }

                }
            catch (Exception ex) {
                return Ok(new { Status = 0 });
                }
            
        }
        [HttpGet("validateuser/{email}/{password}")]
        public IActionResult ValidateUser(string email, string password)
        {
            if (_dbcontext.Customers.Count(u => u.Email == email) > 0)
                {
                var customer = _dbcontext.Customers.SingleOrDefault(u => u.Email == email && u.Password == password);
                if (customer == null)
                    {
                    return Ok(new { Status = 0});
                    }
                else {
                    return Ok(new { Status = 1, data = customer });
                    }
                }
            else {
                return Ok(new { Status=-1 });
                }
           
        }
        [HttpGet("getProducts")]
        public IActionResult getProducts()
        {
            var list = _dbcontext.Product.ToList();
            return Ok(list);
        }
        [HttpGet("getPatterns/{productid}")]
        public IActionResult getPatterns(int productid)
        {

            var list = _dbcontext.Pattern.Where(s => s.ProductId == productid).ToList();
            return Ok(list);
        }
        [HttpPost("OwnPicture")]
        public IActionResult OwnPicture([FromBody] OrderDetails data)
        {
            _dbcontext.OrderDetails.Add(data);
            _dbcontext.SaveChanges();
            return Ok(data);
        }
        [HttpGet("getAddresslist/{customerId}")]
        public IActionResult getAddresslist(int customerId)
        {

            var list = _dbcontext.Addresses.Where(s => s.CustomerId == customerId).ToList();
            return Ok(list);
        }
        [HttpGet("getCartItems/{CustomerId}")]
        public IActionResult getCartItems(int CustomerId)
        {
            
            var orderlist = _dbcontext.Order.Where(s=>s.OrderStatus==0&& s.CustomerId==CustomerId).ToList();
            var cartlist = new List<OrderModel>();
            string[] abc = new string[3];
            int length=abc.Length;
            for (int i = 0; i < orderlist.Count; i++)
            {
                var item = new OrderModel();

                item.Id = orderlist[i].Id;
                item.OrderDate = orderlist[i].OrderDate;
                item.OrderStatus = orderlist[i].OrderStatus;
                item.CustomerId = orderlist[i].CustomerId;
                item.AgentId = orderlist[i].AgentId;
                item.AgentType = orderlist[i].AgentType;
                var details= _dbcontext.DetailsOfOrder.Where(s => s.OrderId == orderlist[i].Id).ToList();
                item.OrderDetail = new List<OrderDetailModel>();
                for(int j = 0; j < details.Count; j++)
                {
                    var detail1 = new OrderDetailModel();
                    detail1.Id = details[j].Id;
                    detail1.OrderId = details[j].OrderId;
                    detail1.PatternId = details[j].PatternId;
                    detail1.Quantity = details[j].Quantity;
                    detail1.StitchingType = details[j].StitchingType;
                    detail1.PatternType = details[j].PatternType;
                    if (detail1.PatternType == "Custom")
                        {
                        var custompattern = _dbcontext.CustomPattern.Find(detail1.PatternId);
                        if (custompattern != null) {
                            detail1.PatternName = "Custom Pattern";
                            detail1.Picture = custompattern.Photo;
                            detail1.Price = custompattern.Price;
                            }
                        }
                    else {

                        var pattern = _dbcontext.Pattern.Where(s => s.Id == detail1.PatternId).FirstOrDefault();
                        if (pattern != null)
                            {
                            detail1.PatternName = pattern.PatternName;
                            detail1.Picture = pattern.Picture;
                            detail1.Picture1 = pattern.Picture1;
                            detail1.Picture2 = pattern.Picture2;
                            detail1.Picture3 = pattern.Picture3;
                            detail1.Picture4 = pattern.Picture4;
                            detail1.Price = pattern.Price;
                            }
                        }
                    
                    item.OrderDetail.Add(detail1);
                }
                cartlist.Add(item);             
            }        
            return Ok(cartlist);           
        }
        public class OrderModel
        {
            public int Id { get; set; }
            public string OrderDate { get; set; }
            public int CustomerId { get; set; }
            public int OrderStatus { get; set; }
            public int? AgentId { get; set; }
            public string AgentType { get; set; }
            public List<OrderDetailModel> OrderDetail { get; set; }

        }
        public class OrderDetailModel
        {
            public int Id { get; set; }
            public string PatternName { get; set; }
            public int Quantity { get; set; }
            public decimal? Price { get; set; }
            public string Picture { get; set; }
            public string Picture1 { get; set; }
            public string Picture2 { get; set; }
            public string Picture3 { get; set; }
            public string Picture4 { get; set; }
            public string StitchingType { get; set; }
            public string PatternType { get; set; }

            public int PatternId{ get; set; }
            public int OrderId { get; set; }

        }
        public class ConfirmOrderModel
        {
            public int? OrderId { get; set; }
            public string AgentType { get; set; }
            public int AddressId { get; set; }
            public string Country { get; set; }
            public string FullName { get; set; }
            public string State { get; set; }
            public string MobileNo { get; set; }
            public string PinCode { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string Landmark { get; set; }
            public string City { get; set; }
        }
        [HttpGet("getCountList")]

        public IActionResult getCountList()
        {
            //var count = _dbcontext.Order.Where(c => c.OrderStatus == 0).Count();
            var count = _dbcontext.Order.Join(_dbcontext.DetailsOfOrder, order => order.Id, details => details.OrderId,
                (order, details) => new { order = order, details = details }).Where(c => c.order.OrderStatus == 0)
                .Select(c => new { c.order.OrderStatus }).Count();
                 return Ok(new{count = count});
                
                }
            [HttpPost("addCart")]
        
        public IActionResult addCart([FromBody] CartDetails body)
        {            
            if (body.order.Id == 0)
            {
                body.order.OrderDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
                _dbcontext.Order.Add(body.order);
                _dbcontext.SaveChanges();
                for (int i = 0; i < body.details.Count; i++)
                {
                    body.details[i].OrderId = body.order.Id;
                    body.details[i].PatternType = "Default";
                    if (body.details[i].Id == 0)
                    {
                        _dbcontext.DetailsOfOrder.Add(body.details[i]);
                    }
                    else
                    {
                        _dbcontext.Entry(body.details[i]).State = EntityState.Modified;
                    }
                }
                _dbcontext.SaveChanges();
            }
            else
            {
                _dbcontext.Entry(body.order).State = EntityState.Modified;
                for (int i = 0; i < body.details.Count; i++)
                {
                    
                    if (body.details[i].Id == 0)
                    {
                        _dbcontext.DetailsOfOrder.Add(body.details[i]);
                    }
                    else
                    {
                        _dbcontext.Entry(body.details[i]).State = EntityState.Modified;
                    }
                }
                _dbcontext.SaveChanges();
            }
            _dbcontext.SaveChanges();
            return Ok(body);
        }

        [HttpPost("addOrder")]

        public IActionResult addOrder([FromBody] OrderCart body)
        {
            if (body.order.Id == 0)
                {
                body.order.OrderDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
                _dbcontext.Order.Add(body.order);
                }
            else {
                _dbcontext.Entry(body.order).State = EntityState.Modified;
                }
            _dbcontext.SaveChanges();
            if (body.PatternType == "Custom")
                {
                _dbcontext.CustomPattern.Add(body.CustomPattern);
                _dbcontext.SaveChanges();
                DetailsOfOrder newdetail = new DetailsOfOrder();
                newdetail.OrderId = body.order.Id;
                newdetail.PatternId = body.CustomPattern.Id;
                newdetail.PatternType = body.PatternType;
                newdetail.Quantity = body.CustomPattern.Quantity;
                newdetail.StitchingType = body.CustomPattern.StitchingType;

                _dbcontext.DetailsOfOrder.Add(newdetail);
                _dbcontext.SaveChanges();
                }
            else {
                for (int i = 0; i < body.details.Count; i++)
                    {
                    body.details[i].PatternType = body.PatternType;
                    body.details[i].OrderId = body.order.Id;
                    if (body.details[i].Id == 0)
                        {
                        _dbcontext.DetailsOfOrder.Add(body.details[i]);
                        }
                    else
                        {
                        _dbcontext.Entry(body.details[i]).State = EntityState.Modified;
                        }
                    }
                _dbcontext.SaveChanges();
            }
            return Ok(body);
        }
        [HttpGet("getOrderItems/{CustomerId}")]
        public IActionResult getOrderItems(int CustomerId)
        {

            var orderlist = _dbcontext.Order.Where(s => s.OrderStatus == 0 && s.CustomerId == CustomerId).ToList();
            var cartlist = new List<OrderModel>();
            string[] abc = new string[3];
            int length = abc.Length;
            for (int i = 0; i < orderlist.Count; i++)
            {
                var item = new OrderModel();

                item.Id = orderlist[i].Id;
                item.OrderDate = orderlist[i].OrderDate;
                item.OrderStatus = orderlist[i].OrderStatus;
                item.CustomerId = orderlist[i].CustomerId;
                item.AgentId = orderlist[i].AgentId;
                item.AgentType = orderlist[i].AgentType;
                var details = _dbcontext.DetailsOfOrder.Where(s => s.OrderId == orderlist[i].Id).ToList();
                item.OrderDetail = new List<OrderDetailModel>();
                for (int j = 0; j < details.Count; j++)
                {
                    var detail1 = new OrderDetailModel();
                    detail1.Id = details[j].Id;
                    detail1.OrderId = details[j].OrderId;
                    detail1.PatternId = details[j].PatternId;
                    detail1.Quantity = details[j].Quantity;
                    detail1.StitchingType = details[j].StitchingType;
                    var pattern = _dbcontext.Pattern.Where(s => s.Id == detail1.PatternId).FirstOrDefault();
                    if (pattern != null)
                    {
                        detail1.PatternName = pattern.PatternName;
                        detail1.Picture = pattern.Picture;
                        detail1.Picture1 = pattern.Picture1;
                        detail1.Picture2 = pattern.Picture2;
                        detail1.Picture3 = pattern.Picture3;
                        detail1.Picture4 = pattern.Picture4;
                        detail1.Price = pattern.Price;

                    }
                    item.OrderDetail.Add(detail1);


                }
                cartlist.Add(item);


            }

            return Ok(cartlist);


        }
        [HttpPost("deleteOrder")]
        public IActionResult deleteOrder([FromBody] Order body)
        {
            _dbcontext.Database.ExecuteSqlRaw
                ("delete from DetailsOfOrder where OrderId={0}", body.Id);
            _dbcontext.Order.Remove(body);
            _dbcontext.SaveChanges();
            return Ok();
        }
        [HttpPost("deleteItem")]
        public IActionResult deleteItem([FromBody] DetailsOfOrder body)
        {
            _dbcontext.DetailsOfOrder.Remove(body);
            _dbcontext.SaveChanges();
            return Ok();
        }
        [HttpGet("getOrder/{orderId}")]
        public IActionResult getOrder(int orderId)
        {
            var order = _dbcontext.Order.FirstOrDefault(s=>s.Id==orderId);
            return Ok(order);
        }
        [HttpPost("confirmOrder")]
        public IActionResult confirmOrder([FromBody] ConfirmOrderModel body)
        {
            var order = _dbcontext.Order.FirstOrDefault(u => u.Id == body.OrderId);
            if (order != null)
            {
                order.AgentType = body.AgentType;
                order.OrderStatus = 1;

                order.AddressId = body.AddressId;
                _dbcontext.SaveChanges();

            }
            return Ok();
        }

        [HttpPost("postAddress")]
        public IActionResult postAddress([FromBody] Address result)
        {
                _dbcontext.Addresses.Add(result);
                _dbcontext.SaveChanges();
                return Ok(_dbcontext.Addresses.Where(s => s.CustomerId == result.CustomerId).ToList());           
        }
        [HttpGet("getTracking/{Customerid}")]
        public IActionResult getTracking(int customerId)
        {
            var list = _dbcontext.Order.Where(s => s.CustomerId == customerId).ToList();
            return Ok(list);
        }
        public class CartDetails
        {
            public Order order { get; set; }
            public List<DetailsOfOrder> details { get; set; } 
        }
        public class OrderCart
        {
            public string PatternType { get; set; }
            public Order order { get; set; }
            public CustomPattern CustomPattern { get; set; }
            public List<DetailsOfOrder> details { get; set; }
        }
        [HttpGet("trackOrder/{CustomerId}")]
        public IActionResult trackOrder(int customerId)
        {
            var orderList = _dbcontext.Order.Where(s => s.CustomerId == customerId && s.OrderStatus < 5).ToList();
            var trackList = new List<trackerModel>();
            for (int i = 0; i < orderList.Count; i++)
            {
                var item = new trackerModel();
                item.OrderId = orderList[i].Id;
                item.OrderStatus = orderList[i].OrderStatus;
                item.CustomerId = orderList[i].CustomerId;

                if (orderList[i].AgentId == null)
                {
                    var agent = _dbcontext.Agent.FirstOrDefault(c => c.Id == orderList[i].AgentId);
                    item.PickupAgent = agent.DeliveryAgent;
                    item.PickupAgentPhoto = agent.Photo;
                }

                var detailsoforder = _dbcontext.DetailsOfOrder.FirstOrDefault(a => a.OrderId == orderList[i].Id);

                var pattern = _dbcontext.Pattern.FirstOrDefault(q => q.Id == item.PatternId);
                item.PatternName = pattern.PatternName;
                item.PatternPhoto = pattern.Picture;
                item.PatternPhoto = pattern.Picture1;
                item.PatternPhoto = pattern.Picture2;
                item.PatternPhoto = pattern.Picture3;
                item.PatternPhoto = pattern.Picture4;
            }
            return Ok(orderList);
        }
        [HttpGet("getcustomercompletedorders/{customerid}")]
        public IActionResult GetCustomerCompletedOrders(int customerid)
            {
            return Ok(_dbcontext.Payment.Where(u=>u.CustomerId== customerid).ToList());
            }
        [HttpGet("getcustomercurrentorders/{customerid}")]
        public IActionResult GetCustomerCurrentOrders(int customerid)
            {
            try
                {
                var responselist = new List<CurrentOrderModel>();
                var templist = _dbcontext.Order.Where(u => u.CustomerId == customerid  && u.OrderStatus != 6 && u.OrderStatus != 0).ToList();
                if(templist!=null)
                    {
                    foreach(var row in templist)
                        {
                        var newitem = new CurrentOrderModel();
                        newitem.order = row;
                        newitem.Total = 0;
                        var detail = _dbcontext.DetailsOfOrder.Where(u => u.OrderId == row.Id).ToList();
                        foreach(var dett in detail)
                            {
                            if (dett.StitchingAmount != null)
                                {
                                newitem.Total += dett.StitchingAmount;
                                }
                            if (dett.PatternId >0)
                                {
                                if (dett.PatternType == "Default")
                                    {
                                    var patt = _dbcontext.Pattern.Find(dett.PatternId);
                                    newitem.Total += (patt.Price * dett.Quantity);
                                    
                                    }
                                else
                                    {
                                        {
                                        var cust = _dbcontext.CustomPattern.Find(dett.PatternId);
                                        newitem.Total += (cust.Price * dett.Quantity);
                                        }
                                    }
                                }
                            }
                        if (row.AgentId!=null && row.AgentId > 0)
                            {
                            var agent = _dbcontext.Agent.Find(row.AgentId);
                            newitem.agent = agent;
                            }
                        responselist.Add(newitem);
                        }
                    }
                var list=responselist
                    .Select(u => new {
                        OrderId = u.order.Id,
                        OrderDate = u.order.OrderDate,
                        OrderStatus = u.order.OrderStatus,
                        AgentName = u.agent == null ? "" : u.agent.DeliveryAgent,
                        Mobile = u.agent == null ? "" : u.agent.MobileNumber,
                        Photo = u.agent == null ? "" : u.agent.Photo,
                        Total= u.Total
                        }).ToList();

                return Ok(list);
                }
            catch(Exception ex)
                {
                return Ok();
                }
            
            }
        public class CurrentOrderModel
            {
            public Order order { get; set; }
            public Agent agent { get; set; }
            public decimal? Total { get; set; }
            }
        public class trackerModel
        {
           public int? OrderId { get; set; }
            public int? PatternId { get; set; }

            public int? OrderStatus { get; set; }
           public int? CustomerId { get; set; }
            public string PickupAgent { get; set; }
            public string DeliveryAgent { get; set; }
            public DateTime? OrderDate { get; set; }
            public string PatternName { get; set; }
            public string PatternPhoto { get; set; }
            public string PatternPhoto1 { get; set; }
            public string PatternPhoto2 { get; set; }
            public string PatternPhoto3 { get; set; }
            public string PatternPhoto4 { get; set; }
            public string PickupAgentPhoto { get; set; }

        }
    }
}

