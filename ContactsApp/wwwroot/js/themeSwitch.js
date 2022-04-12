const themeSwitch = document.getElementById('theme-toggle');
themeSwitch.addEventListener('click', () => {

    if (document.body.classList.contains('dracula-theme')) {
        $.post("home/SaveNewThemePreference", { userId: 2, themePreference: "atom-one-dark-theme" });
        document.body.classList.remove('dracula-theme',);
        document.body.classList.add('atom-one-dark-theme',);

    }
    else
    {
    $.post("home/SaveNewThemePreference", { userId: 2, themePreference: "dracula-theme" });
        document.body.classList.remove('atom-one-dark-theme',);
        document.body.classList.add('dracula-theme',);


    }
});