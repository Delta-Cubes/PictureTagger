﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PictureTagger.Models;
using PictureTagger.Models.ApiModels;
using PictureTagger.Repositories;

namespace PictureTagger.ApiControllers
{
    [Authorize]
    public class TagsController : ApiController
    {
        private IRepository<Tag> dbTagsRepository;

        public TagsController() : this(new DatabaseRepository<Tag>(true))
        {
        }

        public TagsController(IRepository<Tag> dbTagsRepository)
        {
            this.dbTagsRepository = dbTagsRepository;
        }


        // GET: api/Tags
        [AllowAnonymous]
        public IQueryable<TagApi> GetTags()
        {
            return dbTagsRepository.GetAll().Cast<TagApi>();
        }

        // GET: api/Tags/5
        [AllowAnonymous]
        [ResponseType(typeof(TagApi))]
        public IHttpActionResult GetTag(int id)
        {
            Tag tag = dbTagsRepository.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        // PUT: api/Tags/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTag(int id, TagApi tagApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tagApi.TagID)
            {
                return BadRequest();
            }

            try
            {
                dbTagsRepository.Update(tagApi);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tags
        [ResponseType(typeof(Tag))]
        public IHttpActionResult PostTag(TagApi tagApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbTagsRepository.Create(tagApi);

            return CreatedAtRoute("DefaultApi", new { id = tagApi.TagID }, tagApi);
        }

        // DELETE: api/Tags/5
        [ResponseType(typeof(Tag))]
        public IHttpActionResult DeleteTag(int id)
        {
            Tag tag = dbTagsRepository.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            dbTagsRepository.Delete(tag);

            return Ok(tag);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbTagsRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TagExists(int id)
        {
            return dbTagsRepository.GetAll().Count(e => e.TagID == id) > 0;
        }
    }
}