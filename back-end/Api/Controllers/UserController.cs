using Api.Interface;
using Domain.Entities;
using Domain.Interface;
using Domain.Interface.Domain.Interface;
using Infrastructure.Base;
using Infrastructure.Interface;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
       
        private readonly IDbContext _dbContext;
        private readonly IRepository<User> _repository;
        private readonly IRepository<TypeIdentity> _repository1;
        private readonly IUnitOfWork _unitOfWork;


        public UserController(IDbContext dbContext, IUnitOfWork unitOfWork)
        {
           
            _dbContext = dbContext;
            _repository = new GeneryRepository<User>(_dbContext);
            _repository1 = new GeneryRepository<TypeIdentity>(_dbContext);
            _unitOfWork = unitOfWork;
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        
        public ActionResult<object> Add(User entity)
        {
            try
            {



                if (!RegexUtilities.IsValidEmail(entity.Email))
                {
                    return BadRequest(new { Status = false, Message = "correo invalido" });
                }

                var isNumeric = int.TryParse(entity.IdentityNumber, out _);

                if (!isNumeric)
                {
                    return BadRequest(new { Status = false, Message = "Número de identificación invalido" });
                }

                if (_repository.Any(x => x.IdentityNumber == entity.IdentityNumber))
                {
                    return BadRequest(new { Status = false, Message = "Número de identificación ya existe" });
                }

                if (_repository.Any(x => x.Email == entity.Email))
                {
                    return BadRequest(new { Status = false, Message = "Correo electronico ya existe" });
                }

                entity.Password = new PasswordHasher().HashPassword(entity.Password);
                _repository.Add(entity);
                if (_unitOfWork.Commit() > 0)
                {
                    return Ok(new { status = true, Message = "Usuario creado con exito" });
                }
                return BadRequest(new { Status = false, Message = "Usuario no pudo crearse" });
            }catch(Exception e)
            {
                return BadRequest(new { Status = false, e.Message });
            }

        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public ActionResult<object> Get(int id)
        {
            try
            {

                var entities = _repository.Find(id);
                if(entities == null)
                {
                    return NotFound(new { status = false, Message = "No hay datos para mostrar" });
                }
                
                return Ok(new { status = true, Message = "OK", Entities = entities });
            }
            catch(Exception e)
            {
                return BadRequest(new { Status = false, e.Message });
            }

            

           
        }



        [HttpGet("{pageIndex}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public ActionResult<object> GetAll(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var users = _repository.GetAll();

                if(users == null)
                {
                    return NotFound(new { status = false, Message = "No hay datos para mostrar" });
                }

                users.ToList().ForEach(x =>
                {
                    x.TypeIdentity = _repository1.Find(x.TypeId);
                });

                var entities = new PagedList<User>(users.ToList(), pageIndex, pageSize);

                return Ok(new { status = true, Message = "OK", Entities = entities, entities.TotalCount, entities.TotalPages });

            }catch(Exception e)
            {
                return BadRequest(new { Status = false, e.Message });
            }
        }


        [HttpPost("auth")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
       
        public ActionResult<User> Authenticate([FromBody] Api.Models.Login entity)
        {
            try
            {
                var isNumeric = int.TryParse(entity.IdentityNumber, out _);

                if (!isNumeric)
                {
                    return BadRequest(new { Status = false, Message = "Número de indentificación no valido" });
                }
                var userEntity = _repository.FindBy(x => x.IdentityNumber == entity.IdentityNumber && x.TypeId == entity.Type).FirstOrDefault();


                if (userEntity == null)
                {
                    return NotFound(new { Status = false, Message = "Usuario no existe" });
                }

                var verify = new PasswordHasher().VerifyHashedPassword(userEntity.Password, entity.Password);

                if (verify == PasswordVerificationResult.Success)
                {
                    entity.User = new Models.UserModel
                    {
                        IdentityNumber = userEntity.IdentityNumber,
                        Email = userEntity.Email,
                        Id = userEntity.Id,
                        LastName = userEntity.LastName,
                        Name = userEntity.Name,
                        TypeId = userEntity.TypeId,
                    };
                    //var user = _userService.Authenticate(entity);

                    entity.User.TypeIdentity = _repository1.Find(entity.User.TypeId);
                    return Ok(new { Status = true, Message = "Ok", Entities = entity.User });
                }

                return BadRequest(new { Status = false, Message = "Número de identificación o contraseña invalidas" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Status = false, e.Message });
            }
            

        }



        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public ActionResult<object> Edit(int id,[FromBody] User entity)
        {
            try
            {
                if (id != entity.Id)
                {
                    return BadRequest(new { Status = false, Message = "Id no existe" });
                }

                var user = _repository.Find(id);

                if (user == null)
                {
                    return NotFound(new { Status = false, Message = "Usuario no existe" });
                }


                if (entity.Password != null)
                {
                    user.Password = new PasswordHasher().HashPassword(entity.Password);
                }

                if (!RegexUtilities.IsValidEmail(entity.Email))
                {
                    return BadRequest(new { Status = false, Message = "Correo electronico invalido" });
                }

                user.LastName = entity.LastName;
                user.Name = entity.Name;
                user.TypeId = entity.TypeId;
                user.Email = entity.Email;

                user = _repository.Edit(user);
                if (_unitOfWork.Commit() > 0)
                {
                    return Ok(new { Status = true, Message = "Ok", Entities = user });
                }
                return BadRequest(new { Status = false, Message = "error no se pudo realizar la operación" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Status = false, e.Message });
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public ActionResult<object> Del(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest(new { Status = false, Message = "Id no validad" });
                }


                _repository.Delete(id);
                if (_unitOfWork.Commit() > 0)
                {
                    return Ok(new { Status = true, Message = "Ok" });
                }
                return BadRequest(new { Status = false, Message = "No se eliminar" });
            }
            catch(Exception e)
            {
                return BadRequest(new { Status = false, e.Message });
            }
        }

        [HttpPost("deleteList")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public ActionResult<object> Del(int[] id)
        {
            try
            {
                if (id.Length == 0)
                {
                    return BadRequest(new { Status = false, Message = "Id No validad" });
                }

                id.ToList().ForEach(x =>
                {
                    _repository.Delete(x);
                });

                if (_unitOfWork.Commit() > 0)
                {
                    return Ok(new { Status = true, Message = "Ok" });
                }
                return BadRequest(new { Status = false, Message = "No se pudo completar la operación" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Status = false, e.Message });
            }
        }

    }
}
