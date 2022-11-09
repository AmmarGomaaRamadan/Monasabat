﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Monasapat.Models;

namespace ITI.Monasabat.Control.Controllers
{
    public class AdminPlaceOwnersController : Controller
    {
        private readonly MonasabatContext _context;

        public AdminPlaceOwnersController(MonasabatContext context)
        {
            _context = context;
        }

        // GET: AdminPlaceOwners
        public async Task<IActionResult> Index()
        {
              return View(await _context.PlaceOwners.ToListAsync());
        }

        // GET: AdminPlaceOwners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PlaceOwners == null)
            {
                return NotFound();
            }

            var placeOwner = await _context.PlaceOwners
                .FirstOrDefaultAsync(m => m.ID == id);
            if (placeOwner == null)
            {
                return NotFound();
            }

            return View(placeOwner);
        }

        // GET: AdminPlaceOwners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminPlaceOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] PlaceOwner placeOwner)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(placeOwner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(placeOwner);
        }

        // GET: AdminPlaceOwners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PlaceOwners == null)
            {
                return NotFound();
            }

            var placeOwner = await _context.PlaceOwners.FindAsync(id);
            if (placeOwner == null)
            {
                return NotFound();
            }
            return View(placeOwner);
        }

        // POST: AdminPlaceOwners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] PlaceOwner placeOwner)
        {
            if (id != placeOwner.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(placeOwner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceOwnerExists(placeOwner.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(placeOwner);
        }

        // GET: AdminPlaceOwners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PlaceOwners == null)
            {
                return NotFound();
            }

            var placeOwner = await _context.PlaceOwners
                .FirstOrDefaultAsync(m => m.ID == id);
            if (placeOwner == null)
            {
                return NotFound();
            }

            return View(placeOwner);
        }

        // POST: AdminPlaceOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PlaceOwners == null)
            {
                return Problem("Entity set 'MonasabatContext.PlaceOwners'  is null.");
            }
            var placeOwner = await _context.PlaceOwners.FindAsync(id);
            if (placeOwner != null)
            {
                _context.PlaceOwners.Remove(placeOwner);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaceOwnerExists(int id)
        {
          return _context.PlaceOwners.Any(e => e.ID == id);
        }
    }
}