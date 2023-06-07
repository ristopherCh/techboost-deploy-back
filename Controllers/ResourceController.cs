using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TechBoost.Models;
using TechBoost.Repositories;

namespace TechBoost.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ResourceController : ControllerBase
	{
		private readonly IResourceRepository _resourceRepository;
		public ResourceController(IResourceRepository resourceRepository)
		{
			_resourceRepository = resourceRepository;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_resourceRepository.GetAll());
		}

		[HttpGet("details/{id}")]
		public IActionResult GetResourceById(int id)
		{
			return Ok(_resourceRepository.GetResourceById(id));
		}

		[HttpGet("mediaType/{mediaType}")]
		public IActionResult GetResourceByMediaType(string mediaType)
		{
			return Ok(_resourceRepository.GetResourcesByMediaType(mediaType));
		}

		[HttpGet("subject/{subject}")]
		public IActionResult GetResourcesBySubject(string subject)
		{
			return Ok(_resourceRepository.GetResourcesBySubject(subject));
		}

		[HttpGet("creator/{creator}")]
		public IActionResult GetResourcesByCreator(string creator)
		{
			return Ok(_resourceRepository.GetResourcesByCreator(creator));
		}

		[HttpGet("user/{userId}")]
		public IActionResult GetResourcesByUserId(int userId)
		{
			return Ok(_resourceRepository.GetResourcesByUserId(userId));
		}

		[HttpGet("search/{searchTerm}")]
		public IActionResult SearchResources(string searchTerm)
		{
			return Ok(_resourceRepository.SearchResources(searchTerm));
		}

		[HttpPost]
		public IActionResult Post(Resource resource)
		{
			_resourceRepository.Add(resource);
			return CreatedAtAction("Get", new { id = resource.Id }, resource);
		}

		//[HttpPut("{id}")]
		[HttpPut]
		public IActionResult Put(Resource resource)
		{
			_resourceRepository.EditResource(resource);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			_resourceRepository.DeleteResource(id);
			return NoContent(); 
		}
	}
}
