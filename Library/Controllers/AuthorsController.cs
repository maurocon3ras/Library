﻿using AutoMapper;
using Library.API.Entities;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private ILibraryRepository _libraryRepository;

        public AuthorsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var authorsFromRepo = _libraryRepository.GetAuthors();

            var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            return Ok(authors);
        }

        [HttpGet("{id}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid id)
        {
            var authorsFromRepo = _libraryRepository.GetAuthor(id);

            if (authorsFromRepo == null)
                return NotFound();

            var author = Mapper.Map<AuthorDto>(authorsFromRepo);
            return Ok(author);
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorForCreationDto author)
        {
            if (author == null)
                return BadRequest();

            var authorEntity = Mapper.Map<Author>(author);

            _libraryRepository.AddAuthor(authorEntity);

            if (!_libraryRepository.Save())
                throw new Exception("Creating an author failed to save.");

            var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);

            return CreatedAtRoute("GetAuthor", new { id = authorToReturn.Id }, authorToReturn);
        }
    }
}
