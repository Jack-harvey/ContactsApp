//$(function () {
    const usernameSelected = document.getElementById("userLoginSelect");
    $.get("home/UsernameAndTheme", { userId: usernameSelected.value }, function (themePreference) {
        console.log(themePreference);
        if (!themePreference[0].themeSelection == "atomOneDark" || !themePreference[0].themeSelection == "Dracula") {
            return
        };
        if (themePreference[0].themeSelection == "atomOneDark") {
            document.body.classList.toggle('atom-one-dark-theme',);
        };
        document.body.classList.toggle('dracula-theme',);
    });
//});