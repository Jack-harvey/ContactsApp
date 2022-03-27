const themeSwitch = document.getElementById('theme-toggle');
themeSwitch.addEventListener('click', () => {
    //document.body.classList.toggle('dumbest-theme');
    //const theBody = document.body;
    //if (theBody.className == "dumbest-theme")
    //    theBody.className = "dracula-theme";
    //else
    //    theBody.className = "dumbest-theme";
    document.body.classList.toggle('dracula-theme',);
    document.body.classList.toggle('atom-one-dark-theme',);
});