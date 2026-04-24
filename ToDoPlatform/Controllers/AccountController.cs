[HttpGet]
public IActionResult Register()
{
    return View();
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Register(RegisterVM registerVM)
{
    if (ModelState.IsValid && registerVM.Terms)
    {
        var result = await _userService.Register(registerVM);

        if (result.Count == 0)
            TempData["Success"] = "Conta criada com sucesso! Redirecionando...";
        else
        {
            foreach (var error in result)
                TempData["Failure"] += error + "\n";
        }
    }
    else
        TempData["Failure"] = "Dados inválidos. Verifique os campos preenchidos.";

    return View(registerVM);
}