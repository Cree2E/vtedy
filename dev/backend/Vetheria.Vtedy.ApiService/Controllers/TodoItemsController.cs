﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vetheria.Vtedy.ApiService.Dto;
using Vetheria.Vtedy.Application.Core;
using Vetheria.Vtedy.DataModel.Model;

namespace Vetheria.Vtedy.ApiService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TodoItemsController : Controller
    {
        private readonly IQueryHandler<Task<IEnumerable<TodoItem>>> _getTodoItemsQueryHandler;
        private readonly IQueryHandler<int, Task<TodoItem>> _getTodoItemByIdQueryHandler;
        private readonly ICommandHandler<int, Task<Result<long>>> _deleteTodoItemCommandHandler;
        private readonly ICommandHandler<TodoItem, Task<Result<long>>> _addTodoItemCommandHandler;
        private readonly IMapper _mapper;

        public TodoItemsController(
            IQueryHandler<Task<IEnumerable<TodoItem>>> getTodoItemsQueryHandler,
            IQueryHandler<int, Task<TodoItem>> getTodoItemByIdQueryHandler,
            ICommandHandler<int, Task<Result<long>>> deleteTodoItemCommandHandler,
            ICommandHandler<TodoItem, Task<Result<long>>> addTodoItemCommandHandler,
            IMapper mapper
            )
        {
            _getTodoItemsQueryHandler = getTodoItemsQueryHandler;
            _getTodoItemByIdQueryHandler = getTodoItemByIdQueryHandler;
            _deleteTodoItemCommandHandler = deleteTodoItemCommandHandler;
            _addTodoItemCommandHandler = addTodoItemCommandHandler;
            _mapper = mapper;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<IEnumerable<TodoItemDto>> Get()
        {
            var res = await _getTodoItemsQueryHandler.Execute();

            var dto = _mapper.Map<IEnumerable<TodoItemDto>>(res);

            return dto;
        }

        // GET: api/Todo/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var res = await _getTodoItemByIdQueryHandler.ExecuteAsync(id);

            if (res == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<TodoItemDto>(res);


            var resObj = new ObjectResult(dto);
            return resObj;
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var res = await _addTodoItemCommandHandler.ExecuteAsync(item);


            return CreatedAtRoute("Get", new { id = item.Id }, item);
        }

        // PUT: api/Todo/5
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody]TodoItem item)
        //{
        //    if (item == null || item.Id != id)
        //    {
        //        return BadRequest();
        //    }

        //    var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
        //    if (todo == null)
        //    {
        //        return NotFound();
        //    }

        //    todo.IsComplete = item.IsComplete;
        //    todo.Name = item.Name;

        //    _context.TodoItems.Update(todo);
        //    _context.SaveChanges();
        //    return new NoContentResult();
        //}

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var res = await _deleteTodoItemCommandHandler.ExecuteAsync(id);
            if (!res.IsSuccess)
            {
                return NotFound();
            }

            return new NoContentResult();
        }
    }
}
