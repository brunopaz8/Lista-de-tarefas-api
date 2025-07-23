using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTarefa.Context;
using ApiTarefa.Moldels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiTarefa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("FindById")]
        public IActionResult FindById(int id)
        {
            var tarefa = _context.Tarefas.Find(id);
            if (tarefa == null)
            {
                return NotFound($"Nenhuma tarefa com o id {id} foi encontrada!");
            }

            return Ok(tarefa);
        }

        [HttpGet("FindAll")]
        public IActionResult FindAll()
        {
            var ListaTarefas = _context.Tarefas;

            if (ListaTarefas == null)
            {
                return NotFound("Nenhuma tarefa foi encontrada!");
            }

            return Ok(ListaTarefas);
        }

        [HttpGet("FindByTitle")]
        public IActionResult FindByTitle(string titulo)
        {
            var tarefa = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));

            if (tarefa == null)
            {
                return NotFound($"Nenhuma tarefa com o titulo {titulo} foi encontrada!");
            }

            return Ok(tarefa);
        }

        [HttpGet("FindByDate")]
        public IActionResult FindByDate(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("FindByStatus")]
        public IActionResult FindByStatus(EnumStatusTarefa status)
        {

            var tarefa = _context.Tarefas.Where(x => x.Status == status);

            if (tarefa == null)
            {
                return NotFound($"Nenhuma tarefa com o status {status} foi encontrada!");
            }

            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Create(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Add(tarefa);
            _context.SaveChanges();

            return CreatedAtAction(nameof(FindById), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Status = tarefa.Status;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Descricao = tarefa.Descricao;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();
            return Ok();
        }
        
         [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound($"Nenhuma tarefa com o id {id} foi encontrada!");

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return Ok($"A tarefa de id {id} foi deletada!");
        }
    }
}