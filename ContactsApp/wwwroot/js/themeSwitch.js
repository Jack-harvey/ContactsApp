//const bootstrap = require("../lib/bootstrap/dist/js/bootstrap");

const themeSwitch = document.getElementById('theme-toggle');
themeSwitch.addEventListener('click', () => {
    const newTheme = document.body.classList.contains('dracula-theme') ? 'atom-one-dark-theme' : 'dracula-theme';

    const toastclasses = ['bg-primary', 'bg-success', 'bg-danger'];
    var toastyWarm = document.getElementById('theme-toast');

    toastclasses.forEach((c) => toastyWarm.classList.remove(c));

    $.post("home/SaveNewThemePreference", { userId: 2, themePreference: newTheme },
        function (data, status, jqxhr) {
            if (data.result == 1) {
                removeBodyThemes();
                document.body.classList.add(data.theme);
                toastyWarm.classList.add('bg-success');

            } else {
                toastyWarm.classList.add('bg-danger');
            }
            toastyWarm.querySelector('.toast-body').innerHTML = data.message;
            var toastPopsUp = new bootstrap.Toast(toastyWarm);
            toastPopsUp.show();
        }
    );
    //if (document.body.classList.contains('dracula-theme')) {
    //    $.post("home/SaveNewThemePreference", { userId: 2, themePreference: "atom-one-dark-theme" },
    //        function (data, status, jqxhr) {
    //            removeBodyThemes();
    //            if (data.result == 1) {
    //                //document.body.classList.remove('atom-one-dark-theme');
    //                //document.body.classList.add('dracula-theme');
    //                document.body.classList.add(data.theme);
    //                toastyWarm.classList.add('bg-success');
    //                toastyWarm.querySelector('.toast-body').innerHTML = data.message;
    //            } else {
    //                toastyWarm.classList.add('bg-danger');
    //                toastyWarm.querySelector('.toast-body').innerHTML = data.message;
    //            }
    //            var toastPopsUp = new bootstrap.Toast(toastyWarm);
    //            toastPopsUp.show();
    //        }
    //    );
    //    //document.body.classList.remove('dracula-theme',);
    //    //document.body.classList.add('atom-one-dark-theme');
    //    ////var toastyWarm = document.getElementById('theme-toast');
    //    //var toastPopsUp = new bootstrap.Toast(toastyWarm);
    //    //toastPopsUp.show();

    //}
    //else
    //{
    //    $.post("home/SaveNewThemePreference", { userId: 2, themePreference: "dracula-theme" },
    //        function (data, status, jqxhr) {
    //            removeBodyThemes();

    //            if (data.result == 1) {
    //                //document.body.classList.remove('atom-one-dark-theme');
    //                //document.body.classList.add('dracula-theme');
    //                document.body.classList.add('dracula-theme');
    //                toastyWarm.classList.add('bg-success');
    //                toastyWarm.querySelector('.toast-body').innerHTML = data.message;
    //            } else {
    //                toastyWarm.classList.add('bg-danger');
    //                toastyWarm.querySelector('.toast-body').innerHTML = data.message;
    //            }
    //            var toastPopsUp = new bootstrap.Toast(toastyWarm);
    //            toastPopsUp.show();
    //        }
    //    );




    //}
});

function removeBodyThemes() {
    document.body.classList.forEach((c) => {
        if (c.endsWith('theme'))
            document.body.classList.remove(c);
    });
}