﻿using API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity,Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<Entity> Get()
        {
            //if (repository.Get().ToList().Count == 0)
            //{
            //    return StatusCode(404, new { status = HttpStatusCode.NotFound, result = repository.Get(), message = "Data Not Found." });
            //}
            //else
            //{
            //    return StatusCode(200, new { status = HttpStatusCode.OK, result = repository.Get(), message = "Data Found." });
            //}

            return Ok(repository.Get());
        }

        [HttpGet("{key}")]
        public ActionResult<Entity> GetByID(Key key)
        {
            //try
            //{
            //    var master = repository.Get(key);
            //    return StatusCode(200, new { status = HttpStatusCode.OK, result = master, message = $"Get Master Data {key} Successfully!" });
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
            //}
            return Ok(repository.Get(key));
        }

        [HttpPost]
        public ActionResult<Entity> Post(Entity entity)
        {
            try
            {
                var insert = repository.Insert(entity);
                return Ok(new { status = HttpStatusCode.OK, result = entity, message = "Insert Data Successfull" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, result = entity, message = ex.Message });
            }
        }

        [HttpPut]
        public ActionResult<Entity> Put(Entity entity, Key key)
        {
            //try
            //{
            //    repository.Update(entity, key);
            //}
            //catch (Exception ex)
            //{
            //    if (ex is DbUpdateConcurrencyException || ex is DbUpdateException)
            //    {
            //        return StatusCode(400, new { status = HttpStatusCode.BadRequest, result = entity, message = "Data Edit Failed. DbUpdateConcurrencyException or DbUpdateException" });
            //    }
            //}
            //return StatusCode(200, new { status = HttpStatusCode.OK, result = entity, message = "Data Successfully Edited." });
            return Ok(repository.Update(entity, key));
        }

        [HttpDelete("{key}")]
        public ActionResult<Entity> Delete(Key key)
        {
            try
            {
                var erased = repository.Get(key);
                repository.Delete(key);
                return StatusCode(200, new { status = HttpStatusCode.OK, result = erased, message = "Data Successfully Deleted." });
            }
            catch
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, result = repository.Delete(key), message = "Data Delete Failed. ArgumentNullException" });
            }
        }
    }
}
