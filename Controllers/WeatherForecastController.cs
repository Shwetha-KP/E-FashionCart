using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using tailoringapp.Context;
using tailoringapp.Entities;

namespace tailoringapp.Controllers
    {
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
        {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly DataContext _dbcontext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext dbcontext)

            {
            _logger = logger;
            _dbcontext = dbcontext;
            //_dbcontext.Database.EnsureCreated();


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

        [HttpGet("Getcustomer")]
        public IActionResult Getcustomer()
            {
            var customers = _dbcontext.Customers.ToList();

            return Ok(customers);
            }

        [HttpGet("Addcustomer")]

        public IActionResult Addcustomer()
            {
            var newcustomer = new Customer
                {

                };
            _dbcontext.Customers.Add(newcustomer);
            _dbcontext.SaveChanges();

            return Ok(newcustomer);
            }

        [HttpGet("Login/{email}/{password}")]
        public IActionResult Login(string email, string password)
            {

            var user = _dbcontext.Customers.Where(row => row.Email == email && row.Password == password).FirstOrDefault();
            if (user == null) return Ok(null);
            user.Password = "";
            return Ok(user);
            }


        [HttpPost("RegisterCustomer")]
        public IActionResult RegisterCustomer([FromBody] Customer body)
            {
            _dbcontext.Customers.Add(body);
            _dbcontext.SaveChanges();
            return Ok(body);
            }

        [HttpGet("Validateuser/{email}/{password}")]
        public IActionResult ValidateUser(string email, string password)
            {
            if (_dbcontext.Customers.Where(u => u.Email == email && u.Password == password).Count() > 0)
                {
                var customer = _dbcontext.Customers.FirstOrDefault(u => u.Email == email && u.Password == password);
                return Ok(new { isvalid = true, data = customer });
                }
            else
                {
                return Ok(new { isvalid = false, data = new Customer() });
                }
            }
        [HttpGet("Adminlogin/{Email}/{Password}")]
        public IActionResult Adminlogin(string Email, string Password)
            {
            var owner = _dbcontext.Admin.Where(row => row.Email == Email && row.Password == Password).FirstOrDefault();
            if (owner != null)
                {
                owner.Password = "";
                }

            return Ok(owner);
            }
        [HttpPost("Product")]
        public IActionResult Product([FromBody] Product body)
            {
            if (body.Id == 0)
                {
                _dbcontext.Product.Add(body);
                }
            else
                {
                _dbcontext.Entry(body).State = EntityState.Modified;
                }
            _dbcontext.SaveChanges();
            return Ok(body);
            }

        [HttpGet("Product/{productName}/{picture}")]
        public IActionResult Product(string productName, string picture)
            {
            if (_dbcontext.Product.Where(u => u.ProductName == productName && u.Picture == picture).Count() > 0)
                {
                var Product = _dbcontext.Product.FirstOrDefault(u => u.ProductName == productName && u.Picture == picture);
                return Ok(new { isvalid = true, data = Product });
                }
            else
                {
                return Ok(new { isvalid = false, data = new Product() });
                }
            }
        [HttpPost("DeleteProduct")]
        public IActionResult DeleteProduct([FromBody] Product result)
            {
            _dbcontext.Product.Remove(result);
            _dbcontext.SaveChanges();
            return Ok();
            }
        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
            {
            var list = _dbcontext.Product.ToList();
            return Ok(list);
            }
        [HttpPost("Pattern")]
        public IActionResult Pattern([FromBody] Pattern body)
            {
            if (body.Id == 0)
                {
                _dbcontext.Pattern.Add(body);
                }
            else
                {
                _dbcontext.Entry(body).State = EntityState.Modified;
                }
            _dbcontext.SaveChanges();
            return Ok(body);
            }
        [HttpPost("DeletePattern")]
        public IActionResult DeletePattern([FromBody] Pattern result)
            {
            _dbcontext.Pattern.Remove(result);
            _dbcontext.SaveChanges();
            return Ok();
            }
        [HttpGet("GetPatterns")]
        public IActionResult GetPatterns()
            {
            var list = _dbcontext.Pattern.ToList();
            return Ok(list);
            }

        [HttpPost("Agent")]
        public IActionResult Agent([FromBody] Agent body)
            {
            if (body.Id == 0)
                {
                _dbcontext.Agent.Add(body);
                }
            else
                {
                _dbcontext.Entry(body).State = EntityState.Modified;
                }
            _dbcontext.SaveChanges();
            return Ok(body);
            }
        [HttpPost("DeleteAgent")]
        public IActionResult DeleteAgent([FromBody] Agent result)
            {
            _dbcontext.Agent.Remove(result);
            _dbcontext.SaveChanges();
            return Ok();
            }
        [HttpGet("GetAgents")]
        public IActionResult GetAgents()
            {
            var list = _dbcontext.Agent.ToList();
            return Ok(list);
            }
        [HttpGet("GetAvailableAgents")]
        public IActionResult GetAvailableAgents()
            {
            var list1 = _dbcontext.Order.Where(v => v.OrderStatus == 2 || v.OrderStatus == 5).Select(v => v.AgentId).ToList();
            var list = _dbcontext.Agent.Where(u => !list1.Contains(u.Id)).ToList();
            return Ok(list);
            }
        [HttpGet("pendingpickup")]
        public IActionResult PendingPickup()
            {
            var orderlist = _dbcontext.Order.Join(_dbcontext.Agent,
                order => order.AgentId,
                agent => agent.Id,
               (order, agent) => new { order, agent }
                ).Where(s => s.order.OrderStatus == 2).Select(s => new
                    {
                    s.order.Id,
                    s.order.CustomerId,
                    s.agent.DeliveryAgent,
                    s.agent.MobileNumber
                    })
                .ToList();
            return Ok(orderlist);
            }
        [HttpGet("pendingdeliver")]
        public IActionResult PendingDelivery()
            {
            var orderlist = _dbcontext.Order.Join(_dbcontext.Agent,
                order => order.AgentId,
                agent => agent.Id,
               (order, agent) => new { order, agent }
                ).Where(s => s.order.OrderStatus == 5).Select(s => new
                    {
                    s.order.Id,
                    s.order.CustomerId,
                    s.agent.DeliveryAgent,
                    s.agent.MobileNumber
                    })
                .ToList();
            return Ok(orderlist);
            }
        [HttpGet("workinprogressorder")]
        public IActionResult WorkInProgressOrder()
            {
            var orderlist = _dbcontext.Order.
                Where(u => u.OrderStatus == 3)
                .ToList();
            return Ok(orderlist);
            }
        [HttpPost("SaveWorkinDetailData")]
        public IActionResult SaveWorkinDetailData([FromBody] DetailsOfOrder work)
            {
            work.Status = true;
            _dbcontext.Entry(work).State = EntityState.Modified;
            _dbcontext.SaveChanges();
            if (_dbcontext.DetailsOfOrder.Count(u => u.OrderId == work.OrderId && (u.Status == null || u.Status == false)) == 0)
                {
                var order = _dbcontext.Order.Find(work.OrderId);
                order.OrderStatus++;
                _dbcontext.Entry(order).State = EntityState.Modified;
                _dbcontext.SaveChanges();
                }
            return Ok();
            }
        [HttpGet("workinprogressdetail/{orderid}")]
        public IActionResult WorkInProgressDetail(int orderid)
            {
            var list = new List<WorkInProgressModel>();
            var list1 = _dbcontext.DetailsOfOrder.Where(u => u.OrderId == orderid).ToList();
            if (list1 != null)
                {
                foreach(var row in list1)
                    {
                    var item = new WorkInProgressModel();
                    item.det = row;
                    if(row.PatternId >0)
                        {
                        if (row.PatternType == "Default")
                            {
                            item.pat = _dbcontext.Pattern.Find(row.PatternId);
                            }
                        else
                            {
                            item.cust = _dbcontext.CustomPattern.Find(row.PatternId);
                            }
                        }
                    list.Add(item);
                    }
                }
            
            return Ok(list);
            }
        public class WorkInProgressModel
            {
            public DetailsOfOrder det { get; set; }
            public Pattern pat { get; set; }
            public CustomPattern cust { get; set; }
            }
        [HttpGet("workinprogressmeasure/{orderid}/{detailid}")]
        public IActionResult WorkInProgressMeasure(int orderid, int detailid)
            {
            var list = _dbcontext.Measurement
                .Where(u => u.OrderId == orderid && u.DetailId == detailid)
             .ToList();
            return Ok(list);
            }

        [HttpGet("getorderDetails")]
        public IActionResult getorderDetails()
            {

            var orderlist = _dbcontext.Order.Where(s => s.OrderStatus == 1).ToList();
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
                var customer = _dbcontext.Customers.FirstOrDefault(c => c.Id == orderlist[i].CustomerId);
                item.CustomerName = customer.FirstName + " " + customer.LastName;
                item.AddressId = orderlist[i].AddressId;
                var Address = _dbcontext.Addresses.FirstOrDefault(a => a.Id == orderlist[i].AddressId);
                item.CustomerAddress = Address.FullName + " " + Address.Country + " " + Address.CustomerId + " " + Address.Address1 + " " + Address.Address2 + " " + Address.AddressType + " " + Address.City + " " + Address.Landmark + " " + Address.MobileNo + " " + Address.PinCode + " " + Address.State;
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
                    detail1.PatternType = details[j].PatternType;
                    if (detail1.PatternType == "Custom")
                        {
                        var custompattern = _dbcontext.CustomPattern.Find(detail1.PatternId);
                        if (custompattern != null)
                            {
                            detail1.PatternName = "Custom Pattern";
                            detail1.Picture = custompattern.Photo;
                            detail1.Price = custompattern.Price;
                            }
                        }
                    else
                        {
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
        [HttpGet("assigndelivery")]
        public IActionResult AssignDelivery()
            {

            var orderlist = _dbcontext.Order.Where(s => s.OrderStatus == 4).ToList();
            var cartlist = new List<OrderModel>();
            for (int i = 0; i < orderlist.Count; i++)
                {
                var item = new OrderModel();
                item.Id = orderlist[i].Id;
                item.OrderDate = orderlist[i].OrderDate;
                item.OrderStatus = orderlist[i].OrderStatus;
                item.CustomerId = orderlist[i].CustomerId;
                var customer = _dbcontext.Customers.FirstOrDefault(c => c.Id == orderlist[i].CustomerId);
                item.CustomerName = customer.FirstName + " " + customer.LastName;
                item.AddressId = orderlist[i].AddressId;
                var Address = _dbcontext.Addresses.FirstOrDefault(a => a.Id == orderlist[i].AddressId);
                item.CustomerAddress = Address.FullName + " " + Address.Country + " " + Address.CustomerId + " " + Address.Address1 + " " + Address.Address2 + " " + Address.AddressType + " " + Address.City + " " + Address.Landmark + " " + Address.MobileNo + " " + Address.PinCode + " " + Address.State;
                item.AgentId = null;
                //item.AgentId = orderlist[i].AgentId;
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
                    detail1.PatternType = details[j].PatternType;
                    if (detail1.PatternType == "Custom")
                        {
                        var custompattern = _dbcontext.CustomPattern.Find(detail1.PatternId);
                        if (custompattern != null)
                            {
                            detail1.PatternName = "Custom Pattern";
                            detail1.Picture = custompattern.Photo;
                            detail1.Price = custompattern.Price;
                            }
                        }
                    else
                        {
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
        [HttpPost("savependingorderstatus/{detailid}")]
        public IActionResult SavePendingOrderStatus([FromBody] OrderModel order, int detailid)
            {
            var orderentity = _dbcontext.Order.Find(order.Id);
            if (orderentity != null)
                {
                orderentity.AgentId = order.AgentId;
                orderentity.OrderStatus = 2;
                _dbcontext.Entry(orderentity).State = EntityState.Modified;
                var detail = order.OrderDetail.FirstOrDefault(u => u.Id == detailid);
                if (detail != null)
                    {
                    if (detail.PatternType == "Custom")
                        {
                        var custompattern = _dbcontext.CustomPattern.Find(detail.PatternId);
                        if (custompattern != null)
                            {
                            custompattern.Price = detail.Price;
                            _dbcontext.Entry(custompattern).State = EntityState.Modified;
                            }
                        }
                    }
                }
            _dbcontext.SaveChanges();
            return Ok();
            }
        [HttpPost("savependingdeliveryorderstatus/{detailid}")]
        public IActionResult SavePendingDeliveryOrderStatus([FromBody] OrderModel order, int detailid)
            {
            var orderentity = _dbcontext.Order.Find(order.Id);
            if (orderentity != null)
                {
                orderentity.AgentId = order.AgentId;
                orderentity.OrderStatus++;
                _dbcontext.Entry(orderentity).State = EntityState.Modified;
                }
            _dbcontext.SaveChanges();
            return Ok();
            }
        [HttpPost("updatepayment/")]
        public IActionResult UpdatePayment([FromBody] Payment payment)
            {
            var order = _dbcontext.Order.Find(payment.OrderId);
            if (order !=null)
                {
                payment.PaymentDate = DateTime.Now.Date.ToString("dd-MM-yyyy");
                _dbcontext.Payment.Add(payment);

                order.OrderStatus = 6;
                _dbcontext.Entry(order).State = EntityState.Modified;

                _dbcontext.SaveChanges();
                return Ok(new { status=1});
                }

            return Ok(new { status = 0 });
            }
        [HttpGet("getstitchingworks")]
        public IActionResult getstitchingworks()
            {

            var orderlist = _dbcontext.Order.Where(s => s.OrderStatus == 2).ToList();
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
                var customer = _dbcontext.Customers.FirstOrDefault(c => c.Id == orderlist[i].CustomerId);
                item.CustomerName = customer.FirstName + " " + customer.LastName;
                item.AddressId = orderlist[i].AddressId;
                var Address = _dbcontext.Addresses.FirstOrDefault(a => a.Id == orderlist[i].AddressId);
                item.CustomerAddress = Address.FullName + " " + Address.Country + " " + Address.CustomerId + " " + Address.Address1 + " " + Address.Address2 + " " + Address.AddressType + " " + Address.City + " " + Address.Landmark + " " + Address.MobileNo + " " + Address.PinCode + " " + Address.State;
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
                    detail1.PatternType = details[j].PatternType;
                    if (detail1.PatternType == "Custom")
                        {
                        var custompattern = _dbcontext.CustomPattern.Find(detail1.PatternId);
                        if (custompattern != null)
                            {
                            detail1.PatternName = "Custom Pattern";
                            detail1.Picture = custompattern.Photo;
                            detail1.Price = detail1.Price;
                            }
                        }
                    else
                        {
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
        [HttpGet("getallcompletedorders")]
        public IActionResult GetAllCompletedOrders() {
            return Ok(_dbcontext.Payment.ToList());
            }

        public class OrderModel
            {
            public int Id { get; set; }
            public string OrderDate { get; set; }
            public int CustomerId { get; set; }
            public string CustomerName { get; set; }
            public int OrderStatus { get; set; }
            public int? AgentId { get; set; }
            public string AgentType { get; set; }
            public int? AddressId { get; set; }
            public string CustomerAddress { get; set; }
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
            public int PatternId { get; set; }
            public int OrderId { get; set; }
            public string PatternType { get; set; }
            }
        public class ConfirmOrderModel
            {
            public int? OrderId { get; set; }
            public string AgentType { get; set; }


            }

        }
    }




