document.getElementById("userLoginSelect").addEventListener('change', function () {
        $.get("home/UsernameAndTheme", { userId: this.value }, function (themePreference) {
            console.log(themePreference);
        });
    });