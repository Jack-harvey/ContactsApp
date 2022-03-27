document.getElementById("userLoginSelect").addEventListener('change', function () {
    const userIdSelectField = document.getElementById("userLoginSelect");
    let userIdFromSelect = Number(userIdSelectField.value);
    console.log(userIdFromSelect);
    $(document).ready(function () {
        $("#userLoginSelect").on('change', function () {
            $.get("home/UsernameAndTheme", { userId: userIdFromSelect }, function (themePreference) {
                console.log(themePreference);
            });
        });
    });
});