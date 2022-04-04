document.getElementById("userLoginSelect").addEventListener('change', function () {
        $.get("home/UsernameAndTheme", { userId: this.value }, function (themePreference) {
            console.log(themePreference);
            if (!themePreference[0].themeSelection == "atomOneDark" || !themePreference[0].themeSelection == "Dracula") {
                return
            };
            if (themePreference[0].themeSelection == "atomOneDark") {
                document.body.classList.toggle('atom-one-dark-theme',);
            };
            document.body.classList.toggle('dracula-theme',);
        });
    });