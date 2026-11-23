// GolBet.Web/Controllers/MatchesController.cs 

// (agregar; requiere inyectar ITeamService en el constructor) 

using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;   // SelectList 
using Microsoft.AspNetCore.Authorization;

using GolBet.Repositories.Data;


private readonly ITeamService _teamService;



// GET /Matches/Create 

[Authorize(Roles = DbSeeder.AdminRole)]
public async Task<IActionResult> Create()

{

    await LoadTeamsAsync();

    return View(new MatchFormDto());

}



[HttpPost, ValidateAntiForgeryToken]

public async Task<IActionResult> Create(MatchFormDto dto)

{

    if (!ModelState.IsValid)

    {

        await LoadTeamsAsync();

        return View(dto);

    }



    try

    {

        await _matchService.CreateAsync(dto);

        TempData["Success"] = "Partido creado correctamente.";

        return RedirectToAction(nameof(Index));

    }

    catch (InvalidOperationException ex)      // business rule violated 

    {

        ModelState.AddModelError(string.Empty, ex.Message);

        await LoadTeamsAsync();

        return View(dto);

    }

}



// GET /Matches/Edit/5 

public async Task<IActionResult> Edit(int id)

{

    var dto = await _matchService.GetForEditAsync(id);

    if (dto is null) return NotFound();



    await LoadTeamsAsync(); // the form needs the dropdowns too 

    return View(dto);

}



// POST /Matches/Edit 

[HttpPost, ValidateAntiForgeryToken]

public async Task<IActionResult> Edit(MatchFormDto dto)

{

    if (!ModelState.IsValid)

    {

        await LoadTeamsAsync();

        return View(dto);

    }



    try

    {

        await _matchService.UpdateAsync(dto);

        TempData["Success"] = "Partido actualizado correctamente.";

        return RedirectToAction(nameof(Index));

    }

    catch (InvalidOperationException ex)      // business rule violated 

    {

        ModelState.AddModelError(string.Empty, ex.Message);

        await LoadTeamsAsync();

        return View(dto);

    }

}



// POST /Matches/Deactivate/5 

[HttpPost, ValidateAntiForgeryToken]

public async Task<IActionResult> Deactivate(int id)

{

    await _matchService.DeactivateAsync(id);

    TempData["Success"] = "Partido desactivado.";

    return RedirectToAction(nameof(Index));

}



private async Task LoadTeamsAsync()

{

    var teams = await _teamService.GetAllAsync();

    ViewBag.Teams = new SelectList(teams, "Id", "Name");

}