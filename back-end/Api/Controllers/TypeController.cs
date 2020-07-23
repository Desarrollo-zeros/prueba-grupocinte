using Domain.Entities;
using Domain.Interface;
using Domain.Interface.Domain.Interface;
using Infrastructure.Interface;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
  
        private readonly IDbContext _dbContext;
        private readonly IRepository<TypeIdentity> _repository;
        private readonly IUnitOfWork _unitOfWork;


        public TypeController( IDbContext dbContext, IUnitOfWork unitOfWork)
        {
          
            _dbContext = dbContext;
            _repository = new GeneryRepository<TypeIdentity>(_dbContext);
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public ActionResult<object> GetAll()
        {
            return Ok(new { status = true, Message = "OK", Entities = _repository.GetAll() });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public ActionResult<object> Get(int id)
        {
            return Ok(new { status = true, Message = "OK", Entities = _repository.Find(id) });
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [HttpPost]
        public ActionResult<object> Add(TypeIdentity typeIdentity)
        {
            typeIdentity = _repository.Add(typeIdentity);
            if (_unitOfWork.Commit() > 0)
            {
                return Ok(new { status = true, Message = "OK", Entities = typeIdentity });
            }
            return BadRequest(new { status = false, Message = "false", Entities = typeIdentity });
        }
    }
}
