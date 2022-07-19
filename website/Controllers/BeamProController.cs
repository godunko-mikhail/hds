﻿using Microsoft.AspNetCore.Mvc;
using website.Models.Beam.View;

namespace website.Controllers
{
    [Route("/{controller}/pro/{action=Index}")]
    public class BeamController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // try to load 
            return View();
        }

        [HttpPost]
        public IActionResult Index(BeamInputStringModel input)
        {
            BeamInputModel beamModel;
            try
            {
                beamModel = input.Parse();
            }
            catch 
            {
                return Redirect("/Beam/pro/Index"); //TODO: добавить параметр строки ?alert=message и скрипт на js который при загрузке его обработает
            }

            var output = new BeamOutputModel(beamModel);
            Console.WriteLine(input.ToString());
            Console.WriteLine(beamModel.ToString());

            return View("Calculate", output);
        }
    }
}